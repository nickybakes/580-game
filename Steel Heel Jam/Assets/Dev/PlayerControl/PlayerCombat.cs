using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

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

    public GameObject equippedItem;

    private GameManager gameManager;

    // -1 = 360 deg arc, 0 = 180 degree arc, 0.5 = 90 deg...
    [SerializeField]
    [Range(-1, 1)]
    private float targetAngle = 0.5f;

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
        gameManager = FindObjectOfType<GameManager>();

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

        if (_status.stamina > 0 || _status.isHeel)
        {
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
        }

        if (canPickup && _input.pickUpPressed && !_input.wasPickUpPressed)
        {
            TryPickup();
        }

        if (_status.CurrentPlayerState is ItemThrowing && equippedItem != null && !_input.throwIsHeld && _input.throwWasHeld)
        {
            Throw();
        }

        if (_status.CurrentPlayerState is Rest && _input.throwWasHeld)
        {
            _status.SetPlayerStateImmediately(new Idle());
        }

        if (_input.throwIsHeld && canThrow)
        {
            if (equippedItem != null)
            {

                if (_status.CurrentPlayerState != new ItemThrowing())
                {
                    _status.SetPlayerStateImmediately(new ItemThrowing());
                }

                timeHeld += Time.deltaTime;

                if (timeHeld > 3)
                    timeHeld = 3;
            } else
            {
                if (_status.CurrentPlayerState is not Rest) _status.SetPlayerStateImmediately(new Rest());
            }
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
        List<PlayerStatus> potentialTargets = new List<PlayerStatus>();
        equippedItem.transform.parent = null;
        equippedItem.SetActive(true);

        // Look for target, if there is one, use targeted throw, else use straight throw.
        ItemTrajectory itemTrajectory = equippedItem.GetComponent<ItemTrajectory>();

        // Grab all players. (gameManager does this)
        foreach (PlayerStatus s in gameManager.alivePlayerStatuses)
        {
            Vector3 vectorToCollider = (s.transform.position - _status.transform.position).normalized;
            // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
            if (Vector3.Dot(vectorToCollider, _status.movement.ActualFowardDirection) > targetAngle)
            {
                // If in the arc, add to potential target list.
                potentialTargets.Add(s);
            }
        }

        // Now sort the list by distance, shortest to furthest.
        if (potentialTargets.Count != 0)
        {
            potentialTargets = potentialTargets.OrderBy(x => Vector3.Distance(_status.transform.position, x.transform.position)).ToList();

            // Now check if there are any obstacles in the way of the first player in the list, if not, pass to itemTrajectory, else cont. list, if none, pass null.
            // Raycasting magic.

            // For now, adds the first in potentialTargets.
            itemTrajectory.target = potentialTargets[0];
        }
        else
        {
            itemTrajectory.target = null;
        }

        itemTrajectory.isThrown = true;
        itemTrajectory.chargeAmount = timeHeld;
        itemTrajectory.thrower = _status;
        itemTrajectory.throwerObject = _status.gameObject;

        equippedItem = null;
        weaponState = new Unarmed(_status.playerNumber, _hitbox);

        _status.SetPlayerStateImmediately(new ThrowRecovery());

        timeHeld = 0;

    }

}
