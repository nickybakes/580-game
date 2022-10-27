using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SteelChair : DefaultState
{
    public SteelChair(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.FistsOverHead;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.0f, // Damage Multiplier
                0.2f, // Knockback Multiplier
                0.2f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                0.5f,  // Forward Speed Multiplier
                AttackAnimation.Swipe_01
                ),
            new Attack( // *** 2nd Hit ***
                1.0f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                0.5f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                2.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                0.5f,  // Forward Speed Multiplier
                AttackAnimation.SmashHeavy_01
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
