using ECSlike;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mariolike
{
    public interface IGameEventListener
    {
        void onStageStart(int stage);
        void onStageUpdate(float time);
        void onHostLifeChanged(int value);
        void onHostScoreChanged(int value);
    }

    public class GameManager : IECSWorldEventListener
    {
        public static GameManager Instance { get; } = new GameManager();

        private GameManagerScript m_script = null;

        private int m_stage = 0;
        private StageScript m_stageScript = null;

        private GameStates m_state = GameStates.None;
        private float m_passedTime = 0f;

        private HostData m_hostData = null;
        private Entity m_hostEntity = Entity.Null;

        private List<IGameEventListener> m_listeners = new List<IGameEventListener>();

        public GameManager()
        {
            ECSWorld.Instance.setListener(this);
        }

        public void setup(GameManagerScript script)
        {
            m_script = script;
            if (GlobalStates.IsSceneMode)
            {
                startGameInSceneMode();
            }
        }

        public bool hasScript()
        {
            return m_script;
        }

        public void update()
        {
            if (m_state == GameStates.Running)
            {
                m_passedTime += Time.deltaTime;
                int count = m_listeners.Count;
                for (int i = 0; i < count; i++)
                {
                    m_listeners[i].onStageUpdate(Mathf.Max(0f, m_script.world.stageTime - m_passedTime));
                }
                if (m_passedTime >= m_script.world.stageTime)
                {
                    handleEvent(EventTypes.StageFail);
                }
            }
        }

        //

        public void addListener(IGameEventListener value)
        {
            if (m_listeners.Contains(value)) return;
            m_listeners.Add(value);
            if (m_stage > 0)
            {
                value.onStageStart(m_stage);
                value.onHostLifeChanged(m_hostData.life);
                value.onHostScoreChanged(m_hostData.score);
            }
        }

        public void removeListener(IGameEventListener value)
        {
            m_listeners.Remove(value);
        }

        //

        public void registerStage(StageScript stageScript)
        {
            m_stageScript = stageScript;
            createStage(m_stageScript);
            createHost(m_stageScript);
        }

        public void registerHost(Entity entity)
        {
            if (m_hostEntity.Equals(entity)) return;
            m_hostEntity = entity;
            if (!m_hostEntity.isNull())
            {
                m_state = GameStates.Running;
            }
        }

        public GameStates getState()
        {
            return m_state;
        }

        public void handleEvent(EventTypes evt)
        {
            switch (evt)
            {
                case EventTypes.StageClear:
                    {
                        if (m_state != GameStates.Running) return;
                        m_state = GameStates.End;
                        onSaveHostData();
                        onGameWin();
                    }
                    break;
                case EventTypes.StageFail:
                    {
                        if (m_state != GameStates.Running) return;
                        m_state = GameStates.End;
                        onGameLose();
                    }
                    break;
                case EventTypes.KillHost:
                    {
                        if (m_state != GameStates.Running) return;
                        killHost();
                    }
                    break;
            }
        }

        //

        public void startGame()
        {
            m_stage = 1;
            m_hostData = createHostData();
            onStartGame();
        }

        public void startGameInSceneMode()
        {
            m_stage = 1;
            m_hostData = createHostData();
        }

        protected void onStartGame()
        {
            UIManager.Instance.showLoadingUI(() =>
            {
                UIManager.Instance.hideEntryUI();
                LoadManager.Instance.loadSceneAsync(string.Format(m_script.stageResourceFormat, m_stage), (Scene scene) =>
                {
                    UIManager.Instance.showStageUI(() =>
                    {
                        onInitGame();
                        UIManager.Instance.hideLoadingUI();
                    });
                });
            });
        }

        protected void onInitGame()
        {
            m_passedTime = 0f;
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                m_listeners[i].onStageStart(m_stage);
                m_listeners[i].onStageUpdate(Mathf.Max(0f, m_script.world.stageTime - m_passedTime));
                m_listeners[i].onHostLifeChanged(m_hostData.life);
                m_listeners[i].onHostScoreChanged(m_hostData.score);
            }
        }

        public void nextGame()
        {
            onNextGame();
        }

        protected void onNextGame()
        {
            m_stage++;
            onStartGame();
        }

        public void exitGame()
        {
            onExitGame();
        }

        protected void onExitGame()
        {
            UIManager.Instance.showLoadingUI(() =>
            {
                UIManager.Instance.hideStageUI();
                UIManager.Instance.hideWinUI();
                UIManager.Instance.hideLoseUI();
                LoadManager.Instance.loadSceneAsync(string.Format(m_script.stageResourceFormat, 0), (Scene scene) =>
                {
                    UIManager.Instance.showEntryUI();
                    UIManager.Instance.hideLoadingUI();
                });
            });
        }

        protected void onGameWin()
        {
            if (m_stage >= m_script.maxStage)
            {
                UIManager.Instance.showWinUI();
            }
            else
            {
                nextGame();
            }
        }

        protected void onGameLose()
        {
            UIManager.Instance.showLoseUI();
        }

        //

        public HostData getHostData()
        {
            return m_hostData;
        }

        protected HostData createHostData()
        {
            return new HostData()
            {
                life = m_script.world.hostLife
            };
        }

        protected void onSaveHostData()
        {
            m_hostData.scale = Vector3.one;
            m_hostData.attrs.Clear();
            TransformComponent tranformComponent = m_hostEntity.getComponent<TransformComponent>();
            if (tranformComponent != null)
            {
                m_hostData.scale = tranformComponent.transform.localScale;
            }
            AttrComponent attrComponent = m_hostEntity.getComponent<AttrComponent>();
            foreach (KeyValuePair<AttrTypes, int> kv in attrComponent.mAttrs)
            {
                if (kv.Key == AttrTypes.MaxHP) continue;
                m_hostData.attrs.Add(kv.Key, kv.Value);
            }
            m_hostData.time += m_passedTime;
        }

        protected void onLoadHostData()
        {
            TransformComponent tranformComponent = m_hostEntity.getComponent<TransformComponent>();
            if (tranformComponent != null)
            {
                tranformComponent.transform.localScale = m_hostData.scale;
            }
            AttrComponent attrComponent = m_hostEntity.getComponent<AttrComponent>();
            foreach (KeyValuePair<AttrTypes, int> kv in m_hostData.attrs)
            {
                attrComponent.mAttrs[kv.Key] = kv.Value;
            }
        }

        protected void createHost(StageScript stageScript)
        {
            if (stageScript == null) return;
            LoadManager.Instance.loadAssetAsync<GameObject>(m_script.playerResource, (GameObject go) =>
            {
                GameObject inst = GameObject.Instantiate(go);
                inst.name = "Player";
                inst.transform.position = Vector3.zero;
                inst.transform.eulerAngles = Vector3.zero;
                inst.transform.localScale = Vector3.one;
                if (stageScript.born != null)
                {
                    inst.transform.position = stageScript.born;
                }
                if (stageScript.cameraScript != null)
                {
                    stageScript.cameraScript.followTarget = inst.transform;
                }
            });
        }

        protected void killHost()
        {
            if (m_hostEntity.isNull()) return;
            DeathComponent deathComponent = m_hostEntity.getComponent<DeathComponent>();
            if (deathComponent != null)
            {
                deathComponent.startDeath(Entity.Null);
            }
        }

        protected void createStage(StageScript stageScript)
        {
            if (stageScript == null) return;
            createDeathHole(stageScript);
        }

        protected void createDeathHole(StageScript stageScript)
        {
            if (stageScript == null) return;
            LoadManager.Instance.loadAssetAsync<GameObject>(m_script.deathHoleResource, (GameObject go) =>
            {
                GameObject inst = GameObject.Instantiate(go);
                inst.name = "__death_hole__";
                inst.transform.position = new Vector3(0f, -stageScript.cellSize * (stageScript.row / 2 + 1), 0f);
                inst.transform.eulerAngles = Vector3.zero;
                inst.transform.localScale = new Vector3(stageScript.cellSize * (stageScript.column + 2), 1f, 1f);
            });
        }

        // IECSWorldEventListener

        public void onHostInit(Entity entity)
        {
            registerHost(entity);
            onLoadHostData();
        }

        public void onHostAttrChanged(AttrTypes type, int value)
        {
            switch (type)
            {
                case AttrTypes.Score:
                    {
                        m_hostData.score += value;
                        notifyHostScoreChanged(m_hostData.score);
                    }
                    break;
            }
        }

        public void onHostDead()
        {
            m_hostData.life--;
            m_hostData.scale = Vector3.one;
            m_hostData.attrs.Clear();
            notifyHostLifeChanged(m_hostData.life);
            if (m_hostData.life > 0)
            {
                createHost(m_stageScript);
            }
            else
            {
                handleEvent(EventTypes.StageFail);
            }
        }

        public void onWorldEvent(EventTypes type)
        {
            handleEvent(type);
        }

        //

        protected void notifyHostLifeChanged(int value)
        {
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                m_listeners[i].onHostLifeChanged(value);
            }
        }

        protected void notifyHostScoreChanged(int value)
        {
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                m_listeners[i].onHostScoreChanged(value);
            }
        }
    }
}
