using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BotStrat
{
    ReturnToCircle,
    ChasePlayer,
    ChaseItem,

}

public class BotController : MonoBehaviour
{

    private PlayerStatus status;

    private StarterAssetsInputs inputs;


    private Transform tr;

    private new Transform transform;

    private Vector3 destination;

    private NavMeshPath path;

    private int currentCorner;

    private bool idle;

    private bool pathCreated;

    private BotStrat currentStrat;

    private Transform playerToChase;

    private Timer forceStratPickTimer;
    private Timer chaseRemakePathTimer;


    public void Init(PlayerStatus _status)
    {
        status = _status;
        tr = status.transform;
        inputs = status.GetComponent<StarterAssetsInputs>();

        transform = gameObject.transform;

        transform.position = tr.position;

        transform.parent = tr;

        forceStratPickTimer = new Timer(7);
        chaseRemakePathTimer = new Timer(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (status.eliminated || GameManager.game.dontUpdateGameplay)
            return;

        MoveToDestination();

        if (forceStratPickTimer.DoneLoop())
        {
            PickStrategy();
        }

        switch (currentStrat)
        {
            case (BotStrat.ChasePlayer):
                if (chaseRemakePathTimer.DoneLoop())
                {
                    SetDestination(playerToChase.position);
                }
                break;
        }
    }

    public bool IsCompleteTask()
    {
        switch (currentStrat)
        {
            case (BotStrat.ReturnToCircle):
                if (AtDestination())
                    return true;
                break;
        }

        return false;
    }

    public void PickStrategy()
    {
        if (status.IsOOB && status.stamina / PlayerStatus.defaultMaxStamina < .3f)
        {
            SetStrategy(BotStrat.ReturnToCircle);
        }

        SetStrategy(BotStrat.ChasePlayer);
        forceStratPickTimer.Restart();
    }

    public void SetStrategy(BotStrat strat)
    {
        switch (strat)
        {
            case (BotStrat.ReturnToCircle):
                SetDestination(GameManager.game.ringScript.tr.position);
                break;

            case (BotStrat.ChasePlayer):
                playerToChase = GameManager.game.alivePlayerStatuses[Random.Range(0, GameManager.game.alivePlayerStatuses.Count)].transform;
                SetDestination(playerToChase.position);
                break;
        }

        currentStrat = strat;
    }

    public void MoveToDestination()
    {
        if (!pathCreated)
        {
            PickStrategy();
            pathCreated = true;
        }

        if (path == null)
            return;

        if (currentCorner >= path.corners.Length || Vector3.SqrMagnitude(destination - tr.position) < 1.2f)
        {
            PickStrategy();
            return;
        }

        Vector3 desiredVelocity = path.corners[currentCorner] - tr.position;

        if (desiredVelocity.y > 0)
        {
            inputs.JumpInput(true);
        }

        inputs.MoveInput(new Vector2(desiredVelocity.x, desiredVelocity.z));

        if (Vector3.SqrMagnitude(path.corners[currentCorner] - tr.position) < 1.2f)
        {
            currentCorner++;
        }
    }

    public bool AtDestination(float squaredRadius = 1.2f)
    {
        if (currentCorner >= path.corners.Length || Vector3.SqrMagnitude(destination - tr.position) < 1.2f)
        {
            return true;
        }

        return false;
    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
        path = new NavMeshPath();
        //randomly chose to use jump paths or not
        NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        currentCorner = 0;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
