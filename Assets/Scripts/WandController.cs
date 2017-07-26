using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;


    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private Item grabableItem;
    private Item itemInUse;

    private PlayerScript player;
    public PlayerScript GetPlayer()
    {
        return player;
    }

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        player = transform.parent.parent.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update () {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if(controller.GetPressUp(menuButton))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        if (controller.GetPressDown(triggerButton) && grabableItem != null)
        {
            grabableItem.Attach(this);
            itemInUse = grabableItem;
            itemInUse.Untouched(this);
        }

        if (controller.GetPressDown(touchPadButton) && itemInUse != null)
        {
            itemInUse.ActionEvent();
        }

        if (controller.GetPressUp(triggerButton) && itemInUse != null)
        {
            itemInUse.Throw(controller.velocity * 2f);
            itemInUse.Deattach(this);
            itemInUse = null;
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        Item collidedItem = collider.GetComponent<Item>();
        if (itemInUse == collidedItem) return;
        if (collidedItem)
        {
            if (grabableItem != null) grabableItem.Untouched(this);

            collidedItem.Touched(this);
            grabableItem = collidedItem;            
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Item collidedItem = collider.GetComponent<Item>();
        if (collidedItem)
        {
            collidedItem.Untouched(this);
            if (collidedItem == grabableItem) grabableItem = null;
        }
    }
    

    public void Rumble(float length)
    {
        StopCoroutine("LongVibration");
        StartCoroutine(LongVibration(length, .4f));

    }

    //length is how long the vibration should go for
    //strength is vibration strength from 0-1
    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {

            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }

    public Vector3 GetVelocity()
    {
        return controller.velocity;
    }
}
