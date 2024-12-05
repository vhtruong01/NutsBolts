using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public abstract class Item : MonoBehaviour
    {
        public enum ItemType
        {
            Bomb,
            Screwdrivers
        }

        public Sprite icon;
        public string desc;
        public int levelUnlock;

        public ItemType itemType;

        public bool active;

        protected Camera mainCamera => ((ScrewGameManager)MiniGameMaster.Instance.CurrentGame).mainCamera;

        private Coroutine loopCheck;

        public bool HasItem()
        {
            return ItemManager.Instance.ItemCount(itemType) > 0;
        }

        public void SelectItem()
        {
            active = true;
            ItemManager.Instance.isActiveItem = true;
            if (loopCheck != null) StopCoroutine(loopCheck);
            loopCheck = StartCoroutine(LoopCheck());
        }

        public void UnSelectItem()
        {
            active = false;
            if (loopCheck != null) StopCoroutine(loopCheck);
            ItemManager.Instance.isActiveItem = false;
        }

        public virtual void Used()
        {
            active = false;
            ItemManager.Instance.AddItem(itemType, -1);
            ItemManager.Instance.isActiveItem = false;
        }
        protected bool IsTutorial()
        {
            return ScrewBoard.tutorialType == TutorialManager.TutorialType.ItemButton && TutorialManager.ClickedButtonFunc;
        }
        protected void ForTutorial()
        {
            if (IsTutorial())
            {
                MiniGameBottomFunc.Instance.SetActiveButtonForTut(true);
                HandTutorial.Instance.DeactivateHand();
            }
        }
        public abstract IEnumerator LoopCheck();
    }
}
