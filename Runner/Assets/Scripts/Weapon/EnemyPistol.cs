using UnityEngine;
class EnemyPistol : Weapon
{
    protected new void Update()
    {
        base.Update();
    }

    public override void Fire()
    {
        if(CanFire())
        {
            var bulet = Instantiate(Bulet, transform.position, Quaternion.identity);
            _counter = 0;
        }
    }
}

