//////////////////////////////////////////////////////////////////////////////////////
//// This script is an example on how to use the input manager in others scripts ////
////////////////////////////////////////////////////////////////////////////////////

// Importing libraries
using UnityEngine;

namespace NYX {
    public class InputsDemo : MonoBehaviour {
        // Inspector refs
        [Header("References")]
        // First we need a way to get the 'InputManager' component of the scene, here it's just a manual affectation on a inspector variable.
        [SerializeField] InputManager inputs;

        // Display on screen
        void OnGUI()
        {
            GUILayout.Label("AXIS :");
            // Because 'axes' is just an array, we can acces a lot of things just using the index of the element like :
            for (int i = 0; i < inputs.axes.Length; i++) // Length
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(inputs.axes[i].axisName); // Name
                GUILayout.HorizontalSlider(inputs.axes[i].value, -1, 1, GUILayout.Width(100)); // Value
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
            // You can also use NYX's built-in functions to access specific data by name rather than index :
            // inputs.GetAxis("Vertical"); // Return 'float' value
            // inputs.GetAxisObject("Vertical"); Return object of type 'Axis'

            //----//
            GUILayout.Space(20);
            //----//

            GUILayout.Label("KEYS :");
            // Because 'keys' is just an array, we can acces a lot of things just using the index of the element like :
            for (int i = 0; i < inputs.keys.Length; i++) // Length
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(inputs.keys[i].keyName); // Name
                if (Input.GetKey(inputs.keys[i].keyboardKey) || Input.GetKey(inputs.keys[i].gamepadKey)) // Key
                { GUILayout.Button("PRESSED", GUILayout.Width(100)); } // Key Pressed
                else { GUILayout.Button("NOT PRESSED", GUILayout.Width(100)); }// !Key Pressed
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
            // You can also use NYX's built-in functions to access specific data by name rather than index :
            // inputs.GetKey("Reload") // return 'bool' value
            // inputs.GetKeyDown("Reload") // return 'bool' value
            // inputs.GetKeyUp("Reload") // return 'bool' value
            // inputs.GetKeyObject("Reload") // return object of type 'Key'

        }
    }
}