using UnityEngine;

namespace Mariolike
{
    public class CameraScript : MonoBehaviour
    {
        public Transform followTarget = null;
        public float followDistance = 0f;
        public Vector3 followOffset = Vector3.zero;

        void LateUpdate()
        {
            if (followTarget == null)
            {
                return;
            }

            Vector3 pos;
            float cameraAngle = transform.eulerAngles.x;
            if (cameraAngle == 0f)
            {
                pos = followTarget.position + new Vector3(0f, 0f, -followDistance);
            }
            else
            {
                float cameraAngleR = Mathf.Deg2Rad * cameraAngle;
                float followZ = Mathf.Acos(cameraAngleR) * followDistance;
                float followY = Mathf.Asin(cameraAngleR) * followDistance;
                pos = followTarget.position + new Vector3(0f, followY, -followZ);
            }
            pos += followOffset;
            transform.position = pos;
        }
    }
}
