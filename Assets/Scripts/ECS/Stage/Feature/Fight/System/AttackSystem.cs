using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class AttackSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(AttackComponent),
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
            DeathComponent deathComponent = entity.getComponent<DeathComponent>();
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            AttackComponent attackComponent = entity.getComponent<AttackComponent>();
            PositionComponent positionComponent = entity.getComponent<PositionComponent>();
            TransformComponent transformComponent = entity.getComponent<TransformComponent>();
            Vector3 scale = transformComponent != null ? transformComponent.transform.localScale : Vector3.one;
            GroupComponent groupComponent = entity.getComponent<GroupComponent>();
            FightGroups group = groupComponent != null ? groupComponent.group : FightGroups.None;
            int count = attackComponent.attackClips.Count;
            for (int i = 0; i < count; i++)
            {
                onUpdateEntityAttackRange(entity, group, attackComponent.attackClips[i], positionComponent, scale);
            }
        }

        protected void onUpdateEntityAttackRange(Entity entity, FightGroups group, AttackClip attackClip, PositionComponent positionComponent, Vector3 scale)
        {
            Vector3 pos = positionComponent.position;
            Vector3 offset = attackClip.rangeOffset;
            Vector3 size = attackClip.rangeSize;
            offset.x *= scale.x;
            offset.y *= scale.y;
            offset.z *= scale.z;
            size.x *= scale.x;
            size.y *= scale.y;
            size.z *= scale.z;
            Collider[] colliders = Physics.OverlapBox(pos + offset, size * 0.5f, Quaternion.identity, attackClip.layerMask);
            if (colliders == null || colliders.Length <= 0) return;
            EntityScript targetScript;
            GroupComponent targetGroupComponent;
            DeathComponent targetDeathComponent;
            BeattackComponent targetBeattackComponent;
            MotionPlayComponent targetMotionPlayComponent;
            BeattackClip targetBeattackClip;
            MotionPlayComponent motionPlayComponent;
            int count = colliders.Length;
            for (int i = 0; i < count; i++)
            {
                targetScript = colliders[i].GetComponentInParent<EntityScript>();
                if (targetScript == null) continue;
                if (targetScript.entity.isNull() || targetScript.entity.Equals(entity)) continue;
                targetGroupComponent = targetScript.entity.getComponent<GroupComponent>();
                if (!FightUtils.IsAttackableGroup(attackClip.connection, group, targetGroupComponent)) continue;
                if (entity.getComponent<DisposeComponent>() != null) continue;
                targetDeathComponent = entity.getComponent<DeathComponent>();
                if (targetDeathComponent != null && targetDeathComponent.isDying())
                {
                    continue;
                }
                targetBeattackComponent = targetScript.entity.getComponent<BeattackComponent>();
                if (targetBeattackComponent != null && targetBeattackComponent.isBeattacking())
                {
                    continue;
                }
                if (attackClip.targetMotion != null)
                {
                    targetMotionPlayComponent = targetScript.entity.getOrAddComponent<MotionPlayComponent>();
                    targetMotionPlayComponent.playMotion(attackClip.targetMotion, entity);
                }
                targetBeattackComponent = targetScript.entity.getOrAddComponent<BeattackComponent>();
                targetBeattackClip = FightUtils.FindBeattackClip(targetScript.entity, targetBeattackComponent);
                if (targetBeattackClip != null)
                {
                    if (targetBeattackClip.counterMotion != null)
                    {
                        motionPlayComponent = entity.getOrAddComponent<MotionPlayComponent>();
                        motionPlayComponent.playMotion(targetBeattackClip.counterMotion, targetScript.entity);
                    }
                }
                targetBeattackComponent.startBeattack(targetBeattackClip, entity);
            }
        }
    }
}
