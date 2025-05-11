using UnityEngine;

namespace Mariolike
{
    public class EntryUIScript : UIViewScript
    {
        public enum States
        {
            Entry,
            Menu,
        }

        public GameObject entryRoot = null;
        public GameObject menuRoot = null;

        private States m_state = States.Entry;

        private void Update()
        {
            switch (m_state)
            {
                case States.Entry:
                    {
                        if (Input.anyKeyDown)
                        {
                            showMenu();
                        }
                    }
                    break;
            }
        }

        public void onButtonAnyKeyClicked()
        {
            showMenu();
        }

        public void onButtonChallengeClicked()
        {
            startChallenge();
        }

        protected void showMenu()
        {
            if (m_state != States.Entry) return;
            m_state = States.Menu;
            entryRoot?.SetActive(false);
            menuRoot?.SetActive(true);
        }

        protected void startChallenge()
        {
            GameManager.Instance.startGame();
        }

        protected override void onShow()
        {
            base.onShow();

            m_state = States.Entry;
            entryRoot?.SetActive(true);
            menuRoot?.SetActive(false);
        }

        protected override void onHide()
        {
            base.onHide();
        }
    }
}
