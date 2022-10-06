using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : DefaultState
{
    public Saber(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        //SetupHitboxReferences(_hitbox);

        SetInitialHit();
        maxComboCount = 3;
    }

    protected override void SetInitialHit()
    {
        damageMultiplier = 1.2f;
        knockbackMultiplier = 1.2f;
        knockbackHeightMultiplier = 0.4f;
        hitstunMultiplier = 1.5f;
        radiusMultiplier = 1.4f;
        startupMultiplier = 1.0f;
        durationMultiplier = 1.0f;
        recoveryMultiplier = 1.0f;
    }

    public override void UpdateValues()
    {
        switch (currentComboCount)
        {
            case 0:
                SetInitialHit();
                break;
            case 1:
                damageMultiplier = 1.2f;
                knockbackMultiplier = 1.2f;
                knockbackHeightMultiplier = 0.4f;
                radiusMultiplier = 1.6f;
                startupMultiplier = 0.7f;
                forwardSpeedModifierMultiplier = 1.1f;
                break;
            case 2:
                damageMultiplier = 1.5f;
                knockbackMultiplier = 1.5f;
                knockbackHeightMultiplier = 0.6f;
                radiusMultiplier = 2.5f;
                startupMultiplier = 0.7f;
                forwardSpeedModifierMultiplier = 1.4f;
                durationMultiplier = 1.4f;
                break;
        }
    }
}
