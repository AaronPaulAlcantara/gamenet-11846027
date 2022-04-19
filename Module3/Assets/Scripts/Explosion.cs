using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Explosion : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, transform.GetComponent<SphereCollider>().radius + 1);
        foreach(Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                if(hit.gameObject.CompareTag("Player") && !hit.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 90f);
                }
            }
        }
    }

}
