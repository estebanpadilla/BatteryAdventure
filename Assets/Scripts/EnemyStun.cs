using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour
{

    // if Player hits the stun point of the enemy, then call Stunned on the enemy
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // tell the enemy to be stunned
            this.GetComponentInParent<Enemy>().Stunned();

            //Make the enemy bounce over the players head
            //other.gameObject.GetComponent<CharacterController2D>().EnemyBounce();
            other.gameObject.GetComponent<CharacterController2D>().EnableTrippleJump();
        }
    }
}
