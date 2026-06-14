using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackgroundTaskServiceLib;
using Interfaces.FileBrowser;
using Interfaces.Licensing;
using Interfaces.Plugins;

namespace JobSpace.UserForms.PDF
{
    public partial class FormPdfAssistant : Form
    {
        // ─── State ─────────────────────────────────────────────────────────────
        private readonly PdfJobContext _context;
        private readonly List<ToolInfo> _availableTools;
        private readonly PdfAssistantService _assistantService;
        private readonly AudioRecorder _recorder = new AudioRecorder();
        private PdfAssistantSettings _settings;
        private ToolInfo? _suggestedTool;

        // In-memory chat messages for re-render
        private readonly List<(string Role, string Markdown)> _messages = new();

        // Temp file path for voice recording
        private string? _lastWavPath;

        // ─── HTML shell (loaded once, messages appended via JS) ─────────────
        private const string HtmlShell = @"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<meta name='viewport' content='width=device-width,initial-scale=1'>
<script src='https://cdn.jsdelivr.net/npm/marked/marked.min.js'></script>
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    font-family: 'Segoe UI', system-ui, sans-serif;
    font-size: 14px;
    background: #1e1e2e;
    color: #cdd6f4;
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    padding: 12px;
    gap: 10px;
  }
  .msg { display: flex; flex-direction: column; gap: 2px; max-width: 92%; }
  .msg.assistant { align-self: flex-start; }
  .msg.user       { align-self: flex-end; }
  .msg.system     { align-self: center; opacity: 0.55; font-size: 12px; }
  .bubble {
    border-radius: 14px;
    padding: 10px 14px;
    line-height: 1.6;
    word-break: break-word;
  }
  .assistant .bubble { background: #313244; border-bottom-left-radius: 4px; }
  .user       .bubble { background: #89b4fa; color: #1e1e2e; border-bottom-right-radius: 4px; }
  .system     .bubble { background: #45475a; font-style: italic; border-radius: 8px; }
  .role { font-size: 11px; font-weight: 600; color: #a6adc8; padding: 0 4px; }
  .assistant .role { color: #89dceb; }
  .user       .role { color: #89b4fa; text-align: right; }

  /* Markdown inside bubbles */
  .bubble h1,.bubble h2,.bubble h3 { margin-top: 6px; margin-bottom: 4px; }
  .bubble p  { margin: 4px 0; }
  .bubble ul,.bubble ol { padding-left: 18px; margin: 4px 0; }
  .bubble code { background: rgba(0,0,0,0.3); border-radius: 4px; padding: 1px 5px; font-size: 12px; }
  .bubble pre  { background: #11111b; border-radius: 8px; padding: 10px; overflow-x: auto; margin: 6px 0; }
  .bubble pre code { background: none; padding: 0; }
  .bubble strong { color: #f5c2e7; }
  .bubble a { color: #89dceb; }

  .thinking { display: flex; gap: 5px; align-items: center; padding: 10px 14px; }
  .dot { width: 8px; height: 8px; border-radius: 50%; background: #89dceb;
         animation: bounce 1.2s infinite ease-in-out; }
  .dot:nth-child(2) { animation-delay: 0.2s; }
  .dot:nth-child(3) { animation-delay: 0.4s; }
  @keyframes bounce { 0%,80%,100%{ transform:scale(0); } 40%{ transform:scale(1); } }
</style>
</head>
<body id='chat'></body>
<script>
marked.setOptions({ breaks: true, gfm: true });

function appendMsg(role, mdText) {
  const chat = document.getElementById('chat');
  const wrapper = document.createElement('div');
  wrapper.className = 'msg ' + role;
  const lbl  = document.createElement('div');
  lbl.className = 'role';
  lbl.textContent = role === 'user' ? 'Ви' : role === 'assistant' ? 'Асистент' : 'Система';
  const bub  = document.createElement('div');
  bub.className = 'bubble';
  bub.innerHTML = marked.parse(mdText);
  wrapper.appendChild(lbl);
  wrapper.appendChild(bub);
  chat.appendChild(wrapper);
  window.scrollTo(0, document.body.scrollHeight);
}

function showThinking() {
  const chat = document.getElementById('chat');
  const el = document.createElement('div');
  el.id = 'thinking';
  el.className = 'msg assistant';
  el.innerHTML = '<div class=""bubble""><div class=""thinking""><div class=""dot""></div><div class=""dot""></div><div class=""dot""></div></div></div>';
  chat.appendChild(el);
  window.scrollTo(0, document.body.scrollHeight);
}

function hideThinking() {
  const el = document.getElementById('thinking');
  if (el) el.remove();
}
</script>
</html>";

        // ─── Constructor ────────────────────────────────────────────────────────
        public FormPdfAssistant(PdfJobContext context, List<ToolInfo> availableTools)
        {
            InitializeComponent();
            _context = context;
            _availableTools = availableTools;
            _assistantService = new PdfAssistantService();
            _settings = _context.UserProfile.LoadSettings<PdfAssistantSettings>() ?? new PdfAssistantSettings();

            panelSuggestedTool.Visible = false;
            lblSelectedFiles.Text = $"🗂  Вибрано файлів: {_context.ProcessingFiles.Count}";

            // Init WebView2 asynchronously
            _ = InitWebViewAsync();
        }

        private async Task InitWebViewAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Settings.IsStatusBarEnabled = false;
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView21.NavigateToString(HtmlShell);
            webView21.NavigationCompleted += async (s, e) =>
            {
                // Greet message once loaded
                await AppendMessageAsync("assistant",
                    "Привіт! 👋 Я твій **AI-асистент** з PDF-утиліт.  \nОпиши задачу — я підберу найкращий інструмент.");
            };
        }

        // ─── Send message ───────────────────────────────────────────────────────
        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userText = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userText)) return;

            txtInput.Clear();
            btnSend.Enabled = false;
            btnRecord.Enabled = false;

            await AppendMessageAsync("user", userText);
            await ShowThinkingAsync();

            string reply = await _assistantService.AskAssistantAsync(
                _settings, userText, _availableTools, _context.ProcessingFiles);

            await HideThinkingAsync();
            await ParseAndShowReplyAsync(reply);

            btnSend.Enabled = true;
            btnRecord.Enabled = true;
            txtInput.Focus();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSend.PerformClick();
            }
        }

        // ─── Voice input ────────────────────────────────────────────────────────
        private async void btnRecord_Click(object sender, EventArgs e)
        {
            if (_recorder.IsRecording)
            {
                // Stop — save — transcribe
                btnRecord.BackColor = System.Drawing.Color.WhiteSmoke;
                btnRecord.Text = "🎙";
                btnRecord.Enabled = false;

                // Зберігаємо в папку запуску програми, тут назва файлу буде суто з англійських літер і цифр
                string tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                _lastWavPath = Path.Combine(tempDir, $"rec_{Guid.NewGuid():N}.wav");

                _recorder.StopAndSave(_lastWavPath);

                await ShowThinkingAsync();
                string transcript = await _assistantService.TranscribeAudioAsync(_settings, _lastWavPath);
                await HideThinkingAsync();

                try { if (_lastWavPath != null) File.Delete(_lastWavPath); } catch { }

                if (!string.IsNullOrWhiteSpace(transcript) && !transcript.StartsWith("Помилка"))
                {
                    txtInput.Text = transcript;
                    btnSend.PerformClick();
                }
                else
                {
                    await AppendMessageAsync("system", $"⚠ {transcript}");
                }
                btnRecord.Enabled = true;
            }
            else
            {
                // Start recording
                _recorder.StartRecording();
                btnRecord.BackColor = System.Drawing.Color.OrangeRed;
                btnRecord.Text = "⏹";
                await AppendMessageAsync("system", "🎙 Запис... Натисніть ⏹ щоб зупинити.");
            }
        }

        // ─── Parse LLM reply ────────────────────────────────────────────────────
        private async Task ParseAndShowReplyAsync(string reply)
        {
            _suggestedTool = null;
            panelSuggestedTool.Visible = false;

            var match = Regex.Match(reply, @"\[SUGGESTED_TOOL:\s*([a-zA-Z0-9\._]+)\]");
            string cleanReply = reply;

            if (match.Success)
            {
                string toolId = match.Groups[1].Value.Trim();
                _suggestedTool = _availableTools.FirstOrDefault(t =>
                    string.Equals(t.ToolType.FullName, toolId, StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(t.ToolType.Name, toolId, StringComparison.InvariantCultureIgnoreCase));
                cleanReply = reply.Replace(match.Value, "").Trim();
            }

            await AppendMessageAsync("assistant", cleanReply);

            if (_suggestedTool != null)
            {
                lblToolSuggestion.Text = $"✅  Пропоную запустити: {_suggestedTool.Meta.Name}";
                panelSuggestedTool.Visible = true;
            }
        }

        // ─── Run suggested tool ─────────────────────────────────────────────────
        private async void btnRunTool_Click(object sender, EventArgs e)
        {
            if (_suggestedTool == null) return;

            if (!LicenseFeatureGate.RequireFor(_suggestedTool.ToolType, out _))
            {
                MessageBox.Show(this,
                    $"Функція \"{_suggestedTool.Meta.Name}\" доступна тільки з активною підпискою.",
                    "Потрібна підписка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tool = _suggestedTool.Create();

            bool configured = tool is IPdfToolAsync async
                ? await async.ConfigureAsync(_context)
                : tool.Configure(_context);

            if (!configured) return;

            if (_suggestedTool.Meta.IsBackgroundTask)
                BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask(
                    _suggestedTool.Meta.MenuPath,
                    new Action(() => tool.Execute(_context)),
                    _context.ProcessingFiles));
            else
                tool.Execute(_context);

            Close();
        }

        // ─── Settings ───────────────────────────────────────────────────────────
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using var dlg = new FormPdfAssistantSettings(_settings);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _settings = dlg.Settings;
                _context.UserProfile.SaveSettings(_settings);
            }
        }

        // ─── WebView2 JS helpers ────────────────────────────────────────────────
        private async Task AppendMessageAsync(string role, string markdown)
        {
            _messages.Add((role, markdown));
            string escaped = EscapeForJs(markdown);
            await webView21.ExecuteScriptAsync($"appendMsg('{role}', `{escaped}`);");
        }

        private async Task ShowThinkingAsync()
            => await webView21.ExecuteScriptAsync("showThinking();");

        private async Task HideThinkingAsync()
            => await webView21.ExecuteScriptAsync("hideThinking();");

        /// <summary>Escapes a Markdown string so it can be placed inside a JS template literal.</summary>
        private static string EscapeForJs(string text)
        {
            // In a template literal: backticks and backslashes must be escaped
            return text
                .Replace("\\", "\\\\")
                .Replace("`", "\\`")
                .Replace("$", "\\$")
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");
        }
    }
}
