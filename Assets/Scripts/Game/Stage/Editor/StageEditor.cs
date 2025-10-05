using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Mariolike
{
    [CustomEditor(typeof(StageScript))]
    public class StageEditor : Editor
    {
        [MenuItem("GameObject/Mariolike/Create Stage")]
        static void Create()
        {
            StageScript stageScript = GameObject.FindAnyObjectByType<StageScript>();
            if (stageScript != null)
            {
                Selection.activeGameObject = stageScript.gameObject;
            }
            else
            {
                GameObject go = new GameObject("__stage__");
                go.AddComponent<StageScript>();
            }
        }

        public enum EditTypes
        {
            Paint,
            Erase,
        }

        public enum CellTypes
        {
            None,

        }

        public class CellData
        {
            public Vector2Int indexes = Vector2Int.zero;
            public Vector3 center = Vector3.zero;
            public Vector3 top = Vector3.zero;
            public Vector3 bottom = Vector3.zero;
            public List<GameObject> objects = new List<GameObject>();
            public List<string> categories = new List<string>();

            public void addObject(GameObject go, string category)
            {
                if (!objects.Contains(go))
                {
                    objects.Add(go);
                }
                if (!categories.Contains(category))
                {
                    categories.Add(category);
                }
            }
        }

        public static readonly Color GridColor = new Color(0f, 1f, 0f, 0.2f);
        public static GUIContent PaintIcon;
        public static GUIContent EraseIcon;
        public static GUIContent[] PaintToolBar;

        public static readonly string ResourcePoolRoot = "Assets/Resources/Objects/";

        private StageScript m_target = null;

        private bool m_editing = false;

        private int m_selectedEditToolIndex = -1;

        private GameObject[][] m_resourcePool = null;
        private string[] m_resourceCategories = null;
        private Dictionary<string, string> m_resourceToCategory = new Dictionary<string, string>();
        private int m_selectedResourceCategoryIndex = -1;
        private string m_selectedResourceCategory = string.Empty;
        private GameObject[] m_selectedResourceGoes = null;
        private GameObject m_selectedResourceGo = null;

        private Dictionary<Vector2Int, CellData> m_mapCells = new Dictionary<Vector2Int, CellData>();

        private GenericMenu m_rightMenu = null;
        private CellData m_rightSelectedCellData = null;

        private SerializedProperty m_cameraProperty = null;
        private SerializedProperty m_cameraRectPaddingProperty = null;
        private SerializedObject m_cameraPropertyObject = null;
        private SerializedProperty m_cameraFollowDistanceProperty = null;
        private SerializedProperty m_cameraFollowOffsetProperty = null;

        private void OnEnable()
        {
            m_target = target as StageScript;

            PaintIcon = EditorGUIUtility.TrIconContent("ClothInspector.PaintValue", "");
            EraseIcon = EditorGUIUtility.TrIconContent("TreeEditor.Trash", "");
            PaintToolBar = new GUIContent[]
            {
                PaintIcon,
                EraseIcon,
            };

            initResourcePool();
            initRightMenu();
            initCamera();
        }

        private void OnDisable()
        {
            stopEdit();
            m_target = null;
        }

        private void OnSceneGUI()
        {
            if (!m_editing)
            {
                return;
            }
            EditorUtils.BlockSceneViewClick();
            drawGrids(m_target.column, m_target.row, m_target.cellSize);
            drawBorn(m_target.born);
            Handles.BeginGUI();
            {
                Rect rect = SceneView.lastActiveSceneView.position;
                rect.x = rect.width / 2 - 120;
                rect.y = 10;
                rect.width = 120;
                rect.height = 30;
                Color color = GUI.color;
                GUI.color = Color.red;
                if (GUI.Button(rect, "Exit Edit Mode"))
                {
                    EditorApplication.delayCall += stopEdit;
                }
                GUI.color = color;
            }
            Handles.EndGUI();
            if (Event.current.button == 1)
            {
                // ÓÒ¼üµã»÷
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Vector3 pos = ray.origin;
                Vector2Int indexes = getIndexesByWorldPosition(pos);
                if (m_rightSelectedCellData != null && m_rightSelectedCellData.indexes.Equals(indexes))
                {
                    return;
                }
                m_rightSelectedCellData = getMapCellData(indexes, true);
                m_rightMenu.ShowAsContext();
                return;
            }
            if (Event.current.button != 0)
            {
                return;
            }
            switch ((EditTypes)m_selectedEditToolIndex)
            {
                case EditTypes.Paint:
                    {
                        if (m_selectedResourceGo == null) return;
                        bool canPaint = false;
                        if (Event.current.type == EventType.MouseDown)
                        {
                            canPaint = true;
                        }
                        else if (Event.current.type == EventType.MouseDrag)
                        {
                            canPaint = true;
                        }
                        if (canPaint)
                        {
                            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                            Vector3 pos = ray.origin;
                            Vector2Int indexes = getIndexesByWorldPosition(pos);
                            CellData cellData = getMapCellData(indexes, true);
                            if (cellData.objects.Count > 0)
                            {
                                return;
                            }
                            GameObject inst = PrefabUtility.InstantiatePrefab(m_selectedResourceGo) as GameObject;
                            inst.transform.SetParent(m_target.transform);
                            inst.transform.position = cellData.bottom;
                            inst.transform.localRotation = Quaternion.identity;
                            inst.transform.localScale = Vector3.one;
                            cellData.addObject(inst, m_selectedResourceCategory);
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }
                    break;
                case EditTypes.Erase:
                    {
                        bool canErase = false;
                        if (Event.current.type == EventType.MouseDown)
                        {
                            canErase = true;
                        }
                        else if (Event.current.type == EventType.MouseDrag)
                        {
                            canErase = true;
                        }
                        if (canErase)
                        {
                            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                            Vector3 pos = ray.origin;
                            Vector2Int indexes = getIndexesByWorldPosition(pos);
                            CellData cellData = getMapCellData(indexes);
                            if (cellData != null)
                            {
                                int count = cellData.objects.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    GameObject.DestroyImmediate(cellData.objects[i]);
                                }
                                cellData.objects.Clear();
                                cellData.categories.Clear();
                            }
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }
                    break;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            drawCamera();

            EditorGUILayout.Space();

            Color color = GUI.color;
            if (m_editing)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Exit Edit Mode"))
                {
                    stopEdit();
                }
            }
            else
            {
                if (GUILayout.Button("Enter Edit Mode"))
                {
                    startEdit();
                }
            }
            GUI.color = color;
            if (m_editing)
            {
                m_selectedEditToolIndex = GUILayout.Toolbar(m_selectedEditToolIndex, PaintToolBar);
                switch ((EditTypes)m_selectedEditToolIndex)
                {
                    case EditTypes.Paint:
                        {
                            drawResourcePoolMenu();
                        }
                        break;
                }
            }
            else
            {
                GUILayout.Toolbar(m_selectedEditToolIndex, PaintToolBar);
            }
        }

        private void startEdit()
        {
            if (m_editing) return;
            m_editing = true;
            EditorUtils.SetSceneViewToOrthoBackView();
            EditorUtils.LockSceneViewRotation();
            SceneView.lastActiveSceneView.LookAt(m_target.transform.position);
            SceneView.lastActiveSceneView.size = 5;
            EditorApplication.delayCall += EditorUtils.LockInspector;
            Tools.current = Tool.None;
            m_selectedEditToolIndex = (int)EditTypes.Paint;
            initResourcePoolMenu();
            initMap();
            Repaint();
            SceneView.RepaintAll();
        }

        private void stopEdit()
        {
            if (!m_editing) return;
            m_editing = false;
            EditorUtils.UnlockInspector();
            EditorUtils.UnlockSceneViewRotation();
            m_selectedEditToolIndex = -1;
            m_selectedResourceCategoryIndex = -1;
            m_selectedResourceCategory = string.Empty;
            m_selectedResourceGoes = null;
            m_selectedResourceGo = null;
            uninitMap();
            Repaint();
            SceneView.RepaintAll();
        }

        private void drawGrids(int column, int row, float cellSize)
        {
            Color cacheColor = Handles.color;
            Handles.color = GridColor;
            Vector3 gridSize = new Vector3(cellSize, cellSize, 1f);
            int halfRow = row / 2;
            for (int rowIndex = 0; rowIndex < halfRow; rowIndex++)
            {
                if (rowIndex == 0)
                {
                    drawGridRow(rowIndex, column, gridSize);
                }
                drawGridRow(rowIndex + 1, column, gridSize);
                if (rowIndex == halfRow - 1)
                {
                    if (row % 2 == 0)
                    {
                        continue;
                    }
                }
                drawGridRow(-(rowIndex + 1), column, gridSize);
            }
            Handles.color = cacheColor;
        }

        private void drawGridRow(int rowIndex, int column, Vector3 gridSize)
        {
            Vector3 gridCenter;
            int halfColumn = column / 2;
            for (int columnIndex = 0; columnIndex < halfColumn; columnIndex++)
            {
                if (columnIndex == 0)
                {
                    gridCenter = getWorldPositionByIndexes(new Vector2Int(0, rowIndex));
                    Handles.DrawWireCube(gridCenter, gridSize);
                }
                gridCenter = getWorldPositionByIndexes(new Vector2Int(columnIndex + 1, rowIndex));
                Handles.DrawWireCube(gridCenter, gridSize);
                if (columnIndex == halfColumn - 1)
                {
                    if (column % 2 == 0)
                    {
                        continue;
                    }
                }
                gridCenter = getWorldPositionByIndexes(new Vector2Int(-(columnIndex + 1), rowIndex));
                Handles.DrawWireCube(gridCenter, gridSize);
            }
        }

        protected void initResourcePool()
        {
            GameObject[][] pool = null;
            string[] folders = null;
            string[] dirs = System.IO.Directory.GetDirectories(ResourcePoolRoot, "*", System.IO.SearchOption.TopDirectoryOnly);
            if (dirs != null && dirs.Length > 0)
            {
                GameObject[] goes;
                int count = dirs.Length;
                string category;
                pool = new GameObject[count][];
                folders = new string[count];
                for (int i = 0; i < count; i++)
                {
                    category = System.IO.Path.GetFileName(dirs[i]);
                    goes = initResourcePool(category, dirs[i]);
                    pool[i] = goes;
                    folders[i] = category;
                }
            }
            m_resourcePool = pool;
            m_resourceCategories = folders;
        }

        protected GameObject[] initResourcePool(string category, string dir)
        {
            GameObject[] goes = null;
            string[] guids = AssetDatabase.FindAssets("t:GameObject", new string[] { dir });
            if (guids != null && guids.Length > 0)
            {
                string assetPath;
                GameObject src;
                int count = guids.Length;
                goes = new GameObject[count];
                for (int i = 0; i < count; i++)
                {
                    assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    goes[i] = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    m_resourceToCategory[assetPath] = category;
                    if (PrefabUtility.IsPartOfVariantPrefab(goes[i]))
                    {
                        src = PrefabUtility.GetCorrespondingObjectFromSource(goes[i]);
                        if (src != null)
                        {
                            assetPath = AssetDatabase.GetAssetPath(src);
                            m_resourceToCategory[assetPath] = category;
                        }
                    }
                }
            }
            return goes;
        }

        protected void initResourcePoolMenu()
        {
            m_selectedResourceCategoryIndex = 0;
            m_selectedResourceCategory = m_resourceCategories.Length > 0 ? m_resourceCategories[0] : string.Empty;
            m_selectedResourceGoes = m_resourcePool[0];
            m_selectedResourceGo = null;
        }

        protected void drawResourcePoolMenu()
        {
            int folderIndex = GUILayout.Toolbar(m_selectedResourceCategoryIndex, m_resourceCategories);
            if (m_selectedResourceCategoryIndex != folderIndex)
            {
                m_selectedResourceCategoryIndex = folderIndex;
                m_selectedResourceCategory = m_resourceCategories[m_selectedResourceCategoryIndex];
                m_selectedResourceGoes = m_resourcePool[m_selectedResourceCategoryIndex];
            }
            if (m_selectedResourceGoes != null)
            {
                int count = m_selectedResourceGoes.Length;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.ObjectField(m_selectedResourceGoes[i], typeof(GameObject), false);
                        if (m_selectedResourceGo == m_selectedResourceGoes[i])
                        {
                            Color color = GUI.color;
                            GUI.color = Color.red;
                            if (GUILayout.Button("Unselect"))
                            {
                                m_selectedResourceGo = null;
                            }
                            GUI.color = color;
                        }
                        else
                        {
                            if (GUILayout.Button("Select"))
                            {
                                m_selectedResourceGo = m_selectedResourceGoes[i];
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("empty");
            }
        }

        protected void initMap()
        {
            m_mapCells.Clear();
            GameObject[] rootGoes = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            if (rootGoes != null && rootGoes.Length > 0)
            {
                int count = rootGoes.Length;
                for (int i = 0; i < count; i++)
                {
                    initMapObjectsIn(rootGoes[i].transform);
                }
            }
        }

        protected void initMapObjectsIn(Transform transform)
        {
            Transform childTransform;
            string assetPath;
            string category;
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                childTransform = transform.GetChild(i);
                if (PrefabUtility.IsAnyPrefabInstanceRoot(childTransform.gameObject))
                {
                    assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(childTransform.gameObject);
                    if (m_resourceToCategory.ContainsKey(assetPath))
                    {
                        category = m_resourceToCategory[assetPath];
                        initMapObject(childTransform.gameObject, category);
                    }
                }
                else
                {
                    initMapObjectsIn(childTransform);
                }
            }
        }

        protected void initMapObject(GameObject go, string category)
        {
            Collider collider = go.GetComponent<Collider>();
            Vector3 pos = collider.bounds.center;
            Vector2Int indexes = getIndexesByWorldPosition(pos);
            CellData cellData = getMapCellData(indexes, true);
            cellData.addObject(go, category);
        }

        protected void uninitMap()
        {
            m_mapCells.Clear();
        }

        protected CellData getMapCellData(Vector2Int indexes, bool createIfNone = false)
        {
            if (!m_mapCells.ContainsKey(indexes))
            {
                if (!createIfNone)
                {
                    return null;
                }
                Vector3 position = getWorldPositionByIndexes(indexes);
                m_mapCells[indexes] = new CellData()
                {
                    indexes = indexes,
                    center = position,
                    top = position + new Vector3(0f, m_target.cellSize * 0.5f, 0f),
                    bottom = position - new Vector3(0f, m_target.cellSize * 0.5f, 0f),
                };
            }
            return m_mapCells[indexes];
        }

        protected Vector2Int getIndexesByWorldPosition(Vector3 pos)
        {
            return new Vector2Int()
            {
                x = getIndexByLocalPosition(pos.x - m_target.transform.position.x),
                y = getIndexByLocalPosition(pos.y - m_target.transform.position.y),
            };
        }

        protected int getIndexByLocalPosition(float pos)
        {
            float halfCell = m_target.cellSize * 0.5f;
            if (pos <= halfCell && pos >= -halfCell)
            {
                return 0;
            }
            int index = Mathf.FloorToInt((Mathf.Abs(pos) - halfCell) / m_target.cellSize) + 1;
            if (pos < 0)
            {
                index = -index;
            }
            return index;
        }

        protected Vector3 getWorldPositionByIndexes(Vector2Int indexes)
        {
            return new Vector3()
            {
                x = m_target.transform.position.x + getLocalPositionByIndex(indexes.x),
                y = m_target.transform.position.y + getLocalPositionByIndex(indexes.y),
                z = 0f,
            };
        }

        protected float getLocalPositionByIndex(int index)
        {
            if (index == 0) return 0f;
            if (index < 0)
            {
                return index * m_target.cellSize;
            }
            return index * m_target.cellSize;
        }

        protected void initRightMenu()
        {
            m_rightMenu = new GenericMenu();
            m_rightMenu.AddItem(new GUIContent("Set As Born"), false, onRightMenuSetAsBorn);
        }

        protected void onRightMenuSetAsBorn()
        {
            if (m_rightSelectedCellData != null)
            {
                m_target.born = m_rightSelectedCellData.bottom;
            }
            m_rightSelectedCellData = null;
        }

        protected void drawBorn(Vector3 born)
        {
            Color color = Handles.color;
            Handles.color = Color.red;
            Handles.DrawSolidDisc(born, Vector3.back, 0.2f);
            Handles.color = color;
        }

        private void initCamera()
        {
            m_cameraProperty = serializedObject.FindProperty("cameraScript");
            m_cameraRectPaddingProperty = serializedObject.FindProperty("cameraRectPadding");
            if (m_cameraProperty.objectReferenceValue != null)
            {
                m_cameraPropertyObject = new SerializedObject(m_cameraProperty.objectReferenceValue);
                m_cameraFollowDistanceProperty = m_cameraPropertyObject.FindProperty("followDistance");
                m_cameraFollowOffsetProperty = m_cameraPropertyObject.FindProperty("followOffset");
            }
        }

        private void drawCamera()
        {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_cameraProperty);
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (m_cameraProperty.objectReferenceValue != null)
                {
                    m_cameraPropertyObject = new SerializedObject(m_cameraProperty.objectReferenceValue);
                    m_cameraFollowDistanceProperty = m_cameraPropertyObject.FindProperty("followDistance");
                    m_cameraFollowOffsetProperty = m_cameraPropertyObject.FindProperty("followOffset");
                }
                else
                {
                    m_cameraPropertyObject = null;
                }
            }
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_cameraRectPaddingProperty);
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            if (m_cameraPropertyObject == null)
            {
                return;
            }
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_cameraFollowDistanceProperty);
                EditorGUILayout.PropertyField(m_cameraFollowOffsetProperty);
            }
            if (EditorGUI.EndChangeCheck())
            {
                m_cameraPropertyObject.ApplyModifiedProperties();
            }
        }
    }
}
