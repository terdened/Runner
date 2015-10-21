using UnityEngine;

class EnemyGenerator : MonoBehaviour
{
    public GameObject MarineEnemy;
    public GameObject EnemyType2;
    private PlayerPersonController _Player;

    void Start()
    {
        _Player = transform.GetComponentInChildren<PlayerPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Instantiate(MarineEnemy, _Player.transform.position + Vector3.up * 3 + Vector3.right * 8, Quaternion.identity);

        if (Input.GetKeyDown(KeyCode.Keypad2))
            Instantiate(EnemyType2, _Player.transform.position + Vector3.up * 3 + Vector3.right * 8, Quaternion.identity);
    }
}

