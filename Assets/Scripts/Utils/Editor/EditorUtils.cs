using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Mariolike
{
    public class EditorUtils
    {
        public static System.Type InspectorType { get; private set; } = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        public static PropertyInfo InspectorLockProperty { get; private set; } = InspectorType.GetProperty("isLocked");
        public static System.Type SceneViewType { get; private set; } = typeof(Editor).Assembly.GetType("UnityEditor.SceneView");
        public static MethodInfo SceneViewSetBackMethod = SceneViewType.GetMethod("SetOrthoBackView", BindingFlags.Static | BindingFlags.NonPublic);

        public static void LockInspector()
        {
            EditorWindow inspectorWindow = EditorWindow.GetWindow(InspectorType);
            InspectorLockProperty.SetValue(inspectorWindow, true);
        }

        public static void UnlockInspector()
        {
            EditorWindow inspectorWindow = EditorWindow.GetWindow(InspectorType);
            InspectorLockProperty.SetValue(inspectorWindow, false);
        }

        public static void SetSceneViewToOrthoBackView()
        {
            SceneViewSetBackMethod.Invoke(null, new object[]{new ShortcutArguments()
            {
                context = SceneView.lastActiveSceneView
            }});
        }

        public static void LockSceneViewRotation()
        {
            SceneView.lastActiveSceneView.isRotationLocked = true;
        }

        public static void UnlockSceneViewRotation()
        {
            SceneView.lastActiveSceneView.isRotationLocked = false;
        }

        public static void BlockSceneViewClick()
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }
}
