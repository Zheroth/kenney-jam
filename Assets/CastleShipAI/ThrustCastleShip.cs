using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CastleShip/")]
[TaskDescription("Adds thrust to the castle ship")]
public class ThrustCastleShip : Action
{
    [SerializeField]
    private SharedCastleShip sharedCastleShip;

    [Range(-1, 1)]
    [SerializeField]
    private float thrustAmount;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if(thrustAmount > 0)
        {
            sharedCastleShip.Value.SetCurrentThrust(thrustAmount * sharedCastleShip.Value.forwardAcceleration);
        }
        else if(thrustAmount < 0)
        {
            sharedCastleShip.Value.SetCurrentThrust(thrustAmount * sharedCastleShip.Value.backwardAcceleration);
        }
        else
        {
            sharedCastleShip.Value.SetCurrentThrust(0.0f);

        }
        return TaskStatus.Success;
    }
}