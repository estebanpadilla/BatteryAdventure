using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public bool taken;
    GameManager _gm;
    // Use this for initialization
    void Awake()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player") && (!taken))
        {
            // mark as taken so doesn't get taken multiple times
            taken = true;
            _gm.ActivateCheckPoint(gameObject.transform.position);
            other.GetComponent<CharacterController2D>().PlayCheckPoint();
            // destroy the coin
            DestroyObject(this.gameObject);
        }
    }



}
