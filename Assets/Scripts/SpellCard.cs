using UnityEngine;

[CreateAssetMenu(menuName = "Card", fileName = "QuickSpellCard")]
public class SpellCard : OriginalCard {
    public PossibleEffects cardEffect;

    public override void Do(GameObject hand) {
        cardEffect.Do(hand);
    }
}