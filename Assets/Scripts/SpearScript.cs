using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour {

	public GameObject extended;
	public GameObject grip;
	public GameObject head;
	private SpearHeadScript headScript;
	Rigidbody rigid;
	bool fly;
	// Use this for initialization
	void Start () {
		headScript = head.GetComponent<SpearHeadScript> ();
		rigid = GetComponent<Rigidbody> ();
		rigid.useGravity = false;

		extended.SetActive (false);
		grip.SetActive (true);

		fly = false;
	}

	void Update() { 
		if (Input.GetKeyDown (KeyCode.Q)) {
			throwSpear(10.0f);
		}
		if (fly)
			transform.rotation = Quaternion.LookRotation (rigid.velocity);
	}

	private void throwSpear(float speed){
		fly = true;
		headScript.activate ();
		rigid.useGravity = true;
		rigid.AddForce (transform.forward * speed, ForceMode.Impulse);
	}

	public void extend(){
		extended.SetActive (true);
		grip.SetActive (false);
		fly = false;
		Destroy (rigid);
	}
}
