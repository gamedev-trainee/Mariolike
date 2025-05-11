using UnityEngine;

namespace Mariolike
{
    [CreateAssetMenu(fileName = "WorldSettings.asset", menuName = "Mariolike/World Settings")]
    public class WorldSettings : ScriptableObject
    {
        public int hostLife = 0;
        public float stageTime = 0f;
    }
}
