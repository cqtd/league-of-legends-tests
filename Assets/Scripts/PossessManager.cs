using UnityEngine;

namespace GameMode
{
	public class PossessManager : MonoBehaviour
	{
		void Awake()
		{
			AIHeroClient localPlayer = ObjectManager.GetPlayer();
		}
	}
}