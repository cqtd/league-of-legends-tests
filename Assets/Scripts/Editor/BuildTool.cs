using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
	public class BuildTool
	{
		static readonly string projectPath = Application.dataPath.Replace("/Assets", "");
		
		static readonly string date = DateTime.Now.ToString("yyyy-MM-dd");
		static readonly string time = DateTime.Now.ToString("hh:mm:ss tt");

		// for release
		static readonly string deployRootPath = projectPath + "/Deploy";
		static readonly string deployPlatformPath = deployRootPath + "/{0}";
		
		// for initial build
		static readonly string containerRootPath = projectPath + "/Container";
		static readonly string containerPlatformPath = containerRootPath + "/{0}/{1}";
		
		// for archive
		static readonly string archiveRootPath = projectPath + "/Build";
		static readonly string archivePlatformPath = archiveRootPath + "/{0}/{1}";

		#region Build/Platform

		[MenuItem("Build/Platform/All Platforms", false, 500)]
		static void Build_All()
		{
			if (!DisplayConfirmMessage("모든 플랫폼")) return;
			
			UpdateBuildInfo();
			
			bool win64 = Build_Internal(BuildTarget.StandaloneWindows64);;
			bool webgl = Build_Internal(BuildTarget.WebGL);

			if (webgl && win64)
				VersionManager.IncreaseBuild();
		}
		
		[MenuItem("Build/Platform/Web GL", false, 530)]
		static void Build_WebGL()
		{
			if (!DisplayConfirmMessage("WebGl")) return;
			Build_Internal(BuildTarget.WebGL, true);
		}
		
		[MenuItem("Build/Platform/Standalone Windows x64", false, 531)]
		static void Build_Standalone_Win()
		{
			if (!DisplayConfirmMessage("Windows x64")) return;
			Build_Internal(BuildTarget.StandaloneWindows64, true);
		}
		
		
		[MenuItem("Build/Create Build Information")]
		static void Menu_UpdateBuildInfo()
		{
			UpdateBuildInfo();
		}

		#endregion
		
		class Logger
		{
			BuildTarget target;
			bool enable;
			
			public Logger(BuildTarget target, bool enable)
			{
				this.target = target;
				this.enable = enable;
			}

			public void Verbose(string msg)
			{
				if (!enable) return;
				
				Debug.Log($"[{target}] {msg}");
			}
			
			public void Success(string msg)
			{
				if (!enable) return;
				
				Debug.Log($"<color=green>[{target}] {msg}</color>");
			}

			public void Warn(string msg)
			{
				if (!enable) return;
				
				Debug.Log($"<color=yellow>[{target}] {msg}</color>");
			}

			public void Fatal(string msg)
			{
				if (!enable) return;
				
				Debug.Log($"<color=red>[{target}] {msg}</color>");
			}
		}
		
		static bool Build_Internal(BuildTarget target, bool increaseVersion = false, bool logger = true)
		{
			Logger log = new Logger(target, logger);
			
			// 빌드 후 아카이브 될 경로
			string archiveBuildPath = string.Format(archivePlatformPath, VersionManager.GetCurrentBuildVersion(), target.ToString());
			DirectoryInfo archiveDir = new DirectoryInfo(archiveBuildPath);
			if (!archiveDir.Exists) archiveDir.Create();
			log.Verbose($"아카이브 경로 : {archiveDir.FullName}");

			// 배포 경로
			string deployBuildPath = string.Format(deployPlatformPath, target.ToString());
			DirectoryInfo deployDir = new DirectoryInfo(deployBuildPath);
			if (!deployDir.Exists) deployDir.Create();
			log.Verbose($"배포 경로 : {deployDir.FullName}");
			
			// 실제 빌드 될 경로
			string folderDir;
			string playerDir;
			
			switch (target)
			{
				case BuildTarget.StandaloneWindows64:
					folderDir = string.Format(containerPlatformPath, target.ToString(), "League of Legends");
					playerDir = folderDir + "/League of Legends.exe";
					break;

				case BuildTarget.WebGL:
					folderDir = string.Format(containerPlatformPath, target.ToString(), "League of Legends");
					playerDir = folderDir;
					break;
				
				case BuildTarget.iOS:
				case BuildTarget.Android:
				case BuildTarget.StandaloneOSX:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.WSAPlayer:
				case BuildTarget.StandaloneLinux64:
				case BuildTarget.PS4:
				case BuildTarget.XboxOne:
				case BuildTarget.tvOS:
				case BuildTarget.Switch:
				case BuildTarget.Lumin:
				case BuildTarget.Stadia:
				case BuildTarget.NoTarget:
					log.Fatal("이 빌드 플랫폼은 사용할 수 없습니다.");
					return false;
				
				default:
					log.Fatal("알 수 없는 플랫폼");
					return false;
			}

			DirectoryInfo unityPlayerDir = new DirectoryInfo(playerDir);
			DirectoryInfo unityFolderDir = new DirectoryInfo(folderDir);

			if (!unityFolderDir.Exists) unityFolderDir.Create();
			log.Verbose($"빌드 폴더 경로 : {unityFolderDir.FullName}");
			log.Verbose($"빌드 플레이어 경로 : {unityPlayerDir.FullName}");

			// 빌드 옵션
			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = target,
				locationPathName = unityPlayerDir.FullName,
				extraScriptingDefines = new[] {"UNITY_POST_PROCESSING_STACK_V2", "ENABLE_VISUAL_DEBUGGER"},
			};

			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 아카이브 경로로 카피
				CopyFilesRecursively(unityFolderDir, archiveDir);
				
				// 배포 경로로 카피
				CopyFilesRecursively(
					unityFolderDir,
					deployDir,
					"BackUpThisFolder_ButDontShipItWithYourGame"
					);
				
				log.Success($"<color=yellow>[{options.target}] Build Complete.</color>");
				
				if (increaseVersion) VersionManager.IncreaseBuild();
				return true;
			}

			return false;
		}

		#region Utility

		static bool DisplayConfirmMessage(string msg)
		{
			return EditorUtility.DisplayDialog("확인", $"{msg}\n빌드를 진행하시겠습니까?", "빌드", "취소");
		}
		
		static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, string blacklist = null) {
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!string.IsNullOrEmpty(blacklist) && dir.FullName.Contains(blacklist))
					continue;
				
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			}
			
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
		}
		
		static void UpdateBuildInfo()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.AppendLine("public class BuildInfo {");
			sb.AppendLine("\tpublic const string buildVersion = " + $"\"0x{PlayerSettings.Android.bundleVersionCode:X}\";");
			sb.AppendLine("\tpublic const string buildDate = " + $"\"{date}\";");
			sb.AppendLine("\tpublic const string buildTime = " + $"\"{time}\";");
			sb.AppendLine("}");
			
			File.WriteAllText("Assets/BuildInfo.cs", sb.ToString());
			AssetDatabase.Refresh();
		}
		
		#endregion

	}
}