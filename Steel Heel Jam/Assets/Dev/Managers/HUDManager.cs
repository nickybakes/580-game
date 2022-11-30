using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HUDManager : MonoBehaviour
{

    public static HUDManager hud;

    public TextMeshProUGUI countdownText;

    public GameObject headerPanel;
    public GameObject alertsPanel;
    public GameObject indicatorsPanel;

    public UIMatchResults matchResultsPanel;
    public GameObject pausePanel;
    public GameObject cursorPanel;

    public GameObject cursorPrefab;

    [HideInInspector]
    public List<PlayerCursor> cursors;

    public Canvas canvas;
    public RectTransform canvasRect;

    public GameObject headerPrefab;

    public List<PlayerHeader> headers;

    public List<Sprite> buffIcons;

    public GameObject spotlightAlert;
    public GameObject spotlightOffScreenIndicator;

    public GameObject eliminatedAlert;

    public GameObject introCurrentChampAlert;
    public GameObject introHostAlert;



    // Start is called before the first frame update
    void Start()
    {
        hud = this;
    }

    public void CreatePlayerHeader(PlayerStatus status)
    {
        GameObject headerObject = Instantiate(headerPrefab, headerPanel.transform);
        PlayerHeader header = headerObject.GetComponent<PlayerHeader>();
        headers.Add(header);
        header.Setup(status, canvas);
    }

    public void RemoveHeader(PlayerStatus status)
    {
        PlayerHeader header = null;
        foreach (PlayerHeader h in headers)
        {
            if (h.Status == status)
            {
                header = h;
            }
        }

        if (header != null)
        {
            headers.Remove(header);
            header.gameObject.SetActive(false);
        }
    }

    public static Sprite GetBuffIcon(Buff buff)
    {
        return hud.buffIcons[(int)buff];
    }



    public static GameObject CreateSpotlightAlert(Transform spotlightTransform)
    {
        GameObject g = Instantiate(hud.spotlightAlert, hud.alertsPanel.transform);

        UIAlert alert = g.GetComponent<UIAlert>();
        alert.transformToTrackTo = spotlightTransform;

        return g;
    }

    public static GameObject CreateSpotlightOffScreenIndicator(Transform spotlightTransform)
    {
        GameObject g = Instantiate(hud.spotlightOffScreenIndicator, hud.indicatorsPanel.transform);

        UIOffScreenIndicator indicator = g.GetComponent<UIOffScreenIndicator>();
        indicator.transformToTrackTo = spotlightTransform;

        return g;
    }

    public static GameObject CreateEliminatedAlert(Transform playerTransform, int playerNumber)
    {
        GameObject g = Instantiate(hud.eliminatedAlert, hud.alertsPanel.transform);

        UIAlert alert = g.GetComponent<UIAlert>();
        alert.transformToTrackTo = playerTransform;

        alert.textObjects[0].color = PlayerToken.colors[playerNumber - 1];
        alert.textObjects[0].text = "Player " + playerNumber;

        return g;
    }

    public static GameObject CreateMapNameAlert(GameObject alert)
    {
        return Instantiate(alert, hud.alertsPanel.transform);
    }

    public static GameObject CreateCurrentChampAlert()
    {
        GameObject g = Instantiate(hud.introCurrentChampAlert, hud.alertsPanel.transform);

        UIAlert alert = g.GetComponent<UIAlert>();
        if (AppManager.app.currentChampion != -1)
        {
            alert.textObjects[1].color = PlayerToken.colors[AppManager.app.currentChampion - 1];
            alert.textObjects[1].text = "Player " + AppManager.app.currentChampion;
        }

        return g;
    }

    public static GameObject CreateHostAlert()
    {
        return Instantiate(hud.introHostAlert, hud.alertsPanel.transform);
    }
}
