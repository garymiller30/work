using BrightIdeasSoftware;
using Interfaces.FileBrowser;
using JobSpace.Models;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public sealed class FormPdfToolUsageStats : KryptonForm
    {
        private readonly ObjectListView _objectListView = new ObjectListView();
        private readonly ImageList _imageList = new ImageList();
        private readonly KryptonLabel _labelSummary = new KryptonLabel();

        public FormPdfToolUsageStats(
            IEnumerable<ToolInfo> tools,
            PdfToolUsageStats stats,
            Func<ToolInfo, Image> getToolIcon)
        {
            InitializeComponent();
            LoadData(tools ?? Enumerable.Empty<ToolInfo>(), stats ?? new PdfToolUsageStats(), getToolIcon);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            _imageList.ColorDepth = ColorDepth.Depth32Bit;
            _imageList.ImageSize = new Size(16, 16);

            var olvColumnRank = new OLVColumn
            {
                Text = "#",
                AspectName = nameof(PdfToolUsageViewItem.Rank),
                Width = 45
            };

            var olvColumnName = new OLVColumn
            {
                Text = "Утиліта",
                AspectName = nameof(PdfToolUsageViewItem.Name),
                Width = 240,
                ImageGetter = x => ((PdfToolUsageViewItem)x).IconKey
            };

            var olvColumnLaunchCount = new OLVColumn
            {
                Text = "Запусків",
                AspectName = nameof(PdfToolUsageViewItem.LaunchCount),
                Width = 90
            };

            var olvColumnLastStarted = new OLVColumn
            {
                Text = "Останній запуск",
                AspectName = nameof(PdfToolUsageViewItem.LastStartedText),
                Width = 170
            };

            var olvColumnMenuPath = new OLVColumn
            {
                Text = "Розділ меню",
                AspectName = nameof(PdfToolUsageViewItem.MenuPath),
                FillsFreeSpace = true,
                MinimumWidth = 180
            };

            _objectListView.AllColumns.Add(olvColumnRank);
            _objectListView.AllColumns.Add(olvColumnName);
            _objectListView.AllColumns.Add(olvColumnLaunchCount);
            _objectListView.AllColumns.Add(olvColumnLastStarted);
            _objectListView.AllColumns.Add(olvColumnMenuPath);
            _objectListView.Columns.AddRange(new ColumnHeader[] { olvColumnRank, olvColumnName, olvColumnLaunchCount, olvColumnLastStarted, olvColumnMenuPath });
            _objectListView.Dock = DockStyle.Fill;
            _objectListView.FullRowSelect = true;
            _objectListView.GridLines = true;
            _objectListView.HideSelection = false;
            _objectListView.ShowGroups = false;
            _objectListView.SmallImageList = _imageList;
            _objectListView.UseCompatibleStateImageBehavior = false;
            _objectListView.View = View.Details;

            _labelSummary.Dock = DockStyle.Top;
            _labelSummary.Padding = new Padding(8, 8, 8, 6);
            _labelSummary.Values.Text = "Статистика завантажується...";

            Controls.Add(_objectListView);
            Controls.Add(_labelSummary);

            ClientSize = new Size(900, 520);
            MinimumSize = new Size(760, 380);
            StartPosition = FormStartPosition.CenterParent;
            ShowIcon = false;
            Text = "Статистика використання PDF-утиліт";

            ResumeLayout(false);
        }

        private void LoadData(IEnumerable<ToolInfo> tools, PdfToolUsageStats stats, Func<ToolInfo, Image> getToolIcon)
        {
            var toolList = tools.ToList();
            if (stats.Tools == null)
            {
                stats.Tools = new Dictionary<string, PdfToolUsageItem>(StringComparer.OrdinalIgnoreCase);
            }
            var toolsByKey = toolList.ToDictionary(GetToolKey, x => x, StringComparer.OrdinalIgnoreCase);
            var rows = new List<PdfToolUsageViewItem>();

            foreach (var tool in toolList)
            {
                var toolKey = GetToolKey(tool);
                stats.Tools.TryGetValue(toolKey, out var statItem);

                if (!_imageList.Images.ContainsKey(toolKey))
                {
                    _imageList.Images.Add(toolKey, getToolIcon(tool));
                }

                rows.Add(new PdfToolUsageViewItem
                {
                    ToolType = toolKey,
                    Name = tool.Meta?.Name ?? tool.ToolType.Name,
                    MenuPath = tool.Meta?.MenuPath ?? string.Empty,
                    LaunchCount = statItem?.LaunchCount ?? 0,
                    LastStartedText = GetLastStartedText(statItem?.LastStartedUtc),
                    SortLastStartedUtc = statItem?.LastStartedUtc,
                    IconKey = toolKey
                });
            }

            foreach (var statPair in stats.Tools)
            {
                if (toolsByKey.ContainsKey(statPair.Key))
                    continue;

                var statItem = statPair.Value;
                rows.Add(new PdfToolUsageViewItem
                {
                    ToolType = statPair.Key,
                    Name = statItem?.Name ?? statPair.Key,
                    MenuPath = statItem?.MenuPath ?? string.Empty,
                    LaunchCount = statItem?.LaunchCount ?? 0,
                    LastStartedText = GetLastStartedText(statItem?.LastStartedUtc),
                    SortLastStartedUtc = statItem?.LastStartedUtc
                });
            }

            rows = rows
                .OrderByDescending(x => x.LaunchCount)
                .ThenByDescending(x => x.SortLastStartedUtc ?? DateTime.MinValue)
                .ThenBy(x => x.Name)
                .ToList();

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Rank = i + 1;
            }

            _objectListView.SetObjects(rows);

            var totalLaunches = rows.Sum(x => x.LaunchCount);
            var usedTools = rows.Count(x => x.LaunchCount > 0);
            _labelSummary.Values.Text = $"Усього запусків: {totalLaunches}. Використовувалися: {usedTools} з {rows.Count} утиліт.";
        }

        private static string GetToolKey(ToolInfo tool)
        {
            return tool.ToolType.FullName ?? tool.ToolType.Name;
        }

        private static string GetLastStartedText(DateTime? lastStartedUtc)
        {
            if (!lastStartedUtc.HasValue || lastStartedUtc.Value == default)
                return "ще не запускалась";

            return lastStartedUtc.Value.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss");
        }

        private sealed class PdfToolUsageViewItem
        {
            public int Rank { get; set; }
            public string ToolType { get; set; }
            public string Name { get; set; }
            public string MenuPath { get; set; }
            public int LaunchCount { get; set; }
            public string LastStartedText { get; set; }
            public DateTime? SortLastStartedUtc { get; set; }
            public string IconKey { get; set; }
        }
    }
}
