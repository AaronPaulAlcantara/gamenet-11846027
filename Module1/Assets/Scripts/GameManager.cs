using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (playerPrefab != null)
            {
                int xRandom = Random.Range(-20, 20);
                int zRandom = Random.Range(-20, 20);

                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(xRandom, 0, zRandom), Quaternion.identity);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " has joined the room" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has joined the " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " /" + PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("GameLauncherScene"); 
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
