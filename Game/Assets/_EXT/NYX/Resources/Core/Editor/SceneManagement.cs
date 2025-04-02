using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace NYX
{
    public class SceneManagement : Editor
    {
        [MenuItem("GameObject/Inputs/Input Manager", priority = 6)]
        public static void Spawn_InputSystem()
        {
            GameObject Systems = Instantiate(Resources.Load("Core/Prefabs/NYX_Systems")) as GameObject;
            Systems.name = "NYX_Systems";
        }

        [MenuItem("GameObject/Inputs/Keybinds Menu", priority = 6)]
        public static void Spawn_KeybindSystem()
        {
            GameObject GUI = Instantiate(Resources.Load("GUI/Prefabs/NYX_GUI")) as GameObject;
            GameObject Event = Instantiate(Resources.Load("GUI/Prefabs/NYX_EventSystem")) as GameObject;
            GUI.name = "NYX_GUI";
            Event.name = "NYX_EventSystem";
        }

        [MenuItem("GameObject/Inputs/Remove All", priority = 6)]
        public static void Remove_Systems()
        {
            DestroyImmediate(GameObject.Find("NYX_Systems"));
            DestroyImmediate(GameObject.Find("NYX_GUI"));
            DestroyImmediate(GameObject.Find("NYX_EventSystem"));
        }

        [MenuItem("CONTEXT/InputManager/Save Profile")]
        public static void Create_Config()
        {
            if (Selection.activeGameObject != null)
            {
                InputManager inputs = Selection.activeGameObject.GetComponent<InputManager>();

                if (inputs != null)
                {
                    Preset preset = new Preset(inputs);
                    string path = Directory.GetDirectories(Application.dataPath, "Presets", SearchOption.AllDirectories)[0];
                    string relativePath = path.Replace(Application.dataPath, "Assets");
                    AssetDatabase.CreateAsset(preset, relativePath + "/" + "myNewConfig" + ".preset");
                    Object newConfig = AssetDatabase.LoadAssetAtPath(relativePath + "/" + "myNewConfig.preset", typeof(Preset));
                    Selection.activeObject = newConfig;
                    EditorGUIUtility.PingObject(newConfig);
                }
            }
        }

        [MenuItem("CONTEXT/InputManager/Locate Profiles")]
        public static void Select_Config()
        {
            string path = Directory.GetDirectories(Application.dataPath, "Presets", SearchOption.AllDirectories)[0];
            string relativePath = path.Replace(Application.dataPath, "Assets");
            Object config = AssetDatabase.LoadAssetAtPath(relativePath + "/" + "DefaultConfig.preset", typeof(Preset));
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }
    }
}