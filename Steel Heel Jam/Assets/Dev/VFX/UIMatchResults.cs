using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMatchResults : MonoBehaviour
{

    public TextMeshProUGUI playerName;

    public Transform playerStatsList;

    public GameObject playerStatsPrefab;

    public GameObject buttons;

    public void Init(int winningPlayerNumber)
    {
        playerName.text = "Player " + winningPlayerNumber;
        playerName.color = PlayerToken.colors[winningPlayerNumber - 1];

        GameObject g1 = Instantiate(playerStatsPrefab, playerStatsList);
        UIPlayerStats stats1 = g1.GetComponent<UIPlayerStats>();

        PlayerStatus winningStatus = GameManager.game.alivePlayerStatuses[0];

        stats1.Init(1, winningPlayerNumber, (int)winningStatus.totalDamageDealt, winningStatus.totalEliminations, (int)winningStatus.totalDamageTaken);

        GameManager.game.eliminatedPlayerStatuses.Reverse();

        int currentRank = 2;
        foreach (PlayerStatus status in GameManager.game.eliminatedPlayerStatuses)
        {
            GameObject g = Instantiate(playerStatsPrefab, playerStatsList);
            UIPlayerStats stats = g.GetComponent<UIPlayerStats>();
            stats.Init(currentRank, status.playerNumber, (int)status.totalDamageDealt, status.totalEliminations, (int)status.totalDamageTaken);
            currentRank++;
        }

        buttons.SetActive(false);

        StartCoroutine(ShowButtons());
    }

    IEnumerator ShowButtons()
    {
        yield return new WaitForSeconds(3f);

        buttons.SetActive(true);
    }
}
