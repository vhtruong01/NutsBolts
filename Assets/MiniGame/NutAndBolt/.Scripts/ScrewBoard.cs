using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Animas
{
    public class ScrewBoard : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer boardRender;

        public List<BoardScrewHole> boardScrewHoles;
        public List<Screw> screws;
        public List<Plate> plates;

        public List<BoardScrewHole> adScrewHoles;

        public Transform ScrewHoleHolder, ScrewHolder;
        public Transform plateLayer1, plateLayer2, plateLayer3, plateLayer4, plateLayer5, plateLayer6, plateLayer7, plateLayer8;
        float timeToCheckDrop;
        private int indexAdUnlock;
        private SpriteRenderer crBackground;
        public TutorialManager tutorialManager;
        public static TutorialManager.TutorialType tutorialType = TutorialManager.TutorialType.None;
        [SerializeField] private ScrewModeType modeType;
        public static ScrewModeType ModeType;

        private void Start()
        {
            ListLevelConfig listLevelConfig = DataManager.listLevelConfig;
            ModeType = modeType;
            ScrewGameManager screwGameManager = MiniGameMaster.Instance.CurrentGame as ScrewGameManager;

            #region tutorial
            if (tutorialManager != null && tutorialManager.HasTut)
            {
                tutorialType = tutorialManager.buttonType == MiniGameBottomFunc.ButtonType.None ?
                TutorialManager.TutorialType.Normal : TutorialManager.TutorialType.ItemButton;

                if (TutorialManager.LevelPassed < MiniGameMaster.Instance.levelData.GetLevel())
                {
                    if (tutorialType == TutorialManager.TutorialType.ItemButton)
                    {
                        MiniGameBottomFunc.Instance.buttonType4Tut = tutorialManager.buttonType;
                    }
                    else MiniGameBottomFunc.Instance.buttonType4Tut = MiniGameBottomFunc.ButtonType.None;
                }
                else
                {
                    tutorialType = TutorialManager.TutorialType.None;
                    HandTutorial.Instance.DeactivateHand();
                }
            }
            else
            {
                tutorialType = TutorialManager.TutorialType.None;
                HandTutorial.Instance.DeactivateHand();
            }
            #endregion

            if (Config.Screw.EnableAdsUnlockHoleBtn)
            {
                for (int i = 0; i < adScrewHoles.Count; i++)
                {
                    adScrewHoles[i].gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            CheckPlatesDrop();
        }

        public void SetTheme(bool? isBigSize = null)
        {
            if (boardRender == null) boardRender = GetComponent<SpriteRenderer>();
            var cGame = (ScrewGameManager)MiniGameMaster.Instance.CurrentGame;
            var themeData = cGame.themeData;
            boardRender.sprite = themeData.GetPlankTheme.item;
            if (crBackground == null)
            {
                crBackground = Instantiate(cGame.backgroundPrefab, transform);
                crBackground.transform.position = Vector3.zero;
            }

            crBackground.sprite = themeData.GetBackgroundTheme.item;
            var nailTheme = themeData.GetNailTheme;
            for (int i = 0; i < screws.Count; i++)
            {
                if (screws[i] == null) continue;
                screws[i].SetTheme(nailTheme);
            }

            if (isBigSize != null)
            {
                var adv = Vector3.one * (mygame.sdk.SdkUtil.isiPad() ? .25f : 0);

                crBackground.transform.localScale = (isBigSize.Value ? new Vector3(0.75f, 0.75f, 0.75f) : new Vector3(0.68f, 0.68f, 0.68f)) + adv;
            }

        }

        public void UnlockNewScrewHole()
        {
            if (indexAdUnlock < adScrewHoles.Count)
            {
                adScrewHoles[indexAdUnlock].gameObject.SetActive(true);
                indexAdUnlock++;
            }
        }

        public bool HasAdScrewSlot()
        {
            return indexAdUnlock < adScrewHoles.Count;
        }

        private void Awake()
        {
            transform.localScale = Vector2.one * .8f;
            transform.position += Vector3.up * .3f;
        }

        private void CheckPlatesDrop()
        {
            if (modeType == ScrewModeType.Normal)
            {
                timeToCheckDrop -= Time.deltaTime;
                if (timeToCheckDrop < 0)
                {
                    timeToCheckDrop = .2f;
                    for (int i = 0; i < plates.Count; i++)
                    {
                        if (plates[i].transform.position.y < -6)
                        {
                            plates.RemoveAt(i);
                            i--;
                        }
                    }

                    if (plates.Count == 0)
                    {
                        MiniGameMaster.Instance.CurrentGame.OnWin();
                    }
                }
            }
        }


        [ContextMenu("Resize Hole In Plates")]
        public void ResizeHoleInPlates()
        {
            for (int i = 0; i < plates.Count; i++)
            {
                plates[i].ResizeScale();
            }
        }
    }

    public enum ScrewModeType
    {
        Normal, Rescue
    }
}
