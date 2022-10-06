using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeelSpotlight : MonoBehaviour
{

    public GameObject decal;

    public float timeAlive;

    public Vector2 velocity;

    public Transform tr;

    public float blinkTimer;


    // Start is called before the first frame update
    void Start()
    {
        tr = gameObject.transform;
        velocity = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive < 5)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer > .5)
            {
                blinkTimer = 0;
                decal.SetActive(!decal.activeSelf);
            }
        }
        else
        {
            decal.SetActive(true);

            PlayerStatus target = GameManager.game.GetLeastActivePlayer();

            Vector2 inputDirection = (new Vector2(target.transform.position.x, target.transform.position.z) - new Vector2(tr.position.x, tr.position.z)).normalized;

            float differenceX = velocity.x - (8 * inputDirection.x);
            float differenceZ = velocity.y - (8 * inputDirection.y);
            Vector2 speedDecayDirection = new Vector2(differenceX, differenceZ) / 5;

            velocity.x = velocity.x - speedDecayDirection.x * 15 * Time.deltaTime;
            velocity.y = velocity.y - speedDecayDirection.y * 15 * Time.deltaTime;

            tr.position = new Vector3(tr.position.x + velocity.y * Time.deltaTime, tr.position.y, tr.position.z + velocity.y * Time.deltaTime);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (timeAlive < 5)
            return;

        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus status = other.gameObject.GetComponent<PlayerStatus>();
            status.SetHeel();
            gameObject.SetActive(false);
        }
    }
}
