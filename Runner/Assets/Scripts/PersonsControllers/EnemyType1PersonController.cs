using UnityEngine;

class EnemyType1PersonController : EnemyPersonController
{
    protected new void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (PersonPhysics.OnGround)
            PersonPhysics.SetVelocity(PersonModel.Velocity);
        else
            PersonPhysics.SetVelocity(Vector2.zero);
    }

    override protected void UpdateAnimation()
    {
        
    }
}
