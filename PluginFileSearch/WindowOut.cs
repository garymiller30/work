using BrightIdeasSoftware;
using Interfaces;
using Interfaces.Profile;
using JobSpace.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginFileSearch
{
    public sealed class WindowOut : UserControl, IPluginInfo
    {
        private readonly TableLayoutPanel _filtersPanel;
        private readonly ComboBox _comboCustomers;
        private readonly TextBox _textName;
        private readonly TextBox _textMask;
        private readonly NumericUpDown _numWidth;
        private readonly NumericUpDown _numHeight;
        private readonly NumericUpDown _numTolerance;
        private readonly CheckBox _checkWidth;
        private readonly CheckBox _checkHeight;
        private readonly Button _buttonSearch;
        private readonly Button _buttonStop;
        private readonly Label _labelStatus;
        private readonly ObjectListView _results;
        private CancellationTokenSource _searchCancellation;
        private int _foundCount;
        private int _searchVersion;

        public WindowOut()
        {
            _filtersPanel = new TableLayoutPanel();
            _comboCustomers = new ComboBox();
            _textName = new TextBox();
            _textMask = new TextBox();
            _numWidth = CreateNumberInput();
            _numHeight = CreateNumberInput();
            _numTolerance = CreateNumberInput();
            _checkWidth = new CheckBox();
            _checkHeight = new CheckBox();
            _buttonSearch = new Button();
            _buttonStop = new Button();
            _labelStatus = new Label();
            _results = new ObjectListView();

            InitializeComponent();
        }

        public IUserProfile UserProfile { get; set; }
        public string PluginName => GetPluginName();
        public string PluginDescription => "Пошук файлів у робочій папці за замовником, назвою, маскою та форматом сторінки";

        public UserControl GetUserControl() => this;
        public string GetPluginName() => "File Search";
        public void SetCurJob(IJob curJob) { }
        public void BeforeJobChange(IJob job) { }
        public void AfterJobChange(IJob job) { }
        public void ShowSettingsDlg() => MessageBox.Show("Налаштування відсутні");

        public void Start()
        {
            FillCustomers();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _searchCancellation?.Cancel();
                _searchCancellation?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Dock = DockStyle.Fill;

            _filtersPanel.Dock = DockStyle.Top;
            _filtersPanel.AutoSize = true;
            _filtersPanel.Padding = new Padding(8);
            _filtersPanel.ColumnCount = 8;
            _filtersPanel.RowCount = 3;
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _filtersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));

            _comboCustomers.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboCustomers.Dock = DockStyle.Fill;
            _textName.Dock = DockStyle.Fill;
            _textMask.Dock = DockStyle.Fill;
            _textMask.Text = "*.*";

            _checkWidth.Text = "Ширина";
            _checkHeight.Text = "Висота";
            _numTolerance.Value = 1;

            _buttonSearch.Text = "Пошук";
            _buttonSearch.AutoSize = true;
            _buttonSearch.Click += ButtonSearch_Click;

            _buttonStop.Text = "Стоп";
            _buttonStop.AutoSize = true;
            _buttonStop.Enabled = false;
            _buttonStop.Click += ButtonStop_Click;

            _labelStatus.AutoSize = true;
            _labelStatus.Dock = DockStyle.Fill;
            _labelStatus.TextAlign = ContentAlignment.MiddleLeft;

            AddFilterRow(0, "Замовник", _comboCustomers, "Назва", _textName, "Маска", _textMask);
            AddNumberRow(1);
            _filtersPanel.Controls.Add(_buttonSearch, 0, 2);
            _filtersPanel.Controls.Add(_buttonStop, 1, 2);
            _filtersPanel.Controls.Add(_labelStatus, 2, 2);
            _filtersPanel.SetColumnSpan(_labelStatus, 6);

            ConfigureResultsList();

            Controls.Add(_results);
            Controls.Add(_filtersPanel);

            ResumeLayout(false);
        }

        private void AddFilterRow(int row, string label1, Control control1, string label2, Control control2, string label3, Control control3)
        {
            _filtersPanel.Controls.Add(CreateLabel(label1), 0, row);
            _filtersPanel.Controls.Add(control1, 1, row);
            _filtersPanel.Controls.Add(CreateLabel(label2), 2, row);
            _filtersPanel.Controls.Add(control2, 3, row);
            _filtersPanel.Controls.Add(CreateLabel(label3), 4, row);
            _filtersPanel.Controls.Add(control3, 5, row);
            _filtersPanel.SetColumnSpan(control3, 3);
        }

        private void AddNumberRow(int row)
        {
            _filtersPanel.Controls.Add(_checkWidth, 0, row);
            _filtersPanel.Controls.Add(_numWidth, 1, row);
            _filtersPanel.Controls.Add(_checkHeight, 2, row);
            _filtersPanel.Controls.Add(_numHeight, 3, row);
            _filtersPanel.Controls.Add(CreateLabel("Допуск, мм"), 4, row);
            _filtersPanel.Controls.Add(_numTolerance, 5, row);
        }

        private void ConfigureResultsList()
        {
            _results.Dock = DockStyle.Fill;
            _results.FullRowSelect = true;
            _results.GridLines = true;
            _results.HideSelection = false;
            _results.ShowGroups = false;
            _results.UseFiltering = true;
            _results.View = View.Details;
            _results.CellClick += Results_CellClick;

            _results.Columns.Add(CreateColumn("Файл", 260, x => ((FileSearchResult)x).Name));
            _results.Columns.Add(CreateColumn("Замовник", 140, x => ((FileSearchResult)x).CustomerName));
            _results.Columns.Add(CreateColumn("Папка", 380, x => ((FileSearchResult)x).DirectoryName));
            _results.Columns.Add(CreateColumn("Ширина", 75, x => FormatDecimal(((FileSearchResult)x).Width)));
            _results.Columns.Add(CreateColumn("Висота", 75, x => FormatDecimal(((FileSearchResult)x).Height)));
            _results.Columns.Add(CreateColumn("Стор.", 55, x => ((FileSearchResult)x).Pages == 0 ? string.Empty : ((FileSearchResult)x).Pages.ToString(CultureInfo.InvariantCulture)));
            _results.Columns.Add(CreateColumn("Змінено", 130, x => ((FileSearchResult)x).LastWriteTime));
        }

        private static OLVColumn CreateColumn(string text, int width, AspectGetterDelegate getter)
        {
            return new OLVColumn(text, null)
            {
                Width = width,
                AspectGetter = getter
            };
        }

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static NumericUpDown CreateNumberInput()
        {
            return new NumericUpDown
            {
                DecimalPlaces = 1,
                Maximum = 100000,
                Minimum = 0,
                Increment = 1,
                Dock = DockStyle.Fill
            };
        }

        private void FillCustomers()
        {
            var items = new List<CustomerFilterItem> { new CustomerFilterItem(null) };

            if (UserProfile?.Customers != null)
            {
                items.AddRange(UserProfile.Customers
                    .Where(x => x.Show)
                    .OrderBy(x => x.Name)
                    .Select(x => new CustomerFilterItem(x)));
            }

            _comboCustomers.DataSource = items;
            _comboCustomers.DisplayMember = nameof(CustomerFilterItem.Name);
        }

        private async void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (UserProfile?.Jobs?.Settings == null)
            {
                MessageBox.Show("Профіль користувача ще не ініціалізований.");
                return;
            }

            var root = GetSearchRoot();
            if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
            {
                MessageBox.Show("Робоча папка не існує.");
                return;
            }

            _searchCancellation?.Cancel();
            _searchCancellation = new CancellationTokenSource();
            var searchVersion = ++_searchVersion;
            _foundCount = 0;

            SetSearchState(true, "Пошук...");
            _results.ClearObjects();

            try
            {
                var query = CreateQuery(root);
                var count = await Task.Run(() =>
                    SearchFiles(query, _searchCancellation.Token, result => AddSearchResult(result, searchVersion)));

                if (searchVersion != _searchVersion)
                    return;

                var status = _searchCancellation.IsCancellationRequested
                    ? $"Пошук зупинено. Знайдено: {count}"
                    : $"Знайдено: {count}";
                SetSearchState(false, status);
            }
            catch (OperationCanceledException)
            {
                SetSearchState(false, "Пошук зупинено");
            }
            catch (Exception exception)
            {
                SetSearchState(false, exception.Message);
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            _searchCancellation?.Cancel();
        }

        private string GetSearchRoot()
        {
            if (_comboCustomers.SelectedItem is CustomerFilterItem item && item.Customer != null)
                return UserProfile.Customers.GetCustomerWorkFolder(item.Customer);

            return UserProfile.Jobs.Settings.WorkPath;
        }

        private SearchQuery CreateQuery(string root)
        {
            return new SearchQuery
            {
                Root = root,
                Customer = (_comboCustomers.SelectedItem as CustomerFilterItem)?.Customer,
                NamePart = _textName.Text.Trim(),
                Masks = SplitMasks(_textMask.Text),
                UseWidth = _checkWidth.Checked,
                Width = _numWidth.Value,
                UseHeight = _checkHeight.Checked,
                Height = _numHeight.Value,
                Tolerance = _numTolerance.Value
            };
        }

        private int SearchFiles(SearchQuery query, CancellationToken token, Action<FileSearchResult> onFound)
        {
            var count = 0;
            var masks = query.Masks.Count == 0 ? new List<string> { "*.*" } : query.Masks;

            foreach (var file in EnumerateFilesSafe(query.Root, token))
            {
                if (token.IsCancellationRequested)
                    break;

                var fileName = Path.GetFileName(file);
                if (!MatchesName(fileName, query.NamePart)) continue;
                if (!masks.Any(mask => IsMaskMatch(fileName, mask))) continue;

                var info = new FileInfo(file).ToFileSystemInfoExt();
                if (query.UseWidth || query.UseHeight)
                {
                    if (!IsFormatSupported(info.FileInfo.Extension)) continue;

                    info.GetExtendedFileInfoFormat();
                    if (!MatchesSize(info, query)) continue;
                }

                onFound(new FileSearchResult(info, GetCustomerName(query, file)));
                count++;
            }

            return count;
        }

        private void AddSearchResult(FileSearchResult result, int searchVersion)
        {
            if (IsDisposed || searchVersion != _searchVersion)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(() => AddSearchResult(result, searchVersion)));
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            if (searchVersion != _searchVersion)
                return;

            _results.AddObject(result);
            _foundCount++;
            if (_buttonStop.Enabled)
                _labelStatus.Text = $"Пошук... Знайдено: {_foundCount}";
        }

        private IEnumerable<string> EnumerateFilesSafe(string root, CancellationToken token)
        {
            var pending = new Stack<string>();
            pending.Push(root);

            while (pending.Count > 0)
            {
                if (token.IsCancellationRequested)
                    yield break;

                var current = pending.Pop();

                string[] files;
                try
                {
                    files = Directory.GetFiles(current);
                }
                catch
                {
                    files = Array.Empty<string>();
                }

                foreach (var file in files)
                    yield return file;

                string[] dirs;
                try
                {
                    dirs = Directory.GetDirectories(current);
                }
                catch
                {
                    dirs = Array.Empty<string>();
                }

                foreach (var dir in dirs)
                    pending.Push(dir);
            }
        }

        private static bool MatchesName(string fileName, string namePart)
        {
            return string.IsNullOrWhiteSpace(namePart)
                || fileName.IndexOf(namePart, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private static bool MatchesSize(IFileSystemInfoExt info, SearchQuery query)
        {
            if (query.UseWidth && !IsNear(info.Format.Width, query.Width, query.Tolerance)) return false;
            if (query.UseHeight && !IsNear(info.Format.Height, query.Height, query.Tolerance)) return false;
            return true;
        }

        private static bool IsNear(decimal value, decimal target, decimal tolerance)
        {
            return value > 0 && Math.Abs(value - target) <= tolerance;
        }

        private static bool IsMaskMatch(string fileName, string mask)
        {
            if (string.IsNullOrWhiteSpace(mask)) return true;

            var pattern = "^" + Regex.Escape(mask.Trim())
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";

            return Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        private static bool IsFormatSupported(string extension)
        {
            switch ((extension ?? string.Empty).ToLower(CultureInfo.InvariantCulture))
            {
                case ".ai":
                case ".eps":
                case ".jpg":
                case ".jpeg":
                case ".pdf":
                case ".png":
                case ".psd":
                case ".tif":
                case ".tiff":
                    return true;
                default:
                    return false;
            }
        }

        private static List<string> SplitMasks(string masks)
        {
            return (masks ?? string.Empty)
                .Split(new[] { ';', ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToList();
        }

        private string GetCustomerName(SearchQuery query, string file)
        {
            if (query.Customer != null)
                return query.Customer.Name;

            var customers = UserProfile?.Customers;
            if (customers == null) return string.Empty;

            foreach (var customer in customers.Where(x => x.Show))
            {
                var customerFolder = customers.GetCustomerWorkFolder(customer);
                if (IsPathInside(file, customerFolder))
                    return customer.Name;
            }

            return string.Empty;
        }

        private static bool IsPathInside(string path, string folder)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(folder)) return false;

            var normalizedFolder = Path.GetFullPath(folder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                   + Path.DirectorySeparatorChar;
            var normalizedPath = Path.GetFullPath(path);

            return normalizedPath.StartsWith(normalizedFolder, StringComparison.InvariantCultureIgnoreCase);
        }

        private void Results_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Model is FileSearchResult result)
                UserProfile?.FileBrowser?.Browsers?.FirstOrDefault()?.ShowFileInFolder(result.FullName);
        }

        private void SetSearchState(bool searching, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetSearchState(searching, message)));
                return;
            }

            _buttonSearch.Enabled = !searching;
            _buttonStop.Enabled = searching;
            _labelStatus.Text = message;
        }

        private static string FormatDecimal(decimal value)
        {
            return value == 0 ? string.Empty : value.ToString("0.#", CultureInfo.InvariantCulture);
        }

        private sealed class SearchQuery
        {
            public string Root { get; set; }
            public ICustomer Customer { get; set; }
            public string NamePart { get; set; }
            public List<string> Masks { get; set; }
            public bool UseWidth { get; set; }
            public decimal Width { get; set; }
            public bool UseHeight { get; set; }
            public decimal Height { get; set; }
            public decimal Tolerance { get; set; }
        }
    }
}
