using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int scoreAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "spell")
        {
            FindObjectOfType<GameManager>().ChangeScore(scoreAmount);
            Destroy(gameObject);
        }
    }

    public void GiveScore()
    {
        FindObjectOfType<GameManager>().ChangeScore(scoreAmount);
        Destroy(gameObject);
    }
}
