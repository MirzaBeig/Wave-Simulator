
#ifndef WAVE_SIMULATOR
#define WAVE_SIMULATOR

// Neighbour pixel offsets: top, bottom, left, right.

#define off_L int2(-1,  0);
#define off_R int2( 1,  0);
#define off_T int2( 0,  1);
#define off_B int2( 0, -1);

void WaveSimulator_float(Texture2D heightTexture, float2 uv, SamplerState ss, float2 resolution, float4 mouse, float inputRadius, float speed, float decay, out float4 output)
{
    int2 id_C = uv * resolution;
        
    int2 id_L = id_C + off_L;
    int2 id_R = id_C + off_R;
    int2 id_T = id_C + off_T;
    int2 id_B = id_C + off_B;
    
    float4 height = heightTexture[id_C];
        
    // If the mouse is down, add a wave.
    // Can also just multiply by mouse.z to avoid the if statement.
    
    if (mouse.z > 0.0)
    {
        height.x += smoothstep(1.0, 0.0, length(mouse.xy - id_C) / inputRadius);
    }
    
    float height_L = heightTexture[id_L].x;
    float height_R = heightTexture[id_R].x;
    float height_T = heightTexture[id_T].x;
    float height_B = heightTexture[id_B].x;
    
    float neighbourSum = height_L + height_R + height_T + height_B;
    float neighbourAverage = neighbourSum / 4.0;
    
    // The actual propagation...
    // Based off: https://github.com/evanw/webgl-water/blob/master/water.js.
    
    // Move velocity towards local average.    
    // target - current = scaled direction.
    
    height.y += neighbourAverage - height.x;
    height.y *= decay;
    
    // Apply velocity.
    
    height.x += height.y * speed;
    
    height.r = 1.0;
    height.gb = 0.0;
    
    height.a = 1.0;
    
    height = heightTexture[uv * resolution];
    
    output = height;
}

#endif