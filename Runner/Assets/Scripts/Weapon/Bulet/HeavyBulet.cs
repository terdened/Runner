using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BuletPhysics))]
public class HeavyBulet : BaseBulet 
{
    public bool IsPlayer = false;
    public Vector2 InitForce = new Vector2(0, 0);

    private BuletPhysics _buletPhysics;

    protected void Start()
    {
        base.Start();

        _buletPhysics = GetComponent<BuletPhysics>();

        InitLayers(IsPlayer, true);
        _buletPhysics.AddForce(InitForce);
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
        if(_buletPhysics.OnGround)
            OnCollide(null);
    }
}
