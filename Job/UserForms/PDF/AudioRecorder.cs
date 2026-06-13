using System;
using System.Runtime.InteropServices;
using System.Text;

namespace JobSpace.UserForms.PDF
{
    public class AudioRecorder
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        private bool _isRecording = false;

        public bool IsRecording => _isRecording;

        public void StartRecording()
        {
            if (_isRecording) return;

            // Open waveaudio device
            mciSendString("open new Type waveaudio Alias recsound", null, 0, IntPtr.Zero);
            
            // Set audio settings (16-bit, 16kHz, Mono - standard for speech recognition)
            mciSendString("set recsound bitspersample 16 samplespersec 16000 channels 1 alignment 2 bytespersec 32000", null, 0, IntPtr.Zero);
            
            // Start recording
            mciSendString("record recsound", null, 0, IntPtr.Zero);
            
            _isRecording = true;
        }

        public void StopAndSave(string filePath)
        {
            if (!_isRecording) return;

            // Stop recording
            mciSendString("stop recsound", null, 0, IntPtr.Zero);
            
            // Save recording to file
            mciSendString($"save recsound \"{filePath}\"", null, 0, IntPtr.Zero);
            
            // Close device
            mciSendString("close recsound", null, 0, IntPtr.Zero);
            
            _isRecording = false;
        }
    }
}
