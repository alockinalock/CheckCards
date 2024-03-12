using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    private GridManager gridManager;

    public void Init(GridManager manager, bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor;
        gridManager = manager;
    }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        if (gridManager != null)
        {
            gridManager.TileClicked(this);
        }
    }
}
