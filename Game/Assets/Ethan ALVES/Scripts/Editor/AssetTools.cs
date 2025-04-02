using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class AssetTools : EditorWindow
{
    [MenuItem("Tools/Asset Tools")]
    public static void ShowWindow()
    {
        GetWindow<AssetTools>("Asset Tools");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Rename Selected Assets"))
        {
            RenameSelectedAssets();
        }
    }

    static void RenameSelectedAssets()
    {
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.LogWarning("No assets selected.");
            return;
        }

        foreach (Object obj in selectedObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string assetExtension = Path.GetExtension(assetPath);

            string newName = ConvertToPascalCase(assetName);

            if (!string.IsNullOrEmpty(newName) && newName != assetName)
            {
                string newPath = Path.Combine(Path.GetDirectoryName(assetPath), newName + assetExtension);
                AssetDatabase.RenameAsset(assetPath, newName);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static string ConvertToPascalCase(string input)
    {
        string[] words = Regex.Split(input, "[^a-zA-Z0-9]");
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join("", words);
    }
}