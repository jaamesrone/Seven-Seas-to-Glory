using System.Collections;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public PlayerClass playerClass;
    public float recoveryRate = 5f;
    public float recoveryInterval = 1f;

    private bool recovering = false;

    private void Start()
    {
        StartCoroutine(RecoverHealth());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerClass.health = 50f;
        }
    }

    private IEnumerator RecoverHealth()
    {
        while (true)
        {
            if (!recovering && playerClass.health < 100f)
            {
                recovering = true;
                StartCoroutine(ApplyRecovery());
            }
            yield return null;
        }
    }

    private IEnumerator ApplyRecovery()
    {
        while (playerClass.health < 100f)
        {
            playerClass.health += recoveryRate;
            playerClass.health = Mathf.Min(playerClass.health, 100f);
            yield return new WaitForSeconds(recoveryInterval);
        }
        recovering = false;
    }
}