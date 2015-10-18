using UnityEngine;
using System.Collections;

public class BaseBulet : MonoBehaviour{

    #region public params
    public float Damage;
    public Vector2 InitDirection;
    #endregion

    private Vector3 _startDirection;

    protected void Start()
    {
        _startDirection = transform.position;
    }

    protected void Update()
    {
        if (Vector3.Distance(_startDirection, transform.position) > 100)
            Destroy(gameObject);
    }
}
