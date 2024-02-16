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

    //Locations for cam stored in gameobjects so they can move with ship and character
    public GameObject CharacterCam;
    public GameObject ShipCam;
    public GameObject LeftCannonCam;
    public GameObject RightCannonCam;

    private void Start()
    {
        //InCharacter = true;
        InShip = true;
        Character.GetComponent<PlayerController>().enabled = false;
        //Ship.GetComponent<FiringMode>().enabled = false;
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
        //Cannon side switch/ Character & Ship switch
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //
        //}
    }

    void SwitchToShip()
    {
        Camera.transform.parent = Ship.transform;
        Camera.transform.localPosition = ShipCam.transform.localPosition;
        Camera.transform.localEulerAngles = ShipCam.transform.localEulerAngles;
        Character.GetComponent<PlayerController>().enabled = false;
        //Ship.GetComponent<FiringMode>().enabled = false;
        //Ship.GetComponent<ShipController>().enabled = true;
        Debug.Log("In Ship");
        InCannon = false;
        InShip = true;
    }

    void SwitchToCannon()
    {
        Camera.transform.parent = Ship.transform;
        Camera.transform.localPosition = LeftCannonCam.transform.localPosition;
        Camera.transform.localEulerAngles = LeftCannonCam.transform.localEulerAngles;
        Character.GetComponent<PlayerController>().enabled = false;
        //Ship.GetComponent<ShipController>().enabled = false;
        //Ship.GetComponent<FiringMode>().enabled = true;
        Debug.Log("In Cannon");
        InShip = false;
        InCannon = true;
    }

    //void SwitchSides()
    //{
    //
    //}
}
