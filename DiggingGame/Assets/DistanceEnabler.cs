using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DistanceEnabler : MonoBehaviour
{
    public float buffer = 5;
    private Camera _camera;
    private Rect _enabled;
    private List<GameObject> _map = new();

    private void Start()
    {
        _camera = Camera.main;
        Events.OnWallRegister.AddListener(OnWallRegister);
    }

    private void OnWallRegister(GameObject wall)
    {
        _map.Add(wall);
    }

    private void Update()
    {
        Vector2 min = transform.position;
        var screen = new Vector3(Screen.width, Screen.height);
        Vector2 max = _camera.ScreenToWorldPoint(_camera.WorldToScreenPoint(transform.position) + screen);
        var size = max - min;
        var rect = new Rect(min - size / 2 - Vector2.one * 5, size + Vector2.one * 10);

        _map.RemoveAll(x => !x);
        
        foreach (var obj in _map)
        {
            obj.SetActive(rect.Contains(obj.transform.position));
        }
    }

    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        UnityEditor.Handles.DrawSolidRectangleWithOutline(_enabled, new Color(0f, 0f, 0f, 0f), Color.black);
        #endif
    }
}
