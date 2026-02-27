using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

namespace ShadowRace.EditorTools
{
    public class BuildPipelineTool
    {
        [MenuItem("Shadow Race/Build/Windows (64-bit)")]
        public static void BuildWindows()
        {
            GenericBuild(BuildTarget.StandaloneWindows64, "Builds/Windows/ShadowRace.exe");
        }

        [MenuItem("Shadow Race/Build/Mac OS")]
        public static void BuildMacOS()
        {
            GenericBuild(BuildTarget.StandaloneOSX, "Builds/Mac/ShadowRace.app");
        }

        private static void GenericBuild(BuildTarget target, string path)
        {
            Debug.Log($"Starting Automated Build for {target}...");

            // Ensure directory exists
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Get all enabled scenes from Build Settings
            string[] scenes = GetEnabledScenes();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = path,
                target = target,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build Succeeded: {summary.totalSize} bytes");
                // Open the folder containing the build
                EditorUtility.RevealInFinder(path);
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build Failed! Check the console for errors.");
            }
        }

        private static string[] GetEnabledScenes()
        {
            int sceneCount = EditorBuildSettings.scenes.Length;
            System.Collections.Generic.List<string> scenes = new System.Collections.Generic.List<string>();

            for (int i = 0; i < sceneCount; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    scenes.Add(EditorBuildSettings.scenes[i].path);
                }
            }

            return scenes.ToArray();
        }
    }
}
