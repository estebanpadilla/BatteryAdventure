using UnityEngine;

public class SpawnCoin : MonoBehaviour
{

    public int coinValue = 1;
    public bool taken = false;
    public GameObject explosion;

    Rigidbody2D _rigidbody;

    float _animationWaiting = 0.25f;
    float _animationTime;
    bool _isAnimating = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null) // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component missing from this gameobject");

    }

    // if the player touches the coin, it has not already been taken, and the player can move (not dead or victory)
    // then take the coin
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player") && (!taken))
        {
            // mark as taken so doesn't get taken multiple times
            taken = true;

            // if explosion prefab is provide, then instantiate it
            if (explosion)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            // do the player collect coin thing
            other.gameObject.GetComponent<CharacterController2D>().CollectCoin(coinValue);

            // destroy the coin
            DestroyObject(this.gameObject);
        }
    }

    public void SetJump()
    {
        _animationTime = Time.time + _animationWaiting;
        _isAnimating = true;
        _rigidbody.velocity = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(0.0f, 5.0f));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (_isAnimating)
            {
                if (Time.time >= _animationTime)
                {
                    _isAnimating = false;
                    _rigidbody.velocity = new Vector2(0, 0);
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                }

            }
        }
    }

}
