using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestGroundComponent : IComponent
    {
        [ConfigField]
        public float footRadius = 0f;
        [ConfigField]
        public LayerMask layerMask = 1 << (int)GameObjectLayers.Default | 1 << (int)GameObjectLayers.Brick;

        public HitTestFlags mHitFlags = HitTestFlags.None;
    }
}
