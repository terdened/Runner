using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    #region public params
    public int FireRate;
    public GameObject Bulet;
    #endregion

    protected int _counter = 0;

    protected void Update()
    {
        UpdateCounter();
    }

    public abstract void Fire();

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
