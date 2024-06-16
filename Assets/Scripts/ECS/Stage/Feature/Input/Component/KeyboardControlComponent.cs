using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public partial class KeyboardControlComponent : IComponent
    {
        [ConfigField]
        public KeyCode moveLeftKey = KeyCode.LeftArrow;
        [ConfigField]
        public KeyCode moveRightKey = KeyCode.RightArrow;
        [ConfigField]
        public KeyCode jumpKey = KeyCode.UpArrow;
    }
}
