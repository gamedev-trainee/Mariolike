using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class PositionComponent : IComponent
    {
        public Vector3 position = Vector3.zero;
        public Vector3 offset = Vector3.zero;
        public bool changed = false;

        public void setPosition(Vector3 value)
        {
            offset = value - position;
            changed = true;
        }

        public void addPosition(Vector3 value)
        {
            offset += value;
            changed = true;
        }

        public void addX(float value)
        {
            offset.x += value;
            changed = true;
        }

        public void addY(float value)
        {
            offset.y += value;
            changed = true;
        }
    }
}
