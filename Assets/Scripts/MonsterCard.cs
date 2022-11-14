using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Card", fileName = "MonsterCard")]
public class MonsterCard : OriginalCard {
    public PossibleEffects cardEffect;
    public int attackValue;
    

    public override void Do(GameObject hand) {
        cardEffect.Do(hand);
    }
}