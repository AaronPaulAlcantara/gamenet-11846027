using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    public Text TimerText;
    public float TimeToStartRace = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        TimerText = RacingGameManager.instance.timeText;
    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (time > 0)
        {
            TimerText.text = time.ToString("F1");
        }
        else TimerText.text = "";
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (TimeToStartRace > 0)
            {
                TimeToStartRace -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, TimeToStartRace);
            }

            else if (TimeToStartRace < 0)
            {
                photonView.RPC("StartRace", RpcTarget.AllBuffered);
            }
        }

    }
    [PunRPC]
    public void StartRace()
    {
        GetComponent<VehicleMovement>().isControlEnabled = true;
        this.enabled = false;
    }
}
