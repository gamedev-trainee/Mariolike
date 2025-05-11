using UnityEngine;

namespace Mariolike
{
    public class UIObject
    {
        public IUIViewScript script = null;
        public GameObject go = null;

        public void show(System.Action callback = null)
        {
            if (script != null)
            {
                script.show(callback);
            }
            else
            {
                go.SetActive(true);
                callback?.Invoke();
            }
        }

        public void hide(System.Action callback = null)
        {
            if (script != null)
            {
                script.hide(callback);
            }
            else
            {
                go.SetActive(false);
                callback?.Invoke();
            }
        }
    }
}
