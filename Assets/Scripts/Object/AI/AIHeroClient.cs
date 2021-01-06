using UnityEngine;
using UnityEngine.Assertions;

public class AIHeroClient : AIBase
{
	public HeroSpellbook spellbook;

	public string ChampionName  { get; protected set; }
	public int NeutralMinionKillCount  { get; protected set; }

	private GameObject attackRangeRenderer;
	private GameObject boudingRadiusRenderer;

	[Header("Temp Section")] 
	
	[SerializeField]
	private float attackRange = default;
	[SerializeField] private float boundingRadius = default;

	[Header("Color")] 
	
	[SerializeField]
	private Color rangeColor = default;
	[SerializeField] private Color boundColor = default;

	public override float AttackRange {
		get { return attackRange; }
	}

	public override float BoundingRadius {
		get { return boundingRadius; }
	}

	public HeroController GetController()
	{
		#if UNITY_EDITOR
		Assert.IsTrue(this.controller is HeroController);
		#endif
		
		return this.controller as HeroController;
	}

	private Circle attackRangeCircle;
	private Circle boundingRadiusCircle;

	private void Awake()
	{
		Circle.Builder builder;
		
		builder = new Circle.Builder($"{gameObject.name}_AttackRange");
		attackRangeCircle = builder.SetColor(rangeColor)
			.SetTarget(transform)
			.SetIntensity(2)
			.SetSize(BoundingRadius + AttackRange)
			.Build();
		
		builder = new Circle.Builder($"{gameObject.name}_BoundingRadius");
		boundingRadiusCircle = builder.SetColor(boundColor)
			.SetTarget(transform)
			.SetIntensity(1)
			.SetSize(BoundingRadius)
			.Build();
	}

	private void BeginDrawAttackRange()
	{
		attackRangeCircle.BeginDraw();
	}

	private void EndDrawAttackRange()
	{
		attackRangeCircle.EndDraw();
	}

	private void BeginDrawBoundingRadius()
	{
		boundingRadiusCircle.BeginDraw();
	}

	private void EndDrawBoundingRadius()
	{
		boundingRadiusCircle.EndDraw();
	}

	protected override void MoveTo(Vector3 pos)
	{
		
	}

	public void Possess()
	{
		InputHandler.AddBindings(KeyCode.Space, ETriggerType.DOWN, BeginDrawBoundingRadius);
		InputHandler.AddBindings(KeyCode.Space, ETriggerType.UP, EndDrawBoundingRadius);

		InputHandler.AddBindings(KeyCode.Space, ETriggerType.DOWN, CameraController.ForceFocusHero);
		InputHandler.AddBindings(KeyCode.Space, ETriggerType.UP, CameraController.UnforceFocusHero);
		
		InputHandler.AddBindings(KeyCode.A, ETriggerType.DOWN, BeginDrawAttackRange);
		InputHandler.AddBindings(KeyCode.X, ETriggerType.DOWN, EndDrawAttackRange);

		// 멈춤
		InputHandler.AddBindings(KeyCode.S, ETriggerType.STAY, () =>
		{
			IssueOrder(GameObjectOrder.HoldPosition, transform.position, null, false, false);
		});

		// 우클릭 유지
		InputHandler.AddBindings(EMouseButton.Right, ETriggerType.STAY, () =>
		{
			Collider underMouseObj = CursorUtility.GetUnderMouseObject();
			if (underMouseObj != null && underMouseObj.TryGetComponent(typeof(AttackableUnit), out var unit))
			{
				IssueOrder(GameObjectOrder.AttackUnit, underMouseObj.transform.position, unit as AttackableUnit, false, unit is AIMinion);
			}
			else
			{
				IssueOrder(GameObjectOrder.MoveTo, CursorUtility.GetMousePosition(), null, false, false);
			}
		});
	}
}