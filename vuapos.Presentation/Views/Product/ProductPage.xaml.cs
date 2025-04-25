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
using OfficeOpenXml;

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
        private readonly ProductService _productService;
        public ProductPage()
        {
            this.InitializeComponent();
            _categoryService = new CategoryService(new HttpClient());
            _categoryViewModel = new CategoryViewModel();
            _productService = new ProductService(new HttpClient());
            ViewModel = new ProductViewModel();
            LoadInitialData();

        }
        private async void ProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            await _categoryViewModel.LoadCategoriesAsync();
            await ViewModel.LoadProductsAsync();
            //ProductListView.ItemsSource = ViewModel.Products;
        }
        private async void LoadInitialData()
        {
            await _categoryViewModel.LoadCategoriesAsync();
            await ViewModel.LoadProductsAsync();
            CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addProductDialog = new AddProductDialog(ViewModel, _categoryViewModel, _productService)
                {
                    XamlRoot = this.XamlRoot
                };
                await addProductDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
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
        private async void ImportProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var importErrors = new List<ImportError>();

                var importProductsDialog = new ImportExcelProduct(ViewModel, _productService, this.XamlRoot)
                {
                    XamlRoot = this.XamlRoot
                };

                await importProductsDialog.ShowAsync();
                await ViewModel.LoadProductsAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"Failed to open import dialog: {ex.Message}");
            }
        }
        //private async void ImportProducts_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var filePicker = new FileOpenPicker();
        //        var window = App.m_window as MainWindow;
        //        var hwnd = WindowNative.GetWindowHandle(window);
        //        InitializeWithWindow.Initialize(filePicker, hwnd);

        //        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        //        filePicker.FileTypeFilter.Add(".xlsx");

        //        var excelFile = await filePicker.PickSingleFileAsync();
        //        if (excelFile == null)
        //            return;

        //        var folderPicker = new FolderPicker();
        //        InitializeWithWindow.Initialize(folderPicker, hwnd);
        //        folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //        folderPicker.FileTypeFilter.Add("*");

        //        var imageFolder = await folderPicker.PickSingleFolderAsync();
        //        if (imageFolder == null)
        //        {
        //            await ShowErrorDialogAsync("Image folder is required to import products with images.");
        //            return;
        //        }

        //        //var progressDialog = new ContentDialog
        //        //{
        //        //    Title = "Importing Products",
        //        //    Content = new ProgressRing { IsActive = true, Width = 50, Height = 50 },
        //        //    XamlRoot = this.XamlRoot
        //        //};
        //        //var progressTask = progressDialog.ShowAsync();
        //        Debug.WriteLine("Showing progress dialog...");
        //        var products = await ReadProductsFromExcelAsync(excelFile, imageFolder);
        //        if (products == null || !products.Any())
        //        {
        //            //progressDialog.Hide();
        //            await ShowErrorDialogAsync("No valid products found in the Excel file.");
        //            return;
        //        }

        //        var successCount = await ImportProductsAsync(products);
        //        await ViewModel.LoadProductsAsync();

        //        //progressDialog.Hide();
        //        await new ContentDialog
        //        {
        //            Title = "Import Result",
        //            Content = $"Successfully imported {successCount} product(s).",
        //            CloseButtonText = "OK",
        //            XamlRoot = this.XamlRoot
        //        }.ShowAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        await ShowErrorDialogAsync($"Failed to import products: {ex.Message}");
        //    }
        //}

        //private async Task<List<ProductCreateDTO>> ReadProductsFromExcelAsync(StorageFile excelFile, StorageFolder imageFolder)
        //{
        //    var products = new List<ProductCreateDTO>();
        //    ExcelPackage.License.SetNonCommercialPersonal("My Name");
        //    using (var stream = await excelFile.OpenStreamForReadAsync())
        //    using (var package = new ExcelPackage(stream))
        //    {
        //        Debug.WriteLine("Reading Excel file...");
        //        var worksheet = package.Workbook.Worksheets[0];
        //        if (worksheet == null)
        //            return products;

        //        int rowCount = worksheet.Dimension.Rows;
        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            try
        //            {
        //                var product = new ProductCreateDTO
        //                {
        //                    product_code = worksheet.Cells[row, 1].Text,
        //                    product_name = worksheet.Cells[row, 2].Text,
        //                    category_id = worksheet.Cells[row, 3].Text,
        //                    price = decimal.TryParse(worksheet.Cells[row, 4].Text, out var price) ? price : 0,
        //                    cost_price = decimal.TryParse(worksheet.Cells[row, 5].Text, out var costPrice) ? costPrice : 0,
        //                    stock_quantity = int.TryParse(worksheet.Cells[row, 6].Text, out var stock) ? stock : 0,
        //                    discount = int.TryParse(worksheet.Cells[row, 7].Text, out var discount) ? discount : 0,
        //                    image_path = worksheet.Cells[row, 8].Text ?? string.Empty
        //                };

        //                if (string.IsNullOrWhiteSpace(product.product_code) ||
        //                    string.IsNullOrWhiteSpace(product.product_name) ||
        //                    string.IsNullOrWhiteSpace(product.category_id))
        //                {
        //                    Debug.WriteLine($"Skipping row {row}: Missing required fields.");
        //                    continue;
        //                }

        //                if (!string.IsNullOrWhiteSpace(product.image_path))
        //                {
        //                    var imageFile = await imageFolder.GetFileAsync(product.image_path);
        //                    if (imageFile != null)
        //                    {
        //                        await ViewModel.AddProductAsync(product.product_code, product.product_name, product.category_id, product.price, product.cost_price, product.stock_quantity, imageFile); //await _cloudinaryService.UploadImageAsync(imageFile);
        //                        Debug.WriteLine($"Uploaded image for {product.product_name}: {product.image_path}");
        //                    }
        //                    else
        //                    {
        //                        Debug.WriteLine($"Image not found for {product.product_name}: {product.image_path}");
        //                        product.image_path = string.Empty;
        //                    }
        //                }

        //                products.Add(product);
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine($"Error reading row {row}: {ex.Message}");
        //                continue;
        //            }
        //        }
        //    }

        //    return products;
        //}
        private async Task ShowErrorDialogAsync(string message)
        {
            await new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            }.ShowAsync();
        }
        //private async Task<int> ImportProductsAsync(List<ProductCreateDTO> products)
        //{
        //    int successCount = 0;
        //    foreach (var product in products)
        //    {
        //        try
        //        {
        //            var addedProduct = await _productService.AddProductAsync(product);
        //            if (addedProduct != null)
        //            {
        //                successCount++;
        //                //await ViewModel.CheckStockThresholdAsync(addedProduct); // Ki?m tra ngu?ng t?n kho
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error importing product {product.product_name}: {ex.Message}, success count: {successCount}");
        //        }
        //    }
        //    return successCount;
        //}
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

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage > 1)
            {
                ViewModel.currentPage--;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
                await ViewModel.LoadProductsAsync();
            }
        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage < ViewModel.totalPages)
            {
                ViewModel.currentPage++;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                await ViewModel.LoadProductsAsync();
            }
        }
    }
}
