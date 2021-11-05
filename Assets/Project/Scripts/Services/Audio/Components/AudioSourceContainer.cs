using System.Collections.Generic;
using Common.Components;
using UnityEngine;

namespace Services.Audio.Components
{
    public class AudioSourceContainer : Singleton<AudioSourceContainer>
    {
        [SerializeField] private List<AudioSource> sources;

        protected override void BeforeRegister()
        {
            SetSettings(isDestroyWhtiGameObject: true);
        }

        public static void StopAll()
        {
            foreach (var source in Instance.sources)
            {
                source.Stop();
            }
        }

        public static AudioSource Source(int id)
        {
            return Instance.sources[id];
        }
    }
}