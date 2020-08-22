using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class LauncherTool
	{
		[MenuItem("Run/Standalone x64")]
		static void RunLatestStandAlone()
		{
			string path = $"{Application.dataPath}/win64/bin/League of Legends.exe".Replace("/Assets", "");
			
			Process launcher = new Process {StartInfo = {FileName = path}};
			launcher.Start();
		}
	}
}