using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform StartingPosition;
    public GameObject VictoryScreen;
    public static GameManager instance = null;
    public Text timerText;
    public Text victoryText;
    public GameObject[] FinisherTextUI;
    public float Timer = 0;
    public float TimeStarted;


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
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 initPos = StartingPosition.position;
            PhotonNetwork.Instantiate(PlayerPrefab.name, initPos, StartingPosition.rotation);
            TimeStarted = Time.time;
        }
    }

    private void Update()
    {
        Timer = Time.time - TimeStarted;
    }

}
