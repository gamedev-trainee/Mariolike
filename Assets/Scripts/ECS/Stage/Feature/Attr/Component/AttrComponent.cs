using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public class AttrComponent : IComponent, IComponentInitializer
    {
        [ConfigField]
        public List<AttrInfo> attrs = new List<AttrInfo>();

        // IComponentInitializer

        public void initComponent()
        {
            int count = attrs.Count;
            for (int i = 0; i < count; i++)
            {
                setAttr(attrs[i].type, attrs[i].value);
            }
            copyAttr(AttrTypes.MaxHP, AttrTypes.HP);
        }
        //

        public Dictionary<AttrTypes, int> mAttrs = new Dictionary<AttrTypes, int>();

        // 

        public bool containsAttr(AttrTypes type)
        {
            return mAttrs.ContainsKey(type);
        }

        public int getAttr(AttrTypes type)
        {
            int value;
            if (mAttrs.TryGetValue(type, out value))
            {
                return value;
            }
            return 0;
        }

        public void setAttr(AttrTypes type, int value)
        {
            mAttrs[type] = value;
        }

        public void copyAttr(AttrTypes typeFrom, AttrTypes typeTo)
        {
            if (mAttrs.ContainsKey(typeFrom))
            {
                mAttrs[typeTo] = mAttrs[typeFrom];
            }
        }

        public void operateAttr(AttrTypes type, AttrOperations operation, int value)
        {
            switch (operation)
            {
                case AttrOperations.Plus:
                    {
                        if (mAttrs.ContainsKey(type))
                        {
                            mAttrs[type] += value;
                        }
                        else
                        {
                            mAttrs[type] = value;
                        }
                    }
                    break;
                case AttrOperations.Minus:
                    {
                        if (type == AttrTypes.HP)
                        {
                            if (mAttrs.ContainsKey(AttrTypes.ScaleHP))
                            {
                                if (mAttrs[AttrTypes.ScaleHP] >= value)
                                {
                                    mAttrs[AttrTypes.ScaleHP] -= value;
                                    return;
                                }
                                value -= mAttrs[AttrTypes.ScaleHP];
                                mAttrs[AttrTypes.ScaleHP] = 0;
                            }
                        }
                        if (mAttrs.ContainsKey(type))
                        {
                            mAttrs[type] -= value;
                        }
                        else
                        {
                            mAttrs[type] = -value;
                        }
                    }
                    break;
            }
        }
    }
}
