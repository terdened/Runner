﻿using UnityEngine;
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

        InitLayers(IsPlayer);
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

        if(other == null)
        {
            DestroyObject(gameObject);
            return;
        }

        if (other.tag == "Marine Enemy")
        {
            var enemyPhysics = other.gameObject.GetComponent<EnemyPhysics>();
            enemyPhysics.AddForce(Vector2.right * 0.03f + Vector2.up * 0.03f);
        }

        if (other.tag == "Player")
        {
            var playerPhysics = other.gameObject.GetComponent<PlayerPhysics>();
            playerPhysics.AddForce(Vector2.left * 0.03f + Vector2.up * 0.03f);
        }


        var personController = other.GetComponent<BasePersonController>();

        if (personController != null)
            personController.GetDamage(Damage);
        
        DestroyObject(gameObject);       
    }

    override protected void CheckCollision()
    {
        var startPosition = new Vector2(transform.position.x, transform.position.y);
        var direction = new Vector2(_direction.x, _direction.y);
        var raycast = Physics2D.Raycast(startPosition,
                                        _buletPhysics.GetMomentum(),
                                        Vector3.Distance(Vector3.zero, _buletPhysics.GetMomentum()),
                                        LayerMask.GetMask(_activeLayers.ToArray()));

        Debug.DrawLine(transform.position, transform.position + _direction, Color.blue);

        if (raycast.collider != null)
        {
            transform.position = new Vector3(raycast.point.x + 0.5f, raycast.point.y, 0);
            OnCollide(raycast.collider);
        }

        if (_buletPhysics.OnGround)
            OnCollide(null);
    }
}
