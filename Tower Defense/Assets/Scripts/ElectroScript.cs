using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroScript : MonoBehaviour
{
    public float interval;
    public float duration; 
    public float damage;
    float lastattacked = 0;
    private List<GameObject> EnemiesInRange = new List<GameObject>();
    void Update()
    {
        if (Time.time - lastattacked >= interval)
        {
            lastattacked = Time.time;
            AttackAll();
        }
        duration -= Time.deltaTime;
        if (duration < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy") return;
        if (EnemiesInRange.Contains(other.gameObject)) return;
        EnemiesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (EnemiesInRange.Contains(other.gameObject))
        {
            EnemiesInRange.Remove(other.gameObject);
        }
    }

    private void AttackAll()
    {
        for (int i =0;i<EnemiesInRange.Count;i++)
        {
            if (EnemiesInRange[i])
                //2020: what was the point of this comment
                //add visual to it
                EnemiesInRange[i].GetComponent<HealthScript>().TakeDamage(damage,EnemiesInRange[i]);
            else
            {
                EnemiesInRange.RemoveAt(i);
                i--;
            }
        }
    }
}
