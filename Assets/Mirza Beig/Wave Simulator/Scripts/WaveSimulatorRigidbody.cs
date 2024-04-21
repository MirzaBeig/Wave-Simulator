using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSimulatorRigidbody : MonoBehaviour
{
    Rigidbody rigidbody;
    public WaveSimulator waveSimulator;

    public Texture2D analyzePixels;

    [Space]

    public float inputRadius = 10.0f;
    public float collisionForce = 0.1f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        analyzePixels = new Texture2D(32, 32, TextureFormat.RGBA64, false, true);
    }

    void FixedUpdate()
    {
        rigidbody.WakeUp();
    }
    void LateUpdate()
    {
        //Graphics.CopyTexture(waveSimulator.GetTexture(), 0, 0, 8, 8, 32, 32, currenttexture, 0, 0, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 contactPoint = contact.point;
            Vector3 contactNormal = contact.normal;

            Ray ray = new()
            {
                origin = contactPoint + (contactNormal * 0.1f),
                direction = -contactNormal
            };

            if (collision.collider.Raycast(ray, out RaycastHit hit, 0.2f))
            {
                RenderTexture renderTexture = waveSimulator.GetTexture();

                RenderTexture.active = renderTexture;

                Vector2 collisionPixelCoord = hit.textureCoord * waveSimulator.size;
                analyzePixels.ReadPixels(new Rect(collisionPixelCoord.x, collisionPixelCoord.y, 1, 1), 0, 0, false);

                RenderTexture.active = null;

                Color colour = analyzePixels.GetPixel(0, 0);

                float waveHeight = colour.g;
                //Vector3 waveVelocity = new(colour.b, 0.0f, colour.a);

                float waveHeightAbs = Mathf.Abs(waveHeight);

                if (waveHeightAbs > 0.001f)
                {
                    //print(waveHeightAbs);
                    //rigidbody.AddForceAtPosition(waveVelocity, contactPoint, ForceMode.VelocityChange);
                }

                waveSimulator.AddInput(collisionPixelCoord, inputRadius * (collisionForce * collision.relativeVelocity.magnitude));
            }
        }
    }

    //void OnCollisionStay(Collision collision)
    //{
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        Vector3 contactPoint = contact.point;
    //        Vector3 contactNormal = contact.normal;

    //        Ray ray = new()
    //        {
    //            origin = contactPoint + (contactNormal * 0.1f),
    //            direction = -contactNormal
    //        };

    //        if (collision.collider.Raycast(ray, out RaycastHit hit, 0.2f))
    //        {
    //            RenderTexture renderTexture = waveSimulator.GetTexture();
    //            RenderTexture.active = renderTexture;

    //            Vector2 collisionPixelCoord = hit.textureCoord * waveSimulator.size;

    //            // Read 4 pixels around the contact point
    //            Texture2D analyzePixels = new Texture2D(2, 2, TextureFormat.RGBA32, false);
    //            analyzePixels.ReadPixels(new Rect(collisionPixelCoord.x - 0.5f, collisionPixelCoord.y - 0.5f, 2, 2), 0, 0, false);
    //            analyzePixels.Apply();

    //            RenderTexture.active = null;

    //            // Calculate normals based on 4 pixels wave height
    //            Color topLeft = analyzePixels.GetPixel(0, 1);
    //            Color topRight = analyzePixels.GetPixel(1, 1);
    //            Color bottomLeft = analyzePixels.GetPixel(0, 0);
    //            Color bottomRight = analyzePixels.GetPixel(-1, 0);

    //            float heightLeft = (topLeft.g + bottomLeft.g) * 0.5f;
    //            float heightRight = (topRight.g + bottomRight.g) * 0.5f;
    //            float heightTop = (topLeft.g + topRight.g) * 0.5f;
    //            float heightBottom = (bottomLeft.g + bottomRight.g) * 0.5f;

    //            Vector3 calculatedNormal = new Vector3(heightLeft - heightRight, 2.0f, heightBottom - heightTop).normalized;

    //            // Determine force direction and magnitude based on the wave height and normal
    //            float waveHeight = (topLeft.g + topRight.g + bottomLeft.g + bottomRight.g) * 0.25f;
    //            Vector3 waveVelocity = calculatedNormal * waveHeight;

    //            float waveHeightAbs = Mathf.Abs(waveHeight);

    //            print(calculatedNormal);
    //            if (waveHeightAbs > 0.001f)
    //            {
    //                rigidbody.AddForceAtPosition(waveVelocity, contactPoint, ForceMode.VelocityChange);
    //            }

    //            //waveSimulator.AddInput(collisionPixelCoord, inputRadius);
    //        }
    //    }
    //}
}
