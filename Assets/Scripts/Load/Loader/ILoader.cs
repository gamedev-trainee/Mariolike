using UnityEngine.SceneManagement;

namespace Mariolike
{
    public interface ILoader
    {
        void setAddress(string value);
        void load();
        void update();
        void invoke();
        bool isDone();
    }

    public interface ILoader<T> : ILoader
    {
        void addCallback(System.Action<T> callback);
        T getAsset();
    }

    public interface ISceneLoader : ILoader
    {
        void addCallback(System.Action<Scene> callback);
        Scene getScene();
        void reset();
    }
}
