using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int pierce;
    public float speed;
    public float duration;
    public float damage;
    //define area in the collider
    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //add the enemy to a list
        //make sure collided with enemy
        if (pierce <= 0) return;
        if (other.gameObject.tag != "Enemy") return;
        other.gameObject.GetComponent<HealthScript>().TakeDamage(damage,gameObject);
        pierce--;
        if (pierce <= 0) Destroy(gameObject);
    }
}
