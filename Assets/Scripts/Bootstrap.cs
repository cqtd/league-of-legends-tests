using System;
using UnityEngine;


[DefaultExecutionOrder(-2999)]
public class Bootstrap : MonoBehaviour
{
	[Serializable]
	public struct HeroRule
	{
		public HeroProfile hero;
		public bool localPlayer;
		public int team;
	}

	public HeroRule[] heroes;
	
	void Awake()
	{
		foreach (HeroRule heroRule in heroes)
		{
			AIHeroClient instance = SpawnManager.CreateHero(heroRule.hero.heroPrefab);

			if (heroRule.localPlayer)
			{
				ObjectManager.SetPlayer(instance);
			}
		}
	}
}
