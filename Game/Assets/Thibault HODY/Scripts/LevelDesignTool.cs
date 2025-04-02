using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using System.Collections.Generic;
using System.Linq;

public class LevelDesignTool : EditorWindow
{
    public RoadConfiguration roadConfiguration;
    public List<Road> roadElementsList = new List<Road>();
    static void Init() => GetWindow<LevelDesignTool>();
    Vector2 scrollPosition = Vector2.zero;



    [MenuItem("Tools/LevelDesignTool")]
    public static void ShowWindow()
    {

        GetWindow<LevelDesignTool>("LevelDesignTool");


    }

    void OnGUI()
    {
        float windowWidth = position.width;
        float windowHeight = position.height;

        // Rendre la ScrollView responsive en fonction de la taille de la fenêtre
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true,GUILayout.Width(windowWidth),GUILayout.Height(windowHeight)); EditorGUI.BeginChangeCheck();



        roadConfiguration = (RoadConfiguration)EditorGUILayout.ObjectField("Prefab à instancier", roadConfiguration, typeof(RoadConfiguration), false);
        if (GUILayout.Button("Load Configuration"))
        {
            
            roadElementsList.Clear();


            foreach (var road in roadConfiguration.roadElementsList)
            {
                roadElementsList.Add(road);
            }


        }
        foreach (var road in roadElementsList)
        {
            GUIContent buttonContent = new GUIContent(road.prefab.name, road.icon);

            
            // Bouton pour instancier le prefab de route droite
            if (GUILayout.Button(buttonContent))
            {


                AddPrefabToScene(road.prefab);


            }
        }
        if (GUILayout.Button("Clear Selection"))
        {



            foreach (var road in Selection.gameObjects)
            {
                if (road != null)
                {
                    RoadElement roadElement = null;
                    road.TryGetComponent<RoadElement>(out roadElement);
                    if (roadElement != null)
                    {
                        roadElement.Bake();
                    }
                }
            }


        }
        GUILayout.EndScrollView();
    }

    private void AddPrefabToScene(GameObject prefabActuel)
    {
        if (prefabActuel != null)
        {

            Quaternion rotation = Selection.gameObjects[0].transform.rotation;
            Vector3 position = Selection.gameObjects[0].transform.position;

            GameObject instance = Instantiate(prefabActuel, position, rotation);
            Undo.RegisterCreatedObjectUndo(instance, "SpawnedRoad");
            RoadElement road = instance.GetComponent<RoadElement>();
            Selection.activeGameObject = road.points[1].gameObject;

            Debug.Log("Prefab ajouté à la scène !");

        }
        else
        {
            Debug.LogWarning("Veuillez sélectionner un prefab à instancier.");
        }
    }

}
