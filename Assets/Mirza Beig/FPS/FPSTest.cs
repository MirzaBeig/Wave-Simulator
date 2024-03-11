using UnityEngine;

using TMPro;
using System;

[ExecuteAlways]
public class FPSTest : MonoBehaviour
{
    public Vector2 size = new Vector2(128.0f, 64.0f);
    public Vector2 position = new Vector2(16.0f, 64.0f);

    [Space]

    [Range(8, 64)]
    public int fontSize = 16;

    [Range(0.0f, 2.0f)]
    public float scale = 1.0f;

    [Space]

    public float spacing = 72;

    [Space]

    public int[] fpsButtons = new int[] { 0, 10, 30, 45, 60, 90, 120 };

    void Start()
    {

    }

    void OnGUI()
    {
        Vector2 scaledPosition = position * scale;
        Vector2 scaledSize = size * scale;

        float scaledPositionY = scaledPosition.y * scale;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fontSize = Mathf.RoundToInt(fontSize * scale);

        for (int i = 0; i < fpsButtons.Length; i++)
        {
            int fps = fpsButtons[i];

            if (GUI.Button(new Rect(scaledPosition.x, scaledPositionY, scaledSize.x, scaledSize.y), $"FPS: {fps}", buttonStyle))
            {
                Application.targetFrameRate = fps;
            }

            scaledPositionY += spacing * scale;
        }
    }
}