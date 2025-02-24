using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour
{
    public CardData cardData;
    [SerializeField]
    public CardDisplay display; //Currently just an image, will likely have more functionality
    [SerializeField]
    public EventTrigger cardClickEventTrigger;

    public void Init(CardData _cardData)
    {
        cardData = _cardData;
        display.SetImage(_cardData.sprite);
    }

    public void SelectCard()
    {
        display.SelectCard();
    }

    public void DeselectCard()
    {
        display.DeselectCard();
    }
}