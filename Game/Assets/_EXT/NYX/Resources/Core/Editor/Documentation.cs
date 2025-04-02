// Importing libraries
using UnityEngine;
using UnityEditor;

namespace NYX {
    public class OnlineDocumentation {
        // Declaring a new void in editor-menu
        [MenuItem("Window/Documentations/NYX Online Documentation")]
        static void OpenMyWebsite() { Application.OpenURL("https://sharkstudio.gitbook.io/nyx-input-system"); }
    }
}