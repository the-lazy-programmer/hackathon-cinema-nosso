using Platformer2D.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
 
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player"))
        {
            CharacterMovement2D Player = collision.gameObject.GetComponent<CharacterMovement2D>();
            Player.Empurrão();
            Debug.Log("Bateu no player");
        }
        //perde a metade dos pontos 
        //Caso tenha 0, morre// por enquanto apenas reinicia a faze
    }
}
