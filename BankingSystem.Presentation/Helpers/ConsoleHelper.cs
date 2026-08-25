using BankingSystem.Domain.Enums;
using System.Text;

namespace BankingSystem.Presentation.Helpers;

public static class ConsoleHelper
{
    public static string ReadUsername(string message)
    {
        return ReadInput(message, IsValidUsernameChar, false);
    }

    public static string ReadPassword(string message)
    {
        return ReadInput(message, IsValidPasswordChar, true);
    }

    public static string ReadInput(
        string message,
        Func<char, bool>? isValidChar = null,
        bool maskInput = false,
        bool allowEmpty = false)
    {
        const int boxWidth = 40;
        const int layoutIndent = 30;
        const int layoutWidth = 45;

        isValidChar ??= c => !char.IsControl(c);

        int startY = Console.CursorTop;
        int centerX = layoutIndent + (layoutWidth - boxWidth) / 2;
        int inputX = centerX + 2;
        int inputY = startY + 1;
        int maxInputX = centerX + boxWidth - 2;

        Console.SetCursorPosition(centerX, startY);
        Console.WriteLine($"┌{new string('─', boxWidth - 2)}┐");

        Console.SetCursorPosition(centerX, startY + 1);
        Console.Write($"│{new string(' ', boxWidth - 2)}│");

        Console.SetCursorPosition(centerX, startY + 2);
        Console.WriteLine($"└{new string('─', boxWidth - 2)}┘");

        Console.SetCursorPosition(inputX, inputY);
        Console.Write(message);

        var input = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                if (input.Length > 0 || allowEmpty)
                {
                    break;
                }
            }
            else if (isValidChar(key.KeyChar))
            {
                if (Console.CursorLeft < maxInputX)
                {
                    input.Append(key.KeyChar);
                    Console.Write(maskInput ? '*' : key.KeyChar);
                }
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);

                Console.SetCursorPosition(
                    Console.CursorLeft - 1,
                    inputY);

                Console.Write(' ');

                Console.SetCursorPosition(
                    Console.CursorLeft - 1,
                    inputY);
            }
        }

        Console.SetCursorPosition(centerX, startY + 3);

        return input.ToString();
    }

    public static decimal ReadDecimal(string message)
    {
        const int boxWidth = 40;
        const int layoutIndent = 30;
        const int layoutWidth = 45;

        int startY = Console.CursorTop;
        int centerX = layoutIndent + (layoutWidth - boxWidth) / 2;
        int inputX = centerX + 2;
        int inputY = startY + 1;
        int maxInputX = centerX + boxWidth - 2;

        Console.SetCursorPosition(centerX, startY);
        Console.WriteLine($"┌{new string('─', boxWidth - 2)}┐");

        Console.SetCursorPosition(centerX, startY + 1);
        Console.Write($"│{new string(' ', boxWidth - 2)}│");

        Console.SetCursorPosition(centerX, startY + 2);
        Console.WriteLine($"└{new string('─', boxWidth - 2)}┘");

        Console.SetCursorPosition(inputX, inputY);
        Console.Write(message);

        var input = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                if (input.Length > 0 && input.ToString() != ".")
                {
                    break;
                }
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);

                    Console.SetCursorPosition(
                        Console.CursorLeft - 1,
                        inputY);

                    Console.Write(' ');

                    Console.SetCursorPosition(
                        Console.CursorLeft - 1,
                        inputY);
                }
            }
            else if (char.IsAsciiDigit(key.KeyChar))
            {
                if (Console.CursorLeft < maxInputX)
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            else if (key.KeyChar == '.' &&
                     input.Length > 0 &&
                     !input.ToString().Contains('.') &&
                     Console.CursorLeft < maxInputX)
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }

        var result = input.ToString();

        if (result.EndsWith('.'))
        {
            result = result[..^1];
        }

        Console.SetCursorPosition(centerX, startY + 3);

        return Convert.ToDecimal(result);
    }

    public static void PrintCenteredHeader(string header, int totalLength = 45, int indent = 30, bool bordered = true)
    {
        if (string.IsNullOrWhiteSpace(header))
            throw new ArgumentException("Header cannot be empty.", nameof(header));

        if (totalLength < header.Length)
            totalLength = header.Length;

        int totalPadding = totalLength - header.Length;
        int leftPadding = totalPadding / 2;
        int rightPadding = totalPadding - leftPadding;

        string leftSpace = new string(' ', leftPadding);
        string rightSpace = new string(' ', rightPadding);

        if (bordered)
            Console.WriteLine(new string(' ', indent) + new string('=', totalLength));

        Console.WriteLine(new string(' ', indent) + leftSpace + header + rightSpace);

        if (bordered)
            Console.WriteLine(new string(' ', indent) + new string('=', totalLength));

        Console.WriteLine();
    }

    public static void WaitForUser()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public static bool IsValidUsernameChar(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_' || c == '-';
    }

    public static bool IsValidPasswordChar(char c)
    {
        return char.IsAsciiLetterOrDigit(c) || "!@#$%^&*()_-+=[]{}|\\:;\"'<>,.?/".Contains(c);
    }

    public static void PrintCenteredDate()
    {
        PrintCenteredHeader(DateTime.Now.ToString("dddd, MMM dd, yyyy"), 45, 30, false);
    }

    public static char ReadConfirmationChar(string message)
    {
        Console.Write(message);

        char ch = '\0';

        while (ch == '\0')
        {
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true);

                if (char.IsAsciiLetter(key.KeyChar) &&
                    (key.KeyChar == 'y' || key.KeyChar == 'Y' || key.KeyChar == 'n' || key.KeyChar == 'N') &&
                    ch == '\0')
                {
                    ch = key.KeyChar;
                    Console.Write(ch);
                }
                else if (key.Key == ConsoleKey.Backspace && ch != '\0')
                {
                    ch = '\0';
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
        }

        return ch;
    }

    public static void PrintMenuLayout(string menuHeader, Action printOptions, string role, string fullName, string permissions)
    {
        PrintCenteredHeader(menuHeader);
        PrintCenteredDate();

        printOptions();

        PrintNewLines(6);
        PrintTabs(2);
        PrintFooter(role, fullName, permissions);

        MoveCursorUp(6);
    }

    public static void RunMenu(Action printMenu, Action<int> handleOption, int minOption, int maxOption, Func<int, bool> shouldExit)
    {
        int option;

        do
        {
            Console.Clear();
            printMenu();

            Console.Write($"\n{GetTabs(5)}Enter an option: ");

            while (!int.TryParse(Console.ReadLine(), out option)
                   || option < minOption
                   || option > maxOption)
            {
                Console.Clear();
                printMenu();

                Console.Write($"\n{GetTabs(3)} Invalid option, enter an option between {minOption} and {maxOption}: ");
            }

            handleOption(option);

        } while (!shouldExit(option));
    }

    public static void PrintMenu(IEnumerable<(int Number, string Text)> items, int tabs = 5)
    {
        foreach (var item in items)
        {
            Console.WriteLine($"{GetTabs(tabs)}{item.Number}. {item.Text}");
        }
    }

    public static string GetTabs(int tabCount)
    {
        return new string('\t', tabCount);
    }

    public static void PrintTabs(int tabCount)
    {
        Console.Write(GetTabs(tabCount));
    }

    public static void PrintNewLines(int count)
    {
        Console.Write(new string('\n', count));
    }

    public static void PrintFooter(string role, string name, string permissions)
    {
        string coloredRole = Color(role, ConsoleColorCode.Cyan);
        string coloredName = Color(name, ConsoleColorCode.Cyan);
        string coloredPermissions = Color(permissions, ConsoleColorCode.Cyan);

        Console.WriteLine($"Role {coloredRole}  │  Name {coloredName}  │  Permissions {coloredPermissions}");
    }

    public static void PrintClientCard(string id, string firstName, string lastName, string username, string email, string phone, string permissions)
    {
        PrintCenteredHeader("Client Card");

        Console.WriteLine($"{GetTabs(5)}{Color("Id", ConsoleColorCode.Cyan)}: {id}");
        Console.WriteLine($"{GetTabs(5)}{Color("First name", ConsoleColorCode.Cyan)}: {firstName}");
        Console.WriteLine($"{GetTabs(5)}{Color("Last name", ConsoleColorCode.Cyan)}: {lastName}");
        Console.WriteLine($"{GetTabs(5)}{Color("Username", ConsoleColorCode.Cyan)}: {username}");
        Console.WriteLine($"{GetTabs(5)}{Color("Email", ConsoleColorCode.Cyan)}: {email}");
        Console.WriteLine($"{GetTabs(5)}{Color("Phone", ConsoleColorCode.Cyan)}: {phone}");
        Console.WriteLine($"{GetTabs(5)}{Color("Permissions", ConsoleColorCode.Cyan)}: {permissions}");
    }

    public static void PrintMiniClientCard(string id, string fullName, string permissions)
    {
        Console.WriteLine($"{GetTabs(5)}{Color("Id", ConsoleColorCode.Cyan)}: {id}");
        Console.WriteLine($"{GetTabs(5)}{Color("Full Name", ConsoleColorCode.Cyan)}: {fullName}");
        Console.WriteLine($"{GetTabs(5)}{Color("Permissions", ConsoleColorCode.Cyan)}: {Color(permissions, ConsoleColorCode.White)}");
    }

    public static void PrintCurrencyCard(string country, string name, string code, decimal rate)
    {
        int countryW = Math.Max("Country".Length, country.Length) + 2;
        int nameW = Math.Max("Name".Length, name.Length) + 2;
        int codeW = Math.Max("Code".Length, code.Length) + 2;
        int rateW = Math.Max("Rate".Length, rate.ToString().Length) + 2;
        int borderW = countryW + codeW + nameW + rateW + 5;

        Console.WriteLine($"\t\t{GetTabs(1)}{new string('-', borderW)}");
        Console.WriteLine(
            $"\t\t{GetTabs(1)}" +
            $"|{Color("Country".PadRight(countryW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Name".PadRight(nameW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Code".PadRight(codeW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Rate".PadRight(rateW), ConsoleColorCode.Cyan)}|"
        );

        Console.WriteLine($"\t\t{GetTabs(1)}{new string('-', borderW)}");
        Console.WriteLine(
            $"\t\t{GetTabs(1)}" +
            $"|{country.PadRight(countryW)}" +
            $"|{name.PadRight(nameW)}" +
            $"|{code.PadRight(codeW)}" +
            $"|{rate.ToString().PadRight(rateW)}|"
        );

        Console.WriteLine($"\t\t{GetTabs(1)}{new string('-', borderW)}");
    }

    public static void PrintAccountCard(string accountId, string userId, decimal balance, string currencyCode, string pinCode, bool isLocked, bool isDeleted)
    {
        PrintCenteredHeader("Account Card");

        int accountIdW = Math.Max("Account Id".Length, accountId.Length) + 2;
        int userIdW = Math.Max("User Id".Length, userId.Length) + 2;
        int balanceW = Math.Max("Balance".Length, balance.ToString().Length) + 2;
        int currencyCodeW = Math.Max("Currency Code".Length, currencyCode.Length) + 2;
        int pinCodeW = Math.Max("Pin Code".Length, pinCode.Length) + 2;
        int isLockedW = "Is Locked".Length + 2;
        int isDeletedW = "Is Deleted".Length + 2;
        int borderW = accountIdW + userIdW + balanceW + currencyCodeW + pinCodeW + isLockedW + isDeletedW + 8;

        Console.WriteLine($"{GetTabs(1)}{new string('-', borderW)}");
        Console.WriteLine(
            $"{GetTabs(1)}" +
            $"|{Color("Account Id".PadRight(accountIdW), ConsoleColorCode.Cyan)}" +
            $"|{Color("User Id".PadRight(userIdW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Balance".PadRight(balanceW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Currency Code".PadRight(currencyCodeW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Pin Code".PadRight(pinCodeW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Is Locked".PadRight(isLockedW), ConsoleColorCode.Cyan)}" +
            $"|{Color("Is Deleted".PadRight(isDeletedW), ConsoleColorCode.Cyan)}|"
        );

        Console.WriteLine($"{GetTabs(1)}{new string('-', borderW)}");
        Console.WriteLine(
            $"{GetTabs(1)}" +
            $"|{accountId.PadRight(accountIdW)}" +
            $"|{userId.PadRight(userIdW)}" +
            $"|{balance.ToString().PadRight(balanceW)}" +
            $"|{currencyCode.PadRight(currencyCodeW)}" +
            $"|{pinCode.PadRight(pinCodeW)}" +
            $"|{(isLocked ? "Yes" : "No").PadRight(isLockedW)}" +
            $"|{(isDeleted ? "Yes" : "No").PadRight(isDeletedW)}|"
        );

        Console.WriteLine($"{GetTabs(1)}{new string('-', borderW)}");
    }

    public static bool ValidatePermission(Permissions permissions, Permissions required)
    {
        return permissions.HasFlag(required) || DenyAccess();
    }

    public static bool ValidateRole(UserRole role, UserRole requiredRole)
    {
        if (role == requiredRole)
            return true;

        Console.Clear();
        PrintNewLines(5);
        PrintCenteredHeader($"------- {Color("Access Denied", ConsoleColorCode.Red)} -------", 45, 35, false);
        Console.WriteLine();
        PrintCenteredHeader($"This screen is for {Color(requiredRole.ToString(), ConsoleColorCode.Magenta)} only.", 45, 35, false);
        WaitForUser();
        return false;
    }

    public static bool DenyAccess()
    {
        Console.Clear();
        PrintNewLines(5);
        PrintCenteredHeader(
            $"------- {Color("Access Denied", ConsoleColorCode.Red)} -------",
            45, 35, false
        );
        Console.WriteLine();
        PrintCenteredHeader("You do not have permission to perform this action.", 45, 30, false);
        WaitForUser();
        return false;
    }

    public static void MoveCursorUp(int lineCount)
    {
        int targetRow = Math.Max(0, Console.CursorTop - lineCount);

        Console.SetCursorPosition(Console.CursorLeft, targetRow);
    }

    public static string Color(string text, ConsoleColorCode color)
    {
        if (color is < ConsoleColorCode.Black or > ConsoleColorCode.White)
            return text;

        var ansiCode = 30 + (int)color;
        return $"\u001b[{ansiCode}m{text}\u001b[0m";
    }
}