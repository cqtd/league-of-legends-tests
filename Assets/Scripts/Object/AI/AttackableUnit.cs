public abstract class AttackableUnit : UnitBase
{
	public float Health { get; protected set; }
	public float MaxHealth { get; protected set; }
	
	public float Mana { get; protected set; }
	public float MaxMana { get; protected set; }
	
	public float BaseAttack { get; protected set; }
	public float BonusAttack { get; protected set; }
	
	public float AbilityPower { get; protected set; }
	public float BonusAbilityPower { get; protected set; }
	public float AbilityPowerMultiplier { get; protected set; }

	public float BaseArmor { get; protected set; }
	public float BonusArmor { get; protected set; }
	
	public float BaseMagicRegist { get; protected set; }
	public float BonuMagicResist { get; protected set; }
	
	public int Level { get; protected set; }
	public bool HasLevelUpPoint  { get; protected set; }
	
	public uint Experience { get; protected set; }
	
	public float MovemenetSpeed { get; protected set; }
	public float MaxMovementSpeed { get; protected set; }

	protected virtual void Initialize()
	{
		
	}
}