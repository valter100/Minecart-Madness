using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int scoreAmount;
    [SerializeField] GameObject scoreCanvas;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Spell")
    //    {
    //        GiveScore();
    //        Destroy(gameObject);
    //    }
    //}

    public void GiveScore()
    {
        FindObjectOfType<GameManager>().ChangeScore(scoreAmount);
        GameObject scoreCan = Instantiate(scoreCanvas, transform.position, transform.rotation);
        scoreCan.transform.SetParent(null, true);
        scoreCan.transform.position = transform.position;

        scoreCan.GetComponentInChildren<TMP_Text>().text = (scoreAmount * GameManager.scoreMultiplier).ToString();

        Destroy(gameObject.transform.parent.gameObject);
    }
}
