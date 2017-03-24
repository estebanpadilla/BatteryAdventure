using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{

    public bool destroyNonPlayerObjects = true;
    public Transform mainCamaner;

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        gameObject.transform.position = new Vector3(mainCamaner.position.x, gameObject.transform.position.y, 0);
    }

    // Handle gameobjects collider with a deathzone object
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // if player then tell the player to do its FallDeath
            other.gameObject.GetComponent<CharacterController2D>().FallDeath();
        }
        else if (destroyNonPlayerObjects)
        { // not playe so just kill object - could be falling enemy for example
            DestroyObject(other.gameObject);
        }
    }
}
