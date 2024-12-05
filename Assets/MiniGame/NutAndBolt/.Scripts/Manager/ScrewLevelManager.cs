using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class ScrewLevelManager : MiniGameLevelManager
    {
        private GameObject currentLevel;
        public ScrewBoard screwBoard { get; private set; }

        private int amountLevel = 100;
        public DataLevel dataLevel;

        public override void LoadLevel(int level)
        {
            // AdsManager.ShowCollapseBanner("start_level_screw");
            level = GetLevel(level);
            if (level % 5 == 0)
            {
                ((ScrewGameManager)MiniGameMaster.Instance.CurrentGame).mainCamera.orthographicSize = 5.75f;
            }
            else
            {
                ((ScrewGameManager)MiniGameMaster.Instance.CurrentGame).mainCamera.orthographicSize = 4.85f;
            }

            // if (SdkUtil.isiPad())
            // {
            //     ((ScrewGameManager)MiniGameMaster.Instance.CurrentGame).mainCamera.transform.localPosition = new Vector3(0, .15f, -10);
            // }

            dataLevel = DataManager.levelConfigs.dataLevels[level - 1];
            SetTimeCount();
            var prefab = Resources.Load("ScrewNewLevelPrefab/Level " + dataLevel.keyPrefabLevel);
            var obj = Instantiate(prefab, transform);

            if (currentLevel != null)
            {
                CloseLevel();
            }
            currentLevel = obj as GameObject;
            screwBoard = currentLevel.GetComponentInChildren<ScrewBoard>();
            screwBoard.SetTheme(level % 5 == 0);
        }

        public void SetTimeCount()
        {
            if (dataLevel.levelType == TypeManager.LevelType.TIME_COUNT)
            {
                LevelTimeCount.Instance.SetUp((int)dataLevel.time);
            }
            else
            {
                LevelTimeCount.Instance.SetUp(0);
                LevelTimeCount.Instance.SetActiveCover(false);
            }
        }

        public override void CloseLevel()
        {
            Destroy(currentLevel);
            screwBoard = null;
        }

        public int GetLevel(int level)
        {
            if (level <= amountLevel)
            {
                return level;
            }
            else
            {
                return PlayerPrefData.ScrewCurrentMapLevel;
            }
        }

        public override void NextLevel()
        {
            string log = "on_nextlevel_minigame";
            base.NextLevel();
        }


        public Queue<int> lastLevel = new Queue<int>();
        public override void IncreaseLevel()
        {
            if (PlayerPrefData.ScrewLevel > amountLevel)
            {
                int rd = Random.Range(25, amountLevel + 1);
                for (int i = 0; i < 100; i++)
                {
                    if (!lastLevel.Contains(rd))
                    {
                        break;
                    }
                    rd = Random.Range(25, amountLevel + 1);
                }

                PlayerPrefData.ScrewCurrentMapLevel = rd;
                lastLevel.Enqueue(rd);
                if (lastLevel.Count > 4)
                {
                    lastLevel.Dequeue();
                }
            }
        }
    }
}
