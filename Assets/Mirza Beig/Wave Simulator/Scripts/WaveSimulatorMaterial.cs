using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class WaveSimulatorMaterial : MonoBehaviour
{
    public Material material;

    public string textureName = "_Wave_Simulation";

    [Space]

    public string mouseName = "_Mouse";
    public string radiusName = "_Input_Radius";

    [Space]

    public string speedName = "_Speed";
    public string decayName = "_Decay";

    [Space]

    public string deltaTimeName = "_DeltaTime";

    WaveSimulator simulator;

    void Start()
    {
        simulator = GetComponent<WaveSimulator>();
        material.SetTexture(textureName, simulator.GetTexture());
    }

    void FixedUpdate()
    {
        material.SetFloat(deltaTimeName, Time.deltaTime);
    }

    void LateUpdate()
    {
        material.SetFloat(speedName, simulator.speed);
        material.SetFloat(decayName, 1.0f - simulator.decay);
    }
}
