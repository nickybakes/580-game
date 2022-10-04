using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerCombat : MonoBehaviour
{
    private StarterAssetsInputs _input;
    public GameObject _hitbox;
    private Hitbox _hitboxScript;
    private PlayerStatus _status;

    private GameObject _pickUpSphere;

    private PickUpSphere _pickUpSphereScript;

    public DefaultState weaponState;
    private float timeHeld;

    public float attackCooldown;
    public const float attackCooldownMax = .35f;

    private float recentActionCooldown;
    private const float recentActionCooldownMax = 0.5f;

    public bool hasItemEquiped = false;

    /// <summary>
    /// A boolean that represents if the player has attacked recently.
    /// </summary>
    public bool ActedRecently {
        get
        {
            if (recentActionCooldown > 0) return true;
            return false;
        }
    }

    private float dodgeRollCoolDown;
    private const float dodgeRollCoolDownMax = .2f;

    private float blockCoolDown;
    private const float blockCoolDownMax = 1f;


    // Start is called before the first frame update
    public void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _hitbox = transform.GetChild((int)PlayerChild.Hitbox).gameObject;

        _pickUpSphere = transform.GetChild((int)PlayerChild.PickUpSphere).gameObject;

        _pickUpSphereScript = transform.GetChild((int)PlayerChild.PickUpSphere).gameObject.GetComponent<PickUpSphere>();

        _status = GetComponent<PlayerStatus>();

        weaponState = new Unarmed(_status.playerNumber, _hitbox);
    }

    /// <summary>
    /// This is to be updated via the PlayerStatus script's Update method. This will check for combat based inputs such as Attacking, Dodge Rolling, and Blocking.
    /// </summary>
    /// <param name="canAttack">True if the player can interrupt their current state and begin an Attack. Base off the player's current PlayerState</param>
    /// <param name="canDodgeRoll">True if the player can interrupt their current state and go into a dodge roll. Base off the player's current PlayerState</param>
    /// <param name="canBlock">True if the player can interrupt their current state and activate a block. Base off the player's current PlayerState</param>
    /// 
    public void UpdateManual(bool canAttack, bool canDodgeRoll, bool canBlock, bool canPickup, bool canThrow)
    {
        //fixes the null ref exception when recompiling in the Editor
#if UNITY_EDITOR
        if (weaponState == null)
        {
            weaponState = new DefaultState(_status.playerNumber, _hitbox);
        }
#endif

        if (_status.CurrentPlayerState.countAttackCooldown && attackCooldown < attackCooldownMax)
            attackCooldown += Time.deltaTime;

        if (_status.CurrentPlayerState.countDodgeRollCooldown && dodgeRollCoolDown < dodgeRollCoolDownMax)
            dodgeRollCoolDown += Time.deltaTime;

        if (_status.CurrentPlayerState.countBlockCooldown && blockCoolDown < blockCoolDownMax)
            blockCoolDown += Time.deltaTime;

        if (recentActionCooldown > 0)
        {
            recentActionCooldown -= Time.deltaTime;
        }

        if (canAttack && attackCooldown > attackCooldownMax && _input.Attack && _status.stamina > 0)
        {
            if (_status.movement.grounded)
            {
                AttackGround();
            }
            else
            {
                //AttackAir();
            }

            //_status.ReduceStamina(weaponState.staminaCost);
            recentActionCooldown = recentActionCooldownMax;
        }

        if (canDodgeRoll && dodgeRollCoolDown > dodgeRollCoolDownMax && _input.dodgeRoll && !_input.wasDodgeRolling)
        {
            DodgeRoll();
            recentActionCooldown = recentActionCooldownMax;
        }

        if (canBlock && blockCoolDown > blockCoolDownMax && _input.block && !_input.wasBlocking)
        {
            Block();
            recentActionCooldown = recentActionCooldownMax;
        }

        if (canPickup && _input.pickUpPressed && !_input.wasPickUpPressed)
        {
            TryPickup();
        }

        if (canThrow && hasItemEquiped && !_input.throwIsHeld && _input.throwWasHeld)
        {
            Throw();
            timeHeld = 0;
        }

        if (_input.throwIsHeld && hasItemEquiped)
        {
            if (_status.CurrentPlayerState != new ItemThrowing())
            {
                _status.SetPlayerStateImmediately(new ItemThrowing());
            }

            timeHeld += Time.deltaTime;

            if (timeHeld > 3)
                timeHeld = 3;
        }

        Debug.Log(_status.CurrentPlayerState);

    }

    private void AttackGround()
    {
        _input.Attack = false;
        _status.movement.velocity = Vector3.zero;
        _status.SetPlayerStateImmediately(new AttackGroundStartup());

        weaponState.UpdateValues();

        _status.CurrentPlayerState.timeToChangeState = weaponState.Startup;
        _status.CurrentPlayerState.stateToChangeTo.timeToChangeState = weaponState.Duration;
        _status.CurrentPlayerState.stateToChangeTo.moveSpeedMultiplier = weaponState.ForwardSpeedModifier;
        _status.CurrentPlayerState.stateToChangeTo.stateToChangeTo.timeToChangeState = weaponState.Recovery;

        if(weaponState.currentComboCount == 0)
            _status.movement.SetTheSetForwardDirection();
    }

    private void DodgeRoll()
    {
        _input.dodgeRoll = false;
        dodgeRollCoolDown = 0;
        _status.SetPlayerStateImmediately(new DodgeRoll());
        _status.movement.SetTheSetForwardDirection();
        _status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
        _status.ReduceStamina(_status.dodgeRollStaminaDamage);
    }

    private void Block()
    {
        _input.block = false;
        _status.attackBlocked = false;
        blockCoolDown = 0;
        _status.movement.velocity = Vector3.zero;
        _status.SetPlayerStateImmediately(new Block());
    }

    private void TryPickup()
    {
        _input.pickUpPressed = false;
        _pickUpSphere.SetActive(true);
    }

    private void Throw()
    {
        

        // If the player is not in unarmed equip state, the item they are currently holding need to drop before new item is picked up.
        for (int i = 0; i < _status.transform.childCount; i++)
        {
            if(_status.transform.GetChild(i).gameObject.tag == "PickUp")
            {
                GameObject itemToDrop = _status.transform.GetChild(i).gameObject;
                itemToDrop.transform.parent = null;
                itemToDrop.SetActive(true);

                // Look for target, if there is one, use targeted throw, else use straight throw.
                ItemTrajectory itemTrajectory = itemToDrop.GetComponent<ItemTrajectory>();

                // For now, always uses straight throw.
                itemTrajectory.isTargetedThrow = false;
                itemTrajectory.isThrown = true;
                itemTrajectory.chargeAmount = timeHeld;
                itemTrajectory.thrower = _status;

                //_status.SetPlayerStateImmediately(new ThrowRecovery());
                _status.SetPlayerStateImmediately(new Idle());
            }
        }
    }

    // Math to find player to hit
    // private bool WillHitPerson()
    // {

    // }
}
