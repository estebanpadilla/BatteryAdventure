﻿using UnityEngine;

public class Gameboy : MonoBehaviour
{

    [Range(0f, 10f)]
    public float moveSpeed = 4f;  // enemy move speed when moving
    public int damageAmount = 10; // probably deal a lot of damage to kill player immediately

    // public string stunnedLayer = "StunnedEnemy";  // name of the layer to put enemy on when stunned
    public string playerLayer = "Player";  // name of the layer to put enemy on when stunned

    public GameObject[] myWaypoints; // to define the movement waypoints

    [Tooltip("How much time in second to way at each waypoint")]
    public float waitAtWaypointTime = 1f;   // how long to wait at a waypoint

    public bool loopWaypoints = true; // should it loop through the waypoints

    public bool taken = false;
    public GameObject explosion;

    // store references to components on the gameObject
    Transform _transform;
    Rigidbody2D _rigidbody;
    Animator _animator;
    AudioSource _audio;

    // movement tracking
    [SerializeField]
    int _myWaypointIndex = 0; // used as index for My_Waypoints
    float _moveTime;
    float _vx = 0f;
    bool _moving = true;

    void Awake()
    {
        // get a reference to the components we are going to be changing and store a reference for efficiency purposes
        _transform = GetComponent<Transform>();

        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null) // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component missing from this gameobject");

        _animator = GetComponent<Animator>();
        if (_animator == null) // if Animator is missing
            Debug.LogError("Animator component missing from this gameobject");

        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        { // if AudioSource is missing
            Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
            // let's just add the AudioSource component dynamically
            _audio = gameObject.AddComponent<AudioSource>();
        }

        // setup moving defaults
        _moveTime = 0f;
        _moving = true;
    }

    // if not stunned then move the enemy when time is > _moveTime
    void Update()
    {

        if (Time.time >= _moveTime)
        {
            Movement();
        }
        else
        {
            _animator.SetBool("Moving", false);
        }

    }

    // Move the enemy through its rigidbody based on its waypoints
    void Movement()
    {
        // if there isn't anything in My_Waypoints
        if ((myWaypoints.Length != 0) && (_moving))
        {

            // make sure the enemy is facing the waypoint (based on previous movement)
            Flip(_vx);

            // determine distance between waypoint and enemy
            _vx = myWaypoints[_myWaypointIndex].transform.position.x - _transform.position.x;

            // if the enemy is close enough to waypoint, make it's new target the next waypoint
            if (Mathf.Abs(_vx) <= 0.05f)
            {
                // At waypoint so stop moving
                _rigidbody.velocity = new Vector2(0, 0);

                // increment to next index in array
                _myWaypointIndex++;

                // reset waypoint back to 0 for looping
                if (_myWaypointIndex >= myWaypoints.Length)
                {
                    if (loopWaypoints)
                        _myWaypointIndex = 0;
                    else
                        _moving = false;
                }

                // setup wait time at current waypoint
                _moveTime = Time.time + waitAtWaypointTime;
            }
            else
            {
                // enemy is moving
                _animator.SetBool("Moving", true);

                // Set the enemy's velocity to moveSpeed in the x direction.
                _rigidbody.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rigidbody.velocity.y);
            }

        }
    }

    // flip the enemy to face torward the direction he is moving in
    void Flip(float _vx)
    {

        // get the current scale
        Vector3 localScale = _transform.localScale;

        if ((_vx > 0f) && (localScale.x < 0f))
            localScale.x *= -1;
        else if ((_vx < 0f) && (localScale.x > 0f))
            localScale.x *= -1;

        // update the scale
        _transform.localScale = localScale;
    }

    // Attack player
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

            // do the player victory thing
            other.gameObject.GetComponent<CharacterController2D>().Victory();
        }
    }

    // if the Enemy collides with a MovingPlatform, then make it a child of that platform
    // so it will go for a ride on the MovingPlatform
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = other.transform;
        }
    }

    // if the enemy exits a collision with a moving platform, then unchild it
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }

    // play sound through the audiosource on the gameobject
    void playSound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
}
