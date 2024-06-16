using ECSlike;
using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public class AttackComponent : IComponent
    {
        [ConfigField(gizmosDrawer = "DrawAttackClips")]
        public List<AttackClip> attackClips = new List<AttackClip>();

        public static void DrawAttackClips(Transform transform, List<AttackClip> infos)
        {
            Color color = Gizmos.color;
            if (infos != null)
            {
                Vector3 scale = transform.localScale;
                Vector3 offset;
                Vector3 size;
                int count = infos.Count;
                for (int i = 0; i < count; i++)
                {
                    if (infos[i] == null) continue;
                    Gizmos.color = infos[i].color;
                    offset = infos[i].rangeOffset;
                    size = infos[i].rangeSize;
                    offset.x *= scale.x;
                    offset.y *= scale.y;
                    offset.z *= scale.z;
                    size.x *= scale.x;
                    size.y *= scale.y;
                    size.z *= scale.z;
                    Gizmos.DrawWireCube(transform.position + offset, size);
                }
            }
            Gizmos.color = color;
        }
    }
}
