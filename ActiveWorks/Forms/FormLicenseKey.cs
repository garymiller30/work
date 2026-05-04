using System;
using System.Drawing;
using System.Windows.Forms;

namespace ActiveWorks.Forms
{
    public sealed class FormLicenseKey : Form
    {
        private readonly TextBox _textBoxLicenseKey;

        public FormLicenseKey(string licenseKey)
        {
            Text = "Ключ ліцензії";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 150);
            Font = SystemFonts.MessageBoxFont;

            var label = new Label
            {
                AutoSize = true,
                Location = new Point(12, 16),
                Text = "Введіть ключ ліцензії:"
            };

            _textBoxLicenseKey = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(15, 42),
                Size = new Size(410, 23),
                Text = licenseKey ?? string.Empty
            };

            var hint = new Label
            {
                AutoSize = false,
                Location = new Point(15, 70),
                Size = new Size(410, 34),
                Text = "Ключ буде збережено в локальному профілі користувача і використано для активації підписки.",
                ForeColor = SystemColors.GrayText
            };

            var buttonOk = new Button
            {
                DialogResult = DialogResult.OK,
                Location = new Point(264, 112),
                Size = new Size(78, 26),
                Text = "OK"
            };

            var buttonCancel = new Button
            {
                DialogResult = DialogResult.Cancel,
                Location = new Point(348, 112),
                Size = new Size(78, 26),
                Text = "Скасувати"
            };

            AcceptButton = buttonOk;
            CancelButton = buttonCancel;

            Controls.Add(label);
            Controls.Add(_textBoxLicenseKey);
            Controls.Add(hint);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
        }

        public string LicenseKey => _textBoxLicenseKey.Text.Trim();
    }
}
