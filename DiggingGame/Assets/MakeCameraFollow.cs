using UnityEngine;

public class MakeCameraFollow : MonoBehaviour
{
    private Camera _mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = _mainCam.transform.position;
        pos = transform.position;
        pos.z = _mainCam.transform.position.z;
        _mainCam.transform.position = pos;
    }
}
