using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : DefaultState
{
    public Ladder(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.CarryOverHead;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.5f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                1.2f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.2f  // Forward Speed Multiplier
                ),
            new Attack( // *** 2nd Hit ***
                2.5f, // Damage Multiplier
                1.5f, // Knockback Multiplier
                0f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.2f, // Startup Multiplier
                1.2f, // Duration Multiplier
                1.4f, // Recovery Multiplier
                1.4f  // Forward Speed Multiplier
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