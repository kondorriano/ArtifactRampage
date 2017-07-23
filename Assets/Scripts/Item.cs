using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour {

    protected enum State
    {
        STORED = 0,
        ATTACHED = 1,
        FREE = 2
    }

    public Vector3 offsetRotation;
    protected WandController attachedWand;
    protected Rigidbody rb;
    protected Color original;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        original = GetComponent<Renderer>().material.color;
    }

    protected virtual void Update()
    {

    }

    public virtual void Attach(WandController wand)
    {
        if (attachedWand == wand) return;
        attachedWand = wand;
        transform.SetParent(wand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(offsetRotation, Space.Self);
        rb.isKinematic = true;
    }

    public virtual void Deattach(WandController wand)
    {
        if (attachedWand != wand) return;

        attachedWand = null;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    public virtual void Touched(WandController wand)
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    public virtual void Untouched(WandController wand)
    {
        GetComponent<Renderer>().material.color = original;
    }

    public virtual void Throw(Vector3 vel)
    {
        rb.velocity = vel;
    }

    public virtual void ActionEvent()
    {

    }
}
