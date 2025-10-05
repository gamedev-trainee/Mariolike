using UnityEngine;

namespace Mariolike
{
    [ExecuteAlways]
    public class StageScript : MonoBehaviour
    {
        [Header("[Camera]")]

        [HideInInspector]
        public CameraScript cameraScript = null;
        [HideInInspector]
        public Vector4 cameraRectPadding = Vector4.zero;

        [Header("[Born]")]

        public Vector3 born = Vector3.zero;

        [Header("[Stage]")]

        public float cellSize = 1f;
        public int column = 60;
        public int row = 12;

        private bool m_registerDelay = false;

#if UNITY_EDITOR

        private Transform m_bornPreview = null;
        private Vector4 m_lastCameraRectPadding = Vector4.zero;

        private void Awake()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
            {
                return;
            }
            if (!Application.isPlaying)
            {
                if (cameraScript == null)
                {
                    Camera mainCamera = GameObject.FindAnyObjectByType<Camera>();
                    if (mainCamera != null)
                    {
                        GameObject.DestroyImmediate(mainCamera.gameObject);
                    }
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Common/Camera.prefab");
                    if (prefab != null)
                    {
                        GameObject go = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        go.name = "__camera__";
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = Vector3.one;
                        cameraScript = go.GetComponentInChildren<CameraScript>();
                        UnityEditor.EditorUtility.SetDirty(this);
                    }
                }
                if (m_bornPreview == null)
                {
                    m_bornPreview = transform.Find("__born_preview__");
                    if (m_bornPreview == null)
                    {
                        GameObject go = new GameObject("__born_preview__");
                        go.transform.SetParent(transform);
                        go.hideFlags = HideFlags.DontSave | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.SetParent(go.transform);
                        cube.transform.localScale = new Vector3(0.1f, 0.4f, 0.1f);
                        cube.transform.localPosition = new Vector3(0f, 0.2f, 0f);
                        cube.transform.localRotation = Quaternion.identity;
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.SetParent(go.transform);
                        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        sphere.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                        sphere.transform.localRotation = Quaternion.identity;
                        m_bornPreview = go.transform;
                    }
                }
            }
        }
#endif

        private void Start()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
            {
                return;
            }
            if (!Application.isPlaying)
            {
                return;
            }
            if (!GameManager.Instance.hasScript())
            {
                GlobalStates.IsSceneMode = true;
                GameObject prefab = Resources.Load("Common/Entry") as GameObject;
                if (prefab != null)
                {
                    GameObject go = GameObject.Instantiate(prefab);
                    go.name = "__entry__";
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                    m_registerDelay = true;
                }
                return;
            }
#endif
            GameManager.Instance.registerStage(this);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
            {
                return;
            }
            if (!Application.isPlaying)
            {
                updateCameraFollow();
                updateCameraFollowRect();
                if (m_bornPreview != null)
                {
                    m_bornPreview.position = born;
                }
                return;
            }
            if (m_registerDelay)
            {
                m_registerDelay = false;
                GameManager.Instance.registerStage(this);
            }
        }

        protected void updateCameraFollow()
        {
            if (cameraScript == null) return;
            cameraScript.updateFollow(born);
        }

        protected void updateCameraFollowRect()
        {
            if (cameraScript == null) return;
            if (cameraScript.followRect == null) return;
            if (m_lastCameraRectPadding.Equals(cameraRectPadding)) return;
            m_lastCameraRectPadding = cameraRectPadding;
            float xOffset = cameraRectPadding.x - cameraRectPadding.z;
            float yOffset = cameraRectPadding.y - cameraRectPadding.w;
            float horizonPadding = cameraRectPadding.x + cameraRectPadding.z;
            float verticalPadding = cameraRectPadding.y + cameraRectPadding.w;
            cameraScript.followRect.transform.localPosition = new Vector3(xOffset * 0.5f, yOffset * 0.5f, 0f);
            cameraScript.followRect.transform.localScale = new Vector3(cellSize * column - horizonPadding, cellSize * row - verticalPadding, 1f);
            UnityEditor.EditorUtility.SetDirty(cameraScript.followRect);
        }
#endif
    }
}
