using System;
using System.Collections.Generic;
using Interfaces;

namespace JobSpace.Profiles
{
    public sealed class SearchHistory : ISearchHistory
    {
        private readonly Stack<string> _queue = new Stack<string>(10);
        IUserProfile _profile;

        public SearchHistory(IUserProfile profile)
        {
            _profile = profile;

            LoadHistory();
            
        }

        private void LoadHistory()
        {
            var history = _profile.LoadSettings<SearchHistorySettings>().SearchHistory;

            if (history != null)
            {
                int limit = 20;

                for (int i = 0; i < history.Length; i++)
                {
                    _queue.Push(history[i]);
                    limit--;
                    if (limit == 0) break;
                }
            }

        }

        public void Add(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (_queue.Count > 0)
                {
                    var s = _queue.Peek();
                    if (!str.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                        _queue.Push(str);
                }
                else
                {
                    _queue.Push(str);
                }
                SaveHistory();
            }
        }

        private void SaveHistory()
        {
            _profile.SaveSettings(new SearchHistorySettings { SearchHistory = GetHistory() });
        }

        public string[] GetHistory()
        {
            return _queue.ToArray();
        }
    }
}
