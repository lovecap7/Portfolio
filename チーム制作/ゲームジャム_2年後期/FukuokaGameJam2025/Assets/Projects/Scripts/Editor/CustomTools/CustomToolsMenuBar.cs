using mySystem;
using UnityEditor;
using UnityEngine;

namespace myEditor.Creator
{
    public class CustomToolsMenuBar : EditorWindow
    {
        public const string CreatorMenuTab = "MyEditor/Creator/";
        public const string ToolsMenuTab = "MyEditor/Tools/";
        public const string BuildMenuTab = "MyEditor/Build/";
        
        [MenuItem(CreatorMenuTab + "Tag Name")]
        public static void CreateTagName()
        {
            TagNameCreator.TagNameCreate();
        }

        [MenuItem(CreatorMenuTab + "Layer Name")]
        public static void CreateLayerName()
        {
            LayerNameCreator.LayerNameCreate();
        }

        [MenuItem(ToolsMenuTab + "Open GameConfig")]
        public static void OpenGameConfig()
        {
            var config = Resources.Load<GameConfig>("SO_GameConfig");
            if (config == null)
            {
                EditorUtility.DisplayDialog("Error", "GameConfig がありません！\nResources に GameConfig を作成してください", "OK");
            }

            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }

        [MenuItem(BuildMenuTab + "Windows Package")]
        public static void WindowsBuild()
        {
            BuildUtility.AutoRegisterBuildScenes();
            BuildUtility.StartBuildWindowsPackage();
        }

        [MenuItem(BuildMenuTab + "WebGL Package")]
        public static void WebGLBuild()
        {
            BuildUtility.AutoRegisterBuildScenes();
            BuildUtility.StartBuildWebGLPackage();
        }

        [MenuItem(BuildMenuTab + "Auto Register BuildScenes")]
        public static void AutoRegisterBuildScenes()
        {
            BuildUtility.AutoRegisterBuildScenes();
        }
    }
}