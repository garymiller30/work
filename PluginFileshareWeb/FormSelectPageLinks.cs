using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace PluginFileshareWeb
{
    internal sealed class FormSelectPageLinks : Form
    {
        private readonly ObjectListView objectListViewLinks;
        private readonly Button buttonSelectAll;
        private readonly Button buttonClear;
        private readonly Button buttonDownload;
        private readonly Button buttonCancel;
        private readonly Label labelInfo;

        public FormSelectPageLinks(IReadOnlyList<PageDownloadLink> links, string downloadFolder)
        {
            Text = "Завантажити посилання зі сторінки";
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowIcon = false;
            Size = new Size(1100, 560);
            MinimumSize = new Size(850, 420);

            labelInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Padding = new Padding(10, 8, 10, 4),
                Text = $"Знайдено посилань: {links.Count}. Папка завантаження: {downloadFolder}",
                AutoEllipsis = true
            };

            objectListViewLinks = new ObjectListView
            {
                Dock = DockStyle.Fill,
                CheckBoxes = true,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                View = View.Details,
                Font = new Font("Segoe UI", 9F),
                UseAlternatingBackColors = true,
                AlternateRowBackColor = Color.FromArgb(248, 250, 252),
                ShowGroups = false
            };

            var fileColumn = new OLVColumn("Файл", "Text")
            {
                Width = 650,
                AspectGetter = row => GetDisplayText(((PageDownloadLink)row).Text, ((PageDownloadLink)row).Url)
            };
            var sizeColumn = new OLVColumn("Розмір", "SizeText")
            {
                Width = 90,
                TextAlign = HorizontalAlignment.Right
            };
            var hostColumn = new OLVColumn("Джерело", "Host")
            {
                Width = 130
            };
            var urlColumn = new OLVColumn("Посилання", "Url")
            {
                Width = 500
            };

            objectListViewLinks.AllColumns.Add(fileColumn);
            objectListViewLinks.AllColumns.Add(sizeColumn);
            objectListViewLinks.AllColumns.Add(hostColumn);
            objectListViewLinks.AllColumns.Add(urlColumn);
            objectListViewLinks.Columns.AddRange(new ColumnHeader[] { fileColumn, sizeColumn, hostColumn, urlColumn });
            objectListViewLinks.SetObjects(links);
            objectListViewLinks.CheckObjects(links);

            buttonSelectAll = new Button
            {
                Text = "Вибрати всі",
                AutoSize = true,
                Margin = new Padding(8, 8, 0, 8)
            };
            buttonSelectAll.Click += (s, e) => SetAllChecked(true);

            buttonClear = new Button
            {
                Text = "Зняти всі",
                AutoSize = true,
                Margin = new Padding(8, 8, 0, 8)
            };
            buttonClear.Click += (s, e) => SetAllChecked(false);

            buttonDownload = new Button
            {
                Text = "Завантажити вибрані",
                AutoSize = true,
                DialogResult = DialogResult.OK,
                Margin = new Padding(8, 8, 0, 8)
            };

            buttonCancel = new Button
            {
                Text = "Скасувати",
                AutoSize = true,
                DialogResult = DialogResult.Cancel,
                Margin = new Padding(8)
            };

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            buttonsPanel.Controls.Add(buttonCancel);
            buttonsPanel.Controls.Add(buttonDownload);
            buttonsPanel.Controls.Add(buttonClear);
            buttonsPanel.Controls.Add(buttonSelectAll);

            Controls.Add(objectListViewLinks);
            Controls.Add(buttonsPanel);
            Controls.Add(labelInfo);

            AcceptButton = buttonDownload;
            CancelButton = buttonCancel;
        }

        public IReadOnlyList<PageDownloadLink> SelectedLinks
        {
            get
            {
                return objectListViewLinks.CheckedObjects
                    .Cast<PageDownloadLink>()
                    .ToList();
            }
        }

        private void SetAllChecked(bool isChecked)
        {
            if (isChecked)
            {
                objectListViewLinks.CheckAll();
            }
            else
            {
                objectListViewLinks.UncheckAll();
            }
        }

        private static string GetDisplayText(string text, string url)
        {
            string displayText = string.IsNullOrWhiteSpace(text) ? url : text.Trim();
            if (string.IsNullOrWhiteSpace(displayText))
            {
                return "download";
            }

            return displayText;
        }
    }
}
