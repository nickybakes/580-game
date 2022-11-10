using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : DefaultState
{
    public Saber(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.RightHandFist;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.5f, // Damage Multiplier
                1.2f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.5f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                5.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_01,
                AttackDirection.Forward
                ),
            new Attack( // *** 2nd Hit ***
                1.8f, // Damage Multiplier
                1.2f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                5.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.3f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.5f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_02,
                AttackDirection.Forward
                ),
            new Attack( // *** 3rd Hit ***
                2.0f, // Damage Multiplier
                1.2f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                2.0f, // Radius Multiplier
                10.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.6f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                2.0f,  // Forward Speed Multiplier
                AttackAnimation.Stab_01,
                AttackDirection.Forward
                ),
        };

        maxComboCount = combo.Length;
        currentAttack = combo[0];

        InitializeAirAttack();
        airAttack.radiusMultiplier = 1.6f;
    }
}
