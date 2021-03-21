using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{

    EnemyControl enemy;
    Rigidbody2D physics;
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyControl>();
        physics = GetComponent<Rigidbody2D>();
        physics.AddForce(enemy.getDirection() * 1500);
    }

}
