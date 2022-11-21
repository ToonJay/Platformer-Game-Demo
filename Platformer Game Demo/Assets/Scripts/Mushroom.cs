using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D mushRB;

    void Start()
    {
        mushRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mushRB.velocity = new Vector2(speed * transform.localScale.x, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Player") {
            transform.localScale = new Vector3(-1 * transform.localScale.x, 1f, 1f);
        }
    }
}
