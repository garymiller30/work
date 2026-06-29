using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces.FileBrowser;
using Interfaces.Plugins;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Services;
using JobSpace.Static.Pdf.SheetCalculator.Utilities;
using JobSpace.Static.Pdf.SheetCalculator.ViewModels;

namespace JobSpace.Static.Pdf.SheetCalculator.Views
{
    public partial class FormSheetCalculator : Form
    {
        private MainViewModel _viewModel;

        /// <summary>Context passed from the PDF tool runner (may be null).</summary>
        private PdfJobContext? _context;

        public FormSheetCalculator() : this(null) { }

        /// <summary>
        /// Creates the form. If <paramref name="context"/> contains selected files
        /// they are automatically imported as new products (features 11 &amp; 12).
        /// </summary>
        public FormSheetCalculator(PdfJobContext? context)
        {
            _context = context;

            InitializeComponent();
            _viewModel = new MainViewModel();

            // Hook up VM to custom canvas
            editorCanvas.ViewModel = _viewModel;

            // Bind events
            _viewModel.ProjectChanged += OnProjectChanged;
            _viewModel.SelectionChanged += OnSelectionChanged;
            _viewModel.History.HistoryChanged += OnHistoryChanged;

            // Bind UI control events
            BindUiEvents();

            // Set up Grid styling and columns
            SetupGrids();

            // Initialize display
            OnProjectChanged(this, EventArgs.Empty);
            OnSelectionChanged(this, EventArgs.Empty);
            OnHistoryChanged(this, EventArgs.Empty);

            // Auto-import files selected in FileBrowser (features 11 & 12)
            if (_context?.InputFiles?.Count > 0)
                ImportProductsFromContext();
        }

        private void BindUiEvents()
        {
            // File operations
            btnNew.Click += (s, e) => { _viewModel.NewProject(); };
            btnOpen.Click += OnOpenProject;
            btnSave.Click += OnSaveProject;

            // Undo / Redo
            btnUndo.Click += (s, e) => _viewModel.History.Undo();
            btnRedo.Click += (s, e) => _viewModel.History.Redo();

            // Grid & Collision settings
            chkGrid.CheckedChanged += (s, e) => { _viewModel.GridSnapEnabled = chkGrid.Checked; };
            numGridSize.ValueChanged += (s, e) => { _viewModel.GridSize = (double)numGridSize.Value; };
            chkStrictCollision.CheckedChanged += (s, e) => { _viewModel.StrictCollision = chkStrictCollision.Checked; };

            // Zoom controls
            btnZoomIn.Click += (s, e) => { _viewModel.Zoom *= 1.2f; _viewModel.TriggerRedraw(); };
            btnZoomOut.Click += (s, e) => { _viewModel.Zoom /= 1.2f; _viewModel.TriggerRedraw(); };
            btnZoomReset.Click += (s, e) => { _viewModel.Zoom = 1.0f; _viewModel.PanOffset = new PointF(30, 30); _viewModel.TriggerRedraw(); };

            // Sheet side panel buttons
            btnAddSheet.Click += OnAddSheetClick;
            btnDelSheet.Click += (s, e) => _viewModel.DeleteActiveSheet();
            btnApplySheetProps.Click += OnApplySheetPropsClick;

            // Sheet list row selection change
            dgvSheets.SelectionChanged += OnSheetListSelectionChanged;

            // Product side panel buttons
            btnAddProduct.Click += OnAddProductClick;
            btnEditProduct.Click += OnEditProductClick;
            btnDelProduct.Click += OnDelProductClick;
            btnPlaceProduct.Click += OnPlaceProductClick;
            btnAutoLayout.Click += OnAutoLayoutClick;

            // Placed item side panel buttons
            btnRotate90.Click += (s, e) => _viewModel.RotateSelected(90);
            btnRotate180.Click += (s, e) => _viewModel.RotateSelected(180);
            btnRotate270.Click += (s, e) => _viewModel.RotateSelected(270);
            btnGroup.Click += (s, e) => _viewModel.GroupSelected();
            btnUngroup.Click += (s, e) => _viewModel.UngroupSelected();
            chkLocked.CheckedChanged += OnLockCheckboxChanged;

            // Alignment buttons
            btnAlignLeft.Click += (s, e) => _viewModel.AlignSelected("left");
            btnAlignRight.Click += (s, e) => _viewModel.AlignSelected("right");
            btnAlignTop.Click += (s, e) => _viewModel.AlignSelected("top");
            btnAlignBottom.Click += (s, e) => _viewModel.AlignSelected("bottom");

            // Matrix Duplication
            btnMatrixDuplicate.Click += OnMatrixDuplicateClick;
        }

        private void SetupGrids()
        {
            // Dark theme style for DataGridViews
            StyleGrid(dgvSheets);
            StyleGrid(dgvProducts);

            // Columns for Sheets
            dgvSheets.Columns.Add("Name", "Назва");
            dgvSheets.Columns.Add("Format", "Формат");
            dgvSheets.Columns.Add("Circulation", "Тираж листів");

            dgvSheets.Columns[0].FillWeight = 40;
            dgvSheets.Columns[1].FillWeight = 30;
            dgvSheets.Columns[2].FillWeight = 30;

            // Columns for Products
            dgvProducts.Columns.Add("Name",     "Виріб");
            dgvProducts.Columns.Add("Format",   "Розмір");
            dgvProducts.Columns.Add("OnSheets", "На листах");   // feature 10
            dgvProducts.Columns.Add("Required", "Потрібно");
            dgvProducts.Columns.Add("Actual",   "Фактично");
            dgvProducts.Columns.Add("Remaining","Залишок");

            dgvProducts.Columns[0].FillWeight = 26;
            dgvProducts.Columns[1].FillWeight = 19;
            dgvProducts.Columns[2].FillWeight = 14;
            dgvProducts.Columns[3].FillWeight = 14;
            dgvProducts.Columns[4].FillWeight = 14;
            dgvProducts.Columns[5].FillWeight = 13;
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgv.ForeColor = Color.White;
            dgv.GridColor = Color.FromArgb(45, 45, 48);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;

            // Header style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgv.EnableHeadersVisualStyles = false;

            // Cells style
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        // Project Synchronization
        private void OnProjectChanged(object sender, EventArgs e)
        {
            if (_viewModel == null || _viewModel.Project == null) return;

            // Sync Settings Checkboxes without triggering recursive loops
            chkGrid.Checked = _viewModel.GridSnapEnabled;
            numGridSize.Value = (decimal)_viewModel.GridSize;
            chkStrictCollision.Checked = _viewModel.StrictCollision;

            // 1. Sync Sheets List
            dgvSheets.SelectionChanged -= OnSheetListSelectionChanged;
            
            int selectedSheetRowIndex = -1;
            dgvSheets.Rows.Clear();
            foreach (var s in _viewModel.Project.Sheets)
            {
                int rIdx = dgvSheets.Rows.Add(s.Name, $"{s.Width}x{s.Height}", s.CalculatedCirculation);
                dgvSheets.Rows[rIdx].Tag = s;
                if (s == _viewModel.ActiveSheet)
                {
                    selectedSheetRowIndex = rIdx;
                }
            }

            if (selectedSheetRowIndex >= 0 && selectedSheetRowIndex < dgvSheets.Rows.Count)
            {
                dgvSheets.Rows[selectedSheetRowIndex].Selected = true;
            }
            dgvSheets.SelectionChanged += OnSheetListSelectionChanged;

            // 2. Sync Products List — preserve the currently selected product
            Guid? previouslySelectedProductId = GetSelectedProduct()?.Id;

            dgvProducts.SelectionChanged -= OnProductsGridSelectionChanged;
            dgvProducts.Rows.Clear();
            var results = CalculationService.GetCirculationResults(_viewModel.Project);
            int restoreRowIndex = -1;
            foreach (var res in results)
            {
                var prod = _viewModel.Project.Products.FirstOrDefault(x => x.Id == res.ProductId);
                if (prod != null)
                {
                    int rIdx = dgvProducts.Rows.Add(
                        prod.Name,
                        $"{prod.Width}x{prod.Height}",
                        res.PlacedCount,           // "На листах"
                        res.Required,
                        res.Actual,
                        res.Remaining
                    );
                    dgvProducts.Rows[rIdx].Tag = prod;

                    // Restore previous selection
                    if (prod.Id == previouslySelectedProductId)
                        restoreRowIndex = rIdx;

                    // Color remaining/overproduction cells (column index 5 after adding "На листах")
                    if (res.Remaining > 0)
                        dgvProducts.Rows[rIdx].Cells[5].Style.ForeColor = Color.Salmon;      // Shortage
                    else if (res.Remaining < 0)
                        dgvProducts.Rows[rIdx].Cells[5].Style.ForeColor = Color.LightGreen;  // Overproduction
                }
            }

            // Restore row focus without firing selection-changed recursively
            if (restoreRowIndex >= 0 && restoreRowIndex < dgvProducts.Rows.Count)
                dgvProducts.Rows[restoreRowIndex].Selected = true;
            else if (dgvProducts.Rows.Count > 0)
                dgvProducts.Rows[0].Selected = true;

            dgvProducts.SelectionChanged += OnProductsGridSelectionChanged;

            // 3. Sync Active Sheet Properties Panel
            if (_viewModel.ActiveSheet != null)
            {
                txtSheetName.Text = _viewModel.ActiveSheet.Name;
                numSheetWidth.Value = (decimal)_viewModel.ActiveSheet.Width;
                numSheetHeight.Value = (decimal)_viewModel.ActiveSheet.Height;
                numMarginLeft.Value = (decimal)_viewModel.ActiveSheet.MarginLeft;
                numMarginRight.Value = (decimal)_viewModel.ActiveSheet.MarginRight;
                numMarginTop.Value = (decimal)_viewModel.ActiveSheet.MarginTop;
                numMarginBottom.Value = (decimal)_viewModel.ActiveSheet.MarginBottom;
            }

            // Update status bar summary
            statusCalculations.Text = $"Листів у проекті: {_viewModel.Project.Sheets.Count} | Виробів типів: {_viewModel.Project.Products.Count}";
        }

        // Selection Synchronization
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_viewModel == null) return;

            int count = _viewModel.SelectedPlacedItemIds.Count;
            lblItemSelection.Text = $"Виділено об\'єктів: {count}";

            // Enable/Disable controls in Property Item panel based on selection count
            chkLocked.Enabled = count > 0;
            btnRotate90.Enabled = count > 0;
            btnRotate180.Enabled = count > 0;
            btnRotate270.Enabled = count > 0;
            btnGroup.Enabled = count >= 2;
            btnUngroup.Enabled = count > 0;

            btnAlignLeft.Enabled = count >= 2;
            btnAlignRight.Enabled = count >= 2;
            btnAlignTop.Enabled = count >= 2;
            btnAlignBottom.Enabled = count >= 2;

            // Matrix panel only enabled if exactly 1 item is selected
            grpMatrix.Enabled = count == 1;

            if (count > 0 && _viewModel.ActiveSheet != null)
            {
                var selectedItems = _viewModel.ActiveSheet.PlacedItems
                    .Where(x => _viewModel.SelectedPlacedItemIds.Contains(x.Id))
                    .ToList();

                // Check lock status
                chkLocked.CheckedChanged -= OnLockCheckboxChanged;
                chkLocked.Checked = selectedItems.All(x => x.IsLocked);
                chkLocked.CheckedChanged += OnLockCheckboxChanged;

                // Auto-fill default matrix spacing step for single selection
                if (count == 1)
                {
                    var item = selectedItems.First();
                    var prod = _viewModel.Project.Products.FirstOrDefault(x => x.Id == item.ProductId);
                    if (prod != null)
                    {
                        // Rotate dimensions if item is rotated
                        GeometryHelper.GetItemDimensions(prod, item.Angle, out double w, out double h);

                        // step = size + tech bleed * 2
                        numMatrixStepX.Value = (decimal)(w + 2 * prod.TechMargin);
                        numMatrixStepY.Value = (decimal)(h + 2 * prod.TechMargin);
                    }
                }
            }
        }

        private void OnHistoryChanged(object sender, EventArgs e)
        {
            if (_viewModel == null) return;

            btnUndo.Enabled = _viewModel.History.CanUndo;
            btnRedo.Enabled = _viewModel.History.CanRedo;

            btnUndo.Text = _viewModel.History.CanUndo ? $"Undo ({_viewModel.History.GetUndoName()})" : "Undo";
            btnRedo.Text = _viewModel.History.CanRedo ? $"Redo ({_viewModel.History.GetRedoName()})" : "Redo";
        }

        // File Handler Callbacks
        private void OnOpenProject(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Файли проєкту (*.json)|*.json|Усі файли (*.*)|*.*", Title = "Відкрити проєкт" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _viewModel.LoadProject(ofd.FileName);
                        MessageBox.Show("Проєкт завантажено успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка завантаження проекту: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OnSaveProject(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog { Filter = "Файли проєкту (*.json)|*.json", Title = "Зберегти проєкт" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _viewModel.SaveProject(sfd.FileName);
                        MessageBox.Show("Проєкт збережено успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка збереження проекту: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Sheet Events
        private void OnSheetListSelectionChanged(object sender, EventArgs e)
        {
            if (dgvSheets.SelectedRows.Count > 0)
            {
                var row = dgvSheets.SelectedRows[0];
                if (row.Tag is Sheet sheet)
                {
                    _viewModel.ActiveSheet = sheet;
                }
            }
        }

        private void OnAddSheetClick(object sender, EventArgs e)
        {
            using (var diag = new FormSheetEdit("Новий друкарський лист"))
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.AddSheet(diag.SheetName, diag.SheetWidth, diag.SheetHeight, diag.MarginLeft, diag.MarginRight, diag.MarginTop, diag.MarginBottom);
                }
            }
        }

        private void OnApplySheetPropsClick(object sender, EventArgs e)
        {
            if (_viewModel.ActiveSheet == null) return;
            _viewModel.UpdateActiveSheet(
                txtSheetName.Text,
                (double)numSheetWidth.Value,
                (double)numSheetHeight.Value,
                (double)numMarginLeft.Value,
                (double)numMarginRight.Value,
                (double)numMarginTop.Value,
                (double)numMarginBottom.Value
            );
        }

        // Product Events
        private void OnAddProductClick(object sender, EventArgs e)
        {
            using (var diag = new FormProductEdit("Новий виріб"))
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.AddProduct(diag.ProductName, diag.ProductWidth, diag.ProductHeight, diag.RequiredCirculation, diag.TechMargin);
                }
            }
        }

        private void OnEditProductClick(object sender, EventArgs e)
        {
            var selectedProduct = GetSelectedProduct();
            if (selectedProduct == null) return;

            using (var diag = new FormProductEdit("Редагувати виріб", selectedProduct.Name, selectedProduct.Width, selectedProduct.Height, selectedProduct.RequiredCirculation, selectedProduct.TechMargin))
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    _viewModel.UpdateProduct(selectedProduct, diag.ProductName, diag.ProductWidth, diag.ProductHeight, diag.RequiredCirculation, diag.TechMargin);
                }
            }
        }

        private void OnDelProductClick(object sender, EventArgs e)
        {
            var selectedProduct = GetSelectedProduct();
            if (selectedProduct == null) return;

            var res = MessageBox.Show($"Ви дійсно хочете видалити виріб \"{selectedProduct.Name}\" та всі його розміщення на листах?", "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                _viewModel.DeleteProduct(selectedProduct);
            }
        }

        private void OnPlaceProductClick(object sender, EventArgs e)
        {
            var selectedProduct = GetSelectedProduct();
            if (selectedProduct == null) return;
            _viewModel.PlaceProduct(selectedProduct);
        }

        private void OnAutoLayoutClick(object sender, EventArgs e)
        {
            var selectedProduct = GetSelectedProduct();
            if (selectedProduct == null) return;
            _viewModel.AutoLayoutProduct(selectedProduct);
        }

        private Product GetSelectedProduct()
        {
            if (dgvProducts.SelectedRows.Count > 0)
                return dgvProducts.SelectedRows[0].Tag as Product;
            return null;
        }

        // Stub handler so we can subscribe/unsubscribe safely around list rebuilds.
        private void OnProductsGridSelectionChanged(object sender, EventArgs e) { }

        // ─── Feature 11 & 12: Auto-import products from FileBrowser context ───

        /// <summary>
        /// Imports files from <see cref="_context"/>.<see cref="PdfJobContext.InputFiles"/>.
        /// For each file:
        ///   • Parses product name and circulation from the filename (feature 12).
        ///   • Reads PDF page dimensions via iTextSharp (feature 11).
        ///   • Skips files whose base name is already in the product list.
        /// </summary>
        private void ImportProductsFromContext()
        {
            if (_context?.InputFiles == null || _context.InputFiles.Count == 0) return;

            var existingNames = new HashSet<string>(
                _viewModel.Project.Products.Select(p => p.Name),
                StringComparer.OrdinalIgnoreCase);

            foreach (var fileInfo in _context.InputFiles)
            {
                string filePath = fileInfo?.FullName;
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) continue;

                string fileNameNoExt = Path.GetFileNameWithoutExtension(filePath);

                // Feature 12: extract name and circulation from filename
                var (productName, circulation) = FileImportHelper.ParseFileName(fileNameNoExt);

                // Skip if already imported (by product name)
                if (existingNames.Contains(productName)) continue;

                // Feature 11: read PDF page dimensions

                var page_info = PdfHelper.GetPageInfo(filePath);

                double width = 0;
                double height = 0;

                if (page_info != null)
                {
                    width = page_info.Trimbox.wMM();
                    height = page_info.Trimbox.hMM();
                }

                // If dimensions could not be determined, leave them at 0 (default will apply in AddProduct)
                _viewModel.AddProduct(
                    productName,
                    width  > 0 ? width  : 90,
                    height > 0 ? height : 50,
                    circulation,
                    2.0  // default tech margin
                );

                existingNames.Add(productName);
            }
        }

        // Placed Item Events
        private void OnLockCheckboxChanged(object sender, EventArgs e)
        {
            _viewModel.ToggleLockSelected(chkLocked.Checked);
        }

        private void OnMatrixDuplicateClick(object sender, EventArgs e)
        {
            int cols = (int)numMatrixCols.Value;
            int rows = (int)numMatrixRows.Value;
            double stepX = (double)numMatrixStepX.Value;
            double stepY = (double)numMatrixStepY.Value;

            _viewModel.DuplicateSelectedAsMatrix(cols, rows, stepX, stepY);
        }

        // Keyboard shortcuts, nudging, and deletes
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Delete key: deletes selected placed items
            if (keyData == Keys.Delete)
            {
                _viewModel.DeleteSelectedPlacedItems();
                return true;
            }

            // Undo: Ctrl+Z
            if (keyData == (Keys.Control | Keys.Z))
            {
                _viewModel.History.Undo();
                return true;
            }

            // Redo: Ctrl+Y
            if (keyData == (Keys.Control | Keys.Y))
            {
                _viewModel.History.Redo();
                return true;
            }

            // Group: Ctrl+G
            if (keyData == (Keys.Control | Keys.G))
            {
                _viewModel.GroupSelected();
                return true;
            }

            // Ungroup: Ctrl+U
            if (keyData == (Keys.Control | Keys.U))
            {
                _viewModel.UngroupSelected();
                return true;
            }

            // Arrow key nudging
            bool isShift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            double nudgeDelta = isShift ? 0.1 : 1.0; // mm

            if (keyData == Keys.Left || keyData == (Keys.Shift | Keys.Left))
            {
                _viewModel.MoveSelection(-nudgeDelta, 0);
                return true;
            }
            if (keyData == Keys.Right || keyData == (Keys.Shift | Keys.Right))
            {
                _viewModel.MoveSelection(nudgeDelta, 0);
                return true;
            }
            if (keyData == Keys.Up || keyData == (Keys.Shift | Keys.Up))
            {
                _viewModel.MoveSelection(0, -nudgeDelta);
                return true;
            }
            if (keyData == Keys.Down || keyData == (Keys.Shift | Keys.Down))
            {
                _viewModel.MoveSelection(0, nudgeDelta);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
