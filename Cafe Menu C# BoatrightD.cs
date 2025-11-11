using System;
using System.Collections.Generic;
using System.IO;

class CafeProgram
{
    // Configuration Variables
    static string cafeName = "ChatGPT Café";
    static double taxRate = 0.04;           // 4% sales tax
    static string discountCode = "STUDENT10";
    static bool discountUsed = false;      // track if discount has been applied

    // Cart Lists
    static List<string> itemNames = new List<string>();
    static List<double> itemPrices = new List<double>();
    static List<int> itemQuantities = new List<int>();

    // Main method to run the program
    static void Main(string[] args)
    {
        MainMenu();
    }

    // -------------------------
    // Helper Methods
    // -------------------------
    static void ClearScreen()
    {
        Console.Clear();
    }

    static void ShowBanner()
    {
        Console.WriteLine(new string('=', 40));
        Console.WriteLine($"Welcome to {cafeName}!");
        Console.WriteLine($"Sales Tax Rate: {taxRate * 100:F1}%");
        Console.WriteLine(new string('=', 40));
    }

    // -------------------------
    // Cart Methods
    // -------------------------
    static void AddItem(string name, double price, int qty)
    {
        itemNames.Add(name);
        itemPrices.Add(price);
        itemQuantities.Add(qty);
    }

    static void ViewCart()
    {
        if (itemNames.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        double subtotal = 0;
        Console.WriteLine("\n--- Your Cart ---");
        for (int i = 0; i < itemNames.Count; i++)
        {
            double lineTotal = itemPrices[i] * itemQuantities[i];
            subtotal += lineTotal;
            Console.WriteLine($"{i + 1}. {itemNames[i]} - ${itemPrices[i]:F2} x {itemQuantities[i]} = ${lineTotal:F2}");
        }

        // Average line total
        double avgLineTotal = subtotal / itemNames.Count;

        // Most expensive item by unit price
        double maxPrice = 0;
        int maxIndex = -1;
        for (int i = 0; i < itemPrices.Count; i++)
        {
            if (itemPrices[i] > maxPrice)
            {
                maxPrice = itemPrices[i];
                maxIndex = i;
            }
        }
        string mostExpensiveItem = itemNames[maxIndex];

        Console.WriteLine($"\nSubtotal: ${subtotal:F2}");
        Console.WriteLine($"Average line total: ${avgLineTotal:F2}");
        Console.WriteLine($"Most expensive item (by unit price): {mostExpensiveItem} at ${maxPrice:F2}");
    }

    static void RemoveItem(int index)
    {
        if (index >= 1 && index <= itemNames.Count)
        {
            string removed = itemNames[index - 1];
            itemNames.RemoveAt(index - 1);
            itemPrices.RemoveAt(index - 1);
            itemQuantities.RemoveAt(index - 1);
            Console.WriteLine($"Removed {removed} from your cart.");
        }
        else
        {
            Console.WriteLine("Invalid item number.");
        }
    }

    static double ComputeSubtotal()
    {
        double subtotal = 0;
        for (int i = 0; i < itemPrices.Count; i++)
        {
            subtotal += itemPrices[i] * itemQuantities[i];
        }
        return subtotal;
    }

    static double ComputeTax(double subtotal)
    {
        return subtotal * taxRate;
    }

    static double ApplyDiscount(double subtotal, string code)
    {
        if (!discountUsed && code == discountCode)
        {
            discountUsed = true;
            return subtotal * 0.10; // Apply 10% discount
        }
        return 0.0;
    }

    static void Checkout()
    {
        if (itemNames.Count == 0)
        {
            Console.WriteLine("Your cart is empty. Nothing to checkout.");
            return;
        }

        double subtotal = ComputeSubtotal();
        Console.Write("Enter discount code (or press Enter to skip): ");
        string code = Console.ReadLine().Trim();
        double discount = ApplyDiscount(subtotal, code);
        double tax = ComputeTax(subtotal - discount);
        double finalTotal = subtotal - discount + tax;

        Console.WriteLine("\n--- Receipt ---");
        for (int i = 0; i < itemNames.Count; i++)
        {
            double lineTotal = itemPrices[i] * itemQuantities[i];
            Console.WriteLine($"{itemNames[i]} - ${itemPrices[i]:F2} x {itemQuantities[i]} = ${lineTotal:F2}");
        }

        Console.WriteLine($"\nSubtotal: ${subtotal:F2}");
        Console.WriteLine($"Discount: -${discount:F2}");
        Console.WriteLine($"Tax: +${tax:F2}");
        Console.WriteLine($"Final Total: ${finalTotal:F2}");
        Console.WriteLine("Thank you for visiting!");
    }

    // -------------------------
    // File I/O Methods
    // -------------------------
    static void SaveCartToFile()
    {
        using (StreamWriter writer = new StreamWriter("cart.txt"))
        {
            for (int i = 0; i < itemNames.Count; i++)
            {
                writer.WriteLine($"{itemNames[i]},{itemPrices[i]},{itemQuantities[i]}");
            }
        }
        Console.WriteLine("Cart saved to file.");
    }

    static void LoadCartFromFile()
    {
        if (File.Exists("cart.txt"))
        {
            string[] lines = File.ReadAllLines("cart.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string name = parts[0];
                double price = double.Parse(parts[1]);
                int qty = int.Parse(parts[2]);
                AddItem(name, price, qty);
            }
            Console.WriteLine("Cart loaded from file.");
        }
        else
        {
            Console.WriteLine("No saved cart found.");
        }
    }

    // -------------------------
    // Main Menu Loop
    // -------------------------
    static void MainMenu()
    {
        while (true)
        {
            ClearScreen();
            ShowBanner();

            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add item");
            Console.WriteLine("2. View cart");
            Console.WriteLine("3. Remove item");
            Console.WriteLine("4. Checkout");
            Console.WriteLine("5. Save cart");
            Console.WriteLine("6. Load cart");
            Console.WriteLine("7. Quit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine().Trim();

            if (choice == "1")
            {
                ClearScreen();
                Console.Write("Enter item name: ");
                string name = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Item name cannot be empty.");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("Enter item price: ");
                double price = double.Parse(Console.ReadLine());
                if (price <= 0)
                {
                    Console.WriteLine("Price must be positive.");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("Enter quantity: ");
                int qty = int.Parse(Console.ReadLine());
                if (qty <= 0)
                {
                    Console.WriteLine("Quantity must be positive.");
                    Console.ReadLine();
                    continue;
                }

                AddItem(name, price, qty);
                Console.WriteLine($"Added {qty} x {name} at ${price:F2} each.");
                Console.ReadLine();
            }
            else if (choice == "2")
            {
                ClearScreen();
                ViewCart();
                Console.ReadLine();
            }
            else if (choice == "3")
            {
                ClearScreen();
                if (itemNames.Count == 0)
                {
                    Console.WriteLine("Cart is empty, nothing to remove.");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("Enter item number to remove: ");
                int index = int.Parse(Console.ReadLine());
                RemoveItem(index);
                Console.ReadLine();
            }
            else if (choice == "4")
            {
                ClearScreen();
                Checkout();
                Console.ReadLine();
            }
            else if (choice == "5")
            {
                SaveCartToFile();
                Console.ReadLine();
            }
            else if (choice == "6")
            {
                LoadCartFromFile();
                Console.ReadLine();
            }
            else if (choice == "7")
            {
                ClearScreen();
                Console.WriteLine("Thank you for shopping!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
                Console.ReadLine();
            }
        }
    }
}
