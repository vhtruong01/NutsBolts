using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class ScrewPlayerInput : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        private Screw currentScrew;
        public static bool canInput = true;

        private void Update()
        {
            InputHandle();
        }

        private void InputHandle()
        {
            if (Input.GetMouseButtonDown(0) && canInput)
            {
                OnMouseDown();
            }
        }

        private void OnMouseDown()
        {
            if (ItemManager.Instance.isActiveItem || PopupManager.Instance.HasPopup()) return;
            if (currentScrew == null)
            {
                Pick();
            }
            else
            {
                Pin();
            }
        }

        private void ForTutorial(BoardScrewHole hole)
        {
            if (ScrewBoard.tutorialType == TutorialManager.TutorialType.Normal)
            {
                ScrewLevelManager screwLevelManager = ((ScrewGameManager)MiniGameMaster.Instance.CurrentGame).levelManager as ScrewLevelManager;
                screwLevelManager.screwBoard.tutorialManager.ClickOnHole(hole);
            }
        }

        private void Pin()
        {
            int holeCount = 0;
            int plateCount = 0;
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] cols = Physics2D.OverlapPointAll(mousePosition, Config.Screw.HoleLayerMask);
            Vector2 holdPos = new Vector2(-1000, -1000);
            BoardScrewHole holeC = null;
            foreach (var col in cols)
            {
                if (col.TryGetComponent(out holeC))
                {
                    holdPos = holeC.transform.position;
                    if (holeC.isLockAll || holeC.isLocked && !BoardScrewHole.lockScrewOnly)
                    {
                        return;
                    }
                    break;
                }
            }
            if (holeC == null) return;
            cols = Physics2D.OverlapCircleAll(holdPos, 0.125f * holeC.transform.lossyScale.x);

            List<PlateScrewHole> plateScrewHoles = new List<PlateScrewHole>();
            BoardScrewHole boardScrewHole = null;

            foreach (var col in cols)
            {
                if (col.TryGetComponent(out IHole hole))
                {
                    if (hole is BoardScrewHole)
                    {
                        boardScrewHole = hole as BoardScrewHole;
                        ForTutorial(boardScrewHole);
                        if (currentScrew.BoardScrewHole != null && currentScrew.BoardScrewHole == boardScrewHole)
                        {
                            if (!boardScrewHole.isLockAll)
                            {
                                currentScrew.PlayAnimPin();
                                currentScrew = null;
                            }
                            return;
                        }

                        if (!boardScrewHole.CanPin)
                        {
                            RePick(currentScrew);
                            return;
                        }
                    }
                    else
                    {
                        plateScrewHoles.Add(hole as PlateScrewHole);
                    }
                    holeCount += 1;
                }
                else if (col.TryGetComponent(out Plate plate))
                {
                    plateCount += 1;
                }
            }

            if (boardScrewHole != null)
            {
                bool canPin = false;
                if (holeCount == plateCount + 1)
                {
                    canPin = true;
                    foreach (var hole in plateScrewHoles)
                    {
                        float dis = Vector2.Distance(boardScrewHole.transform.position, hole.transform.position);
                        if (dis > .05f || !boardScrewHole.CanPin)
                        {
                            canPin = false;
                            break;
                        }
                    }
                }

                if (canPin && !boardScrewHole.isLockAll && !boardScrewHole.adsLocked && !boardScrewHole.isLocked)
                {
                    OnPin();
                }
                else
                {
                    if (boardScrewHole.adsLocked)
                    {
                        UnlockHole(boardScrewHole, OnPin);
                    }

                }

                void OnPin()
                {
                    currentScrew.UnPin();
                    currentScrew.Pin(boardScrewHole, plateScrewHoles);
                    currentScrew.PlayAnimPin();
                    boardScrewHole.Pin(currentScrew);
                    foreach (var item in plateScrewHoles)
                    {
                        item.Pin(currentScrew);
                    }
                    currentScrew = null;
                }
            }
        }

        private void RePick(Screw lastPickScrew)
        {
            currentScrew = null;
            Pick();
            if (currentScrew != null)
            {
                lastPickScrew.PlayAnimPin();
            }
        }


        private void Pick()
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(mousePosition, LayerMask.GetMask("InputPlayer"));

            if (col == null)
            {
                var cols = Physics2D.OverlapPointAll(mousePosition, Config.Screw.HoleLayerMask);

                foreach (var col2 in cols)
                {
                    if (col2.TryGetComponent(out BoardScrewHole hole) && hole.adsLocked)
                    {
                        UnlockHole(hole);
                    }
                }
            }
            else if (col.transform.parent.TryGetComponent(out Screw screw))
            {
                if (screw.BoardScrewHole.isLocked || screw.BoardScrewHole.isLockAll) return;
                ForTutorial(screw.BoardScrewHole);
                currentScrew = screw;
                currentScrew.PlayAnimUnPin();
            }
        }

        private void UnlockHole(BoardScrewHole screwHole, Action cb = null)
        {
            screwHole.UnlockAds();
            cb?.Invoke();
        }
    }
}