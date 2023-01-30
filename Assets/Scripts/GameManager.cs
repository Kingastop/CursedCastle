using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistentManager
{
    public static GameManager instance;
    public int totalpoints;
    private int record;

    public KeyCode jump { get; set; }
    public KeyCode right { get; set; }
    public KeyCode left { get; set; }
    public KeyCode attack { get; set; }
    public KeyCode crouch { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }



        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backKey", "S"));
    }

    public void LoadData(GameData gameData)
    {
        this.record = gameData.HighestScore;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.HighestScore = this.record;
    }

    public int Points(int reward)
    {
        return totalpoints += reward;
    }

    public void SetRecord()
    {
        if(totalpoints > record)
        {
            record = totalpoints;
        }
    }

    public int GetRecord()
    {
        return record;
    }

}
