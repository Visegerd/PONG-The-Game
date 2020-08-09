using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVScaler : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start() //Scale orto size of camera to start screen resolution
    {
        cam = GetComponent<Camera>();
        float width = Screen.width;
        float height = Screen.height;
        float ratio = width / height;
        cam.orthographicSize = (7.5f + ratio) / ratio;
    }
}
