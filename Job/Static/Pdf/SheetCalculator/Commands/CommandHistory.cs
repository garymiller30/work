using System;
using System.Collections.Generic;

namespace JobSpace.Static.Pdf.SheetCalculator.Commands
{
    public class CommandHistory
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public event EventHandler HistoryChanged;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void Execute(ICommand command)
        {
            if (command == null) return;
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetUndoName()
        {
            return _undoStack.Count > 0 ? _undoStack.Peek().Name : string.Empty;
        }

        public string GetRedoName()
        {
            return _redoStack.Count > 0 ? _redoStack.Peek().Name : string.Empty;
        }
    }
}
