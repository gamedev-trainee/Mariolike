using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class RotationComponent : IComponent
    {
        public Vector3 rotation = Vector3.zero;
        public bool changed = false;

        public void setRotation(Vector3 value)
        {
            rotation = value;
            changed = true;
        }

        public void setDirection(int value)
        {
            rotation.y = value < 0 ? 180 : 0;
            changed = true;
        }
    }
}
