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
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f  // Forward Speed Multiplier
                ),
            new Attack( // *** 2nd Hit ***
                1.8f, // Damage Multiplier
                1.2f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                2.3f, // Radius Multiplier
                1.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.3f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.5f  // Forward Speed Multiplier
                ),
            new Attack( // *** 3rd Hit ***
                2.0f, // Damage Multiplier
                1.2f, // Knockback Multiplier
                0.4f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                2.9f, // Radius Multiplier
                1.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.6f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                2.0f  // Forward Speed Multiplier
                ),
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
