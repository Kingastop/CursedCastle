using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Static Data", menuName = "Static Data/Create Level Static Data", order = 55)]

public class LevelStaticData : ScriptableObject
{
    public MapStaticData[] maps;
}
