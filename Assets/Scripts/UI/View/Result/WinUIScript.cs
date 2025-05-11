using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class WinUIScript : UIViewScript
    {
        public static readonly string RecordsKey = "stage_records";
        public static readonly int MaxRecords = 10;

        public enum States
        {
            Display,
            End,
        }

        public class StageRecordData
        {
            public int score = 0;
            public float time = 0f;
        }

        public Text currentValue = null;
        public Text bestValue = null;

        private StageRecordData m_bestRecord = new StageRecordData();
        private StageRecordData m_currentRecore = new StageRecordData();

        private States m_state = States.Display;

        private void Update()
        {
            switch (m_state)
            {
                case States.Display:
                    {
                        if (Input.anyKeyDown)
                        {
                            finish();
                        }
                    }
                    break;
            }
        }

        protected override void onShow()
        {
            base.onShow();

            m_bestRecord.score = 0;
            m_bestRecord.time = 0f;

            onLoadBestRecord();

            HostData hostData = GameManager.Instance.getHostData();

            m_currentRecore.score = hostData.score;
            m_currentRecore.time = hostData.time;

            if (m_bestRecord.score < m_currentRecore.score)
            {
                m_bestRecord.score = m_currentRecore.score;
                m_bestRecord.time = m_currentRecore.time;
                onSaveBestRecord();
            }
            else if (m_bestRecord.score == m_currentRecore.score)
            {
                if (m_bestRecord.time > m_currentRecore.time)
                {
                    m_bestRecord.time = m_currentRecore.time;
                    onSaveBestRecord();
                }
            }

            currentValue.text = string.Format("Current: {0} ({1}s)", m_currentRecore.score, m_currentRecore.time);
            bestValue.text = string.Format("Best: {0} ({1}s)", m_bestRecord.score, m_bestRecord.time);
        }

        protected override void onHide()
        {
            base.onHide();
        }

        //

        public void onButtonBackClicked()
        {
            finish();
        }

        //

        protected void finish()
        {
            if (m_state != States.Display) return;
            m_state = States.End;
            GameManager.Instance.exitGame();
        }

        //

        protected void onLoadBestRecord()
        {
            if (!PlayerPrefs.HasKey(RecordsKey)) return;
            string content = PlayerPrefs.GetString(RecordsKey);
            content = content.Trim();
            if (string.IsNullOrEmpty(content)) return;
            string[] contentArr = content.Split('|');
            if (contentArr.Length != 3) return;
            int.TryParse(contentArr[0].Trim(), out m_bestRecord.score);
            float.TryParse(contentArr[1].Trim(), out m_bestRecord.time);
        }

        protected void onSaveBestRecord()
        {
            string content = string.Format("{0}|{1}", m_bestRecord.score, m_bestRecord.time);
            PlayerPrefs.SetString(RecordsKey, content);
        }
    }
}
