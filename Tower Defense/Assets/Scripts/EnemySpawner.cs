using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public TileOwnership tOwnership;
    public GameObject[] Enemies;
    public GlobalVariable globalVariable;
    //2020: this comment is only useful to someone who doesn't know unity
    // Start is called before the first frame update
    public void SpawnBaddie(string EnemyName = "lacking",float dist = 0f,GameObject DeathSource = null)
    {
        GameObject Enemy = null;
        switch (EnemyName)
        {
            case "red":
                Enemy = Instantiate(Enemies[1]);
                break;
            case "white":
                Enemy = Instantiate(Enemies[0]);
                break;
            case "blue":
                Enemy = Instantiate(Enemies[2]);
                break;
            case "yellow":
                Enemy = Instantiate(Enemies[3]);
                break;
            case "black":
                Enemy = Instantiate(Enemies[4]);
                break;
            case "green":
                Enemy = Instantiate(Enemies[5]);
                break;
        }
        if (Enemy == null) return;
        Enemy.transform.position = new Vector3(-0.5f, 0.1f, 7.5f);
        Enemy.GetComponent<MovementScript>().tOwnership = tOwnership;
        //distance is used in order to spawn the children immediately at the death location
        if (dist < 0) dist = 0;
        Enemy.GetComponent<MovementScript>().Distancetocover = dist;
        Enemy.GetComponent<MovementScript>().globalVariable = globalVariable;
        Enemy.GetComponent<HealthScript>().EnemSpawner = this;
        Enemy.GetComponent<HealthScript>().globalVariable = globalVariable;
        Enemy.GetComponent<HealthScript>().DeathSource = DeathSource;
        globalVariable.numofenem++;
    }
}
