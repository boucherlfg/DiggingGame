using System;
using UnityEngine;

public class WallRegister : MonoBehaviour
{
    private void Start()
    {
        Events.OnWallRegister.Invoke(gameObject);
    }
}