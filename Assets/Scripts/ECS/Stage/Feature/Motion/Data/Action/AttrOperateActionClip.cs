using System.Collections.Generic;
using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("attr_operate")]
    public class AttrOperateActionClip : ActionClip
    {
        public List<AttrOperateInfo> operations = new List<AttrOperateInfo>();
        public bool dispatch = true;
    }
}
