using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CardManager : MonoBehaviour
{
    public RectTransform cardPanel; //UI panel where cards are displayed
    public RectTransform handParent; //Parent to our hand of cards
    public Card cardPrefab; // Prefab for a card
    public CardData[] cardDatas; // array to store available card informations
    private Card[] cards = new Card[5]; // Array to store card instances
    public Card selectedCard = null;
    [SerializeField]
    private float cardRespawnTime = 1.5f;
    

    void Start()
    {
        //Subscribe to click event
        GridTileSelector.instance.onTileClick += UseCardOnTile;

        // Initialize with 5 cards
        for (int i = 0; i < 5; i++)
        {
            AddRandomCard(i);
        }

        //StartCoroutine(AddCardRoutine());
    }

    private void OnDestroy()
    {
        //Unsubscribe from click event
        GridTileSelector.instance.onTileClick -= UseCardOnTile;
    }

    void Update()
    {
        // Number key selection (1-5)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectCard(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectCard(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectCard(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectCard(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectCard(4);
        }

        /*
        for (int i = 0; i < cards.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && cards[i] != null)
            {
                SelectCard(cards[i]);
            }
        }
        */
    }

    public void AddRandomCard(int _index)
    {
        if (_index < 0 || _index >= cards.Length || cards[_index] != null) return;

        Card _newCard = Instantiate(cardPrefab, handParent);

        // Assign a random card type image to the card
        if (cardDatas.Length == 0)
        {
            Debug.LogError($"No valid card datas to choose from!");
            return;
        }

        CardData _newCardData = cardDatas[Random.Range(0, cardDatas.Length)];
        _newCard.Init(_newCardData);

        // Copy RectTransform values from prefab to new card
        /*
        canvasRect.anchorMin = mainCanvasRect.anchorMin;
        canvasRect.anchorMax = mainCanvasRect.anchorMax;
        canvasRect.pivot = mainCanvasRect.pivot;
        canvasRect.anchoredPosition = mainCanvasRect.anchoredPosition;
        canvasRect.sizeDelta = mainCanvasRect.sizeDelta;
        canvasRect.rotation = mainCanvasRect.rotation;
        canvasRect.localScale = mainCanvasRect.localScale;
        canvasRect.localPosition = Vector3.zero; 
        */

        //  CardImage's position increments x by 100 per card
        _newCard.transform.localPosition = new Vector3(-280f + (_index * 140f), 20f, 0f);

        cards[_index] = _newCard;

        // click selection
        EventTrigger.Entry _triggerEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        _triggerEntry.callback.AddListener((data) => SelectCard(_index)); // Select card on click

        _newCard.cardClickEventTrigger.triggers.Add(_triggerEntry);

        Debug.Log($"Card {_index} instantiated successfully!");
    }

    private CardType GetCardTypeForSprite(Sprite sprite)
    {
        // Set card type based on name of sprite
        if (sprite.name.Contains("Horizontal"))
        {
            return CardType.Horizontal;
        }
        else if (sprite.name.Contains("Vertical"))
        {
            return CardType.Vertical;
        }
        else if (sprite.name.Contains("Cross"))
        {
            return CardType.Cross;
        }
        else
        {
            return CardType.X;  // Supposed to be for a cross type but not implemented, so default
        }
    }

    private void SelectCard(int _cardIndex)
    {
        Card _card = cards[_cardIndex];

        //If we try to select a bad card, ignore the instruction
        if(_card == null)
        {
            return;
        }

        if (selectedCard == _card)
        {
            DeselectCurrentCard();
            GridTileSelector.instance.DeactivateSelector();
            return;
        }

        if (selectedCard != null)
        {
            DeselectCurrentCard();
        }

        selectedCard = _card;
        GridTileSelector.instance.ActivateSelector();

        // Get the card and highlight it when selected
        _card.SelectCard();
        /*
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
        */
    }

    private void DeselectCurrentCard()
    {
        if (selectedCard == null) return;

        selectedCard.DeselectCard();

        // Reset the image color
        /*
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
        */

        // Reset the selected card to null
        selectedCard = null;
    }

    public void UseCardOnTile(Vector3Int _tilePos, bool _isValidTile)
    {
        if (selectedCard == null) return;
        if (!_isValidTile)
        {
            Debug.LogWarning($"Card used on invalid tile {_tilePos}!");
            return;
        }

        Debug.Log("Card used on tile!");

        // Remove the used card
        //TODO: Rework to not require this
        //      List<> refactoring is a valid solution
        for(int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == selectedCard)
            {
                cards[i] = null;
                this.StartCallAfterSeconds(() => { AddRandomCard(i); }, cardRespawnTime);
                break;
            }
        }
        //callBulletSpawn(cardType) ** not implemented, but psuedocode for future implementation of remote call
        Destroy(selectedCard.gameObject);
        selectedCard = null; //Remove reference to held card

        GridTileSelector.instance.DeactivateSelector();
    }
}