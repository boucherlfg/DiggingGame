using System;
using UnityEngine;

public class SpawnOnConsume : ConsumableScript
{
    private Camera _mainCamera;
    public Targetter targetter;
    public GameObject toSpawn;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public override void Consume(PlayerInteraction player)
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Physics2D.OverlapPoint(mousePos)) return;
        
        if (!toSpawn) return;
        
        Instantiate(toSpawn, mousePos, Quaternion.identity);
    }
}