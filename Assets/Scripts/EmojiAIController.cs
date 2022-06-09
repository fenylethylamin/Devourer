using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmojiAIController : MonoBehaviour


{
    public bool chasing;
    public float chasingspeed = 5f;
    public float normalspeed = 1f;
    public Transform Player;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;
    private Vector3 targetPoint, startPoint;
    public NavMeshAgent agent;
    public float keepChasingTime = 5f;
    private float chaseCounter;
    public bool wasShot;
    private Vector3 enemyPosition;
    public Vector3 walkPoint;
    public float walkFromCenter = 2f;
    private float timer;

    void Start()
    {
        startPoint = transform.position;
        enemyPosition = transform.position;
        Patrolling();
        agent.speed = normalspeed;
    } 


    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;



        if (!chasing)
        {
            agent.speed = normalspeed;

            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
            }

            agent.destination = walkPoint;

            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 0;
                Patrolling();
            }

        }

        else
        {

            agent.speed = chasingspeed;
           
            transform.LookAt(Player);

            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = walkPoint;


                timer += Time.deltaTime;
                if (timer > 2f)
                {
                    timer = 0;
                    Patrolling();
                }
            

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                if (!wasShot)
                {
                    chasing = false;
                    chaseCounter = keepChasingTime;

                }
            }

            else
            {
                wasShot = false;
            }

        }
    }

    }

    private void Patrolling()

    {

        float randomZ = Random.Range(-walkFromCenter, walkFromCenter) + enemyPosition.z;
        float randomX = Random.Range(-walkFromCenter, walkFromCenter) + +enemyPosition.x;

        walkPoint = new Vector3(randomX, transform.position.y, randomZ);
        
    }


    public void GetShot()
    {
        wasShot = true;

        chasing = true;

    }
}