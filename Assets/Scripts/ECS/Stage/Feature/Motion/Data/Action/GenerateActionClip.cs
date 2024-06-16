using System.ComponentModel;
using UnityEngine;

namespace Mariolike
{
    [DisplayName("generate")]
    public class GenerateActionClip : ActionClip
    {
        public GameObject src = null;
        public Vector3 bornOffset = Vector3.zero;
        public MotionClip startMotion = null;
    }
}
