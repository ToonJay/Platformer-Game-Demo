using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    PolygonCollider2D playerBody;

    [SerializeField] int coinValue = 1;
    void Start() {
        playerBody = FindObjectOfType<PlayerController>().GetComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision == playerBody) {
            Destroy(this.gameObject);
            if (this.gameObject.tag == "Coin") {
                FindObjectOfType<GameSession>().CoinPickup(coinValue);
            }
        }
    }
}
