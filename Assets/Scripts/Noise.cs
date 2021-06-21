using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Noise class
public static class Noise
{
    public enum NormalizeMode { Local, Global };

    // Generate a noise map that returns a grid of values between 0 and 1
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // the seed is useful for when we want to get the same map again
        System.Random prng = new System.Random(settings.seed);       // pseudo-random number generator
        // Each octave will be sampled from a different location, therefore we create an array
        Vector2[] octaveOffsets = new Vector2[settings.octaves];

        float amplitude = 1;        //amplitude
        float frequency = 1;        //frequency
        float noiseHeight = 0;      //height value

        float maxPossibleHeight = 0;

        //Loop through all octaves
        for (int i = 0; i < settings.octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + settings.offset.x + sampleCentre.x;  // scroll through the noise on the x axis
            float offsetY = prng.Next(-100000, 100000) - settings.offset.y + sampleCentre.y;  // scroll through the noise on the y axis
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= settings.persistance;
        }

        float maxLocalNoiseHeight = float.MinValue;      //max noise height
        float minLocalNoiseHeight = float.MaxValue;      //min noise height

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;        //amplitude
                frequency = 1;        //frequency
                noiseHeight = 0;      //height value

                for (int i = 0; i < settings.octaves; i++)
                {
                    // this is useful for when we change the noise scale - zoom in to the center of the noise map
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / settings.scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / settings.scale * frequency;

                    // the perlin value could sometimes be negative so that the noise height would decrease
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    // increase noise height by the Perlin value of each octave
                    noiseHeight += perlinValue * amplitude;

                    // at the end of each octave, the amplitude gets multipliet by the persistance value
                    amplitude *= settings.persistance;
                    // the frequency increases each octave
                    frequency *= settings.lacunarity;
                }

                // keep track of the lowest and highest values in the noise map
                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;

                if (settings.normalizeMode == NormalizeMode.Global)
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
            }
        }

        if (settings.normalizeMode == NormalizeMode.Local)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
            }
        }

            return noiseMap;
        }
    }

    [System.Serializable]
    public class NoiseSettings
    {
        public Noise.NormalizeMode normalizeMode;

        public float scale = 50;

        public int octaves = 6;
        [Range(0, 1)]
        public float persistance = .6f;
        public float lacunarity = 2;

        public int seed;
        public Vector2 offset;

        public void ValidateValues()
        {
            scale = Mathf.Max(scale, 0.01f);
            octaves = Mathf.Max(octaves, 1);
            lacunarity = Mathf.Max(lacunarity, 1);
            persistance = Mathf.Clamp01(persistance);
        }
    }

