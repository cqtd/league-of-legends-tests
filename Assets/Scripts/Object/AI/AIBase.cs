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
			case GameObjectOrder.Stop:
				controller.HoldPosition();
				break;
			case GameObjectOrder.MoveTo:
				controller.UpdateDestination(position);
				
				break;
			case GameObjectOrder.AttackUnit:
				break;
			case GameObjectOrder.AutoAttackPet:
				if (Pet == null)
				{
					
					break;
				}
				break;
			case GameObjectOrder.AutoAttack:
				break;
			case GameObjectOrder.MovePet:
				if (Pet == null)
				{
					
					break;
				}
				break;
			case GameObjectOrder.AttackTo:
				break;
			
			default:
				throw new ArgumentOutOfRangeException(nameof(order), order, null);
		}
	}

	protected abstract void MoveTo(Vector3 pos);

	public AIBase Pet { get; protected set; }
}