using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSync : MonoBehaviour {

    [Header("Master Objects")]
    public GameObject RealHolder;
    public Transform RealHead;
    public Transform RealHandLeft;
    public Transform RealHandRight;

    [Header("Dummy Objects")]
    public GameObject DummyHolder;
    public Transform DummyHead;
    public Transform DummyHandLeft;
    public Transform DummyHandRight;

    //Internal Lerp Values
    Vector3 HeadPos;
    Vector3 LHPos;
    Vector3 RHPos;
    Quaternion HeadRot;
    Quaternion LHRot;
    Quaternion RHRot;

    PhotonView view;

    bool receivedInfo = false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void SetupReal()
    {
        RealHolder.SetActive(true);
        DummyHolder.SetActive(false);
    }

    private void Update()
    {
        //Lerp Body Positions
        if(!view.isMine && receivedInfo)
        {
            DummyHead.position = Vector3.Lerp(DummyHead.position, HeadPos, Time.deltaTime * 9);
            DummyHead.rotation = Quaternion.Lerp(DummyHead.rotation, HeadRot, Time.deltaTime * 9);
            /*DummyHandLeft.position = Vector3.Lerp(DummyHandLeft.position, LHPos, Time.deltaTime * 9);
            DummyHandLeft.rotation = Quaternion.Lerp(DummyHandLeft.rotation, LHRot, Time.deltaTime * 9);
            DummyHandRight.position = Vector3.Lerp(DummyHandRight.position, RHPos, Time.deltaTime * 9);
            DummyHandRight.rotation = Quaternion.Lerp(DummyHandRight.rotation, RHRot, Time.deltaTime * 9);*/
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            //Body Positions
            stream.SendNext(RealHead.position);
            stream.SendNext(RealHead.rotation);
           /* stream.SendNext(RealHandLeft.position);
            stream.SendNext(RealHandLeft.rotation);
            stream.SendNext(RealHandRight.position);
            stream.SendNext(RealHandRight.rotation);*/
        } else
        {
            //Body Positions
            HeadPos = (Vector3)stream.ReceiveNext();
            HeadRot = (Quaternion)stream.ReceiveNext();
            /*LHPos = (Vector3)stream.ReceiveNext();
            LHRot = (Quaternion)stream.ReceiveNext();
            RHPos = (Vector3)stream.ReceiveNext();
            RHRot = (Quaternion)stream.ReceiveNext();*/

            receivedInfo = true;

        }
    }

}
