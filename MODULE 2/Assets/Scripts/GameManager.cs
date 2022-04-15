using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

        // Start is called before the first frame update
        void Start()
      {
        if(PhotonNetwork.IsConnectedAndReady)
        {

                Vector3 randomPoint = RespawnPoints.Instance.SpawnPoints[Random.Range(0, RespawnPoints.Instance.SpawnPoints.Count)];

                Debug.Log(randomPoint);

                PhotonNetwork.Instantiate(playerPrefab.name, randomPoint, Quaternion.identity);
            
        }
        }
        


}
