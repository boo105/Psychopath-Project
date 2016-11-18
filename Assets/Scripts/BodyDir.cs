using UnityEngine;
using System.Collections;

public class BodyDir : MonoBehaviour
{

    private float v;
    private float h;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");


        if(v != 0)
            animator.SetFloat("speed", Mathf.Abs(v));
        else if(h != 0)
            animator.SetFloat("speed", Mathf.Abs(h));

    }
}