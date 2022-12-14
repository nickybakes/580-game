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

        InitializeAirAttack();
        airAttack.radiusMultiplier = 1.5f;
        airAttack.damageMultiplier = 1.5f;
        airAttack.knockbackMultiplier = 1.1f;
        airAttack.knockbackHeightMultiplier = 1.5f;
    }
}
