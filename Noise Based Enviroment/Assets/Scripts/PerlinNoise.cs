using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;
    public double powLvl = 1.0f;
    public double powLvl2 = 1.0f;
    public double level1Threshold = 0.3;
    public float colorCorrection = 0.1f;
    public float r = 252f;
    public float g = 206f;
    public float b = 5f;

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
        float xCoord = (float)x / width * scale + offsetX; 
        float yCoord = (float)y / height * scale + offsetY;

        float baseNoise = (1 * Mathf.PerlinNoise(xCoord, yCoord) + 0.5f * Mathf.PerlinNoise(2 * xCoord, 2 * yCoord) + 0.25f * Mathf.PerlinNoise(4 * xCoord, 4 * yCoord)) / (1 + 0.5f + 0.25f);

        float elevation = (float)Math.Pow((double)baseNoise, powLvl);

        // make second noise field to combine
        float xCoord2 = (float)x / width * scale + offsetX;
        float yCoord2 = (float)y / height * scale + offsetY;

        float baseNoise2 = (1 * Mathf.PerlinNoise(xCoord2, yCoord2) + 0.5f * Mathf.PerlinNoise(2 * xCoord2, 2 * yCoord2) + 0.25f * Mathf.PerlinNoise(4 * xCoord2, 4 * yCoord2)) / (1 + 0.5f + 0.25f);

        float Noise2 = (float)Math.Pow((double)baseNoise2, powLvl2);

        if(elevation < level1Threshold)
        {
            return new Color(0, 0, elevation + colorCorrection);
        }
        else
        {
            if(Noise2 < 0.2f)
            {
                return new Color(r/255f, g/255f, b/255f);
            }

            if (Noise2 > 0.4f)
            {
                return new Color(255, 0, 0);
            }
            return new Color(0, elevation, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTexture = GenerateTexture();     
    }
}
