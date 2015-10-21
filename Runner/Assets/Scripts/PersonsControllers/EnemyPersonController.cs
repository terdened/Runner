using UnityEngine;

abstract class EnemyPersonController : BasePersonController
{
    public Vector2 Velocity
    {
        get { return PersonModel.Velocity; }
    }

    public Vector2 Momentum
    {
        get { return PersonPhysics.GetMomentum(); }
    }
    protected new void Start()
    {
        base.Start();
    }
}
