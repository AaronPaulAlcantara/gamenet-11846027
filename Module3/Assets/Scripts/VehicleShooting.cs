using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class VehicleShooting : MonoBehaviourPunCallbacks
{
    public float ShootingType;

    public float fireRate;
    public float damage;
    public Camera cam;
    public float NextFire;
    public GameObject ProjectilePrefab;
    public Transform Gunpoint;

    public GameObject HitEffectPrefab;

     private void Start()
     {


        if(ShootingType == 0)
        {
            fireRate = 1.0f;
            damage = 50f;
        }
        else if (ShootingType == 1)
        {
            fireRate = 0.25f;
            damage = 10f;
        }
        else if (ShootingType == 2)
        {
            fireRate = 1.0f;
            damage = 50f;
        }
    }
    private void LateUpdate()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetButton("Fire1") && Time.time > NextFire)
        {
            NextFire = Time.time + fireRate;
            
            if (ShootingType < 2)
            {               
                //Ray Cast
                RayCastShooting();
            }
           else if (ShootingType == 2)
            {
                //Projectile
                ProjectileShooting();
            }
        }
    }

    //RayCast-Shooting
    void RayCastShooting()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 200))
        {
            photonView.RPC("CreateHitEffect", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Detected");
                if (hit.collider.GetComponent<VehicleStats>().CurrentHealth > 0)
                {
                    Debug.Log("Passed");
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
                }
            }
        }
    }

    //Projectile Shooting
    void ProjectileShooting()
    {
       Instantiate(ProjectilePrefab, Gunpoint.position, Gunpoint.rotation);
       
    }
    

    [PunRPC]
    public void CreateHitEffect(Vector3 pos)
    {
        GameObject hitEffectGameObject = Instantiate(HitEffectPrefab, pos, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }



    
}
