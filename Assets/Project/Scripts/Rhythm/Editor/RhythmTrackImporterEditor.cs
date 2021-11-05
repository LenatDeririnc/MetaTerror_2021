using System;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Standards;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Rhythm
{
    [CustomEditor(typeof(RhythmTrackImporter))]
    public class RhythmTrackImporterEditor : ScriptedImporterEditor
    {
        [NonSerialized]
        private string cachedMidiFilePath;
        
        [NonSerialized]
        private MidiFile cachedMidiFile;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var importer = (RhythmTrackImporter) target;

            if (cachedMidiFilePath != importer.assetPath)
            {
                cachedMidiFile = MidiFile.Read(importer.assetPath);
                cachedMidiFilePath = importer.assetPath;
            }
            
            var names = new StringBuilder();

            foreach (var note in cachedMidiFile.GetNotes()
                .DistinctBy(n => n.NoteNumber)
                .OrderBy(n => n.NoteNumber))
            {
                int number = note.NoteNumber;
                
                if (note.Channel == GeneralMidi.PercussionChannel)
                {
                    names.Append(number + " (" + (GeneralMidiPercussion) number + ")\n");
                }
                else
                {
                    names.Append(number + " (" + note.NoteName + ")\n");
                }
            }

            EditorGUILayout.LabelField("Укажи в списке выше маппинг между midi нотами\n" +
                                       "и игровыми каналами, например 47 нота -> 1 канал\n" +
                                       "Ноты которые юзаются в MIDI: \n" + names.ToString().TrimEnd('\n'), EditorStyles.helpBox);
        }
    }
}