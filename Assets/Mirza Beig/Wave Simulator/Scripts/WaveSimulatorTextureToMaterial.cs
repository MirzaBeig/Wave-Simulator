using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class WaveSimulatorTextureToMaterial : MonoBehaviour
{
    public Material material;
    public string textureName = "_WaveSimulation";

    WaveSimulator simulator;

    void Start()
    {
        simulator = GetComponent<WaveSimulator>();
        material.SetTexture(textureName, simulator.GetTexture());
    }
}
