using System;
using System.Collections.Generic;
using System.Linq;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Services;

namespace JobSpace.Static.Pdf.SheetCalculator.Commands
{
    public class AddSheetCommand : ICommand
    {
        private readonly Project _project;
        private readonly Sheet _sheet;
        private readonly Action _onChanged;

        public string Name => "Додати лист";

        public AddSheetCommand(Project project, Sheet sheet, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            _project.Sheets.Add(_sheet);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Sheets.Remove(_sheet);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }

    public class RemoveSheetCommand : ICommand
    {
        private readonly Project _project;
        private readonly Sheet _sheet;
        private readonly List<PlacedItemGroup> _removedGroups;
        private readonly Dictionary<Guid, Guid?> _oldGroupIds;
        private readonly Action _onChanged;

        public string Name => "Видалити лист";

        public RemoveSheetCommand(Project project, Sheet sheet, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _onChanged = onChanged;

            var removedItemIds = new HashSet<Guid>(_sheet.PlacedItems.Select(x => x.Id));
            _removedGroups = _project.Groups
                .Where(g => g.PlacedItemIds.Any(id => removedItemIds.Contains(id)))
                .ToList();

            var affectedGroupIds = new HashSet<Guid>(_removedGroups.Select(g => g.Id));
            _oldGroupIds = _project.Sheets
                .SelectMany(s => s.PlacedItems)
                .Where(i => i.GroupId.HasValue && affectedGroupIds.Contains(i.GroupId.Value))
                .ToDictionary(i => i.Id, i => i.GroupId);
        }

        public void Execute()
        {
            _project.Sheets.Remove(_sheet);

            foreach (var group in _removedGroups)
            {
                _project.Groups.Remove(group);
            }

            foreach (var item in _project.Sheets.SelectMany(s => s.PlacedItems))
            {
                if (_oldGroupIds.ContainsKey(item.Id))
                {
                    item.GroupId = null;
                }
            }

            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Sheets.Add(_sheet);

            foreach (var group in _removedGroups)
            {
                if (!_project.Groups.Any(g => g.Id == group.Id))
                {
                    _project.Groups.Add(group);
                }
            }

            foreach (var item in _project.Sheets.SelectMany(s => s.PlacedItems))
            {
                if (_oldGroupIds.TryGetValue(item.Id, out var groupId))
                {
                    item.GroupId = groupId;
                }
            }

            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }
    public class UpdateSheetCommand : ICommand
    {
        private readonly Sheet _sheet;
        private readonly Sheet _oldState;
        private readonly Sheet _newState;
        private readonly Project _project;
        private readonly Action _onChanged;

        public string Name => "Змінити параметри листа";

        public UpdateSheetCommand(Project project, Sheet sheet, Sheet newState, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _oldState = sheet.Clone();
            _newState = newState;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            CopyState(_newState, _sheet);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            CopyState(_oldState, _sheet);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        private void CopyState(Sheet src, Sheet dst)
        {
            dst.Name = src.Name;
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.MarginLeft = src.MarginLeft;
            dst.MarginRight = src.MarginRight;
            dst.MarginTop = src.MarginTop;
            dst.MarginBottom = src.MarginBottom;
        }
    }

    public class AddProductCommand : ICommand
    {
        private readonly Project _project;
        private readonly Product _product;
        private readonly Action _onChanged;

        public string Name => "Додати виріб";

        public AddProductCommand(Project project, Product product, Action onChanged)
        {
            _project = project;
            _product = product;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            _project.Products.Add(_product);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Products.Remove(_product);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }

    public class RemoveProductCommand : ICommand
    {
        private readonly Project _project;
        private readonly Product _product;
        private readonly Dictionary<Guid, List<PlacedItem>> _sheetPlacements = new Dictionary<Guid, List<PlacedItem>>();
        private readonly List<PlacedItemGroup> _removedGroups;
        private readonly Dictionary<Guid, Guid?> _oldGroupIds;
        private readonly Action _onChanged;

        public string Name => "Видалити виріб";

        public RemoveProductCommand(Project project, Product product, Action onChanged)
        {
            _project = project;
            _product = product;
            _onChanged = onChanged;

            // Remember all placed instances of this product across all sheets.
            foreach (var sheet in _project.Sheets)
            {
                var matches = sheet.PlacedItems.Where(x => x.ProductId == product.Id).ToList();
                if (matches.Any())
                {
                    _sheetPlacements[sheet.Id] = matches;
                }
            }

            var removedItemIds = new HashSet<Guid>(_sheetPlacements.Values.SelectMany(x => x).Select(x => x.Id));
            _removedGroups = _project.Groups
                .Where(g => g.PlacedItemIds.Any(id => removedItemIds.Contains(id)))
                .ToList();

            var affectedGroupIds = new HashSet<Guid>(_removedGroups.Select(g => g.Id));
            _oldGroupIds = _project.Sheets
                .SelectMany(s => s.PlacedItems)
                .Where(i => i.GroupId.HasValue && affectedGroupIds.Contains(i.GroupId.Value))
                .ToDictionary(i => i.Id, i => i.GroupId);
        }

        public void Execute()
        {
            _project.Products.Remove(_product);
            foreach (var sheet in _project.Sheets)
            {
                if (_sheetPlacements.TryGetValue(sheet.Id, out var items))
                {
                    foreach (var item in items)
                    {
                        sheet.PlacedItems.Remove(item);
                    }
                }
            }

            foreach (var group in _removedGroups)
            {
                _project.Groups.Remove(group);
            }

            foreach (var item in _project.Sheets.SelectMany(s => s.PlacedItems))
            {
                if (_oldGroupIds.ContainsKey(item.Id))
                {
                    item.GroupId = null;
                }
            }

            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Products.Add(_product);
            foreach (var sheet in _project.Sheets)
            {
                if (_sheetPlacements.TryGetValue(sheet.Id, out var items))
                {
                    sheet.PlacedItems.AddRange(items);
                }
            }

            foreach (var group in _removedGroups)
            {
                if (!_project.Groups.Any(g => g.Id == group.Id))
                {
                    _project.Groups.Add(group);
                }
            }

            foreach (var item in _project.Sheets.SelectMany(s => s.PlacedItems))
            {
                if (_oldGroupIds.TryGetValue(item.Id, out var groupId))
                {
                    item.GroupId = groupId;
                }
            }

            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }
    public class UpdateProductCommand : ICommand
    {
        private readonly Product _product;
        private readonly Product _oldState;
        private readonly Product _newState;
        private readonly Project _project;
        private readonly Action _onChanged;

        public string Name => "Змінити параметри виробу";

        public UpdateProductCommand(Project project, Product product, Product newState, Action onChanged)
        {
            _project = project;
            _product = product;
            _oldState = product.Clone();
            _newState = newState;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            CopyState(_newState, _product);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            CopyState(_oldState, _product);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        private void CopyState(Product src, Product dst)
        {
            dst.Name = src.Name;
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.RequiredCirculation = src.RequiredCirculation;
            dst.TechMargin = src.TechMargin;
        }
    }
}
