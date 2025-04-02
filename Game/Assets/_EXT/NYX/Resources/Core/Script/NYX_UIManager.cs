using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace NYX {
    public class NYX_UIManager : MonoBehaviour {
        // Vars
        [Header("Setup")]
        [SerializeField] SelectedItem defaultSelected = SelectedItem.SaveButton;

        [Space]

        [Header("References")]
        [SerializeField] Transform itemHandler;
        [SerializeField] GameObject awaitInputPanel;
        [SerializeField] GameObject keyItem;
        [SerializeField] GameObject axisItem;

        // Hidden
        [HideInInspector] public List<GameObject> axisItems;
        [HideInInspector] public List<GameObject> keyItems;

        // Shared
        public static NYX_UIManager instance;

        // Privates
        InputManager inputs;
        EventSystem events;

        enum SelectedItem
        {
            None,
            ScrollBar,
            SaveButton,
            ResetButton,
            BackButton
        }

        void Awake()
        {
            // Get static reference
            instance = this;
        }

        void Start()
        {
            // Get the InputManager
            inputs = InputManager.instance;

            // Get the EventSystem
            GameObject.Find("NYX_EventSystem").TryGetComponent(out events);

            // Set default NavElement
            if(inputs != null)
            {
                GameObject defaultObject = null;
                if (defaultSelected == SelectedItem.ScrollBar) { defaultObject = GameObject.Find("Scrollbar"); }
                else if (defaultSelected == SelectedItem.BackButton) { defaultObject = GameObject.Find("BackButton"); }
                else if (defaultSelected == SelectedItem.SaveButton) { defaultObject = GameObject.Find("SaveButton"); }
                else if (defaultSelected == SelectedItem.ResetButton) { defaultObject = GameObject.Find("ResetButton"); }
                events.firstSelectedGameObject = defaultObject;
            }

            // Init Axis
            for (int i = 0; i < inputs.axes.Length; i++)
            {
                GameObject item = Instantiate(axisItem, itemHandler);
                item.GetComponent<AxisItem>().axisIndex = i;
                axisItems.Add(item);
            }

            // Init Keys
            for (int i = 0; i < inputs.keys.Length; i++)
            {
                GameObject item = Instantiate(keyItem, itemHandler);
                item.GetComponent<KeyItem>().keyIndex = i;
                keyItems.Add(item);
            }
        }

        // FUNCTIONS //

        public void AwaitInputPanel(bool value) { awaitInputPanel.SetActive(value); }
        public void Save() { inputs.SaveInputs(); Debug.Log("Inputs saved to custom file !"); }

        public void Reset()
        {   
            // Restore InputManager
            inputs.ResetInputs();

            #region UpdateAxis
            for (int i = 0; i < axisItems.Count; i++)
            {
                GameObject item = axisItems[i];
                item.GetComponent<AxisItem>().UpdateValues();
            }
            #endregion

            # region UpdateKeys
            for (int i = 0; i < keyItems.Count; i++)
            {
                GameObject item = keyItems[i];
                item.GetComponent<KeyItem>().UpdateValues();
            }
            #endregion
        }
    }
}