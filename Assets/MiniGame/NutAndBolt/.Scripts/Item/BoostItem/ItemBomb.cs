using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class ItemBomb : Item
    {
        public override IEnumerator LoopCheck()
        {
            while (active)
            {
                yield return new WaitForEndOfFrame();
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D[] cols = Physics2D.OverlapPointAll(mousePosition, Config.Screw.ScrewLayerMask);
                    foreach (var col in cols)
                    {
                        if (col.TryGetComponent(out Plate plate))
                        {
                            // Explorer
                            ForTutorial();
                            plate.GetBomb();
                            Explode();
                            Used();
                            MiniGameBottomFunc.Instance.RefreshItemBtn();
                            MiniGameBottomFunc.Instance.UnSelectItem();
                            yield break;
                        }
                    }
                    if (!IsTutorial())
                    {
                        UnSelectItem();
                        MiniGameBottomFunc.Instance.UnSelectItem();
                    }
                }
            }
        }

        private void Explode()
        {
            //effect here
            ParticleSystem effect = BombStorage.Instance.GetBomb().GetComponent<ParticleSystem>();
            effect.transform.position = transform.position;
            effect.Play();
            AudioManager.Instance.PlaySound(Audio.Bomb);
            BombStorage.Instance.ReturnBomb(effect.gameObject, 1);
        }
    }
}
