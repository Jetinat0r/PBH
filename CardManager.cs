using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public Transform cardPanel; // Parent UI panel where cards are displayed
    public GameObject cardPrefab; // UI prefab for a card
    private List<GameObject> cards = new List<GameObject>();
    private GameObject selectedCard = null;

    void Start()
    {
        // Initialize with 5 cards
        for (int i = 0; i < 5; i++)
        {
            AddCard();
        }

        StartCoroutine(AddCardRoutine());
    }

    void Update()
    {
        // Number key selection (1-5)
        for (int i = 0; i < cards.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectCard(cards[i]);
            }
        }
    }

    public void AddCard()
    {
        GameObject newCard = Instantiate(cardPrefab, cardPanel);
        newCard.GetComponentInChildren<Text>().text = "Card " + Random.Range(1, 10);
        newCard.GetComponent<Button>().onClick.AddListener(() => SelectCard(newCard));
        cards.Add(newCard);
    }

    private void SelectCard(GameObject card)
    {
        if (selectedCard != null)
            selectedCard.GetComponent<Image>().color = Color.white; // Reset color

        selectedCard = card;
        selectedCard.GetComponent<Image>().color = Color.yellow; // Highlight selection
    }

    public void UseCardOnTile(GameObject tile)
    {
        if (selectedCard != null)
        {
            tile.GetComponent<Image>().color = Color.green; // Simulate effect
            cards.Remove(selectedCard);
            Destroy(selectedCard);
            selectedCard = null;
        }
    }

    IEnumerator AddCardRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            if (cards.Count < 5) AddCard();
        }
    }
}
