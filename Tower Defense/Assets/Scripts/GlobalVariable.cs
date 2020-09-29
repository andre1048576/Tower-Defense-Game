using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//i left the code intact for archival purposes
//however comments have been added in 2020
//they will start with "2020:"


//2020: past me had no idea what how to OOP.
public class GlobalVariable : MonoBehaviour
{
    string GameState = "editor";
    int Money = 200;
    public int Lives = 20;
    public GameObject MenuScreen;
    public GameObject EditorScreen;
    public GameObject TowerSelectScreen;
    public GameObject UpgradeScreen;
    string PlacingTower = "";
    public UiManager UiManagerObj;
    public TileOwnership tOwnership;
    public GameObject UpgradeText;
    public int numofenem;
    GameObject TowerUpgrading;
    public void Start()
    {
        ChangeMoney(0);
        ChangeLives(0);
    }
    public void ChangeGameState(string value)
    {
        GameState = value;
        if (GameState == "editor")
        {
            MenuScreen.SetActive(false);
            EditorScreen.SetActive(true);
            TowerSelectScreen.SetActive(false);
        }
        else if (GameState == "menu")
        {
            MenuScreen.SetActive(true);
            EditorScreen.SetActive(false);
            TowerSelectScreen.SetActive(false);
        }
        else if (GameState == "match")
        {
            MenuScreen.SetActive(false);
            EditorScreen.SetActive(false);
            TowerSelectScreen.SetActive(true);
        }
    }
    //2020: atleast i had getters and setters (half the time)
    public string GetGameState()
    {
        return GameState;
    }
    
    public void SetPlacingTower(string value)
    {
        PlacingTower = value;
    }
    public string GetPlacingTower()
    {
        return PlacingTower;
    }

    public int GetMoney()
    {
        return Money;
    }

    public void ChangeMoney(int variance)
    {
        Money += variance;
        UiManagerObj.ChangeMoneyDisplay(Money);
    }

    //UI and player input in one
    public void ShowUpgradeScreen(int x,int y)
    {
        TowerUpgrading = tOwnership.GetTower(x, y);
        if (TowerUpgrading == null) return;
        UpgradeScreen.SetActive(true);
        if (x < 3) x = 3;
        if (x > 12) x = 11;
        x -= 4;
        if (y > 10) y -= 4;
        UpgradeScreen.transform.position = new Vector3(x, 4, y);
        WriteUpgradeScreen();
    }

    public void CloseUpgradeScreen()
    {
        UpgradeScreen.SetActive(false);
    }

    public void WriteUpgradeScreen()
    {
        int Numgrades = TowerUpgrading.GetComponent<RangeScript>().numgrades;
        string text = "";
        switch (TowerUpgrading.tag)
        {
            case "BasicTower":
                text = AvailableTextPerTower(Numgrades, "The tower does 1 more damage Cost: 150", "The tower shoots farther Cost: 100", "The projectiles pierce two enemies Cost: 500");
                break;
            case "CannonTower":
                text = AvailableTextPerTower(Numgrades, "The attack lasts 50% longer Cost: 200", "The plasma ball is larger Cost: 400", "The tower attacks twice as fast Cost: 800");
                break;
            case "SharpTower":
                text = AvailableTextPerTower(Numgrades, "The shots are faster Cost: 150", "The shots are larger Cost: 200", "The shots deal double damage Cost: 600");
                break;
        }
        UpgradeText.GetComponent<UnityEngine.UI.Text>().text = text;
    }

    //determines the upgrade text of the tower
    public string AvailableTextPerTower(int numgrades,string option1,string option2,string option3,string deflt = "Max Upgrades")
    {
        switch (numgrades)
        {
            case 0: return option1;
            case 1: return option2;
            case 2: return option3;
            default: return deflt;
        }
    }

    //determines the cost of an upgrade
    public int CostPerUpgrade(int numgrades,int Cost1, int Cost2, int Cost3)
    {
        switch(numgrades)
        {
            case 0: return Cost1;
            case 1: return Cost2;
            case 2: return Cost3;
            default: return 0;
        }
    }
    //confirms that money requirements are met for the towers
    public void UpgradeTower()
    {
        int Numgrades = TowerUpgrading.GetComponent<RangeScript>().numgrades;
        int Cost = -1;
        switch (TowerUpgrading.tag)
        {
            case "BasicTower":
                Cost = CostPerUpgrade(Numgrades, 150, 100, 500);
                break;
            case "CannonTower":
                Cost = CostPerUpgrade(Numgrades, 200, 400, 800);
                break;
            case "SharpTower":
                Cost = CostPerUpgrade(Numgrades, 150, 200, 600);
                break;
        }
        if (Money < Cost || Cost < 0) return;
        TowerUpgrading.GetComponent<RangeScript>().numgrades++;
        Renderer renderer = TowerUpgrading.GetComponent<Renderer>();
        //2020: why did i pick these colors
        switch (Numgrades) {
            case 0: renderer.material.SetColor("_Color", Color.red); break;
            case 1: renderer.material.SetColor("_Color", Color.magenta); break;
            case 2: renderer.material.SetColor("_Color", Color.black); break;
        }
        ChangeMoney(-Cost);
        ApplyUpgrade(TowerUpgrading.GetComponent<RangeScript>().numgrades);
        WriteUpgradeScreen();
    }

    //allows you to upgrade towers
    public void ApplyUpgrade(int Numgrades)
    {
        RangeScript TowerScript = TowerUpgrading.GetComponent<RangeScript>();
        switch (TowerUpgrading.tag)
        {
            case "BasicTower":
                switch (Numgrades)
                {
                    case 1:
                        TowerScript.bonusDmg++;
                        break;
                    case 2:
                        TowerScript.Radius++;
                        break;
                    case 3:
                        TowerScript.bonusPierce += 2;
                        break;
                }
                break;
            case "CannonTower":
                switch (Numgrades)
                {
                    case 1:
                        TowerScript.bonusDuration += 0.5f;
                        break;
                    case 2:
                        TowerScript.bonusShotSize += 1f;
                        break;
                    case 3:
                        TowerScript.Reload /=2;
                        break;
                }
                break;
            case "SharpTower":
                switch (Numgrades)
                {
                    case 1:
                        TowerScript.bonusSpd+= 4;
                        break;
                    case 2:
                        TowerScript.bonusShotSize += 0.5f;
                        break;
                    case 3:
                        TowerScript.bonusDmg *= 2;
                        break;
                }
                break;
        }
    }

    //this loads the end screen to display whether the player has won
    public void EndMatch(bool agagne)
    {
        ChangeGameState("match");
        TowerSelectScreen.SetActive(false);
        Time.timeScale = 0;
        Money = 200;
        Lives = 20;
        UiManagerObj.ChangerEcranFinal(agagne);
        PlacingTower = "";
    }

    //changes the amount of lives there are and ends the game if all are lost
    public void ChangeLives(int variance)
    {
        Lives += variance;
        UiManagerObj.ChangeLivesDisplay(Lives);
        if (Lives > 0) return;
        EndMatch(false);
    }
}
