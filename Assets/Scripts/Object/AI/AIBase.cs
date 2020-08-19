using System;
using UnityEngine;

public abstract class AIBase : AttackableUnit
{
	public virtual void IssueOrder(GameObjectOrder order, Vector3 position, AttackableUnit target, bool isAttackMove, bool isMinion)
	{
		switch (order)
		{
			case GameObjectOrder.HoldPosition:
				break;
			case GameObjectOrder.MoveTo:
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
}