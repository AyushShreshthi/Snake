using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField] private GameObject staticFood;
    [SerializeField] private GameObject dynamicFood;

    bool nowStaticFood = true;

    private void Update()
    {
        
        if (transform.childCount <= 0)
        {
            if (nowStaticFood)
            {
                float randX = Random.Range(-20, 20);
                float randZ = Random.Range(-20, 20);

                GameObject go = Instantiate(staticFood.gameObject, new Vector3(randX, 0, randZ), Quaternion.identity);
                go.transform.SetParent(this.transform);

                nowStaticFood = false;
            }
            else
            {
                float randX = Random.Range(-20, 20);
                float randZ = Random.Range(-20, 20);

                GameObject go = Instantiate(dynamicFood.gameObject, new Vector3(randX, 0, randZ), Quaternion.identity);
                go.transform.SetParent(this.transform);

                nowStaticFood = true;
            }
        }
    }

}
