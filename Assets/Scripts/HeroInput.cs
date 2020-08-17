using System;
using System.Collections.Generic;
using UnityEngine;

public enum ETriggerType
{
	DOWN = 0,
	UP = 1,
	STAY = 2,
}

public class HeroInput : MonoBehaviour
{
	readonly Dictionary<KeyCode, Action> downActions = new Dictionary<KeyCode, Action>();
	readonly Dictionary<KeyCode, Action> upActions = new Dictionary<KeyCode, Action>();
	readonly Dictionary<KeyCode, Action> stayActions = new Dictionary<KeyCode, Action>();
	

	void Update()
	{
		foreach (KeyValuePair<KeyCode,Action> pair in downActions)
		{
			if (Input.GetKeyDown(pair.Key))
			{
				pair.Value.Invoke();
			}
		}
		
		foreach (KeyValuePair<KeyCode,Action> pair in upActions)
		{
			if (Input.GetKeyDown(pair.Key))
			{
				pair.Value.Invoke();
			}
		}
		
		foreach (KeyValuePair<KeyCode,Action> pair in stayActions)
		{
			if (Input.GetKeyDown(pair.Key))
			{
				pair.Value.Invoke();
			}
		}
	}

	public bool GetKeyDown(KeyCode vKey)
	{
		return Input.GetKeyDown(vKey);
	}

	public bool GetKeyUp(KeyCode vKey)
	{
		return Input.GetKeyUp(vKey);
	}

	public bool GetKey(KeyCode vKey)
	{
		return Input.GetKey(vKey);
	}

	public void AddBindings(KeyCode vKey, ETriggerType triggerType, Action action)
	{
		Action _action;
		
		switch (triggerType)
		{
			case ETriggerType.DOWN:
				if (!downActions.TryGetValue(vKey, out _action))
				{
					downActions[vKey] = action;
					return;
				}

				_action += action;
				downActions[vKey] = _action;		
				break;
			
			case ETriggerType.UP:
				if (!upActions.TryGetValue(vKey, out _action))
				{
					upActions[vKey] = action;
					return;
				}
				
				_action += action;
				upActions[vKey] += _action;
				break;
			
			case ETriggerType.STAY:
				if (!stayActions.TryGetValue(vKey, out _action))
				{
					stayActions[vKey] = action;
					return;
				}
				
				_action += action;
				stayActions[vKey] += _action;
				break;
			
			default:
				break;
		}
	}
}