using UnityEngine;

class EnemyGenerator : MonoBehaviour
{
    public GameObject MarineEnemy;
    private PlayerController _Player;

    void Start()
    {
        _Player = transform.GetComponentInChildren<PlayerController>();
    }

    void Update()
    {
        Debug.Log(_Player);
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Instantiate(MarineEnemy, _Player.transform.position + Vector3.up * 3 + Vector3.right * 8, Quaternion.identity);
    }
}

