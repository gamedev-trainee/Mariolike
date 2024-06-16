using System.ComponentModel;
using UnityEngine;

namespace Mariolike
{
    [DisplayName("scale")]
    public class ScaleActionClip : TimeActionClip
    {
        public Vector3 scale = Vector3.one;
    }
}
