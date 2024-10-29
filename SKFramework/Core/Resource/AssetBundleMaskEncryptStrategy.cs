/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Text.RegularExpressions;

namespace SK.Framework.Resource
{
    public class AssetBundleMaskEncryptStrategy : AssetBundleEncryptStrategy
    {
        public override bool IsSecretKeyValid(string secretKey)
        {
            return Regex.IsMatch(secretKey, @"^\d+$");
        }

        protected override byte[] EncryptInternal(byte[] bytes, string secretKey)
        {
            return Encode(bytes, secretKey);
        }
        protected override byte[] DecryptInternal(byte[] bytes, string secretKey)
        {
            return Encode(bytes, secretKey);
        }

        private byte[] Encode(byte[] bytes, string secretKey)
        {
            int[] array = StringToIntArray(secretKey);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ array[i % array.Length]);
            }
            return bytes;
        }

        private int[] StringToIntArray(string text)
        {
            int[] array = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                array[i] = int.Parse(text[i].ToString());
            }
            return array;
        }
    }
}