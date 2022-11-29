using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager game;

    public GameSceneSettings gameSceneSettings;

    public GameObject playerPrefab;

    public GameObject ringPrefab;

    public GameObject spotlightPrefab;

    public HUDManager hudManager;

    public ItemManager itemManager;

    public CameraManager cameraManager;

    public GameObject introSequenceManager;

    public GameObject matchResultsView;

    [HideInInspector]
    public Ring ringScript;
    [HideInInspector]
    public List<PlayerStatus> allPlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> alivePlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> eliminatedPlayerStatuses;

    public int countdownTime;

    [HideInInspector]
    /// <summary>
    /// Starts counting up after the game starts
    /// </summary>
    public float gameTime;

    public float maxGameTime = 150;

    [HideInInspector]
    public bool gameWon;

    //public GameObject heelSpotlightPrefab;
    //public HeelSpotlight heelSpotlightScript;

    [SerializeField] private GameObject explosion;

    [HideInInspector]
    public GameObject spotlight;
    private BuffSpotlight spotlightScript;
    private GameObject spotlightOffScreenIndicator;
    [HideInInspector]
    public float spotlightRespawnCooldownMax;
    [HideInInspector]
    public float spotlightRespawnCooldown;

    [HideInInspector]
    //start as true for our intro sequence
    public bool dontUpdateGameplay = true;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.game = this;
        allPlayerStatuses = new List<PlayerStatus>();
        alivePlayerStatuses = new List<PlayerStatus>();
        eliminatedPlayerStatuses = new List<PlayerStatus>();


        SpawnPlayerPrefabs();
        MoveAllPlayersToGround();
        SpawnRing();

        InitializeSpotlight();

        StartCoroutine(IntroSequence());
    }

    /// <summary>
    /// Plays the intro sequence
    /// </summary>
    IEnumerator IntroSequence()
    {
        cameraManager.gameObject.SetActive(false);
        introSequenceManager.SetActive(true);
        hudManager.headerPanel.SetActive(false);
        AudioManager.aud.Play("intro");

        yield return new WaitForSeconds(1f);

        HUDManager.CreateMapNameAlert(gameSceneSettings.mapNameAlert);

        yield return new WaitForSeconds(1f);

        HUDManager.CreateCurrentChampAlert();

        yield return new WaitForSeconds(.5f);

        HUDManager.CreateHostAlert();

        yield return new WaitForSeconds(4.5f);

        EnableGameplayCameraAndCountdown();
    }

    public void EnableGameplayCameraAndCountdown()
    {
        cameraManager.gameObject.SetActive(true);
        introSequenceManager.gameObject.SetActive(false);
        hudManager.headerPanel.SetActive(true);

        HUDManager.hud.countdownText.gameObject.SetActive(true);

        StartCoroutine(CountdownToStart());
    }

    /// <summary>
    /// Creates match countdown, limiting player movement until over.
    /// </summary>
    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            HUDManager.hud.countdownText.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        HUDManager.hud.countdownText.text = "BRAWL!";
        AudioManager.aud.Play("bellStart");
        dontUpdateGameplay = false;

        StartMovingRing();

        // Player movement limited by PlayerStatus update().

        yield return new WaitForSeconds(0.5f);

        // Play matchBegin announcer line.
        AudioManager.aud.Play("matchBegin");

        yield return new WaitForSeconds(0.5f);

        HUDManager.hud.countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameWon)
            return;

        if (dontUpdateGameplay)
        {
            if (cameraManager.gameObject.activeSelf)
                cameraManager.UpdateCamera(alivePlayerStatuses, eliminatedPlayerStatuses);
            return;
        }

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

        // Lower audience volume to min.
        AudioManager.aud.UpdateFade("cheer", -0.1f, 0.0f);
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

    public void MoveAllPlayersToGround()
    {
        foreach (PlayerStatus s in allPlayerStatuses)
        {
            for (int i = 0; i < 100; i++)
            {
                s.movement.UpdateManual(true, false, false, false);
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
        status.waitingToBeEliminated = true;
        status.combat.DropWeapon();

        if (status.playerLastHitBy != null)
        {
            status.playerLastHitBy.totalEliminations++;
            status.playerLastHitBy.IncreaseSpotlightMeter(25.0f);
        }

        // Plays VO line. (only if hit out by another player... walk-out lines as a stretch goal)
        AnnouncerManager.PlayLine("eliminated", Priority.Elimination);
        AudioManager.aud.StartFade("cheer", 1.5f, 0.5f);

        alivePlayerStatuses.Remove(status);
        eliminatedPlayerStatuses.Add(status);
        status.timeOfEliminiation = gameTime;
        hudManager.RemoveHeader(status);

        status.transform.GetChild((int)PlayerChild.GrabHitbox).gameObject.SetActive(false);
        status.transform.GetChild((int)PlayerChild.PickUpSphere).gameObject.SetActive(false);
        status.transform.GetChild((int)PlayerChild.RingDecal).gameObject.SetActive(false);

        if (alivePlayerStatuses.Count == 1)
        {
            EndGame();
        }

        HUDManager.CreateEliminatedAlert(status.transform, status.playerNumber);
    }

    public void EndGame()
    {
        gameWon = true;
        AppManager.app.currentChampion = alivePlayerStatuses[0].playerNumber;

        alivePlayerStatuses[0].gameObject.SetActive(false);

        cameraManager.gameObject.SetActive(false);
        matchResultsView.gameObject.SetActive(true);
        hudManager.headerPanel.SetActive(false);
        hudManager.matchResultsPanel.gameObject.SetActive(true);
        spotlight.SetActive(false);
        ringScript.gameObject.SetActive(false);
        Shader.SetGlobalFloat("ringRadius", 500);

        hudManager.matchResultsPanel.Init(alivePlayerStatuses[0].playerNumber);

        MenuCharacterDisplay characterDisplay = matchResultsView.GetComponentInChildren<MenuCharacterDisplay>();
        characterDisplay.playerNumber = alivePlayerStatuses[0].playerNumber;
        characterDisplay.Init();

        // Plays MatchEnd VO.
        AnnouncerManager.PlayLine("MatchEnd", Priority.MatchEnd);
    }

    public static void PauseGame()
    {
        game.dontUpdateGameplay = true;
        Physics.autoSimulation = false;
    }

    public static void UpPauseGame()
    {
        game.dontUpdateGameplay = false;
        Physics.autoSimulation = true;
    }

    public static void RestartGame()
    {

    }

    public static void ExitToMenu()
    {
        
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

        g.transform.localScale = new Vector3(72, 7, 72);
        g.transform.position = new Vector3(ringCenter.position.x, g.transform.position.y, ringCenter.position.z);

        ringScript.UpdateRingShaderProperties();
    }

    private void StartMovingRing()
    {
        // Resize ring to a diameter of 10 units in 2-ish minutes
        ringScript.ResizeRing(10, 5);
    }

    public void InitializeSpotlight()
    {
        spotlight = Instantiate(spotlightPrefab);
        spotlight.transform.position = gameSceneSettings.transform.GetChild(8).position;
        spotlightScript = spotlight.gameObject.GetComponent<BuffSpotlight>();
        DespawnSpotlight();
    }

    public void SpawnSpotlight()
    {
        //int randomXCoordinate = r.Next(-40, 40);
        //int randomZCoordinate = r.Next(-40, 40);
        float radius = (GameManager.game.ringScript.tr.localScale.x / 2) + 8;
        float xMin = Mathf.Max(GameManager.game.ringScript.transform.position.x - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.x);
        float xMax = Mathf.Min(GameManager.game.ringScript.transform.position.x + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.x);
        float zMin = Mathf.Max(GameManager.game.ringScript.transform.position.z - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.z);
        float zMax = Mathf.Min(GameManager.game.ringScript.transform.position.z + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.z);
        float randomXCoordinate = Random.Range(xMin, xMax);
        float randomZCoordinate = Random.Range(zMin, zMax);

        spotlightScript.tr.position = new Vector3(randomXCoordinate, 0, randomZCoordinate);
        spotlightScript.DecideWanderDirection();
        spotlight.SetActive(true);

        HUDManager.CreateSpotlightAlert(spotlight.transform.GetChild(4));

        spotlightOffScreenIndicator = HUDManager.CreateSpotlightOffScreenIndicator(spotlight.transform.GetChild(4));
        // VO
        AnnouncerManager.PlayLine("spotlight", Priority.SpotlightSpawn);
    }

    public void SpawnExplosion(Vector3 position, PlayerStatus ownerStatus, bool damagerOwnerToo, float knockbackScale = 1, float transformScale = 1)
    {
        GameObject explosionInstance = Instantiate(explosion, position, Quaternion.identity);
        Explosion explosionScript = explosionInstance.GetComponent<Explosion>();
        explosionScript.Init(ownerStatus, damagerOwnerToo, knockbackScale, transformScale);
    }

    public void DespawnSpotlight()
    {
        spotlightRespawnCooldown = 0;
        spotlightRespawnCooldownMax = Random.Range(13, 24);
        spotlight.SetActive(false);

        if (spotlightOffScreenIndicator)
        {
            Destroy(spotlightOffScreenIndicator);
            spotlightOffScreenIndicator = null;
        }
    }

}
