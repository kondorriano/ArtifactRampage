using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour {

    Rigidbody rb;
    CapsuleCollider col;

    private bool grounded = false;
    private float gravityForce = 9.8f;
    private Vector3 gravityDir = Vector3.down;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!grounded)
        {
            rb.velocity += gravityDir * gravityForce * Time.deltaTime;
        }
        //Debug.Log(grounded + " " + rb.velocity);
	}

    private void OnCollisionStay(Collision collision)
    {
        grounded = collision.gameObject.CompareTag("Floor");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) ;
    }

    public void TeleportTo(Vector3 feetPos)
    {
        transform.position = feetPos + new Vector3(0, transform.localScale.y * col.height * .5f, 0);
    }
}
