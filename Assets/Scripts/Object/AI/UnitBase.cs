using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
	public string UnitName { get; protected set; }
	public int Index { get; protected set; }
	public int Team { get; protected set; }
	public bool Visibility { get; protected set; }
	public Vector3 Position { get { return transform.position; } }
	
	public virtual float BoundingRadius { get; protected set; }
}