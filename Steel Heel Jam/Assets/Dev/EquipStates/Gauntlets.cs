using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlets : DefaultState
{
    public Gauntlets(int _playerNumber, GameObject _hitbox) : base(_playerNumber, _hitbox)
    {
        //SetupHitboxReferences(_hitbox);

        SetInitialHit();
        maxComboCount = 3;
    }

    protected override void SetInitialHit()
    {
        damageMultiplier = 1.2f;
        knockbackMultiplier = 1.0f;
        knockbackHeightMultiplier = 0.7f;
        hitstunMultiplier = 1.0f;
        radiusMultiplier = 1.0f;
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
                damageMultiplier = 1.3f;
                knockbackMultiplier = 0.7f;
                knockbackHeightMultiplier = 0.7f;
                radiusMultiplier = 1.5f;
                startupMultiplier = 0.7f;
                forwardSpeedModifierMultiplier = 1.1f;
                break;
            case 2:
                damageMultiplier = 1.7f;
                knockbackMultiplier = 1.7f;
                knockbackHeightMultiplier = 2.0f;
                radiusMultiplier = 2.5f;
                startupMultiplier = 0.7f;
                forwardSpeedModifierMultiplier = 1.3f;
                break;
        }
    }
}
