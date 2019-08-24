using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    private Player playerRef;
    private Player PlayerRef
    {
        get
        {
            if (playerRef == null)
            {
                playerRef = ReInput.players.GetPlayer(1);
            }
            return playerRef;
        }
    }

    [SerializeField]
    private MenuSelectable currentSelected;

    [SerializeField]
    public Image winnerFlag;
    [SerializeField]
    public TMPro.TextMeshProUGUI winnerLabel;

    [SerializeField]
    public Image[] loserFlags;
    [SerializeField]
    public GameObject[] loserFlagGOs;

    public void Populate(GamePlayer winner, List<GamePlayer> players)
    {
        List<GamePlayer> losers = new List<GamePlayer>(players);
        losers.Remove(winner);

        this.winnerFlag.color = winner.PlayerColour;
        this.winnerLabel.text = winner.PlayerName;

        for (int i = 0; i < loserFlagGOs.Length; i++)
        {
            loserFlagGOs[i].SetActive(false);
        }
        for (int i = 0; i < losers.Count; i++)
        {
            if(losers[i].HasPlayer)
            {
                loserFlagGOs[i].SetActive(true);
                loserFlags[i].color = losers[i].PlayerColour;
            }
        }
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PlayerRef == null || currentSelected == null) return;

        currentSelected?.OnUpdate(PlayerRef);
    }

    public void Next()
    {
        battleManager.ChangeToSetup();
    }
}