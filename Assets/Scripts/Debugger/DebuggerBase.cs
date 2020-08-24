using System;
using UnityEngine;

[Serializable]
public enum EUpdateTiming
{
	Update,
	LateUpdate,
	FixedUpdate,
	
	None,
}

[Serializable]
public enum EInitTiming
{
	Awake,
	Start,
	OnEnable,
	
	None,
}

public abstract class DebuggerBase : MonoBehaviour
{
	public bool debugger = default;
	public GameObject instance = default;
	
	[Header("Configs")]
	public EUpdateTiming updateTiming = EUpdateTiming.Update;
	public EInitTiming initialTiming = EInitTiming.Awake;
	
	protected virtual EUpdateTiming UpdateTiming {
		get { return this.updateTiming; }
	}
	
	protected virtual EInitTiming InitTiming {
		get { return this.initialTiming; }
	}

	bool initialized = false;
	
	void Awake()
	{
		if (!initialized && InitTiming == EInitTiming.Awake)
			OnInitialized();
	}

	void Start()
	{
		if (!initialized && InitTiming == EInitTiming.Start)
			OnInitialized();
	}

	void OnEnable()
	{
		if (!initialized && InitTiming == EInitTiming.OnEnable)
			OnInitialized();
	}

	void Update()
	{
		if (!debugger) return;
		if (UpdateTiming == EUpdateTiming.Update)
			OnUpdateInstancePosition();
	}

	void FixedUpdate()
	{
		ValidateEnable();
		OnFixedUpdate();
		
		if (!debugger) return;
		if (UpdateTiming == EUpdateTiming.FixedUpdate)
			OnUpdateInstancePosition();
	}

	void LateUpdate()
	{
		if (!debugger) return;
		if (UpdateTiming == EUpdateTiming.LateUpdate)
			OnUpdateInstancePosition();
	}

	protected virtual void OnInitialized()
	{
		initialized = true;
	}

	protected void ValidateEnable()
	{
		if (debugger)
		{
			if (!instance.gameObject.activeSelf)
				instance.gameObject.SetActive(true);
		}
		else
		{
			if (instance.gameObject.activeSelf)
				instance.gameObject.SetActive(false);
		}
	}

	protected virtual void OnFixedUpdate()
	{
		
	}

	protected virtual void OnUpdateInstancePosition()
	{
		
	}
}