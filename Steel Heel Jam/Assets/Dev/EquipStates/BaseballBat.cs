using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : DefaultState
{
    public BaseballBat(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.RightHandFist;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                2.0f, // Damage Multiplier
                2.0f, // Knockback Multiplier
                1.6f, // Knockback Height Multiplier
                1.5f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                2.0f, // Startup Multiplier
                1.3f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                0.3f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_01,
                AttackDirection.Horizontal
                )
        };

        maxComboCount = combo.Length;
        currentAttack = combo[0];

        airAttack = new Attack( // *** 1st Hit ***
                1.0f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f  // Forward Speed Multiplier
                );
    }
}
