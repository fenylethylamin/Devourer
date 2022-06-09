using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JLiftAnimation : MonoBehaviour
{
    [SerializeField] public Animator _afterBossIsDeadAnimation;

    public BossHealthController _theBoss;
    
    public bool _isBossAlive;
    public GameObject cube;

    private bool isPlatformLowered = false;

    private void Update()
    {
        CheckForDeadBoss();
    }


    public void CheckForDeadBoss()
    {
        if (_theBoss == null || _theBoss.currentHealth <= 0)

        {
            _isBossAlive = false;
        }

        else

        {
            _isBossAlive = true;
        }

        LowerThePlatform();
    }

    public void LowerThePlatform()
    {
        if (!_isBossAlive && !isPlatformLowered)
        {
            isPlatformLowered = true;
            _afterBossIsDeadAnimation.SetTrigger("bossIsDead");
          //  cube.SetActive(true);

        }
    }
}


