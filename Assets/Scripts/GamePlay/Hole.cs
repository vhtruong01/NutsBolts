using UnityEngine;

class Hole : MonoBehaviour
{
    [SerializeField] private bool extra;

    public bool isExtra => extra;
    private SpriteRenderer spriteRenderer;
    private SpriteMask mask;

    public bool isPlateHole => parent != null;
    public Plate parent { get; private set; }

    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
    }
    public void SetOrderLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer + 1;
        mask.frontSortingOrder = layer + 1;
        mask.backSortingOrder = layer - 1;
    }
    public void Unlock()
    {
        if (extra)
        {
            transform.GetChild(0)?.gameObject.SetActive(false);
            extra = false;
        }
    }
    public void SetParent(Plate parent)
    {
        this.parent = parent;
        transform.SetParent(parent.transform);
        Vector3 parentScale = parent.transform.localScale;
        transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, parentScale.z);
        transform.localRotation = Quaternion.identity;
    }
}