using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] public float speed;

    Rigidbody2D flyRB;
    Animator flyAnim;

    void Start() {
        flyRB = GetComponent<Rigidbody2D>();
        flyAnim = GetComponent<Animator>();
    }

    void Update() {
        flyRB.velocity = new Vector2(speed * transform.localScale.x *-1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Player") {
            flyAnim.SetTrigger("Die");
            flyRB.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            StartCoroutine(Die());
        }
    }

    IEnumerator Die() {
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(this.gameObject);
    }
}
