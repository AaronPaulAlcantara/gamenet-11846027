using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviourPunCallbacks
{
  
    private void Start()
    {
       
    }
    public enum RaiseEventsCode
    {
        WhoFinishedEventCode = 0
    }

    private int finishOrder = 0;

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
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoFinishedEventCode)
        {
            
            
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfFinishedPlayer = (string)data[0];
            finishOrder = (int)data[1];
            int viewId = (int)data[2];
            if(viewId == photonView.ViewID)
             GameManager.instance.VictoryScreen.SetActive(true);
            float finishTime = (float)data[3];
            Debug.Log(nickNameOfFinishedPlayer + " " + finishOrder + " " + finishTime.ToString("F1"));
            GameObject orderUiText = GameManager.instance.FinisherTextUI[finishOrder - 1];
            orderUiText.SetActive(true);

            if (viewId == photonView.ViewID)
            {
                orderUiText.GetComponent<FinisherPrefabInit>().PlayerNameText.text = nickNameOfFinishedPlayer + "(You)";
                orderUiText.GetComponent<FinisherPrefabInit>().PlaceText.text = finishOrder.ToString();
                orderUiText.GetComponent<FinisherPrefabInit>().TimeText.text = finishTime.ToString("F2");
            }
            else
            {
                orderUiText.GetComponent<FinisherPrefabInit>().PlayerNameText.text = nickNameOfFinishedPlayer;
                orderUiText.GetComponent<FinisherPrefabInit>().PlaceText.text = finishOrder.ToString();
                orderUiText.GetComponent<FinisherPrefabInit>().TimeText.text = finishTime.ToString("F2");
            }


        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Catcher"))
        {
            transform.position = GameManager.instance.StartingPosition.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            Debug.Log("Finish");
            Finish();
        }
    }

    private void Finish()
    {
        GetComponent<PlayerSetup>().cam.enabled = false;
        GetComponent<PlayerMovement>().speed = 0;
        GetComponent<PlayerMovement>().currentSpeed = 0;

        GetComponent<PlayerMovement>().enabled = false;

        finishOrder++;
        Debug.LogWarning(finishOrder);

        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;
        float timeFinished = GameManager.instance.Timer;

        object[] data = new object[] { nickName, finishOrder, viewId, timeFinished };
        

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoFinishedEventCode, data, raiseEventOptions, sendOption);
        
    }

}
