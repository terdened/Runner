using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

    private Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        var animationState = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        if (animationState.ToString() == "-1502611000")
            DestroyObject(gameObject);
	}
}
