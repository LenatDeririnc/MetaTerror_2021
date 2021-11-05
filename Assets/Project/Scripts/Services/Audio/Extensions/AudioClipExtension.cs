using Services.Audio.Components;
using UnityEngine;

namespace Services.Audio.Extensions
{
    public static class AudioClipExtension
    {
        public static void Play(this AudioClip clip, int audioSourceId)
        {
            var source = AudioSourceContainer.Source(audioSourceId);
            source.clip = clip;
            source.Play();
        }

        public static void PlayOnce(this AudioClip clip, int audioSourceId)
        {
            var source = AudioSourceContainer.Source(audioSourceId);
            source.PlayOneShot(clip);
        }
    }
}