using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float Speed = 1f; //units per second
    public float Distance = 0;
    public GlobalVariable globalVariable;
    private Vector3 LastPosition;
    public TileOwnership tOwnership;
    Vector3 Direction;
    public float Distancetocover;
    private void Start()
    {
        LastPosition = transform.position;
    }
    //2020: this block of code is actually really impressive 
    //2020: also half the comments here are redundant
    private void Update()
    {
        Distancetocover += Speed * Time.deltaTime;
        //moves by a complete tile
        while (Distancetocover >= 1)
        {
            Move(1-(Distance%1));
        }
        //if it would swap tiles move the enemy to the end of the current tile
        if (Distancetocover > (Mathf.Ceil(Distance)-Distance))
        {
            Move(Mathf.Ceil(Distance) - Distance);
        }
        //moves the enemy
        Move(Distancetocover);
    }
    private Vector3 CalculateDirection(int index)
    {
        int iDirection = tOwnership.GetDirection(index);
        switch (iDirection)
        {
            case 3:
                return Vector3.right;
            case 6:
                return Vector3.back;
            case 9:
                return Vector3.left;
            case 12:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }

    }
    private void Move(float amount)
    {
        Direction = CalculateDirection(Mathf.FloorToInt(Distance));
        if (Direction == Vector3.zero)
        {
            this.gameObject.GetComponent<HealthScript>().OnDeath(null);
            globalVariable.ChangeLives(-1);
        }
        transform.position += Direction * amount;
        Distance += amount;
        Distancetocover -= amount;
    }
}
