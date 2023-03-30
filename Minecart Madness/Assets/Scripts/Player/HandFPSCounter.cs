using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandFPSCounter : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float refreshRate = 4f;

    private HandController handController;
    private float timer;

    private void Start()
    {
        handController = GetComponentInParent<HandController>();
    }

    private void Update()
    {
        if (handController.PrimaryPressed)
        {
            if (!canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(true);
                timer = 0f;
            }
            else
            {
                timer -= Time.unscaledDeltaTime;
            }

            if (timer <= 0f)
            {
                timer += 1f / refreshRate;
                int fps = (int)(1f / Time.unscaledDeltaTime);
                fpsText.text = fps.ToString();

                if (fps > 30)
                    fpsText.color = Color.green;
                else if (fps > 20)
                    fpsText.color = Color.yellow;
                else
                    fpsText.color = Color.red;
            }
        }
        else
        {
            if (canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(false);
            }
                
        }
    }
}
