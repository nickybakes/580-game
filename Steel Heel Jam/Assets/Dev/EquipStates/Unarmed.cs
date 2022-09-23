using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Unarmed : DefaultState
{
    public Unarmed(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        //SetupHitboxReferences(_hitbox);

        SetInitialHit();
        maxComboCount = 3;
    }

    protected override void SetInitialHit()
    {
        damageMultiplier = 1.0f;
        knockbackMultiplier = 1.0f;
        knockbackHeightMultiplier = 1.0f;
        hitstunMultiplier = 1.0f;
        radiusMultiplier = 1.0f;
        startupMultiplier = 1.0f;
        durationMultiplier = 1.0f;
        recoveryMultiplier = 1.0f;
    }

    public override void Attack()
    {
        switch (currentComboCount)
        {
            case 0:
                SetInitialHit();
                break;
            case 1:
                damageMultiplier = 0.7f;
                knockbackMultiplier = 0.7f;
                knockbackHeightMultiplier = 0.5f;
                hitstunMultiplier = 0.5f;
                radiusMultiplier = 1.5f;
                startupMultiplier = 0.5f;
                durationMultiplier = 0.7f;
                recoveryMultiplier = 0.7f;
                forwardDisplacementMultiplier = 0.5f;
                break;
            case 2:
                damageMultiplier = 1.5f;
                knockbackMultiplier = 1.5f;
                knockbackHeightMultiplier = 1.5f;
                hitstunMultiplier = 1.0f;
                radiusMultiplier = 2.5f;
                startupMultiplier = 0.5f;
                durationMultiplier = 1.0f;
                recoveryMultiplier = 5.0f;
                forwardDisplacementMultiplier = 1.0f;
                break;
        }
        InitHitbox();

        base.Attack();
    }
}
