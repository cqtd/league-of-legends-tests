using UnityEngine;

public class AIMinion : AIBase
{
	public int CampIndex { get; protected set; }
	public int MinionLevel  { get; protected set; }
	public Vector3 LeashedPosition  { get; protected set; }
}