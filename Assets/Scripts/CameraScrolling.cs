using System;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class CameraScrolling : MonoBehaviour
{
    [SerializeField] Transform destroyTransform;
    [SerializeField] Transform spawnTransform;
    [SerializeField] private LevelStaticData levelStaticData;
    [SerializeField] private EnemyStaticData enemyStaticData;
    [SerializeField] private ObstacleStaticData obstacleStaticData;
    [SerializeField] Transform WhereToSpawn;

    [SerializeField] Canvas canvasSize;

    private Transform MapScale;

    private Vector2 size = new Vector2(0.01f, 0.01f);
    private Vector2 BackgroundSize;

    private int mapNum = 0;

    private float scrollSpeed = 0.2f; //0.2f
    private bool destroyed = true;
    private bool spawned = false;
    Vector3 ScrollingVector;

    private int rangedEnemiesOnLevel = 0;
    private int simpleEnemiesOnLevel = 0;
    private int levelsEnded = 0;

    private GameObject spawnedMap;

    void Start()
    {
        ScrollingVector.Set(scrollSpeed, 0, 0);
        BackgroundSize = new Vector2(canvasSize.renderingDisplaySize.x / 480.0f, 1.82f);
        this.gameObject.transform.localScale = new Vector3(BackgroundSize.x, BackgroundSize.y, 1);
        MapScale = Instantiate(levelStaticData.maps[0].Map, new Vector3(0, 0, 0), new Quaternion()).transform;
        MapScale.localScale = new Vector3(this.gameObject.transform.localScale.x / 1.74f, 1, 1);
    }


    void Update()
    {
        gameObject.transform.position += ScrollingVector * Time.deltaTime;
    }

    private void FixedUpdate()
    {

        if (destroyed)
        {
            Collider2D[] spawnCollider = Physics2D.OverlapBoxAll(spawnTransform.position, size, LayerMask.GetMask("Map"));
            for (int i = 0; i < spawnCollider.Length; i++)
            {
                if (spawnCollider[i] != null)
                {
                    SpawnMap(WhereToSpawn.position);
                    spawned = true;
                    destroyed = false;
                }
            }
        }

        if (spawned)
        {
            Collider2D[] destroyCollider = Physics2D.OverlapBoxAll(destroyTransform.position, size, LayerMask.GetMask("Map"));
            for (int i = 0; i < destroyCollider.Length; i++)
            {
                if (destroyCollider[i] != null)
                {
                    MapScale = spawnedMap.transform;
                    MapScale.localScale = new Vector3(this.gameObject.transform.localScale.x / 1.74f, 1, 1);
                    Destroy(destroyCollider[i].GetComponentInParent<Grid>().gameObject);
                    destroyed = true;
                    spawned = false;
                }
            }
        }
    }

    private void SpawnMap(Vector3 position)
    {
        if (mapNum == 0)
        {
            mapNum = UnityEngine.Random.Range(1, levelStaticData.maps.Length);
            
        }
        else
        {
            GameManager.instance.Points(1000);
            mapNum = 0;
            levelsEnded++;
            if ((levelsEnded + 1) % 3 == 0)
                rangedEnemiesOnLevel++;
            if ((levelsEnded + 1) % 4 == 0)
                simpleEnemiesOnLevel++;
        }

        position = new Vector3(position.x, position.y, 0);

        spawnedMap = Instantiate(levelStaticData.maps[mapNum].Map, position, new Quaternion());
        spawnedMap.transform.SetParent(null, false);
        spawnedMap.transform.localScale = MapScale.localScale;
        spawnedMap.transform.position = new Vector3(position.x + spawnedMap.transform.position.x - spawnedMap.GetComponent<SpawnPoints>().BeginPoint.position.x, spawnedMap.transform.position.y, 0);

        if(mapNum != 0)
            RandomSpawnIterator(spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints.Length, rangedEnemiesOnLevel, simpleEnemiesOnLevel);

    }

    private void RandomSpawnIterator(int Count, int rangedEnemies, int simpleEnemies)
    {
        int i = 0, j = 0;

        if (Count - 2 <= rangedEnemies + simpleEnemies)
        {
            int[] numbers = new int[Count - 2];
            int[] obst = new int[2];
            

            for (i = 0; i < Count; i++)
            {
                if (numbers.Length > i)
                {
                    numbers[i] = UnityEngine.Random.Range(0, Count);

                    for (j = i - 1; j >= 0; j--)
                    {
                        if (numbers[j] == numbers[i])
                        {
                            numbers[i] = UnityEngine.Random.Range(0, Count);
                            j = i - 1;
                        }
                    }
                }
                else
                {
                    obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);

                    for (j = i - 1; j >= 0; j--)
                    {
                        if (j >= numbers.Length && obst[i - numbers.Length] == obst[j - numbers.Length])
                        {
                            obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);
                            j = i;
                            continue;
                        }
                        if (j < numbers.Length && numbers[j] == obst[i - numbers.Length])
                        {
                            obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);
                            j = i;
                            continue;
                        }
                    }
                }


            }

            for (i = 0; i < Count;)
            {
                if (rangedEnemies > 0 && UnityEngine.Random.Range(0, enemyStaticData.enemies.Length) == 1 && numbers.Length > i)
                {

                    Instantiate(enemyStaticData.enemies[1], spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[numbers[i]]);
                    rangedEnemies--;
                    i++;
                }
                else if (simpleEnemies > 0 && UnityEngine.Random.Range(0, enemyStaticData.enemies.Length) == 0 && numbers.Length > i)
                {
                    Instantiate(enemyStaticData.enemies[0], spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[numbers[i]]);
                    simpleEnemies--;
                    i++;
                }
                else if (numbers.Length <= i)
                {
                    Instantiate(obstacleStaticData.obstacles[UnityEngine.Random.Range(0, 2)], spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[obst[i - numbers.Length]]);
                    i++;
                }

            }
            Array.Clear(numbers, 0, numbers.Length);
            Array.Clear(obst, 0, obst.Length);
        }
        else
        {
            int[] numbers = new int[rangedEnemies + simpleEnemies];
            int[] obst = new int[UnityEngine.Random.Range(0,Count - numbers.Length)];


            for (i = 0; i < numbers.Length + obst.Length; i++)
            {
                if (numbers.Length > i)
                {
                    numbers[i] = UnityEngine.Random.Range(0, Count);

                    for (j = i - 1; j >= 0; j--)
                    {
                        if (numbers[j] == numbers[i])
                        {
                            numbers[i] = UnityEngine.Random.Range(0, Count);
                            j = i - 1;
                        }
                    }

                }
                else
                {
                    obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);

                    for (j = i - 1; j >= 0; j--)
                    {
                        if(j >= numbers.Length && obst[i - numbers.Length] == obst[j - numbers.Length])
                        {
                            obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);
                            j = i;
                            continue;
                        }
                        if (j < numbers.Length && numbers[j] == obst[i - numbers.Length])
                        {
                            obst[i - numbers.Length] = UnityEngine.Random.Range(0, Count);
                            j = i;
                            continue;
                        }
                    }

                }

                
            }
            
            

            for (i = 0; i < numbers.Length + obst.Length;)
            {
                if (rangedEnemies > 0 && UnityEngine.Random.Range(0, enemyStaticData.enemies.Length) == 1 && numbers.Length > i)
                {
                    Instantiate(enemyStaticData.enemies[1], spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[numbers[i]]);
                    rangedEnemies--;
                    i++;
                }
                else if (simpleEnemies > 0 && UnityEngine.Random.Range(0, enemyStaticData.enemies.Length) == 0 && numbers.Length > i)
                {
                    Instantiate(enemyStaticData.enemies[0], spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[numbers[i]]);
                    simpleEnemies--;
                    i++;
                }
                else if (numbers.Length <= i)
                {
                    Instantiate(Instantiate(obstacleStaticData.obstacles[UnityEngine.Random.Range(0,2)]), spawnedMap.gameObject.GetComponent<SpawnPoints>().transformPoints[obst[i - numbers.Length]]);
                    i++;
                }

            }
            Array.Clear(numbers, 0, numbers.Length);
            Array.Clear(obst, 0, obst.Length);
        }

    }
}
