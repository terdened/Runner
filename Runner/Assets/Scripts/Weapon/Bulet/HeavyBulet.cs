using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BuletPhysics))]
public class HeavyBulet : BaseBulet 
{
    public bool IsPlayer = false;
    public Vector2 InitForce = new Vector2(0, 0);
    public float SplashDistance = 1f;
    public float SplashPower = 0.1f;

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

            Debug.Log(objectPosition.x);

            var distance = Vector2.Distance(objectPosition, buletPosition);


            Debug.Log(objectPosition.x);

            if (distance != 0 && distance < SplashDistance)
            {
                var forceDirection = new Vector2(objectPosition.x - buletPosition.x, objectPosition.y - buletPosition.y);

                if (Mathf.Abs(forceDirection.x) > Mathf.Abs(forceDirection.y))
                {
                    forceDirection.y = forceDirection.y / Mathf.Abs(forceDirection.x);
                    forceDirection.x = forceDirection.x / Mathf.Abs(forceDirection.x);
                }
                else
                {
                    forceDirection.x = forceDirection.x / Mathf.Abs(forceDirection.y);
                    forceDirection.y = forceDirection.y / Mathf.Abs(forceDirection.y);
                }

                var distanceK = (1 - (distance / SplashDistance));

                physicObject.AddForce(forceDirection * SplashPower * distanceK);
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
            transform.position = new Vector3(raycast.point.x + 0.5f, raycast.point.y, 0);
            OnCollide(raycast.collider);
        }

        if (_buletPhysics.OnGround)
            OnCollide(null);
    }
}
