using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public Image cardImage;

    public void setImage(Sprite newSprite)
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

}
