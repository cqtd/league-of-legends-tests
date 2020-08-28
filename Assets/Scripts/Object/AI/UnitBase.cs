using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
	public bool IsIdentified { get; private set; }

	public string UnitName { get; protected set; }
	public int Index { get; protected set; }
	public long ID { get; protected set; }
	public int Team { get; protected set; }
	public bool Visibility { get; protected set; }
	public Vector3 Position { get { return transform.position; } }
	
	public virtual float BoundingRadius { get; protected set; }

	public virtual bool SetIdentification(long id)
	{
		if (IsIdentified) return false;
		
		this.ID = id;
		this.IsIdentified = true;
		
		return true;
	}
}