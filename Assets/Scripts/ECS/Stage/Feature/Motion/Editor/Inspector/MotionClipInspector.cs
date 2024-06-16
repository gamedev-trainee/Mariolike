using UnityEditor;
using UnityEngine;

namespace Mariolike
{
    [CustomEditor(typeof(MotionClip))]
    public class MotionClipInspector : Editor
    {
        private MotionClip m_target = null;

        private void OnEnable()
        {
            m_target = target as MotionClip;
        }

        private void OnDisable()
        {
            m_target = null;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            bool changed = false;

            if (GUILayout.Button("Delete"))
            {
                if (m_target.action != null)
                {
                    Undo.RecordObject(m_target, "Delete MotionClip Action");
                    AssetDatabase.RemoveObjectFromAsset(m_target.action);
                    Undo.RecordObject(m_target.action, "Delete MotionClip Action");
                    GameObject.DestroyImmediate(m_target.action);
                    m_target.action = null;
                    changed = true;
                }
            }

            if (m_target.action != null)
            {
                ActionClip actionClip = MotionEditorUtils.DrawActionCreateMenu(m_target, "Replace");
                if (actionClip != null)
                {
                    if (m_target.action != null)
                    {
                        Undo.RecordObject(m_target, "Replace MotionClip Action");
                        AssetDatabase.RemoveObjectFromAsset(m_target.action);
                        Undo.RecordObject(m_target.action, "Replace MotionClip Action");
                        GameObject.DestroyImmediate(m_target.action);
                        m_target.action = null;
                    }
                    m_target.action = actionClip;
                    changed = true;
                }
            }
            else
            {
                ActionClip actionClip = MotionEditorUtils.DrawActionCreateMenu(m_target, "Create");
                if (actionClip != null)
                {
                    Undo.RecordObject(m_target, "Create MotionClip Action");
                    m_target.action = actionClip;
                    changed = true;
                }
            }

            if (changed)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
