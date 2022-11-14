using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class InstantiateAleatoryCard : MonoBehaviour {
    public static void CreateARandomCard(GameObject hand) {
        int i = Random.Range(1, 4);
        switch (i) {
            case 1:
                CreateAMonsterCard(hand);
                break;
            case 2:
                CreateAQuickSpellCard(hand);
                break;
            case 3:
                CreateASpellCard(hand);
                break;
        }
    }
    
    private static void CreateAMonsterCard(GameObject hand) {
        GameObject newMonsterCard = Instantiate(Manager.GetACard(), hand.transform);
        var monsterCard = newMonsterCard.GetComponent<CardUI>().monsterCard = ScriptableObject.CreateInstance<MonsterCard>();
        var monsterEffect = monsterCard.cardEffect = ScriptableObject.CreateInstance<PossibleEffects>();
        monsterCard.attackValue = Random.Range(500, 1001);
        monsterEffect.numberOfDrawedCards = 0;
        monsterEffect.numberOfDestroyedCards = 0;
    }

    private static void CreateAQuickSpellCard(GameObject hand) {
        GameObject newQuickSpellCard = Instantiate(Manager.GetACard(), hand.transform);
        var quickSpellCard = newQuickSpellCard.GetComponent<CardUI>().quickSpellCard = ScriptableObject.CreateInstance<QuickSpellCard>();
        var quickSpellEffect = quickSpellCard.cardEffect = ScriptableObject.CreateInstance<PossibleEffects>();
        quickSpellEffect.numberOfDrawedCards = Random.Range(1,3);

    }

    private static void CreateASpellCard(GameObject hand) {
        GameObject newSpellCard = Instantiate(Manager.GetACard(), hand.transform);
        var spellCard = newSpellCard.GetComponent<CardUI>().spellCard = ScriptableObject.CreateInstance<SpellCard>();
        var spellEffect = spellCard.cardEffect = ScriptableObject.CreateInstance<PossibleEffects>();
        spellEffect.numberOfDestroyedCards = Random.Range(1, 3);
    }
}