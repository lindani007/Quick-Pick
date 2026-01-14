# QuickPick

A multi-project solution for retailer platform that lets customers buy items online or in-store, and collect at the store, and uses several cooperating services (APIs, client libraries and .NET MAUI frontends).

---

## About The Project
QuickPick is a small retail platform composed of multiple projects. It enables users to:
- Browse and buy items in-app or in-store
- Collect order at a store
- Upload and serve product images via a blob storage API
- Receive real-time order/status updates via SignalR

The repo intentionally does not include secrets. Credentials previously removed must be replaced by each developer or deployment with their own connection strings and secrets (see Configuration).

---

## Built With
- .NET 10
- ASP.NET Core Web API
- Azure.Storage.Blobs
- Microsoft.Extensions.Azure
- .NET MAUI (client apps)
- SignalR for real‑time messaging
- System.Text.Json

---

## Getting Started
Follow these steps to run the project locally for development.

1. Clone the repository:
   ```bash
   git clone https://github.com/lindani007/Quick-Pick.git
   ```
2. Open the solution in Visual Studio 2026 (recommended for MAUI) or use the `dotnet` CLI.


---

## Prerequisites
- .NET 10 SDK
- Visual Studio 2026 (recommended for MAUI development)
- An Azure Storage account or Azurite for local blob emulation
- A database (e.g., Azure SQL Server or local SQL) if you run `QuickPickDBApi`
- (Optional) Azure Storage Explorer for inspecting blobs

---

## Installation
1. Restore packages:
   ```bash
   dotnet restore
   ```
2. Build the solution:
   ```bash
   dotnet build
   ```

---

## Configuration (IMPORTANT — add your credentials)
Secrets were removed. You MUST supply your own connection strings and credentials before running the services and do not forget to run  
  ```bash
   dotnet ef migrations add InitialCreate
   ```
and 
  ```bash
   dotnet ef database update
   ```
to add tables to your data base.
Recommended approaches (do NOT commit secrets):

About Apis
- Use `appsettings.json` for local connection strings
- Use environment variables or Azure App Service settings for production
- Consider using Azure Key Vault for secret management

- Maui Clients/ Apps
- Use AppXamal.cs and MauiProgram.cs for adding apis urtls
-
  RunTime Error
- If Deploying to azure you must use different app service and supply a port for each api so that you can not have a conflict in ports.

Security notes:
- Use Azure Key Vault or other secret stores in production.
- Do not commit real credentials to source control.
- Sanitize and validate file names used as blob names (avoid invalid characters, leading/trailing slashes).


## Usage
-	QuickPick_Employer — store/staff app (manage inventory, create/edit items, view incoming orders, mark readiness for pick‑up). Integrates with APIs to change product data and images.
	- This is the page that you land in when you start QuickPick_Employer app.
   

https://github.com/user-attachments/assets/16c3da60-2a35-45b7-ac8f-9efa0f0c814a


	- The app allows you to add new products with images, on their respective aisles, edit existing products, and view incoming orders.
	- The app uses the ImageStorer API to upload product images to blob storage.
	- The app uses QuickPickDBApi to manage product and order data.
	- The app uses SignalR to receive real-time order updates.

- QuickPick_Customer — customer-facing app (browse items, place orders). Integrates with APIs to fetch product data and images.
	- This is the page that you land in when you start QuickPick_Customer app and click Get Started.
   

https://github.com/user-attachments/assets/bcf7bff1-4e11-48f1-b54e-098a522d5239


	- The app allows customers to browse products by aisle, add items to their cart, place orders for in-store pickup.
	- The app uses QuickPickDBApi to fetch product data and submit orders.
	- The app uses SignalR to receive real-time order status updates.
	
-QuickPick_Employee_Packing — staff app for packing orders (view orders to pack, mark items as packed). Integrates with APIs to fetch order data and update packing status.
	- This is the page that you land in when you start QuickPick_EmployeePacking app.
	- The app allows staff to view incoming orders that need packing, mark items as packed, and update order status.
	- The app uses QuickPickDBApi to fetch order data and update packing status.
	- The app uses SignalR to receive real-time order updates.

_QuickPick_OrderDistribution - staff app for distributing packed orders to customers (view ready orders, mark as distributed). Integrates with APIs to fetch order data and update distribution status.
	- This is the page that you land in when you start QuickPick_OrderDistribution app.
	- The app allows staff to view packed orders that are ready for customer pickup, mark orders as distributed, and update order status.
	- The app uses QuickPickDBApi to fetch order data and update distribution status.
	- The app uses SignalR to receive real-time order updates.
	-The order needs it code that was given to the customer when they placed the order to pick up their order.

-In Apis : If you give the apis different ports when deploying to azure you will not have a conflict in ports.
- QuickPickDBApi — main API for product and order data (CRUD operations, order management).
- SignalRHubApi — real-time messaging API for order status updates.
- QuickPickBlobService — client library for uploading images to ImageStorerApi.


Notes:
- The API previously returned `BlobUrl` (PascalCase); clients may expect `blobUrl` (camelCase). Ensure client and server agree on naming.
- Client helper `QuickPickBlobService` provides a convenience method for uploads; set its `HttpClient.BaseAddress` to your running API.
- Prefer streaming uploads on the client to reduce memory pressure.



## Roadmap
Planned improvements:
- Health-check endpoints for services (blob storage, DB)
- Structured logging and centralized diagnostics (ILogger + aggregator)
- Integration tests and CI/CD pipeline
- Configurable container names and retention policies
- Retry and transient-fault handling for blob operations
- Authentication and authorization for APIs

---

## Contributing
Contributions are welcome. Please:
1. Fork the repo and create a branch
2. Keep commits small and focused
3. Add or update tests when appropriate
4. Open a pull request with a clear description and testing notes


---

## License
This repository is provided under the MIT License.

---

## Contact
Project maintainer: Lindani  

Email : lindanidev@outlook.com 

Repository: https://github.com/lindani007/Quick-Pick.git

---

## Acknowledgments
- Azure SDK (`Azure.Storage.Blobs`)
- .NET and .NET MAUI teams

- Any third-party libraries used throughout the projects





