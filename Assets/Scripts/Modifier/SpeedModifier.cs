using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : Modifier
{
    public override void ActivateModifier(CastleShip castleShipRef)
    {
        Debug.Log("Activate modifier");
        base.ActivateModifier(castleShipRef);
        castleShipRef.SetCurrentThrustModifier(2.0f);
    }

    public override void DeactivateModifier(CastleShip castleShipRef)
    {
        base.DeactivateModifier(castleShipRef);
        castleShipRef.SetCurrentThrustModifier(1.0f);
    }
}
