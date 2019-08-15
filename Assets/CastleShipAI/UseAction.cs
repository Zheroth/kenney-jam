using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("CastleShip/")]
[TaskDescription("Finds a seek target")]
public class UseAction : Action
{
    [SerializeField]
    private SharedCastleShip castleShip;
    private enum ShipAction { A, B, C, D }
    [SerializeField]
    private ShipAction shipAction = ShipAction.A;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if(shipAction == ShipAction.A)
        {
            castleShip.Value.FireActionA();
        }
        else if(shipAction == ShipAction.B)
        {
            castleShip.Value.FireActionB();
        }
        else if (shipAction == ShipAction.C)
        {
            castleShip.Value.FireActionC();
        }
        else if (shipAction == ShipAction.D)
        {
            castleShip.Value.FireActionD();
        }
        return TaskStatus.Success;
    }
}