using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.FrequentlyBoughtTogether;

namespace vuapos.Presentation.Models
{
    public class FrequentlyBoughtTogetherViewModel
    {
        private readonly FrequentlyBoughtTogetherService _frequentlyBoughtTogetherService;
        public ObservableCollection<ProductGroup> ProductGroups { get; } = new ObservableCollection<ProductGroup>();
        private bool _isLoaded = false;
        public FrequentlyBoughtTogetherViewModel()
        {
            _frequentlyBoughtTogetherService = App.Services.GetRequiredService<FrequentlyBoughtTogetherService>();

        }
        public async Task LoadFrequentlyBoughtTogetherAsync()
        {
            try
            {
                if (_isLoaded) 
                    return;
                var response = await _frequentlyBoughtTogetherService.GetFrequentlyBoughtTogetherAsync();
                Debug.WriteLine("test ---------------------");
                Debug.WriteLine(response);
                if (response != null)
                {
                    ProductGroups.Clear();
                    int i = 1;
                    foreach (var group in response.Groups)
                    {
                        group.GroupName = $"Group #{i++}";
                        ProductGroups.Add(group);
                    }
                    Debug.WriteLine("test pridct group ---------------------");
                    Debug.WriteLine(ProductGroups[0].GroupItems[0].ImagePath);
                    _isLoaded = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading frequently bought together groups: {ex.Message}");
            }
        }
    }
}
