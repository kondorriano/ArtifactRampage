using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAreaScript : MonoBehaviour {

    public Vector3 gravityDir;

	void Start() {
		gravityDir = gravityDir.normalized;
	}

	    void Update()
	    {
	        Debug.DrawLine(transform.position, transform.position + gravityDir * 5.0f, Color.red);
	    }
    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.GetComponent<GravityScript>() != null)
            collider.gameObject.GetComponent<GravityScript>().setGravityDir(gravityDir);
    }

}
