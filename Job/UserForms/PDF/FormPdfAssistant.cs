using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using Interfaces.Licensing;
using BackgroundTaskServiceLib;
using JobSpace.UC;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfAssistant : Form
    {
        private readonly PdfJobContext _context;
        private readonly List<ToolInfo> _availableTools;
        private readonly PdfAssistantService _assistantService;
        private PdfAssistantSettings _settings;
        private ToolInfo _suggestedTool;

        public FormPdfAssistant(PdfJobContext context, List<ToolInfo> availableTools)
        {
            InitializeComponent();
            _context = context;
            _availableTools = availableTools;
            _assistantService = new PdfAssistantService();

            _settings = _context.UserProfile.LoadSettings<PdfAssistantSettings>() ?? new PdfAssistantSettings();
            
            panelSuggestedTool.Visible = false;
            lblSelectedFiles.Text = $"Вибрано файлів: {_context.ProcessingFiles.Count}";
            
            AppendMessage("Асистент", "Привіт! Я твій асистент з PDF-утиліт. Опиши задачу, яку тобі потрібно виконати з вибраними файлами.", Color.DarkBlue);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userText = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userText)) return;

            AppendMessage("Ви", userText, Color.Black);
            txtInput.Clear();
            btnSend.Enabled = false;

            AppendMessage("Система", "Асистент думає...", Color.Gray);

            string reply = await _assistantService.AskAssistantAsync(
                _settings, 
                userText, 
                _availableTools, 
                _context.ProcessingFiles
            );

            RemoveLastMessage();

            ParseAssistantReply(reply);
            btnSend.Enabled = true;
        }

        private void ParseAssistantReply(string reply)
        {
            _suggestedTool = null;
            panelSuggestedTool.Visible = false;

            var match = Regex.Match(reply, @"\[SUGGESTED_TOOL:\s*([a-zA-Z0-9\._]+)\]");
            string cleanReply = reply;

            if (match.Success)
            {
                string toolId = match.Groups[1].Value.Trim();
                _suggestedTool = _availableTools.FirstOrDefault(t => 
                    t.ToolType.FullName.Equals(toolId, StringComparison.InvariantCultureIgnoreCase) ||
                    t.ToolType.Name.Equals(toolId, StringComparison.InvariantCultureIgnoreCase));
                
                cleanReply = reply.Replace(match.Value, "").Trim();
            }

            AppendMessage("Асистент", cleanReply, Color.DarkGreen);

            if (_suggestedTool != null)
            {
                lblToolSuggestion.Text = $"Пропоную запустити: {_suggestedTool.Meta.Name}";
                panelSuggestedTool.Visible = true;
            }
        }

        private async void btnRunTool_Click(object sender, EventArgs e)
        {
            if (_suggestedTool == null) return;

            var toolInfo = _suggestedTool;

            if (!LicenseFeatureGate.RequireFor(toolInfo.ToolType, out _))
            {
                MessageBox.Show(
                    this,
                    $"Функція \"{toolInfo.Meta.Name}\" доступна тільки з активною підпискою.",
                    "Потрібна підписка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var tool = toolInfo.Create();

            bool configured = false;
            if (tool is IPdfToolAsync toolAsync)
            {
                configured = await toolAsync.ConfigureAsync(_context);
            }
            else
            {
                configured = tool.Configure(_context);
            }

            if (!configured) return;

            if (toolInfo.Meta.IsBackgroundTask)
            {
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask(
                    toolInfo.Meta.MenuPath, 
                    new Action(() => tool.Execute(_context)), 
                    _context.ProcessingFiles));
            }
            else
            {
                tool.Execute(_context);
            }

            this.Close();
        }

        private void AppendMessage(string sender, string message, Color color)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.SelectionColor = color;
            rtbChat.AppendText($"{sender}: ");
            
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.SelectionColor = Color.Black;
            rtbChat.AppendText($"{message}{Environment.NewLine}{Environment.NewLine}");
            
            rtbChat.ScrollToCaret();
        }

        private void RemoveLastMessage()
        {
            int lastIndex = rtbChat.Text.LastIndexOf("Система: Асистент думає...");
            if (lastIndex >= 0)
            {
                rtbChat.Select(lastIndex, rtbChat.Text.Length - lastIndex);
                rtbChat.SelectedText = "";
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new FormPdfAssistantSettings(_settings))
            {
                if (settingsForm.ShowDialog(this) == DialogResult.OK)
                {
                    _settings = settingsForm.Settings;
                    _context.UserProfile.SaveSettings(_settings);
                }
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // prevent beep sound
                btnSend.PerformClick();
            }
        }
    }
}
