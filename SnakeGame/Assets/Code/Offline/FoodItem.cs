using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FoodItem : MonoBehaviour
{
    public bool isDynamic = false;
    public bool isOnline = false;

    private void Update()
    {
        if (isDynamic)
        {
            Vector3 pos = transform.position;
            float bottomFloor = -20f;
            pos.z = Mathf.PingPong(Time.time * 5f, 40) + bottomFloor;
            transform.position = pos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isOnline)
        {
            other.GetComponent<SnakeController>().GrowSnake();
            other.GetComponent<SnakeController>().EarnPoint();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player" && isOnline)
        {
            other.GetComponent<OnlineSnakeController>().GrowSnake();
            other.GetComponent<OnlineSnakeController>().EarnPoint();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
