using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public class StageScript : MonoBehaviour
    {
        [Header("[Runtime]")]

        public CameraScript cameraScript = null;
        public Vector3 born = Vector3.zero;

        [Header("[Edit]")]

        public float cellSize = 1f;
        public int column = 60;
        public int row = 12;

        private bool m_registerDelay = false;

        private void Start()
        {
#if UNITY_EDITOR
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
            if (m_registerDelay)
            {
                m_registerDelay = false;
                GameManager.Instance.registerStage(this);
            }
        }
#endif
    }
}
