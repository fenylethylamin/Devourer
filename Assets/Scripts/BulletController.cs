using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public bool damageEnemy, damagePlayer;

    public int damage;

    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var destroyBullet = false;
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>()?.DamageEnemy(damage);
            destroyBullet = true;
        }
        if (other.gameObject.tag == "BossEnemy" && damageEnemy)
        {
            other.gameObject.GetComponent<BossHealthController>()?.DamageEnemy(damage);
            destroyBullet = true;
        }
        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.DamagePlayer(damage);
            destroyBullet = true;
        }
        if (other.gameObject.layer == 3)
        {
            destroyBullet = true;
        }

        if (destroyBullet)
        {
            Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
            Destroy(gameObject);
        }
    }
}