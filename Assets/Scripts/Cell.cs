using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region GameObject Variables
    [SerializeField] GameObject _goSelect = null;
    [SerializeField] UISprite _spriteIcon = null;

    [HideInInspector] public int _spriteIdx = -1;
    [HideInInspector] public Coord _coord;
    #endregion

    public void SetIcon(TILE_SET tile, int index)
    {
        _spriteIdx = index;
        _spriteIcon.spriteName = string.Format("{0}", index - 1);
    }

    public void SetCoord(Coord coord)
    {
        _coord = coord;
        _goSelect.SetActive(false);
    }

    public void ButtonPressed()
    {
        GameManager.Instance.OnClickCell(this);
    }

    public void ToggleSelect(bool select)
    {
        _goSelect.SetActive(select);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}