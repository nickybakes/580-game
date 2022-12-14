using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerCombat : MonoBehaviour
{
    private StarterAssetsInputs _input;
    public GameObject _hitbox;
    private Hitbox _hitboxScript;
    private PlayerStatus _status;

    private PickUpSphere _pickUpSphere;

    [HideInInspector]
    public GrabHitbox grabHitbox;


    public DefaultState weaponState;
    private float timeHeld;

    public float attackCooldown;
    public const float attackCooldownMax = .35f;

    private float recentActionCooldown;
    private const float recentActionCooldownMax = 1.0f;

    private float pickupHeldLength = 0;

    public GameObject equippedItem;


    // -1 = 360 deg arc, 0 = 180 degree arc, 0.5 = 90 deg...
    [SerializeField]
    [Range(-1, 1)]
    private float targetAngle = 0.5f;

    public int NumberOfItemsWithinRange
    {
        get
        {
            return _pickUpSphere.ItemCount;
        }
    }

    /// <summary>
    /// A boolean that represents if the player has attacked recently.
    /// </summary>
    public bool ActedRecently
    {
        get
        {
            if (recentActionCooldown > 0)
                return true;
            return false;
        }
        set
        {
            if (value)
            {
                recentActionCooldown = recentActionCooldownMax;
            }
            else
            {
                recentActionCooldown = 0;
            }
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

        _pickUpSphere = GetComponentInChildren<PickUpSphere>();
        grabHitbox = GetComponentInChildren<GrabHitbox>(true); // Set true for inactive objects.

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
    public void UpdateManual(
        bool canAttack,
        bool canDodgeRoll,
        bool canBlock,
        bool canPickup,
        bool canThrow
    )
    {

        if (_status.CurrentPlayerState.countAttackCooldown && attackCooldown < attackCooldownMax)
            attackCooldown += Time.deltaTime;

        if (
            _status.CurrentPlayerState.countDodgeRollCooldown
            && dodgeRollCoolDown < dodgeRollCoolDownMax
        )
            dodgeRollCoolDown += Time.deltaTime;

        if (_status.CurrentPlayerState.countBlockCooldown && blockCoolDown < blockCoolDownMax)
            blockCoolDown += Time.deltaTime;

        if (recentActionCooldown > 0)
        {
            recentActionCooldown -= Time.deltaTime;
        }

        if (_status.stamina > 0)
        {
            if (canAttack && attackCooldown > attackCooldownMax && _input.Attack)
            {
                if (_status.movement.grounded)
                {
                    AttackGround();
                }
                else
                {
                    AttackAir();
                }

                recentActionCooldown = recentActionCooldownMax;
            }

            if (
                canDodgeRoll
                && dodgeRollCoolDown > dodgeRollCoolDownMax
                && _input.dodgeRoll
                && !_input.wasDodgeRolling
            )
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

        // If you hold the pickup button for 0.3 seconds, sends to SuplexStartup.
        if (canPickup && _input.pickUpPressed)
        {
            pickupHeldLength += Time.deltaTime;

            if (pickupHeldLength > 0.3f)
            {
                _status.SetPlayerStateImmediately(new GrabStartup());
                pickupHeldLength = 0;
                _input.pickUpPressed = false;
            }
        }

        // TryPickup should be called if the pickup button is released and it's not a suplex...
        if (canPickup && !_input.pickUpPressed && _input.wasPickUpPressed)
        {
            TryPickup();
            pickupHeldLength = 0;
        }

        if (
            _status.CurrentPlayerState is ItemThrowing
            && equippedItem != null
            && !_input.throwIsHeld
            && _input.throwWasHeld
        )
        {
            Throw();
        }

        if (_status.CurrentPlayerState is Flexing && _input.throwWasHeld)
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

                if (timeHeld > 2)
                    timeHeld = 2;
            }
            else
            {
                if (_status.CurrentPlayerState is not Flexing)
                {
                    _status.SetPlayerStateImmediately(new Flexing());
                }
            }
        }
    }

    private void AttackGround()
    {
        _input.Attack = false;
        _status.movement.velocity = Vector3.zero;
        AttackGroundStartup attackState = new AttackGroundStartup();

        weaponState.currentAttack = weaponState.combo[weaponState.currentComboCount];

        attackState.animationState = DefaultState.GetAttackAnimation(
            weaponState.currentAttack.animation,
            0
        );
        _status.SetPlayerStateImmediately(attackState);

        _status.CurrentPlayerState.stateToChangeTo.animationState = DefaultState.GetAttackAnimation(
            weaponState.currentAttack.animation,
            1
        );
        _status.CurrentPlayerState.stateToChangeTo.stateToChangeTo.animationState =
            DefaultState.GetAttackAnimation(weaponState.currentAttack.animation, 2);

        _status.CurrentPlayerState.timeToChangeState = weaponState.Startup;
        _status.CurrentPlayerState.stateToChangeTo.timeToChangeState = weaponState.Duration;
        _status.CurrentPlayerState.stateToChangeTo.moveSpeedMultiplier =
            weaponState.ForwardSpeedModifier;
        _status.CurrentPlayerState.stateToChangeTo.stateToChangeTo.timeToChangeState =
            weaponState.Recovery;

        if (weaponState.currentAttack.attackParticle == AttackParticle.AnimationDefault)
        {
            _status.visuals.SetAttackParticle(DefaultState.defaultAttackParticles[(int)weaponState.currentAttack.animation]);
        }
        else
        {
            _status.visuals.SetAttackParticle(weaponState.currentAttack.attackParticle);
        }

        if (weaponState.currentComboCount == 0)
            _status.movement.SetTheSetForwardDirection();
    }

    private void AttackAir()
    {
        _input.Attack = false;

        weaponState.currentAttack = weaponState.airAttack;

        //_status.movement.velocity = Vector3.zero;
        _status.SetPlayerStateImmediately(new AttackAirStartup());

        _status.visuals.SetAttackParticle(AttackParticle.AirAttack);

        if (weaponState.currentComboCount == 0)
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
        _input.wasPickUpPressed = false;
        _pickUpSphere.TryPickup();
    }

    private void Throw()
    {
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.transform.parent = null;
        equippedItem.SetActive(true);

        // Look for target, if there is one, use targeted throw, else use straight throw.
        ItemTrajectory itemTrajectory = equippedItem.GetComponent<ItemTrajectory>();

        List<PlayerStatus> potentialTargets = ReturnPotentialTargets();

        if (potentialTargets.Count != 0)
            itemTrajectory.target = potentialTargets[0];
        else
            itemTrajectory.target = null;

        itemTrajectory.isMidAir = false;
        itemTrajectory.isFirstFrameOfThrow = true;
        itemTrajectory.chargeAmount = timeHeld;
        itemTrajectory.thrower = _status;
        itemTrajectory.throwerObject = _status.gameObject;

        equippedItem = null;
        _status.playerHeader.SetWeaponText("");
        weaponState = new Unarmed(_status.playerNumber, _hitbox);

        _status.SetPlayerStateImmediately(new ThrowRecovery());

        timeHeld = 0;

        _status.visuals.SetAnimationModifier(AnimationModifier.None);
        _status.visuals.ShowWeaponVisual(ItemType.Unarmed);
    }

    public void DropWeapon()
    {
        if (!equippedItem)
            return;

        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.transform.parent = null;
        equippedItem.SetActive(true);

        ItemTrajectory itemTrajectory = equippedItem.GetComponent<ItemTrajectory>();
        itemTrajectory.despawnTimerActive = true;

        equippedItem = null;
        if (_status.playerHeader)
            _status.playerHeader.SetWeaponText("");
        weaponState = new Unarmed(_status.playerNumber, _hitbox);

        _status.visuals.SetAnimationModifier(AnimationModifier.None);
        _status.visuals.ShowWeaponVisual(ItemType.Unarmed);
    }

    public void BreakWeapon()
    {
        if (equippedItem != null)
        {
            GameObject weapon = equippedItem;
            DropWeapon();
            Destroy(weapon);
        }
    }

    /// <summary>
    /// Looks for players in an arc in front of current player.
    /// </summary>
    /// <param name="targetAngle">Angle in front of the player that it looks in.</param>
    /// <returns>Returns list of players in front sorted by shortest to furthest, null if none.</returns>
    public List<PlayerStatus> ReturnPotentialTargets(float targetAngle = 0.5f, float dist = 25)
    {
        List<PlayerStatus> potentialTargets = new List<PlayerStatus>();

        // Grab all players. (gameManager does this)
        foreach (PlayerStatus s in GameManager.game.alivePlayerStatuses)
        {
            Vector3 vectorToCollider = (
                s.transform.position - _status.transform.position
            ).normalized;
            // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
            if (
                Vector3.Dot(vectorToCollider, _status.movement.ActualFowardDirection) > targetAngle
                && Vector3.Distance(_status.transform.position, s.transform.position) < dist
                && Mathf.Abs(_status.transform.position.y - s.transform.position.y) < 10
                && !s.waitingToBeEliminated
            )
            {
                // If in the arc, add to potential target list.
                potentialTargets.Add(s);
            }
        }

        // Now sort the list by distance, shortest to furthest.
        if (potentialTargets.Count != 0)
            potentialTargets = potentialTargets
                .OrderBy(x => Vector3.Distance(_status.transform.position, x.transform.position))
                .ToList();

        return potentialTargets;
    }

    public void PauseWeaponAnimationModifier()
    {
        switch (weaponState.animationModifier)
        {
            case (AnimationModifier.CarryOverHead):
                _status.visuals.SetAnimationModifier(AnimationModifier.None);
                break;

            case (AnimationModifier.FistsOverHead):
                // _status.visuals.SetAnimationModifier(AnimationModifier.RightHandFist);
                break;
        }
    }

    public void ResumeWeaponAnimationModifier()
    {
        _status.visuals.SetAnimationModifier(weaponState.animationModifier);
    }
}
