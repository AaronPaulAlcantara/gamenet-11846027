using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetUp : MonoBehaviourPunCallbacks
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {     
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = photonView.IsMine;
            cam.enabled = photonView.IsMine;
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = photonView.IsMine;
            cam.enabled = photonView.IsMine;
        }
    }
}
 