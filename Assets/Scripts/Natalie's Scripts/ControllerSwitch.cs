using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerSwitch : MonoBehaviour
{
    //Temporary Tutorial
    public TextMeshProUGUI GuideText;
    public GameObject ReticleImage; // Reference to the reticle image
    public GameObject reload;

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private TMP_Text dialogueText;

    public GameObject Ship;
    public GameObject Character;
    public GameObject Camera;
    public GameObject cannonLeft;
    public GameObject cannonRight;

    public bool InCannon = false;
    public bool InCharacter = false;
    public bool InShip = false;
    public bool CanDriveShip = false;
    public bool LeftCannon = true;
    private bool awaitingCombatDecision = false;
    private bool isCooldownActive = false;


    //Locations for cam stored in gameobjects so they can move with ship and character
    public GameObject CharacterCam;
    public GameObject ShipCam;
    public GameObject LeftCannonCam;
    public GameObject RightCannonCam;

    public PauseMenu menuOption;
    public InventoryUI inventoryActive;
    public GameObject playerHealthBar;
    public GameObject shipHealthBar;

    private void Start()
    {
        InCharacter = true;
        Character.GetComponent<PlayerController>().enabled = true;
        cannonLeft.GetComponent<Cannon>().enabled = false;
        cannonRight.GetComponent<Cannon>().enabled = false;
        Ship.GetComponent<ShipController>().isDriving = false;

        inventoryActive.UpdateActive(0);

        playerHealthBar.SetActive(true);
        shipHealthBar.SetActive(false);

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
        //shop no longer available
        if (other.gameObject.CompareTag("Shop"))
        {
            menuOption.Shop = false;
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
        //allows for shop
        if (other.gameObject.CompareTag("Shop"))
        {
            menuOption.Shop = true;

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
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        Ship.GetComponent<ShipController>().isDriving = false;
        Ship.GetComponent<ShipController>().currentForwardSpeed = 0;
        cannonLeft.GetComponent<Cannon>().enabled = false;
        cannonRight.GetComponent<Cannon>().enabled = false;
        Character.GetComponent<PlayerController>().enabled = true;
        inventoryActive.UpdateActive(inventoryActive.lastCombatIndex);
        InCannon = false;
        InShip = false;
        InCharacter = true;
        playerHealthBar.SetActive(true);
        shipHealthBar.SetActive(false);
        ReticleImage.SetActive(false); // Hide the reticle image
        reload.SetActive(false);
    }


    void SwitchToCharacter()
    {
        Character.transform.localPosition = playerSpawnPoint.localPosition;
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        Ship.GetComponent<ShipController>().isDriving = false;
        Ship.GetComponent<ShipController>().currentForwardSpeed = 0;
        cannonLeft.GetComponent<Cannon>().enabled = false;
        cannonRight.GetComponent<Cannon>().enabled = false;
        Character.GetComponent<PlayerController>().enabled = true;
        inventoryActive.UpdateActive(inventoryActive.lastCombatIndex);
        InCannon = false;
        InShip = false;
        InCharacter = true;
        playerHealthBar.SetActive(true);
        shipHealthBar.SetActive(false);
        if (inventoryActive.lastCombatIndex == 1)
        {
            ReticleImage.SetActive(true);
        }
        else
        {
            ReticleImage.SetActive(false);
        }
        reload.SetActive(false);
    }

    void SwitchToShip()
    { 
        Character.transform.position = playerSpawnPoint.position;
        Camera.transform.parent = Ship.transform;
        Camera.transform.localPosition = ShipCam.transform.localPosition;
        Camera.transform.localEulerAngles = ShipCam.transform.localEulerAngles;
        Character.GetComponent<PlayerController>().enabled = false;
        cannonLeft.GetComponent<Cannon>().enabled = false;
        cannonRight.GetComponent<Cannon>().enabled = false;
        Ship.GetComponent<ShipController>().isDriving = true;
        inventoryActive.UpdateActive(inventoryActive.lastCombatIndex);
        InCharacter = false;
        InCannon = false;
        InShip = true;
        GuideText.text = "Press Shift to switch to cannon or sail close to enemies for hand-to-hand combat";
        shipHealthBar.SetActive(true);
        playerHealthBar.SetActive(false);
        ReticleImage.SetActive(false); // Hide the reticle image
        reload.SetActive(false);
    }

    void SwitchToCannon()
    {
        if (LeftCannon)
        {
            Camera.transform.localPosition = LeftCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = LeftCannonCam.transform.localEulerAngles;
            cannonLeft.GetComponent<Cannon>().enabled = true;
        }
        else
        {
            Camera.transform.localPosition = RightCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = RightCannonCam.transform.localEulerAngles;
            cannonRight.GetComponent<Cannon>().enabled = true;
        }
        Character.GetComponent<PlayerController>().enabled = false;
        Ship.GetComponent<ShipController>().isDriving = false;
        inventoryActive.UpdateActive(inventoryActive.lastCannonIndex);
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
            cannonLeft.GetComponent<Cannon>().enabled = false;
            cannonRight.GetComponent<Cannon>().enabled = true;
            LeftCannon = false;
        }
        else
        {
            Camera.transform.localPosition = LeftCannonCam.transform.localPosition;
            Camera.transform.localEulerAngles = LeftCannonCam.transform.localEulerAngles;
            cannonRight.GetComponent<Cannon>().enabled = false;
            cannonLeft.GetComponent<Cannon>().enabled = true;
            LeftCannon = true;
        }
    }
}
