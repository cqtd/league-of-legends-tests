using UnityEngine;

public class AIHeroClient : AIBase
{
	public HeroSpellbook spellbook;

	public string ChampionName  { get; protected set; }
	public int NeutralMinionKillCount  { get; protected set; }

	GameObject attackRangeRenderer;
	GameObject boudingRadiusRenderer;

	[Header("Temp Section")] 
	
	[SerializeField] float attackRange = default;
	[SerializeField] float boundingRadius = default;

	[Header("Color")] 
	
	[SerializeField] Color rangeColor = default;
	[SerializeField] Color boundColor = default;

	public override float AttackRange {
		get { return attackRange; }
	}

	public override float BoundingRadius {
		get { return boundingRadius; }
	}

	public HeroController GetController()
	{
		return this.controller as HeroController;
	}

	// void CreateCircle(ref GameObject spawned, Color color, int intensity = 1)
	// {
	// 	if (spawned == null)
	// 	{
	// 		spawned = Instantiate(Resources.Load<GameObject>("DrawCircle"), transform);
	// 		Material mat = Instantiate(Resources.Load<Material>("MAT_DrawCircle"));
	// 		
	// 		mat.SetColor("_Color", color * intensity);
	//
	// 		spawned.GetComponent<Renderer>().sharedMaterial = mat;
	// 	}
	// }

	Circle attackRangeCircle;
	Circle boundingRadiusCircle;

	void Awake()
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

	const float planeOffset = 0.01f;
	void BeginDrawAttackRange()
	{
		attackRangeCircle.BeginDraw();
		
		// CreateCircle(ref attackRangeRenderer, rangeColor, 2);
		//
		// attackRangeRenderer.transform.position = transform.position + Vector3.up * planeOffset ;
		// attackRangeRenderer.transform.localScale = (BoundingRadius + AttackRange) * Vector3.one;
		//
		// attackRangeRenderer.SetActive(true);
	}

	void EndDrawAttackRange()
	{
		attackRangeCircle.EndDraw();
		
		// if (attackRangeRenderer == null) return;
		//
		// attackRangeRenderer.SetActive(false);
	}

	void BeginDrawBoundingRadius()
	{
		boundingRadiusCircle.BeginDraw();
		
		// CreateCircle(ref boudingRadiusRenderer, boundColor);
		//
		// boudingRadiusRenderer.transform.position = transform.position + Vector3.up * planeOffset ;
		// boudingRadiusRenderer.transform.localScale = BoundingRadius * Vector3.one;
		//
		// boudingRadiusRenderer.SetActive(true);
	}

	void EndDrawBoundingRadius()
	{
		boundingRadiusCircle.EndDraw();
		
		// if (boudingRadiusRenderer == null) return;
		//
		// boudingRadiusRenderer.SetActive(false);
	}

	//@TODO :: IssueOrder로 Controller 로직 다 옮기기
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
			var underMouseObj = CursorUtility.GetUnderMouseObject();
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