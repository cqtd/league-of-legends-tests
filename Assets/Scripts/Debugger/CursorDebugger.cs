using UnityEngine;

[RequireComponent(typeof(CursorUtility))]
class CursorDebugger : DebuggerBase
{
#if UNITY_EDITOR || ENABLE_VISUAL_DEBUGGER
	CursorUtility utility;
	
	protected override void OnInitialized()
	{
		base.OnInitialized();
		
		utility = GetComponent<CursorUtility>();
		utility.onRaycastSucess.AddListener(OnRaycast);
	}

	void OnRaycast(Vector3 pos)
	{
		instance.transform.position = pos;
	}

	protected override EInitTiming InitTiming {
		get { return EInitTiming.Start; }
	}

	protected override EUpdateTiming UpdateTiming {
		get { return EUpdateTiming.None; }
	}

	void Reset()
	{
		updateTiming = EUpdateTiming.None;
		initialTiming = EInitTiming.None;
	}
#endif
}