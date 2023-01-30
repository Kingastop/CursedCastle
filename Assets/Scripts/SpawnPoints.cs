using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnPoints: MonoBehaviour
{
    [SerializeField] public Transform[] transformPoints;
    [SerializeField] public Transform BeginPoint;
}
