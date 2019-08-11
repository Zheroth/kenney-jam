using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : Modifier
{
    public override void ActivateModifier(CastleShip castleShipRef)
    {
        castleShipRef.SetCurrentThrust(2.0f);
    }

    public override void DeactivateModifier(CastleShip castleShipRef)
    {
        castleShipRef.SetCurrentThrust(1.0f);
    }
}
