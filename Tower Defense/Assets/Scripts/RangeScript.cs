using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{
    public int Radius = 2;
    public float Reload = 0.5f;
    float LastShotTime = 0f;
    public GameObject bulletObj;
    public int numgrades = 0;
    public float bonusDmg = 0;
    public float bonusDuration = 1f; 
    public int bonusSpd = 1; 
    public float bonusShotSize = 0; 
    public int bonusPierce = 0;

    private void Start()
    {
        bonusDuration = 1f;
        bonusSpd = 1;
    }
    
    //2020: i am so grateful to have learnt OOP i can't bear this code anymore
    //every tower uses this script to attack
    void Attack(GameObject Enemy)
    {
        GameObject bullet = Instantiate(bulletObj, transform.position, transform.rotation);
        BulletScript bltscript = bullet.GetComponent<BulletScript>();
        if (bltscript != null)
        {
            bltscript.damage += bonusDmg;
            bltscript.speed *= bonusSpd;
            bltscript.duration *= bonusDuration;
            bltscript.pierce += bonusPierce;
            bullet.transform.localScale += new Vector3(bonusShotSize,0,0);
            bltscript.speed += bonusSpd;
        } else
        {
            ElectroScript electroScript = bullet.GetComponent<ElectroScript>();
            electroScript.duration *= bonusDuration;
            bullet.GetComponent<BoxCollider>().size += new Vector3(bonusShotSize, 0, bonusShotSize);
        }
        bullet.transform.position += transform.forward * 1;
    }

    void Update()
    {
        Collider[] ColliersInRange = Physics.OverlapBox(transform.position, new Vector3(Radius, 2, Radius));
        GameObject Enemy = null;
        GameObject tempEnemy;
        float EnemDist = 0;
        if (Time.time - LastShotTime < Reload) return;
        for (int i = 0;i < ColliersInRange.Length;i++)
        {

            //finds the enemy furthest ahead
            tempEnemy = ColliersInRange[i].gameObject;
            if (tempEnemy.tag == "Enemy")
            {
                if (tempEnemy.GetComponent<MovementScript>().Distance > EnemDist)
                {
                    Enemy = tempEnemy;
                    EnemDist = tempEnemy.GetComponent<MovementScript>().Distance;
                }
            }
        }
        //if there's an enemy damage it
        if (Enemy != null)
        {
            LastShotTime = Time.time;
            gameObject.transform.LookAt(Enemy.transform);
            Attack(Enemy);
        }
    }
}
