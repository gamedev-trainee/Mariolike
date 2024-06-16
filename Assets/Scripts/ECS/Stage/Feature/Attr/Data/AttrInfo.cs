namespace Mariolike
{
    [System.Serializable]
    public class AttrInfo
    {
        public AttrTypes type = AttrTypes.None;
        public int value = 0;
    }

    public struct AttrChangeInfo
    {
        public AttrTypes type;
        public int value;
    }
}
