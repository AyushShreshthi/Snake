using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField] private FoodItem foodItems;

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            float randX = Random.Range(-20, 20);
            float randZ = Random.Range(-20, 20);

            GameObject go = Instantiate(foodItems.gameObject, new Vector3(randX, 0, randZ), Quaternion.identity);
            go.transform.SetParent(this.transform);

        }
    }
}
