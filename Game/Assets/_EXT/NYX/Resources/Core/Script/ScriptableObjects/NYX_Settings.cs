using UnityEngine;

namespace NYX
{
    public class NYX_Settings : ScriptableObject
    {
        [Header("Project Settings")]

        [Range(0, 100)]
        public int smoothingMutliplier = 50;
        [Range(0, 8)]
        public int smoothingPrecision = 3;
        [Space]
        public string saveFilePath = "inputs.nyx";
    }
}