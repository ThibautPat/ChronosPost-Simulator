using UnityEngine;
using UnityEditor;

using System.IO;

namespace NYX
{
    public class SettingsManagement : EditorWindow
    {
        static bool security;
        static NYX_Settings settings;

        //[SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            #region Utils
            void LoadSettings()
            {
                string[] assets = AssetDatabase.FindAssets("t:NYX_Settings");
                if (assets.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                    settings = AssetDatabase.LoadAssetAtPath<NYX_Settings>(path);
                }
            }
            #endregion

            LoadSettings();

            SettingsProvider provider = new SettingsProvider("Project/Input Manager", SettingsScope.Project)
            {
                label = "Input Manager",
                guiHandler = (searchContext) =>
                {

                    GUILayout.Space(8);

                    ////////////////////////////////
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(5.4f);
                    GUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
                    ////////////////////////////////

                    EditorGUILayout.LabelField("Generals", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Smoothing Multiplier : x" + settings.smoothingMutliplier, GUILayout.MaxWidth(200));
                    settings.smoothingMutliplier = EditorGUILayout.IntSlider(settings.smoothingMutliplier, 0, 100);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Smoothing Precision : 10^" + settings.smoothingPrecision, GUILayout.MaxWidth(200));
                    settings.smoothingPrecision = EditorGUILayout.IntSlider(settings.smoothingPrecision, 1, 8);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Save-File Path : ", GUILayout.MaxWidth(200));
                    settings.saveFilePath = EditorGUILayout.TextField(settings.saveFilePath);
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(8);

                    EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);

                    if(GUILayout.Button("Fix Input System") & !security)
                    {
                        security = true;

                        string backupFileName = "InputManager.nyx_backup";
                        string[] backupFilePaths = Directory.GetFiles(Application.dataPath, backupFileName, SearchOption.AllDirectories);
                        if (backupFilePaths.Length == 0) { Debug.LogError("InputManager.nyx_backup file not found."); return; }
                        string backupFilePath = backupFilePaths[0];
                        string resourcesFolderPath = Path.GetDirectoryName(backupFilePath);
                        string backupFileFullPath = Path.Combine(resourcesFolderPath, backupFileName);
                        string renamedBackupFilePath = Path.Combine(resourcesFolderPath, Path.GetFileNameWithoutExtension(backupFilePath) + ".asset");
                        string projectSettingsPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), "ProjectSettings", "InputManager.asset");
                        File.Move(backupFileFullPath, renamedBackupFilePath);
                        File.Copy(renamedBackupFilePath, projectSettingsPath, true);
                        string originalBackupFilePath = Path.Combine(Path.GetDirectoryName(renamedBackupFilePath), Path.GetFileNameWithoutExtension(renamedBackupFilePath) + ".nyx_backup");
                        File.Move(renamedBackupFilePath, originalBackupFilePath);

                        Debug.Log("Unity InputSystem Fixed !");
                        security = false;

                        RestartEditor();
                    }

                    if(GUILayout.Button("Reset Settings"))
                    {
                        settings.smoothingMutliplier = 50;
                        settings.smoothingPrecision = 3;
                        settings.saveFilePath = "inputs.nyx";
                    }

                    ////////////////////////////////
                    GUILayout.EndVertical();
                    GUILayout.Space(5.4f);
                    EditorGUILayout.EndHorizontal();
                    ////////////////////////////////
                },
                keywords = new[] { "Input", "input", "Manager", "manager", "inputs", "management", "NYX", "Nyx", "nyx" }
            };

            return provider;
        }

        public static void RestartEditor()
        {
            bool choice = EditorUtility.DisplayDialog("NYX - InputSystem", "In order for NYX to apply changes to Unity's InputManager, you need to restart the editor.", "Restart Now", "Maybe Later");
            if (choice)
            {
                string projectPath = Application.dataPath.Remove(
                Application.dataPath.Length - "/Assets".Length, "/Assets".Length);
                EditorApplication.OpenProject(projectPath);
            }
        }
    }
}