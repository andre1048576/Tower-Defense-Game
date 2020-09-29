using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TileOwnership : MonoBehaviour
{
    private string[,] WorldTiles = new string[15,15];
    private GameObject[,] GfxTiles = new GameObject[15, 15];
    private List<int> Directions = new List<int>();
    public GameObject DefaultTile;
    private Dictionary<string,GameObject> TowerObjects = new Dictionary<string, GameObject>();
    public List<string> Keys;
    public List<GameObject> Towers;
    public List<int> Costs;
    public GlobalVariable globalVariable;
    private Dictionary<string, int> PathOptionDirections = new Dictionary<string, int>();
    private Dictionary<string, int> TowerCosts = new Dictionary<string, int>();
    void Start()
    {
        for (int i = 0;i < Towers.Count;i++)
        {
            TowerObjects.Add(Keys[i], Towers[i]);
            TowerCosts.Add(Keys[i], Costs[i]);
        }
        PathOptionDirections.Add("pathoption3", 3);
        PathOptionDirections.Add("pathoption6", 6);
        PathOptionDirections.Add("pathoption9", 9);
        PathOptionDirections.Add("pathoption12", 12);
        for (int x = 0;x<15;x++)
        {
            for (int y = 0; y < 15; y++)
            {
                AddTileGfx(x, y);
                
            }
        }
        ResetBoard();
    }

    public void ResetBoard()
    {
        for (int x = 0; x < 15; x++)
        {
            for (int y = 0; y < 15; y++)
            {
                ChangeTile(x, y, "grassoverride");
            }
        }
        Directions.Clear();
        Directions.Add(3);
        ChangeTile(0, 7, "pathoption");
        ChangeTile(14,7, "pathend");
    }
    //directions are based on a clock
    public void ChangeTile(int x, int y, string tiletype)
    {

        GameObject tilechanged = GfxTiles[x, y];
        Material m_aterial = tilechanged.GetComponent<Renderer>().material;
        if (tiletype == "grass")
        {
            if (CheckTileValue(x, y) != "path" && CheckTileValue(x, y) != "pathend")
            {
                m_aterial.color = Color.green;
                WorldTiles[x, y] = tiletype;
            }
        }
        //erases the tile
        else if (tiletype == "grassoverride")
        {
            m_aterial.color = Color.green;
            WorldTiles[x, y] = "grass";
        }
        //add a path if it's a choice
        else if (tiletype == "path")
        {
            if (!WorldTiles[x, y].Contains("pathoption")) return;
            m_aterial.color = Color.gray;
            if (WorldTiles[x, y] !="pathoption") Directions.Add(PathOptionDirections[CheckTileValue(x,y)]);
            WorldTiles[x, y] = tiletype;
            
            //add the future path options
            if (x > 0) ChangeTile(x - 1, y, "pathoption9");
            if (y > 0) ChangeTile(x, y - 1, "pathoption6");
            if (x < 14) ChangeTile(x + 1, y, "pathoption3");
            if (y < 14) ChangeTile(x, y + 1, "pathoption12");
            //removes all of the out of bounds path options
            if (x == 14 && y == 7)
            {
                Directions.Add(3);
                ClearOther(0, 0);
            }
            else ClearOther(x, y);
        }
        //used in the editor
        else if (tiletype.Contains("pathoption"))
        {
            if (WorldTiles[x, y] == "path") return;
            WorldTiles[x, y] = tiletype;
            m_aterial.color = Color.red;
        }
        //used in the editor
        else if (tiletype == "pathend")
        {
            if (WorldTiles[x, y] == "path") return;
            WorldTiles[x, y] = tiletype;
            m_aterial.color = Color.white;
        }
    }

    //removes all of the paths not used in the editor
    void ClearOther(int x,int y)
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0;j < 15;j++)
            {
                if (ManhattanDistance(x,i,y,j) > 1)
                {
                    if (i == 14 && j == 7) ChangeTile(i, j, "pathend");
                    else ChangeTile(i,j,"grass");
                }
            }
        }
    }
    //2020 : i added a comment telling a programmer to google something :neutral_face:
    //wikipedia this bad boy
    int ManhattanDistance(int x1,int x2,int y1,int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    void AddTileGfx(int x, int y)
    {
        GameObject addedtile = Instantiate(DefaultTile);
        addedtile.transform.position = new Vector3(0.5f + x,0.1f, 0.5f + y);
        GfxTiles[x, y] = addedtile;
    }

    public bool CheckPathValid()
    {
        return (WorldTiles[14, 7] == "path");
    }

    public string CheckTileValue(int x,int y)
    {
        return WorldTiles[x, y];
    }

    public int GetDirection(int index)
    {
        //if the index is outside of the list size
        if (index >= Directions.Count) return 0;
        return Directions[index];
    }

    public void PlaceTower(int x,int y,string Tower)
    {
        if (Tower == "") return;
        //if they have enough money
        if (TowerCosts[Tower] > globalVariable.GetMoney()) return;
        GameObject tower = Instantiate(TowerObjects[Tower], GfxTiles[x, y].transform,false);
        globalVariable.ChangeMoney(-TowerCosts[Tower]);
        WorldTiles[x, y] = Tower;
    }

    public GameObject GetTower(int x,int y)
    {
        //if the tower doesn't exist
        if (GfxTiles[x, y].transform.childCount < 1) return null;
        return GfxTiles[x, y].transform.GetChild(0).gameObject;
    }
}
