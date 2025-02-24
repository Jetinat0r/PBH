using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BHP // lol I thought it was bullet hell party but it was party bullet hell
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public CardType cardType;

        public enum CardType // Defines bullet pattern
        {
            Horizontal,
            Vertical,
            Cross,
            X
        }


        
    }
}