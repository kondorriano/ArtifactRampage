using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionGlove : Item {

    private enum GloveState
    {
        FLYING = 0,
        ATTACHED = 1,
        RETRIEVE = 2
    }

    private class TimedVelocity
    {
        public TimedVelocity(Vector3 v, float t) { vel = v; time = t; }
        public Vector3 vel;
        public float time;
    }

    private List<TimedVelocity> vTimeline;
    private GloveState state = GloveState.ATTACHED;
    public float liftoffSpeed = 5.0f;
    public float liftoffWindow = 0.25f;
    public float flySpeed = 10.0f;
    public float flyTime = 2f;
    public float retrieveTime = .5f;
    float timeCounter = 0f;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        vTimeline = new List<TimedVelocity>();
        original = transform.GetChild(0).GetComponent<Renderer>().material.color;
    }
    protected override void Update() {
        base.Update();
        // Delete old times we don't care about
        DeleteOldTimes(Time.time);
        // Return if no wand owns me
        if (attachedWand == null) return;
        // Return if already flying
        if (state == GloveState.FLYING) {
            timeCounter += Time.deltaTime;
            if (timeCounter >= flyTime)
            {
                timeCounter = 0;
                state = GloveState.RETRIEVE;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = true;
            }
            //rb.velocity += (attachedWand.transform.position - transform.position).normalized*flybackSpeed*Time.deltaTime;
            return;
        }
        if(state == GloveState.RETRIEVE)
        {
            timeCounter += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, attachedWand.transform.position, timeCounter / retrieveTime);
            if(timeCounter >= retrieveTime)
            {
                state = GloveState.ATTACHED;
                transform.SetParent(attachedWand.transform);
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = true;
            }
            return;
        }
        // Track current
        vTimeline.Add(new TimedVelocity(attachedWand.GetVelocity(), Time.time));
        // Check for liftoff
        Vector3 liftVel = TimelineVelocityChange();
        if(liftVel != Vector3.zero) {
            state = GloveState.FLYING;
            transform.SetParent(null);
            rb.velocity = liftVel.normalized*flySpeed;
            rb.isKinematic = false;
            rb.useGravity = false;
            timeCounter = 0;
        }
    }

    private Vector3 TimelineVelocityChange()
    {
        if (vTimeline.Count <= 1) return Vector3.zero;
        TimedVelocity latest = vTimeline[vTimeline.Count - 1];
        for (int i = vTimeline.Count - 2; i >= 0; --i) {
            if ((vTimeline[i].vel.sqrMagnitude - latest.vel.sqrMagnitude) > liftoffSpeed*liftoffSpeed)
            {
                return vTimeline[i].vel;
            }
        }
        return Vector3.zero;
    }

    private void DeleteOldTimes(float time)
    {
        while(vTimeline.Count != 0)
        {
            if (time - vTimeline[0].time >= liftoffWindow)
                vTimeline.RemoveAt(0);
            else
                return;
        }
    }

    public override void Attach(WandController wand)
    {
        base.Attach(wand);
        state = GloveState.ATTACHED;
    }

    public override void Deattach(WandController wand)
    {
        if (attachedWand != wand) return;
        if (state == GloveState.RETRIEVE) rb.velocity = (attachedWand.transform.position - transform.position).normalized * flySpeed *(flyTime/retrieveTime);
        attachedWand = null;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public override void Throw(Vector3 vel)
    {
        if (state != GloveState.ATTACHED) return;
        rb.velocity = vel;
    }

    public override void Touched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void Untouched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = original;
    }
}
