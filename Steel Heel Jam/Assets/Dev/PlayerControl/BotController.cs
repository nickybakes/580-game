using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BotStrat
{
    ReturnToCircle,
    ChasePlayer,
    Attack,
    Block,
    Suplex,
    DodgeRoll,
    ChaseItem,
    PickupItem,
    ThrowItem,
    ChaseSpotlight,
    Flex,
    DoubleJump,
    DoElbowDrop,
    Idle,
    ForceUnStuck,

}

public class BotController : MonoBehaviour
{

    public static readonly bool[] allPossibleStrats = { true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    public static readonly bool[] allAttackStrats = { true, false, true, true, true, true, false, false, false, false, false, true, false, false };
    public static readonly bool[] noItemStrats = { true, true, true, true, true, true, false, false, false, true, true, true, true, true };

    private PlayerStatus status;

    private StarterAssetsInputs inputs;


    private Transform tr;

    private new Transform transform;

    private Vector3 destination;

    private NavMeshPath path;

    private int currentCorner;

    private bool idle;

    private bool firstStrategyPicked;

    private BotStrat currentStrat;

    private PlayerStatus playerToChase;

    private Transform transformToChase;

    private Timer forceStratPickTimer;
    private Timer chaseRemakePathTimer;
    private Timer chaseRandomDodgeRollTimer;
    private Timer attackTimer;
    private Timer blockTimer;
    private Timer dodgeRollTimer;
    private Timer suplexTimer;
    private Timer pickupTimer;
    private Timer idleTimer;
    private Timer flexTimer;
    private Timer throwTimer;
    private Timer doubleJumpTimer;

    private Timer idleCooldownTimer;

    private Timer checkForClosePlayerTimer;
    private Timer checkForCloseItemTimer;
    private Timer forceItemPickupCooldownTimer;

    private Vector3 previousPosition;

    private Timer stuckTimer;
    private Timer forceUnstuckTimer;


    public void Init(PlayerStatus _status)
    {
        status = _status;
        tr = status.transform;
        inputs = status.GetComponent<StarterAssetsInputs>();

        transform = gameObject.transform;

        transform.position = tr.position;

        transform.parent = tr;

        forceStratPickTimer = new Timer(7, 1.5f);
        chaseRemakePathTimer = new Timer(1);
        chaseRandomDodgeRollTimer = new Timer(4, 2);
        attackTimer = new Timer(2);
        blockTimer = new Timer(.3f);
        dodgeRollTimer = new Timer(.15f);
        suplexTimer = new Timer(.5f);
        pickupTimer = new Timer(.1f);
        idleTimer = new Timer(.75f, .5f);
        idleCooldownTimer = new Timer(6, 2);
        flexTimer = new Timer(2.5f, 2);
        throwTimer = new Timer(.75f, .5f);
        doubleJumpTimer = new Timer(1f, .25f);
        checkForClosePlayerTimer = new Timer(1.5f, 1f);
        checkForCloseItemTimer = new Timer(4, 1.5f);
        forceItemPickupCooldownTimer = new Timer(3, 1f);

        stuckTimer = new Timer(2.5f);
        forceUnstuckTimer = new Timer(1.5f);

        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (status.eliminated || GameManager.game.dontUpdateGameplay)
            return;

        if (transform.position == previousPosition)
        {
            stuckTimer.Update();
            if (stuckTimer.Done())
            {
                SetStrategy(BotStrat.ForceUnStuck);
                stuckTimer.currentTime = 0;
            }
        }
        else
        {
            stuckTimer.currentTime = 0;
        }

        previousPosition = transform.position;

        if (!firstStrategyPicked)
        {
            PickStrategy();
            firstStrategyPicked = true;
        }

        MoveToDestination();

        if (checkForClosePlayerTimer.DoneLoop())
        {
            CheckForClosePlayer();
        }

        if (checkForCloseItemTimer.DoneLoop())
        {
            if (Random.value > (status.combat.equippedItem != null ? .5 : 0))
            {
                SetStrategy(BotStrat.ChaseItem);
            }
        }

        forceItemPickupCooldownTimer.Update();

        if (status.combat.NumberOfItemsWithinRange != 0)
        {
            if (forceItemPickupCooldownTimer.Done())
            {
                forceItemPickupCooldownTimer.Restart();
                if (status.combat.equippedItem == null || Random.value > .5f)
                    SetStrategy(BotStrat.PickupItem);
            }
        }

        idleCooldownTimer.Update();

        switch (currentStrat)
        {
            case (BotStrat.ReturnToCircle):
                if (chaseRandomDodgeRollTimer.DoneLoop() && Random.value > .3f && (status.IsOOB || status.IsOOB && status.StaminaPercentage > .1f))
                {
                    inputs.DodgeRollInput(true);
                }
                break;

            case (BotStrat.ChaseSpotlight):
            case (BotStrat.ChaseItem):
            case (BotStrat.ChasePlayer):
                if (chaseRemakePathTimer.DoneLoop())
                {
                    if (transformToChase != null)
                        SetDestination(transformToChase.position);
                    else
                        PickStrategy();
                }

                if (chaseRandomDodgeRollTimer.DoneLoop() && Random.value > .3f && (status.IsOOB || status.IsOOB && status.StaminaPercentage > .1f))
                {
                    inputs.DodgeRollInput(true);
                }
                break;

            case (BotStrat.ThrowItem):
                inputs.MoveInput(new Vector2(transformToChase.position.x - transform.position.x, transformToChase.position.z - transform.position.z));
                break;

            case (BotStrat.Attack):
                inputs.AttackInput(true);
                inputs.MoveInput(new Vector2(playerToChase.GetTransform.position.x - transform.position.x, playerToChase.GetTransform.position.z - transform.position.z));
                break;

            case (BotStrat.Suplex):
                inputs.MoveInput(new Vector2(playerToChase.GetTransform.position.x - transform.position.x, playerToChase.GetTransform.position.z - transform.position.z));
                break;

            case (BotStrat.Block):
                inputs.BlockInput(true);
                break;

            case (BotStrat.DodgeRoll):
                inputs.DodgeRollInput(true);
                break;

            case (BotStrat.ForceUnStuck):
                Vector2 direction = new Vector2(GameManager.game.ringScript.tr.position.x - transform.position.x, GameManager.game.ringScript.tr.position.z - transform.position.z);
                if (Mathf.Abs(direction.x) < .5f)
                {
                    direction.x = 1;
                }
                if (Mathf.Abs(direction.y) < .5f)
                {
                    direction.y = 1;
                }
                inputs.MoveInput(direction);
                inputs.JumpInput(true);
                break;
        }

        if (IsCompleteTask() || forceStratPickTimer.DoneLoop())
        {
            PickStrategy();
        }
    }

    public bool IsCompleteTask()
    {
        switch (currentStrat)
        {
            case (BotStrat.ReturnToCircle):
                if (AtDestination(5))
                    return true;
                break;

            case (BotStrat.Attack):
                if (attackTimer.DoneLoop())
                {
                    playerToChase = null;
                    return true;
                }
                break;

            case (BotStrat.Block):
                if (blockTimer.DoneLoop())
                {
                    playerToChase = null;
                    return true;
                }
                break;

            case (BotStrat.DodgeRoll):
                if (dodgeRollTimer.DoneLoop())
                {
                    playerToChase = null;
                    return true;
                }
                break;

            case (BotStrat.Suplex):
                if (suplexTimer.DoneLoop())
                {
                    playerToChase = null;
                    return true;
                }
                break;

            case (BotStrat.ChasePlayer):
                if (playerToChase == null || playerToChase.eliminated || AtDestination())
                {
                    return true;
                }
                break;

            case (BotStrat.ChaseItem):
                if (transformToChase == null || !transformToChase.gameObject.activeSelf)
                {
                    return true;
                }
                if (AtDestination())
                {
                    SetStrategy(BotStrat.PickupItem);
                }
                break;

            case (BotStrat.PickupItem):
                if (pickupTimer.DoneLoop())
                {
                    return true;
                }
                break;

            case (BotStrat.ThrowItem):
                if (throwTimer.DoneLoop())
                {
                    return true;
                }
                break;

            case (BotStrat.DoubleJump):
                if (doubleJumpTimer.DoneLoop())
                {
                    SetStrategy(BotStrat.Attack);
                }
                if (doubleJumpTimer.currentTime > .5f)
                    inputs.JumpInput(true);
                break;

            case (BotStrat.ChaseSpotlight):
                if (status.isInSpotlight)
                {
                    if (Random.value > .5f && CheckForClosePlayer())
                    {
                        return false;
                    }
                    else
                    {
                        SetStrategy(BotStrat.Flex);
                    }
                }
                if (!GameManager.game.spotlight.activeSelf)
                {
                    return true;
                }
                break;

            case (BotStrat.Flex):
                if (status.isInSpotlight)
                {
                    flexTimer.Update();
                }
                else if (flexTimer.DoneLoop() || status.StaminaPercentage > .8f)
                {
                    return true;
                }
                break;

            case (BotStrat.Idle):
                if (idleTimer.DoneLoop())
                    return true;
                break;

            case (BotStrat.ForceUnStuck):
                if (forceUnstuckTimer.DoneLoop())
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public void PickStrategy(bool[] possibleStrats = null)
    {
        if (possibleStrats == null)
        {
            possibleStrats = allPossibleStrats;
        }

        forceStratPickTimer.Restart();

        if (possibleStrats[(int)BotStrat.ReturnToCircle] && status.IsOOB && status.StaminaPercentage < .3f)
        {
            SetStrategy(BotStrat.ReturnToCircle);
            return;
        }

        if (possibleStrats[(int)BotStrat.ChaseSpotlight] && GameManager.game.spotlight.activeSelf)
        {
            Vector2 topDownPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 spotlightTopDownPos = new Vector2(GameManager.game.spotlight.transform.position.x, GameManager.game.spotlight.transform.position.z);
            if (Vector2.SqrMagnitude(topDownPos - spotlightTopDownPos) < 40 || Random.value > .5f)
            {
                SetStrategy(BotStrat.ChaseSpotlight);
                return;
            }
        }

        if (possibleStrats[(int)BotStrat.ThrowItem] && status.combat.equippedItem != null && Random.value > .65f)
        {
            SetStrategy(BotStrat.ThrowItem);
            return;
        }

        float distanceToPlayerToChase = float.MaxValue;

        if (playerToChase != null)
        {
            distanceToPlayerToChase = Vector3.SqrMagnitude(playerToChase.GetTransform.position - transform.position);
        }


        if (possibleStrats[(int)BotStrat.DoubleJump] && playerToChase != null && distanceToPlayerToChase < 36 && (status.buffs[(int)Buff.TopRopes] && Random.value > .4f))
        {
            SetStrategy(BotStrat.DoubleJump);
            return;
        }

        if (possibleStrats[(int)BotStrat.Block] && playerToChase != null && distanceToPlayerToChase < 26 && (status.buffs[(int)Buff.MachoBlock] && Random.value > .77f))
        {
            SetStrategy(BotStrat.Block);
            return;
        }

        if (possibleStrats[(int)BotStrat.Suplex] && playerToChase != null && distanceToPlayerToChase < 30 && Random.value > .4f)
        {
            SetStrategy(BotStrat.Suplex);
            return;
        }


        if (possibleStrats[(int)BotStrat.Attack] && playerToChase != null && distanceToPlayerToChase < 26 && Random.value > .2f)
        {
            SetStrategy(BotStrat.Attack);
            return;
        }

        if ((possibleStrats[(int)BotStrat.Block] || possibleStrats[(int)BotStrat.DodgeRoll]) && playerToChase != null && distanceToPlayerToChase < 26 && ((playerToChase.CurrentPlayerState is AttackGroundStartup && Random.value > .3f) || Random.value > .2f))
        {
            if (Random.value > .5f - (status.buffs[(int)Buff.MachoBlock] ? .2f : 0))
            {
                SetStrategy(BotStrat.Block);
            }
            else
            {
                SetStrategy(BotStrat.DodgeRoll);
            }
            return;
        }


        float value = Random.value;

        if (possibleStrats[(int)BotStrat.ChasePlayer] && value < .33f)
        {
            SetStrategy(BotStrat.ChasePlayer);
            return;
        }
        else if (possibleStrats[(int)BotStrat.ChaseItem] && value >= .33f && value < .66f)
        {
            SetStrategy(BotStrat.ChaseItem);
            return;
        }

        if (status.IsOOB)
        {
            SetStrategy(BotStrat.ReturnToCircle);
            return;
        }

        if (idleCooldownTimer.Done())
        {
            SetStrategy(BotStrat.Idle);
            return;
        }

        if (status.StaminaPercentage < .2)
        {
            SetStrategy(BotStrat.Flex);
            return;
        }

        if (GameManager.game.alivePlayerStatuses.Count == 1)
        {
            if (Random.value > .5f)
                SetStrategy(BotStrat.Flex);
            else
                SetStrategy(BotStrat.Idle);
            return;
        }

        SetStrategy(BotStrat.ChasePlayer);
    }

    public void SetStrategy(BotStrat strat)
    {
        ResetInputs();

        switch (strat)
        {
            case (BotStrat.ReturnToCircle):
                SetDestination(GameManager.game.ringScript.tr.position);
                break;

            case (BotStrat.ChasePlayer):
                playerToChase = GameManager.game.GetRandomAlivePlayerButNotThisOne(status);
                transformToChase = playerToChase.GetTransform;
                SetDestination(transformToChase.position);
                break;

            case (BotStrat.ChaseItem):
                playerToChase = null;
                float closestDistance = float.MaxValue;
                Transform closestItem = null;

                foreach (GameObject g in ItemManager.itemManager.itemsOnGround)
                {
                    if (g == null)
                        continue;
                    float distance = Vector3.SqrMagnitude(g.transform.position - transform.position);
                    bool insideRing = GameManager.game.PositionInsideRing(g.transform.position);
                    if (insideRing || (!insideRing && status.StaminaPercentage > .3f))
                    {
                        if (distance < closestDistance)
                        {
                            closestItem = g.transform;
                            closestDistance = distance;
                        }
                    }
                }

                if (closestItem != null)
                {
                    transformToChase = closestItem;
                    SetDestination(transformToChase.position);
                }
                else
                {
                    PickStrategy(noItemStrats);
                    return;
                }

                break;

            case (BotStrat.ChaseSpotlight):
                transformToChase = GameManager.game.spotlight.transform;
                SetDestination(GameManager.game.spotlight.transform.position);
                break;

            case (BotStrat.Flex):
                //if we want to flex but we have an item in hand, then quickly throw it
                if (status.combat.equippedItem != null)
                {
                    throwTimer.Restart(0, .25f);
                    SetStrategy(BotStrat.ThrowItem);
                    return;
                }
                path = null;
                inputs.ThrowInput(true);
                break;

            case (BotStrat.ThrowItem):
                if (status.combat.equippedItem == null)
                {
                    SetStrategy(BotStrat.Flex);
                    return;
                }
                transformToChase = GameManager.game.GetRandomAlivePlayerButNotThisOne(status).transform;
                path = null;
                inputs.ThrowInput(true);
                break;

            case (BotStrat.PickupItem):
                path = null;
                inputs.PickUpInput(true);
                break;

            case (BotStrat.Suplex):
                inputs.PickUpInput(true);
                break;

            case (BotStrat.DoubleJump):
                inputs.JumpInput(true);
                break;

            case (BotStrat.Idle):
                path = null;
                idleCooldownTimer.Restart();
                break;

            case (BotStrat.ForceUnStuck):
                path = null;
                forceStratPickTimer.Restart();
                break;
        }

        currentStrat = strat;
    }

    public void MoveToDestination()
    {
        if (path == null)
            return;

        if (currentCorner >= path.corners.Length || Vector3.SqrMagnitude(destination - tr.position) < 1.2f)
        {
            PickStrategy();
            return;
        }


        Vector3 desiredVelocity = path.corners[currentCorner] - tr.position;

        float topDownDistance = Vector2.SqrMagnitude(new Vector2(path.corners[currentCorner].x - tr.position.x, path.corners[currentCorner].z - tr.position.z));


        if (desiredVelocity.y > 0 && topDownDistance < 7)
        {
            inputs.JumpInput(true);
        }

        inputs.MoveInput(new Vector2(desiredVelocity.x, desiredVelocity.z));

        if (Vector3.SqrMagnitude(path.corners[currentCorner] - tr.position) < 1.2f)
        {
            currentCorner++;
        }
    }

    public void DrawPathDebug()
    {
        for (int i = -1; i < path.corners.Length - 1; i++)
        {
            if (i == -1)
                Debug.DrawLine(transform.position, path.corners[0]);
            else
                Debug.DrawLine(path.corners[i], path.corners[i + 1]);
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
        NavMeshHit destinationHit;
        destination = position;
        if (NavMesh.SamplePosition(position, out destinationHit, 20, NavMesh.AllAreas))
        {
            destination = destinationHit.position;
        }
        path = new NavMeshPath();

        NavMeshHit ourPosHit;
        Vector3 currentPos = transform.position;
        if (NavMesh.SamplePosition(currentPos, out ourPosHit, 20, NavMesh.AllAreas))
        {
            currentPos = ourPosHit.position;
        }
        //randomly chose to use jump paths or not
        NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        currentCorner = 0;
    }

    public void ResetInputs()
    {
        inputs.MoveInput(Vector2.zero);
        inputs.JumpInput(false);
        inputs.BlockInput(false);
        inputs.ThrowInput(false);
        inputs.AttackInput(false);
        inputs.PickUpInput(false);
        inputs.DodgeRollInput(false);
    }

    public bool CheckForClosePlayer()
    {
        PlayerStatus closestPlayer = null;
        float closestDistance = float.MaxValue;
        foreach (PlayerStatus p in GameManager.game.alivePlayerStatuses)
        {
            if (p.playerNumber != status.playerNumber)
            {
                float distance = Vector3.SqrMagnitude(p.GetTransform.position - transform.position);
                if (distance < closestDistance && distance < 16)
                {
                    closestPlayer = p;
                    closestDistance = distance;
                }
            }
        }

        if (closestPlayer != null)
        {
            playerToChase = closestPlayer;
            transformToChase = closestPlayer.transform;
            PickStrategy(allAttackStrats);
            return true;
        }
        return false;
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
