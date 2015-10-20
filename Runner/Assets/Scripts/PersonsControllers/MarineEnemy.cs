using UnityEngine;

[RequireComponent(typeof(BasePhysics))]
class MarineEnemy : MonoBehaviour
{
    public Vector2 Velocity = Vector2.left * 0.05f;
    public float HealthPower = 100f;
    private BasePhysics _enemyPhysics;

    void Start()
    {
        _enemyPhysics = GetComponent<EnemyPhysics>();
    }

    void Update()
    {
        if (_enemyPhysics.OnGround)
            _enemyPhysics.SetVelocity(Velocity);
        else
            _enemyPhysics.SetVelocity(Vector2.zero);
    }

    public Vector2 GetMomenut()
    {
        return _enemyPhysics.GetMomentum();
    }

    public void GetDamage(float damage)
    {
        HealthPower -= damage;

        if (HealthPower <= 0)
            DestroyObject(gameObject);
    }
}


