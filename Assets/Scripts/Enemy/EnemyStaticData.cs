using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Enemy Static Data", menuName = "Static Data/Create Enemy Static Data", order = 55)]

    public class EnemyStaticData : ScriptableObject
    {
        public GameObject[] enemies;

    }
}
