using System;
using UnityEngine;

namespace SK.Framework.Utility
{
    public class AudioClipUtility
    {
        public static byte[] ToPCM16Data(AudioClip audioClip)
        {
            float[] samples = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(samples, 0);
            short[] samples_int16 = new short[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                float f = samples[i];
                samples_int16[i] = (short)(f * short.MaxValue);
            }
            byte[] retBytes = new byte[samples_int16.Length * 2];
            Buffer.BlockCopy(samples_int16, 0, retBytes, 0, retBytes.Length);
            return retBytes;
        }
    }
}