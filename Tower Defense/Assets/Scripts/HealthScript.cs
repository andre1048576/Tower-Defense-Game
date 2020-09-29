using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float Health = 3f;
    public int CoinWorth = 10;
    public int Bleeding = 0;
    public int Regen = 0;
    public List<string> onDefeat;
    public EnemySpawner EnemSpawner;
    public GlobalVariable globalVariable;
    private float BleedingDelay;
    private float RegenDelay;
    private float MaxHealth;
    public GameObject DeathSource;

    public void TakeDamage(float Damage,GameObject deathSource)
    {
        if (deathSource == DeathSource && deathSource != null) return;
        Health -= Damage;
        if (Health > MaxHealth) Health = MaxHealth;
        if (Health > 0) return;
        OnDeath(deathSource);
    }

    void ResetBleeding()
    {
        switch (Bleeding)
        {
            case 1:
                BleedingDelay = 4;
                break;
            case 2:
                BleedingDelay = 3;
                break;
            default:
                BleedingDelay = 10000;
                break;
        }
    }

    void ResetRegen()
    {
        switch (Regen)
        {
            case 1:
                RegenDelay = 1.5f;
                break;
            case 2:
            case -1:
                RegenDelay = 0.75f;
                break;
            default:
                RegenDelay = 10000;
                break;
        }
    }

    private void Start()
    {
        MaxHealth = Health;
        ResetBleeding();
        ResetRegen();
    }

    void Update()
    {
        BleedingDelay -= Time.deltaTime;
        if (BleedingDelay <= 0)
        {
            Bleed();
            ResetBleeding();
        }
        RegenDelay -= Time.deltaTime;
        if (RegenDelay <= 0)
        {
            Regenerate();
            ResetRegen();
        }
    }

    void Bleed()
    {
        //creates an enemy at the location of the current one
        EnemSpawner.SpawnBaddie(onDefeat[Random.Range(0,onDefeat.Count-1)], gameObject.GetComponent<MovementScript>().Distance );
    }

    void Regenerate()
    {
        if (Regen > 0) TakeDamage(-1, gameObject);
        else TakeDamage(1, gameObject);
    }

    public void OnDeath(GameObject deathSource)
    {
        globalVariable.numofenem--;
        globalVariable.ChangeMoney(CoinWorth);
        for (int i = 0;i < onDefeat.Count;i++)
        {
            //creates other enemies
            EnemSpawner.SpawnBaddie(onDefeat[i], gameObject.GetComponent<MovementScript>().Distance - (i * 0.4f),deathSource);
        }
        Destroy(gameObject, 0);
    }
}
