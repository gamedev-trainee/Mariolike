/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

using UnityEngine;

namespace Mariolike
{
    [DisallowMultipleComponent]
    public class KeyboardControlConfigScript : MonoBehaviour, ECSlike.IComponentConfig
    {
        public UnityEngine.KeyCode moveLeftKey = KeyCode.LeftArrow;
        public UnityEngine.KeyCode moveRightKey = KeyCode.RightArrow;
        public UnityEngine.KeyCode jumpKey = KeyCode.UpArrow;
    }
}