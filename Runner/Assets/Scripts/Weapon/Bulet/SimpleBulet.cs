using UnityEngine;
using System.Collections;

public class SimpleBulet : BaseBulet 
{
    protected void Start()
    {
        base.Start();
        _direction = new Vector3(InitDirection.x, InitDirection.y, 0);
        this.Damage = 15f;
    }

    protected void Update()
    {
        base.Update();
        transform.Translate(_direction);
    }

    override protected void OnCollide(Collider2D other)
    {
        var splashEffect = Instantiate(Splash, transform.position, Quaternion.identity);

        if (other.tag == "Marine Enemy")
        {
            var enemyPhysics = other.gameObject.GetComponent<EnemyPhysics>();
            enemyPhysics.AddForce(Vector2.right * 0.03f + Vector2.up * 0.03f);

            var enemyController = other.GetComponent<MarineEnemy>();
            enemyController.GetDamage(Damage);
        }

        DestroyObject(gameObject);       
    }

    override protected void CheckCollision()
    {
        var startPosition = new Vector2(transform.position.x, transform.position.y);
        var direction = new Vector2(_direction.x, _direction.y);
        var raycast = Physics2D.Raycast(startPosition,
                                        direction,
                                        Vector3.Distance(Vector3.zero, _direction),
                                        LayerMask.GetMask("Floor", "Enemy"));

        Debug.DrawLine(transform.position, transform.position + _direction, Color.blue);

        if (raycast.collider != null)
        {
            transform.position = new Vector3(raycast.point.x + 0.5f, raycast.point.y, 0);
            OnCollide(raycast.collider);
        }
    }
}
