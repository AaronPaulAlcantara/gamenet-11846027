using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerMovement>().enabled = photonView.IsMine;
        GetComponent<PlayerEvents>().enabled = photonView.IsMine;
        cam.enabled = photonView.IsMine;
    }

}
