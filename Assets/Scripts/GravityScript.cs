using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScript : MonoBehaviour {

    Rigidbody rb;

    private bool grounded = false;
    private float gravityForce = 9.8f;
    private Vector3 gravityDir = Vector3.down;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
		if (grounded)
			rb.velocity = Vector3.zero;
        else
            rb.velocity += gravityDir * gravityForce * Time.deltaTime;
  
		//Debug.Log(grounded + " " + rb.velocity + " " + gravityDir);
    }

    private void OnCollisionStay(Collision collision)
    {
		//Debug.Log (collision.gameObject.tag);
        grounded = collision.gameObject.CompareTag("Floor");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            grounded = false;
    }


	//SETERS FOR GRAVITY PARAM
	    public void setGravityDir(Vector3 newDir)
	    {
	        gravityDir = newDir;
	    }

	    public void setGravityForce(float newForce)
	    {
	        gravityForce = newForce;
	    }

	    public void setGravity(Vector3 newDir, float newForce)
	    {
	        gravityDir = newDir;
	        gravityForce = newForce;
	    }
}
