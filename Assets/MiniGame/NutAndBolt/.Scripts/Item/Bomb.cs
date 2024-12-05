using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class Bomb : MonoBehaviour
    {

        Plate plateOwner;
        private void Awake()
        {
            plateOwner = transform.parent.GetComponent<Plate>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Plate plate;
            if (collision.TryGetComponent<Plate>(out plate))
            {
                if (plate == plateOwner)
                {
                    return;
                }
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,1f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<Plate>(out plate))
                    {
                        if (plate == plateOwner)
                        {
                            continue;
                        }
                        plate.GetBomb();
                        break;
                    }
                }
                Explode();

                //plate.GetBomb();
                //Explode();
            }
        }

        public void Explode()
        {
            //effect here
            ParticleSystem effect = BombStorage.Instance.GetBomb().GetComponent<ParticleSystem>();
            effect.transform.position = transform.position;
            effect.Play();
            AudioManager.Instance.PlaySound(Audio.Bomb);
            BombStorage.Instance.ReturnBomb(effect.gameObject, 1);
            gameObject.SetActive(false);
        }
    }
}
