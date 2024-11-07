using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestSystem : EntitySystem
    {
        public static readonly float MinCheckDistance = 0.001f;

        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(HitTestComponent),
                typeof(PositionComponent),
            };
        }

        protected override System.Type[] getUnwantedTypes()
        {
            return new System.Type[]
            {
                typeof(DisposeComponent),
            };
        }

        protected override void onUpdateEntity(Entity entity)
        {
            HitTestComponent hitTestComponent = entity.getComponent<HitTestComponent>();
            PositionComponent positionComponent = entity.getComponent<PositionComponent>();

            hitTestComponent.mLastHitFlags = hitTestComponent.mHitFlags;
            hitTestComponent.mHitFlags = HitTestFlags.None;

            Vector3 lastPosition = positionComponent.position;
            bool isOnGround = (hitTestComponent.mLastHitFlags & HitTestFlags.HitGround) == HitTestFlags.HitGround;

            if (positionComponent.offset.x != 0f)
            {
                Vector3 curPosition = lastPosition;
                if (positionComponent.offset.y > 0)
                {
                    // 像上的Y轴移动不会计算地面碰撞，必然会更新，所以加起来一起计算
                    curPosition.y += positionComponent.offset.y;
                }
                int moveForward = positionComponent.offset.x > 0 ? 1 : -1;
                float checkDistance = Mathf.Abs(positionComponent.offset.x) + hitTestComponent.radius * 2f;
                Vector3 pointBottom = positionComponent.position + new Vector3(0f, hitTestComponent.radius, 0f) + new Vector3(-moveForward * hitTestComponent.radius, 0f, 0f);
                Vector3 pointTop = positionComponent.position + new Vector3(0f, hitTestComponent.height - hitTestComponent.radius, 0f) + new Vector3(-moveForward * hitTestComponent.radius, 0f, 0f);
                RaycastHit[] hits = Physics.CapsuleCastAll(pointBottom, pointTop, hitTestComponent.radius, new Vector3(moveForward, 0f, 0f), checkDistance, hitTestComponent.layerMask);
                if (hits != null && hits.Length > 0)
                {
                    float hitY;
                    int hitCount = 0;
                    float nextX = 0f;
                    int count = hits.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (hits[i].distance <= 0) continue;
                        if (moveForward > 0)
                        {
                            if (hits[i].point.x < curPosition.x) continue;
                            if (hits[i].point.x > curPosition.x + hitTestComponent.radius + positionComponent.offset.x) continue;
                        }
                        else
                        {
                            if (hits[i].point.x > curPosition.x) continue;
                            if (hits[i].point.x < curPosition.x - hitTestComponent.radius + positionComponent.offset.x) continue;
                        }
                        hitY = trimValue(hits[i].point.y);
                        if (isOnGround)
                        {
                            if (hitY <= curPosition.y + hitTestComponent.stepOffset)
                            {
                                // 如果是在地面上，碰撞到的位置的Y轴必须大于台阶高度才算有效
                                continue;
                            }
                        }
                        if (hitCount <= 0)
                        {
                            nextX = hits[i].point.x - hitTestComponent.radius * moveForward;
                        }
                        else
                        {
                            // 选择最近的碰撞位置
                            if (moveForward > 0)
                            {
                                nextX = Mathf.Min(nextX, hits[i].point.x - hitTestComponent.radius * moveForward);
                            }
                            else
                            {
                                nextX = Mathf.Max(nextX, hits[i].point.x - hitTestComponent.radius * moveForward);
                            }
                        }
                        hitCount++;
                    }
                    if (hitCount > 0)
                    {
                        positionComponent.offset.x = nextX - curPosition.x;
                        hitTestComponent.mHitFlags |= HitTestFlags.HitWall;
                    }
                }
            }

            if (positionComponent.offset.y <= 0f)
            {
                Vector3 curPosition = lastPosition + new Vector3(positionComponent.offset.x, 0f, positionComponent.offset.z);
                float startYOffset = Mathf.Max(MinCheckDistance, -positionComponent.offset.y);
                float checkHeight = hitTestComponent.height * 0.5f + startYOffset * 2f;
                RaycastHit[] hits = Physics.BoxCastAll(curPosition + new Vector3(0f, hitTestComponent.height + startYOffset, 0f), new Vector3(hitTestComponent.footRadius, hitTestComponent.height * 0.5f, hitTestComponent.footRadius), Vector3.down, Quaternion.identity, checkHeight, hitTestComponent.layerMask);
                if (hits != null && hits.Length > 0)
                {
                    float minY = curPosition.y;
                    if (isOnGround)
                    {
                        minY += hitTestComponent.stepOffset;
                    }
                    float hitY;
                    float nextY = 0;
                    int hitCount = 0;
                    int count = hits.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (hits[i].distance <= 0) continue;
                        hitY = trimValue(hits[i].point.y);
                        if (hitY > minY) continue;
                        // 选择最高的碰撞位置
                        if (hitCount <= 0)
                        {
                            nextY = hitY;
                        }
                        else
                        {
                            nextY = Mathf.Max(nextY, hitY);
                        }
                        hitCount++;
                    }
                    if (hitCount > 0)
                    {
                        positionComponent.offset.y = nextY - curPosition.y;
                        hitTestComponent.mHitFlags |= HitTestFlags.HitGround;
                    }
                }
            }
        }

        protected float trimValue(float value)
        {
            return Mathf.Round(value * 1000f) * 0.001f;
        }
    }
}
