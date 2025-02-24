using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card
{
    private readonly cardSpawn cardType;
    public Card(cardSpawn cardType)
    {
        this.cardType = cardType;
    }

    public Sprite Sprite { get ->cardType.Sprite; }

}
