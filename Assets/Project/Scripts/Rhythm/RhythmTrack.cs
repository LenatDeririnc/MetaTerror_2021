using System;
using UnityEngine;

namespace Rhythm
{
    [Serializable]
    public class RhythmTrack : ScriptableObject
    {
        public AudioClip goodAudioClip;
        public AudioClip badAudioClip;
        public Note[] trackNotes;
    
        [Serializable]
        public struct Note
        {
            public int channel;
            public float onTime;
            public float offTime;
        }
    }
}