using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    //20 waves in total
    public GlobalVariable globalVariable;
    string[] Enemies = { "white", "red","blue","yellow","black","green"};
    List<float> Wave1 = new List<float>();
    List<float> Wave2 = new List<float>();
    List<float> Wave3 = new List<float>();
    List<float> Wave4 = new List<float>();
    List<float> Wave5 = new List<float>();
    List<float> Wave6 = new List<float>();
    List<float> Wave7 = new List<float>();
    List<float> Wave8 = new List<float>();
    List<float> Wave9 = new List<float>();
    List<float> Wave10 = new List<float>();
    List<float> Wave11 = new List<float>();
    List<float> Wave12 = new List<float>();
    List<float> Wave13 = new List<float>();
    List<float> Wave14 = new List<float>();
    List<float> Wave15 = new List<float>();
    List<float> Wave16 = new List<float>();
    List<float> Wave17 = new List<float>();
    List<float> Wave18 = new List<float>();
    List<float> Wave19 = new List<float>();
    List<float> Wave20 = new List<float>();
    float timeelapsed;
    public bool spawningwave = false;
    public EnemySpawner enemySpawner;

    //The 2 ways to add info to a round
    void ListAdd(List<float> list,float amount,float towertype,float delay,float between = 0f)
    {
        list.Add(amount);
        list.Add(towertype);
        list.Add(delay);
        list.Add(between);
    }

    void AlternatingAdd(List<float> list,float amountalternate,float amountper,float towertype1,float towertype2,float delay,float between = 0f)
    {
        float spawningtype = towertype1;
        for (int i=0;i < amountalternate-1;i++)
        {
            ListAdd(list, amountper, spawningtype, delay);
            if (spawningtype == towertype1) spawningtype = towertype2;
            else spawningtype = towertype1;
        }
        ListAdd(list, amountper, spawningtype, delay, between);
    }

    private void Start()
    {
        //defines all the levels
        ListAdd(Wave1, 10, 0, 0.8f);
        ListAdd(Wave2, 20, 0, 0.75f);
        ListAdd(Wave3, 8, 0, 0.5f,1f);
        ListAdd(Wave3, 3, 1, 1.5f);
        AlternatingAdd(Wave4, 5, 2, 0, 1, 1f);
        ListAdd(Wave5, 7, 0, 0.6f);
        ListAdd(Wave5, 3, 1, 0.7f);
        ListAdd(Wave6, 10, 3, 0.5f);
        AlternatingAdd(Wave7, 4, 5, 0, 3, 0.3f);
        AlternatingAdd(Wave8, 15, 1, 1, 3, 0.4f);
        ListAdd(Wave9, 5, 0, 0.4f,0.6f);
        ListAdd(Wave9, 5, 1, 0.4f, 0.6f);
        ListAdd(Wave9, 5, 3, 0.4f);
        ListAdd(Wave10, 2, 5, 2f);
        ListAdd(Wave11, 1, 5, 0, 1);
        ListAdd(Wave11, 10, 1, 0.4f);
        ListAdd(Wave12, 4, 5, 0.1f, 0.5f);
       
        AlternatingAdd(Wave12, 5, 5, 1, 3, 0.2f);
        AlternatingAdd(Wave13, 5, 2, 5, 1, 0.4f);
        ListAdd(Wave14, 10, 3, 0.4f);
        AlternatingAdd(Wave15, 8, 8, 0, 1, 0.1f);
        ListAdd(Wave16, 1, 4, 0);
        ListAdd(Wave17, 7, 5, 0.2f);
        AlternatingAdd(Wave18, 5, 5, 1, 2, 0.1f,0.1f);
        AlternatingAdd(Wave18, 5, 5, 1, 3, 0.1f, 0.1f);
        AlternatingAdd(Wave19, 20, 1, 5, 1, 0.4f);
        ListAdd(Wave20, 5, 4, 0.5f);
    }
    public IEnumerator SpawnWave(int num)
    {
        if (spawningwave) yield break;
        spawningwave = true;
        List<float> SpawnedWave = DetermineWave(num);
        if (SpawnedWave != null)
        {
            for (int i = 0; i < SpawnedWave.Count; i += 4)
            {
                //adds the enemies to the game
                for (int j = 0; j < SpawnedWave[i]; j++)
                {
                    enemySpawner.SpawnBaddie(Enemies[Mathf.RoundToInt(SpawnedWave[i + 1])]);
                    yield return new WaitForSeconds(SpawnedWave[i + 2]);
                }
                yield return new WaitForSeconds(SpawnedWave[i + 3]);
            }
            spawningwave = false;
        } else
        {
            //ends the game after 20 rounds
            globalVariable.EndMatch(true);
        }
        yield break;
    }

    //2020: why did i do it like this
    List<float> DetermineWave(int num)
    {
        switch (num)
        {
            case 1: return Wave1;
            case 2: return Wave2;
            case 3: return Wave3;
            case 4: return Wave4;
            case 5: return Wave5;
            case 6: return Wave6;
            case 7: return Wave7;
            case 8: return Wave8;
            case 9: return Wave9;
            case 10: return Wave10;
            case 11: return Wave11;
            case 12: return Wave12;
            case 13: return Wave13;
            case 14: return Wave14;
            case 15: return Wave15;
            case 16: return Wave16;
            case 17: return Wave17;
            case 18: return Wave18;
            case 19: return Wave19;
            case 20: return Wave20;
            default: return null;
        };
    }
}
