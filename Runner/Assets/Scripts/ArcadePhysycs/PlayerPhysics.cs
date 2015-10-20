
using UnityEngine;

class PlayerPhysics : BasePhysics
{
    private float enemyDistance = 2f;

    override protected void Update()
    {
        base.Update();
        EnemyInFront();
    }

    private void EnemyInFront()
    {
        this._forceVelocity = false;
        
        for(int i = 0; i < 3; i++ )
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x + _size.x / 2;
            rayStartPosition.y = transform.position.y - _size.y / 2 + _size.y / 2 * i;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                                    Vector2.right,
                                                    enemyDistance,
                                                    LayerMask.GetMask("Enemy"));

            if (raycast.collider != null && raycast.collider.gameObject.tag == "Marine Enemy")
            {
                var enemyPhysics = raycast.collider.gameObject.GetComponent<MarineEnemy>();
                this._velocity = enemyPhysics.Velocity;
                this._momentum.x = Mathf.MoveTowards(_momentum.x, enemyPhysics.GetMomenut().x, 0.001f);

                if(raycast.point.x - rayStartPosition.x < 1.9f)
                    this._momentum.x = Mathf.MoveTowards(_momentum.x, enemyPhysics.GetMomenut().x/1.5f, 0.005f);
                else
                    this._momentum.x = Mathf.MoveTowards(_momentum.x, enemyPhysics.GetMomenut().x, 0.001f);

                this._forceVelocity = true;
            }
        }
    }
}
