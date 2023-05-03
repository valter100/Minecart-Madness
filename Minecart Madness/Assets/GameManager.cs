using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    static NetworkVariable<int> score = new NetworkVariable<int>();
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text multiplierText;
    public static float scoreMultiplier;
    [SerializeField] float maximumMultiplier;
    [SerializeField] float multiplierIncrements;

    public void Start()
    {
        scoreText.text = "Score: 0";
        scoreMultiplier = 1;
        multiplierText.text = "x" + scoreMultiplier;
    }

    public void ChangeScore(int scoreChange)
    {
        score.Value += (int)(scoreChange * scoreMultiplier);
        if(scoreMultiplier < maximumMultiplier)
        {
            scoreMultiplier = Mathf.Clamp(scoreMultiplier + multiplierIncrements, 1, maximumMultiplier);
            multiplierText.text = "x" + scoreMultiplier;
        }

        scoreText.text = "Score: " + score.Value;
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = 1.0f;
        multiplierText.text = "x" + scoreMultiplier;
    }


}
