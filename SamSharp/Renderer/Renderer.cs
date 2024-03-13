using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SamSharp.Renderer
{
    public partial class Renderer
    {
        private class Formants
        {
            public Dictionary<int, int> Mouth { get; set; }
            public Dictionary<int, int> Throat { get; set; }
            public Dictionary<int, int> Formant3 { get; set; }

            public Formants()
            {
                Mouth = new Dictionary<int, int>();
                Throat = new Dictionary<int, int>();
                Formant3 = new Dictionary<int, int>();
            }
        }
        
                
        private class FramesData
        {
            public Dictionary<int, int> Pitches { get; set; }
            public Formants Frequencies { get; set; }
            public Formants Amplitudes { get; set; }
            public Dictionary<int, int> SampledConsonantFlags { get; set; }
            
            public int T { get; set; }

            public FramesData()
            {
                Pitches = new Dictionary<int, int>();
                Frequencies = new Formants();
                Amplitudes = new Formants();
                SampledConsonantFlags = new Dictionary<int, int>();
            }
        }

        /// <summary>
        /// Renders audio from an array of phoneme data. 
        /// </summary>
        /// <param name="phonemes">The phoneme data output by the parser.</param>
        /// <param name="options">Speech options such as pitch, mouth/throat, speed and sing mode.</param>
        /// <returns>A byte buffer with audio data.</returns>
        public byte[] Render(Parser.Parser.PhonemeData[] phonemes, Options options)
        {
            var sentences = PrepareFrames(phonemes, options);

            var output = new OutputBuffer(
                (int)(176.4f  // 22050 / 125
                * phonemes.Sum(data => data.Length!.Value)
                * options.Speed)
            );
            
            PrintOutput(sentences);
            
            ProcessFrames(output, sentences.T, options.Speed, sentences);

            return output.Get();
        }

        private void PrintOutput(FramesData framesData)
        {
            System.Text.StringBuilder sb = new();
            sb.AppendLine("===============================================");
            sb.AppendLine("Final data for speech output:");
            sb.AppendLine("flags ampl1 freq1 ampl2 freq2 ampl3 freq3 pitch");
            sb.AppendLine("-----------------------------------------------");
            for (int i = 0; i < framesData.SampledConsonantFlags.Count; i++)
            {
                sb.AppendLine($" {framesData.SampledConsonantFlags[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Amplitudes.Mouth[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Frequencies.Mouth[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Amplitudes.Throat[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Frequencies.Throat[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Amplitudes.Formant3[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Frequencies.Formant3[i].ToString().PadLeft(5, '0')}" +
                              $" {framesData.Pitches[i].ToString().PadLeft(5, '0')}");
            }
            sb.AppendLine("===============================================");
            UnityEngine.Debug.Log(sb.ToString());
        }
    }
}