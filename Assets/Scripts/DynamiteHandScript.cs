using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteHandScript : MonoBehaviour {

	//Variable para el modelo
	public GameObject dinamite;

	//Variables de generacion
	private int ammo = 6;
	private int totalPos = 4;
	private List<Transform> positions;
	private List<GameObject> loaded;

	//Variables de disparo
	private GameObject actual;
	private int nextToLit = 3;

	void Start () {
		positions = new List<Transform> ();
		loaded = new List<GameObject> ();
		int i = 0;
		foreach (Transform child in transform) {
			positions.Add (child);
			if (ammo > 0 && i < totalPos) {
				GameObject t = GameObject.Instantiate (dinamite, child.position, child.rotation, transform);
				t.GetComponent<Rigidbody> ().useGravity = false;
				loaded.Add(t);
				--ammo; ++i;
			}
		}
	}

	void Update () {
		if (loaded.Count > 0) {
			if (Input.GetButtonDown ("Fire1")) {
				actual = loaded [loaded.Count-1];
				actual.transform.parent = null;
				if (nextToLit > 0)
					--nextToLit;
				loaded.RemoveAt (loaded.Count-1);
				throwDinamite (actual);
			}
			if (Input.GetButtonDown ("Fire2")) {
				actual = loaded [loaded.Count-1];
				actual.GetComponent<DynamiteScript> ().ignite ();
				actual.GetComponent<DynamiteScript> ().parent = this;
			}
		} else {
			Debug.Log ("No ammo.");
		}
		if (ammo > 0 && loaded.Count < totalPos) {
			if (Input.GetKeyDown (KeyCode.R)) {
				GameObject t = GameObject.Instantiate (
					dinamite, 
					positions [loaded.Count].position, 
					positions [loaded.Count].rotation, 
					transform
				);
				t.GetComponent<Rigidbody> ().useGravity = false;
				loaded.Add(t);
				--ammo;
			}
		}
	}

	private void throwDinamite(GameObject go){
		go.GetComponent<Rigidbody> ().AddForce (transform.forward * 6.0f, ForceMode.Impulse);
		go.GetComponent<Rigidbody> ().useGravity = true;
	}

	public bool loadedContains(GameObject go) {
		return loaded.Contains (go);
	}
		
	public void loadedRemove(GameObject go){
		loaded.Remove (go);
		int i = 0;
		foreach (GameObject g in loaded) {
			g.transform.position = positions [i].position;
			g.transform.rotation = positions [i].rotation;
			++i;
		}
	}
}
