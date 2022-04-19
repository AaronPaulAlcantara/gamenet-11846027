using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviourPunCallbacks
{
    public float ProjectileSpeed;
    public GameObject ExplosionPrefab;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward*ProjectileSpeed,ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            GameObject obj = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 1f);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Player") && !col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GameObject obj = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 1f);
            Destroy(gameObject);
        }


    }


}
