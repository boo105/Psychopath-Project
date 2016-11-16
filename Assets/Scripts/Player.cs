using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour {

    public float moveSpeed=5f;

    private Animator animator;

    private Transform tr;
    private float v;
    private float h;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
	}

    void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        animator.SetFloat("speed", v);

        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }


        /*Vector2 moveDir = (Vector2.up * v) + (Vector2.right * h);
        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);*/

      

    }
}
