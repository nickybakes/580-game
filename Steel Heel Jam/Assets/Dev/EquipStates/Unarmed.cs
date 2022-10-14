using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Unarmed : DefaultState
{
    public Unarmed(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {

    }

    protected override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
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
                ),
            new Attack( // *** 2nd Hit ***
                0.8f, // Damage Multiplier
                0.7f, // Knockback Multiplier
                0.5f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.1f  // Forward Speed Multiplier
                ),
            new Attack( // *** 3rd Hit ***
                1.5f, // Damage Multiplier
                1.5f, // Knockback Multiplier
                1.5f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                2.5f, // Radius Multiplier
                1.0f, // Height Multiplier
                0.7f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.3f  // Forward Speed Multiplier
                ),
        };

        maxComboCount = combo.Length;
    }
}
