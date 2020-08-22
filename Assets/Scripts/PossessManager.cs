using UnityEngine;

namespace GameMode
{
	[DefaultExecutionOrder(1200)]
	public class PossessManager : MonoBehaviour
	{
		void Awake()
		{
			AIHeroClient localPlayer = ObjectManager.GetPlayer();
		}
	}
}