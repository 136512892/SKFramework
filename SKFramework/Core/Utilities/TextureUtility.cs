using System.IO;
using UnityEngine;

namespace SK.Framework.Utility
{
    public class TextureUtility
    {
        public static Sprite ToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        public static Sprite ToSprite(Texture2D texture, Vector2 pivot)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
        }
        public static void WriteToPNGFile(Texture2D texture, string path)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }
        public static void WriteToJPGFile(Texture2D texture, string path)
        {
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(path, bytes);
        }
    }
}