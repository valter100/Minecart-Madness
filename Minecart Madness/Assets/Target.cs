using System.Collections;
using System.Collections.Generic;
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
        //GameObject tempCanvas = Instantiate(scoreCanvas, transform.position, transform.localRotation);
        //tempCanvas.transform.SetParent(null);
        //tempCanvas.GetComponent<Animator>().Play("Popup Text"); //FIX
        Destroy(gameObject.transform.parent.gameObject);
    }
}
