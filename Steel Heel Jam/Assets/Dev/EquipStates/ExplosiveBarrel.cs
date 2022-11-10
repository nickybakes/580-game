using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DefaultState
{
    public ExplosiveBarrel(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        animationModifier = AnimationModifier.CarryOverHead;
        explodeOnHit = true;
    }

    public override void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack( // *** 1st Hit ***
                1.0f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                2.0f, // Hitstun Multiplier
                2.5f, // Radius Multiplier
                1.5f, // Height Multiplier
                2.0f, // Startup Multiplier
                3.0f, // Duration Multiplier
                2.0f, // Recovery Multiplier
                0.7f,  // Forward Speed Multiplier
                AttackAnimation.SmashHeavy_01
                )
        };

        maxComboCount = combo.Length;
        currentAttack = combo[0];

        InitializeAirAttack();
        airAttack.radiusMultiplier = 2.0f;
    }
}
