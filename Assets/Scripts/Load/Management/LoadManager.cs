using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mariolike
{
    public class LoadManager
    {
        public static LoadManager Instance { get; private set; } = new LoadManager();

        private Dictionary<string, ILoader> m_loaders = new Dictionary<string, ILoader>();
        private List<ILoader> m_loadingLoaders = new List<ILoader>();

        public void loadAssetAsync<T>(string address, System.Action<T> callback = null) where T : Object
        {
            ILoader loader;
            if (m_loaders.TryGetValue(address, out loader))
            {
                ILoader<T> tLoader = loader as ILoader<T>;
                if (loader.isDone())
                {
                    callback?.Invoke(tLoader.getAsset());
                }
                else
                {
                    tLoader.addCallback(callback);
                }
                if (m_loadingLoaders.IndexOf(tLoader) == -1)
                {
                    loader.load();
                    m_loadingLoaders.Add(loader);
                }
            }
            else
            {
                ResourcesLoader<T> resourcesLoader = new ResourcesLoader<T>();
                resourcesLoader.addCallback(callback);
                resourcesLoader.setAddress(address);
                resourcesLoader.load();
                m_loaders[address] = resourcesLoader;
                m_loadingLoaders.Add(resourcesLoader);
            }
        }

        public void loadSceneAsync(string address, System.Action<Scene> callback = null)
        {
            ILoader loader;
            if (m_loaders.TryGetValue(address, out loader))
            {
                ISceneLoader tLoader = loader as ISceneLoader;
                if (loader.isDone())
                {
                    callback?.Invoke(tLoader.getScene());
                }
                else
                {
                    tLoader.addCallback(callback);
                }
                if (m_loadingLoaders.IndexOf(tLoader) == -1)
                {
                    loader.load();
                    m_loadingLoaders.Add(loader);
                }
            }
            else
            {
                SceneLoader sceneLoader = new SceneLoader();
                sceneLoader.addCallback(callback);
                sceneLoader.setAddress(address);
                sceneLoader.load();
                m_loaders[address] = sceneLoader;
                m_loadingLoaders.Add(sceneLoader);
            }
        }

        public void update()
        {
            if (m_loadingLoaders.Count > 0)
            {
                ILoader loader;
                int count = m_loadingLoaders.Count;
                for (int i = 0; i < count; i++)
                {
                    loader = m_loadingLoaders[i];
                    loader.update();
                    if (loader.isDone())
                    {
                        m_loadingLoaders.RemoveAt(i);
                        i--;
                        count--;
                        loader.invoke();
                        if (loader is ISceneLoader)
                        {
                            (loader as ISceneLoader).reset();
                        }
                    }
                }
            }
        }
    }
}
