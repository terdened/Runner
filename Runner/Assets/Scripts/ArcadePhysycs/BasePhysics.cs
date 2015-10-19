using UnityEngine;
using System.Collections;

public abstract class BasePhysics : MonoBehaviour {

    #region public params
    public float G = 0.15f;
    public Vector2 GravityDirection = Vector2.down;
    public bool OnGround = false;
    public float AirResistance = 0.0001f;
    public float FloorResistance = 0.001f;
    #endregion

    #region private params
    //private float Skin = 0.001f;
    protected float MinRaycastDistance = 0.33f;
    protected Vector2 _momentum = new Vector2(0, 0);
    protected Vector2 _velocity = new Vector2(0, 0);
    protected float _stairHeight = 0.2f;
    protected float _aVelocity = 0.001f;
    protected Vector2 _resistance;
    protected Vector2 _size;
    protected int _horizontalRasycastCount;
    protected int _verticalRasycastCount;
    #endregion

    void Start()
    {
        _size = GetComponent<BoxCollider2D>().size;
        _horizontalRasycastCount = (int)(_size.y / MinRaycastDistance) + 1;
        _verticalRasycastCount = (int)(_size.x / MinRaycastDistance) + 1;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 aIncrease = new Vector2(0, 0);

        aIncrease = HandleGravity(aIncrease);
        aIncrease = HandleVelocity(aIncrease);
        
        aIncrease = UpdateRaycasts(aIncrease);

        _momentum.x += aIncrease.x;
        _momentum.y += aIncrease.y;

        HandleFade();

        transform.Translate(_momentum.x, _momentum.y, 0);
    }

    protected abstract Vector2 UpdateRaycasts(Vector2 resultMovement);   

    private Vector2 HandleGravity(Vector2 resultMovement)
    {
        resultMovement.x += G * GravityDirection.x * Time.deltaTime;
        resultMovement.y += G * GravityDirection.y * Time.deltaTime;

        return resultMovement;
    }

    private Vector2 HandleVelocity(Vector2 resultMovement)
    {
        if(_velocity.x > 0)
        {
            if (_momentum.x < _velocity.x)
                resultMovement.x += _aVelocity;
        }

        if (_velocity.x < 0)
        {
            if (_momentum.x > _velocity.x)
                resultMovement.x -= _aVelocity;
        }

        if (_velocity.y > 0)
        {
            if (_momentum.y < _velocity.y)
                resultMovement.y += _aVelocity;
        }

        if (_velocity.y < 0)
        {
            if (_momentum.y > _velocity.y)
                resultMovement.y -= _aVelocity;
        }

        return resultMovement;
    }

    private void HandleFade()
    {
        if (_momentum.x > 0)
        {
            _momentum.x -= _resistance.x;
            if (_momentum.x < 0)
                _momentum.x = 0;
        }

        if (_momentum.x < 0)
        {
            _momentum.x += _resistance.x;
            if (_momentum.x > 0)
                _momentum.x = 0;
        }

        if (_momentum.y > 0)
        {
            _momentum.y -= _resistance.y;
            if (_momentum.y < 0)
                _momentum.y = 0;
        }

        if (_momentum.y < 0)
        {
            _momentum.y += _resistance.y;
            if (_momentum.y > 0)
                _momentum.y = 0;
        }
    }

    public void AddForce(Vector2 direction)
    {
        _momentum += direction;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    public Vector2 GetMomentum()
    {
        return _momentum;
    }
}
