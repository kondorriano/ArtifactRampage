using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBall : Item {

    PlayerScript myPlayer;

    public override void Deattach(WandController wand)
    {
        if (attachedWand == wand) attachedWand = null;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    public override void Throw(Vector3 vel)
    {
        rb.velocity = vel;
        if(myPlayer == null) myPlayer = attachedWand.GetPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Floor")) return;
        if (myPlayer != null)
        {
            myPlayer.TeleportTo(collision.contacts[0].point);
            Destroy(gameObject);
        }
    }
}
