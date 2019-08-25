using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : Modifier
{
    public enum ModifierAddType { Add, Multiply, Override }
    private ModifierAddType modifierAddType = ModifierAddType.Add;
    private float speedMod = 2.0f;

    public SpeedModifier(float speedMod, ModifierAddType addType)
    {
        this.speedMod = speedMod;
        this.modifierAddType = addType;
    }

    public float ModifySpeed(float speed)
    {
        switch (modifierAddType)
        {
            case ModifierAddType.Add:
                return speed + speedMod;
            case ModifierAddType.Multiply:
                return speed * speedMod;
            case ModifierAddType.Override:
                return speedMod;
            default:
                return speed;
        }
    }

    public override void ActivateModifier(CastleShip castleShipRef)
    {
        Debug.Log("Activate modifier");
        base.ActivateModifier(castleShipRef);
        castleShipRef.AddSpeedModifier(this);
    }

    public override void DeactivateModifier(CastleShip castleShipRef)
    {
        base.DeactivateModifier(castleShipRef);
        castleShipRef.RemoveSpeedModifier(this);
    }
}