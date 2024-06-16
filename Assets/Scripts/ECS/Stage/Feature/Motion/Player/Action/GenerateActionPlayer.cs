using UnityEngine;

namespace Mariolike
{
    public class GenerateActionPlayer : ActionPlayer
    {
        private GenerateActionClip clip => getData<GenerateActionClip>();

        public override void play()
        {
            base.play();

            TransformComponent transformComponent = getComponent<TransformComponent>();
            if (transformComponent != null)
            {
                GameObject inst = GameObject.Instantiate(clip.src);
                inst.transform.position = transformComponent.transform.position + clip.bornOffset;
                inst.transform.localScale = Vector3.one;
                inst.transform.localRotation = Quaternion.identity;
                if (clip.startMotion != null)
                {
                    EntityScript entityScript = inst.GetComponent<EntityScript>();
                    if (entityScript != null)
                    {
                        int entityID = ECSWorld.Instance.createEntityBy(entityScript);
                        MotionPlayComponent motionPlayComponent = ECSWorld.Instance.getOrAddComponent<MotionPlayComponent>(entityID);
                        motionPlayComponent.playMotion(clip.startMotion, getTrigger());
                    }
                }
            }
            setDone();
        }
    }
}
