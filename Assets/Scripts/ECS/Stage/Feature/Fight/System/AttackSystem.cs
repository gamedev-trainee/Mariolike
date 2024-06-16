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

        protected override void onUpdateEntity(World world, int entity)
        {
            DeathComponent deathComponent = world.getComponent<DeathComponent>(entity);
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            AttackComponent attackComponent = world.getComponent<AttackComponent>(entity);
            PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
            TransformComponent transformComponent = world.getComponent<TransformComponent>(entity);
            Vector3 scale = transformComponent != null ? transformComponent.transform.localScale : Vector3.one;
            GroupComponent groupComponent = world.getComponent<GroupComponent>(entity);
            FightGroups group = groupComponent != null ? groupComponent.group : FightGroups.None;
            int count = attackComponent.attackClips.Count;
            for (int i = 0; i < count; i++)
            {
                onUpdateEntityAttackRange(world, entity, group, attackComponent.attackClips[i], positionComponent, scale);
            }
        }

        protected void onUpdateEntityAttackRange(World world, int entity, FightGroups group, AttackClip attackClip, PositionComponent positionComponent, Vector3 scale)
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
                if (targetScript.entity <= 0 || targetScript.entity == entity) continue;
                targetGroupComponent = world.getComponent<GroupComponent>(targetScript.entity);
                if (!FightUtils.IsAttackableGroup(attackClip.connection, group, targetGroupComponent)) continue;
                if (world.getComponent<DisposeComponent>(entity) != null) continue;
                targetDeathComponent = world.getComponent<DeathComponent>(entity);
                if (targetDeathComponent != null && targetDeathComponent.isDying())
                {
                    continue;
                }
                targetBeattackComponent = world.getComponent<BeattackComponent>(targetScript.entity);
                if (targetBeattackComponent != null && targetBeattackComponent.isBeattacking())
                {
                    continue;
                }
                if (attackClip.targetMotion != null)
                {
                    targetMotionPlayComponent = world.getOrAddComponent<MotionPlayComponent>(targetScript.entity);
                    targetMotionPlayComponent.playMotion(attackClip.targetMotion, entity);
                }
                targetBeattackComponent = world.getOrAddComponent<BeattackComponent>(targetScript.entity);
                targetBeattackClip = FightUtils.FindBeattackClip(world, targetScript.entity, targetBeattackComponent);
                if (targetBeattackClip != null)
                {
                    if (targetBeattackClip.counterMotion != null)
                    {
                        motionPlayComponent = world.getOrAddComponent<MotionPlayComponent>(entity);
                        motionPlayComponent.playMotion(targetBeattackClip.counterMotion, targetScript.entity);
                    }
                }
                targetBeattackComponent.startBeattack(targetBeattackClip, entity);
            }
        }
    }
}
