# vuapos.Presentation

Short description
This project folder implements the Presentation layer (WinUI) of the vuapos application. It contains UI pages, dialogs, view-models and services that communicate with the backend API and external services (Cloudinary).

Key responsibilities
- UI screens and dialogs (pages, ContentDialogs)
- ViewModels that coordinate UI <-> Services
- Local services that call the backend API (via `ApiService`) and Cloudinary
- Initialization and DI registration for Presentation-layer dependencies

Important files and symbols
- [`vuapos.Presentation.App`](vuapos.Presentation/App.xaml.cs) — application entry, dependency registration
- [`vuapos.Presentation.MainWindow`](vuapos.Presentation/MainWindow.xaml.cs) — shell window and navigation
- Pages / Views:
  - [vuapos.Presentation/Views/Product/ProductPage.xaml.cs](vuapos.Presentation/Views/Product/ProductPage.xaml.cs) — product listing, import, add/edit flows
  - [vuapos.Presentation/Views/Category/CategoryPage.xaml.cs](vuapos.Presentation/Views/Category/CategoryPage.xaml.cs)
  - [vuapos.Presentation/Views/Customer/CustomerPage.xaml.cs](vuapos.Presentation/Views/Customer/CustomerPage.xaml.cs)
  - [vuapos.Presentation/Views/Promotion/PromotionPage.xaml.cs](vuapos.Presentation/Views/Promotion/PromotionPage.xaml.cs)
- Dialogs:
  - [vuapos.Presentation/Views/Product/AddProductDialog.xaml.cs](vuapos.Presentation/Views/Product/AddProductDialog.xaml.cs)
  - [vuapos.Presentation/Views/Product/EditProductDialog.xaml.cs](vuapos.Presentation/Views/Product/EditProductDialog.xaml.cs)
  - [vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs](vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs)
  - [vuapos.Presentation/Views/Promotion/AddPromotionDialog.xaml.cs](vuapos.Presentation/Views/Promotion/AddPromotionDialog.xaml.cs)
  - [vuapos.Presentation/Views/Promotion/EditPromotionDialog.xaml.cs](vuapos.Presentation/Views/Promotion/EditPromotionDialog.xaml.cs)
- ViewModels:
  - [`vuapos.Presentation.Models.ProductViewModel`](vuapos.Presentation/Models/ProductViewModel.cs)
  - [`vuapos.Presentation.Models.CategoryViewModel`](vuapos.Presentation/Models/CategoryViewModel.cs)
  - [`vuapos.Presentation.Models.CustomerViewModel`](vuapos.Presentation/Models/CustomerViewModel.cs)
  - [`vuapos.Presentation.Models.PromotionViewModel`](vuapos.Presentation/Models/PromotionViewModel.cs)
- Services:
  - [`vuapos.Presentation.Services.ApiService`](vuapos.Presentation/Services/ApiService.cs) — base HTTP helper
  - [`vuapos.Presentation.Services.ProductService`](vuapos.Presentation/Services/ProductService.cs)
  - [`vuapos.Presentation.Services.CategoryService`](vuapos.Presentation/Services/CategoryService.cs)
  - [`vuapos.Presentation.Services.CustomerService`](vuapos.Presentation/Services/CustomerService.cs)
  - [`vuapos.Presentation.Services.PromotionService`](vuapos.Presentation/Services/PromotionService.cs)
  - [`vuapos.Presentation.Services.CloudinaryService`](vuapos.Presentation/Services/CloudinaryService.cs)

How the Presentation layer works (high level)
- `App.xaml.cs` registers services and creates the main window. See [`vuapos.Presentation.App`](vuapos.Presentation/App.xaml.cs).
- `MainWindow` hosts navigation and swaps pages (see [`vuapos.Presentation.MainWindow`](vuapos.Presentation/MainWindow.xaml.cs)).
- Pages use ViewModels to load data and call services. For example, `ProductPage` uses [`vuapos.Presentation.Models.ProductViewModel`](vuapos.Presentation/Models/ProductViewModel.cs) which uses [`vuapos.Presentation.Services.ProductService`](vuapos.Presentation/Services/ProductService.cs) and [`vuapos.Presentation.Services.CloudinaryService`](vuapos.Presentation/Services/CloudinaryService.cs) to upload images and call the API.
- API requests go through [`vuapos.Presentation.Services.ApiService`](vuapos.Presentation/Services/ApiService.cs) which wraps HttpClient and JSON (see SendRequestAsync implementation).

Run / Debug (UI)
- Open the solution file [vuapos/vuapos.sln](vuapos.sln) in Visual Studio / Visual Studio Code with the C# WinUI tooling installed.
- Set the vuapos.Presentation project as the startup project.
- Build and run; UI is hosted in `MainWindow` (see [`vuapos.Presentation.MainWindow`](vuapos.Presentation/MainWindow.xaml.cs)).

Notes and recommended actions
- Secrets and tokens are currently hardcoded in services:
  - Cloudinary keys in [`vuapos.Presentation.Services.CloudinaryService`](vuapos.Presentation/Services/CloudinaryService.cs)
  - JWT tokens in `ProductService`, `CategoryService`, `CustomerService`, `PromotionService`
  Move these to secure configuration (environment variables, user secrets, or a secured store).
- API base URL is read from env var in [`vuapos.Presentation.Services.ApiService`](vuapos.Presentation/Services/ApiService.cs). Ensure API_BASE_URL is set for production/testing.
- Import flow: Excel import logic is implemented in [vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs](vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs) using EPPlus.
- Error handling and UX: dialogs set args.Cancel = true on errors; many flows use deferrals — verify deferral handling where async UI actions are required.

Useful files (quick links)
- [vuapos.Presentation/App.xaml.cs](vuapos.Presentation/App.xaml.cs)
- [vuapos.Presentation/MainWindow.xaml.cs](vuapos.Presentation/MainWindow.xaml.cs)
- [vuapos.Presentation/Services/ApiService.cs](vuapos.Presentation/Services/ApiService.cs)
- [vuapos.Presentation/Services/ProductService.cs](vuapos.Presentation/Services/ProductService.cs)
- [vuapos.Presentation/Services/CloudinaryService.cs](vuapos.Presentation/Services/CloudinaryService.cs)
- [vuapos.Presentation/Views/Product/ProductPage.xaml.cs](vuapos.Presentation/Views/Product/ProductPage.xaml.cs)
- [vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs](vuapos.Presentation/Views/Product/ImportExcelProduct.xaml.cs)
- [vuapos.Presentation/Models/ProductViewModel.cs](vuapos.Presentation/Models/ProductViewModel.cs)

Contributing / Extending
- Follow MVVM: add View + ViewModel + Service updates.
- Prefer registering new services in [`vuapos.Presentation.App`](vuapos.Presentation/App.xaml.cs).
- Centralize configuration and secrets; remove hardcoded credentials before shipping.

License
This folder follows the repository license: see [vuapos/LICENSE.txt](../LICENSE.txt).