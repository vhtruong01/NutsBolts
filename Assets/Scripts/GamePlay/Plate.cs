using System.Collections;
using UnityEngine;

class Plate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public int layer { get; private set; }

    public void Start() => SetLayer();
    public void SetLayer()
    {
        int curLayer = gameObject.layer;
        spriteRenderer = GetComponent<SpriteRenderer>();
        layer = Mathf.Max(curLayer, 0) * 2;
        spriteRenderer.sortingOrder = layer;
        int holeOrderLayer = spriteRenderer.sortingOrder;
        foreach (Hole h in GetComponentsInChildren<Hole>())
            h.SetOrderLayer(holeOrderLayer);
    }
    public IEnumerator Disapear()
    {

        Color c = spriteRenderer.color;
        while (c.a > 0)
        {
            c.a -= 0.05f;
            spriteRenderer.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}