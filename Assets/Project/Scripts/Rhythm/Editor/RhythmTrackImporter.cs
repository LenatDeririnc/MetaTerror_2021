using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Rhythm
{
    [ScriptedImporter(1, new []{ "mid", "midi" })]
    public class RhythmTrackImporter : ScriptedImporter
    {
        public NoteRemap[] remaps;
        public AudioClip goodAudioClip;
        public AudioClip badAudioClip;
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var midi = MidiFile.Read(ctx.assetPath);
            var notes = midi.GetNotes();
            var tempoMap = midi.GetTempoMap();
            var asset = ScriptableObject.CreateInstance<RhythmTrack>();
            var notesList = new List<RhythmTrack.Note>();
            var noteDictionary = remaps.ToDictionary(
                r => r.midiNote, 
                r => r.gameChannel);

            foreach (var note in notes)
            {
                var noteOffTime = note.GetTimedNoteOffEvent().TimeAs<MetricTimeSpan>(tempoMap);
                var noteOnTime = note.GetTimedNoteOnEvent().TimeAs<MetricTimeSpan>(tempoMap);

                if (noteDictionary.TryGetValue(note.NoteNumber, out var result))
                {
                    notesList.Add(new RhythmTrack.Note
                    {
                        onTime = (float) ((TimeSpan) noteOnTime).TotalSeconds,
                        offTime = (float) ((TimeSpan) noteOffTime).TotalSeconds,
                        channel = result
                    });
                }
            }

            asset.trackNotes = notesList
                .OrderBy(n => n.onTime)
                .ToArray();

            asset.goodAudioClip = goodAudioClip;
            asset.badAudioClip = badAudioClip;
            
            ctx.AddObjectToAsset("Track", asset);
        }

        [Serializable]
        public struct NoteRemap
        {
            public int midiNote;
            public int gameChannel;
        }
    }
}