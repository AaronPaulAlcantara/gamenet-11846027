using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login Ui Panel")]
    public InputField PlayerNameInput;
    public GameObject LoginPanel;

    [Header("Connection Status Panel")]
    public Text ConnectionStatusText;

    [Header("Game Options Panel")]
    public GameObject GameOptionsPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;
    public InputField RoomNameInputField;
    public InputField PlayerCountInputField;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListParent;
    public GameObject startGameBtn;
    public Text RoomInfoText;


    [Header("Room List Panel")]
    public GameObject RoomListPanel;
    public GameObject RoomListPrefab;
    public GameObject RoomListParent;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;


    #region Unity functions

    private void Start()
    {
        ActivatePanel(LoginPanel);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void Update()
    {
        ConnectionStatusText.text = "Connection status:" + PhotonNetwork.NetworkClientState; 
    }


    #endregion

    #region UI Callbacks
    public void OnLoginBtnClicked()
    {
        string playerName = PlayerNameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Invalid Name.");

        }

        else
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;

        if(string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(PlayerCountInputField.text);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
       
    }

    public void OnRoomCancelBtn()
    {
        ActivatePanel(GameOptionsPanel);
    }

    public void OnBackBtnClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }


        ActivatePanel(GameOptionsPanel);

    }

    public void OnShowRoomListBtn()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(RoomListPanel);
    }

    public void OnLeaveGameBtnClicked()
    {
        PhotonNetwork
.LeaveRoom();
    }

    public void OnJoinRandomRoomBtnClicked()
    {
        ActivatePanel(JoinRandomRoomPanel);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnStartBtnClicked() {

        PhotonNetwork.LoadLevel("SampleScene");
    }
    #endregion

    #region PUN Callbacks

    public override void OnLeftLobby()
    {
        ClearRoomListGameObjects();
        cachedRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListGameObjects();

        startGameBtn.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);

        foreach(RoomInfo info in roomList)
        {
            Debug.Log(info.Name);
            if(!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else
            {
                if(cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                cachedRoomList.Add(info.Name, info);
            }
            
        }

        foreach(RoomInfo info in cachedRoomList.Values)
        {
            GameObject listItem = Instantiate(RoomListPrefab);

            listItem.transform.SetParent(RoomListParent.transform);
            listItem.transform.localScale = Vector3.one;

            listItem.transform.Find("RoomNameText").GetComponent<Text>().text = info.Name;
            listItem.transform.Find("RoomPlayersText").GetComponent<Text>().text = "( " + info.PlayerCount 
                + "/" + info.MaxPlayers + ")";
            listItem.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomClicked(info.Name));

            roomListGameObjects.Add(info.Name,listItem);
            
        }
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to the internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Connected to the server");
        ActivatePanel(GameOptionsPanel);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Has Joined " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoomPanel);
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + "  Players: " + PhotonNetwork.CurrentRoom.PlayerCount +
         " / " + PhotonNetwork.CurrentRoom.MaxPlayers;


        if(playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerItem = Instantiate(PlayerListPrefab);

            playerItem.transform.SetParent(PlayerListParent.transform);
            playerItem.transform.localScale = Vector3.one;

            playerItem.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;
            playerItem.transform.Find("PlayerIndicator").gameObject.SetActive(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

            playerListGameObjects.Add(player.ActorNumber, playerItem);
            
        }

    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + "  Players: " + PhotonNetwork.CurrentRoom.PlayerCount +
        " / " + PhotonNetwork.CurrentRoom.MaxPlayers;


        GameObject playerItem = Instantiate(PlayerListPrefab);

        playerItem.transform.SetParent(PlayerListParent.transform);
        playerItem.transform.localScale = Vector3.one;

        playerItem.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;
        playerItem.transform.Find("PlayerIndicator").gameObject.SetActive(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

        playerListGameObjects.Add(player.ActorNumber, playerItem);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        startGameBtn.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);

        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + "  Players: " + PhotonNetwork.CurrentRoom.PlayerCount +
        " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber]);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
       foreach(var gameObject in playerListGameObjects.Values)
        {
            Destroy(gameObject);
        }

        playerListGameObjects.Clear();
        playerListGameObjects = null;

       ActivatePanel(GameOptionsPanel);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning(message);
 
        string roomName = "Room " + Random.Range(1000, 10000);   
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #endregion

    #region Public Functions
    public void ActivatePanel(GameObject targetPanel)
    {
        LoginPanel.SetActive(targetPanel.Equals(LoginPanel));
        GameOptionsPanel.SetActive(targetPanel.Equals(GameOptionsPanel));
        InsideRoomPanel.SetActive(targetPanel.Equals(InsideRoomPanel));
        CreateRoomPanel.SetActive(targetPanel.Equals(CreateRoomPanel));
        JoinRandomRoomPanel.SetActive(targetPanel.Equals(JoinRandomRoomPanel));
        RoomListPanel.SetActive(targetPanel.Equals(RoomListPanel));
    }
    #endregion


    #region Private Functions
    private void OnJoinRoomClicked(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    private void ClearRoomListGameObjects()
    {
        foreach(var item in roomListGameObjects.Values)
        {
            Destroy(item);
        }

        roomListGameObjects.Clear();
       // roomListGameObjects = null;

    }

    #endregion
}
