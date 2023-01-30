using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButton : MonoBehaviour
{
    [SerializeField] Canvas screen;
    [SerializeField] TextMeshProUGUI currentPoints;

    [SerializeField] public Button returnToMenuBtn;

    //[SerializeField] GameObject[] otherUis;
    //[SerializeField] GameObject gameOverUi;

    private Image shade;
    private bool loading = false;
    private int loadingNumber;
    private bool paused;
    public int score = 0;

    private float cutsceneSpeed = 0.0015f;

    float alpha = 0;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            cutsceneSpeed *= 100;
        }
        else
        {

        }
    }

    void Update()
    {
        if (loading) 
        {
            alpha += cutsceneSpeed;
            
            if(shade != null)
            {
                shade.color = new Color(0, 0, 0, alpha);
            }

            if(alpha > 1)
            {
                if(loading)
                    SceneLoaded(loadingNumber);
                loading = false;
            }
        }
        if (Equals(SceneManager.GetActiveScene(), SceneManager.GetSceneByBuildIndex(2)))
        {
            currentPoints.text = "Points: " + GameManager.instance.Points(0);
        }
        else if(Equals(SceneManager.GetActiveScene(), SceneManager.GetSceneByBuildIndex(1)))
        {
            score = DataPersistanceManager.Instance.GetData();
            currentPoints.text = "HighestScore: " + score;
        }
    }

    public void NextScene(int level)
    {
        Time.timeScale = 1;
        paused = false;
        alpha = 0;
        loadingNumber = level;
        loading = true;
        screen.gameObject.SetActive(true);
        shade = screen.gameObject.AddComponent<Image>();
        shade.color = new Color(0, 0, 0, alpha);
    }

    private void SceneLoaded(int level)
    {
       SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void PauseButton()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            Time.timeScale = 1;
        }
    }

    public void ExitButton()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
        else
        {
            Application.Quit();
        }
    }

    //public void OnPlayerDeath()
    //{
    //    for(int i = 0; i < otherUis.Length; i++)
    //    {
    //        otherUis[i].SetActive(false);
    //    }
    //    gameOverUi.SetActive(true);
        
    //}

}
