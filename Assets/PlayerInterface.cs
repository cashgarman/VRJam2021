using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{

    public bool occupied = false;
    public GameObject helo;
    public GameObject player;
    public GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (occupied)
        {
            transform.position = player.transform.position;
        }
        else
        {
           
        }
    }

    public void TakeASeat()
    {
        Debug.Log("TakeASeat called");
        occupied = true;
        controls.SetActive(true);
        player.transform.position = helo.transform.position;
    }

    public void Eject()
    {
        occupied = false;
        controls.SetActive(false);
        player.transform.position = helo.transform.position + new Vector3(0, 3, 0); 
    }
}
