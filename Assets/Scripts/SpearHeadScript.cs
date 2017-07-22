using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearHeadScript : MonoBehaviour {

	bool activated = false;
	public GameObject parent;
	SpearScript ss;
	// Use this for initialization
	void Start () {
		ss = parent.GetComponent<SpearScript> ();
	}

	public void activate() {
		activated = true;
	}

	void OnTriggerEnter(Collider col) {
		if (activated) {
			ss.extend ();
			Destroy (this);
		}
	}
}
