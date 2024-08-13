using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupConfig : MonoBehaviour
{

    public void onClickResumeGame()
    {
        GameManager.Instance.ResumeGame();
        Destroy(gameObject);
    }

    public void OnClickNewGame()
    {
        GameManager.Instance.Initialize();
        Destroy(gameObject);
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OnClickClosePopup()
    {
        Destroy(gameObject);
    }
}
