using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpace.Models
{
    public class JobListFilter
    {
        public string Customer { get; set; } = string.Empty;
        public int[] Statuses { get; set; } = new int[0]; 
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Text { get; set; } = string.Empty;

        public bool IsStatuses => Statuses != null && Statuses.Length > 0;
        public bool IsCustomer => !string.IsNullOrEmpty(Customer);
        public bool IsDate => Date != DateTime.MinValue;
        public bool IsText => !string.IsNullOrEmpty(Text);
    }
}
