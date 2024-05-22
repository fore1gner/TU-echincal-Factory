using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyJumpscare : MonoBehaviour
{
    [Header("Settings")]
    public GameObject CreepyBaby;
    public GameObject Player;
    public bool used = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && used == false)
        {
            CreepyBaby.SetActive(true);
            used = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && used == true)
        {
            CreepyBaby.SetActive(false);
        }
    }


}
