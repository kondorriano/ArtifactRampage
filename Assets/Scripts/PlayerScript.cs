using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    float speedHorizontal = 3.0f;
    Vector3 dir;
	
	// Update is called once per frame
	void Update () {
		dir = Input.GetAxisRaw ("Vertical") * transform.forward;
		dir += Input.GetAxisRaw("Horizontal") * transform.right;
		transform.Translate (dir * Time.deltaTime * speedHorizontal);
	}
}
