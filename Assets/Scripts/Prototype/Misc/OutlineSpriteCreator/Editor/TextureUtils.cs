using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JoshExtensions
{
    public static class TextureExtensions
    {
        public static Texture2D CreateScaledCopy(this Texture2D sourceTexture, int width, int height, FilterMode filterMode = FilterMode.Trilinear)
        {
            Rect textureRect = new Rect(0, 0, width, height);
            GPUScalingMethod(sourceTexture, width, height, filterMode);

            Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
            result.Resize(width, height);
            result.ReadPixels(textureRect, 0, 0, true);
            return result;
        }

        public static void Scale(this Texture2D sourceTexture, int width, int height, FilterMode filterMode = FilterMode.Trilinear)
        {
            Rect textureRect = new Rect(0, 0, width, height);
            GPUScalingMethod(sourceTexture, width, height, filterMode);

            sourceTexture.Resize(width, height);
            sourceTexture.ReadPixels(textureRect, 0, 0, true);
            sourceTexture.Apply();
        }

        public static Texture2D CreatePowerOfTwoCopyOfTexture(this Texture2D texture, int sizeOffset = 0)
        {
            int size = Mathf.ClosestPowerOfTwo(Mathf.Max(texture.width + sizeOffset, texture.height + sizeOffset));
            Vector2Int offset = new Vector2Int(size - texture.width, size - texture.height);
            Texture2D powerOfTwoTexture = new Texture2D(size, size, texture.format, texture.mipmapCount > 1);

            Color32[] textureColours = powerOfTwoTexture.GetPixels32();
            for (int i = 0; i < textureColours.Length; i++)
            {
                textureColours[i] = new Color32(255, 255, 255, 0);
            }
            powerOfTwoTexture.SetPixels32(textureColours);

            Graphics.CopyTexture(texture, 0, 0, 0, 0, texture.width, texture.height, powerOfTwoTexture, 0, 0, offset.x / 2, offset.y / 2);
            return powerOfTwoTexture;
        }

        private static void GPUScalingMethod(Texture2D sourceTexture, int width, int height, FilterMode filterMode)
        {
            sourceTexture.filterMode = filterMode;
            sourceTexture.Apply(true);

            RenderTexture renderTarget = new RenderTexture(width, height, 32);
            Graphics.SetRenderTarget(renderTarget);

            GL.LoadPixelMatrix(0, 1, 1, 0);
            GL.Clear(true, true, new Color(0, 0, 0, 0));

            Graphics.DrawTexture(new Rect(0, 0, 1, 1), sourceTexture);
        }

        public static void SaveTexture(this Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            for (int i = 0; i < bytes.Length; i++)
            {
                writer.Write(bytes[i]);
            }
            writer.Close();
            stream.Close();
        }

    }
}
