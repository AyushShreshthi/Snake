using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager om;
    private void Awake()
    {
        om = this;
    }
    public GameObject onlineSnakePrefab;
    public GameObject onlineFood;
    public GameObject endingPanel;
    public Transform[] playerPos;

    float timer = 90f;
    public TMP_Text timerText;

    int score_1 = 0;
    int score_2 = 0;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    public int playerNum = 0;

    private void Start()
    {

        timerText.text = timer.ToString();
        player1ScoreText.text = score_1.ToString();
        player2ScoreText.text = score_2.ToString();

        SpawnPlayers();
        SpawnFood();

    }
    private void Update()
    {


        timer -= Time.deltaTime;
        if (timer >= 0)
        {
            timerText.text = ((int)timer).ToString();
        }
        else
        {
            endingPanel.SetActive(true);


            if (playerNum == 0)
            {
                
                if (score_1 > score_2)
                {
                    endingPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "YOU WIN !!! ";
                }
                else
                {
                    endingPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "YOU LOSE !!! ";
                }
            }
            else
            {

                if (score_1 < score_2)
                {
                    endingPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "YOU WIN !!! ";
                }
                else
                {
                    endingPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "YOU LOSE !!! ";
                }
            }
        }
    }
    void SpawnPlayers()
    {
        int playerPos = 0;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                playerPos = i;
                playerNum = playerPos;
                break;
            }
        }
        GameObject go = PhotonNetwork.Instantiate(onlineSnakePrefab.name, 
                                                   this.playerPos[playerPos].position, 
                                                   this.playerPos[playerPos].rotation);


    }

    public void SpawnFood()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float randX = Random.Range(-20, 20);
            float randZ = Random.Range(-20, 20);

            GameObject go = PhotonNetwork.Instantiate(onlineFood.name, new Vector3(randX, 0, randZ), Quaternion.identity);
        }
    }

   
    public void ScoreUpdate( int playerNum)
    {
        int scores = 0;
        if (playerNum == 0)
        {
            scores = score_1 + 1;
            score_1 = scores;
        }
        else
        {
            scores = score_2 + 1;
            score_2 = scores;
        }

       
        photonView.RPC("EarnScore", RpcTarget.All, scores, playerNum);
    }
    [PunRPC]
    void EarnScore(int score, int playerNum)
    {
        if (playerNum == 0)
        {
            player1ScoreText.text = score.ToString();
        }
        else
        {
            player2ScoreText.text = score.ToString();
        }
    }

    public void DelayCancel()
    {
        StartCoroutine(LeavingScene());
    }
    IEnumerator LeavingScene()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene(0);
    }


}
