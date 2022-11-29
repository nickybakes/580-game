using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayerStats : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI playerName;

    public TextMeshProUGUI damageDealt;
    public TextMeshProUGUI eliminationAmount;
    public TextMeshProUGUI damageTaken;

    public void Init(int _rank, int _playerNumber, int _damageDealt, int _eliminationAmount, int _damageTaken)
    {
        rank.text = _rank + ".";
        playerName.text = "Player " + _playerNumber;
        playerName.color = PlayerToken.colors[_playerNumber - 1];

        damageDealt.text = "" + _damageDealt;
        eliminationAmount.text = "" + _eliminationAmount;
        damageTaken.text = "" + _damageTaken;
    }

}
