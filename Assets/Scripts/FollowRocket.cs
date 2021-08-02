using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour
{

    public GameObject rocket;
    public float yOffset = -5;
   

    // Update is called once per frame
    void Update()
    {
        transform.position = rocket.transform.position + new Vector3(0, yOffset, 0);
    }
}
