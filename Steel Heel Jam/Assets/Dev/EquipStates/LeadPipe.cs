using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadPipe : DefaultState
{
    public LeadPipe(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.RightHandFist;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.7f, // Damage Multiplier
                1.4f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.3f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.5f, // Startup Multiplier
                1.3f, // Duration Multiplier
                1.2f, // Recovery Multiplier
                1.4f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_02
                ),
            new Attack( // *** 2nd Hit ***
                1.7f, // Damage Multiplier
                1.4f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.3f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.5f, // Startup Multiplier
                1.3f, // Duration Multiplier
                1.2f, // Recovery Multiplier
                1.4f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_01
                )
        };

        maxComboCount = combo.Length;
        currentAttack = combo[0];

        InitializeAirAttack();
        airAttack.radiusMultiplier = 1.4f;
        airAttack.damageMultiplier = 1.5f;
        airAttack.knockbackMultiplier = 1.1f;
        airAttack.knockbackHeightMultiplier = 1.7f;
    }
}
