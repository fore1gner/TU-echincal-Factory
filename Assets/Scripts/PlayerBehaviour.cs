using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerBehaviour : MonoBehaviour
{

    private FirstPersonController firstPersonController;

    [Header("Health Settings")]
    public GameObject healthBar;
    public float health = 100;
    public float healthMax = 100;
    public float healValue = 5;
    public float secondToHeal = 10;

    [Header("Flashlight Battery Settings")]
    public GameObject Flashlight;
    public float battery = 100;
    public float batteryMax = 100;
    public float removeBatteryValue = 0.05f;
    public float secondToRemoveBaterry = 5f;

    [Header("Stamina Settings")]
    public GameObject staminaBar;
    public float stamina = 100;
    public float staminaMax = 100;
    public float removeStaminaValue = 1;
    public float fillStaminaValue = 10;
    public float secondToRecargeStamina = 2f;

    [Header("Audio Settings")]
    public AudioClip cameraNoise;
    public AudioClip enemyNoise;
    public AudioClip damageNoise;
    public AudioClip dyingNoise;
    public AudioClip batteryGoingDown;
    public AudioClip scream;

    [Header("Page System Settings")]
    public List<GameObject> pages = new List<GameObject>();
    public int collectedPages;
    public GlitchEffect glitcheffect;

    [Header("UI Settings")]
    public GameObject inGameMenuUI;
    public GameObject pickUpUI;
    public GameObject finishedGameUI;
    public GameObject pagesCount;
    public Text pickupbattery;
    public bool paused;
    public GameObject StoryLine;
    Scene scene;
    void Start ()
    {
        // set initial health values
        health = healthMax;
        battery = batteryMax;
        stamina = staminaMax;
        scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);

        healthBar.GetComponent<Image>().fillAmount = health / healthMax;

        staminaBar.GetComponent<Image>().fillAmount = stamina / staminaMax;
        firstPersonController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<FirstPersonController>();

        // start consume flashlight battery
        StartCoroutine(RemoveBaterryCharge(removeBatteryValue, secondToRemoveBaterry));
        StartCoroutine(StartHealPlayer(25, 30));
        StartCoroutine(FillStaminaCharge(fillStaminaValue, secondToRecargeStamina));
    }
	
	void Update ()
    {
        // update player health slider
        healthBar.GetComponent<Image>().fillAmount = health / healthMax;

        staminaBar.GetComponent<Image>().fillAmount = stamina / staminaMax;
        if (stamina <= 10)
        {
            firstPersonController.m_RunSpeed = firstPersonController.m_WalkSpeed;
        }
        else
        {
            firstPersonController.m_RunSpeed = 6f;
        }

        if (firstPersonController.m_IsWalking == false)
        {
            RemoveStamina(removeStaminaValue);
        }

        // if health is low than 0
        if (health / healthMax * 100 <= 0)
        {
            Debug.Log("You are dead.");
            this.GetComponent<AudioSource>().PlayOneShot(dyingNoise);
            health = 0.0f;
        }

        // if battery out%
        if (battery / batteryMax * 100 <= 0)
        {
            battery = 0.00f;
            Debug.Log("The flashlight battery is out and you are out of the light.");
            Flashlight.transform.Find("Spotlight").gameObject.GetComponent<Light>().intensity = 0.0f;
        }
        


           // pagesCount.GetComponent<Text>().text = "Collected Flowers: " + collectedPages + "/8";
        
        //animations
        if (Input.GetKey(KeyCode.LeftShift))
            this.gameObject.GetComponent<Animation>().CrossFade("Run", 1);
        else
            this.gameObject.GetComponent<Animation>().CrossFade("Idle", 1);
        if (collectedPages >= 8)
        {
            glitcheffect.enabled=true;
            this.GetComponent<AudioSource>().PlayOneShot(cameraNoise);
            //this.GetComponent<AudioSource>().loop = true;
            StartCoroutine(LoadLevelAfterDelay(3));

        }

        if(collectedPages == 1 && scene.buildIndex == 2)
        {
            StoryLine.SetActive(false);
        }

        // collected all pages
        if (collectedPages == 5 && scene.buildIndex==2)
        {
            Debug.Log("You finished the game, congratulations...");
            Cursor.visible = true;

            // disable first person controller and show finished game UI
            this.gameObject.GetComponent<FirstPersonController>().enabled = false;
            inGameMenuUI.SetActive(false);
            finishedGameUI.SetActive(true);       

            // set play again button
            Button playAgainBtn = finishedGameUI.gameObject.transform.Find("PlayAgainBtn").GetComponent<Button>();
            playAgainBtn.onClick.AddListener(this.gameObject.GetComponent<MenuInGame>().PlayAgain);

            // set quit button
            Button quitBtn = finishedGameUI.gameObject.transform.Find("QuitBtn").GetComponent<Button>();
            quitBtn.onClick.AddListener(this.gameObject.GetComponent<MenuInGame>().QuitGame);
        } 

    }

    public void RemoveStamina(float value)
    {
       // Debug.Log("Removing stamina value: " + value);

        if (stamina > 0)
            stamina -= value;
       // else
          //  Debug.Log("The stamina is out");
    }

    public IEnumerator FillStaminaCharge(float value, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (firstPersonController.m_IsWalking == true)
            {
             //   Debug.Log("Filling stamina value: " + value);

                if (stamina >= -0.3f && stamina < staminaMax)
                    stamina += value;
                //else
                  //  stamina = staminaMax;
            }
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }
    public IEnumerator RemoveBaterryCharge(float value, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            Debug.Log("Removing baterry value: " + value);

            if (battery > 0)
                battery -= value;
            else
                Debug.Log("The flashlight battery is out");
        }
    }

    public IEnumerator RemovePlayerHealth(float value, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            Debug.Log("Removing player health value: " + value);

            if (health > 0)
                health -= value;
            else
            {
                Debug.Log("You're dead");
                paused = true;
                inGameMenuUI.SetActive(true);
                inGameMenuUI.transform.Find("ContinueBtn").gameObject.GetComponent<Button>().interactable = false;
                inGameMenuUI.transform.Find("PlayAgainBtn").gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    // function to heal player
    public IEnumerator StartHealPlayer(float value, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            Debug.Log("Healling player value: " + value);

            if (health > 0 && health < healthMax)
                health += value;
            else
                health = healthMax;
        }
    }

    // page system - show UI
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.transform.tag == "CreepyMan")
        {
            if (health > 0 && paused == false)
            {
                this.GetComponent<AudioSource>().PlayOneShot(cameraNoise);
                this.GetComponent<AudioSource>().loop = true;
            }            
        }

        if (collider.gameObject.transform.tag == "Page")
        {
            Debug.Log("You Found a Page: " + collider.gameObject.name + ", Press 'E' to pickup");
            pickUpUI.SetActive(true);
            pickupbattery.text = "Press 'E' to pickup Page";
        }
        if (collider.gameObject.transform.tag == "Battery")
        {
            Debug.Log("You Found a Page: " + collider.gameObject.name + ", Press 'E' to pickup");
            pickupbattery.text = "Press E to pick up battery";
            pickUpUI.SetActive(true);
            
        }

        if (collider.gameObject.transform.tag == "CreepyMan")
        {
            if (health <= 0)
            {
                Debug.Log("You're dead");
                this.GetComponent<AudioSource>().PlayOneShot(dyingNoise);
                paused = true;
                inGameMenuUI.SetActive(true);
                inGameMenuUI.transform.Find("ContinueBtn").gameObject.GetComponent<Button>().interactable = false;
                inGameMenuUI.transform.Find("PlayAgainBtn").gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                this.GetComponent<AudioSource>().PlayOneShot(damageNoise);
                health -= 25;
            }

        }
        
    }

    // page system - pickup system
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.transform.tag == "Page")
        {       
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("You get this page: " + collider.gameObject.name);

                // disable UI
                pickUpUI.SetActive(false);

                // add page to list
                pages.Add(collider.gameObject);
                collectedPages ++;

                // disable game object
                collider.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // remove noise sound
        if (collider.gameObject.transform.tag == "CreepyMan")
        {
            if (health > 0 && paused == false)
            {
                this.GetComponent<AudioSource>().clip = null;
                this.GetComponent<AudioSource>().loop = false;
            }          
        }

        // disable UI
        if (collider.gameObject.transform.tag == "Page")
            pickUpUI.SetActive(false);
    }
}
