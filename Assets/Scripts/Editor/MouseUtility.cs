// using System.Collections;
// using System.Threading;
// using Unity.EditorCoroutines.Editor;
// using UnityEditor;
// using UnityEngine;
//
// namespace GameMode
// {
// 	[InitializeOnLoad]
// 	public class MouseUtility
// 	{
// 		static MouseUtility()
// 		{
// 			// Register();
// 		}
// 		
// 		static void callback()
// 		{
// 			var e = Event.current;
// 			if (e != null && e.keyCode != KeyCode.None)
// 			{
// 				if (e.keyCode == KeyCode.Mouse4)
// 				{
// 					Debug.Log("Mouse Button 4");
// 				}
// 				
// 				Debug.Log("Something ");
// 			}
// 		}
//
// 		[InitializeOnLoadMethod]
// 		static void Register()
// 		{
// 			// EditorApplication.update += callback;
// 			Debug.Log("Registered");
//
// 			// SceneView.duringSceneGui += a =>
// 			// {
// 			// 	callback();
// 			// };
//
// 			EditorCoroutineUtility.StartCoroutineOwnerless(loop());
// 		}
//
// 		static IEnumerator loop()
// 		{
// 			while (true)
// 			{
// 				callback();
// 				yield return null;
// 			}
//
// 			yield break;
// 		}
// 	}
// }