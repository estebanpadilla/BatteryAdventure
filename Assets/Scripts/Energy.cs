using UnityEngine;

public class Energy : MonoBehaviour
{
    public bool taken = false;
    public GameObject explosion;
    public int energyType = 1;
    float _energy = 2;

    [Range(10f, 20f)]
    public float waitingTime = 15f;
    float _lastPickupTime = 0;
    Color _color;

    void Awake()
    {
        switch (energyType)
        {
            case 1:
                _energy = 5;
                _color = new Color(0.6f, 1.0f, 1.0f);
                gameObject.GetComponent<SpriteRenderer>().color = _color;
                break;
            case 2:
                _energy = 10;
                _color = new Color(1.0f, 1.0f, 0.0f);
                gameObject.GetComponent<SpriteRenderer>().color = _color;
                break;
            case 3:
                _energy = 20;
                _color = new Color(1.0f, 0.3f, 0.0f);
                gameObject.GetComponent<SpriteRenderer>().color = _color;
                break;
            default:
                _color = new Color(0.6f, 1.0f, 1.0f);
                gameObject.GetComponent<SpriteRenderer>().color = _color;
                break;
        }
    }

    void Update()
    {
        if (taken)
        {
            if (Time.time >= _lastPickupTime)
            {
                EnableEnergy();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // mark as taken so doesn't get taken multiple times
            taken = true;

            // if explosion prefab is provide, then instantiate it
            if (explosion)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            other.GetComponent<CharacterController2D>().AddEnergy(_energy);

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

            _lastPickupTime = Time.time + waitingTime;
        }
    }

    void EnableEnergy()
    {
        taken = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = _color;
    }

}
