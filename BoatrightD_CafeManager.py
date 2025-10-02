# Danika Boatright
# cafeManager.py
import os

# -------------------------
# Configuration Variables
# -------------------------
cafeName = "ChatGPT Café"
taxRate = 0.04           # 4% sales tax
discountCode = "STUDENT10"
discountUsed = False     # track if discount already applied

# -------------------------
# Parallel Lists for the Cart
# -------------------------
itemNames = []
itemPrices = []
itemQuantities = []


# -------------------------
# Helper Functions
# -------------------------
def clearScreen():
    """Clear the terminal screen."""
    os.system('cls' if os.name == 'nt' else 'clear')


# -------------------------
# Functions
# -------------------------
def showBanner():
    """Display the welcome banner with cafe info."""
    print("=" * 40)
    print(f"Welcome to {cafeName}!")
    print(f"Sales Tax Rate: {taxRate * 100:.1f}%")
    print("=" * 40)


def addItem(name: str, price: float, qty: int):
    """Add an item to the cart lists."""
    itemNames.append(name)
    itemPrices.append(price)
    itemQuantities.append(qty)


def viewCart():
    """Display items in the cart with subtotal, averages, and most expensive."""
    if not itemNames:
        print("Your cart is empty.")
        return

    print("\n--- Your Cart ---")
    subtotal = 0
    for i in range(len(itemNames)):
        lineTotal = itemPrices[i] * itemQuantities[i]
        subtotal += lineTotal
        print(f"{i+1}. {itemNames[i]} - ${itemPrices[i]:.2f} x {itemQuantities[i]} = ${lineTotal:.2f}")

    # Average line total
    avgLineTotal = subtotal / len(itemNames)

    # Most expensive item by unit price
    maxPrice = max(itemPrices)
    maxIndex = itemPrices.index(maxPrice)
    mostExpItem = itemNames[maxIndex]

    print(f"\nSubtotal: ${subtotal:.2f}")
    print(f"Average line total: ${avgLineTotal:.2f}")
    print(f"Most expensive item (by unit price): {mostExpItem} at ${maxPrice:.2f}")


def removeItem(index: int):
    """Remove item by its 1-based index."""
    if 1 <= index <= len(itemNames):
        removed = itemNames[index-1]
        itemNames.pop(index-1)
        itemPrices.pop(index-1)
        itemQuantities.pop(index-1)
        print(f"Removed {removed} from your cart.")
    else:
        print("Invalid item number.")


def computeSubtotal() -> float:
    """Return subtotal of cart."""
    return sum(itemPrices[i] * itemQuantities[i] for i in range(len(itemNames)))


def computeTax(subtotal: float, taxRate: float) -> float:
    """Return tax amount."""
    return subtotal * taxRate


def applyDiscount(subtotal: float, code: str) -> float:
    """Apply discount if valid code and not used yet."""
    global discountUsed
    if not discountUsed and code == discountCode:
        discountUsed = True
        return subtotal * 0.10
    return 0.0


def checkout():
    """Print receipt with totals and handle discount."""
    if not itemNames:
        print("Your cart is empty. Nothing to checkout.")
        return

    subtotal = computeSubtotal()
    code = input("Enter discount code (or press Enter to skip): ").strip()
    discount = applyDiscount(subtotal, code)
    tax = computeTax(subtotal - discount, taxRate)
    finalTotal = subtotal - discount + tax

    print("\n--- Receipt ---")
    for i in range(len(itemNames)):
        lineTotal = itemPrices[i] * itemQuantities[i]
        print(f"{itemNames[i]} - ${itemPrices[i]:.2f} x {itemQuantities[i]} = ${lineTotal:.2f}")

    print(f"\nSubtotal: ${subtotal:.2f}")
    print(f"Discount: -${discount:.2f}")
    print(f"Tax: +${tax:.2f}")
    print(f"Final Total: ${finalTotal:.2f}")
    print("Thank you for visiting!")


def mainMenu():
    """Main loop that shows menu until quit."""
    while True:
        clearScreen()
        showBanner()

        print("\nMenu:")
        print("1. Add item")
        print("2. View cart")
        print("3. Remove item")
        print("4. Checkout")
        print("5. Quit")

        choice = input("Choose an option: ").strip()

        if choice == "1":
            clearScreen()
            name = input("Enter item name: ").strip()
            if name == "":
                print("Item name cannot be empty.")
                input("Press Enter to continue...")
                continue

            price = float(input("Enter item price: "))
            if price <= 0:
                print("Price must be positive.")
                input("Press Enter to continue...")
                continue

            qty = int(input("Enter quantity: "))
            if qty <= 0:
                print("Quantity must be positive.")
                input("Press Enter to continue...")
                continue

            addItem(name, price, qty)
            print(f"Added {qty} x {name} at ${price:.2f} each.")
            input("Press Enter to continue...")

        elif choice == "2":
            clearScreen()
            viewCart()
            input("\nPress Enter to continue...")

        elif choice == "3":
            clearScreen()
            if not itemNames:
                print("Cart is empty, nothing to remove.")
                input("Press Enter to continue...")
                continue
            index = int(input("Enter item number to remove: "))
            removeItem(index)
            input("Press Enter to continue...")

        elif choice == "4":
            clearScreen()
            checkout()
            input("Press Enter to continue...")

        elif choice == "5":
            clearScreen()
            print("Thank you for shopping!")
            break

        else:
            print("Invalid choice, try again.")
            input("Press Enter to continue...")


# -------------------------
# Run Program
# -------------------------
if __name__ == "__main__":
    mainMenu()
