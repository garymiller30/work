using Amazon;
using Interfaces;
using JobSpace.Profiles;
using JobSpace.Statuses;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile = JobSpace.Profiles.Profile;

namespace JobSpace.Fasades
{
    public sealed class SearchManager
    {
        Profile _profile;

        Dictionary<IJobStatus, bool> _viewFilters = new Dictionary<IJobStatus, bool>();
        public SearchManager(Profile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
            // Initialize or load statuses if needed
            LoadStatuses();
        }

        private void LoadStatuses()
        {
            var result = _profile.StatusManager.GetJobStatuses();
            if (result != null)
            {
                foreach (var status in result)
                {
                    _viewFilters.Add(status, true); // Default to true for all statuses
                }
            }
        }


        public Dictionary<IJobStatus, bool> GetStatuses() => _viewFilters;

        public void ChangeStatus(IJobStatus s, bool @checked)
        {
            if (_viewFilters.ContainsKey(s))
            {
                _viewFilters[s] = @checked;
            }
            else
            {
                throw new ArgumentException("Status not found in the view filters.", nameof(s));
            }
        }

        public void Search(string customer, string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) _profile.SearchHistory.Add(text);

            var jobs = _profile.Base.ApplyViewFilter(customer, text, _viewFilters.Where(x => x.Value).Select(x => x.Key.Code).ToArray());

            _profile.Events.Jobs.OnJobsAdd(this, jobs);
        }

        public void ClearFilters()
        {
            var jobs = _profile.Base.ApplyViewFilter(string.Empty, string.Empty, _profile.StatusManager.GetEnabledViewStatuses());

            _profile.Events.Jobs.OnJobsAdd(this, jobs);
        }
    }
}
