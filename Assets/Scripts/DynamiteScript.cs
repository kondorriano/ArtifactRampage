using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteScript : MonoBehaviour {

	private float limitTime = 3.0f;
	private bool activated = false;
	private bool explode = false;
	private IEnumerator coroutine;

	public int pId = 1;
	public DynamiteHandScript parent = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			//TODODODODODOOD:
			//Particula de la mecha
		}
		if (explode) {
			createExplosion ();
			activated = false;
			Destroy (gameObject);
		}
	}

	public void ignite() {
		if (!activated) {
			activated = true;
			coroutine = timer ();
			Debug.Log ("U need to fix dis sht, enable collisions after throwing.");
			StartCoroutine (coroutine);
		}
	}

	IEnumerator timer() {
		yield return new WaitForSeconds(limitTime);
		explode = true;
	}

	//TODO: crear explosion
	void createExplosion() {
		//TODOTODOTODOTODO
		//Particula explosion
		//Impactos con entorno
	}

	void OnDestroy(){
		if (activated)
			StopCoroutine (coroutine);
		if (parent != null && parent.loadedContains (gameObject))
			parent.loadedRemove (gameObject);
	}

	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Fire"))
			ignite ();
	}

	void OnCollisionEnter(Collision col){
		DynamiteScript ds= col.gameObject.GetComponent<DynamiteScript> ();
		if (ds != null && ds.pId == pId)
			Physics.IgnoreCollision (
				ds.GetComponent<CapsuleCollider> (),
				GetComponent<CapsuleCollider> ()
			);
	}
}
