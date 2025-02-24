using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BHP;
using Unity.VisualScripting;

public class CardManager : MonoBehaviour
{
    public Transform cardPanel; //UI panel where cards are displayed
    public GameObject cardPrefab; // UI prefab for a card
    public Sprite[] cardSprites; // array to store card types
    public Canvas MainCanvasCard;
    private GameObject[] cards = new GameObject[5]; // Array to store card instances
    public GameObject selectedCard = null;

    

    void Start()
    {

        // Initialize with 5 cards
        for (int i = 0; i < 5; i++)
        {
            AddCard(i);
        }

        StartCoroutine(AddCardRoutine());
    }

    void Update()
    {
        // Number key selection (1-5)
        for (int i = 0; i < cards.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && cards[i] != null)
            {
                SelectCard(cards[i]);
            }
        }
    }

    public void AddCard(int index)
    {
        if (index >= cards.Length || cards[index] != null) return;

        GameObject newCard = Instantiate(cardPrefab, cardPanel);
        Debug.Log($"Card {index} instantiated successfully!");

        // Get CardDisplay script
        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogError("CardDisplay script is missing on the card prefab!");
            return;
        }

        // Assign a random card type image to the card
        if (cardSprites.Length > 0)
        {
            Sprite randomSprite = cardSprites[Random.Range(0, cardSprites.Length)];
            cardDisplay.setImage(randomSprite);

            Card.CardType cardType = GetCardTypeForSprite(randomSprite);

            // Create a new Card ScriptableObject and assign the card type based on the sprite
            Card cardObject = ScriptableObject.CreateInstance<Card>();
            cardObject.cardType = cardType;

            // Attach the Card 
            newCard.AddComponent<CardComponent>().card = cardObject;
        }

        // Get the RectTransform for CardCanvas from prefab
        RectTransform prefabCanvasRect = cardPrefab.transform.Find("CardCanvas").GetComponent<RectTransform>();
        RectTransform canvasRect = newCard.transform.Find("CardCanvas").GetComponent<RectTransform>();

        if (canvasRect == null || prefabCanvasRect == null)
        {
            Debug.LogError("CardCanvas RectTransform is missing!");
            return;
        }
        RectTransform mainCanvasRect = MainCanvasCard.GetComponent<RectTransform>(); // Get mainCanvasCard's RectTransform

        // Copy RectTransform values from prefab to new card
        canvasRect.anchorMin = mainCanvasRect.anchorMin;
        canvasRect.anchorMax = mainCanvasRect.anchorMax;
        canvasRect.pivot = mainCanvasRect.pivot;
        canvasRect.anchoredPosition = mainCanvasRect.anchoredPosition;
        canvasRect.sizeDelta = mainCanvasRect.sizeDelta;
        canvasRect.rotation = mainCanvasRect.rotation;
        canvasRect.localScale = mainCanvasRect.localScale;
        canvasRect.localPosition = Vector3.zero; 

        // Get the RectTransform for CardImage
        RectTransform imageRect = newCard.transform.Find("CardCanvas/CardImage").GetComponent<RectTransform>();
        if (imageRect == null)
        {
            Debug.LogError("CardImage is missing a RectTransform!");
            return;
        }

        //  CardImage's position increments x by 100 per card
        imageRect.localPosition = new Vector3(-200 + (index * 100), -310, 0);

       
        cards[index] = newCard;

        // click selection
        EventTrigger trigger = newCard.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = newCard.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) => SelectCard(newCard)); // Select card on click

        trigger.triggers.Add(entry);
    }

    private Card.CardType GetCardTypeForSprite(Sprite sprite)
    {
        // Set card type based on name of sprite
        if (sprite.name.Contains("Horizontal"))
        {
            return Card.CardType.Horizontal;
        }
        else if (sprite.name.Contains("Vertical"))
        {
            return Card.CardType.Vertical;
        }
        else if (sprite.name.Contains("Cross"))
        {
            return Card.CardType.Cross;
        }
        else
        {
            return Card.CardType.X;  // Supposed to be for a cross type but not implemented, so default
        }
    }

    private void SelectCard(GameObject card)
    {
       
        if (selectedCard == card)
        {
            DeselectCard();
            return;
        }

        if (selectedCard != null)
        {
            DeselectCard();
        }

        selectedCard = card;

        // Get the card and highlight it when selected
        Image selectedCardImage = selectedCard.transform.Find("CardCanvas/CardImage").GetComponent<Image>();
        if (selectedCardImage != null)
        {
            selectedCardImage.color = Color.yellow; 
        }

        // pop out effect
        RectTransform selectedCardRect = selectedCard.transform.Find("CardCanvas/CardImage").GetComponent<RectTransform>();
        selectedCardRect.localPosition = new Vector3(selectedCardRect.localPosition.x, -260, 0); 

        // Set the Canvas sorting order to make the selected card the topmost (Does not work)
        Canvas selectedCanvas = selectedCard.transform.Find("CardCanvas").GetComponent<Canvas>();
        selectedCanvas.sortingOrder = 10; 

        // Adjust sortingOrder for all other cards
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != selectedCard)
            {
                Canvas otherCanvas = cards[i].transform.Find("CardCanvas").GetComponent<Canvas>();
                if (otherCanvas != null)
                {
                    otherCanvas.sortingOrder = 0; // Reset sorting order for other cards
                }
            }
        }
    }

    private void DeselectCard()
    {
        if (selectedCard == null) return;

        // Reset the image color
        Image selectedCardImage = selectedCard.transform.Find("CardCanvas/CardImage").GetComponent<Image>();
        if (selectedCardImage != null)
        {
            selectedCardImage.color = Color.white; 
        }

        // Reset the position of the selected card
        RectTransform selectedCardRect = selectedCard.transform.Find("CardCanvas/CardImage").GetComponent<RectTransform>();
        selectedCardRect.localPosition = new Vector3(selectedCardRect.localPosition.x, -310, 0); // Reset Y position

        // Reset sorting order for the deselected card
        Canvas selectedCanvas = selectedCard.transform.Find("CardCanvas").GetComponent<Canvas>();
        if (selectedCanvas != null)
        {
            selectedCanvas.sortingOrder = 0; // Reset sorting order
        }

        // Reset the selected card to null
        selectedCard = null;
    }

    public void UseCardOnTile()
    {
        if (selectedCard == null) return;

        Debug.Log("Card used on tile!");

        // Remove the used card
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == selectedCard)
            {
                Destroy(selectedCard);
                cards[i] = null; // Free up slot
                break;
            }
        }

        selectedCard = null; // Reset selection
    }

    IEnumerator AddCardRoutine() // Add a card every X amount of seconds if a free slot is available
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Change seconds here

            // Find the first empty slot and add a card there
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null)
                {
                    AddCard(i);
                    break;
                }
            }
        }
    }
}