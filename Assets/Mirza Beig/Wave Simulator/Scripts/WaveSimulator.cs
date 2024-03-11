using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering;

public class WaveSimulator : MonoBehaviour
{
    public ComputeShader shader;

    // Make sure this matches shader.

    const int THREAD_COUNT = 16;

    [Space]

    [Range(32, 1024)]
    public int size = 256;

    [Range(0.0f, 1.0f)]
    public float decay = 0.1f;

    [Space]

    [Range(0.0f, 2.0f)]
    public float speed = 1.0f;

    [Range(1, 32)]
    public int solverIterations = 1;

    [Space]

    public bool filteredAdvection;
    public FilterMode textureFilterMode = FilterMode.Point;

    // Textures.

    RenderTexture heightTexture;

    int kernelCount = 0;

    int kernel_Clear;

    int kernel_WaveSimulator;
    int kernel_CalculateNormals;

    int dispatchSize;

    RenderTexture CreateTexture(GraphicsFormat format)
    {
        RenderTexture tex = new(size, size, 0, format)
        {
            filterMode = textureFilterMode,
            wrapMode = TextureWrapMode.Clamp,

            enableRandomWrite = true
        };

        tex.Create();

        return tex;
    }

    void DispatchKernel(int kernel)
    {
        shader.Dispatch(kernel, dispatchSize, dispatchSize, 1);
    }

    void Start()
    {
        heightTexture = CreateTexture(GraphicsFormat.R16G16B16A16_SFloat);

        kernel_Clear = shader.FindKernel("Kernel_Clear"); kernelCount++;

        kernel_WaveSimulator = shader.FindKernel("Kernel_WaveSimulator"); kernelCount++;
        kernel_CalculateNormals = shader.FindKernel("Kernel_CalculateNormals"); kernelCount++;

        // Bind textures.

        for (int kernel = 0; kernel < kernelCount; kernel++)
        {
            // Not all kernels read/write into all textures, but this is easier to manage.

            shader.SetTexture(kernel, "heightTexture", heightTexture);
        }

        shader.SetInt("size", size);
        dispatchSize = Mathf.CeilToInt(size / THREAD_COUNT);

        // Clear.

        DispatchKernel(kernel_Clear);
    }

    void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;

        // Update properties.

        shader.SetFloat("deltaTime", deltaTime);

        shader.SetFloat("decay", 1.0f - decay);
        shader.SetFloat("speed", speed);

        // Propagate.

        for (int i = 0; i < solverIterations; i++)
        {
            DispatchKernel(kernel_WaveSimulator);
        }

        // Dispatch.

        DispatchKernel(kernel_CalculateNormals);
    }

    public void SetMouseInput(Vector4 mouse, float radius)
    {
        shader.SetVector("iMouse", mouse);
        shader.SetFloat("inputRadius", radius);
    }   

    public RenderTexture GetTexture()
    {
        return heightTexture;
    }
}
