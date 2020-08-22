using System;
using System.Collections.Generic;
using UnityEngine;

public enum ETriggerType
{
	DOWN = 0,
	UP = 1,
	STAY = 2,
}

public enum EMouseButton
{
	Left = 0,
	Right = 1,
	Middle = 2,
}

public class InputHandler : MonoBehaviour
{
	static InputHandler instance;
	
	readonly Dictionary<KeyCode, Action> downActions = new Dictionary<KeyCode, Action>();
	readonly Dictionary<KeyCode, Action> upActions = new Dictionary<KeyCode, Action>();
	readonly Dictionary<KeyCode, Action> stayActions = new Dictionary<KeyCode, Action>();
	
	readonly Dictionary<EMouseButton, Action> downMouse = new Dictionary<EMouseButton, Action>();
	readonly Dictionary<EMouseButton, Action> upMouse = new Dictionary<EMouseButton, Action>();
	readonly Dictionary<EMouseButton, Action> stayMouse = new Dictionary<EMouseButton, Action>();

	void Awake()
	{
		instance = this;
	}

	void OnDestroy()
	{
		instance = null;
	}

	void OnApplicationQuit()
	{
		instance = null;
	}

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
			if (Input.GetKeyUp(pair.Key))
			{
				pair.Value.Invoke();
			}
		}
		
		foreach (KeyValuePair<KeyCode,Action> pair in stayActions)
		{
			if (Input.GetKey(pair.Key))
			{
				pair.Value.Invoke();
			}
		}

		foreach (KeyValuePair<EMouseButton,Action> pair in downMouse)
		{
			if (Input.GetMouseButtonDown((int) pair.Key))
			{
				pair.Value.Invoke();
			}
		}
		
		foreach (KeyValuePair<EMouseButton,Action> pair in upMouse)
		{
			if (Input.GetMouseButtonUp((int) pair.Key))
			{
				pair.Value.Invoke();
			}
		}
		
		foreach (KeyValuePair<EMouseButton,Action> pair in stayMouse)
		{
			if (Input.GetMouseButton((int) pair.Key))
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

	void AddBindings_Internal(KeyCode vKey, ETriggerType triggerType, Action action)
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

	public static void AddBindings(KeyCode vKey, ETriggerType triggerType, Action action)
	{
		instance.AddBindings_Internal(vKey, triggerType, action);
	}

	void AddBindings_Internal(EMouseButton vMouse, ETriggerType triggerType, Action action)
	{
		Action _action;
		
		switch (triggerType)
		{
			case ETriggerType.DOWN:
				if (!downMouse.TryGetValue(vMouse, out _action))
				{
					downMouse[vMouse] = action;
					return;
				}

				_action += action;
				downMouse[vMouse] = _action;		
				break;
			
			case ETriggerType.UP:
				if (!upMouse.TryGetValue(vMouse, out _action))
				{
					upMouse[vMouse] = action;
					return;
				}
				
				_action += action;
				upMouse[vMouse] += _action;
				break;
			
			case ETriggerType.STAY:
				if (!stayMouse.TryGetValue(vMouse, out _action))
				{
					stayMouse[vMouse] = action;
					return;
				}
				
				_action += action;
				stayMouse[vMouse] += _action;
				break;
			
			default:
				break;
		}
	}
	
	public static void AddBindings(EMouseButton vMouse, ETriggerType triggerType, Action action)
	{
		instance.AddBindings_Internal(vMouse, triggerType, action);
	}
}