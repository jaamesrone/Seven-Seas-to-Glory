using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerSwitch : MonoBehaviour
{
    //Temporary Tutorial
    public TextMeshProUGUI GuideText;
    public GameObject ReticleImage; // Reference to the reticle image

    public GameObject Ship;
    public GameObject Character;
    public GameObject Camera;

    public bool InCannon = false;
    public bool InCharacter = false;
    public bool InShip = false;
    public bool CanDriveShip = false;
    public bool LeftCannon = false;

    //Locations for cam stored in gameobjects so they can move with ship and character
    public GameObject CharacterCam;
    public GameObject ShipCam;
    public GameObject LeftCannonCam;
    public GameObject RightCannonCam;

    private void Start()
    {
        InCharacter = true;
        Character.GetComponent<PlayerController>().enabled = true;

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wheel")
        {
            CanDriveShip = true;
        }
        else if (other.tag == "HandtoHand")
        {
            other.gameObject.transform.parent.GetComponent<EnemyShipAI>().speed = 0;
            SwitchToCharacter();
            GuideText.text = "Fight with the LMB and block with RMB";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wheel")
        {
            CanDriveShip = false;
        }
    }

    void SwitchToCharacter()
    {
        Character.transform.parent = null;
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        Ship.GetComponent<ShipController>().enabled = false;
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
