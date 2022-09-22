using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Unarmed : DefaultState
{
    public Unarmed(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        //SetupHitboxReferences(_hitbox);

        damageMultiplier = 1.0f;
        knockbackMultiplier = 1.0f;
        knockbackHeightMultiplier = 1.0f;
        hitstunMultiplier = 1.0f;
        radiusMultiplier = 1.0f;
        startupMultiplier = 1.0f;
        durationMultiplier = 1.0f;
        recoveryMultiplier = 1.0f;
        comboCount = 3;
    }

    private void SetInitialHit()
    {

    }

    public override void Attack()
    {
        switch (currentHit)
        {
            case 1:

                break;
            case 2:
                damageMultiplier = 0.7f;
                knockbackMultiplier = 0.7f;
                knockbackHeightMultiplier = 0.5f;
                hitstunMultiplier = 0.5f;
                startupMultiplier = 0.5f;
                durationMultiplier = 0.7f;
                
                break;
            case 3:
                break;
        }

        currentHit += 1;

        base.Attack();
    }
}
