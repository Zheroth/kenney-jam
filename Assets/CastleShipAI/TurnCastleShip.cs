using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CastleShip/")]
[TaskDescription("Adds turn to the castle ship")]
public class TurnCastleShip : Action
{
    [SerializeField]
    private SharedCastleShip sharedCastleShip;

    [Range(-1,1)]
    [SerializeField]
    private float turnAmount;

    public override void OnStart()
	{
	}

	public override TaskStatus OnUpdate()
	{
        sharedCastleShip.Value.SetCurrentTurn(turnAmount);
		return TaskStatus.Success;
	}
}
