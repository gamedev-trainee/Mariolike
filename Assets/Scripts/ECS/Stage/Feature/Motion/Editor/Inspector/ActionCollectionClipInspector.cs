using UnityEditor;
using UnityEngine;

namespace Mariolike
{
    [CustomEditor(typeof(ActionCollectionClip))]
    public class ActionCollectionClipInspector : Editor
    {
        private ActionCollectionClip m_target = null;

        private void OnEnable()
        {
            m_target = target as ActionCollectionClip;
        }

        private void OnDisable()
        {
            m_target = null;
        }

        public override void OnInspectorGUI()
        {
            bool changed = false;

            EditorGUI.BeginChangeCheck();
            {
                m_target.sortingType = (SortingTypes)EditorGUILayout.EnumPopup("SortingType", m_target.sortingType);
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(m_target);
                changed = true;
            }

            EditorGUILayout.LabelField(string.Format("Total: {0}", m_target.actions.Count));
            ActionClip actionClip;
            int count = m_target.actions.Count;
            for (int i = 0; i < count; i++)
            {
                actionClip = m_target.actions[i];
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(EditorGUIUtility.labelWidth));
                    EditorGUILayout.ObjectField(actionClip, typeof(ActionClip), false, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("X", GUILayout.Width(30f)))
                    {
                        Undo.RecordObject(m_target, "Remove ActionClip Action");
                        if (actionClip != null)
                        {
                            AssetDatabase.RemoveObjectFromAsset(actionClip);
                            Undo.RecordObject(actionClip, "Remove ActionClip Action");
                            ScriptableObject.DestroyImmediate(actionClip);
                        }
                        m_target.actions.RemoveAt(i);
                        i--;
                        count--;
                        changed = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            actionClip = MotionEditorUtils.DrawActionCreateMenu(m_target, "Add");
            if (actionClip != null)
            {
                Undo.RecordObject(m_target, "Add ActionClip Action");
                m_target.actions.Add(actionClip);
                changed = true;
            }

            if (changed)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
