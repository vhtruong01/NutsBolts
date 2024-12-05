using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AV;
using DG.Tweening;

namespace Animas
{
    [SelectionBase]
    public class Screw : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer screwRender;
        [SerializeField] private CircleCollider2D circleCollider2D;
        [SerializeField] private Transform body;
        [SerializeField] private List<PlateScrewHole> plateScrewHoles;
        [SerializeField] private BoardScrewHole boardScrewHole;
        [SerializeField] private List<HingeJoint2D> hingeJoint2Ds;
        public List<PlateScrewHole> PlateScrewHoles { get => plateScrewHoles; set => plateScrewHoles = value; }
        public BoardScrewHole BoardScrewHole { get => boardScrewHole; set => boardScrewHole = value; }

        public void SetTheme(ThemeData.NailTheme nailTheme)
        {
            screwRender.sprite = nailTheme.item;
        }
        
        public void Pin(BoardScrewHole boardScrewHole, List<PlateScrewHole> m_plateScrewHoles)
        {
            hingeJoint2Ds = new List<HingeJoint2D>();
            plateScrewHoles = new List<PlateScrewHole>();

            this.boardScrewHole = boardScrewHole;
            plateScrewHoles = m_plateScrewHoles;
            transform.position = boardScrewHole.transform.position;

            foreach (var screwHole in m_plateScrewHoles)
            {
                screwHole.Pin(this);
                HingeJoint2D hingeJoint2D = gameObject.AddComponent<HingeJoint2D>();
                hingeJoint2D.connectedBody = screwHole.Plate.Rigidbody2D;
                hingeJoint2Ds.Add(hingeJoint2D);
            }
        }

        public void UnPin(bool editorMode = false)
        {
            boardScrewHole?.UnPin();
            foreach (var item in plateScrewHoles)
            {
                item?.UnPin();
            }
            boardScrewHole = null;
            plateScrewHoles?.Clear();
            foreach (var item in hingeJoint2Ds)
            {
                if (editorMode) DestroyImmediate(item);
                else Destroy(item);
            }
        }

        public void ClearScrew()
        {
            boardScrewHole?.UnPin();
            foreach (var item in plateScrewHoles)
            {
                item?.UnPin();
            }
            boardScrewHole = null;
            plateScrewHoles?.Clear();
            foreach (var item in hingeJoint2Ds)
            {
                Destroy(item);
            }
            Destroy(gameObject);
        }

        public void PlayAnimUnPin()
        {
            AudioManager.Instance.PlaySound(Audio.Unpin);
            body.DOLocalRotate(new Vector3(0, 0, 360), .5f, RotateMode.FastBeyond360);
            body.DOLocalMoveY(.2f, .3f);
            body.DOScale(Vector3.one * 1.2f, .3f);
        }

        public void PlayAnimPin()
        {
            AudioManager.Instance.PlaySound(Audio.Pin);
            body.DOLocalRotate(new Vector3(0, 0, -360), .5f, RotateMode.FastBeyond360);
            body.DOLocalMoveY( 0, .3f);
            body.DOScale(Vector3.one * 1, .3f);
        }

        #region Editor
        public void EditorPin()
        {
#if UNITY_EDITOR
            // if (BoardScrewHole != null) return;
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position, Config.Screw.HoleLayerMask);

            bool canPin = false;
            foreach (var hit in cols)
            {
                if (hit.TryGetComponent(out IHole hole))
                {
                    if (hole is BoardScrewHole)
                    {
                        canPin = true;
                    }
                }
            }

            if (canPin)
            {
                UnPin(true);
                List<PlateScrewHole> plateScrewHoles = new List<PlateScrewHole>();
                foreach (var hit in cols)
                {
                    if (hit.TryGetComponent(out IHole hole))
                    {
                        hole.Pin(this);
                        UnityEditor.EditorUtility.SetDirty(hit);
                        if (hole is PlateScrewHole)
                        {
                            plateScrewHoles.Add(hole as PlateScrewHole);
                        }
                        else
                        {
                            BoardScrewHole = hole as BoardScrewHole;
                        }
                    }
                }
                Pin(BoardScrewHole, plateScrewHoles);
            }
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        #endregion

    }
}
