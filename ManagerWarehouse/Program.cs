using Dal.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;






    IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("manager.json")
    .Build();


//string connStr = "Server=nataliia\\mssqlserver02;Database=ManagerWH;Integrated Security=True;";
string connectionString = configuration.GetConnectionString("ManagerWH") ?? "";


Users loggedInUser = null;
Console.WriteLine("Welcome to Warehouse Management System!\n");


bool isLoggedIn = false;
while (!isLoggedIn)
{
    Console.WriteLine("Please log in or register.\n1. Log In\n2. Register\n");
    string choice = Console.ReadLine();

    if (choice == "1") 
    {
        Console.WriteLine("Enter Username:");
        string Username = Console.ReadLine();

        Console.WriteLine("Enter Password:");
        string Password = Console.ReadLine();

        var userDal = new UsersDal(connectionString);
        var user = userDal.GetUserByUsernameAndPassword(Username, Password);

        if (user != null)
        {
            Console.WriteLine($"Welcome, {user.Username}!\n");
            loggedInUser = user; 
            isLoggedIn = true;
        }
        else
        {
            Console.WriteLine("Invalid username or password. Try again.\n");
        }
    }
    else if (choice == "2") 
    {
        Console.WriteLine("Enter Username:");
        string newUsername = Console.ReadLine();

        Console.WriteLine("Enter Password:");
        string newPassword = Console.ReadLine();

        var userDal = new UsersDal(connectionString);

        if (userDal.GetUserByUsername(newUsername) != null)
        {
            Console.WriteLine("Username already exists. Please try again.");
            continue;
        }

      
        var newUser = new Users
        {
            Username = newUsername,
            Password = newPassword,
           
        };

        userDal.Insert(newUser); 
        Console.WriteLine("Registration successful! You can now log in.\n");
    }
    else
    {
        Console.WriteLine("Invalid option. Please select 1 to log in or 2 to register.");
    }
}



char option = 's';

while (option != 'q')
{
    Console.WriteLine("Please select option:\n1. - View Products\n2. - Search Products\n3. - Sort Products\n4. - Create Order\n5. - View Active Orders\n6. - Edit Active Orders\n7. - Change Password\n8. - Delete Account\n9. - Add Product\n10. - Delete Product\n11. - Add Supplier\n12. - Delete Supplier\n13. - Delete Order\n14. - View Suppliers\nQ. - Logout\n");

    string selectedOption = Console.ReadLine() ?? "";

    if (selectedOption.Trim().ToLower() == "q")
    {
        Console.WriteLine("Logged out successfully.");
        isLoggedIn = false;
        return;
    }

    
    if (string.IsNullOrWhiteSpace(selectedOption) || !int.TryParse(selectedOption, out int optionNum) || optionNum < 1 || optionNum > 14)
    {
        Console.WriteLine("Incorrect option selected!");
        continue;
    }

    
    switch (selectedOption.Trim())
    {
        case "1":
            ListAllProducts();
            break;
        case "2":
            SearchProducts();
            break;
        case "3":
            SortProducts();
            break;
        case "4":
            CreateOrder();
            break;
        case "5":
            ViewActiveOrders();
            break;
        case "6":
            EditOrder();
            break;
        case "7":
            ChangePassword();
            break;
        case "8":
            DeleteAccount();
            break;
        case "9":
            AddProduct();
            break;
        case "10":
            DeleteProduct();
            break;
        case "11":
            AddSupplier();
            break;
        case "12":
            DeleteSupplier();
            break;
        case "13":
            DeleteOrder();
            break;
        case "14":
            ListAllSuppliers();
            break;
        default:
            Console.WriteLine("Incorrect option selected!");
            break;
    }
}


void ListAllProducts()
{
    var productDal = new ProductsDal(connectionString);
    List<Products> products = productDal.GetAll();

    Console.WriteLine("\nAvailable Products:\n");
    foreach (var product in products)
    {
        Console.WriteLine($"ProductID: {product.ProductID}, Name: {product.Name}, In Stock: {product.QuantityInStock}, Price: {product.Price}");
    }
}



void SearchProducts()
{
    Console.WriteLine("Please enter Product Name or ID to search:");
    string searchTerm = Console.ReadLine();

    var productDal = new ProductsDal(connectionString);
    List<Products> products = productDal.SearchByNameOrId(searchTerm); 

    if (products.Count == 0)
    {
        Console.WriteLine("No products found.");
    }
    else
    {
        Console.WriteLine("\nSearch Results:\n");
        foreach (var product in products)
        {
            Console.WriteLine($"ProductID: {product.ProductID}, Name: {product.Name}, In Stock: {product.QuantityInStock}, Price: {product.Price}");
        }
    }
}


void SortProducts()
{
    Console.WriteLine("Sort by:\n1. - Name\n2. - Quantity in Stock\n3. - Price");
    string sortOption = Console.ReadLine();

    var productDal = new ProductsDal(connectionString);
    List<Products> products = null;

    switch (sortOption)
    {
        case "1":
            products = productDal.SortByName();
            break;
        case "2":
            products = productDal.SortByQuantity();
            break;
        case "3":
            products = productDal.SortByPrice();
            break;
        default:
            Console.WriteLine("Invalid option.");
            return;
    }

    Console.WriteLine("\nSorted Products:\n");
    foreach (var product in products)
    {
        Console.WriteLine($"ProductID: {product.ProductID}, Name: {product.Name}, In Stock: {product.QuantityInStock}, Price: {product.Price}");
    }
}


void CreateOrder()
{
    Console.WriteLine("Please enter Product ID:");
    int productId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Supplier ID:");
    int supplierId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Quantity:");
    int quantity = Convert.ToInt32(Console.ReadLine());

    var order = new Orders
    {
        ProductID = productId,
        SupplierID = supplierId,
        UserID = loggedInUser.UserID, 
        Quantity = quantity,
        Status = "Pending",
        OrderDate = DateTime.Now
    };

    var orderDal = new OrdersDal(connectionString);
    Orders newOrder = orderDal.Insert(order);

    Console.WriteLine($"Order Created: {newOrder.OrderID}, Status: {newOrder.Status}, Quantity: {newOrder.Quantity}");
}



void ViewActiveOrders()
{
    var orderDal = new OrdersDal(connectionString);
    List<Orders> orders = orderDal.GetActiveOrdersByUserId(loggedInUser.UserID); 
    Console.WriteLine("\nActive Orders:\n");
    if (orders.Count == 0)
    {
        Console.WriteLine("No active orders found for your account.");
    }
    else
    {
        foreach (var order in orders)
        {
            Console.WriteLine($"OrderID: {order.OrderID}, ProductID: {order.ProductID}, SupplierID: {order.SupplierID}, Status: {order.Status}, Quantity: {order.Quantity}");
        }
    }
}


void EditOrder()
{
    Console.WriteLine("Please enter Order ID to edit:");
    int orderId = Convert.ToInt32(Console.ReadLine());

    var orderDal = new OrdersDal(connectionString);
    Orders order = orderDal.GetById(orderId);

    if (order != null && order.UserID == loggedInUser.UserID) 
    {
        Console.WriteLine($"Editing Order {order.OrderID}. Status: {order.Status}, Quantity: {order.Quantity}");

        Console.WriteLine("Enter new Quantity:");
        order.Quantity = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter new Status:");
        order.Status = Console.ReadLine();

        orderDal.Update(order);

        Console.WriteLine($"Order {order.OrderID} updated successfully.");
    }
    else
    {
        Console.WriteLine(order == null ? "Order not found." : "You do not have permission to edit this order.");
    }
}

void ChangePassword()
{
    Console.WriteLine("Enter your current password:");
    string currentPassword = Console.ReadLine();

    if (loggedInUser.Password != currentPassword)
    {
        Console.WriteLine("Incorrect current password.");
        return;
    }

    Console.WriteLine("Enter new password:");
    string newPassword = Console.ReadLine();

    Console.WriteLine("Confirm new password:");
    string confirmPassword = Console.ReadLine();

    if (newPassword != confirmPassword)
    {
        Console.WriteLine("Passwords do not match.");
        return;
    }

    loggedInUser.Password = newPassword;

    var userDal = new UsersDal(connectionString);
    userDal.UpdatePassword(loggedInUser); 

    Console.WriteLine("Password changed successfully.");
}


void DeleteAccount()
{
    Console.WriteLine("Are you sure you want to delete your account? (y/n):");
    string confirmation = Console.ReadLine().ToLower();

    if (confirmation == "y")
    {
        var userDal = new UsersDal(connectionString);
        userDal.Delete(loggedInUser.UserID); 

        Console.WriteLine("Account deleted successfully. Logging out...");
        isLoggedIn = false; 
        loggedInUser = null;
        Environment.Exit(0); 
    }
    else
    {
        Console.WriteLine("Account deletion canceled.");
    }
}

void AddProduct()
{
    Console.WriteLine("Enter product name:");
    string productName = Console.ReadLine();

    Console.WriteLine("Enter quantity in stock:");
    int quantityInStock = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Enter price:");
    decimal price = Convert.ToDecimal(Console.ReadLine());

    var product = new Products
    {
        Name = productName,
        QuantityInStock = quantityInStock,
        Price = price
    };

    var productDal = new ProductsDal(connectionString);
    productDal.Insert(product);

    Console.WriteLine("Product added successfully.");
}


void DeleteProduct()
{
    Console.WriteLine("Enter Product ID to delete:");
    int productId = Convert.ToInt32(Console.ReadLine());

    var productDal = new ProductsDal(connectionString);
    productDal.Delete(productId);

    Console.WriteLine("Product deleted successfully.");
}


void AddSupplier()
{
    Console.WriteLine("Enter supplier name:");
    string name = Console.ReadLine();

    Console.WriteLine("Enter supplier phone:");
    string phone = Console.ReadLine(); 

    Console.WriteLine("Enter supplier address:");
    string address = Console.ReadLine();

    var supplier = new Suppliers
    {
        Name = name,
        Phone = phone, 
        Address = address
    };

    var supplierDal = new SuppliersDal(connectionString);
    supplierDal.Insert(supplier);

    Console.WriteLine("Supplier added successfully.");
}


void DeleteSupplier()
{
    Console.WriteLine("Enter Supplier ID to delete:");
    int supplierId = Convert.ToInt32(Console.ReadLine());

    var supplierDal = new SuppliersDal(connectionString);
    supplierDal.Delete(supplierId);

    Console.WriteLine("Supplier deleted successfully.");
}

void ListAllSuppliers()
{
    var supplierDal = new SuppliersDal(connectionString);
    List<Suppliers> suppliers = supplierDal.GetAll(); 

    Console.WriteLine("\nAvailable Suppliers:\n");
    foreach (var supplier in suppliers)
    {
        Console.WriteLine($"SupplierID: {supplier.SupplierID}, Name: {supplier.Name}, Phone: {supplier.Phone}, Address: {supplier.Address}");
    }
}


void DeleteOrder()
{
    Console.WriteLine("Please enter Order ID to delete:");
    int orderId = Convert.ToInt32(Console.ReadLine());

    var orderDal = new OrdersDal(connectionString);
    var order = orderDal.GetById(orderId);

    if (order != null && order.UserID == loggedInUser.UserID) 
    {
        orderDal.Delete(orderId); 
        Console.WriteLine($"Order {orderId} deleted successfully.");
    }
    else
    {
        Console.WriteLine(order == null ? "Order not found." : "You do not have permission to delete this order.");
    }
}


