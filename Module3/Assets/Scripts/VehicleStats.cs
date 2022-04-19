using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class VehicleStats : MonoBehaviourPunCallbacks
{

    public float MaxHealth = 100;
    public float CurrentHealth;
    public GameObject Car;

    

    
    public enum RaiseEventsCode
    {
        WhoDiedEventCode = 0
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoDiedEventCode)
        {
            if(DeathRaceGameManager.instance.PlayersAlive == 1)
            {
                object[] data = (object[])photonEvent.CustomData;
                string winner = (string)data[0];
                DeathRaceGameManager.instance.YouDiedUI.SetActive(false);
                DeathRaceGameManager.instance.VictoryPanel.SetActive(true);
                DeathRaceGameManager.instance.VictoryPanel.GetComponentInChildren<Text>().text = winner + " has won the game!";

            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo info)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die(info);
        }
    }

    void Die(PhotonMessageInfo info)
    {

        string nickName = info.Sender.NickName;
        int viewId = photonView.ViewID;


        object[] data = new object[] { nickName, viewId };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoDiedEventCode, data, raiseEventOptions, sendOption);

       // this.enabled = false;
        GetComponent<PlayerSetUp>().cam.transform.parent = null;
        GetComponent<VehicleMovement>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;        
        DeathRaceGameManager.instance.PlayersAlive--;
        Car.SetActive(false);

        if (photonView.IsMine)
        {
            DeathRaceGameManager.instance.YouDiedUI.SetActive(true);
        }
    }
}
