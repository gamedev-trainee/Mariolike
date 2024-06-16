using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("move_to_ui_attr")]
    public class MoveToUIAttrActionClip : TimeActionClip
    {
        public AttrTypes attrType = AttrTypes.None;
    }
}
