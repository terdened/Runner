using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    #region public params
    public int FireRate;
    public GameObject Bulet;
    #endregion

    private int _counter = 0;

    void Update()
    {
        UpdateCounter();

        if (Input.GetButtonDown("Fire1") && CanFire())
        {
            Fire();
            _counter = 0;
        }
    }

    protected abstract void Fire();

    protected bool CanFire()
    {
        return _counter == FireRate;
    }

    private void UpdateCounter()
    {
        if (_counter < FireRate)
            _counter++;
    } 
}
