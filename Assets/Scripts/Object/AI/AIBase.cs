using System;
using UnityEngine;

public abstract class AIBase : AttackableUnit
{
	[SerializeField] protected ControllerBase controller = default;
	
	public float ReduceCooldown { get; protected set; }
	public float MaxReduceCooldown  { get; protected set; }
	public bool Lethality  { get; protected set; }
	
	public float CriticalPossibility  { get; protected set; }
	public float CriticalMultiplier  { get; protected set; }
	
	public virtual float AttackRange  { get; protected set; }
	
	public float Gold  { get; protected set; }
	public float TotalGold  { get; protected set; }
	
	public int EvolvePoint  { get; protected set; }
	public float ExperienceRadisu  { get; protected set; }
	
	public int CombatType  { get; protected set; }
	
	
	public void IssueOrder(GameObjectOrder order, Vector3 position, AttackableUnit target, bool isAttackMove, bool isMinion)
	{
		switch (order)
		{
			case GameObjectOrder.HoldPosition:
				controller.HoldPosition();
				break;
			case GameObjectOrder.MoveTo:
				controller.UpdateDestination(position);
				// MoveTo(position);
				
				break;
			case GameObjectOrder.AttackUnit:
				break;
			case GameObjectOrder.AutoAttackPet:
				break;
			case GameObjectOrder.AutoAttack:
				break;
			case GameObjectOrder.MovePet:
				break;
			case GameObjectOrder.AttackTo:
				break;
			case GameObjectOrder.Stop:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(order), order, null);
		}
	}

	protected abstract void MoveTo(Vector3 pos);
}