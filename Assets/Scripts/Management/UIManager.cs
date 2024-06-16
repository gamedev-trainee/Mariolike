using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class UIManager : MonoBehaviour, IECSWorldUIAgent
    {
        public static UIManager Instance { get; private set; }

        public Canvas canvas = null;
        public CanvasScaler canvasScaler = null;

        public List<Transform> attrLocs = new List<Transform>();

        private void Start()
        {
            Instance = this;

            if (canvas == null) canvas = GetComponent<Canvas>();
            if (canvasScaler == null) canvasScaler = GetComponent<CanvasScaler>();

            ECSWorld.Instance.setUIAgent(this);
        }

        public Vector3 uiPositionToWorld(Vector3 uiPosition, float z)
        {
            Vector3 screenPosition = uiPosition;
            if (canvasScaler.matchWidthOrHeight < 0.5f)
            {
                float resolutionRatio = canvasScaler.referenceResolution.y / canvasScaler.referenceResolution.x;
                screenPosition.x = canvasScaler.referenceResolution.x / Screen.width * screenPosition.x;
                screenPosition.y = canvasScaler.referenceResolution.y / (resolutionRatio * Screen.width) * screenPosition.y;
            }
            else
            {
                float resolutionRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
                screenPosition.x = canvasScaler.referenceResolution.x / (resolutionRatio * Screen.height) * screenPosition.x;
                screenPosition.y = canvasScaler.referenceResolution.y / Screen.height * screenPosition.y;
            }
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            return ray.GetPoint(z - ray.origin.z);
        }

        // IECSWorldUIAgent

        public Vector3 getUIAttrWorldPosition(AttrTypes attrType, float z)
        {
            int index = (int)attrType;
            if (index >= 0 && index < attrLocs.Count)
            {
                return uiPositionToWorld(attrLocs[index].TransformPoint(Vector3.zero), z);
            }
            return Vector3.zero;
        }
    }
}
