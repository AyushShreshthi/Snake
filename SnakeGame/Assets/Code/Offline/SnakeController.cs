using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour {


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

    bool GameEnd = false;
    [SerializeField] private GameObject gameEndPanel;

    // Start is called before the first frame update
    void Start() {
        GrowSnake();
        GrowSnake();
    }

    // Update is called once per frame
    void Update() {

        if (GameEnd) return;


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
        ///float steerDirection = Input.GetAxis("Horizontal");

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
        GameObject body = Instantiate(BodyPrefab,transform.position,transform.rotation);
        BodyParts.Add(body);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player")
        {
            GameEnd = true;

            gameEndPanel.SetActive(true);
        }
    }

    private int score = 0;
    public TMP_Text scoreText;
    public void EarnPoint()
    {
        score++;
        scoreText.text = "Score - " + score.ToString();
        MoveSpeed += 0.2f;
        BodySpeed += 0.2f;
    }
    public void MenuBtn()
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