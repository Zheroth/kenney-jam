using BehaviorDesigner.Runtime;

[System.Serializable]
public class SharedComputerControlled : SharedVariable<ComputerControlled>
{
    public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
    public static implicit operator SharedComputerControlled(ComputerControlled value) { return new SharedComputerControlled { mValue = value }; }
}