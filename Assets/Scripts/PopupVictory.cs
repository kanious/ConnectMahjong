using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupVictory : MonoBehaviour
{

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
