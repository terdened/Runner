using UnityEngine;
class EnemyType2PersonController : EnemyPersonController
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

        HandleAttack();
    }

    void HandleAttack()
    {
        var rayStartPosition = new Vector2(0, 0);
        rayStartPosition.x = transform.position.x;
        rayStartPosition.y = transform.position.y;

        var raycast = Physics2D.Raycast(rayStartPosition,
                                                Vector2.left,
                                                4f,
                                                LayerMask.GetMask("Player"));

        if (raycast.collider != null)
        {
            var weapon = GetComponentInChildren<EnemyPistol>(); ;
            weapon.Fire();
        }
    }

    override protected void UpdateAnimation()
    {

    }
}
