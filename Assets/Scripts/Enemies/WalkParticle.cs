using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticle : MonoBehaviour
{
    private Animator animator;
    private string currentStateName;
    public Component particle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentAnimationStateName();
    }

    void GetCurrentAnimationStateName() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running")) {
            particle.gameObject.SetActive(true);
        }
        else {
            particle.gameObject.SetActive(false);
        }
    }
}
