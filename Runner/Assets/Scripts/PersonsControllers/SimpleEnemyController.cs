using UnityEngine;

[RequireComponent(typeof(EnemyPhysics))]
class SimpleEnemyController : MonoBehaviour
{
    public Vector2 Velocity = Vector2.left * 0.05f;
    private EnemyPhysics _enemyPhysics;

    void Start()
    {
        _enemyPhysics = GetComponent<EnemyPhysics>();
        _enemyPhysics.SetVelocity(Velocity);
    }
}

