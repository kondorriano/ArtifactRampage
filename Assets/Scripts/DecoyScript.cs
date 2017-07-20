using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyScript : MonoBehaviour {

	bool activated = false;

	GameObject owner;
	GameObject t;

	void OnCollisionEnter(Collision col){
		if (!activated)
			return;
		//si toca suelo
			//desactivar rigidbody
			//desactivar collider
			//orientarlo en direccion a la normal del plano
			activate();
	}

	public void setOwner(GameObject go) {
		owner = go;
	}

	private void activate(){
		float offSetY = owner.GetComponent<MeshFilter> ().mesh.bounds.size.y * owner.transform.localScale.y;
		t = GameObject.Instantiate (owner,transform.position + transform.up * offSetY, transform.rotation);
		foreach (Component c in t.GetComponents(typeof(Component))) {
			Type type = c.GetType ();
			if(	type != typeof(MeshRenderer) && 
				type != typeof(Transform) && 
				type != typeof(MeshFilter)){
					Destroy (c);
			}
		}
	}

}
