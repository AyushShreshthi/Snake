using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public Button multiplayerBtn;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        multiplayerBtn.interactable = true;
    }

    public void OnlineBtn()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        createRooms();
    }
    public void createRooms()
    {
        int name = Random.Range(10, 99);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("Room" + name, roomOps, TypedLobby.Default);
        Debug.Log("We are creating a room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        createRooms();
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("we joined a random room");
        SceneManager.LoadScene(2);
    }
    public void OfflineBtn()
    {
        SceneManager.LoadScene(1);
    }
}
