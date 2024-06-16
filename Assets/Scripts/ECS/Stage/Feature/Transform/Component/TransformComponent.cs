using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class TransformComponent : IComponent
    {
        [ConfigField]
        public Transform transform = null;
    }
}
