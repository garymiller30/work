using Interfaces;

namespace PluginFileSearch
{
    internal sealed class CustomerFilterItem
    {
        public CustomerFilterItem(ICustomer customer)
        {
            Customer = customer;
        }

        public ICustomer Customer { get; }
        public string Name => Customer?.Name ?? "Усі замовники";
        public override string ToString() => Name;
    }
}
