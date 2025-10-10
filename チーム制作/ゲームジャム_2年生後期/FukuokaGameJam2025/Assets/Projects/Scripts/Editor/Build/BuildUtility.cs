using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace myEditor
{
    public static class BuildUtility
    {
        static readonly string path = "../Package/";
        static readonly string targetFolder = "Assets/Projects/Scenes";
        public static void AutoRegisterBuildScenes()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { targetFolder });
            string[] scenePaths = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith(".unity"))
                .ToArray();

            // Build Settings 用に変換
            var newScenes = scenePaths
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();

            EditorBuildSettings.scenes = newScenes;
        }

        public static void StartBuildWindowsPackage()
        {        
            // Build Settings に登録されているシーンのみ使用
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string buildFolder = Path.Combine(path, "Windows_" + timestamp);
            string exeName = PlayerSettings.productName + ".exe";

            // 出力パス
            string buildPath = Path.Combine(buildFolder, exeName);

            // 出力先フォルダがなければ作成
            Directory.CreateDirectory(buildFolder);

            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                UnityEngine.Debug.Log("Windows ビルド成功: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                UnityEngine.Debug.LogError("Windows ビルド失敗");
            }
        }

        public static void StartBuildWebGLPackage()
        {        
            // Build Settings に登録されているシーンのみ使用
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string buildFolder = Path.Combine(path, "WebGL_" + timestamp);
            string exeName = PlayerSettings.productName;

            // 出力パス
            string buildPath = Path.Combine(buildFolder, exeName);

            // 出力先フォルダがなければ作成
            Directory.CreateDirectory(buildFolder);

            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                UnityEngine.Debug.Log("Windows ビルド成功: " + summary.totalSize + " bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                UnityEngine.Debug.LogError("Windows ビルド失敗");
            }
        }
    }
}
