using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshExtensions;

public class OutlineSpriteFactory {

    public Texture2D CreateSpriteOutline(Sprite sprite, int outlineWidth, Gradient outlineGradient, bool removeInterior = true)
    {
        Texture2D powerOfTwoTexture = sprite.texture.CreatePowerOfTwoCopyOfTexture(1 + outlineWidth);
        Texture2D cleanCopyTexture = Object.Instantiate(powerOfTwoTexture);

        Color32[] colourArray;
        Color32[] newColourArray;

        colourArray = powerOfTwoTexture.GetPixels32();
        newColourArray = powerOfTwoTexture.GetPixels32();
        for (int i = 0; i < outlineWidth; i++)
        {
            Color32 fillColour = outlineGradient.Evaluate((1f / (float)outlineWidth) * i);

            Debug.Log(fillColour);

            for (int x = 1; x < powerOfTwoTexture.width - 1; x++)
            {
                for (int y = 1; y < powerOfTwoTexture.height - 1; y++)
                {
                    FillEmptyNeighbours(new Vector2Int(x, y), powerOfTwoTexture.width, powerOfTwoTexture.height, fillColour, colourArray, ref newColourArray);
                }
            }
            System.Array.Copy(newColourArray, colourArray, newColourArray.Length);
        }

        for (int i = 0; i < outlineWidth; i++)
        {
            Color32 fillColour = outlineGradient.Evaluate((1f / (float)outlineWidth) * i);

            for (int x = 1; x < powerOfTwoTexture.width - 1; x++)
            {
                for (int y = 1; y < powerOfTwoTexture.height - 1; y++)
                {
                    SmoothWithNeighbours(new Vector2Int(x, y), powerOfTwoTexture.width, powerOfTwoTexture.height, fillColour, colourArray, ref newColourArray);
                }
            }
            System.Array.Copy(newColourArray, colourArray, newColourArray.Length);
        }

        if (removeInterior)
        {

            Color32[] exclusionColourArray = cleanCopyTexture.GetPixels32();
            for (int x = 0; x < powerOfTwoTexture.width; x++)
            {
                for (int y = 0; y < powerOfTwoTexture.height; y++)
                {
                    Color32 colour = colourArray[x + (y * powerOfTwoTexture.height)];
                    if (colour.a > 0)
                        newColourArray[x + (y * powerOfTwoTexture.height)].a = (byte)Mathf.Min((255 - exclusionColourArray[x + (y * powerOfTwoTexture.height)].a), colour.a);
                }
            }

        }

        powerOfTwoTexture.SetPixels32(newColourArray);

        return powerOfTwoTexture;
    }

    private void SmoothWithNeighbours(Vector2Int position, int width, int height, Color32 fillColour, Color32[] srcColourArray, ref Color32[] dstColourArray)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            int index = position.x + (width * position.y);
            if (srcColourArray[index].a > 0)
            {
                int neighbours = 0;

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        int offsetIndex = (position.x + x) + (width * (position.y + y));
                        if (srcColourArray[offsetIndex].a > 0)
                        {
                            neighbours++;
                        }
                    }
                }

                byte alpha = 0;
                if (neighbours == 5)
                {
                    alpha = 85;
                }
                else if (neighbours == 6)
                {
                    alpha = 170;
                }
                else if (neighbours > 6)
                {
                    alpha = 255;
                }

                dstColourArray[index].a = alpha;

            }
        }
    }

    private void FillEmptyNeighbours(Vector2Int position, int width, int height, Color32 fillColour, Color32[] srcColourArray, ref Color32[] dstColourArray)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            int index = position.x + (width * position.y);
            if (srcColourArray[index].a > 0)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        index = (position.x + x) + (width * (position.y + y));
                        if (srcColourArray[index].a == 0)
                        {
                            dstColourArray[index] = fillColour;
                        }
                    }
                }
            }
        }
    }
}
