using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Rendering;
using JobSpace.Static.Pdf.SheetCalculator.Utilities;
using JobSpace.Static.Pdf.SheetCalculator.ViewModels;
using JobSpace.Static.Pdf.SheetCalculator.Commands;

namespace JobSpace.Static.Pdf.SheetCalculator.Views
{
    public class EditorControl : UserControl
    {
        private MainViewModel _viewModel;
        public MainViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    if (_viewModel != null)
                    {
                        _viewModel.RedrawRequested -= OnRedrawRequested;
                    }
                    _viewModel = value;
                    if (_viewModel != null)
                    {
                        _viewModel.RedrawRequested += OnRedrawRequested;
                    }
                    Invalidate();
                }
            }
        }

        // Mouse Drag States
        private enum InteractionMode { None, Pan, DragItems, Marquee }
        private InteractionMode _mode = InteractionMode.None;

        private PointF _mouseStartScreen;
        private PointF _panStartOffset;

        private PointF _dragStartWorld;
        private Dictionary<Guid, (double X, double Y)> _dragStartItemPositions = new Dictionary<Guid, (double X, double Y)>();
        private Guid _primaryDragItemId;

        private PointF _marqueeStartScreen;
        private RectangleF? _marqueeRectScreen;

        private PointF _currentMousePosScreen;

        public EditorControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = GdiRenderer.ColorBg;
        }

        private void OnRedrawRequested()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ViewModel == null) return;

            // Gather products for rendering
            var productMap = ViewModel.Project.Products.ToDictionary(p => p.Id);

            GdiRenderer.Render(
                e.Graphics,
                Width,
                Height,
                ViewModel.ActiveSheet,
                productMap,
                ViewModel.SelectedPlacedItemIds,
                _marqueeRectScreen,
                ViewModel.Zoom,
                ViewModel.PanOffset,
                _currentMousePosScreen,
                ViewModel.GridSnapEnabled,
                ViewModel.GridSize,
                ViewModel.ActiveSnapX,
                ViewModel.ActiveSnapY
            );
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ViewModel == null || ViewModel.ActiveSheet == null) return;

            _currentMousePosScreen = e.Location;

            // 1. Panning Mode (Right or Middle Click)
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                _mode = InteractionMode.Pan;
                _mouseStartScreen = e.Location;
                _panStartOffset = ViewModel.PanOffset;
                Cursor = Cursors.Hand;
                return;
            }

            // 2. Left Click actions
            if (e.Button == MouseButtons.Left)
            {
                // Check if mouse is in the canvas drawing area (not on rulers)
                if (e.X < GdiRenderer.RulerSize || e.Y < GdiRenderer.RulerSize)
                {
                    return; // clicked on rulers
                }

                PointF worldClick = GeometryHelper.ScreenToWorld(e.Location, ViewModel.Zoom, ViewModel.PanOffset);
                var productMap = ViewModel.Project.Products.ToDictionary(p => p.Id);

                // Find clicked item (check top-most first)
                PlacedItem clickedItem = null;
                for (int i = ViewModel.ActiveSheet.PlacedItems.Count - 1; i >= 0; i--)
                {
                    var item = ViewModel.ActiveSheet.PlacedItems[i];
                    if (productMap.TryGetValue(item.ProductId, out var prod))
                    {
                        RectangleF rect = GeometryHelper.GetProductRect(item, prod);
                        if (rect.Contains(worldClick))
                        {
                            clickedItem = item;
                            break;
                        }
                    }
                }

                if (clickedItem != null)
                {
                    _mode = InteractionMode.DragItems;
                    _dragStartWorld = worldClick;
                    _primaryDragItemId = clickedItem.Id;

                    // Handle Multi-Selection / Group Selection
                    bool isModifierPressed = (ModifierKeys & Keys.Control) == Keys.Control || (ModifierKeys & Keys.Shift) == Keys.Shift;

                    if (!ViewModel.SelectedPlacedItemIds.Contains(clickedItem.Id))
                    {
                        if (!isModifierPressed)
                        {
                            ViewModel.SelectedPlacedItemIds.Clear();
                        }

                        // If item belongs to a group, select the entire group!
                        if (clickedItem.GroupId.HasValue)
                        {
                            var group = ViewModel.Project.Groups.FirstOrDefault(g => g.Id == clickedItem.GroupId.Value);
                            if (group != null)
                            {
                                foreach (var id in group.PlacedItemIds)
                                {
                                    ViewModel.SelectedPlacedItemIds.Add(id);
                                }
                            }
                        }
                        else
                        {
                            ViewModel.SelectedPlacedItemIds.Add(clickedItem.Id);
                        }
                        
                        ViewModel.OnSelectionChanged();
                    }

                    // Record start positions for all selected items (for drag movement and undo registration)
                    _dragStartItemPositions.Clear();
                    foreach (var itemId in ViewModel.SelectedPlacedItemIds)
                    {
                        var item = ViewModel.ActiveSheet.PlacedItems.FirstOrDefault(x => x.Id == itemId);
                        if (item != null)
                        {
                            _dragStartItemPositions[itemId] = (item.X, item.Y);
                        }
                    }
                }
                else
                {
                    // Clicked on empty space -> start Marquee Selection
                    _mode = InteractionMode.Marquee;
                    _marqueeStartScreen = e.Location;
                    _marqueeRectScreen = new RectangleF(e.X, e.Y, 0, 0);

                    if ((ModifierKeys & Keys.Control) != Keys.Control && (ModifierKeys & Keys.Shift) != Keys.Shift)
                    {
                        ViewModel.SelectedPlacedItemIds.Clear();
                        ViewModel.OnSelectionChanged();
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ViewModel == null) return;

            _currentMousePosScreen = e.Location;

            // Update ruler guide tracker lines
            Invalidate();

            if (_mode == InteractionMode.Pan)
            {
                float dx = e.X - _mouseStartScreen.X;
                float dy = e.Y - _mouseStartScreen.Y;
                ViewModel.PanOffset = new PointF(_panStartOffset.X + dx, _panStartOffset.Y + dy);
                ViewModel.TriggerRedraw();
            }
            else if (_mode == InteractionMode.DragItems && ViewModel.ActiveSheet != null)
            {
                PointF worldPos = GeometryHelper.ScreenToWorld(e.Location, ViewModel.Zoom, ViewModel.PanOffset);
                double dx = worldPos.X - _dragStartWorld.X;
                double dy = worldPos.Y - _dragStartWorld.Y;

                // Move items temporarily on screen
                var productMap = ViewModel.Project.Products.ToDictionary(p => p.Id);
                var itemsToMove = ViewModel.ActiveSheet.PlacedItems.Where(x => ViewModel.SelectedPlacedItemIds.Contains(x.Id) && !x.IsLocked).ToList();

                // Snapping check (only snap if single or based on the primary item dragged)
                var primaryItem = itemsToMove.FirstOrDefault(x => x.Id == _primaryDragItemId);
                
                ViewModel.ActiveSnapX.Clear();
                ViewModel.ActiveSnapY.Clear();

                double finalDx = dx;
                double finalDy = dy;

                if (primaryItem != null && productMap.TryGetValue(primaryItem.ProductId, out var primaryProd))
                {
                    // Compute what primary position would be without snap
                    if (_dragStartItemPositions.TryGetValue(primaryItem.Id, out var startPos))
                    {
                        double targetX = startPos.X + dx;
                        double targetY = startPos.Y + dy;

                        // Create temporary PlacedItem with target position to calculate snapping
                        var tempItem = new PlacedItem
                        {
                            ProductId = primaryItem.ProductId,
                            X = targetX,
                            Y = targetY,
                            Angle = primaryItem.Angle
                        };

                        var otherItems = ViewModel.ActiveSheet.PlacedItems
                            .Where(x => !ViewModel.SelectedPlacedItemIds.Contains(x.Id))
                            .ToList();

                        // 5px threshold
                        var snapRes = GeometryHelper.CalculateSnap(
                            tempItem,
                            primaryProd,
                            otherItems,
                            productMap,
                            ViewModel.ActiveSheet,
                            ViewModel.GridSnapEnabled,
                            ViewModel.GridSize,
                            5.0,
                            ViewModel.Zoom,
                            ViewModel.PanOffset
                        );

                        // Snapped positions
                        finalDx = snapRes.SnappedX - startPos.X;
                        finalDy = snapRes.SnappedY - startPos.Y;

                        ViewModel.ActiveSnapX.AddRange(snapRes.VerticalSnapLines);
                        ViewModel.ActiveSnapY.AddRange(snapRes.HorizontalSnapLines);
                    }
                }

                // Update items in real-time
                foreach (var item in itemsToMove)
                {
                    if (_dragStartItemPositions.TryGetValue(item.Id, out var startPos))
                    {
                        item.X = startPos.X + finalDx;
                        item.Y = startPos.Y + finalDy;
                    }
                }

                ViewModel.TriggerRedraw();
            }
            else if (_mode == InteractionMode.Marquee)
            {
                // Calculate selection rect
                float x = Math.Min(_marqueeStartScreen.X, e.X);
                float y = Math.Min(_marqueeStartScreen.Y, e.Y);
                float w = Math.Abs(_marqueeStartScreen.X - e.X);
                float h = Math.Abs(_marqueeStartScreen.Y - e.Y);

                // Constrain marquee within canvas drawing area
                if (x < GdiRenderer.RulerSize) { w -= (GdiRenderer.RulerSize - x); x = GdiRenderer.RulerSize; }
                if (y < GdiRenderer.RulerSize) { h -= (GdiRenderer.RulerSize - y); y = GdiRenderer.RulerSize; }
                if (w < 0) w = 0;
                if (h < 0) h = 0;

                _marqueeRectScreen = new RectangleF(x, y, w, h);

                // Perform real-time selection check
                if (ViewModel.ActiveSheet != null)
                {
                    RectangleF marqueeWorld = GeometryHelper.ScreenToWorld(_marqueeRectScreen.Value, ViewModel.Zoom, ViewModel.PanOffset);
                    var productMap = ViewModel.Project.Products.ToDictionary(p => p.Id);

                    for (int i = 0; i < ViewModel.ActiveSheet.PlacedItems.Count; i++)
                    {
                        var item = ViewModel.ActiveSheet.PlacedItems[i];
                        if (productMap.TryGetValue(item.ProductId, out var prod))
                        {
                            RectangleF itemRect = GeometryHelper.GetProductRect(item, prod);
                            bool intersects = marqueeWorld.IntersectsWith(itemRect);

                            if (intersects)
                            {
                                // Add to selection
                                if (item.GroupId.HasValue)
                                {
                                    // Add the whole group
                                    var group = ViewModel.Project.Groups.FirstOrDefault(g => g.Id == item.GroupId.Value);
                                    if (group != null)
                                    {
                                        foreach (var id in group.PlacedItemIds)
                                        {
                                            ViewModel.SelectedPlacedItemIds.Add(id);
                                        }
                                    }
                                }
                                else
                                {
                                    ViewModel.SelectedPlacedItemIds.Add(item.Id);
                                }
                            }
                            else
                            {
                                // Remove if ctrl/shift is not held
                                bool isModifierPressed = (ModifierKeys & Keys.Control) == Keys.Control || (ModifierKeys & Keys.Shift) == Keys.Shift;
                                if (!isModifierPressed)
                                {
                                    ViewModel.SelectedPlacedItemIds.Remove(item.Id);
                                }
                            }
                        }
                    }
                    ViewModel.OnSelectionChanged();
                }
                ViewModel.TriggerRedraw();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ViewModel == null) return;

            if (_mode == InteractionMode.Pan)
            {
                _mode = InteractionMode.None;
                Cursor = Cursors.Default;
            }
            else if (_mode == InteractionMode.DragItems && ViewModel.ActiveSheet != null)
            {
                // Clear snaps
                ViewModel.ActiveSnapX.Clear();
                ViewModel.ActiveSnapY.Clear();

                // Compute final movement and register in command history to support Undo/Redo
                var itemsMoved = ViewModel.ActiveSheet.PlacedItems.Where(x => ViewModel.SelectedPlacedItemIds.Contains(x.Id) && !x.IsLocked).ToList();
                
                bool actuallyMoved = false;
                var newPositions = new Dictionary<Guid, (double X, double Y)>();

                foreach (var item in itemsMoved)
                {
                    if (_dragStartItemPositions.TryGetValue(item.Id, out var startPos))
                    {
                        if (Math.Abs(item.X - startPos.X) > 0.001 || Math.Abs(item.Y - startPos.Y) > 0.001)
                        {
                            actuallyMoved = true;
                        }
                    }
                    newPositions[item.Id] = (item.X, item.Y);
                }

                if (actuallyMoved)
                {
                    // Restore original positions first so Command.Execute works cleanly and supports Redo
                    foreach (var item in itemsMoved)
                    {
                        if (_dragStartItemPositions.TryGetValue(item.Id, out var startPos))
                        {
                            item.X = startPos.X;
                            item.Y = startPos.Y;
                        }
                    }

                    // Execute command
                    ViewModel.History.Execute(new MoveItemsToPositionsCommand(
                        ViewModel.Project,
                        itemsMoved,
                        newPositions,
                        "Перемістити вироби",
                        ViewModel.OnProjectChanged
                    ));
                }

                _mode = InteractionMode.None;
                _dragStartItemPositions.Clear();
            }
            else if (_mode == InteractionMode.Marquee)
            {
                _mode = InteractionMode.None;
                _marqueeRectScreen = null;
                ViewModel.OnSelectionChanged();
            }

            _mode = InteractionMode.None;
            ViewModel.TriggerRedraw();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (ViewModel == null) return;

            // Zoom centered on cursor position
            float oldZoom = ViewModel.Zoom;
            float zoomFactor = e.Delta > 0 ? 1.15f : 0.85f;
            float newZoom = Math.Max(0.05f, Math.Min(20f, oldZoom * zoomFactor));

            PointF cursor = e.Location;
            PointF worldCursor = GeometryHelper.ScreenToWorld(cursor, oldZoom, ViewModel.PanOffset);

            ViewModel.Zoom = newZoom;
            ViewModel.PanOffset = new PointF(
                cursor.X - worldCursor.X * newZoom,
                cursor.Y - worldCursor.Y * newZoom
            );

            ViewModel.TriggerRedraw();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}
