using System.Collections.Generic;
using Common.Components;
using UnityEngine;

namespace Services.Audio.Components
{
    public class GlobalAudioClipContainer : Singleton<GlobalAudioClipContainer>
    {
        [SerializeField] private List<AudioClip> clips;

        protected override void BeforeRegister()
        {
            SetSettings(isDestroyWhtiGameObject: true);
        }

        public static AudioClip Clip(int id)
        {
            return Instance.clips[id];
        }
    }
}