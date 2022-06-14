using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour


{
    private bool chasing;
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

    //shooting
    public GameObject bullet;
    public Transform[] firePoints;
    public float fireRate, waitBetweenShots = 2f, timeToShoot = 1f;
    private float fireCount, shotWaitCounter, shootTimeCounter;
    //shooting


    void Start()
    {
        startPoint = transform.position;
        enemyPosition = transform.position;
        //Patrolling();
        agent.speed = normalspeed;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;

    }


    void Update()
    {

        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;

            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;

                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }

                if (timer > 2f)
                {
                    timer = 0;
                    //Patrolling();
                }
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
                    //Patrolling();
                }
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

            if (shotWaitCounter > 0)

            {
                shotWaitCounter -= Time.deltaTime;

                if (shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }
            }
            else

            {

                if (PlayerController.instance.gameObject.activeInHierarchy)

                {
                    shootTimeCounter -= Time.deltaTime;

                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;
                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            //@ChochosanDev limit the fire angle while keeping it dynamic, no hardcoded fire points
                            foreach(Transform firePoint in firePoints)
                            {
                                firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 0.6f, 0f));
                            }

                            //check the angle to the player
                            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                //@ChochosanDev made dynamic
                                foreach (Transform firePoint in firePoints)
                                {
                                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                                }                         


                            }
                            else

                            {
                                shotWaitCounter = waitBetweenShots;
                            }
                        }

                        agent.destination = transform.position;


                    }
                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }



                }
            }
        }
    }

    //private void Patrolling()

    //{

        //float randomZ = Random.Range(-walkFromCenter, walkFromCenter) + enemyPosition.z;
        //float randomX = Random.Range(-walkFromCenter, walkFromCenter) + +enemyPosition.x;

        //walkPoint = new Vector3(randomX, transform.position.y, randomZ);

    //}

    public void MoveAside()

    {
        chasing = false;
        wasShot = false;
        transform.Translate(-Vector3.right * normalspeed * Time.deltaTime);

    }

    public void GetShot()
    {
        wasShot = true;

        chasing = true;
    }

    public void DoNotAttack()
    {
        wasShot = false;

        chasing = false;

    }
}



