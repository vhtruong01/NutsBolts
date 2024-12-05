using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Screw : MonoBehaviour
{
    private SpriteRenderer img1;
    private SpriteRenderer img2;
    private List<AnchoredJoint2D> joints;
    private Animator animator;

    public void Awake()
    {
        joints = new();
        img1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
        img2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void SetSkin(Sprite sp1, Sprite sp2)
    {
        img1.sprite = sp1;
        img2.sprite = sp2;
    }
    public void Select(bool b)
    {
        animator.SetBool("Pin", !b);
    }
    public void Connect(Plate plate)
    {
        var joint = gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = plate.GetComponent<Rigidbody2D>();
        joints.Add(joint);
    }
    public void Disconnect()
    {
        foreach (var joint in joints)
            Destroy(joint);
        joints.Clear();
    }
    public void Move(Vector3 pos)
    {
        Disconnect();
        transform.position = pos;
    }
    public IEnumerator Disapear()
    {
        animator.SetBool("Destroy", true);
        Color c = img1.color;
        while (c.a > 0)
        {
            c.a -= 0.05f;
            img1.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}
