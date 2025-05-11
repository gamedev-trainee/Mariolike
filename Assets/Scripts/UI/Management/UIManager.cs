using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class UIManager : IECSWorldUIAgent
    {
        public static UIManager Instance { get; private set; } = new UIManager();

        private UIManagerScript m_script = null;
        private CanvasScaler m_canvasScaler = null;

        private Dictionary<int, Transform> m_locTransforms = new Dictionary<int, Transform>();
        private Dictionary<string, UIObject> m_uiObjects = new Dictionary<string, UIObject>();

        public UIManager()
        {
            ECSWorld.Instance.setUIAgent(this);
        }

        //

        public void setup(UIManagerScript script)
        {
            m_script = script;
            m_canvasScaler = script.canvas.GetComponent<CanvasScaler>();
            showEntryUI();
        }

        //

        public void registerAttrLoc(AttrTypes attrType, Transform transform)
        {
            m_locTransforms[(int)attrType] = transform;
        }

        public void unregisterAttrLoc(AttrTypes attrType)
        {
            m_locTransforms[(int)attrType] = null;
        }

        //

        public Vector3 uiPositionToWorld(Vector3 uiPosition, float z)
        {
            Vector3 screenPosition = uiPosition;
            if (m_canvasScaler.matchWidthOrHeight < 0.5f)
            {
                float resolutionRatio = m_canvasScaler.referenceResolution.y / m_canvasScaler.referenceResolution.x;
                screenPosition.x = m_canvasScaler.referenceResolution.x / Screen.width * screenPosition.x;
                screenPosition.y = m_canvasScaler.referenceResolution.y / (resolutionRatio * Screen.width) * screenPosition.y;
            }
            else
            {
                float resolutionRatio = m_canvasScaler.referenceResolution.x / m_canvasScaler.referenceResolution.y;
                screenPosition.x = m_canvasScaler.referenceResolution.x / (resolutionRatio * Screen.height) * screenPosition.x;
                screenPosition.y = m_canvasScaler.referenceResolution.y / Screen.height * screenPosition.y;
            }
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            return ray.GetPoint(z - ray.origin.z);
        }

        //

        public void showUI(string address, System.Action callback = null)
        {
            if (string.IsNullOrEmpty(address)) return;
            UIObject uiObj;
            if (m_uiObjects.TryGetValue(address, out uiObj))
            {
                uiObj.show(callback);
            }
            else
            {
                LoadManager.Instance.loadAssetAsync<GameObject>(address, (GameObject prefab) =>
                {
                    GameObject go = GameObject.Instantiate(prefab);
                    go.transform.SetParent(m_script.canvas.transform);
                    go.name = prefab.name;
                    go.transform.position = Vector3.zero;
                    go.transform.eulerAngles = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    RectTransform rectTransform = go.transform as RectTransform;
                    rectTransform.offsetMin = Vector3.zero;
                    rectTransform.offsetMax = Vector3.zero;
                    uiObj = new UIObject();
                    uiObj.go = go;
                    uiObj.script = go.GetComponent<IUIViewScript>();
                    m_uiObjects.Add(address, uiObj);
                    uiObj.show(callback);
                });
            }
        }

        public void hideUI(string address, System.Action callback = null, bool destroy = false)
        {
            if (string.IsNullOrEmpty(address)) return;
            UIObject uiObj;
            if (m_uiObjects.TryGetValue(address, out uiObj))
            {
                uiObj.hide(() =>
                {
                    callback?.Invoke();
                    if (destroy)
                    {
                        m_uiObjects.Remove(address);
                        GameObject.Destroy(uiObj.go);
                    }
                });
            }
            else
            {
                callback?.Invoke();
            }
        }

        //

        public void showEntryUI(System.Action callback = null) => showUI(m_script.entryUI, callback);
        public void hideEntryUI(System.Action callback = null) => hideUI(m_script.entryUI, callback);

        public void showLoadingUI(System.Action callback = null) => showUI(m_script.loadingUI, callback);
        public void hideLoadingUI(System.Action callback = null) => hideUI(m_script.loadingUI, callback);

        public void showStageUI(System.Action callback = null) => showUI(m_script.stageUI, callback);
        public void hideStageUI(System.Action callback = null) => hideUI(m_script.stageUI, callback);

        public void showWinUI(System.Action callback = null) => showUI(m_script.winUI, callback);
        public void hideWinUI(System.Action callback = null) => hideUI(m_script.winUI, callback);

        public void showLoseUI(System.Action callback = null) => showUI(m_script.loseUI, callback);
        public void hideLoseUI(System.Action callback = null) => hideUI(m_script.loseUI, callback);

        // IECSWorldUIAgent

        public Vector3 getUIAttrWorldPosition(AttrTypes attrType, float z)
        {
            int loc = (int)attrType;
            if (m_locTransforms.ContainsKey(loc))
            {
                return uiPositionToWorld(m_locTransforms[loc].TransformPoint(Vector3.zero), z);
            }
            return Vector3.zero;
        }
    }
}
