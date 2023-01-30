using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private void Update()
    {
        if (Player != null)
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, -10);
    }
}
