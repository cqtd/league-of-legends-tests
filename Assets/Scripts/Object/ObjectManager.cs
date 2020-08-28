using System;
using System.Collections.Generic;
using UnityEngine;
using Identify;
using Object = UnityEngine.Object;

[DefaultExecutionOrder(-3000)]
public class ObjectManager : MonoBehaviour
{
	static ObjectManager _inst;
	public static ObjectManager Instance
	{
		get { return _inst; }
	}

	void Awake()
	{
		_inst = this;
		DontDestroyOnLoad(this.gameObject);
	}

	[NonSerialized]
	public AIHeroClient localPlayer = default;

	public static AIHeroClient GetPlayer()
	{
		return _inst.localPlayer;
	}

	public static bool SetPlayer(AIHeroClient player)
	{
		if (_inst.localPlayer != null) return false;
		
		_inst.localPlayer = player;
		return true;
	}
	
	readonly List<UnitBase> units = new List<UnitBase>();

	public static void OnObjectCreated(UnitBase unit)
	{
		_inst.units.Add(unit);
	}

	public static void OnObjectDestroyed(UnitBase unit)
	{
		_inst.units.Remove(unit);
	}

	public static void ValidateList()
	{
		int current = 0;
		while (current <= _inst.units.Count)
		{
			if (_inst.units[current] == null)
			{
				_inst.units.RemoveAt(current);
			}
			else
			{
				current++;
			}
		}
	}

	public static T GetObjectByIndex<T>(int index) where T : UnitBase
	{
		if (_inst.units.Count <= index) return default;
		if (!(_inst.units[index] is T casted)) return default;
		
		return casted;
	}
}

static class SpawnManager
{
	const string resourcePath = "Hero/Hero_{0}.prefab";
	
	public static AIHeroClient CreateHero(string name)
	{
		AIHeroClient resource = Resources.Load<AIHeroClient>(string.Format(resourcePath, name));

		return CreateHero(resource);
	}
	
	public static AIHeroClient CreateHero(AIHeroClient prefab)
	{
		AIHeroClient instance = Object.Instantiate<AIHeroClient>(prefab, null);
		
		instance.SetIdentification(ID.Generate());
		ObjectManager.OnObjectCreated(instance);

		return instance;
	}
}