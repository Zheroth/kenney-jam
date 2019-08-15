using UnityEngine;
using BehaviorDesigner.Runtime;

[System.Serializable]
public class SharedCastleShip : SharedVariable<CastleShip>
{
	public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
	public static implicit operator SharedCastleShip(CastleShip value) { return new SharedCastleShip { mValue = value }; }
}
