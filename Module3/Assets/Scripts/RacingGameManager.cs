using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RacingGameManager : MonoBehaviour
{
    public GameObject[] VehiclePrefabs;
    public Transform[] StartingPositions;
    public GameObject[] FinisherTextUI;

    public static RacingGameManager instance = null;

    public Text timeText;
    public List<GameObject> lapTriggers = new List<GameObject>(); 
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log((int)playerSelectionNumber);

                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 initPos = StartingPositions[actorNumber-1].position;               
                PhotonNetwork.Instantiate(VehiclePrefabs[(int)playerSelectionNumber].name,initPos, StartingPositions[actorNumber - 1].rotation);
            }

            foreach(GameObject obj in FinisherTextUI)
            {
                obj.SetActive(false);
            }
        }
    }

}
