using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager game;

    public AudioManager audioManager;

    public GameSceneSettings gameSceneSettings;

    public GameObject playerPrefab;

    public HUDManager hudManager;

    public CameraManager cameraManager;

    public List<PlayerStatus> allPlayerStatuses;
    public List<PlayerStatus> alivePlayerStatuses;
    public List<PlayerStatus> eliminatedPlayerStatuses;

    /// <summary>
    /// Starts counting up after the game starts
    /// </summary>
    public float gameTime;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.game = this;
        audioManager = FindObjectOfType<AudioManager>();
        allPlayerStatuses = new List<PlayerStatus>();
        alivePlayerStatuses = new List<PlayerStatus>();
        eliminatedPlayerStatuses = new List<PlayerStatus>();
        SpawnPlayerPrefabs();
        //audioManager.Play("Smack");
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        cameraManager.UpdateCamera(alivePlayerStatuses, eliminatedPlayerStatuses);
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

    public void EliminatePlayer(PlayerStatus status)
    {
        alivePlayerStatuses.Remove(status);
        eliminatedPlayerStatuses.Add(status);
        status.timeOfEliminiation = gameTime;
        hudManager.RemoveHeader(status);
        status.SetPlayerStateImmediately(new Eliminated());
        status.playerHeader = null;
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
}
