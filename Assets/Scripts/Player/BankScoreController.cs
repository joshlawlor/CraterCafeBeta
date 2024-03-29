using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BankScoreController : MonoBehaviour
{
    public TextMeshProUGUI bankScoreText;

    private int bankScore = 0;

    private void FixedUpdate()
    {
        UpdateBankScoreUI();
    }

    public void UpdateBankScore(int amount)
    {
        bankScore += amount;
        UpdateBankScoreUI();
    }

    private void UpdateBankScoreUI()
    {
        if (bankScoreText != null)
        {
            bankScoreText.text = "$ " + bankScore;
        }
    }
    public int GetBankScore()
    {
        return bankScore;
    }

    public void SetBankScore(int newBankScore)
    {
        bankScore = newBankScore;
        UpdateBankScoreUI();
    }
}
