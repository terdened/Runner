using UnityEngine;
using System.Collections;

public class SimpleBulet : BaseBulet 
{
    private Vector3 _direction;
    private int _collisionCount = 0;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Here");
        _collisionCount++;

        if (_collisionCount > 1)
        {
            DestroyObject(gameObject);
        }
    }
}
