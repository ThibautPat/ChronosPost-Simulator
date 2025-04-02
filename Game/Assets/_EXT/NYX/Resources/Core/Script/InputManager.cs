// Importing libraries
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace NYX {
    public class InputManager : MonoBehaviour {
        // Vars
        [Header("[ Inputs ]")]
        [Tooltip("List of all keys in scene.")] public Key[] keys;

        #region KeyClass
        // Backup Class
        [HideInInspector] public List<DefaultKey> defaultKeys;
        [Serializable]
        public class DefaultKey
        {
            public KeyCode keyboard_key;
            public KeyCode gamepad_key;
        }

        // Class
        [Serializable]
        public class Key
        {
            [Tooltip("Name of your key action.")] public string keyName = "myKey";
            [Tooltip("The keycode to use with keyboards.")] public KeyCode keyboardKey = KeyCode.Space;
            [Tooltip("The button to use on gamepads.")] public KeyCode gamepadKey = KeyCode.Joystick1Button0;
        }
        #endregion

        [Header("[ Axes ]")]
        [Tooltip("List of all axes in scene.")] public Axis[] axes;

        #region AxisClass
        // Backup Class
        [HideInInspector] public List<DefaultAxis> defaultAxes;
        [Serializable]
        public class DefaultAxis
        {
            public KeyCode keyboard_positive;
            public KeyCode keyboard_negative;

            public KeyCode gamepad_positive;
            public KeyCode gamepad_negative;

            public GamepadInputs gamepad_axis;
        }

        // Enum for controllers
        public enum GamepadInputs
        {
            None,
            LeftJoyX,
            LeftJoyY,
            Triggers,
            RightJoyX,
            RightJoyY,
            CrossX,
            CrossY
        }

        // Classes
        [Serializable]
        public class KeyboardAxis
        {
            [Tooltip("The key that will return '1' value.")] public KeyCode positiveKey = KeyCode.UpArrow;
            [Tooltip("The key that will return '-1' value.")] public KeyCode negativeKey = KeyCode.DownArrow;
        }

        [Serializable]
        public class GamepadAxis
        {
            [Tooltip("The key that will return '1' value.")] public KeyCode positiveKey = KeyCode.None;
            [Tooltip("The key that will return '-1' value.")] public KeyCode negativeKey = KeyCode.None;

            [Tooltip("The axis to use when a gamepad is connected.")]
            public GamepadInputs axis = GamepadInputs.LeftJoyY;
        }

        [Serializable]
        public class Axis
        {
            [Tooltip("Name of your axis.")] public string axisName = "myAxis";

            [Tooltip("Keyboard inputs config.")] public KeyboardAxis keyboard;
            [Tooltip("Gamepad inputs config.")] public GamepadAxis gamepad;
            
            [Tooltip("Should your axis be smoothed ?")] public bool useAxisSmoothing = false;
            [Tooltip("The amount of smoothing added to the axis.")] [Range(0, 1)] public float axisSmoothingAmount = 0.5f;
            [Tooltip("The realtime value returned by the axis.")] public float value = 0f;
            
            [HideInInspector] public float refValue = 0;
        }
        #endregion

        // Shared
        public static InputManager instance;

        // Privates
        NYX_Settings project_settings;
        int round; float multiplier, limit;
        string path;
        

        void Awake()
        {
            // Get static reference
            InputManager[] managers = FindObjectsOfType<InputManager>();
            if (managers.Length < 2) { instance = this; }

            // Get project settings
            project_settings = Resources.Load("Presets/ProjectConfiguration/NYX_settings") as NYX_Settings;
            path = Application.dataPath + "/" + project_settings.saveFilePath;
            round = Mathf.RoundToInt(Mathf.Pow(10, project_settings.smoothingPrecision));
            multiplier = project_settings.smoothingMutliplier / 64f;
            limit = Mathf.Pow(4.85f, -project_settings.smoothingPrecision);

            // Backup the inputs
            PackDefaultInputs();
            // Loads the saved inputs
            LoadInputs();
        }

        // FUNCTIONS //

        public void PackDefaultInputs()
        {
            defaultAxes = new List<DefaultAxis>();
            defaultKeys = new List<DefaultKey>();

            #region packing keys
            for (int i = 0; i < keys.Length; i++)
            {
                // Create new instance of a backup
                DefaultKey newItem = new DefaultKey();

                // Store correct data inside
                newItem.keyboard_key = keys[i].keyboardKey;
                newItem.gamepad_key = keys[i].gamepadKey;

                // Add the new instance in the list of backups keys
                defaultKeys.Add(newItem);
            }
            #endregion

            #region packing axis
            for (int i = 0; i < axes.Length; i++)
            {
                // Create new instance of a backup
                DefaultAxis newItem = new DefaultAxis();

                // Store correct data inside
                newItem.keyboard_positive = axes[i].keyboard.positiveKey;
                newItem.keyboard_negative = axes[i].keyboard.negativeKey;
                newItem.gamepad_negative = axes[i].gamepad.negativeKey;
                newItem.gamepad_negative = axes[i].gamepad.negativeKey;
                newItem.gamepad_axis = axes[i].gamepad.axis;

                // Add the new instance in the list of backups axis
                defaultAxes.Add(newItem);
            }
            #endregion
        }

        public void LoadInputs()
        {
            // Check if save already exist to avoid load-errors
            if (File.Exists(path))
            {
                // Get inputs by index from custom-save file

                #region LoadKeys
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i].keyboardKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_key_keyboard"));
                    keys[i].gamepadKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_key_gamepad"));
                }
                #endregion

                #region LoadAxis
                for (int i = 0; i < axes.Length; i++)
                {
                    axes[i].keyboard.positiveKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_axis_keyboard_positive"));
                    axes[i].keyboard.negativeKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_axis_keyboard_negative"));
                    axes[i].gamepad.positiveKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_axis_gamepad_positive"));
                    axes[i].gamepad.negativeKey = (KeyCode)Enum.Parse(typeof(KeyCode), ReadString(i.ToString() + "_axis_gamepad_negative"));
                    axes[i].gamepad.axis = (GamepadInputs)Enum.Parse(typeof(GamepadInputs), ReadString(i.ToString() + "_axis_gamepad_axis"));
                }
                #endregion
            }

        }

        public void SaveInputs()
        {
            // Save data by index in custom-save file

            #region SaveKeys
            for (int i = 0; i < keys.Length; i++)
            {
                SaveString(i.ToString() + "_key_keyboard", keys[i].keyboardKey.ToString());
                SaveString(i.ToString() + "_key_gamepad", keys[i].gamepadKey.ToString());
            }
            #endregion

            #region SaveAxis
            for (int i = 0; i < axes.Length; i++)
            {
                SaveString(i.ToString() + "_axis_keyboard_positive", axes[i].keyboard.positiveKey.ToString());
                SaveString(i.ToString() + "_axis_keyboard_negative", axes[i].keyboard.negativeKey.ToString());
                SaveString(i.ToString() + "_axis_gamepad_positive", axes[i].gamepad.positiveKey.ToString());
                SaveString(i.ToString() + "_axis_gamepad_negative", axes[i].gamepad.negativeKey.ToString());
                SaveString(i.ToString() + "_axis_gamepad_axis", axes[i].gamepad.axis.ToString());
            }
            #endregion
        }

        public void ResetInputs()
        {
            // Delete the saved file
            File.Delete(path);

            // Reset values based on backups classes
            #region ResetKeys
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].keyboardKey = defaultKeys[i].keyboard_key;
                keys[i].gamepadKey = defaultKeys[i].gamepad_key;
            }
            #endregion

            #region ResetAxis
            for (int i = 0; i < axes.Length; i++)
            {
                axes[i].keyboard.positiveKey = defaultAxes[i].keyboard_positive; axes[i].keyboard.negativeKey = defaultAxes[i].keyboard_negative;
                axes[i].gamepad.positiveKey = defaultAxes[i].gamepad_positive; axes[i].gamepad.negativeKey = defaultAxes[i].gamepad_negative;
                axes[i].gamepad.axis = defaultAxes[i].gamepad_axis;
            }
            #endregion
        }

        // SAVE SYSTEM //

        #region SaveUtils
        void SaveString(string key, string value)
        {
            // If file exist, read to avoid key-duplication
            if (File.Exists(path))
            {
                // Get all the lines in the file
                string[] lines = File.ReadAllLines(path);

                // Get all lines different from the key
                string[] filteredLines = lines.Where(line => !line.Contains("%" + key + "%")).ToArray();

                // re-write file with only thoses to avoid duplication
                File.WriteAllLines(path, filteredLines);
            }

            // Save key to file
            File.AppendAllText(path, "%" + key + "%" + ":" + value + "\n");
        }

        string ReadString(string key)
        {
            // Get value by key
            string data = File.ReadLines(path).SkipWhile(line => !line.Contains("%" + key + "%")).First();
            string value = data.Split(":")[1];
            
            // Return value if finded
            return value;
        }
        #endregion

        // INPUT SYSTEM //

        public float GetAxis(string name) { return axes[Array.FindIndex(axes, obj => obj.axisName == name)].value; }
        public Axis GetAxisObject(string axisName) { return axes[Array.FindIndex(axes, obj => obj.axisName == axisName)]; }

        public bool GetKey(string name)
        {
            Key key = keys[Array.FindIndex(keys, obj => obj.keyName == name)];
            return Input.GetKey(key.keyboardKey) || Input.GetKey(key.gamepadKey);
        }
        public bool GetKeyDown(string name)
        {
            Key key = keys[Array.FindIndex(keys, obj => obj.keyName == name)];
            return Input.GetKeyDown(key.keyboardKey) || Input.GetKeyDown(key.gamepadKey);
        }
        public bool GetKeyUp(string name)
        {
            Key key = keys[Array.FindIndex(keys, obj => obj.keyName == name)];
            return Input.GetKeyUp(key.keyboardKey) || Input.GetKeyUp(key.gamepadKey);
        }
        public Key GetKeyObject(string name) { return keys[Array.FindIndex(keys, obj => obj.keyName == name)]; }

        // Update keys & axes
        void Update()
        {
            for (int i = 0; i < axes.Length; i++)
            {
                // Do we use gamepad ?
                float gamepadValue = 1;
                if (Input.GetAxisRaw(axes[i].gamepad.axis.ToString()) !=0)
                { gamepadValue = Mathf.Abs(Input.GetAxisRaw(axes[i].gamepad.axis.ToString())); }

                // Max smoothing
                float smooth = axes[i].axisSmoothingAmount * multiplier;

                // Positive
                if (Input.GetKey(axes[i].keyboard.positiveKey) || Input.GetKey(axes[i].gamepad.positiveKey) || Input.GetAxis(axes[i].gamepad.axis.ToString()) > 0)
                {
                    if (axes[i].useAxisSmoothing)
                    {
                        if(axes[i].value > 1 - limit) { axes[i].value = 1; }
                        else { axes[i].value = Mathf.Round(Mathf.SmoothDamp(axes[i].value, gamepadValue, ref axes[i].refValue, smooth) * round) / round; }
                    }
                    else {  axes[i].value = 1; }
                }

                // Negative
                else if (Input.GetKey(axes[i].keyboard.negativeKey) || Input.GetKey(axes[i].gamepad.negativeKey) || Input.GetAxis(axes[i].gamepad.axis.ToString()) < 0)
                {
                    if (axes[i].useAxisSmoothing)
                    {
                        if (axes[i].value < -1 + limit) { axes[i].value = -1; }
                        else { axes[i].value = Mathf.Round(Mathf.SmoothDamp(axes[i].value, -gamepadValue, ref axes[i].refValue, smooth) * round) / round; }
                    }
                    else { axes[i].value = -1; }
                }

                // Zero Return
                else
                {
                    if (axes[i].useAxisSmoothing)
                    {
                        if (axes[i].value < 0 + limit && axes[i].value > 0 - limit) { axes[i].value = 0; }
                        else { axes[i].value = Mathf.Round(Mathf.SmoothDamp(axes[i].value, 0, ref axes[i].refValue, smooth) * round) / round; }
                    }
                    else { axes[i].value = 0; }
                }
            }
        }
    }
}