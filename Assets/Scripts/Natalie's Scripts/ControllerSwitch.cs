using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSwitch : MonoBehaviour
{
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
        //Ship.GetComponent<FiringMode>().enabled = false;
        //Ship.GetComponent<ShipController>().enabled = false;
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
            if(InCharacter && CanDriveShip && !InCannon && !InShip)
            {
                SwitchToShip();
            }
            else if(InShip && !InCannon && !InCharacter)
            {
                SwitchToCharacter();
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
            Debug.Log("Drive Ship?");
            CanDriveShip = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wheel")
        {
            Debug.Log("Can't Drive");
            CanDriveShip = false;
        }
    }

    private void SwitchToCharacter()
    {
        Camera.transform.parent = Character.transform;
        Camera.transform.localPosition = CharacterCam.transform.localPosition;
        Camera.transform.localEulerAngles = CharacterCam.transform.localEulerAngles;
        //Ship.GetComponent<FiringMode>().enabled = false;
        //Ship.GetComponent<ShipController>().enabled = false;
        Character.GetComponent<PlayerController>().enabled = true;
        Debug.Log("In Ship");
        InCannon = false;
        InShip = false;
        InCharacter = true;
    }

    private void SwitchToShip()
    {
        Camera.transform.parent = Ship.transform;
        Camera.transform.localPosition = ShipCam.transform.localPosition;
        Camera.transform.localEulerAngles = ShipCam.transform.localEulerAngles;
        Character.GetComponent<PlayerController>().enabled = false;
        //Ship.GetComponent<FiringMode>().enabled = false;
        //Ship.GetComponent<ShipController>().enabled = true;
        Debug.Log("In Ship");
        InCharacter = false;
        InCannon = false;
        InShip = true;
    }

    private void SwitchToCannon()
    {
        Camera.transform.parent = Ship.transform;
        //switch to cannon last used
        if(LeftCannon)
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
        //Ship.GetComponent<ShipController>().enabled = false;
        //Ship.GetComponent<FiringMode>().enabled = true;
        Debug.Log("In Cannon");
        InCharacter = false;
        InShip = false;
        InCannon = true;
    }

    void SwitchSides()
    {
        if(LeftCannon)
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
