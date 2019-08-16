using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CastleShip/")]
[TaskDescription("Finds a seek target")]
public class FindSeekTarget : Action
{
    [SerializeField]
    private SharedCastleShip castleShip;
    [SerializeField]
    private SharedTransform steerTarget;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        CastleShip[] castleShips = GameObject.FindObjectsOfType<CastleShip>();

        for (int i = 0; i < castleShips.Length; i++)
        {
            if(castleShips[i] != castleShip.Value)
            {
                steerTarget.Value = castleShips[i].transform;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }
}
