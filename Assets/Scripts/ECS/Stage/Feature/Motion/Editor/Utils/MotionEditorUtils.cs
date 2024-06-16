using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Mariolike
{
    public class MotionEditorUtils
    {
        private static List<System.Type> ms_actionClipTypes = null;

        public static int CompareType(System.Type typeA, System.Type typeB)
        {
            return typeA.Name.CompareTo(typeB.Name);
        }

        public static ActionClip DrawActionCreateMenu(Object src, string title = "Create")
        {
            InitActionClipTypes();
            ActionClip actionSettings;
            string content;
            DisplayNameAttribute displayNameAttribute;
            string displayName;
            int count = ms_actionClipTypes.Count;
            for (int i = 0; i < count; i++)
            {
                content = string.Format("{0} {1}", title, ms_actionClipTypes[i].Name);
                if (GUILayout.Button(content))
                {
                    Undo.RecordObject(src, content);
                    actionSettings = ActionClip.CreateInstance(ms_actionClipTypes[i]) as ActionClip;
                    displayNameAttribute = ms_actionClipTypes[i].GetCustomAttribute<DisplayNameAttribute>();
                    displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : ms_actionClipTypes[i].Name;
                    if (AssetDatabase.IsMainAsset(src))
                    {
                        actionSettings.name = displayName;
                    }
                    else
                    {
                        actionSettings.name = string.Format("{0}/{1}", src.name, displayName);
                    }
                    AssetDatabase.AddObjectToAsset(actionSettings, AssetDatabase.GetAssetPath(src));
                    return actionSettings;
                }
            }
            return null;
        }

        protected static void InitActionClipTypes()
        {
            if (ms_actionClipTypes != null) return;
            ms_actionClipTypes = new List<System.Type>();
            TypeCache.TypeCollection typeCollection = TypeCache.GetTypesDerivedFrom<ActionClip>();
            int count = typeCollection.Count;
            for (int i = 0; i < count; i++)
            {
                if (typeCollection[i].IsAbstract) continue;
                ms_actionClipTypes.Add(typeCollection[i]);
            }
            ms_actionClipTypes.Sort(CompareType);
        }
    }
}
