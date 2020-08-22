using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
	public class BuildTool
	{
		const string platform_webgl = "webgl";
		const string platform_standalone_win64 = "win64";

		static void Build(BuildTarget target)
		{
			if (!EditorUtility.DisplayDialog("WebGL", "빌드를 진행하시겠습니까?", "빌드", "취소"))
				return;
			
			var projectPath = Application.dataPath.Replace("/Assets", "");
			var buildPath = projectPath + "/Build";
			
			var date = DateTime.Now.ToString("yyyy-MM-dd");
			var datePath = buildPath + "/" + platform_webgl + "/" + date;
			
			var backupDirectory = new DirectoryInfo(datePath + "/" + VersionManager.GetNextBuildVersion());
			if (!backupDirectory.Exists)
			{
				backupDirectory.Create();
			}

			var storagePath = projectPath + "/League of Legends";
			var buildStorage = new DirectoryInfo(storagePath);
			
			if (!buildStorage.Exists)
				buildStorage.Create();

			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = target,
				locationPathName = buildStorage.FullName
			};

			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 빌드 데이터 백업
				CopyFilesRecursively(buildStorage, backupDirectory);
				
				// 배포를 위한 경로
				CopyFilesRecursively(buildStorage, new DirectoryInfo(projectPath + "/webgl"));
				
				Debug.Log($"<color=yellow>[{options.target}] Build Complete.</color>");
			}
		}
		
		[MenuItem("Build/Web GL", false, 500)]
		static void Build_WebGL()
		{
			if (!EditorUtility.DisplayDialog("WebGL", "빌드를 진행하시겠습니까?", "빌드", "취소"))
				return;
			
			var projectPath = Application.dataPath.Replace("/Assets", "");
			var buildPath = projectPath + "/Build";
			
			var date = DateTime.Now.ToString("yyyy-MM-dd");
			var datePath = buildPath + "/" + platform_webgl + "/" + date;
			
			var backupDirectory = new DirectoryInfo(datePath + "/" + VersionManager.GetNextBuildVersion());
			if (!backupDirectory.Exists)
			{
				backupDirectory.Create();
			}

			var storagePath = projectPath + "/League of Legends";
			var buildStorage = new DirectoryInfo(storagePath);
			
			if (!buildStorage.Exists)
				buildStorage.Create();

			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = BuildTarget.WebGL,
				locationPathName = buildStorage.FullName
			};

			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 빌드 데이터 백업
				CopyFilesRecursively(buildStorage, backupDirectory);
				
				// 배포를 위한 경로
				CopyFilesRecursively(buildStorage, new DirectoryInfo(projectPath + "/webgl"));
				
				Debug.Log($"<color=yellow>[{options.target}] Build Complete.</color>");
			}
		}

		[MenuItem("Build/Standalone Windows x64", false, 501)]
		static void Build_Standalone_Win()
		{
			if (!EditorUtility.DisplayDialog("WebGL", "빌드를 진행하시겠습니까?", "빌드", "취소"))
				return;
			
			var projectPath = Application.dataPath.Replace("/Assets", "");
			var buildPath = projectPath + "/Build";
			
			var date = DateTime.Now.ToString("yyyy-MM-dd");
			var datePath = buildPath + "/" + platform_standalone_win64 + "/" + date;
			
			var backupDirectory = new DirectoryInfo(datePath + "/" + VersionManager.GetNextBuildVersion());
			if (!backupDirectory.Exists)
			{
				backupDirectory.Create();
			}

			var storagePath = projectPath + "/win64/League of Legends";
			var playerPath = storagePath + "/League of Legends.exe";
			var buildStorage = new DirectoryInfo(storagePath);
			
			// if (!buildStorage.Exists)
			// 	buildStorage.Create();

			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = BuildTarget.StandaloneWindows64,
				locationPathName = new FileInfo(playerPath).FullName
			};

			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 빌드 데이터 백업
				CopyFilesRecursively(buildStorage, backupDirectory);
				
				// 배포를 위한 경로
				CopyFilesRecursively(buildStorage, new DirectoryInfo(projectPath + "/win64/bin"), "BackUpThisFolder_ButDontShipItWithYourGame");
				
				Debug.Log($"<color=yellow>[{options.target}] Build Complete.</color>");
			}
		}
		
		public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, string blacklist = null) {
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!string.IsNullOrEmpty(blacklist) && dir.FullName.Contains(blacklist))
					continue;
				
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			}
			
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
		}
	}
}