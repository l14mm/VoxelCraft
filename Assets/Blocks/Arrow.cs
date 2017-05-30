using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    Rigidbody rb;
    private bool landed = false;
    private float startTime;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!landed)
            transform.LookAt(transform.position - rb.velocity);
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("landed");
        landed = true;
        //rb.isKinematic = true;
    }
}
