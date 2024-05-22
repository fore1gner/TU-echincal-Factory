using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    [Header("Settings")]
    public GameObject OpenDoorUI;
    public Animator animator;

    private bool opened;
    

    // Start is called before the first frame update
    void Start()
    {
        opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ReachTool")
        {
            OpenDoorUI.SetActive(true);
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ReachTool")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (opened)
                {
                    animator.SetBool("close", true);
                    animator.SetBool("open", false);
                    opened = false;
                }else
                {
                    animator.SetBool("close", false);
                    animator.SetBool("open", true);
                    opened = true;
                }
                
                Debug.Log("anayın amı");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ReachTool")
        {
            OpenDoorUI.SetActive(false);
        }
    }
}
