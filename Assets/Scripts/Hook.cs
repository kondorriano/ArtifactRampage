using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
    Rigidbody myRig;
    Hookshot hookshot;
    float hookDistance = 0;
    float hookSpeed = 0;
    float retrieveTime;
    float counter = 0;
    Quaternion initRot;
    Vector3 initPos;

    private enum HookState
    {
        IDLE = 0,
        SHOOTING = 1,
        HOOKED = 2,
        RETRIEVING = 3
    }

    HookState hookState = HookState.IDLE;

    private void Start()
    {
        hookshot = transform.parent.GetComponent<Hookshot>();
        myRig = GetComponent<Rigidbody>();
        initPos = transform.localPosition;
        initRot = transform.localRotation;
    }

    private void Update()
    {
        if(hookState == HookState.SHOOTING)
        {
            if((transform.position-hookshot.transform.position).magnitude >= hookDistance)
            {
                //Unhook;
                Retrieve();
                
            }
        } 
        else if (hookState == HookState.RETRIEVING)
        {
            counter += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, hookshot.transform.position,counter/retrieveTime);
            if(counter >= retrieveTime)
            {
                hookshot.Retrieved();
                EndHook();

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Hook;
        myRig.isKinematic = true;

        hookState = HookState.HOOKED;
        hookshot.Hook(transform.position);
    }
    public void Shoot(Vector3 direction, float speed, float distance)
    {
        myRig.isKinematic = false;
        myRig.velocity = direction * speed;
        hookState = HookState.SHOOTING;
        hookDistance = distance;
        hookSpeed = speed;
    } 

    public void EndHook()
    {
        myRig.isKinematic = true;
        hookState = HookState.IDLE;
        transform.SetParent(hookshot.transform);
        transform.localRotation = initRot;
        transform.localPosition = initPos;

    }

    public void Retrieve()
    {
        myRig.isKinematic = true;
        hookState = HookState.RETRIEVING;
        hookshot.UnHook();
        retrieveTime = (hookshot.transform.position - transform.position).magnitude / hookSpeed;
        counter = 0;
    }
}
