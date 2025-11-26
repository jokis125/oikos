using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 start;
    Vector3 destination;
    float lerpIndex;


    public Camera cam;
    public static CameraFollow instance;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        start = destination = transform.position = new Vector3(80, 40, -10);
        lerpIndex = 1;
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpIndex < 1.0f)
        {
            lerpIndex += Time.deltaTime*2;
            transform.position = Vector3.Lerp(start, destination, lerpIndex);
        }
    }

    public void MoveCamera(Vector2 from, Vector2 to)
    {
        start = new Vector3(from.x,from.y, -10f);
        destination = new Vector3(to.x, to.y, -10f);
        lerpIndex = 0.0f;
    }
}
