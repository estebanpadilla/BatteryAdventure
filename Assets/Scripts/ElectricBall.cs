using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    public float speed = 5;
    int _direction = 0;
    bool _isMoving = true;

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving)
        {
            return;
        }

        if (_direction == 2)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        else if (_direction == 1)
        {
            transform.Translate(-Vector3.left * Time.deltaTime * speed);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player"))
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
            if (player.playerCanMove && !player.isElectric && !player.isGhosted)
            {
                // stop moving
                _isMoving = false;
                gameObject.SetActive(false);

                // apply damage to the player
                player.ApplyElectri();

            }
        }

    }

    public void BallDirection(int direction)
    {
        _direction = direction;
    }
}
