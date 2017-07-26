using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Item {

    protected enum BoomState
    {
        THROWN = 0,
        DECELERATING = 1,
        RETURNING = 2,
        IDLE = 3
    }
    BoomState boomerangState = BoomState.IDLE;
    public float rotationSpeed = 800f;
    public float boomerangDistance = 20;
    public float speed = 10;
    public float decelerationTime = 1;
    float counter = 0;
    Vector3 startPosition;

    Transform oldWand;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        original = transform.GetChild(0).GetComponent<Renderer>().material.color;
    }

    protected override void Update()
    {
        if (boomerangState == BoomState.IDLE) return;
        //ROTATE
        transform.Rotate(transform.up * rotationSpeed * Time.deltaTime);
        if(boomerangState == BoomState.THROWN)
        {
            //FROm init to END
            if ((transform.position - startPosition).magnitude >= boomerangDistance)
            {
                counter = 0;
                boomerangState = BoomState.DECELERATING;
            }
        }

        if (boomerangState == BoomState.DECELERATING)
        {
            //decelerate until speed negative
            rb.velocity = Vector3.Lerp(rb.velocity, (oldWand.position - transform.position).normalized * speed, counter * decelerationTime);
            counter += Time.deltaTime;
            if (counter >= 1) boomerangState = BoomState.RETURNING;
            if ((oldWand.position - transform.position).sqrMagnitude <= 1f)
            {
                rb.useGravity = true;
                boomerangState = BoomState.IDLE;
            }

        }
        if (boomerangState == BoomState.RETURNING)
        {
            //focus on player hand
            rb.velocity = (oldWand.position - transform.position).normalized * speed;
            if ((oldWand.position - transform.position).sqrMagnitude <= 1f)
            {
                rb.useGravity = true;
                boomerangState = BoomState.IDLE;
            }
        }
    }

    public override void Touched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
        transform.GetChild(1).GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void Untouched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = original;
        transform.GetChild(1).GetComponent<Renderer>().material.color = original;
    }

    void Bounce(Vector3 normal)
    {
        boomerangState = BoomState.DECELERATING;
        counter = 0;
        rb.velocity = normal * Mathf.Min(rb.velocity.magnitude * 1.5f, speed);
    }

    public override void Throw(Vector3 vel)
    {
        rb.useGravity = false;
        boomerangState = BoomState.THROWN;
        rb.velocity = vel;
        oldWand = attachedWand.transform;
        startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (boomerangState == BoomState.IDLE) return; 
        Bounce(collision.contacts[0].normal);
    }
}
