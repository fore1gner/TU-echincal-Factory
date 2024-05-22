using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MobController : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private NavMeshAgent Mob;

    public AudioClip scream;

    public GameObject Player;
    bool triggered;
    

    public float MobDistanceRun = 4.0f;

    void Start()
    {
        triggered = false;
        playerBehaviour = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerBehaviour>();
        Mob = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if(distance < MobDistanceRun)
        {
            triggered = true;
            Player.GetComponent<AudioSource>().PlayOneShot(scream);
            Vector3 dirToPlayer = transform.position - Player.transform.position;

            Vector3 newPos = transform.position - dirToPlayer;

            Mob.SetDestination(newPos);
        } else
        {
            if(triggered)
                this.gameObject.SetActive(false);
        }
    }

    
}
