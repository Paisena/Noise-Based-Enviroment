using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;
    public float offsetX3 = 200f;
    public float offsetY3 = 200f;
    public float offsetX4 = 300f;
    public float offsetY4 = 300f;

    public double powLvl = 1.0f;
    public double powLvl2 = 1.0f;
    public double level1Threshold = 0.3;
    public double level3Threshold = 0.7;
    public double level4Threshold = 0.5;
    public float colorCorrection = 0.1f;
    public float r = 252f;
    public float g = 206f;
    public float b = 5f;
    public float dividerX = 0;
    public float dividerVal = 1f;

    [SerializeField] GameObject dividerGO;

    Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        dividerX = dividerGO.transform.position.x*(dividerVal) + 256f/2;

        float xCoord = (float)x / width * scale + offsetX; 
        float yCoord = (float)y / height * scale + offsetY;

        float baseNoise = (1 * Mathf.PerlinNoise(xCoord, yCoord) + 0.5f * Mathf.PerlinNoise(2 * xCoord, 2 * yCoord) + 0.25f * Mathf.PerlinNoise(4 * xCoord, 4 * yCoord)) / (1 + 0.5f + 0.25f);

        float elevation = (float)Math.Pow((double)baseNoise, powLvl);

        // make second noise field to combine
        float xCoord2 = (float)x / width * scale + offsetX;
        float yCoord2 = (float)y / height * scale + offsetY;

        float baseNoise2 = (1 * Mathf.PerlinNoise(xCoord2, yCoord2) + 0.5f * Mathf.PerlinNoise(2 * xCoord2, 2 * yCoord2) + 0.25f * Mathf.PerlinNoise(4 * xCoord2, 4 * yCoord2)) / (1 + 0.5f + 0.25f);

        float Noise2 = (float)Math.Pow((double)baseNoise2, powLvl2);

        // make two more to deremine sides
        float xCoord3 = (float)x / width * scale + offsetX3;
        float yCoord3 = (float)y / height * scale + offsetY3;

        float baseNoise3 = (1 * Mathf.PerlinNoise(xCoord3, yCoord3) + 0.5f * Mathf.PerlinNoise(2 * xCoord3, 2 * yCoord3) + 0.25f * Mathf.PerlinNoise(4 * xCoord3, 4 * yCoord3)) / (1 + 0.5f + 0.25f);

        float Noise3 = (float)Math.Pow((double)baseNoise3, powLvl2);
        float xCoord4 = (float)x / width * scale + offsetX4;
        float yCoord4 = (float)y / height * scale + offsetY4;

        float baseNoise4 = (1 * Mathf.PerlinNoise(xCoord4, yCoord4) + 0.5f * Mathf.PerlinNoise(2 * xCoord4, 2 * yCoord4) + 0.25f * Mathf.PerlinNoise(4 * xCoord4, 4 * yCoord4)) / (1 + 0.5f + 0.25f);

        float Noise4 = (float)Math.Pow((double)baseNoise4, powLvl2);

        if(elevation < level1Threshold)
        {   
            //water
            return new Color(0, 0, elevation + colorCorrection);
        }
        else
        {
            if (Noise3 > level3Threshold && x > dividerX)
            {
                return new Color(255, 0, 0);
            }
            if (Noise4 > level4Threshold && x < dividerX)
            {
                return new Color(0, 1, 1);
            }
            // if(Noise2 < 0.2f)
            // {
            //     //beaches/border
            //     return new Color(r/255f, g/255f, b/255f);
            // }

            if (Noise2 > 0.4f)
            {
                // snow
                return new Color(255, 255, 255);
            }
            // if (Noise4 > level4Threshold)
            // {
            //     return new Color(0.5f, 0.5f, 0.5f);
            // }
            // green ground
            return new Color(0, elevation, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTexture = GenerateTexture();     
    }
}
