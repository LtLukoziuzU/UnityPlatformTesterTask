using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Rendering;

public class BuildWindow : EditorWindow
{
    
    bool buildAndroid = true, buildiOS = true;
    bool monoGLES3 = false, monoVulkan = false, il2cppGLES3 = false, il2cppVulkan = false;
    bool low = false, medium = false, high = false;

    [MenuItem("Build/Select what to build")]
    static void Init()
    {
        BuildWindow window = (BuildWindow)EditorWindow.GetWindow(typeof(BuildWindow));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 200;       //the iOS labels didn't fit in the default, so had to increase size

        //toggles for Android options
        buildAndroid = EditorGUILayout.BeginToggleGroup ("Android Builds", buildAndroid);
            monoGLES3 = EditorGUILayout.Toggle("Mono + GLES3", monoGLES3);
            il2cppGLES3 = EditorGUILayout.Toggle("IL2CPP + GLES3", il2cppGLES3);
            monoVulkan = EditorGUILayout.Toggle("Mono + Vulkan", monoVulkan);
            il2cppVulkan = EditorGUILayout.Toggle("IL2CPP + GLES3", il2cppVulkan);
        EditorGUILayout.EndToggleGroup ();

        //toggles for iOS options
        buildiOS = EditorGUILayout.BeginToggleGroup ("iOS Builds", buildiOS);
            low = EditorGUILayout.Toggle("Managed Stripping Level: Low", low);
            medium = EditorGUILayout.Toggle("Managed Stripping Level: Medium", medium);
            high = EditorGUILayout.Toggle("Managed Stripping Level: High", high);
        EditorGUILayout.EndToggleGroup ();

        if (GUILayout.Button("Build"))
        {
            string[] splitDataPath = Application.dataPath.Split('/');       //split dataPath items between each slash
            string name = splitDataPath[splitDataPath.Length - 2];          //grab the name before "Assets"
            string path = "";
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] {"Assets/Scenes/Farm.unity"};
            buildPlayerOptions.options = BuildOptions.None;

            //requirements: Scripting Backend (Mono or IL2CPP), GfxAPI (GLES3.0 or Vulkan)
            //total: 4 builds
            if (buildAndroid) 
            {
                path = Path.GetDirectoryName(Application.dataPath) + "/Builds/Android/";
                buildPlayerOptions.target = BuildTarget.Android;
                
                if (monoGLES3)
                {
                    PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new [] { GraphicsDeviceType.OpenGLES3 });
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                    buildPlayerOptions.locationPathName = path + name + "_Mono_OpenGLES.apk";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

                if (il2cppGLES3)
                {
                    PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new [] { GraphicsDeviceType.OpenGLES3 });
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                    buildPlayerOptions.locationPathName = path + name + "_IL2CPP_OpenGLES.apk";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

                if (monoVulkan)
                {
                    PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new [] { GraphicsDeviceType.Vulkan });
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                    buildPlayerOptions.locationPathName = path + name + "_Mono_Vulkan.apk";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

                if (il2cppVulkan)
                {
                    PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new [] { GraphicsDeviceType.Vulkan });
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                    buildPlayerOptions.locationPathName = path + name + "_IL2CPP_Vulkan.apk";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

            }

            //requirements: all three "Managed Stripping Level" variants (Low, Medium, High)
            //total: 3 builds
            if (buildiOS) 
            {
                path = Path.GetDirectoryName(Application.dataPath) + "/Builds/iOS/";
                buildPlayerOptions.target = BuildTarget.iOS;
                
                if (low)
                {
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
                    buildPlayerOptions.locationPathName = path + name + "_Low";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

                if (medium)
                {
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Medium);
                    buildPlayerOptions.locationPathName = path + name + "_Medium";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

                if (high)
                {
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.High);
                    buildPlayerOptions.locationPathName = path + name + "_High";
                    BuildPipeline.BuildPlayer(buildPlayerOptions);
                }

            }
        }
    }
}