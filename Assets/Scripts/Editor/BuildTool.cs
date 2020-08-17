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
		
		[MenuItem("Build/Web GL")]
		static void Build_Folder()
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
			}
		}
		
		public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target) {
			foreach (DirectoryInfo dir in source.GetDirectories())
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
		}
	}
}