using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFly : MonoBehaviour
{
    [SerializeField] GameObject flyPrefab;
    [SerializeField] float spawnRate;
    [SerializeField] float flySpeed;
    [SerializeField] bool flyRight;

    void Start() {
        InvokeRepeating("Spawn", 0, spawnRate);
    }

    void Spawn () {
        GameObject flyInstance = Instantiate(flyPrefab, transform.position, Quaternion.identity);
        flyInstance.GetComponent<Fly>().speed = flySpeed;
        if (flyRight == true) {
            flyInstance.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
