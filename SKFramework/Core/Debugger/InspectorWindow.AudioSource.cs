using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(AudioSource))]
    public class AudioSourceInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            AudioSource audioSource = component as AudioSource;

            DrawText("Audio Clip", audioSource.clip != null ? audioSource.clip.name : "None(Audio Clip)", 175f);
            
            DrawText("Output", audioSource.outputAudioMixerGroup != null ? audioSource.outputAudioMixerGroup.name : "None(Audio Mixer Group)", 175f);

            audioSource.mute = DrawToggle("Mute", audioSource.mute, 175f);
            
            audioSource.bypassEffects = DrawToggle("Bypass Effects", audioSource.bypassEffects, 175f);

            audioSource.bypassListenerEffects = DrawToggle("Bypass Listener Effects", audioSource.bypassListenerEffects, 175f);

            audioSource.bypassReverbZones = DrawToggle("Bypass Reverb Zones", audioSource.bypassReverbZones, 175f);

            audioSource.playOnAwake = DrawToggle("Play On Awake", audioSource.playOnAwake, 175f);

            audioSource.loop = DrawToggle("Loop", audioSource.loop, 175f);

            floatValue = DrawHorizontalSlider("Priority", audioSource.priority, 0f, 256f, 175f, 50f);
            if (floatValue != audioSource.priority)
            {
                audioSource.priority = Mathf.RoundToInt(floatValue);
            }

            audioSource.volume = DrawHorizontalSlider("Volume", audioSource.volume, 0f, 1f, 175f, 50f);

            audioSource.pitch = DrawHorizontalSlider("Pitch", audioSource.pitch, -3f, 3f, 175f, 50f);

            audioSource.panStereo = DrawHorizontalSlider("Stereo Pan", audioSource.panStereo, -1f, 1f, 175f, 50f);

            audioSource.spatialBlend = DrawHorizontalSlider("Spatial Blend", audioSource.spatialBlend, 0f, 1f, 175f, 50f);

            audioSource.reverbZoneMix = DrawHorizontalSlider("Reverb Zone Mix", audioSource.reverbZoneMix, 0f, 1.1f, 175f, 50f);

            GUILayout.Label("3D Sound Settings");

            audioSource.dopplerLevel = DrawHorizontalSlider(30f, "Doppler Level", audioSource.dopplerLevel, 0f, 5f, 149f, 50f);

            audioSource.spread = DrawHorizontalSlider(30f, "Spread", audioSource.spread, 0f, 360f, 149f, 50f);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(30f);
                GUILayout.Label("Volume Rolloff", GUILayout.Width(149f));
                if (GUILayout.Toggle(audioSource.rolloffMode == AudioRolloffMode.Logarithmic, "Logarithmic"))
                    audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                if (GUILayout.Toggle(audioSource.rolloffMode == AudioRolloffMode.Linear, "Linear"))
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                if (GUILayout.Toggle(audioSource.rolloffMode == AudioRolloffMode.Custom, "Custom"))
                    audioSource.rolloffMode = AudioRolloffMode.Custom;
            }
            GUILayout.EndHorizontal();

            GUI.enabled = audioSource.rolloffMode != AudioRolloffMode.Custom;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(30f);
                GUILayout.Label("Min Distance", GUILayout.Width(149f));
                if (audioSource.rolloffMode != AudioRolloffMode.Custom)
                {
                    valueStr = audioSource.minDistance.ToString();
                    newValueStr = GUILayout.TextField(valueStr, GUILayout.ExpandWidth(true));
                    if (newValueStr != valueStr)
                    {
                        if (float.TryParse(newValueStr, out floatValue))
                        {
                            audioSource.minDistance = floatValue;
                        }
                    }
                }
                else
                {
                    GUILayout.Label("Controlled by curve");
                }
            }
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            audioSource.maxDistance = DrawFloat(30f, "Max Distance", audioSource.maxDistance, 149f, GUILayout.ExpandWidth(true));
        }
    }
}