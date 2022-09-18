using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerCombat : MonoBehaviour
{
    private StarterAssetsInputs _input;
    private GameObject _hitbox;
    private PlayerStatus _status;

    private DefaultState weaponState;

    private float dodgeRollCoolDown;
    private const float dodgeRollCoolDownMax = .2f;

    private float blockCoolDown;
    private const float blockCoolDownMax = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _hitbox = transform.GetChild((int)PlayerChild.Hitbox).gameObject;

        _status = GetComponent<PlayerStatus>();

        weaponState = new DefaultState(1, _hitbox);
        weaponState.InitHitbox();
    }

    /// <summary>
    /// This is to be updated via the PlayerStatus script's Update method. This will check for combat based inputs such as Attacking, Dodge Rolling, and Blocking.
    /// </summary>
    /// <param name="canAttack">True if the player can interrupt their current state and begin an Attack. Base off the player's current PlayerState</param>
    /// <param name="canDodgeRoll">True if the player can interrupt their current state and go into a dodge roll. Base off the player's current PlayerState</param>
    /// <param name="canBlock">True if the player can interrupt their current state and activate a block. Base off the player's current PlayerState</param>
    /// 
    public void UpdateManual(bool canAttack, bool canDodgeRoll, bool canBlock)
    {
        //fixes the null ref exception when recompiling in the Editor
#if UNITY_EDITOR
        if (weaponState == null)
        {
            weaponState = new DefaultState(1, _hitbox);
            weaponState.InitHitbox();
        }
#endif

        if (_input.attack)
        {
            weaponState.Attack();
            _input.attack = false;
        }

        if(_status.CurrentPlayerState.countDodgeRollCooldown && dodgeRollCoolDown < dodgeRollCoolDownMax)
            dodgeRollCoolDown += Time.deltaTime;

        if (_status.CurrentPlayerState.countBlockdown && blockCoolDown < blockCoolDownMax)
            blockCoolDown += Time.deltaTime;

        if (canDodgeRoll && dodgeRollCoolDown > dodgeRollCoolDownMax && _input.dodgeRoll && !_input.wasDodgeRolling)
        {
            DodgeRoll();
        }

        if(canBlock && blockCoolDown > blockCoolDownMax && _input.block && !_input.wasBlocking)
        {
            Block();
        }
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
        _status.SetPlayerStateImmediately(new Block());
    }


}
