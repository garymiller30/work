using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using JobSpace.Static.Pdf.SheetCalculator.Commands;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Services;
using JobSpace.Static.Pdf.SheetCalculator.Utilities;

namespace JobSpace.Static.Pdf.SheetCalculator.ViewModels
{
    public class MainViewModel
    {
        public Project Project { get; private set; }
        public CommandHistory History { get; } = new CommandHistory();

        private Sheet _activeSheet;
        public Sheet ActiveSheet
        {
            get => _activeSheet;
            set
            {
                if (_activeSheet != value)
                {
                    _activeSheet = value;
                    SelectedPlacedItemIds.Clear();
                    OnSelectionChanged();
                    OnProjectChanged();
                }
            }
        }

        public HashSet<Guid> SelectedPlacedItemIds { get; } = new HashSet<Guid>();

        // Settings (mapped to Project settings, but view-model manages interaction)
        public bool GridSnapEnabled
        {
            get => Project?.GridSnapEnabled ?? false;
            set
            {
                if (Project != null && Project.GridSnapEnabled != value)
                {
                    Project.GridSnapEnabled = value;
                    OnProjectChanged();
                }
            }
        }

        public double GridSize
        {
            get => Project?.GridSize ?? 5.0;
            set
            {
                if (Project != null && Project.GridSize != value)
                {
                    Project.GridSize = value;
                    OnProjectChanged();
                }
            }
        }

        public bool StrictCollision
        {
            get => Project?.StrictCollision ?? false;
            set
            {
                if (Project != null && Project.StrictCollision != value)
                {
                    Project.StrictCollision = value;
                    OnProjectChanged();
                }
            }
        }

        // Zoom & Pan
        public float Zoom { get; set; } = 1.0f;
        public PointF PanOffset { get; set; } = new PointF(30, 30);

        // Snap states for rendering helpers
        public List<double> ActiveSnapX { get; } = new List<double>();
        public List<double> ActiveSnapY { get; } = new List<double>();

        // Events
        public event EventHandler ProjectChanged;
        public event EventHandler SelectionChanged;
        public event Action RedrawRequested;

        public MainViewModel()
        {
            NewProject();
        }

        public void NewProject()
        {
            Project = ProjectService.CreateNewProject();
            ActiveSheet = Project.Sheets.FirstOrDefault();
            History.Clear();
            SelectedPlacedItemIds.Clear();
            OnProjectChanged();
            OnSelectionChanged();
        }

        public void LoadProject(string filePath)
        {
            Project = ProjectService.LoadProject(filePath);
            ActiveSheet = Project.Sheets.FirstOrDefault();
            History.Clear();
            SelectedPlacedItemIds.Clear();
            OnProjectChanged();
            OnSelectionChanged();
        }

        public void SaveProject(string filePath)
        {
            ProjectService.SaveProject(Project, filePath);
        }

        public void TriggerRedraw()
        {
            RedrawRequested?.Invoke();
        }

        public void OnProjectChanged()
        {
            CalculationService.Recalculate(Project);
            ProjectChanged?.Invoke(this, EventArgs.Empty);
            TriggerRedraw();
        }

        public void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            TriggerRedraw();
        }

        // Sheet Operations
        public void AddSheet(string name, double width, double height, double ml, double mr, double mt, double mb)
        {
            var sheet = new Sheet
            {
                Name = name,
                Width = width,
                Height = height,
                MarginLeft = ml,
                MarginRight = mr,
                MarginTop = mt,
                MarginBottom = mb
            };
            History.Execute(new AddSheetCommand(Project, sheet, () =>
            {
                ActiveSheet = sheet;
                OnProjectChanged();
            }));
        }

        public void UpdateActiveSheet(string name, double width, double height, double ml, double mr, double mt, double mb)
        {
            if (ActiveSheet == null) return;

            var newState = ActiveSheet.Clone();
            newState.Name = name;
            newState.Width = width;
            newState.Height = height;
            newState.MarginLeft = ml;
            newState.MarginRight = mr;
            newState.MarginTop = mt;
            newState.MarginBottom = mb;

            History.Execute(new UpdateSheetCommand(Project, ActiveSheet, newState, OnProjectChanged));
        }

        public void DeleteActiveSheet()
        {
            if (ActiveSheet == null || Project.Sheets.Count <= 1) return;

            var sheetToDelete = ActiveSheet;
            int index = Project.Sheets.IndexOf(sheetToDelete);
            var nextActive = Project.Sheets[index == 0 ? 1 : index - 1];

            History.Execute(new RemoveSheetCommand(Project, sheetToDelete, () =>
            {
                ActiveSheet = nextActive;
                OnProjectChanged();
            }));
        }

        // Product Operations
        public void AddProduct(string name, double width, double height, int reqCirc, double techMargin)
        {
            var prod = new Product
            {
                Name = name,
                Width = width,
                Height = height,
                RequiredCirculation = reqCirc,
                TechMargin = techMargin
            };
            History.Execute(new AddProductCommand(Project, prod, OnProjectChanged));
        }

        public void UpdateProduct(Product product, string name, double width, double height, int reqCirc, double techMargin)
        {
            if (product == null) return;

            var newState = product.Clone();
            newState.Name = name;
            newState.Width = width;
            newState.Height = height;
            newState.RequiredCirculation = reqCirc;
            newState.TechMargin = techMargin;

            History.Execute(new UpdateProductCommand(Project, product, newState, OnProjectChanged));
        }

        public void DeleteProduct(Product product)
        {
            if (product == null) return;
            History.Execute(new RemoveProductCommand(Project, product, () =>
            {
                SelectedPlacedItemIds.Clear();
                OnSelectionChanged();
                OnProjectChanged();
            }));
        }

        // Layout Operations
        public void PlaceProduct(Product product)
        {
            if (ActiveSheet == null || product == null) return;

            // Place centered in printable area
            var printable = GeometryHelper.GetPrintableArea(ActiveSheet);
            double px = printable.X + (printable.Width - product.Width) / 2;
            double py = printable.Y + (printable.Height - product.Height) / 2;

            if (px < 0) px = 0;
            if (py < 0) py = 0;

            var item = new PlacedItem
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                X = px,
                Y = py,
                Angle = 0
            };

            if (!CanPlaceItems(new[] { item }, Enumerable.Empty<Guid>())) return;

            History.Execute(new PlaceItemsCommand(Project, ActiveSheet, new List<PlacedItem> { item }, () =>
            {
                SelectedPlacedItemIds.Clear();
                SelectedPlacedItemIds.Add(item.Id);
                OnSelectionChanged();
                OnProjectChanged();
            }));
        }

        public void AutoLayoutProduct(Product product)
        {
            if (ActiveSheet == null || product == null) return;

            var items = NestingService.AutoLayout(product, ActiveSheet);
            if (!items.Any()) return;
            if (!CanPlaceItems(items, Enumerable.Empty<Guid>())) return;

            History.Execute(new PlaceItemsCommand(Project, ActiveSheet, items, () =>
            {
                SelectedPlacedItemIds.Clear();
                foreach (var item in items)
                {
                    SelectedPlacedItemIds.Add(item.Id);
                }
                OnSelectionChanged();
                OnProjectChanged();
            }));
        }

        public void DeleteSelectedPlacedItems()
        {
            if (ActiveSheet == null || !SelectedPlacedItemIds.Any()) return;

            var itemsToRemove = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            
            History.Execute(new RemoveItemsCommand(Project, ActiveSheet, itemsToRemove, () =>
            {
                SelectedPlacedItemIds.Clear();
                OnSelectionChanged();
                OnProjectChanged();
            }));
        }

        public void RotateSelected(int angleDelta)
        {
            if (ActiveSheet == null || !SelectedPlacedItemIds.Any()) return;

            var itemsToRotate = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            var rotatedItems = itemsToRotate
                .Where(x => !x.IsLocked)
                .Select(x =>
                {
                    var clone = x.Clone();
                    clone.Angle = (clone.Angle + angleDelta) % 360;
                    return clone;
                })
                .ToList();
            if (!CanPlaceItems(rotatedItems, rotatedItems.Select(x => x.Id))) return;

            History.Execute(new RotateItemsCommand(Project, itemsToRotate, angleDelta, OnProjectChanged));
        }

        public void GroupSelected()
        {
            if (ActiveSheet == null || SelectedPlacedItemIds.Count < 2) return;

            var itemsToGroup = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            History.Execute(new GroupItemsCommand(Project, itemsToGroup, OnProjectChanged));
        }

        public void UngroupSelected()
        {
            if (ActiveSheet == null || !SelectedPlacedItemIds.Any()) return;

            var itemsToUngroup = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            History.Execute(new UngroupItemsCommand(Project, itemsToUngroup, OnProjectChanged));
        }

        public void ToggleLockSelected(bool lockState)
        {
            if (ActiveSheet == null || !SelectedPlacedItemIds.Any()) return;

            var items = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            History.Execute(new LockItemsCommand(items, lockState, OnProjectChanged));
        }

        public void AlignSelected(string direction)
        {
            if (ActiveSheet == null || SelectedPlacedItemIds.Count < 2) return;

            var items = ActiveSheet.PlacedItems
                .Where(x => SelectedPlacedItemIds.Contains(x.Id) && !x.IsLocked)
                .ToList();

            if (!items.Any()) return;

            // Get product map for dimension calculations
            var productMap = Project.Products.ToDictionary(p => p.Id);

            var itemRects = items.Select(item =>
            {
                productMap.TryGetValue(item.ProductId, out var prod);
                return new { Item = item, Rect = GeometryHelper.GetProductRect(item, prod) };
            }).ToList();

            var newPositions = new Dictionary<Guid, (double X, double Y)>();
            string commandName = "Вирівнювання";

            switch (direction.ToLowerInvariant())
            {
                case "left":
                    float minX = itemRects.Min(r => r.Rect.Left);
                    foreach (var ir in itemRects)
                    {
                        newPositions[ir.Item.Id] = (minX, ir.Item.Y);
                    }
                    commandName = "Вирівняти ліворуч";
                    break;
                case "right":
                    float maxX = itemRects.Max(r => r.Rect.Right);
                    foreach (var ir in itemRects)
                    {
                        newPositions[ir.Item.Id] = (maxX - ir.Rect.Width, ir.Item.Y);
                    }
                    commandName = "Вирівняти праворуч";
                    break;
                case "top":
                    float minY = itemRects.Min(r => r.Rect.Top);
                    foreach (var ir in itemRects)
                    {
                        newPositions[ir.Item.Id] = (ir.Item.X, minY);
                    }
                    commandName = "Вирівняти по верхньому краю";
                    break;
                case "bottom":
                    float maxY = itemRects.Max(r => r.Rect.Bottom);
                    foreach (var ir in itemRects)
                    {
                        newPositions[ir.Item.Id] = (ir.Item.X, maxY - ir.Rect.Height);
                    }
                    commandName = "Вирівняти по нижньому краю";
                    break;
                default:
                    return;
            }

            var alignedItems = items
                .Select(x =>
                {
                    var clone = x.Clone();
                    if (newPositions.TryGetValue(x.Id, out var pos))
                    {
                        clone.X = pos.X;
                        clone.Y = pos.Y;
                    }
                    return clone;
                })
                .ToList();
            if (!CanPlaceItems(alignedItems, items.Select(x => x.Id))) return;

            History.Execute(new MoveItemsToPositionsCommand(Project, items, newPositions, commandName, OnProjectChanged));
        }

        public void DuplicateSelectedAsMatrix(int cols, int rows, double stepX, double stepY)
        {
            if (ActiveSheet == null || SelectedPlacedItemIds.Count != 1) return; // Single item matrix copy
            
            Guid sourceId = SelectedPlacedItemIds.First();
            var sourceItem = ActiveSheet.PlacedItems.FirstOrDefault(x => x.Id == sourceId);
            if (sourceItem == null) return;

            var newItems = new List<PlacedItem>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    // Skip 0,0 since it's the source itself
                    if (r == 0 && c == 0) continue;

                    double newX = sourceItem.X + c * stepX;
                    double newY = sourceItem.Y + r * stepY;

                    newItems.Add(new PlacedItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = sourceItem.ProductId,
                        X = newX,
                        Y = newY,
                        Angle = sourceItem.Angle,
                        IsLocked = false
                    });
                }
            }

            if (!newItems.Any()) return;
            if (!CanPlaceItems(newItems, Enumerable.Empty<Guid>())) return;

            History.Execute(new PlaceItemsCommand(Project, ActiveSheet, newItems, () =>
            {
                SelectedPlacedItemIds.Clear();
                SelectedPlacedItemIds.Add(sourceId);
                foreach (var ni in newItems)
                {
                    SelectedPlacedItemIds.Add(ni.Id);
                }
                OnSelectionChanged();
                OnProjectChanged();
            }));
        }

        public void MoveSelection(double dx, double dy)
        {
            if (ActiveSheet == null || !SelectedPlacedItemIds.Any() || (dx == 0 && dy == 0)) return;

            var itemsToMove = ActiveSheet.PlacedItems.Where(x => SelectedPlacedItemIds.Contains(x.Id)).ToList();
            var movedItems = itemsToMove
                .Where(x => !x.IsLocked)
                .Select(x =>
                {
                    var clone = x.Clone();
                    clone.X += dx;
                    clone.Y += dy;
                    return clone;
                })
                .ToList();
            if (!CanPlaceItems(movedItems, movedItems.Select(x => x.Id))) return;

            History.Execute(new MoveItemsCommand(Project, itemsToMove, dx, dy, OnProjectChanged));
        }

        public bool CanPlaceItems(IEnumerable<PlacedItem> candidateItems, IEnumerable<Guid> ignoredExistingItemIds)
        {
            if (!StrictCollision) return true;
            if (ActiveSheet == null) return false;

            var candidates = candidateItems?.ToList() ?? new List<PlacedItem>();
            if (!candidates.Any()) return true;

            var ignoredIds = new HashSet<Guid>(ignoredExistingItemIds ?? Enumerable.Empty<Guid>());
            var productMap = Project.Products.ToDictionary(p => p.Id);

            foreach (var item in candidates)
            {
                if (!productMap.TryGetValue(item.ProductId, out var product)) return false;
                if (GeometryHelper.IsOutsideSheet(item, product, ActiveSheet)) return false;
                if (GeometryHelper.IsOutsidePrintableArea(item, product, ActiveSheet)) return false;
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                var item = candidates[i];
                var product = productMap[item.ProductId];

                for (int j = i + 1; j < candidates.Count; j++)
                {
                    var other = candidates[j];
                    if (GeometryHelper.CheckCollision(item, product, other, productMap[other.ProductId]))
                    {
                        return false;
                    }
                }

                foreach (var existing in ActiveSheet.PlacedItems)
                {
                    if (ignoredIds.Contains(existing.Id)) continue;
                    if (!productMap.TryGetValue(existing.ProductId, out var existingProduct)) continue;
                    if (GeometryHelper.CheckCollision(item, product, existing, existingProduct))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
