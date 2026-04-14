using Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginSoundNotify.Forms
{
    public partial class FormSettings : Form
    {
        SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();
        GlobalPlaySoundSettings _settings;
        private PlaySoundSettings _selectedSoundSettings;

        public FormSettings()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormSettings(GlobalPlaySoundSettings settings) : this()
        {
            _settings = settings;

            comboBoxSoundType.Items.AddRange(Enum.GetNames(typeof(PlayEnum)));
            comboBoxVoices.Items.AddRange(GetVoices().ToArray());

            Bind();

        }

        IEnumerable<string> GetVoices()
        {

            foreach (var voice in _speechSynthesizer.GetInstalledVoices())
            {
                yield return voice.VoiceInfo.Description;
            }
        }

        private void Bind()
        {
            listBoxAvailableSoundsEvent.Items.AddRange(Enum.GetNames(typeof(AvailableSound)));
        }

        private void listBoxAvailableSoundsEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedSoundSettings = _settings.GetPlaySoundSettings((AvailableSound)Enum.Parse(typeof(AvailableSound), (string)listBoxAvailableSoundsEvent.SelectedItem));

            ApplySettings(_selectedSoundSettings);
        }

        private void ApplySettings(PlaySoundSettings settings)
        {
            var playTypeName = Enum.GetName(typeof(PlayEnum), settings.PlayType);

            var sel = comboBoxSoundType.Items.Cast<string>().FirstOrDefault(x => x.Equals(playTypeName));
            if (sel != null)
            {
                comboBoxSoundType.SelectedItem = null;
                comboBoxSoundType.SelectedItem = sel;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxSoundType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSoundType.SelectedItem == null) return;

            var playType = (PlayEnum)(Enum.Parse(typeof(PlayEnum), (string)comboBoxSoundType.SelectedItem));

            _selectedSoundSettings.PlayType = playType;

            switch (playType)
            {
                case PlayEnum.None:

                    panelSpeech.Visible = false;
                    panelFile.Visible = false;
                    panelMail.Visible = false;
                    break;

                case PlayEnum.PlayFile:

                    panelSpeech.Visible = false;
                    panelFile.Visible = true;
                    panelMail.Visible = false;
                    textBoxFile.Text = _selectedSoundSettings.FileName;

                    break;
                case PlayEnum.Speech:

                    panelSpeech.Visible = true;
                    panelFile.Visible = false;
                    panelMail.Visible = false;

                    textBox1.Text = _selectedSoundSettings.SpeechText;
                    if (string.IsNullOrEmpty(_selectedSoundSettings.SpeechVoiceName))
                        comboBoxVoices.SelectedIndex = -1;
                    else
                    {
                        var sel = comboBoxVoices.Items.Cast<string>()
                            .FirstOrDefault(x => x.Equals(_selectedSoundSettings.SpeechVoiceName));
                        if (sel == null)
                        {
                            comboBoxVoices.SelectedIndex = -1;
                        }
                        else
                        {
                            comboBoxVoices.SelectedItem = sel;
                        }
                    }
                    break;
                case PlayEnum.Mail:
                    panelSpeech.Visible = false;
                    panelFile.Visible = false;
                    panelMail.Visible = true;

                    textBoxMails.Text = _selectedSoundSettings.Emails;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = openFileDialog1.FileName;
                _selectedSoundSettings.FileName = openFileDialog1.FileName;

            }
        }

        private void comboBoxVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedSoundSettings.SpeechVoiceName = (string)comboBoxVoices.SelectedItem;
        }

        //private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    _selectedSoundSettings.SpeechText = textBox1.Text;
        //}

        private void buttonPlayFile_Click(object sender, EventArgs e)
        {
            PlayFile();
        }

        private void buttonPlaySpeech_Click(object sender, EventArgs e)
        {
            PlaySpeech();
        }

        private void PlaySpeech()
        {
            var voice = _speechSynthesizer.GetInstalledVoices().FirstOrDefault(x => x.VoiceInfo.Description.Equals(_selectedSoundSettings.SpeechVoiceName));
            if (voice != null)
            {
                if (!string.IsNullOrEmpty(_selectedSoundSettings.SpeechText))
                {
                    _speechSynthesizer.SelectVoice(voice.VoiceInfo.Name);

                    _speechSynthesizer.SpeakAsync(_selectedSoundSettings.SpeechText);

                }

            }
        }


        void PlayFile()
        {
            Task.Run(() =>
                {
                    var wplayer = new WMPLib.WindowsMediaPlayer { URL = _selectedSoundSettings.FileName };
                    wplayer.controls.play();
                }).ConfigureAwait(false);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            _selectedSoundSettings.SpeechText = textBox1.Text;
        }

        private void textBoxMails_KeyUp(object sender, KeyEventArgs e)
        {
            _selectedSoundSettings.Emails = textBoxMails.Text;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            _speechSynthesizer?.Dispose();
        }
    }
}
