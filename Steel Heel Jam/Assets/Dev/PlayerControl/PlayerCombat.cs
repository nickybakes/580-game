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

    public float attackCooldown;
    public const float attackCooldownMax = .35f;

    private float recentAttackCooldown;
    private const float recentAttackCooldownMax = 0.5f;

    public bool hasItemEquiped = false;

    /// <summary>
    /// A boolean that represents if the player has attacked recently.
    /// </summary>
    public bool AttackedRecently {
        get
        {
            if (recentAttackCooldown > 0) return true;
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
    public void UpdateManual(bool canAttack, bool canDodgeRoll, bool canBlock, bool canPickup)
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

        if (recentAttackCooldown > 0)
        {
            recentAttackCooldown -= Time.deltaTime;
        }

        if (canAttack && attackCooldown > attackCooldownMax && _input.Attack)
        {
            if (_status.movement.grounded)
            {
                AttackGround();
            }
            else
            {
                //AttackAir();
            }

            recentAttackCooldown = recentAttackCooldownMax;
        }

        if (canDodgeRoll && dodgeRollCoolDown > dodgeRollCoolDownMax && _input.dodgeRoll && !_input.wasDodgeRolling)
        {
            DodgeRoll();
        }

        if (canBlock && blockCoolDown > blockCoolDownMax && _input.block && !_input.wasBlocking)
        {
            Block();
        }

        if (canPickup && _input.pickUpPressed && !_input.wasPickUpPressed)
        {
            TryPickup();
        }
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
        _status.movement.SetTheSetForwardDirection();
    }

    private void DodgeRoll()
    {
        _input.dodgeRoll = false;
        dodgeRollCoolDown = 0;
        _status.SetPlayerStateImmediately(new DodgeRoll());
        _status.movement.SetTheSetForwardDirection();
        _status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    private void Block()
    {
        _input.block = false;
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

    }
}
