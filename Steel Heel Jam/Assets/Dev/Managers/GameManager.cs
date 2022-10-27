using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager game;

    public AudioManager audioManager;

    public GameSceneSettings gameSceneSettings;

    public GameObject playerPrefab;

    public GameObject ringPrefab;

    public HUDManager hudManager;
    public GameObject hudCountdown;

    public ItemManager itemManager;

    public CameraManager cameraManager;

    public Ring ringScript;

    public List<PlayerStatus> allPlayerStatuses;
    public List<PlayerStatus> alivePlayerStatuses;
    public List<PlayerStatus> eliminatedPlayerStatuses;

    public int countdownTime;
    private TextMeshProUGUI countdownDisplay;

    /// <summary>
    /// Starts counting up after the game starts
    /// </summary>
    public float gameTime;

    public float maxGameTime = 154;

    public bool gameWon;

    //public GameObject heelSpotlightPrefab;
    //public HeelSpotlight heelSpotlightScript;

    public GameObject spotlight;
    private BuffSpotlight spotlightScript;

    public const float spotlightRespawnCooldownMax = 3.0f;
    public float spotlightRespawnCooldown;

    private bool heelSpotlightSpawned;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.game = this;
        audioManager = FindObjectOfType<AudioManager>();
        allPlayerStatuses = new List<PlayerStatus>();
        alivePlayerStatuses = new List<PlayerStatus>();
        eliminatedPlayerStatuses = new List<PlayerStatus>();

        //temporary
        itemManager = FindObjectOfType<ItemManager>();


        SpawnPlayerPrefabs();
        SpawnRing();

        spotlightScript = spotlight.GetComponent<BuffSpotlight>();
        SpawnSpotlight();

        //Grabs the countdown text from GameHUD and starts a countdown.
        countdownDisplay = hudCountdown.GetComponent<TextMeshProUGUI>();
        StartCoroutine(CountdownToStart());
    }

    /// <summary>
    /// Creates match countdown, limiting player movement until over.
    /// </summary>
    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "BRAWL!";
        audioManager.Play("bellStart");

        // Player movement limited by PlayerStatus update().

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        cameraManager.UpdateCamera(alivePlayerStatuses, eliminatedPlayerStatuses);

        if (spotlightRespawnCooldown < spotlightRespawnCooldownMax)
        {
            spotlightRespawnCooldown += Time.deltaTime;

            if (spotlightRespawnCooldown > spotlightRespawnCooldownMax)
            {
                SpawnSpotlight();
            }
        }

        // if (gameTime > 13 && !heelSpotlightSpawned)
        // {
        //     heelSpotlightSpawned = true;
        //     SpawnHeelSpotlight();
        // }
    }

    public void SpawnPlayerPrefabs()
    {
        Transform spawnPoints = gameSceneSettings.transform.GetChild(Mathf.Max(0, AppManager.app.TokenAmount - 2));
        spawnPoints.gameObject.SetActive(true);
        int[] spawnPointOrder = new int[spawnPoints.childCount];

        for (int i = 0; i < spawnPointOrder.Length; i++)
        {
            spawnPointOrder[i] = i;
        }

        ShuffleArray(spawnPointOrder);

        int j = 0;

        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                PlayerToken token = AppManager.app.playerTokens[i];
                token.input.SwitchCurrentActionMap("Player");

                GameObject player = Instantiate(playerPrefab, spawnPoints.GetChild(spawnPointOrder[j]).position, spawnPoints.GetChild(spawnPointOrder[j]).rotation);
                PlayerStatus status = token.SetUpPlayerPrefab(player);
                j++;

                allPlayerStatuses.Add(status);
                alivePlayerStatuses.Add(status);

                hudManager.CreatePlayerHeader(status);
            }
        }
    }

    public float GetTotalActivityTime()
    {
        float score = 0;
        foreach (PlayerStatus p in alivePlayerStatuses)
        {
            score += p.recentActivityTimeCurrent;
        }

        return score;
    }

    public PlayerStatus GetLeastActivePlayer()
    {
        PlayerStatus status = alivePlayerStatuses[0];

        foreach (PlayerStatus s in alivePlayerStatuses)
        {
            if (s.ActivityScore < status.ActivityScore)
                status = s;
        }

        return status;
    }

    public void EliminatePlayer(PlayerStatus status)
    {
        status.eliminated = true;
        status.SetPlayerStateImmediately(new Eliminated());

        if (status.playerLastHitBy != null)
        {
            status.playerLastHitBy.totalEliminations++;
            status.playerLastHitBy.IncreaseSpotlightMeter(25.0f);
        }

        alivePlayerStatuses.Remove(status);
        eliminatedPlayerStatuses.Add(status);
        status.timeOfEliminiation = gameTime;
        hudManager.RemoveHeader(status);
        status.playerHeader = null;

        //if (status.isHeel)
        //{
        //    heelSpotlightScript.gameObject.transform.position = new Vector3(status.gameObject.transform.position.x, heelSpotlightScript.gameObject.transform.position.y, status.gameObject.transform.position.z);

        //    heelSpotlightScript.gameObject.SetActive(true);
        //}

        if (alivePlayerStatuses.Count == 1)
            gameWon = true;
    }

    private void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private void SpawnRing()
    {
        Transform ringCenter = gameSceneSettings.ringCenters.transform.GetChild(Random.Range(0, gameSceneSettings.ringCenters.transform.childCount));

        GameObject g = Instantiate(ringPrefab);
        ringScript = g.GetComponent<Ring>();

        g.transform.localScale = new Vector3(65, 7, 65);
        g.transform.position = new Vector3(ringCenter.position.x, g.transform.position.y, ringCenter.position.z);

        ringScript.UpdateRingShaderProperties();

        // Resize ring to a diameter of 10 units in 2-ish minutes
        ringScript.ResizeRing(10, maxGameTime);
    }

    public void SpawnSpotlight()
    {
        float xOffset = Random.Range(-10.0f - ringScript.tr.localScale.x, 11.0f + ringScript.tr.localScale.x);
        float zOffset = Random.Range(-10.0f - ringScript.tr.localScale.z, 11.0f + ringScript.tr.localScale.z);

        spotlightScript.tr.localPosition = new Vector3(ringScript.tr.position.x + xOffset, 0, ringScript.tr.position.z + zOffset);
        spotlight.SetActive(true);
    }

    public void DespawnSpotlight()
    {
        spotlightRespawnCooldown = 0;
        spotlight.SetActive(false);
    }

    //public void SpawnHeelSpotlight()
    //{
    //    GameObject g = Instantiate(heelSpotlightPrefab);

    //    heelSpotlightScript = g.GetComponent<HeelSpotlight>();


    //}
}
