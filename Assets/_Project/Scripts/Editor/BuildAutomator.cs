using UnityEngine;
using UnityEditor;

namespace BlockForge.Editor
{
    public class BuildAutomator
    {
        [MenuItem("BlockForge/Build/Setup Android")]
        public static void SetupAndroid()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.applicationIdentifier = "com.Musab.AntiGravityIdleTD";
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.productName = "AntiGravity Idle TD";
            PlayerSettings.companyName = "Musab";
            
            // Orientation
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            
            Debug.Log("[BuildAutomator] Android settings configured.");
        }
        
        [MenuItem("BlockForge/Build/Build Android APK")]
        public static void BuildAndroid()
        {
            SetupAndroid();
            
            string[] scenes = new string[]
            {
                "Assets/_Project/Scenes/Boot.unity",
                "Assets/_Project/Scenes/Home.unity",
                "Assets/_Project/Scenes/Run.unity"
            };
            
            string buildPath = "Builds/Android/AntiGravityIdleTD.apk";
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = buildPath;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            
            Debug.Log($"[BuildAutomator] Build started/finished at {buildPath}");
        }
    }
}