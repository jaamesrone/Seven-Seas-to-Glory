using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpareOrKill : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject currentEnemy;
    public Player player;
    private bool hasMadeChoice = false;

    public TextMeshProUGUI recruitText;
    public TextMeshProUGUI choiceText;

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
                player.recruits++;
                UpdateRecruitText();
                Debug.Log("Sparing enemy");
                hasMadeChoice = true;
                killGain.Spare();
                choiceText.gameObject.SetActive(false);
                isPaused = false;
                Time.timeScale = 1f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Implement the logic for not sparing the enemy
                Debug.Log("Not sparing enemy");
                hasMadeChoice = true;
                killGain.Kill();
                choiceText.gameObject.SetActive(false);
                isPaused = false;
                Time.timeScale = 1f;
            }
        }
    }

    private void UpdateRecruitText()
    {
        recruitText.text = "Recruits: " + player.recruits;
    }



}
