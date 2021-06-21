using UnityEngine;
using System.Collections;

// FallOffGenerator class
public static class FalloffGenerator
{
    // GenerateFallOffMap method
    public static float[,] GenerateFalloffMap(int size)
    {
        // create map
        float[,] map = new float[size, size];

        // populate map 
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                // find out which value (X or Y) is closest to the edge of the square
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                // set the map value
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }


    // Evaluate method
    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
