using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class DeathRaceGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] VehiclePrefabs;
    public Transform[] StartingPositions;

    public GameObject YouDiedUI;
    public GameObject VictoryPanel;

    public int PlayersAlive;

    public static DeathRaceGameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
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
                Vector3 initPos = StartingPositions[actorNumber - 1].position;
                GameObject obj = PhotonNetwork.Instantiate(VehiclePrefabs[(int)playerSelectionNumber].name, initPos, StartingPositions[actorNumber - 1].rotation);
                obj.GetComponent<VehicleShooting>().ShootingType = (int)playerSelectionNumber;
                obj.GetComponent<VehicleMovement>().isControlEnabled = true;                               
            }

            PlayersAlive = PhotonNetwork.CurrentRoom.PlayerCount;

        }
    }

    public override void OnLeftRoom()
    {
        PlayersAlive--;
    }
}
