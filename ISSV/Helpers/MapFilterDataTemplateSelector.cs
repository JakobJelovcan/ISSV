using ISSV.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ISSV.Helpers
{
    public class MapFilterDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Customer { get; set; }

        public DataTemplate Location { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Customer)
            {
                return Customer;
            }
            else if (item is Location)
            {
                return Location;
            }
            else
            {
                return base.SelectTemplateCore(item);
            }
        }
    }
}
