using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            Destroy(gameObject);
    }
}
