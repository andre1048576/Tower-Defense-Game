using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : MonoBehaviour
{
    public TileOwnership tOwnership;
    public GlobalVariable globalVariable;
    void Update()
    {
        //when they click in the area of the map
        if (Input.GetMouseButtonDown(0)) {
            float[] fCoord = DetCoord();
            if (fCoord[0] < 0 || fCoord[1] < 0) return;
            if (fCoord[0] >= 15 || fCoord[1] >= 15) return;
            int[] intCoords = { Mathf.FloorToInt(fCoord[0]), 14 - Mathf.FloorToInt(fCoord[1]) };

            string gamestate = globalVariable.GetGameState();
            //add a path in the editor
            if (gamestate == "editor")
            {
                tOwnership.ChangeTile(intCoords[0], intCoords[1], "path");
            }
            else if (gamestate == "match")
            {
                string TileValue = tOwnership.CheckTileValue(intCoords[0], intCoords[1]);
                if (TileValue == "path") return;
                
                if (EventSystem.current.IsPointerOverGameObject()) return;
                //if there's not a tower
                if (TileValue == "grass")
                {
                    tOwnership.PlaceTower(intCoords[0], intCoords[1], globalVariable.GetPlacingTower());
                }
                //if there's a tower
                else
                {
                    globalVariable.ShowUpgradeScreen(intCoords[0],intCoords[1]);
                }
                globalVariable.SetPlacingTower("");
            }
        }
        //if they hover over the map while a tower is selected, display its range
    }

    float[] DetCoord()
    {
        Vector3 Vpos = Input.mousePosition;
        float[] pos = { (Vpos.x / Screen.width - (4f / 19f))*19, 15-(Vpos.y / Screen.height)*15 };
        return pos;
    }
}
