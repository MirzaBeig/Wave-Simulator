using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class WaveSimulatorInput : MonoBehaviour
{
    Vector4 mouse;
    WaveSimulator waveSimulator;

    public float inputRadius = 10.0f;

    WaveSimulatorMaterial waveSimulatorTextureToMaterial;

    void Start()
    {
        waveSimulator = GetComponent<WaveSimulator>();
        waveSimulatorTextureToMaterial = GetComponent<WaveSimulatorMaterial>();
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

                Vector2Int mouseCoord = new(Mathf.RoundToInt(mouse.x), Mathf.RoundToInt(mouse.y));

                waveSimulator.AddInput(mouseCoord, inputRadius);
            }
        }

        //waveSimulator.SetMouseInput(mouse, inputRadius);

        waveSimulatorTextureToMaterial.material.SetVector(waveSimulatorTextureToMaterial.mouseName, mouse);
        waveSimulatorTextureToMaterial.material.SetFloat(waveSimulatorTextureToMaterial.radiusName, inputRadius);
    }

    void Update()
    {
        UpdateMouseInput();
    }
}
