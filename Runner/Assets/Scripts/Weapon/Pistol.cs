using UnityEngine;
using System.Collections;


class Pistol : Weapon
{
    protected override void Fire()
    {
        var bulet = Instantiate(Bulet, transform.position, Quaternion.identity);
    } 
}

