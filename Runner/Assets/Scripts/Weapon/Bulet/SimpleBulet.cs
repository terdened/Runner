using UnityEngine;
using System.Collections;

public class SimpleBulet : BaseBulet 
{
    
    protected void Start()
    {
        base.Start();
        _direction = new Vector3(InitDirection.x, InitDirection.y, 0);
    }

    protected void Update()
    {
        base.Update();
        transform.Translate(_direction);
    }

    override protected void OnCollide(Collider2D other)
    {
        var splashEffect = Instantiate(Splash, transform.position, Quaternion.identity);

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
            if (raycast.collider.tag != "Player")
            {
                transform.position = new Vector3(raycast.point.x, raycast.point.y, 0);
                OnCollide(raycast.collider);
            }
        }
    }
}
