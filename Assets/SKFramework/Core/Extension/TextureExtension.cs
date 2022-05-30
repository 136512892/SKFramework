using System.IO;
using UnityEngine;

namespace SK.Framework
{
    public static class TextureExtension
    {
        public static Sprite ToSprite(this Texture2D self)
        {
            return Sprite.Create(self, new Rect(0, 0, self.width, self.height), Vector2.one * 0.5f);
        }
        public static Sprite ToSprite(this Texture2D self, Vector2 pivot)
        {
            return Sprite.Create(self, new Rect(0, 0, self.width, self.height), pivot);
        }
        public static void WriteToPNGFile(this Texture2D self, string path)
        {
            byte[] bytes = self.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }
        public static void WriteToJPGFile(this Texture2D self, string path)
        {
            byte[] bytes = self.EncodeToJPG();
            File.WriteAllBytes(path, bytes);
        }
    }
}