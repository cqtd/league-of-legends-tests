using UnityEngine;

[DefaultExecutionOrder(-500)]
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

	[SerializeField] AIHeroClient localPlayer = default;

	public static AIHeroClient GetPlayer()
	{
		return _inst.localPlayer;
	}
}
