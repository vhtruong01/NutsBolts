using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class ScrewGameManager : MiniGameManager
    {
        public Camera mainCamera;

        public ThemeData themeData;

        public SpriteRenderer backgroundPrefab;
        public ScrewPlayerInput playerInput;

       

        public override void LoadLevel()
        {
            base.LoadLevel();
            //Test

            
                AudioManager.Instance.PlayMusic(Audio.Music);

        }

        public override void OnWin()
        {
            Config.shouldShowFull = true;
            if (gameState == MiniGameState.Playing)
            {
                gameState = MiniGameState.Win;
                miniGameMaster.levelData.LevelUp();
                levelManager.CloseLevel();

                if (Config.ConfigShowQuestProgress == 1)
                {
                    if (Config.ConfigShowScreenOrder == 0)
                    {
                        PopupManager.Instance.Show<PopupWinMiniGame>();
                    }
                    else
                    {
                        PopupManager.Instance.Show<PopupQuestProgress>();
                    }
                }
                else
                {
                    PopupManager.Instance.Show<PopupWinMiniGame>();
                }
                LevelTimeCount.Instance.Pause();
            }
        }

        public void ShowCollapse()
        {
            if (PlayerPrefData.CFLevelShowCollapse < PlayerPrefData.ScrewLevel)
            {
                CancelInvoke("HideCollapse");
                Invoke("HideCollapse", PlayerPrefData.CFTimeShowCollapse);
            }

        }

        private void HideCollapse()
        {
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnWin();
            }
#endif
        }

        public override void OnCloseGame()
        {
            levelManager.CloseLevel();
            base.OnCloseGame();
        }
    }
}
