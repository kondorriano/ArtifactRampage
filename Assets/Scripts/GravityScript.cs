using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScript : MonoBehaviour {

    Rigidbody rb;

    private bool grounded = false;
    private float gravityForce = 9.8f;
    private Vector3 gravityDir = Vector3.down;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		if (grounded)
			rb.velocity = Vector3.zero;
        else
            rb.velocity += gravityDir * gravityForce * Time.deltaTime;
  
		Debug.Log(grounded + " " + rb.velocity + " " + gravityDir);
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
