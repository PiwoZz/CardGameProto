using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerDownHandler {
    public MonsterCard monsterCard;
    public QuickSpellCard quickSpellCard;
    public SpellCard spellCard;
    public Text destroyText;
    public Text drawText;
    public Text attackText;
    public bool canBePlayed;
    public bool onlyQuickSpell;

    private void Start() {
        if (monsterCard != null) {
            this.gameObject.tag = "MonsterCard";
            attackText.text = monsterCard.attackValue.ToString();
        }
        else if(quickSpellCard != null)
        {
            this.gameObject.tag = "QuickSpellCard";
            destroyText.text = new string("YOU DRAW " + quickSpellCard.cardEffect.numberOfDrawedCards + " CARDS");
        }
        else if (spellCard != null) {
            this.gameObject.tag = "SpellCard";
            drawText.text = new string("YOU DESTROY " + spellCard.cardEffect.numberOfDestroyedCards + " CARDS");
        }

        if (Manager.ActualPhase == Manager.Phases.BattlePhase ||
            Manager.ActualPhase == Manager.Phases.OpponentBattlePhase) {
            onlyQuickSpell = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (canBePlayed) {
            Manager.PutCard(this.gameObject);
        }
        else if (onlyQuickSpell && quickSpellCard != null) {
            if (this.gameObject.GetComponentInParent<HorizontalLayoutGroup>().gameObject.CompareTag("OpponentHand")) {
                quickSpellCard.Do(GameObject.FindGameObjectWithTag("OpponentHand"));
                Destroy(this.gameObject);
            }
            else if (this.gameObject.GetComponentInParent<HorizontalLayoutGroup>().gameObject.CompareTag("PlayerHand")) {
                quickSpellCard.Do(GameObject.FindGameObjectWithTag("PlayerHand"));
                Destroy(this.gameObject);
            }
        }
    }
}