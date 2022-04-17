using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class LapController : MonoBehaviourPunCallbacks
{
    public List<GameObject> lapTriggers = new List<GameObject>();

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
        if(photonEvent.Code == (byte)RaiseEventsCode.WhoFinishedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfFinishedPlayer = (string)data[0];
            finishOrder = (int)data[1];
            int viewId = (int)data[2];
            Debug.Log(nickNameOfFinishedPlayer + " " + finishOrder);

            GameObject orderUiText = RacingGameManager.instance.FinisherTextUI[finishOrder - 1];
            orderUiText.SetActive(true);
            if (viewId == photonView.ViewID)
            {
                orderUiText.GetComponent<Text>().text = finishOrder + " " + nickNameOfFinishedPlayer + " (YOU) ";
                orderUiText.GetComponent<Text>().color = Color.red;
            }
            else
            {
                orderUiText.GetComponent<Text>().text = finishOrder + " " + nickNameOfFinishedPlayer;
            }



        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in RacingGameManager.instance.lapTriggers)
        {
            lapTriggers.Add(go);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (lapTriggers.Contains(col.gameObject))
        {
            int indexOfTrigger = lapTriggers.IndexOf(col.gameObject);
            lapTriggers[indexOfTrigger].SetActive(false);
        }

        if (col.gameObject.tag == "FinishTrigger")
        {
            GameFinish();
           // Debug.LogWarning("FT");
        }
    }

    public void GameFinish()
    {
        GetComponent<PlayerSetUp>().cam.transform.parent = null;
        GetComponent<VehicleMovement>().enabled = false;

        finishOrder++;
        Debug.LogWarning(finishOrder);

        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;

        object[] data = new object[] { nickName, finishOrder,viewId };
        //Debug.LogWarning("Test: " + nickName + " " + finishOrder);

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
