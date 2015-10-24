using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BuletPhysics))]
public class HeavyBulet : BaseBulet 
{
    public bool IsPlayer = false;
    public Vector2 InitForce = new Vector2(0, 0);
    public float SplashDistance = 1f;
    public Vector2 SplashPower = new Vector2(0.1f, 0.3f);

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

        var physicObjects = (BasePhysics[]) FindObjectsOfType(typeof(BasePhysics));
        var buletPosition = new Vector2(transform.position.x, transform.position.y);

        foreach (var physicObject in physicObjects)
        {
            var objectPosition = new Vector2(physicObject.transform.position.x, physicObject.transform.position.y);
            
            var distance = Vector2.Distance(objectPosition, buletPosition);
            
            if (distance != 0 && distance < SplashDistance)
            {
                
                var distanceK = (1 - (distance / SplashDistance));

                physicObject.AddForce(new Vector2(Mathf.Sign(objectPosition.x - buletPosition.x) * SplashPower.x, SplashPower.y) * distanceK);
                var personController = physicObject.transform.GetComponent<BasePersonController>();

                if (personController != null)
                    personController.GetDamage(Damage * distanceK);
            }
        }

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
            OnCollide(raycast.collider);
        }

        if (_buletPhysics.OnGround)
            OnCollide(null);
    }
}
