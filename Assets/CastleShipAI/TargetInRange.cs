using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CastleShip/")]
[TaskDescription("Finds a seek target")]
public class TargetInRange : Conditional
{
    [SerializeField]
    private SharedCastleShip castleShip;
    [SerializeField]
    private SharedTransform target;
    [SerializeField]
    private SharedFloat range;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) { return TaskStatus.Failure; }

        float distance = Vector3.Distance(this.castleShip.Value.transform.position, this.target.Value.position);

        if(distance < range.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
