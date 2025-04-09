// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using ExtensionMethods;
using MailNotifier.Shablons;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BackgroundTaskServiceLib;
using Krypton.Toolkit;

namespace MailNotifier
{
    public sealed partial class FormSendMail : KryptonForm
    {
        private readonly List<string> _attachList = new List<string>();
        private readonly Mail _mail;
        //readonly MailShablonManager _shablonsManager;

        private readonly List<string> _sendToList = new List<string>();

        public FormSendMail(Mail mail)
        {
            InitializeComponent();

            _mail = mail;

            CreateShablonMenuItems();

        }

        public string GetHeader() => textBoxHeader.Text;

        public void InitSendToList(IEnumerable<string> sendTo)
        {
            _sendToList.AddRange(sendTo);

            comboBoxTo.Items.Clear();
            comboBoxTo.Items.AddRange(_sendToList.ToArray());
            if (sendTo.Count() == 1)
            {
                comboBoxTo.SelectedIndex = 0;
            }
        }

        public void SetAttachmentList(IEnumerable<string> attach)
        {
            _attachList.Clear();
            _attachList.AddRange(attach);

            foreach (var att in attach)
            {
                listBoxAttach.Items.Add(new Attach(att));
            }

            SetAttachmentTotal();
        }

        private void SetAttachmentTotal()
        {
            if (listBoxAttach.Items.Count > 0)
            {
                var sum = listBoxAttach.Items.Cast<Attach>().Sum(x => x.Size);
                var inMb = sum / (1024*1024);
                labelTotal.Text = inMb.ToString("N01");

            }
        }

        internal void SetBody(string body)
        {
            if (body.TrimStart().StartsWith(@"{\rtf1", StringComparison.Ordinal))
            {
                richTextBoxMessage.Rtf = body;
            }
            else
            {
                richTextBoxMessage.Text = body;
            }
            
        }

        internal void SetHeader(string header)
        {
            textBoxHeader.Text = header;
        }

        internal void SetShablon(string shablonName)
        {
            var shablon = _mail.ShablonManager.GetShablons().FirstOrDefault(x => x.ShablonName.Equals(shablonName));
            if (shablon != null)
            {
                SetShablon(shablon);
            }
        }

        private void AddAttachFiles()
        {
            using (var d = new VistaOpenFileDialog())
            {
                d.CheckFileExists = true;
                d.Multiselect = true;

                if (d.ShowDialog() == DialogResult.OK)
                {
                    listBoxAttach.Items.Add(new Attach(d.FileName));
                    _attachList.Add(d.FileName);
                }
                SetAttachmentTotal();
            }
        }

        private void AddToolStripMenuItem(ToolStripMenuItem tsmi)
        {
            var menus = toolStripDropDownButtonMailShablons.DropDownItems.Find(((MailShablon)tsmi.Tag).ShablonName, false);
            if (!menus.Any())
            {
                toolStripDropDownButtonMailShablons.DropDownItems.Add(tsmi);
            }
        }

        private void ButtonAddAttach_Click(object sender, EventArgs e)
        => AddAttachFiles();

        private void ButtonRemoveAttach_Click(object sender, EventArgs e)
        => RemoveAttachFromList();

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            Hide();
            SendEmail();
            Close();
        }

        private void CreateShablonMenuItems()
        {
            var shablons = _mail.ShablonManager.GetShablons();

            ClearDropDownButtonMailShablons();

            if (shablons.Any())
            {
                foreach (MailShablon shablon in shablons)
                {
                    var tsmi = CreateToolStripMenuItem(shablon);
                   
                    toolStripDropDownButtonMailShablons.DropDownItems.Add(tsmi);
                }
            }
        }

        private void ClearDropDownButtonMailShablons()
        {
            if (toolStripDropDownButtonMailShablons.HasDropDownItems)
            {
                List<ToolStripMenuItem> deleteList = new List<ToolStripMenuItem>();

                foreach (var toolItem in toolStripDropDownButtonMailShablons.DropDownItems)
                {
                    if (toolItem is ToolStripMenuItem item && item.Tag != null)
                    {
                        deleteList.Add(item);
                    }
                }

                foreach (ToolStripMenuItem menuItem in deleteList)
                {
                    toolStripDropDownButtonMailShablons.DropDownItems.Remove(menuItem);
                }
            }

        }

        private void Del_Click(object sender, EventArgs e)
        {
            var shablon = (MailShablon)((ToolStripMenuItem)sender).Tag;
            _mail.ShablonManager.DeleteShablon(shablon);
            _mail.ShablonManager.Save();
            CreateShablonMenuItems();
        }

        private ToolStripMenuItem CreateToolStripMenuItem(MailShablon shablon)
        {
            var tsmi = new ToolStripMenuItem
            {
                Name = shablon.ShablonName,
                Text = shablon.ShablonName,
                Tag = shablon
            };
            tsmi.Click += Tsmi_Click;
            tsmi.MouseUp += Tsmi_MouseUp;

            {
                var del = new ToolStripMenuItem()
                {
                    Tag = shablon,
                    Image = Properties.Resources.Delete
                };
                del.Click += Del_Click;
                tsmi.DropDownItems.Add(del);
            }


            return tsmi;
        }

        private void RemoveAttachFromList()
        {
            if (listBoxAttach.SelectedItems.Count > 0)
            {
                var delList = listBoxAttach.SelectedItems.Cast<Attach>().ToList();

                foreach (var attach in delList)
                {
                    listBoxAttach.Items.Remove(attach);
                    _attachList.Remove(attach.FullPath);
                }
                
            }
            SetAttachmentTotal();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FormGetName())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var shablon = _mail.ShablonManager.AddShablon(f.ShablonName);

                    shablon.Header = textBoxHeader.Text;
                    shablon.SendTo = comboBoxTo.Text;
                    shablon.Message = richTextBoxMessage.Rtf;

                    _mail.ShablonManager.Save();

                    AddToolStripMenuItem(CreateToolStripMenuItem(shablon));
                }
            }
        }

        private void SendEmail()
        {
            if (string.IsNullOrEmpty(comboBoxTo.Text))
            {
                comboBoxTo.DroppedDown = true;
                return;
            }

            if (string.IsNullOrEmpty(textBoxHeader.Text))
            {
                if (MessageBox.Show(@"Header is empty. Send as is?", @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            var sendTo = comboBoxTo.Text;
            var header = textBoxHeader.Text;
            
            
            Debug.WriteLine(richTextBoxMessage.Rtf);//.
            var message = $"<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"></head><body>{RtfPipe.Rtf.ToHtml(richTextBoxMessage.Rtf)}";
            Debug.WriteLine(message);
            
            BackgroundTaskService.AddTask(BackgroundTaskService.CreateTask($"send mail to {sendTo}", new Action(
                () => _mail.SendToMany(sendTo, header, message, _attachList.ToArray())
                )));
        }

        private void SetShablon(MailShablon shablon)
        {
            

            if (!comboBoxTo.Items.Contains(shablon.SendTo))
            {
                comboBoxTo.Items.Add(shablon.SendTo);
            }

            comboBoxTo.Text = shablon.SendTo;
            textBoxHeader.Text = shablon.Header;
            richTextBoxMessage.Rtf = shablon.Message;
        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            var shablon = ((ToolStripMenuItem)sender).Tag as MailShablon;

            SetShablon(shablon);
        }

        private void Tsmi_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var shablon = ((ToolStripMenuItem)sender).Tag as MailShablon;

                if (MessageBox.Show("Delete?", ((ToolStripMenuItem)sender).Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    _mail.ShablonManager.DeleteShablon(shablon);

                    toolStripDropDownButtonMailShablons.DropDownItems.Remove((ToolStripMenuItem)sender);
                    _mail.ShablonManager.Save();
                }
            }
        }

        private class Attach
        {
            public Attach(string fullPath)
            {
                Size = new FileInfo(fullPath).Length;
                Name = $"{Path.GetFileName(fullPath)} ({Size.GetFileSizeInString()})";
                FullPath = fullPath;
            }

            public string FullPath { get; private set; }
            public string Name { get; private set; }
            public long Size { get;private set;}
            
        }

        private void RichTextBoxMessage_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}