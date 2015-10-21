using UnityEngine;

[RequireComponent(typeof(BasePersonModel))]
[RequireComponent(typeof(BasePhysics))]
[RequireComponent(typeof(Animator))]
abstract class BasePersonController : MonoBehaviour
{
    protected BasePersonModel PersonModel;
    protected BasePhysics PersonPhysics;
    protected Animator PersonAnimator;

    protected void Start()
    {
        PersonModel = GetComponent<BasePersonModel>();
        PersonPhysics = GetComponent<BasePhysics>();
        PersonAnimator = GetComponent<Animator>();
        PersonPhysics.SetVelocity(PersonModel.Velocity);
    }

    abstract protected void UpdateAnimation();

    public void GetDamage(float damage)
    {
        PersonModel.HealthPower -= damage;

        if (PersonModel.HealthPower <= 0)
        {
            if (gameObject.tag == "Player")
                Application.LoadLevel("perspective");

            DestroyObject(gameObject);
        }
    }
}

