using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginTelegramNotify
{
    public partial class FormSettings : Form
    {
        public TelegramNotifySettings Settings { get; set; }

        public FormSettings(TelegramNotifySettings settings)
        {
            InitializeComponent();
            Settings = settings;
            DialogResult = DialogResult.Cancel;
            tb_bot_id.Text = Settings.BotId;
            tb_chat_id.Text = Settings.ChatId;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            var chatId = tb_chat_id.Text.Trim();
            var botId = tb_bot_id.Text.Trim();
            if (string.IsNullOrEmpty(chatId) || string.IsNullOrEmpty(botId))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Settings.ChatId = chatId;
            Settings.BotId = botId;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
