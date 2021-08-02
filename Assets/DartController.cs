using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "DartBoard")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Achievements.Award("BullsEye");
        }
    }
}
