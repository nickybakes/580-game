using UnityEngine;

public enum PlayerChild
{
    Model = 0,
    Hitbox = 1,
    Visuals = 3,
    PickUpSphere = 4,
    GrabHitbox = 5,
    RingDecal = 6,
    Particles = 8,
}

public enum ParticleChild
{
    PoisonTrail_01
}

public enum Buff
{
    PlotArmor,
    RedemptionArc,
    SpeedySubversion,
    MachoBlock,
    TopRopes,
    TheStink
}

public enum Tag
{
    Player,
    PickUp,
    UICursor,
}


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerStatus : MonoBehaviour
{
    //public List<Buff> buffs = new List<Buff>();

    [SerializeField] private const float HeelStaminaDamage = 5f;

    public const float defaultMaxStamina = 100f;
    public const float defaultMaxSpotlight = 100f;

    /// <summary>
    /// The stamina value for the player. Stamina is consumed for actions and is lost upon being hit, being the heel, or being outside of the ring.
    /// When a player's stamina is empty and they are knocked out of the zone, they are eliminated.
    /// </summary>
    public float stamina = defaultMaxStamina;
    /// <summary>
    /// The player's current maximum stamina value.
    /// </summary>
    public float maxStamina = defaultMaxStamina;
    /// <summary>
    /// The lowest a player's maximum stamina can get.
    /// </summary>
    /// 

    public float spotlight;
    public bool isInSpotlight;

    [SerializeField] private const float MinMaxStamina = 50;

    private BasicState currentPlayerState;

    [HideInInspector]
    public PlayerMovement movement;

    [HideInInspector]
    public PlayerCombat combat;

    [HideInInspector]
    public PlayerVisuals visuals;

    public int playerNumber;

    public int PlayerNumber { get { return playerNumber; } }

    public BasicState CurrentPlayerState { get { return currentPlayerState; } }

    public bool attackBlocked = false;

    [SerializeField] public float missedBlockStaminaDamage = 20f;

    [SerializeField] public float dodgeRollStaminaDamage = 16f;

    [SerializeField] public float suplexStaminaDamage = 50f;
    [SerializeField] public float missedGrabStaminaDamage = 10f;

    /// <summary>
    /// A boolean that represents if the player is outside of the ring.
    /// </summary>
    private bool isOOB = false;

    /// <summary>
    /// The damage to stamina that the player takes every interval while out of bounds.
    /// </summary>
    [SerializeField] public const float OOBStaminaDamage = 10f;

    /// <summary>
    /// The loss of max stamina that the player accrues every interval while out of bounds.
    /// </summary>
    [SerializeField] private const float OOBMaxStaminaDamage = 5f;

    /// <summary>
    /// The amount of stamina restored every interval when not active.
    /// </summary>
    [SerializeField] private const float PassiveStaminaRegen = 5.0f;

    public float totalDamageTaken;

    public float recentDamageTaken;

    public float totalDamagerDealt;

    public int totalEliminations;

    public PlayerStatus playerLastHitBy;

    public float recentDamageTakenMax;

    public float recentDamageTimeCurrent;

    private const float recentActivityTimeMax = 5;

    public float recentActivityTimeCurrent;

    public bool eliminated;
    public bool waitingToBeEliminated;

    public float timeOfEliminiation;

    public GameObject heelFireHitbox;

    public PlayerHeader playerHeader;

    private new Transform transform;

    // ******************************
    // Buff Values
    public bool[] buffs;
    private int maxBuffs = 2;
    private int buffCount = 0;

    public float plotArmorAdditionalHeal = 8.0f;
    public float redemptionArcDamageMultiplier = 2.0f;
    public float redemptionArcKnockbackMultiplier = 2.0f;
    public bool canDoubleJump = false;
    public bool isHeel = false;
    public const float heelFireCooldownMax = 10.0f;
    public float heelFireCooldown;

    private bool isPoisoned = false;
    private float poisonTimeCurrent;
    private float poisonTimeMax = 4;
    private float poisonDamageAmountPerSecond = 4;
    // ******************************

    private bool iFrames;

    public float ActivityScore
    {
        get
        {
            return recentDamageTaken + totalDamagerDealt + (totalEliminations * 25);
        }
    }

    public bool IsFlexing
    {
        get
        {
            return (currentPlayerState is Flexing);
        }
    }

    /// <summary>
    /// Use this to check if the player is currently dodging when you want to hit them with an attack
    /// </summary>
    /// <value>True if the player is currently dodging on this frame</value>
    public bool IsDodgeRolling
    {
        get
        {
            return (currentPlayerState is DodgeRoll);
        }
    }

    public bool IsBlocking
    {
        get
        {
            return (currentPlayerState is Block);
        }
    }

    public Transform GetTransform
    {
        get { return transform; }
    }

    public int BuffCount
    {
        get { return buffCount; }
    }

    public bool IFrames
    {
        get
        {
            return iFrames;
        }

        set
        {
            if (value)
            {
                iFrames = true;
                visuals.EnableIFrames();
            }
            else
            {
                iFrames = false;
                visuals.DisableIFrames();
            }
        }
    }

    public bool IsOOB
    {
        get { return isOOB; }
    }

    /// <summary>
    /// Gives you the current moveSpeed of the character (base move speed multiplied by the current state's move speed multiplier)
    /// </summary>
    /// <value>the current moveSpeed of the character</value>
    public float CurrentMoveSpeed { get { return movement.moveSpeed * currentPlayerState.moveSpeedMultiplier; } }

    // Start is called before the first frame update
    void Awake()
    {
        transform = gameObject.transform;
        currentPlayerState = new Idle();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();

        movement.Start();
        combat.Start();

        visuals = new PlayerVisuals(this, transform);
        buffs = new bool[6];
    }

    // Update is called once per frame
    void Update()
    {
        //fixes the null ref exception when recompiling in the Editor
#if UNITY_EDITOR
        if (currentPlayerState == null)
            currentPlayerState = new Idle();
#endif

        // Prevents player movement on game start countdown.
        if (GameManager.game.dontUpdateGameplay)
            return;

        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate, currentPlayerState.alternateFriction);

        combat.UpdateManual(currentPlayerState.canAttack, currentPlayerState.canDodgeRoll, currentPlayerState.canBlock, currentPlayerState.canPickUp, currentPlayerState.canThrow);

        currentPlayerState.Update(this);

        if (currentPlayerState.changeStateNow)
            SetPlayerStateImmediately(currentPlayerState.stateToChangeTo);

        if (!eliminated && waitingToBeEliminated && !(currentPlayerState is Knockback || currentPlayerState is ImpactStun))
        {
            SetPlayerStateImmediately(new Eliminated());
            eliminated = true;
        }

        if (isHeel)
        {
            heelFireCooldown += Time.deltaTime;

            if (heelFireCooldown > heelFireCooldownMax)
            {
                heelFireHitbox.SetActive(false);
                isHeel = false;
                playerHeader.SetHeel(false);

                AudioManager.aud.Stop("heelFire");
            }
            // If close to finishing, start to fade sfx.
            if (heelFireCooldown > heelFireCooldownMax - 1)
            {
                AudioManager.aud.StartFade("heelFire", 1.0f, 0.0f);
            }
        }

        // If the player is out of bounds . . .
        if (isOOB && !currentPlayerState.isInvincibleToRing)
        {
            ReduceStamina(OOBStaminaDamage * Time.deltaTime);
        }

        if (!IsFlexing)
        {
            {
                IncreaseStamina(PassiveStaminaRegen * Time.deltaTime);
            }
        }

        if (isPoisoned)
        {
            poisonTimeCurrent += Time.deltaTime;

            if (!currentPlayerState.isInvincibleToRing) ReduceStamina(poisonDamageAmountPerSecond * Time.deltaTime);

            if (poisonTimeCurrent > poisonTimeMax)
            {
                isPoisoned = false;
                poisonTimeCurrent = 0;
                transform.GetChild((int)PlayerChild.Particles).GetChild((int)ParticleChild.PoisonTrail_01).gameObject.SetActive(false);
            }
        }

        if (recentDamageTimeCurrent > 0)
        {
            recentDamageTimeCurrent -= Time.deltaTime;

            recentDamageTaken = Mathf.Max(0, Mathf.Lerp(0, recentDamageTakenMax, recentDamageTimeCurrent / 6f));
        }

        if (recentActivityTimeCurrent > 0)
        {
            recentActivityTimeCurrent -= Time.deltaTime;
        }
    }

    public void SetPlayerStateImmediately(BasicState state)
    {
        if (eliminated)
            return;

        if (waitingToBeEliminated && !eliminated && !(state is Eliminated))
        {
            state = new Eliminated();
            eliminated = true;
        }



        currentPlayerState.OnExitThisState(state, this);
        state.OnEnterThisState(currentPlayerState, this);

        visuals.SetAnimationState(state.animationState);
        visuals.EnableVisual(state.visual);

        // Debug.Log(currentPlayerState + ", " + state);

        currentPlayerState = state;
    }

    public void SetAnimationState(AnimationState state)
    {
        visuals.SetAnimationState(state);
    }

    private void GetHit(Vector3 hitDirection, float damage, float knockback, float knockbackHeight, float timeInKnockback, PlayerStatus attackingPlayerStatus, bool moveVictimWithAttacker, bool forceActivateIFrames)
    {
        if (attackingPlayerStatus != null && attackingPlayerStatus.playerNumber != playerNumber)
            playerLastHitBy = attackingPlayerStatus;

        Vector3 knockbackDir = hitDirection;
        if(knockbackDir.x == 0)
            knockbackDir.x = .01f;
        transform.forward = new Vector3(knockbackDir.x * -1, 0, knockbackDir.z * -1);
        knockback = knockback * (2 + stamina / defaultMaxStamina);

        float staminaRatio = (maxStamina - stamina) * 0.2f;

        float staminaRatioX = staminaRatio;
        float staminaRatioZ = staminaRatio;

        if (knockbackDir.x < 0) staminaRatioX = -staminaRatio;
        else if (knockbackDir.x == 0) staminaRatioX = 0;

        if (knockbackDir.z < 0) staminaRatioZ = -staminaRatio;
        else if (knockbackDir.z == 0) staminaRatioZ = 0;

        Vector3 knockbackVelocity = new Vector3(knockbackDir.x * knockback + staminaRatioX, knockbackHeight + staminaRatio, knockbackDir.z * knockback + staminaRatioZ);
        movement.grounded = false;

        CameraManager.cam.ShakeCamera(Mathf.Clamp01(knockback - 14f) / 4f);

        SetPlayerStateImmediately(new ImpactStun(attackingPlayerStatus, knockbackVelocity, moveVictimWithAttacker));
        currentPlayerState.stateToChangeTo.timeToChangeState = timeInKnockback;

        if (attackingPlayerStatus != null)
        {
            attackingPlayerStatus.totalDamagerDealt += damage;
            attackingPlayerStatus.IncreaseSpotlightMeter(damage / 3);
            attackingPlayerStatus.combat.weaponState.gotAHit = true;
            attackingPlayerStatus.recentActivityTimeCurrent = recentActivityTimeMax;
        }

        ReduceStamina(damage);
        totalDamageTaken += damage;

        IncreaseSpotlightMeter(damage / 4);

        recentDamageTaken += damage;
        recentDamageTakenMax = recentDamageTaken;
        recentDamageTimeCurrent = 6f;

        if (forceActivateIFrames || recentDamageTaken >= 50f)
        {
            IFrames = true;
        }

        if (totalDamageTaken > 175f && recentDamageTaken > 40f)
        {
            combat.DropWeapon();
            ReduceMaxStamina(damage);

            // For now, VO bigHit sounds will coincide with dropping a weapon (as an audio queue)
            AnnouncerManager.PlayLine("bigHitDropWeapon", Priority.Damage);
        }

        combat.ActedRecently = true;

        recentActivityTimeCurrent = recentActivityTimeMax;

    }

    public void GetHitByElbowDrop(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float timeInKnockback, PlayerStatus attackingPlayerStatus, bool unblockable)
    {
        if (eliminated || waitingToBeEliminated)
            return;

        if (attackingPlayerStatus.combat.weaponState.explodeOnHit)
        {
            GameManager.game.SpawnExplosion(attackingPlayerStatus.combat.weaponState.hitbox.transform.position, attackingPlayerStatus, true);
            attackingPlayerStatus.combat.BreakWeapon();
            AudioManager.aud.Play("punch", 0.8f, 1.2f);
            return;
        }

        if (!unblockable && IsBlocking)
        {
            attackingPlayerStatus.SetPlayerStateImmediately(new BlockedStun());
            attackingPlayerStatus.movement.velocity = attackingPlayerStatus.transform.position - transform.position;
            attackingPlayerStatus.combat.weaponState.currentComboCount = 0;
            IncreaseSpotlightMeter(15);
            SetPlayerStateImmediately(new Idle());
            attackBlocked = true;
            if (buffs[(int)Buff.MachoBlock])
            {
                GameManager.game.SpawnExplosion(transform.position, this, false);
            }

            // Play sfx and VO.
            AudioManager.aud.Play("blockedPunch");
            AnnouncerManager.PlayLine("block", Priority.Block);
            return;
        }

        if (!currentPlayerState.isInvincibleToAttacks && !iFrames)
        {
            Vector3 direction = transform.position - attackingPlayerStatus.transform.position;
            direction.y = 0;
            GetHit(direction.normalized, damage, knockback, knockbackHeight, timeInKnockback, attackingPlayerStatus, false, true);

            //play crunch sound
            AudioManager.aud.Play("punch", 0.8f, 1.2f);
        }
    }

    public void GetHitByExplosive(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float timeInKnockback, PlayerStatus attackingPlayerStatus)
    {
        if (eliminated || waitingToBeEliminated)
            return;

        if (!currentPlayerState.isInvincibleToAttacks)
        {
            Vector3 direction = collisionPos - hitboxPos;
            direction.y = 0;
            GetHit(direction.normalized, damage, knockback, knockbackHeight, timeInKnockback, attackingPlayerStatus, false, true);
        }
    }


    public void GetHitByMelee(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float timeInKnockback, PlayerStatus attackingPlayerStatus)
    {
        if (eliminated || waitingToBeEliminated)
            return;

        if (attackingPlayerStatus.combat.weaponState.explodeOnHit)
        {
            GameManager.game.SpawnExplosion(attackingPlayerStatus.combat.weaponState.hitbox.transform.position, attackingPlayerStatus, true);
            attackingPlayerStatus.combat.BreakWeapon();
            AudioManager.aud.Play("punch", 0.8f, 1.2f);
            return;
        }

        if (IsBlocking)
        {
            attackingPlayerStatus.SetPlayerStateImmediately(new BlockedStun());
            attackingPlayerStatus.movement.velocity = attackingPlayerStatus.transform.position - transform.position;
            attackingPlayerStatus.combat.weaponState.currentComboCount = 0;
            IncreaseSpotlightMeter(20);
            SetPlayerStateImmediately(new Idle());
            attackBlocked = true;
            if (buffs[(int)Buff.MachoBlock])
            {
                GameManager.game.SpawnExplosion(transform.position, this, false);
            }

            AudioManager.aud.Play("blockedPunch");
            return;
        }

        if (!currentPlayerState.isInvincibleToAttacks && !iFrames)
        {
            GetHit(attackingPlayerStatus.transform.forward, damage, knockback, knockbackHeight, timeInKnockback, attackingPlayerStatus, true, false);

            if (attackingPlayerStatus.combat.weaponState.currentComboCount >= attackingPlayerStatus.combat.weaponState.maxComboCount)
            {
                if (attackingPlayerStatus.buffs[(int)Buff.RedemptionArc] == true)
                {
                    // Spawn explosion here
                    GameManager.game.SpawnExplosion(attackingPlayerStatus.combat.weaponState.hitbox.transform.position, attackingPlayerStatus, false, .5f, .65f);
                }
                if (attackingPlayerStatus.buffs[(int)Buff.TheStink] == true)
                {
                    // Poison player
                    isPoisoned = true;
                    transform.GetChild((int)PlayerChild.Particles).GetChild((int)ParticleChild.PoisonTrail_01).gameObject.SetActive(true);
                }
            }

            // Plays orchestra hits for combo.
            int combo = attackingPlayerStatus.combat.weaponState.currentComboCount;

            if (combo > 0 && combo < attackingPlayerStatus.combat.weaponState.maxComboCount)
                AudioManager.aud.Play("orchestraHitShort", combo, combo);
            if (combo == attackingPlayerStatus.combat.weaponState.maxComboCount)
            {
                // If the full combo causes Iframes, plays VO line.
                if (IFrames)
                    AnnouncerManager.PlayLine("bigHitCombo", Priority.Damage);

                AudioManager.aud.Play("orchestraHitLong");
                CameraManager.cam.ShakeCamera(.2f);
            }

            // Always plays default punch. If a weapon is equipped, also plays that specific sound.
            AudioManager.aud.Play("punch", 0.8f, 1.2f);

            if (attackingPlayerStatus.combat.equippedItem != null)
            {
                string soundToPlay = attackingPlayerStatus.combat.equippedItem.name;
                // Remove '(clone)' from end of string. (Sounds in AudioManager named same as weapons)
                soundToPlay = soundToPlay.Remove(soundToPlay.Length - 7);
                AudioManager.aud.Play(soundToPlay);
            }
        }
    }

    public bool GetHitByThrowable(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, PlayerStatus attackingPlayerStatus, bool explosionOnHit)
    {
        // If eliminated/blocking/dodgerolling, nothing happens.
        if (eliminated || currentPlayerState.isInvincibleToAttacks || waitingToBeEliminated || iFrames)
            return false;

        if (explosionOnHit)
        {
            GameManager.game.SpawnExplosion(hitboxPos, attackingPlayerStatus, true);
            return true;
        }

        if (IsBlocking)
        {
            IncreaseSpotlightMeter(20);
            IncreaseStamina(15);
            SetPlayerStateImmediately(new Idle());
            attackBlocked = true;
            if (buffs[(int)Buff.MachoBlock])
            {
                GameManager.game.SpawnExplosion(transform.position, this, false);
            }

            AudioManager.aud.Play("blockedPunch");
            return false;
        }

        GetHit((collisionPos - hitboxPos).normalized, damage, knockback, knockbackHeight, .3f, attackingPlayerStatus, false, false);

        AudioManager.aud.Play("hitByItem", 0.8f, 1.2f);
        return true;
    }

    public void SetHeel()
    {
        if (playerHeader)
        {
            playerHeader.SetHeel(true);
            heelFireCooldown = 0;
            heelFireHitbox.SetActive(true);
            isHeel = true;

            // HeelFire sfx Fade in.
            AudioManager.aud.Play("heelFire");
            AudioManager.aud.StartFade("heelFire", 1.0f, 0.5f);
        }
    }

    /// <summary>
    /// Provides a buff to the player (Maximum of 2).
    /// </summary>
    /// <param name="buff">Enum of the buff to provide.</param>
    public void GiveBuff()
    {
        // Heel Fire
        if (buffCount == maxBuffs)
        {
            SetHeel();
            //buffs[(int)Buff.HeelFire] = true;

            // VO
            AnnouncerManager.PlayLine("TheHeel", Priority.HeelFire);
            AudioManager.aud.StartFade("cheer", heelFireCooldownMax, 0.7f);
        }
        else
        {
            buffCount++;

            Buff buffToGive = (Buff)Random.Range(0, buffs.Length);
            while (buffs[(int)buffToGive] == true)
            {
                buffToGive = (Buff)Random.Range(0, buffs.Length);
            }
            buffs[(int)buffToGive] = true;

            playerHeader.ShowBuff(buffToGive);

            // VO
            AnnouncerManager.PlayLine(buffToGive.ToString(), Priority.Buff);
            AudioManager.aud.StartFade("cheer", 1.0f, 0.4f);
        }
        CameraManager.cam.ShakeCamera(.5f);
    }

    /// <summary>
    /// Increases the player's stamina. This value will never go above the max.
    /// </summary>
    /// <param name="value">The value to increase the stamina value by.</param>
    public void IncreaseStamina(float value)
    {
        stamina += value;

        if (stamina > maxStamina) stamina = maxStamina;

        if (playerHeader != null)
            playerHeader.UpdateStaminaMeter();
    }

    /// <summary>
    /// Reduces the player's stamina. This value will never go below 0.
    /// </summary>
    /// <param name="value">The value to decrease the stamina value by.</param>
    public void ReduceStamina(float value)
    {
        if (GameManager.game.gameWon)
            return;

        stamina -= value;

        if (stamina < 0) stamina = 0;

        if (playerHeader != null)
            playerHeader.UpdateStaminaMeter();

        if (!(waitingToBeEliminated || eliminated) && stamina == 0 && isOOB)
        {
            GameManager.game.EliminatePlayer(this);
        }
    }

    public void IncreaseSpotlightMeter(float value)
    {
        spotlight += value;

        if (spotlight > defaultMaxSpotlight) spotlight = defaultMaxSpotlight;

        if (playerHeader != null)
            playerHeader.UpdateSpotlightMeter();
    }

    public void ReduceSpotlightMeter(float value)
    {
        spotlight -= value;

        if (spotlight < 0) spotlight = 0;

        if (playerHeader != null)
            playerHeader.UpdateSpotlightMeter();
    }

    /// <summary>
    /// Reduces the player's maximum stamina. If the value would be decreased past the minimum-maximum value, it will be set to the minimum-maximum.
    /// </summary>
    /// <param name="value">The value to decrease the maximum stamina by.</param>
    private void ReduceMaxStamina(float value)
    {
        maxStamina -= value;

        if (maxStamina < MinMaxStamina) maxStamina = MinMaxStamina;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        if (playerHeader != null)
            playerHeader.UpdateStaminaMeter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            isOOB = false;
            if(playerHeader != null)
            {
                playerHeader.SetOutOfRing(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ring")
        {
            isOOB = true;
            if(playerHeader != null)
            {
                playerHeader.SetOutOfRing(true);
            }
        }
    }
}
