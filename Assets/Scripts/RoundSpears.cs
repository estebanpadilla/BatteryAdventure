using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSpears : MonoBehaviour
{

    [Range(1.0f, 10.0f)]
    public float speed = 5;
    public AudioClip attackSFX;
    public int damageAmount = 10; // probably deal a lot of damage to kill player immediately
    AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        { // if AudioSource is missing
            Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
            // let's just add the AudioSource component dynamically
            _audio = gameObject.AddComponent<AudioSource>();
        }

    }


    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.RotateAroundLocal(Vector3.forward, speed * Time.deltaTime);
    }

    // Attack player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player"))
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();

            // attack sound
            playSound(attackSFX);

            // apply damage to the player
            player.ApplyDamage(damageAmount);

        }
    }
    // play sound through the audiosource on the gameobject
    void playSound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }


}
