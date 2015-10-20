using UnityEngine;

class LevelGenerator : MonoBehaviour
{
    public GameObject[] Template;
    private PlayerController _Player;
    public GameObject LastTemplate;
    public bool WithWhole = true;

    void Start()
    {
        _Player = transform.GetComponentInChildren<PlayerController>();
    }

    void Update()
    {
        if (_Player.transform.position.x > LastTemplate.transform.position.x + 20)
        {
            var distance = 46.5f;

            if (WithWhole)
                distance = 49;

            LastTemplate = (GameObject)Instantiate(Template[0], Vector3.right * (LastTemplate.transform.position.x + distance), Quaternion.identity);
        }
    }
}

