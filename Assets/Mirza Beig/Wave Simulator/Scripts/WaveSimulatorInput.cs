using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSimulatorInput : MonoBehaviour
{
    Vector4 mouse;
    WaveSimulator waveSimulator;

    public float inputRadius = 10.0f;

    void Start()
    {
        waveSimulator = GetComponent<WaveSimulator>();
    }

    void UpdateMouseInput()
    {
        mouse.z = Input.GetMouseButton(0) ? 1.0f : 0.0f;
        mouse.w = Input.GetMouseButtonDown(0) ? 1.0f : 0.0f;
    }
    void UpdateMousePosition(Vector2 position)
    {
        mouse.x = position.x;
        mouse.y = position.y;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.0f);

                // Using textureCoord requires mesh collider (Convex toggle must be OFF).
                // https://docs.unity3d.com/ScriptReference/RaycastHit-textureCoord.html

                Vector2 mousePosition = (hit.textureCoord) * waveSimulator.size;

                UpdateMousePosition(mousePosition);
            }
        }

        waveSimulator.SetMouseInput(mouse, inputRadius);
    }

    void Update()
    {
        UpdateMouseInput();
    }
}
