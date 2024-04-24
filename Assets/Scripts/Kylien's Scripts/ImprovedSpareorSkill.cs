using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImprovedSpareorSkill : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject spareOrKillUI;

    public float rotateSpeed = 50f;

    private bool aKeyPressed = false;
    private bool dKeyPressed = false;

    void Update()
    {
        object1.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        object2.transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);

        // Check for input to hide text2
        if (Input.GetKeyDown(KeyCode.A) && !dKeyPressed)
        {
            SetTextVisibility(text2, false);
            aKeyPressed = true;
            DeactivateAfterDelay(spareOrKillUI, 2f);
        }

        // Check for input to hide text3
        if (Input.GetKeyDown(KeyCode.D) && !aKeyPressed)
        {
            SetTextVisibility(text3, false);
            dKeyPressed = true;
            DeactivateAfterDelay(spareOrKillUI, 2f);
        }
    }

    void SetTextVisibility(GameObject textObject, bool isVisible)
    {
        textObject.SetActive(isVisible);
    }

    void DeactivateAfterDelay(GameObject obj, float delay)
    {
        StartCoroutine(DeactivateObject(obj, delay));
    }

    IEnumerator DeactivateObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}