using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndingBtn : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] MenuButton mBtn;

    private void Update()
    {
        if (mBtn.score > 999)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }

    public void ResetData()
    {
        DataPersistanceManager.Instance.NewGame();
    }
}
