using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Collections.ObjectModel;
using vuapos.Presentation.Models;
using System.Diagnostics;
using vuapos.Presentation.DTO.Product;
using System.Net.Http;
using WinRT.Interop;
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using vuapos.Presentation.Services;
using System.Threading.Tasks;
using vuapos.Presentation.Views.Category;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Product
{

    public sealed partial class ProductPage : UserControl
    {
        private StorageFile? selectedImageFile;
        public ProductViewModel ViewModel { get; }
        private readonly ICategoryService _categoryService;
        private readonly CategoryViewModel _categoryViewModel;
        public ProductPage()
        {
            this.InitializeComponent();
            _categoryService = new CategoryService(new HttpClient());
            _categoryViewModel = new CategoryViewModel();
            ViewModel = new ProductViewModel();
            LoadInitialData();

        }

        private async void LoadInitialData()
        {
            await _categoryViewModel.LoadCategoriesAsync();
            await ViewModel.LoadProductsAsync();
        }
        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (categories == null)
                {
                    throw new Exception("Failed to load categories");
                }

                var stackPanel = new StackPanel { };
                var productCodeTextBox = new TextBox { PlaceholderText = "Enter product code", Width = 300 };
                var productNameTextBox = new TextBox { PlaceholderText = "Enter product name", Width = 300 };
                var categoryComboBox = new ComboBox
                {
                    PlaceholderText = "Select a category",
                    Width = 300,
                    ItemsSource = categories,
                    DisplayMemberPath = "Name",
                    SelectedValuePath = "Category_Id"
                };
                var priceTextBox = new TextBox { PlaceholderText ="Enter price", Width = 300 };
                var costPriceTextBox = new TextBox { PlaceholderText = "Enter cost price", Width = 300 };
                var stockQuantityTextBox = new TextBox { PlaceholderText = "Enter stock quantity", Width = 300 };
                var chooseImageButton = new Button { Content = "Choose Image", Width = 300, HorizontalAlignment = HorizontalAlignment.Left };
                var imageFilePathTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap };
                var errorTextBlock = new TextBlock { Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red), Visibility = Visibility.Collapsed };
                stackPanel.Children.Add(new TextBlock { Text = "Product Code:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(productCodeTextBox);
                stackPanel.Children.Add(new TextBlock { Text = "Product Name:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(productNameTextBox);
                stackPanel.Children.Add(new TextBlock { Text = "Category:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(categoryComboBox);
                stackPanel.Children.Add(new TextBlock { Text = "Price:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(priceTextBox);
                stackPanel.Children.Add(new TextBlock { Text = "Cost Price:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(costPriceTextBox);
                stackPanel.Children.Add(new TextBlock { Text = "Stock Quantity:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(stockQuantityTextBox);
                stackPanel.Children.Add(new TextBlock { Text ="Image:", FontWeight = Microsoft.UI.Text.FontWeights.Bold });
                stackPanel.Children.Add(chooseImageButton);
                stackPanel.Children.Add(imageFilePathTextBlock);
                stackPanel.Children.Add(errorTextBlock);

                var productDialog = new ContentDialog
                {
                    Title = "Add New Product",
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Cancel",
                    Content = stackPanel,
                    XamlRoot = this.XamlRoot,
                    DefaultButton = ContentDialogButton.Primary
                };

                chooseImageButton.Click += async (s, args) =>
                {
                    var picker = new FileOpenPicker();
                    var window = App.m_window as MainWindow;
                    var hwnd = WindowNative.GetWindowHandle(window);
                    InitializeWithWindow.Initialize(picker, hwnd);

                    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");

                    selectedImageFile = await picker.PickSingleFileAsync();
                    if (selectedImageFile != null)
                    {
                        imageFilePathTextBlock.Text = selectedImageFile.Path;
                        errorTextBlock.Visibility = Visibility.Collapsed;
                    }
                };

                productDialog.SecondaryButtonClick += (s, args) => productDialog.Hide();
                productDialog.PrimaryButtonClick += async (s, args) =>
                {
                    try
                    {
                        errorTextBlock.Visibility = Visibility.Collapsed;
                        if (string.IsNullOrWhiteSpace(productCodeTextBox.Text))
                            throw new Exception("Product code is required");
                        if (string.IsNullOrWhiteSpace(productNameTextBox.Text))
                            throw new Exception("Product name is required");
                        if (categoryComboBox.SelectedValue == null)
                            throw new Exception("Please select a category");
                        if (string.IsNullOrWhiteSpace(priceTextBox.Text) || !decimal.TryParse(priceTextBox.Text, out var price))
                            throw new Exception("Price must be a valid number");
                        if (string.IsNullOrWhiteSpace(costPriceTextBox.Text) || !decimal.TryParse(costPriceTextBox.Text, out var costPrice))
                            throw new Exception("Cost price must be a valid number");
                        if (string.IsNullOrWhiteSpace(stockQuantityTextBox.Text) || !int.TryParse(stockQuantityTextBox.Text, out var stockQuantity))
                            throw new Exception("Stock quantity must be a valid integer");
                        if (selectedImageFile == null)
                            throw new Exception("Image is required");
                        var productCode = productCodeTextBox.Text;
                        var productName = productNameTextBox.Text;
                        var categoryId = categoryComboBox.SelectedValue.ToString();

                        await ViewModel.AddProductAsync(productCode, productName, categoryId, price, costPrice, stockQuantity, selectedImageFile);

                        productDialog.Hide();
                        selectedImageFile = null;
                    }
                    catch (Exception ex)
                    {
                        errorTextBlock.Text = $"Error: {ex.Message}";
                        errorTextBlock.Visibility = Visibility.Visible;
                        args.Cancel = true;
                    }
                };

                await productDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to open dialog: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                var categories = _categoryService.GetAllCategoriesAsync();

                var editProductDialog = new EditProductDialog(ViewModel, _categoryViewModel, product);
                editProductDialog.XamlRoot = this.XamlRoot;
                var result = await editProductDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
            }
        }

        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;

            var confirmDialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = $"Are you sure you want to delete '{product?.Product_Name}'?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                XamlRoot = this.XamlRoot,
                DefaultButton = ContentDialogButton.Secondary
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await ViewModel.DeleteProductAsync(product.Product_Id);
                if (success)
                {
                    await ViewModel.LoadProductsAsync();
                }
            }
            
        }
    }
}