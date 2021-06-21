using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    // Create a texture out of a colour map
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;      // fix blurriness
        texture.wrapMode = TextureWrapMode.Clamp;   //fix wrapping
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    // Create a texture out of a height map
    public static Texture2D TextureFromHeightMap(HeightMap heightMap)
    {
        // get the width and height of the noisemap
        int width = heightMap.values.GetLength(0);
        int height = heightMap.values.GetLength(1);

        // create a colour array
        Color[] colourMap = new Color[width * height];
        // loop through all of the values in the noisemap
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // get the index of the colour map and set it to a colour between black and white
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(heightMap.minValue, heightMap.maxValue, heightMap.values[x, y]));
            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }
}
