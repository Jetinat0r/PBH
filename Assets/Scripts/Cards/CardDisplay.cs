using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public Image cardImage;

    public void SetImage(Sprite newSprite)
    {
        if (cardImage != null)
        {
            cardImage.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Card Image is NULL!");
        }
    }

    public void SelectCard()
    {
        cardImage.color = Color.yellow;
        transform.localPosition = new Vector3(0f, 50f, 0f);
    }

    public void DeselectCard()
    {
        cardImage.color = Color.white;
        transform.localPosition = Vector3.zero;
    }
}
