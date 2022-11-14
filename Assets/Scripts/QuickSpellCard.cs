using UnityEngine;

[CreateAssetMenu(menuName = "Card", fileName = "QuickSpellCard")]
public class QuickSpellCard : OriginalCard {
    public PossibleEffects cardEffect;

    public override void Do(GameObject hand) {
        cardEffect.Do(hand);
    }
}