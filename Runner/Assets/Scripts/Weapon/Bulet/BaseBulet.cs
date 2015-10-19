using UnityEngine;
using System.Collections;

public abstract class BaseBulet : MonoBehaviour {

    #region public params
    public float Damage;
    public Vector2 InitDirection;
    public GameObject Splash;
    #endregion

    private Vector3 _startPosition;
    protected Vector3 _direction;
    private int _collisionCount = 0;

    protected void Start()
    {
        _startPosition = transform.position;
    }

    protected void Update()
    {
        if (Vector3.Distance(_startPosition, transform.position) > 100)
            Destroy(gameObject);

        CheckCollision();
    }

    protected abstract void OnCollide(Collider2D other);

    protected abstract void CheckCollision();
}
