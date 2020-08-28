using UnityEngine;

[RequireComponent(typeof(ControllerBase))]
public class ControllerDebugger : DebuggerBase
{
#if UNITY_EDITOR || ENABLE_VISUAL_DEBUGGER
	[Header("Controller")]
	public float debugOffset = 0.01f;

	ControllerBase controller;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		
		controller = GetComponent<ControllerBase>();
	}

	protected override void OnUpdateInstancePosition()
	{
		base.OnUpdateInstancePosition();
		
		instance.transform.position = controller.Destination + Vector3.up * debugOffset;
	}
#endif
}