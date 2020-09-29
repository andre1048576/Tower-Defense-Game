using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    public Button Editor2Menu;
    public TileOwnership tOwnership;
    public GlobalVariable globalVariable;
    public WaveScript WaveSpawn;
    public GameObject MoneyDisplay;
    public GameObject LivesDisplay;
    public GameObject FinTexte;
    public GameObject FinEcran;
    int WaveCount = 1;

    public void LeaveEditor()
    {
        if (!tOwnership.CheckPathValid()) return;
        globalVariable.ChangeGameState("menu");
    }

    public void EnterEditor()
    {
        globalVariable.ChangeGameState("editor");
    }

    public void StartMatch()
    {
        globalVariable.ChangeGameState("match");
        //sets the money to 300
        globalVariable.ChangeMoney(-globalVariable.GetMoney()+600);
    }

    //when the next round starts
    public void SpawnWave()
    {
        if (WaveSpawn.spawningwave || globalVariable.numofenem > 0) return;
        StartCoroutine(WaveSpawn.SpawnWave(WaveCount));
        globalVariable.ChangeMoney(40);
        WaveCount++;
    }

    public void ChangeMoneyDisplay(int money)
    {
        MoneyDisplay.GetComponent<Text>().text = "Cash : " + money;
    }
    public void ChangeLivesDisplay(int lives)
    {
        LivesDisplay.GetComponent<Text>().text = "Lives : " + lives;
    }
    public void AddUpgrade()
    {
        globalVariable.UpgradeTower();
    }

    //loads the final screen
    public void ChangerEcranFinal(bool agagne)
    {
        FinEcran.SetActive(true);
        if (!agagne) FinTexte.GetComponent<Text>().text = "you lost!";
        else FinTexte.GetComponent<Text>().text = "You won!";
    }

    public void RecommenceJeu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
