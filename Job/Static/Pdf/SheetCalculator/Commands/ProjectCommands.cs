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
        private readonly Action _onChanged;

        public string Name => "Видалити лист";

        public RemoveSheetCommand(Project project, Sheet sheet, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            _project.Sheets.Remove(_sheet);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Sheets.Add(_sheet);
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
            dst.PlacedItems = src.PlacedItems.Select(x => x.Clone()).ToList();
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
        private readonly List<PlacedItem> _removedPlacements = new List<PlacedItem>();
        private readonly Dictionary<Guid, List<PlacedItem>> _sheetPlacements = new Dictionary<Guid, List<PlacedItem>>();
        private readonly Action _onChanged;

        public string Name => "Видалити виріб";

        public RemoveProductCommand(Project project, Product product, Action onChanged)
        {
            _project = project;
            _product = product;
            _onChanged = onChanged;

            // Remember all placed instances of this product across all sheets
            foreach (var sheet in _project.Sheets)
            {
                var matches = sheet.PlacedItems.Where(x => x.ProductId == product.Id).ToList();
                if (matches.Any())
                {
                    _sheetPlacements[sheet.Id] = matches;
                }
            }
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
