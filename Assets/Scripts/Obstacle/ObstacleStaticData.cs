using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Static Data", menuName = "Static Data/Obstacle Level Static Data", order = 55)]

public class ObstacleStaticData : ScriptableObject
{
    public GameObject[] obstacles;
}
