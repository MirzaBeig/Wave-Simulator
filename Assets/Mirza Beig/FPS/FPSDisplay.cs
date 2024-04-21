using UnityEngine;

using TMPro;
using System;

[ExecuteAlways]
public class FPSDisplay : MonoBehaviour
{
    public float fps { get; private set; }      // Frames per second (interval average).
    public float frameMS { get; private set; }  // Milliseconds per frame (interval average).

    GUIStyle style = new GUIStyle();

    public int size = 32;

    [Range(0.0f, 2.0f)]
    public float scale = 1.0f;

    [Space]

    public Vector2 position = new Vector2(16.0f, 16.0f);

    public enum Alignment { Left, Right }
    public Alignment alignment = Alignment.Left;

    [Space]

    public Color colour = Color.green;

    [Space]

    public float updateInterval = 0.5f;

    float elapsedIntervalTime;
    int intervalFrameCount;

    [Space]

    [Tooltip("Optional. Will render using GUI if not assigned.")]
    public TextMeshProUGUI textMesh;

    // Get average FPS and frame delta (ms) for current interval (so far, if called early).

    public float GetIntervalFPS()
    {
        // 1 / time.unscaledDeltaTime for same-frame results.
        // Same as above, but uses accumulated frameCount and deltaTime.

        return intervalFrameCount / elapsedIntervalTime;
    }
    public float GetIntervalFrameMS()
    {
        // Calculate average frame delta during interval (time / frames).
        // Same as Time.unscaledDeltaTime * 1000.0f, using accumulation.

        return (elapsedIntervalTime * 1000.0f) / intervalFrameCount;
    }

    void Update()
    {
        intervalFrameCount++;
        elapsedIntervalTime += Time.unscaledDeltaTime;

        if (elapsedIntervalTime >= updateInterval)
        {
            fps = GetIntervalFPS();
            frameMS = GetIntervalFrameMS();

            fps = (float)Math.Round(fps, 2);
            frameMS = (float)Math.Round(frameMS, 2);

            intervalFrameCount = 0;
            elapsedIntervalTime = 0.0f;
        }

        if (textMesh)
        {
            textMesh.text = GetFPSText();
        }
        else
        {
            style.fontSize = Mathf.RoundToInt(size * scale);
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = colour;
        }
    }

    string GetFPSText()
    {
        return $"FPS: {fps:.00} ({frameMS:.00} ms)";
    }

    void OnGUI()
    {
        string fpsText = GetFPSText();

        if (!textMesh)
        {
            Vector2 scaledPosition = position * scale;

            float x = scaledPosition.x;

            if (alignment == Alignment.Right)
            {
                x = Screen.width - x - style.CalcSize(new GUIContent(fpsText)).x;
            }

            GUI.Label(new Rect(x, scaledPosition.y, 200, 100), fpsText, style);
        }
    }
}