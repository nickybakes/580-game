using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public void Init(PlayerStatus _status)
    {
        status = _status;
        tr = status.transform;
        inputs = status.GetComponent<StarterAssetsInputs>();

        transform = gameObject.transform;

        transform.position = tr.position;


    }

    // Update is called once per frame
    void Update()
    {
        if (status.eliminated || GameManager.game.dontUpdateGameplay)
            return;

        transform.position = tr.position;

        if (!pathCreated)
        {
            SetDestination(GameManager.game.alivePlayerStatuses[0].transform.position);

            pathCreated = true;
        }

        if (path == null)
            return;

        Vector3 desiredVelocity = path.corners[currentCorner] - tr.position;

        if (desiredVelocity.y > 0)
        {
            inputs.JumpInput(true);
        }

        inputs.MoveInput(new Vector2(desiredVelocity.x, desiredVelocity.z));

        if (Vector3.SqrMagnitude(path.corners[currentCorner] - tr.position) < 1.2f)
        {
            currentCorner++;
            if (currentCorner >= path.corners.Length)
            {
                Debug.Log("Completed path");
                SetDestination(GameManager.game.alivePlayerStatuses[0].transform.position);
            }
        }
    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
        path = new NavMeshPath();
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
