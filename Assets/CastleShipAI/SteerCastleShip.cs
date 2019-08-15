using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("CastleShip/")]
[TaskDescription("Steers the castle ship to a target")]
public class CastleShipSeek : Action
{
    [SerializeField]
    private SharedCastleShip sharedCastleShip;

    [SerializeField]
    private SharedTransform steerTarget;

    void Seek()
    {
        Vector3 targetPosition = steerTarget.Value.position;
        Vector3 castleShipPosition = sharedCastleShip.Value.transform.position;
        targetPosition.y = castleShipPosition.y;

        Vector3 targetDir = targetPosition - castleShipPosition;
        Vector3 forward = sharedCastleShip.Value.transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        int sign = 0;
        if(angle < 0)
        {
            sign = -1;
        }
        else if(angle > 0)
        {
            sign = 1;
        }

        sharedCastleShip.Value.SetCurrentTurn(-sign);

        float distance = Vector3.Distance(targetPosition, castleShipPosition);

        float speed = 1;
        speed = Mathf.Lerp(0,1,distance/3);

        if(distance > 5)
        {
            sharedCastleShip.Value.SetCurrentThrust(speed * sharedCastleShip.Value.forwardAcceleration);
        }
        else if(distance < 3)
        {
            sharedCastleShip.Value.SetCurrentThrust(-1 * sharedCastleShip.Value.backwardAcceleration);
        }
        else
        {
            sharedCastleShip.Value.SetCurrentThrust(0);
        }
    }

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        //sharedCastleShip.Value.SetCurrentTurn(turnAmount);
        Seek();
        return TaskStatus.Running;
    }
}