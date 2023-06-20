using Interfaces;
using Job.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.ModelView
{
    public sealed class CategoryToCustomerView
    {
        private readonly Category _category;

        public bool isChecked { get;set; }
        public string Name => _category.Name;
        public object Id => _category.Id;
        public CategoryToCustomerView(Category category)
        {
            _category = category;
        }

    }
}
