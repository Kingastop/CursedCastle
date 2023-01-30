using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistentManager> dataPersistentObjects;

    private FileDataHandler fileDataHandler;

    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        Instance = this;
    }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistentObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();

        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistentManager dataPersistentObj in dataPersistentObjects)
        {
            dataPersistentObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (IDataPersistentManager dataPersistentObj in dataPersistentObjects)
        {
            dataPersistentObj.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistentManager> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistentManager> dataPersistentObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistentManager>();

        return new List<IDataPersistentManager>(dataPersistentObjects);
    }

    public int GetData() =>
        gameData.HighestScore;
}
