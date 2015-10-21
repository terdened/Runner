using UnityEngine;
using System.Collections;


class Pistol : Weapon
{
    protected new void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Fire1") && CanFire())
        {
            Fire();
        }
    }

    public override void Fire()
    {
        var bulet = Instantiate(Bulet, transform.position, Quaternion.identity);
        _counter = 0;
    } 
}

