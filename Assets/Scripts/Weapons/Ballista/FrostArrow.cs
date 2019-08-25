using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostArrow : BallistaArrow
{
    [SerializeField]
    private float speedModifier;
    protected override void OnCastleShipHit(CastleShip hitCastleShip)
    {
        base.OnCastleShipHit(hitCastleShip);
        hitCastleShip.AddModifier(new SpeedModifier(speedModifier, SpeedModifier.ModifierAddType.Multiply), 8.0f);
    }
}