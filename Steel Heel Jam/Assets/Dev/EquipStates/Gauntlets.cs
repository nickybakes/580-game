using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlets : DefaultState
{
    public Gauntlets(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.RightHandFist;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.8f, // Damage Multiplier
                1.8f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                2.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                4.0f, // Startup Multiplier
                2.5f, // Duration Multiplier
                2.0f, // Recovery Multiplier
                3.0f,  // Forward Speed Multiplier
                AttackAnimation.Punch_03,
                _attackParticle: AttackParticle.Thrust
                )
        };

        maxComboCount = combo.Length;
        currentAttack = combo[0];

        InitializeAirAttack();
        airAttack.radiusMultiplier = 1;
    }
}
