using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform cardPlaceHolder;
    [SerializeField] private RectTransform enemyPlaceHolder;
    [SerializeField] private RectTransform playerSpellLayout;
    [SerializeField] private RectTransform opponentSpellLayout;
    [SerializeField] private Text actualPhaseText;
    [SerializeField] private Text playerHpText;
    [SerializeField] private Text enemyHpText;

    public int playerHp = 5;
    public int enemyHp = 5;
    public static Phases ActualPhase = Phases.MainPhase;
    
    private static GameObject _giveACard;
    private static RectTransform _playerPlaceHolder;
    private static RectTransform _enemyPlaceHolder;
    private static RectTransform _playerSpellLayout; 
    private static RectTransform _opponentSpellLayout;
    

    private void Start() {
        _giveACard = cardPrefab;
        _playerPlaceHolder = cardPlaceHolder;
        _enemyPlaceHolder = enemyPlaceHolder;
        _playerSpellLayout = playerSpellLayout;
        _opponentSpellLayout = opponentSpellLayout;
        playerHpText.text = playerHp.ToString();
        enemyHpText.text = enemyHp.ToString();
        for (int i = 0; i < 5; i++) {
            InstantiateAleatoryCard.CreateARandomCard(GameObject.FindGameObjectWithTag("PlayerHand"));
            InstantiateAleatoryCard.CreateARandomCard(GameObject.FindGameObjectWithTag("OpponentHand"));
        }
        actualPhaseText.text = "PlayerMainPhase";
        foreach (var variable in GameObject.FindGameObjectWithTag("PlayerHand").GetComponentsInChildren<CardUI>()) {
            variable.canBePlayed = true;
            variable.onlyQuickSpell = false;
        }
    }

    public void PhaseManager() {
        if (ActualPhase == Phases.OpponentDrawPhase) {
            ActualPhase = Phases.MainPhase;
        }
        else {
            ActualPhase += 1;
        }
        switch (ActualPhase) {
            case Phases.BattlePhase:
            case Phases.OpponentBattlePhase:
            {
                actualPhaseText.text = "BattlePhase";
                foreach (var variable in GameObject.FindGameObjectWithTag("OpponentHand").GetComponentsInChildren<CardUI>()) {
                    variable.canBePlayed = false;
                    variable.onlyQuickSpell = true;
                }
                foreach (var variable in GameObject.FindGameObjectWithTag("PlayerHand").GetComponentsInChildren<CardUI>()) {
                    variable.canBePlayed = false;
                    variable.onlyQuickSpell = true;
                }
                if (cardPlaceHolder.childCount > 0 && enemyPlaceHolder.childCount > 0) {
                    var playerMonster = cardPlaceHolder.GetChild(0).GetComponent<CardUI>();
                    var enemyMonster = enemyPlaceHolder.GetChild(0).GetComponent<CardUI>();
                    playerMonster.monsterCard.Do(playerMonster.gameObject);
                    enemyMonster.monsterCard.Do(enemyMonster.gameObject);
                    if (playerMonster.monsterCard.attackValue > enemyMonster.monsterCard.attackValue) {
                        enemyHp--;
                        enemyHpText.text = enemyHp.ToString();
                        if (enemyHp == 0) {
                            Winner("Player1");
                        }
                    }
                    else if (playerMonster.monsterCard.attackValue < enemyMonster.monsterCard.attackValue) {
                        playerHp--;
                        playerHpText.text = playerHp.ToString();
                        if (playerHp == 0) {
                            Winner("Player2");
                        }
                    }
                    Destroy(playerMonster.gameObject);
                    Destroy(enemyMonster.gameObject);
                }
                else {
                    Debug.LogWarning("Il faut deux monstre sur le terrain pour faire lancer le combat.");
                }

                break;
            }
            case Phases.DrawPhase: {
                actualPhaseText.text = "PlayerDrawPhase";
                foreach (var variable in GameObject.FindGameObjectWithTag("PlayerHand").GetComponentsInChildren<CardUI>()) {
                    variable.onlyQuickSpell = false;
                }
                var playerLayout = GameObject.FindGameObjectWithTag("PlayerHand");
                if (playerLayout.GetComponentsInChildren<Image>().Length < 5) {
                    for (int i = playerLayout.GetComponentsInChildren<Image>().Length; i < 5; i++) {
                        InstantiateAleatoryCard.CreateARandomCard(GameObject.FindGameObjectWithTag("PlayerHand"));
                    }
                }
                else if (playerLayout.GetComponentsInChildren<Image>().Length > 10) {
                    for (int i = playerLayout.GetComponentsInChildren<Image>().Length; i > 5; i--) {
                        Destroy(playerLayout.GetComponentsInChildren<Image>()[0].gameObject);
                    }
                }

                break;
            }
            case Phases.OpponentDrawPhase: {
                actualPhaseText.text = "Player2DrawPhase";
                foreach (var variable in GameObject.FindGameObjectWithTag("OpponentHand").GetComponentsInChildren<CardUI>()) {
                    variable.onlyQuickSpell = false;
                }
                var enemyLayout = GameObject.FindGameObjectWithTag("OpponentHand");
                if (enemyLayout.GetComponentsInChildren<Image>().Length < 5) {
                    for (int i = enemyLayout.GetComponentsInChildren<Image>().Length; i < 5; i++) {
                        InstantiateAleatoryCard.CreateARandomCard(GameObject.FindGameObjectWithTag("OpponentHand"));
                    }
                }
                else if (enemyLayout.GetComponentsInChildren<Image>().Length > 10) {
                    for (int i = enemyLayout.GetComponentsInChildren<Image>().Length; i > 5; i--) {
                        Destroy(enemyLayout.GetComponentsInChildren<Image>()[0].gameObject);
                    }
                }

                break;
            }
            case Phases.OpponentMainPhase: {
                actualPhaseText.text = "Player2MainPhase";
                foreach (var variable in GameObject.FindGameObjectWithTag("OpponentHand").GetComponentsInChildren<CardUI>()) {
                    variable.canBePlayed = true;
                    variable.onlyQuickSpell = false;
                }
                foreach (var variable in _playerSpellLayout.GetComponentsInChildren<CardUI>()) {
                    variable.spellCard.Do(GameObject.FindGameObjectWithTag("OpponentHand"));
                    Destroy(variable.gameObject);
                }
                
                break;
            }
            case Phases.MainPhase: {
                actualPhaseText.text = "PlayerMainPhase";
                foreach (var variable in GameObject.FindGameObjectWithTag("PlayerHand").GetComponentsInChildren<CardUI>()) {
                    variable.canBePlayed = true;
                    variable.onlyQuickSpell = false;
                }
                foreach (var variable in _opponentSpellLayout.GetComponentsInChildren<CardUI>()) {
                    variable.spellCard.Do(GameObject.FindGameObjectWithTag("PlayerHand"));
                    Destroy(variable.gameObject);
                }
                break;
            }
        }
    }

    public static void PutCard(GameObject cardObject) {
        switch (ActualPhase) {
            case Phases.MainPhase:
                if (cardObject.CompareTag("MonsterCard") && _playerPlaceHolder.childCount == 0) {
                    cardObject.transform.SetParent(_playerPlaceHolder);
                    SetAnchors(cardObject);
                }
                else if (cardObject.CompareTag("SpellCard") && _playerSpellLayout.childCount < 4) {
                    cardObject.transform.SetParent(_playerSpellLayout);
                    SetAnchors(cardObject);
                }
                break;
            case Phases.OpponentMainPhase:
                if (cardObject.CompareTag("MonsterCard") && _enemyPlaceHolder.childCount == 0) {
                    cardObject.transform.SetParent(_enemyPlaceHolder);
                    SetAnchors(cardObject);
                }
                else if (cardObject.CompareTag("SpellCard") && _opponentSpellLayout.childCount < 4) {
                    cardObject.transform.SetParent(_opponentSpellLayout);
                    SetAnchors(cardObject);
                }
                break;
        }
    }

    private static void SetAnchors(GameObject cardObject) {
        cardObject.GetComponent<RectTransform>().anchorMax = Vector2.one;
        cardObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        cardObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        cardObject.GetComponent<RectTransform>().offsetMax = Vector2.one;
    }

    private void Winner(string winner) {
        Debug.Log(winner + " a gagné la partie !");
    }
    public static GameObject GetACard() {
        return _giveACard;
    }
    
    public enum Phases {
        MainPhase,
        BattlePhase,
        DrawPhase,
        OpponentMainPhase,
        OpponentBattlePhase,
        OpponentDrawPhase
    }
}