using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpareOrKill : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject currentEnemy;
    private int recruits = 0;
    private bool hasMadeChoice = false;

    public TextMeshProUGUI recruitText;
    public TextMeshProUGUI choiceText;

    //For death drops
    public KillGain killGain;

    private void Start()
    {
        UpdateRecruitText();
        choiceText.gameObject.SetActive(false); // Hide the choice text at the beginning
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isPaused)
            {
                // Pause the game
                isPaused = true;
                Time.timeScale = 0f;
                choiceText.text = "A to Spare / D to Kill";
                choiceText.gameObject.SetActive(true);
            }
        }

        if (isPaused && !hasMadeChoice)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Implement the logic for sparing the enemy
                recruits++;
                UpdateRecruitText();
                Debug.Log("Sparing enemy");
                hasMadeChoice = true;
                choiceText.gameObject.SetActive(false);
                isPaused = false;
                Time.timeScale = 1f;
                killGain.Spare();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Implement the logic for not sparing the enemy
                Debug.Log("Not sparing enemy");
                hasMadeChoice = true;
                choiceText.gameObject.SetActive(false);
                isPaused = false;
                Time.timeScale = 1f;
                killGain.Kill();
            }
        }
    }

    private void UpdateRecruitText()
    {
        recruitText.text = "Recruits: " + recruits;
    }
}
