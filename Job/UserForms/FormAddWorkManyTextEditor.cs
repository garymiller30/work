using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    internal sealed class FormAddWorkManyTextEditor : KryptonForm
    {
        private const string VisibleTab = "\u2192";

        private readonly RichTextBox _textBox;
        private readonly KryptonButton _buttonOk;
        private readonly KryptonButton _buttonCancel;
        private readonly List<SelectionRange> _rectangularSelection = new List<SelectionRange>();
        private bool _isRectangularSelecting;
        private int _selectionStartLine;
        private int _selectionStartColumn;

        public string EditText { get; private set; } = string.Empty;

        public FormAddWorkManyTextEditor()
        {
            Text = "Редагувати список замовлень";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            Size = new Size(820, 520);

            _textBox = new RichTextBox
            {
                AcceptsTab = true,
                BorderStyle = BorderStyle.FixedSingle,
                DetectUrls = false,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericMonospace, 10F),
                HideSelection = false,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                WordWrap = false
            };

            _buttonOk = new KryptonButton
            {
                DialogResult = DialogResult.OK,
                Margin = new Padding(6, 10, 6, 10),
                Size = new Size(88, 30),
            };
            _buttonOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _buttonOk.Values.Text = "OK";
            _buttonOk.Click += ButtonOk_Click;

            _buttonCancel = new KryptonButton
            {
                DialogResult = DialogResult.Cancel,
                Margin = new Padding(6, 10, 10, 10),
                Size = new Size(88, 30),
            };
            _buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _buttonCancel.Values.Text = "Скасувати";

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                Width = 210
            };
            buttonsPanel.Controls.Add(_buttonOk);
            buttonsPanel.Controls.Add(_buttonCancel);

            var bottomPanel = new KryptonPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            bottomPanel.Controls.Add(buttonsPanel);

            Controls.Add(_textBox);
            Controls.Add(bottomPanel);
            AcceptButton = _buttonOk;
            CancelButton = _buttonCancel;
            DialogResult = DialogResult.Cancel;

            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.KeyPress += TextBox_KeyPress;
            _textBox.MouseDown += TextBox_MouseDown;
            _textBox.MouseMove += TextBox_MouseMove;
            _textBox.MouseUp += TextBox_MouseUp;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            EditText = ToRawText(_textBox.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                InsertText(ToVisibleText(Clipboard.GetText()));
                e.SuppressKeyPress = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.C && _rectangularSelection.Count > 0)
            {
                Clipboard.SetText(ToRawText(GetRectangularSelectedText()));
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Tab)
            {
                InsertText(VisibleTab);
                e.SuppressKeyPress = true;
                return;
            }

            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && _rectangularSelection.Count > 0)
            {
                DeleteRectangularSelection();
                e.SuppressKeyPress = true;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || _rectangularSelection.Count == 0)
            {
                return;
            }

            ReplaceRectangularSelection(e.KeyChar.ToString());
            e.Handled = true;
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control || e.Button != MouseButtons.Left)
            {
                ClearRectangularSelection();
                return;
            }

            var location = GetLineColumnFromPoint(e.Location);
            _selectionStartLine = location.Line;
            _selectionStartColumn = location.Column;
            _isRectangularSelecting = true;
            UpdateRectangularSelection(e.Location);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isRectangularSelecting)
            {
                UpdateRectangularSelection(e.Location);
            }
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isRectangularSelecting)
            {
                UpdateRectangularSelection(e.Location);
                _isRectangularSelecting = false;
            }
        }

        private void InsertText(string text)
        {
            ClearRectangularSelection();
            _textBox.SelectedText = text;
        }

        private void UpdateRectangularSelection(Point point)
        {
            ClearRectangularSelection();

            var location = GetLineColumnFromPoint(point);
            var firstLine = Math.Min(_selectionStartLine, location.Line);
            var lastLine = Math.Max(_selectionStartLine, location.Line);
            var firstColumn = Math.Min(_selectionStartColumn, location.Column);
            var lastColumn = Math.Max(_selectionStartColumn, location.Column);

            if (firstColumn == lastColumn)
            {
                return;
            }

            var savedStart = _textBox.SelectionStart;
            var savedLength = _textBox.SelectionLength;

            for (var line = firstLine; line <= lastLine; line++)
            {
                var range = GetRange(line, firstColumn, lastColumn);
                if (range.Length == 0)
                {
                    continue;
                }

                _rectangularSelection.Add(range);
                _textBox.Select(range.Start, range.Length);
                _textBox.SelectionBackColor = SystemColors.Highlight;
                _textBox.SelectionColor = SystemColors.HighlightText;
            }

            _textBox.Select(savedStart, savedLength);
        }

        private void ClearRectangularSelection()
        {
            if (_rectangularSelection.Count == 0)
            {
                return;
            }

            var savedStart = _textBox.SelectionStart;
            var savedLength = _textBox.SelectionLength;

            foreach (var range in _rectangularSelection)
            {
                _textBox.Select(range.Start, range.Length);
                _textBox.SelectionBackColor = _textBox.BackColor;
                _textBox.SelectionColor = _textBox.ForeColor;
            }

            _rectangularSelection.Clear();
            _textBox.Select(Math.Min(savedStart, _textBox.TextLength), Math.Min(savedLength, Math.Max(0, _textBox.TextLength - savedStart)));
        }

        private string GetRectangularSelectedText()
        {
            return string.Join(
                Environment.NewLine,
                _rectangularSelection
                    .OrderBy(range => range.Start)
                    .Select(range => _textBox.Text.Substring(range.Start, range.Length)));
        }

        private void DeleteRectangularSelection()
        {
            var caret = _rectangularSelection.Count > 0 ? _rectangularSelection[0].Start : _textBox.SelectionStart;
            foreach (var range in _rectangularSelection.OrderByDescending(range => range.Start))
            {
                _textBox.Select(range.Start, range.Length);
                _textBox.SelectedText = string.Empty;
            }

            _rectangularSelection.Clear();
            _textBox.Select(Math.Min(caret, _textBox.TextLength), 0);
        }

        private void ReplaceRectangularSelection(string text)
        {
            var ranges = _rectangularSelection.OrderByDescending(range => range.Start).ToList();
            var caret = ranges.Count > 0 ? ranges[ranges.Count - 1].Start + text.Length : _textBox.SelectionStart;

            foreach (var range in ranges)
            {
                _textBox.Select(range.Start, range.Length);
                _textBox.SelectedText = text;
            }

            _rectangularSelection.Clear();
            _textBox.Select(Math.Min(caret, _textBox.TextLength), 0);
        }

        private SelectionRange GetRange(int line, int firstColumn, int lastColumn)
        {
            var lineStart = _textBox.GetFirstCharIndexFromLine(line);
            if (lineStart < 0)
            {
                return SelectionRange.Empty;
            }

            var lineText = GetLineText(line);
            var startColumn = Math.Min(firstColumn, lineText.Length);
            var endColumn = Math.Min(lastColumn, lineText.Length);
            return endColumn <= startColumn
                ? SelectionRange.Empty
                : new SelectionRange(lineStart + startColumn, endColumn - startColumn);
        }

        private (int Line, int Column) GetLineColumnFromPoint(Point point)
        {
            var index = _textBox.GetCharIndexFromPosition(point);
            var line = _textBox.GetLineFromCharIndex(index);
            var lineStart = _textBox.GetFirstCharIndexFromLine(line);
            return (line, Math.Max(0, index - lineStart));
        }

        private string GetLineText(int line)
        {
            if (line < 0 || line >= _textBox.Lines.Length)
            {
                return string.Empty;
            }

            return _textBox.Lines[line];
        }

        private static string ToVisibleText(string text)
        {
            return (text ?? string.Empty).Replace("\t", VisibleTab);
        }

        private static string ToRawText(string text)
        {
            return (text ?? string.Empty).Replace(VisibleTab, "\t");
        }

        private readonly struct SelectionRange
        {
            public static readonly SelectionRange Empty = new SelectionRange(0, 0);

            public SelectionRange(int start, int length)
            {
                Start = start;
                Length = length;
            }

            public int Start { get; }
            public int Length { get; }
        }
    }
}
