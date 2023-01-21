using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class OnlineSnakeController : MonoBehaviourPunCallbacks {

    public int playerNum = 0;
    // Settings
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 10;

    // References
    public GameObject BodyPrefab;

    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start() {

        if (!photonView.IsMine)
            return;

        playerNum = OnlineManager.om.playerNum;
        //GrowSnake();
        //GrowSnake();
    }

    // Update is called once per frame
    void Update() {

        if (!photonView.IsMine)
            return;

        // Move forward
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;/// * Input.GetAxis("Vertical");
        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }

        float steerDirection;// = Input.GetAxis("Horizontal");
        
        float halfWidth = Screen.width / 2;

        var touch = Input.GetTouch(0);
        
        var touchPos = touch.position;
        float valuedAD;
        if (touchPos.x < halfWidth)
        {
            valuedAD = -1;
        }
        else
        {
            valuedAD = 1;
        }

        steerDirection = valuedAD * (touchPos.x / halfWidth);

        // Returns value -1, 0, or 1
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        
    }

    public void GrowSnake() {
        // Instantiate body instance and
        // add it to the list

        if (!photonView.IsMine) return;

        GameObject body = PhotonNetwork.Instantiate(BodyPrefab.name,transform.position,transform.rotation);
        BodyParts.Add(body);
    }
    public void EarnPoint()
    {
        if (!photonView.IsMine) return;

        OnlineManager.om.ScoreUpdate(playerNum);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" )
        {
            transform.position = OnlineManager.om.playerPos[playerNum].position;
        }
        
    }

}