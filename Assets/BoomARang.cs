using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomARang : MonoBehaviour
{
    private bool flying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 1000f);
        }
    }

    public void Thrown()
    {
        flying = true;
    }

    public void Grabbed()
    {
        flying = false;
    }
}
