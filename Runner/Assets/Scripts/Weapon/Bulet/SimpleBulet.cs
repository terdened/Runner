using UnityEngine;
using System.Collections;

public class SimpleBulet : BaseBulet 
{
    private Vector3 _direction;
    
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
}
