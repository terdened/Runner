﻿using UnityEngine;

class PlayerPersonController : BasePersonController
{
    protected GameObject PlayerCamera;
    private int _currentWeapon = 0;
    private GameObject _lastWeapon;

    public GameObject[] Weapons;

    protected new void Start()
    {
        base.Start();
        InitWeapon();
        PlayerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
            ((PlayerPersonModel)PersonModel).JetpackCharge = 100f;

        if (PersonPhysics.OnGround && Input.GetButtonDown("Jump"))
            PersonPhysics.AddForce(((PlayerPersonModel)PersonModel).JumpDirection * ((PlayerPersonModel)PersonModel).JumpPower);
        else if (Input.GetButton("Jump") && ((PlayerPersonModel)PersonModel).JetpackCharge > 0)
        {
            ((PlayerPersonModel)PersonModel).JetpackCharge -= 0.1f;
            PersonPhysics.AddForce(Vector2.up * 0.006f);
            Debug.Log(((PlayerPersonModel)PersonModel).JetpackCharge);
        }

        if (PersonPhysics.OnGround)
            PersonPhysics.SetVelocity(PersonModel.Velocity);
        else
            PersonPhysics.SetVelocity(Vector2.zero);

        UpdateAnimation();
        UpdateCamera();
        HandleChangeWeapon();
    }

    override protected void UpdateAnimation()
    {
        if (PersonPhysics.OnGround && PersonPhysics.GetMomentum().x <= 0)
            PersonAnimator.SetInteger("State", 0);

        if (PersonPhysics.OnGround && PersonPhysics.GetMomentum().x > 0)
            PersonAnimator.SetInteger("State", 1);

        if (!PersonPhysics.OnGround && PersonPhysics.GetMomentum().y > 0)
            PersonAnimator.SetInteger("State", 2);

        if (!PersonPhysics.OnGround && PersonPhysics.GetMomentum().y < 0)
            PersonAnimator.SetInteger("State", 3);
    }

    private void HandleChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentWeapon--;
            if (_currentWeapon < 0)
                _currentWeapon = Weapons.Length - 1;

            InitWeapon();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentWeapon++;
            if (_currentWeapon >= Weapons.Length - 1)
                _currentWeapon = 0;

            InitWeapon();
        }
    }

    private void InitWeapon()
    {
        if (_lastWeapon != null)
            DestroyObject(_lastWeapon);

        var newWeapon = (GameObject)Instantiate(Weapons[_currentWeapon]);
        newWeapon.transform.SetParent(transform);
        newWeapon.transform.localPosition = Vector3.zero;

        _lastWeapon = newWeapon;
    }

    public void UpdateCamera()
    {
        Vector2 momentum = PersonPhysics.GetMomentum();
        Vector3 toPosition = new Vector3(momentum.x * 16 + 2, momentum.y * 6, -10);
        Vector3 cameraPosition = MoveTowards(PlayerCamera.transform.localPosition, toPosition, 0.1f);
        PlayerCamera.transform.localPosition = cameraPosition;
    }

    private Vector3 MoveTowards(Vector3 from, Vector3 to, float speed)
    {
        Vector3 result = Vector3.Lerp(from, to, speed);
        return result;
    }


}
