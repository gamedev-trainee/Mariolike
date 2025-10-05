using UnityEngine;

namespace Mariolike
{
    public class CameraScript : MonoBehaviour
    {
        public Transform followTarget = null;
        public float followDistance = 0f;
        public Vector3 followOffset = Vector3.zero;
        public BoxCollider followRect = null;

        void LateUpdate()
        {
            if (followTarget == null)
            {
                return;
            }

            if (followRect != null)
            {
                if (!followRect.bounds.Contains(followTarget.position))
                {
                    return;
                }
            }
            updateFollow(followTarget.position);
        }

        private void OnDrawGizmos()
        {
            if (followRect == null) return;
            Color color = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(followRect.transform.position + followRect.center, new Vector3(
                followRect.transform.localScale.x * followRect.size.x,
                followRect.transform.localScale.y * followRect.size.y,
                followRect.transform.localScale.z * followRect.size.z));
            Gizmos.color = color;
        }

        public void updateFollow(Vector3 position)
        {
            Vector3 pos;
            float cameraAngle = transform.eulerAngles.x;
            if (cameraAngle == 0f)
            {
                pos = position + new Vector3(0f, 0f, -followDistance);
            }
            else
            {
                float cameraAngleR = Mathf.Deg2Rad * cameraAngle;
                float followZ = Mathf.Acos(cameraAngleR) * followDistance;
                float followY = Mathf.Asin(cameraAngleR) * followDistance;
                pos = position + new Vector3(0f, followY, -followZ);
            }
            pos += followOffset;
            transform.position = pos;
        }
    }
}
