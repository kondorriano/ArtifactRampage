using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : Item {

    private enum HookState
    {
        IDLE = 0,
        SHOOTING = 1,
        HOOKED = 2,
        RETRIEVING = 3        
    }

    HookState hookState = HookState.IDLE;
    Hook hook;
    public float hookSpeed = 10f;
    public float hookDistance = 20f;
    Vector3 hookedPos;
    float hookTime;
    float counter;
    PlayerScript player;

    protected override void Start()
    {
        hook = transform.GetChild(1).GetComponent<Hook>();
        rb = GetComponent<Rigidbody>();
        original = transform.GetChild(0).GetComponent<Renderer>().material.color;
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), hook.GetComponent<Collider>());
    }

    protected override void Update()
    {
        if(hookState == HookState.HOOKED)
        {
            if(attachedWand == null)
            {
                hook.Retrieve();
                return;
            }

            counter += Time.deltaTime;
            Vector3 playerPos = attachedWand.GetPlayer().transform.position;
            Vector3 playerDiff = attachedWand.transform.position - playerPos;
            Vector3 wandDir = (hookedPos - attachedWand.transform.position).normalized;
            playerDiff = new Vector3(Mathf.Abs(playerDiff.x) * wandDir.x, Mathf.Abs(playerDiff.y) * wandDir.y, Mathf.Abs(playerDiff.z) * wandDir.z);
            attachedWand.GetPlayer().TeleportTo(Vector3.Lerp(playerPos, hookedPos+playerDiff, counter / hookTime));
            if (counter >= hookTime)
            {
                hookState = HookState.IDLE;

                hook.EndHook();
            }
        }
    }
    public override void Touched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void Untouched(WandController wand)
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = original;
    }

    public override void ActionEvent()
    {
        base.ActionEvent();
        if (hookState != HookState.IDLE) return;
        hookState = HookState.SHOOTING;
        //Set positions and speeds
        hook.transform.SetParent(null);
        hook.Shoot(transform.forward, hookSpeed,hookDistance);

    }

    public void Hook(Vector3 newPos)
    {
        if(attachedWand == null)
        {
            hook.Retrieve();
            return;
        }
        hookState = HookState.HOOKED;
        hookedPos = newPos;
        hookTime = (hookedPos - attachedWand.GetPlayer().transform.position).magnitude / hookSpeed;
        counter = 0;
    }

    public void UnHook()
    {
        hookState = HookState.RETRIEVING;
    }

    public void Retrieved()
    {
        hookState = HookState.IDLE;
    }
}
