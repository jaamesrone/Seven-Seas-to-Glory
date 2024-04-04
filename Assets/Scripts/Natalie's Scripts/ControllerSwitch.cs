using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerSwitch : MonoBehaviour
{
    //Temporary Tutorial
    public TextMeshProUGUI GuideText;
    public GameObject ReticleImage; // Reference to the reticle image

    [SerializeField] private Transform playerSpawnPoint; 
    [SerializeField] private TMP_Text dialogueText;

    public GameObject Ship;
    public GameObject Character;
    public GameObject Camera;

    public bool InCannon = false;
    public bool InCharacter = false;
    public bool InShip = false;
    public bool CanDriveShip = false;
    public bool LeftCannon = false;
    private bool awaitingCombatDecision = false;
    private bool isCooldownActive = false;


    //Locations for cam stored in gameobjects so they can move with ship and character
    public GameObject CharacterCam;
    public GameObject ShipCam;
    public GameObject LeftCannonCam;
    public GameObject RightCannonCam;

    private void Start()
    {
        InCharacter = true;
        Character.GetComponent<PlayerController>().enabled = true;
        Camera.GetComponent<FiringMode>().enabled = false;
        Ship.GetComponent<ShipController>().enabled = false;

        // Disable the reticle image at the start of the game
        ReticleImage.SetActive(false);
    }

    void Update()
    {
        //Ship & Cannon switch
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (InCannon && !InShip && !InCharacter)
            {
                SwitchToShip();
            }
            else if (InShip && !InCannon && !InCharacter)
            {
                SwitchToCannon();
            }
        }
        //Cannon side switch/ Switch to Character
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InCharacter && CanDriveShip && !InCannon && !InShip)
            {
                SwitchToShip();
            }
            else if (InShip && !InCannon && !InCharacter)
            {
                SwitchToCharacter();
                GuideText.text = "Press E at wheel to drive ship";
            }
            else if (InCannon && !InShip && !InCharacter)
            {
                SwitchSides();
            }
        }

        if (awaitingCombatDecision)//james' script
        {
            if (Input.GetKeyDown(KeyCode.Y))//if player presses Y switch to combat mode and spawn on the ship
            {
                SwitchToCombat();
                Character.transform.position = playerSpawnPoint.position;
                dialogueText.text = ""; 
                awaitingCombatDecision = false;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                // ignore combat and stay in ship
                dialogueText.text = ""; 
                awaitingCombatDecision = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wheel")
        {
            CanDriveShip = false;
        }
        if (other.CompareTag("HandtoHand"))//james' script
        {
            dialogueText.text = "";
            awaitingCombatDecision = false;
            other.GetComponentInParent<EnemyShipAI>().SetHandToHandCombat(false);//pirate ai ship goes back to ai state
            other.gameObject.transform.parent.GetComponent<EnemyShipAI>().speed = 2;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wheel")
        {
            CanDriveShip = true;
        }
        else if (other.tag == "HandtoHand" && !isCooldownActive && !awaitingCombatDecision) //if player enters the trigger dialogue pops up asking a question
        {//james' script
            other.GetComponentInParent<EnemyShipAI>().SetHandToHandCombat(true);//stops the pirate ai ship from shooting cannonballs
            other.gameObject.transform.parent.GetComponent<EnemyShipAI>().speed = 0;
            dialogueText.text = "Do you want to engage in hand-to-hand combat? (Y/N)";
            awaitingCombatDecision = true;
            StartCoroutine(DialogueCooldown());
        }
    }

    IEnumerator DialogueCooldown() //60second cooldown timer for the dialoguetext to pop up again if you're in the collider
    {//james' script
        isCooldownActive = true; 
        yield return new WaitForSeconds(2); 
        isCooldownActive = false;
        awaitingCombatDecision = false;
    }


    void SwitchToCombat() //james' script
    {
        // this function only happens if player presses Y
        Character.transform.parent = null;
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        Ship.GetComponent<ShipController>().enabled = false;
        Camera.GetComponent<FiringMode>().enabled = false;
        Character.GetComponent<PlayerController>().enabled = true;
        InCannon = false;
        InShip = false;
        InCharacter = true;
        ReticleImage.SetActive(false); // Hide the reticle image
    }


    void SwitchToCharacter()
    {
        Character.transform.parent = null;
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        Ship.GetComponent<ShipController>().enabled = false;
        Camera.GetComponent<FiringMode>().enabled = false;
        Character.GetComponent<PlayerController>().enabled = true;
        InCannon = false;
        InShip = false;
        InCharacter = true;
        ReticleImage.SetActive(false); // Hide the reticle image
    }

    void SwitchToShip()
    {
        Character.transform.parent = Ship.transform;
        Camera.transform.parent = Ship.transform;
        Camera.transform.localPosition = ShipCam.transform.localPosition;
        Camera.transform.localEulerAngles = ShipCam.transform.localEulerAngles;
        Character.GetComponent<PlayerController>().enabled = false;
        Camera.GetComponent<FiringMode>().enabled = false;
        Ship.GetComponent<ShipController>().enabled = true;
        InCharacter = false;
        InCannon = false;
        InShip = true;
        GuideText.text = "Press Shift to switch to cannon or sail close to enemies for hand-to-hand combat";
        ReticleImage.SetActive(false); // Hide the reticle image
    }

    void SwitchToCannon()
    {
        if (LeftCannon)
        {
            Camera.transform.localPosition = LeftCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = LeftCannonCam.transform.localEulerAngles;
        }
        else
        {
            Camera.transform.localPosition = RightCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = RightCannonCam.transform.localEulerAngles;
        }
        Character.GetComponent<PlayerController>().enabled = false;
        Ship.GetComponent<ShipController>().enabled = false;
        Camera.GetComponent<FiringMode>().enabled = true;
        ReticleImage.SetActive(true); // Show the reticle image
        InCharacter = false;
        InShip = false;
        InCannon = true;
        GuideText.text = "Press Space to shoot and Press E to switch sides. Use 1, 2, 3 to switch ammo.";
    }

    void SwitchSides()
    {
        if (LeftCannon)
        {
            Camera.transform.localPosition = RightCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = RightCannonCam.transform.localEulerAngles;
            LeftCannon = false;
        }
        else
        {
            Camera.transform.localPosition = LeftCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = LeftCannonCam.transform.localEulerAngles;
            LeftCannon = true;
        }
    }
}
