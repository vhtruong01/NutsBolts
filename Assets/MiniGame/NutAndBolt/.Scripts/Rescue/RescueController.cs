using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AV;
using DG.Tweening;
namespace Animas
{
    public class RescueController : MonoBehaviour
    {
        private int countUnlock = 0;
        [SerializeField] private RescueLock[] rescueLocks;
        [SerializeField] private Transform cage;
        [SerializeField] private ParticleSystem fxDestroyCage;
        [SerializeField] private ParticleSystem fxAnimalRescue;


        public void Unlock()
        {
            countUnlock++;
            if (countUnlock >= rescueLocks.Length)
            {
                Rescue();
            }
        }

        public void Rescue()
        {
            cage.DOShakePosition(.7f, 1, 10).OnComplete(() =>
            {
                fxDestroyCage.Play();
                cage.gameObject.SetActive(false);
            });

            this.Invoke(() =>
            {
                MiniGameMaster.Instance.CurrentGame.OnWin();
            }, 1);
        }

    }
}
