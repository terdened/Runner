﻿using UnityEngine;

[RequireComponent(typeof(BasePhysics))]
class SimpleEnemyController : MonoBehaviour
{
    public Vector2 Velocity = Vector2.left * 0.05f;
    private BasePhysics _enemyPhysics;

    void Start()
    {
        _enemyPhysics = GetComponent<NewColliderLogicPhysics>();
    }

    void Update()
    {
        if(_enemyPhysics.OnGround)
            _enemyPhysics.SetVelocity(Velocity);
        else
            _enemyPhysics.SetVelocity(Vector2.zero);
    }
}

