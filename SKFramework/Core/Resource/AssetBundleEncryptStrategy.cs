/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Resource
{
    public abstract class AssetBundleEncryptStrategy
    {
        public abstract bool IsSecretKeyValid(string secretKey);

        public bool Encrypt(ref byte[] bytes, string secretKey)
        {
            if (IsSecretKeyValid(secretKey))
            {
                bytes = EncryptInternal(bytes, secretKey);
                return true;
            }
            return false;
        }
        public bool Decrypt(ref byte[] bytes, string secretKey)
        {
            if (IsSecretKeyValid(secretKey))
            {
                bytes = DecryptInternal(bytes, secretKey);
                return true;
            }
            return false;
        }

        protected abstract byte[] EncryptInternal(byte[] bytes, string secretKey);
        protected abstract byte[] DecryptInternal(byte[] bytes, string secretKey);
    }
}