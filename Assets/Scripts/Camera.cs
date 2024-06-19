using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private const float MOUSE_SENSITIVETY = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var cam = GetComponent<Camera>();
        if (Input.GetMouseButton(2))
        {
            float moveX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVETY;
            float moveY = Input.GetAxis("Mouse Y") * MOUSE_SENSITIVETY;
            cam.transform.localPosition -= new Vector3(moveX, 0, moveY);
        }
    }
}