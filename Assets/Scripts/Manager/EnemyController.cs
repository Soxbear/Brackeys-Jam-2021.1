using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameManager gameManage;
    // Enemy Count
    private int enemyLeft = 0;

    public void enemyDies()
    {
        enemyLeft--;
        print(enemyLeft);

        if(enemyLeft <= 0)
        {
            print("Victory");
            gameManage.GameWin();
        }
    }
    public void enemySpawned()
    {
        enemyLeft++;
        print(enemyLeft);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
