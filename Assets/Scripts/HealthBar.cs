using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// goes in the background element in the health bar

public class HealthBar : MonoBehaviour
{
    private RectTransform rectTransform; // the child element's rect transform
    private Vector2 originalSize;

    public void Refresh(float ratio){ // updates the health bar, 0 represents all red, while 1 represents all green
        ratio *= Screen.width;
        rectTransform.sizeDelta = new Vector2(ratio,originalSize.y); // change the size of the healthbar based on the player's health
    }

    private void Start() {
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
    }
}
