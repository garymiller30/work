using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using JobSpace.Static.Pdf.SheetCalculator.Models;
using JobSpace.Static.Pdf.SheetCalculator.Services;

namespace JobSpace.Static.Pdf.SheetCalculator.Commands
{
    public class PlaceItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly Sheet _sheet;
        private readonly List<PlacedItem> _items;
        private readonly Action _onChanged;

        public string Name => "Розмістити вироби";

        public PlaceItemsCommand(Project project, Sheet sheet, List<PlacedItem> items, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _items = items;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            _sheet.PlacedItems.AddRange(_items);
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var item in _items)
            {
                _sheet.PlacedItems.Remove(item);
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }

    public class RemoveItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly Sheet _sheet;
        private readonly List<PlacedItem> _items;
        private readonly List<PlacedItemGroup> _removedGroups;
        private readonly Dictionary<Guid, Guid?> _oldGroupIds;
        private readonly Action _onChanged;

        public string Name => "Видалити розкладені вироби";

        public RemoveItemsCommand(Project project, Sheet sheet, List<PlacedItem> items, Action onChanged)
        {
            _project = project;
            _sheet = sheet;
            _items = items;
            _onChanged = onChanged;

            var removedItemIds = new HashSet<Guid>(_items.Select(x => x.Id));
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
            foreach (var item in _items)
            {
                _sheet.PlacedItems.Remove(item);
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
            _sheet.PlacedItems.AddRange(_items);

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
    public class MoveItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly List<PlacedItem> _items;
        private readonly double _dx;
        private readonly double _dy;
        private readonly Action _onChanged;

        public string Name => "Перемістити вироби";

        public MoveItemsCommand(Project project, List<PlacedItem> items, double dx, double dy, Action onChanged)
        {
            _project = project;
            // Only move unlocked items
            _items = items.Where(x => !x.IsLocked).ToList();
            _dx = dx;
            _dy = dy;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            foreach (var item in _items)
            {
                item.X += _dx;
                item.Y += _dy;
            }
            // Move calculations don't change counts, so recalculation is optional but good for consistency
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var item in _items)
            {
                item.X -= _dx;
                item.Y -= _dy;
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }

    public class MoveItemsToPositionsCommand : ICommand
    {
        private readonly Project _project;
        private readonly List<PlacedItem> _items;
        private readonly Dictionary<Guid, (double X, double Y)> _oldPositions;
        private readonly Dictionary<Guid, (double X, double Y)> _newPositions;
        private readonly string _commandName;
        private readonly Action _onChanged;

        public string Name => _commandName;

        public MoveItemsToPositionsCommand(
            Project project,
            List<PlacedItem> items,
            Dictionary<Guid, (double X, double Y)> newPositions,
            string commandName,
            Action onChanged)
        {
            _project = project;
            _items = items.Where(x => !x.IsLocked).ToList();
            _newPositions = newPositions;
            _commandName = commandName;
            _onChanged = onChanged;
            _oldPositions = _items.ToDictionary(x => x.Id, x => (x.X, x.Y));
        }

        public void Execute()
        {
            foreach (var item in _items)
            {
                if (_newPositions.TryGetValue(item.Id, out var pos))
                {
                    item.X = pos.X;
                    item.Y = pos.Y;
                }
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var item in _items)
            {
                if (_oldPositions.TryGetValue(item.Id, out var pos))
                {
                    item.X = pos.X;
                    item.Y = pos.Y;
                }
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }


    public class RotateItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly List<PlacedItem> _items;
        private readonly Dictionary<Guid, int> _oldAngles;
        private readonly int _angleDelta; // e.g. 90, 180, 270
        private readonly Action _onChanged;

        public string Name => "Повернути вироби";

        public RotateItemsCommand(Project project, List<PlacedItem> items, int angleDelta, Action onChanged)
        {
            _project = project;
            _items = items.Where(x => !x.IsLocked).ToList();
            _angleDelta = angleDelta;
            _onChanged = onChanged;
            _oldAngles = _items.ToDictionary(x => x.Id, x => x.Angle);
        }

        public void Execute()
        {
            foreach (var item in _items)
            {
                item.Angle = (item.Angle + _angleDelta) % 360;
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var item in _items)
            {
                if (_oldAngles.TryGetValue(item.Id, out int oldAngle))
                {
                    item.Angle = oldAngle;
                }
            }
            CalculationService.Recalculate(_project);
            _onChanged?.Invoke();
        }
    }

    public class GroupItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly List<PlacedItem> _items;
        private readonly PlacedItemGroup _group;
        private readonly Action _onChanged;

        public string Name => "Згрупувати вироби";

        public GroupItemsCommand(Project project, List<PlacedItem> items, Action onChanged)
        {
            _project = project;
            _items = items;
            _onChanged = onChanged;

            _group = new PlacedItemGroup
            {
                Id = Guid.NewGuid(),
                PlacedItemIds = items.Select(x => x.Id).ToList()
            };
        }

        public void Execute()
        {
            _project.Groups.Add(_group);
            foreach (var item in _items)
            {
                item.GroupId = _group.Id;
            }
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _project.Groups.Remove(_group);
            foreach (var item in _items)
            {
                item.GroupId = null;
            }
            _onChanged?.Invoke();
        }
    }

    public class UngroupItemsCommand : ICommand
    {
        private readonly Project _project;
        private readonly List<PlacedItemGroup> _groups;
        private readonly Dictionary<Guid, Guid> _itemToGroupMap;
        private readonly Action _onChanged;

        public string Name => "Розгрупувати вироби";

        public UngroupItemsCommand(Project project, List<PlacedItem> items, Action onChanged)
        {
            _project = project;
            _onChanged = onChanged;

            // Find all unique groups associated with these items
            var groupIds = items.Where(x => x.GroupId.HasValue).Select(x => x.GroupId.Value).Distinct().ToList();
            _groups = _project.Groups.Where(x => groupIds.Contains(x.Id)).ToList();

            _itemToGroupMap = new Dictionary<Guid, Guid>();
            foreach (var g in _groups)
            {
                foreach (var itemId in g.PlacedItemIds)
                {
                    _itemToGroupMap[itemId] = g.Id;
                }
            }
        }

        public void Execute()
        {
            foreach (var g in _groups)
            {
                _project.Groups.Remove(g);
            }
            
            // Set matching items GroupId to null
            foreach (var itemId in _itemToGroupMap.Keys)
            {
                var sheet = _project.Sheets.FirstOrDefault(s => s.PlacedItems.Any(i => i.Id == itemId));
                var item = sheet?.PlacedItems.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    item.GroupId = null;
                }
            }
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var g in _groups)
            {
                _project.Groups.Add(g);
            }

            foreach (var kvp in _itemToGroupMap)
            {
                var itemId = kvp.Key;
                var groupId = kvp.Value;
                var sheet = _project.Sheets.FirstOrDefault(s => s.PlacedItems.Any(i => i.Id == itemId));
                var item = sheet?.PlacedItems.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    item.GroupId = groupId;
                }
            }
            _onChanged?.Invoke();
        }
    }

    public class LockItemsCommand : ICommand
    {
        private readonly List<PlacedItem> _items;
        private readonly bool _lockState;
        private readonly Dictionary<Guid, bool> _oldLockStates;
        private readonly Action _onChanged;

        public string Name => _lockState ? "Заблокувати вироби" : "Розблокувати вироби";

        public LockItemsCommand(List<PlacedItem> items, bool lockState, Action onChanged)
        {
            _items = items;
            _lockState = lockState;
            _onChanged = onChanged;
            _oldLockStates = items.ToDictionary(x => x.Id, x => x.IsLocked);
        }

        public void Execute()
        {
            foreach (var item in _items)
            {
                item.IsLocked = _lockState;
            }
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            foreach (var item in _items)
            {
                if (_oldLockStates.TryGetValue(item.Id, out bool oldState))
                {
                    item.IsLocked = oldState;
                }
            }
            _onChanged?.Invoke();
        }
    }
}
