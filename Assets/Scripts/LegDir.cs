using UnityEngine;
using System.Collections;

public class LegDir : MonoBehaviour {

    private Vector3 rot;

    private float v;
    private float h;

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rot = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(v!=0)
            animator.SetFloat("speed",Mathf.Abs(v));
        if(h!=0)
            animator.SetFloat("speed",Mathf.Abs(h));



        if (Input.GetKey(KeyCode.W))
        {
            rot = new Vector3(0, 0, 0);
            transform.eulerAngles = rot;
        }
        if(Input.GetKey(KeyCode.S))
        {
            rot = new Vector3(0, 0, 180);
            transform.eulerAngles = rot;
        }
        if(Input.GetKey(KeyCode.A))
        {
            rot = new Vector3(0, 0, 270);
            transform.eulerAngles = rot;
        }
        if(Input.GetKey(KeyCode.D))
        {
            rot = new Vector3(0, 0, 90);
            transform.eulerAngles = rot;
        }
	        
	}
}
