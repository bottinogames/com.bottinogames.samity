#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Samity
{
    [CustomEditor(typeof(SamVoice))]
    internal class SamVoiceEditor : Editor
    {
        private new SamVoice target;

        private Preset currentPreset = Preset.Custom;

        private void OnEnable()
        {
            target = (SamVoice)base.target;
            if (settingsToPresets.TryGetValue((target.Pitch, target.Mouth, target.Throat, target.Speed), out Preset preset))
                currentPreset = preset;
        }

        public override void OnInspectorGUI()
        {
            Preset preset = (Preset)EditorGUILayout.EnumPopup("Preset", currentPreset);
            if (preset != currentPreset)
            {
                currentPreset = preset;
                foreach (var kvp in settingsToPresets)
                {
                    if (kvp.Value == currentPreset)
                    {
                        Undo.RecordObject(target, "Apply SamVoice Preset");
                        target.Pitch = kvp.Key.p;
                        target.Mouth = kvp.Key.m;
                        target.Throat = kvp.Key.t;
                        target.Speed = kvp.Key.s;
                        continue;
                    }
                }
            }

            base.OnInspectorGUI();
        }

        private string previewText = "Hello, my name is SAM!";

        public override bool HasPreviewGUI() => true;
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            const float PADDING = 5f;

            r.x += PADDING;
            r.y += PADDING;
            r.height -= PADDING * 2f;
            r.width -= PADDING * 2f;

            Rect textRect = new Rect(r.x, r.y, r.width, r.height - 25f);
            previewText = EditorGUI.TextArea(textRect, previewText);

            Rect buttonRect = new Rect(r.x, textRect.yMax + 5f, r.width, 20f);
            if (GUI.Button(buttonRect, "Listen"))
            {
                var clip = target.GetClip(previewText, false);
                AudioUtil.PlayClipPreview(clip);
            }
        }




        private enum Preset
        {
            SAM,
            Elf,
            LittleRobot,
            StuffyGuy,
            LittleOldLady,
            ExtraTerrestrial,
            Custom
        }

        private Dictionary<(byte p, byte m, byte t, byte s), Preset> settingsToPresets = new Dictionary<(byte p, byte m, byte t, byte s), Preset>(
                new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>[] {
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((64, 128, 128, 72), Preset.SAM),
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((64, 160, 110, 72), Preset.Elf),
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((60, 190, 190, 92), Preset.LittleRobot),
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((72, 105, 105, 82), Preset.StuffyGuy),
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((32, 145, 145, 82), Preset.LittleOldLady),
                    new KeyValuePair<(byte p, byte m, byte t, byte s), Preset>((64, 150, 200, 100), Preset.ExtraTerrestrial)
                }
            );
    }
}
#endif