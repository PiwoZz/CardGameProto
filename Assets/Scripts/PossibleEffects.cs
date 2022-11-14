using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffect")]
public class PossibleEffects : ScriptableObject {
    [Header("Destroy card(s) in opponent hand.")]
    public int numberOfDestroyedCards;
    [Header("Draw card(s).")]
    public int numberOfDrawedCards;
    
    public void Do(GameObject gameObject) {
        if (numberOfDestroyedCards > 0) {
            DestroyCards(gameObject.GetComponentsInChildren<Image>());
        }

        if (numberOfDrawedCards > 0) {
            DrawCards(gameObject);
        }
    }

    private void DestroyCards(Image[] hand) {
        if (hand.Length <= numberOfDestroyedCards) {
            foreach (var variable in hand) {
                Destroy(variable.gameObject);
            }
        }
        else {
            for (int i = numberOfDestroyedCards; i > 0; i--) {
                Destroy(hand[i].gameObject);
            }
        }
    }

    private void DrawCards(GameObject hand) {
        for (int i = numberOfDrawedCards; i > 0; i--) {
            InstantiateAleatoryCard.CreateARandomCard(hand);
        }
    }
}