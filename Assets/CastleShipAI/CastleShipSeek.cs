using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnitySteer.Behaviors;
using System.Linq;

[TaskCategory("CastleShip/")]
[TaskDescription("Steers the castle ship to a target")]
public class CastleShipSeek : Action
{
    [SerializeField]
    private SharedCastleShip sharedCastleShip;

    [SerializeField]
    private SharedTransform steerTarget;

    private DetectableObject detectableObject;
    private DetectableObject SteerTargetDetectableObject
    {
        get
        {
            if(detectableObject == null)
            {
                steerTarget.Value.gameObject.GetComponent<DetectableObject>();
            }
            if (detectableObject == null)
            {
                detectableObject = steerTarget.Value.gameObject.AddComponent<DetectableObject>();
            }
            return detectableObject;
        }
    }

    private float _speed;
    private float MaxSpeed = 8;
    /// <summary>
    /// Array of steering behaviors
    /// </summary>
    public Steering[] Steerings { get; private set; }

    /// <summary>
    /// Array of steering post-processor behaviors
    /// </summary>
    public Steering[] SteeringPostprocessors { get; private set; }

    SteerForPursuit steerForPursuit;
    SteerForPursuit SteerForPursuit
    {
        get
        {
            if(steerForPursuit == null)
            {
                steerForPursuit = sharedCastleShip.Value.gameObject.GetComponent<SteerForPursuit>();
            }
            if (steerForPursuit == null)
            {
                steerForPursuit = sharedCastleShip.Value.gameObject.AddComponent<SteerForPursuit>();
            }
            return steerForPursuit;
        }
    }

    public override void OnStart()
    {
        SteerForPursuit.Quarry = SteerTargetDetectableObject;

        var allSteerings = sharedCastleShip.Value.GetComponents<Steering>();
        Steerings = allSteerings.Where(x => !x.IsPostProcess).ToArray();
        SteeringPostprocessors = allSteerings.Where(x => x.IsPostProcess).ToArray();
    }

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

        if(distance > 7)
        {
            sharedCastleShip.Value.SetCurrentThrust(speed * sharedCastleShip.Value.forwardAcceleration);
        }
        else if(distance < 5)
        {
            sharedCastleShip.Value.SetCurrentThrust(-1 * sharedCastleShip.Value.backwardAcceleration);
        }
        else
        {
            sharedCastleShip.Value.SetCurrentThrust(0);
        }
    }

    void Seek(Vector3 direction)
    {
        Vector3 targetPosition = sharedCastleShip.Value.transform.position + direction;
        Vector3 castleShipPosition = sharedCastleShip.Value.transform.position;
        targetPosition.y = castleShipPosition.y;

        Vector3 targetDir = targetPosition - castleShipPosition;
        Vector3 forward = sharedCastleShip.Value.transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        int sign = 0;
        if (angle < 0)
        {
            sign = -1;
        }
        else if (angle > 0)
        {
            sign = 1;
        }

        sharedCastleShip.Value.SetCurrentTurn(-sign);

        //float distance = Vector3.Distance(targetPosition, castleShipPosition);

        //Figure out whether going backwards is a better idea

        float speed = direction.magnitude;
        speed = Mathf.Clamp(speed, -1, 1);
        //speed = Mathf.Lerp(0, 1, distance / 3);

        sharedCastleShip.Value.SetCurrentThrust(speed * sharedCastleShip.Value.forwardAcceleration);
    }

    void SeekWithSteering()
    {
        //float sideThrust = 0;//playerRef.GetAxis("SideThrust");
        //new Vector3(playerRef.GetAxis("MoveHorizontal"), 0, playerRef.GetAxis("MoveVertical"));

        //Turning
        //Thrust

        //Steering
        Vector3 force = Vector3.zero;
        UnityEngine.Profiling.Profiler.BeginSample("Adding up basic steerings");
        for (var i = 0; i < Steerings.Length; i++)
        {
            var s = Steerings[i];
            if (s.enabled)
            {
                force += s.WeighedForce;
            }
        }
        UnityEngine.Profiling.Profiler.EndSample();
        var targetDirection = Vector3.ClampMagnitude(force / sharedCastleShip.Value.RigidbodyRef.mass, 1);

        //TODO: Fix NaN NaN NaN Postprocessor bug
        //Post processing steering
        Vector3 adjustedVelocity = Vector3.zero;
        //UnityEngine.Profiling.Profiler.BeginSample("Adding up post-processing steerings");
        //for (var i = 0; i < SteeringPostprocessors.Length; i++)
        //{
        //    var s = SteeringPostprocessors[i];
        //    if (s.enabled)
        //    {
        //        adjustedVelocity += s.WeighedForce;
        //    }
        //}
        //UnityEngine.Profiling.Profiler.EndSample();

        if (adjustedVelocity != Vector3.zero)
        {
            adjustedVelocity = Vector3.ClampMagnitude(adjustedVelocity, 1);
            TraceDisplacement(adjustedVelocity, Color.cyan);
            TraceDisplacement(targetDirection, Color.white);
            targetDirection = adjustedVelocity;
        }

        Seek(targetDirection * 2);
        //TraceDisplacement(targetDirection*2, Color.red);

        //float TargetSpeed = targetDirection.magnitude;
        //Vector3 orientationVelocity = Mathf.Approximately(_speed, 0) ? sharedCastleShip.Value.transform.forward : targetDirection / TargetSpeed;
    }

    Vector3 CalculatePositionDelta(float TargetSpeed, Vector3 orientationVelocity)
    {
        /*
     * Notice that we clamp the target speed and not the speed itself, 
     * because the vehicle's maximum speed might just have been lowered
     * and we don't want its actual speed to suddenly drop.
     */
        var targetSpeed = Mathf.Clamp(TargetSpeed, 0, MaxSpeed);
        if (Mathf.Approximately(_speed, targetSpeed))
        {
            _speed = targetSpeed;
        }
        else
        {
            var rate = TargetSpeed > _speed ? sharedCastleShip.Value.forwardAcceleration : sharedCastleShip.Value.forwardAcceleration;
            _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * rate);
        }

        return sharedCastleShip.Value.transform.forward * _speed * Time.deltaTime;
    }

    private void TraceDisplacement(Vector3 delta, Color color)
    {
        Debug.DrawLine(sharedCastleShip.Value.transform.position, sharedCastleShip.Value.transform.position + delta, color);
    }

    public override TaskStatus OnUpdate()
    {
        if (steerTarget.Value == null)
        {
            SteerForPursuit.Quarry = null;
            sharedCastleShip.Value.SetCurrentThrust(0);
            sharedCastleShip.Value.SetCurrentTurn(0);
            return TaskStatus.Failure;
        }
        SeekWithSteering();
        //Seek();
        return TaskStatus.Running;
    }
}