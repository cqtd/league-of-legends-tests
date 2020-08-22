using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class VersionManager
{
    const string menu = "Build/Version/";
    static bool autoIncreaseBuildVersion;
    
    const string autoIncreaseMenuName = menu + "빌드 버전 자동 올리기";
    
    static VersionManager()
    {
        autoIncreaseBuildVersion = EditorPrefs.GetBool(autoIncreaseMenuName, true);
    }

    [MenuItem(autoIncreaseMenuName, false, 1)]
    static void SetAutoIncrease()
    {
        autoIncreaseBuildVersion = !autoIncreaseBuildVersion;
        EditorPrefs.SetBool(autoIncreaseMenuName, autoIncreaseBuildVersion);
        Debug.Log("Auto Increase : " + autoIncreaseBuildVersion);
    }

    [MenuItem(autoIncreaseMenuName, true)]
    static bool SetAutoIncreaseValidate()
    {
        Menu.SetChecked(autoIncreaseMenuName, autoIncreaseBuildVersion);
        return true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    [MenuItem(menu + "현재 버전 확인", false, 2)]
    static void CheckCurrentVersion()
    {
        Debug.Log(PlayerSettings.bundleVersion + 
            " [0x" + PlayerSettings.Android.bundleVersionCode.ToString("X") + "]");
    }

    [MenuItem(menu + "Increase Season Version", false, 50)]
    static void IncreaseSeason()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(1, 0, 0, 0);
    }

    [MenuItem(menu + "Increase Major Version", false, 51)]
    static void IncreaseMajor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(0, 1, 0, 0);
    }

    [MenuItem(menu + "Increase Minor Version", false, 52)]
    static void IncreaseMinor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(0, 0, 1, 0);
    }

    public static void IncreaseBuild()
    {
        EditVersion(0, 0, 0, 1);
    }
    
    static void EditVersion(int season, int majorIncr, int minorIncr, int buildIncr)
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int SeasonVersion = int.Parse(lines[0]) + season;
        int MajorVersion = int.Parse(lines[1]) + majorIncr;
        int MinorVersion = int.Parse(lines[2]) + minorIncr;
        int Build = int.Parse(lines[3]) + buildIncr;

        PlayerSettings.bundleVersion = SeasonVersion.ToString("0") + "." +
                                       MajorVersion.ToString("0") + "." +
                                       MinorVersion.ToString("0") + "." +
                                       Build.ToString("0");
        
        PlayerSettings.Android.bundleVersionCode = 
            SeasonVersion * 100000000 + MajorVersion * 1000000 + MinorVersion * 10000 + Build;
        
        CheckCurrentVersion();
    }
    
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (autoIncreaseBuildVersion) IncreaseBuild();
    }

    public static string GetNextBuildVersion()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int SeasonVersion = int.Parse(lines[0]);
        int MajorVersion = int.Parse(lines[1]);
        int MinorVersion = int.Parse(lines[2]);
        int Build = int.Parse(lines[3]) + 1;

        return SeasonVersion.ToString("0") + "." +
               MajorVersion.ToString("0") + "." +
               MinorVersion.ToString("0") + "." +
               Build.ToString("0");
    }
}