using System.Collections;
using UnityEngine;

class WarningUI : MonoBehaviour
{
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator Display(string triggerName)
    {
        animator.SetTrigger("Start");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetTrigger(triggerName);
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetTrigger("Stop");
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        gameObject.SetActive(false);
    }
    public void Show() => gameObject.SetActive(true) ;
}