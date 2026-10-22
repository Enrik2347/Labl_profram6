using System;
using System.Globalization;

class Program
{
    const int Capacity = 100;

    static int[] ids = new int[Capacity];
    static DateTime[] dates = new DateTime[Capacity];
    static string[] categories = new string[Capacity];
    static string[] descriptions = new string[Capacity];
    static decimal[] amounts = new decimal[Capacity];
    static bool[] isIncome = new bool[Capacity];
    static string[] accounts = new string[Capacity];

    static int count = 0;
    static int nextId = 1;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Store(new DateTime(2026, 10, 1), "Зарплата", "Аванс", 12000m, true, "Картка");
        Store(new DateTime(2026, 10, 3), "Продукти", "Супермаркет", 850.50m, false, "Картка");
        Store(new DateTime(2026, 10, 5), "Транспорт", "Проїзний", 400m, false, "Готівка");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Додати операцію");
            Console.WriteLine("2 - Показати всі");
            Console.WriteLine("3 - Знайти за номером");
            Console.WriteLine("4 - Видалити за номером");
            Console.WriteLine("0 - Вихід");
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine();

            if (choice == "0") break;
            else if (choice == "1") AddRecord();
            else if (choice == "2") PrintAll();
            else if (choice == "3") FindRecord();
            else if (choice == "4") DeleteRecord();
            else Console.WriteLine("Немає такого пункту меню.");
        }
    }

    static void Store(DateTime date, string category, string description,
                      decimal amount, bool income, string account)
    {
        ids[count] = nextId;
        dates[count] = date;
        categories[count] = category;
        descriptions[count] = description;
        amounts[count] = amount;
        isIncome[count] = income;
        accounts[count] = account;
        count++;
        nextId++;
    }

    static int IndexOfId(int id)
    {
        for (int i = 0; i < count; i++)
        {
            if (ids[i] == id) return i;
        }
        return -1;
    }

    static void AddRecord()
    {
        if (count == Capacity)
        {
            Console.WriteLine("Сховище заповнене.");
            return;
        }

        Console.Write("Дата (дд.мм.рррр): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Неправильна дата.");
            return;
        }

        Console.Write("Категорія: ");
        string category = Console.ReadLine() ?? "";

        Console.Write("Опис: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Сума (дробова частина через крапку): ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number,
            CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Сума має бути додатним числом.");
            return;
        }

        Console.Write("Тип (1 - дохід, 2 - витрата): ");
        bool income = Console.ReadLine() == "1";

        Console.Write("Рахунок: ");
        string account = Console.ReadLine() ?? "";

        Store(date, category, description, amount, income, account);
        Console.WriteLine("Операцію додано.");
    }

    static void PrintAll()
    {
        if (count == 0)
        {
            Console.WriteLine("Операцій немає.");
            return;
        }

        decimal totalIncome = 0;
        decimal totalExpense = 0;

        Console.WriteLine("№  | Дата       | Категорія    | Опис         | Сума       | Тип     | Рахунок");
        for (int i = 0; i < count; i++)
        {
            string type = isIncome[i] ? "дохід" : "витрата";
            Console.WriteLine($"{ids[i],-2} | {dates[i]:dd.MM.yyyy} | {categories[i],-12} | {descriptions[i],-12} | {amounts[i],10:F2} | {type,-7} | {accounts[i]}");

            if (isIncome[i]) totalIncome += amounts[i];
            else totalExpense += amounts[i];
        }

        Console.WriteLine($"Усього операцій: {count}");
        Console.WriteLine($"Доходи: {totalIncome:F2} | Витрати: {totalExpense:F2} | Баланс: {totalIncome - totalExpense:F2}");
    }

    static void FindRecord()
    {
        Console.Write("Номер операції: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Номер має бути цілим числом.");
            return;
        }

        int i = IndexOfId(id);
        if (i == -1)
        {
            Console.WriteLine("Такої операції немає.");
            return;
        }

        Console.WriteLine($"Дата: {dates[i]:dd.MM.yyyy}");
        Console.WriteLine($"Категорія: {categories[i]}");
        Console.WriteLine($"Опис: {descriptions[i]}");
        Console.WriteLine($"Сума: {amounts[i]:F2}");
        Console.WriteLine($"Тип: {(isIncome[i] ? "дохід" : "витрата")}");
        Console.WriteLine($"Рахунок: {accounts[i]}");
    }

    static void DeleteRecord()
    {
        Console.Write("Номер операції: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Номер має бути цілим числом.");
            return;
        }

        int index = IndexOfId(id);
        if (index == -1)
        {
            Console.WriteLine("Такої операції немає.");
            return;
        }

        for (int i = index; i < count - 1; i++)
        {
            ids[i] = ids[i + 1];
            dates[i] = dates[i + 1];
            categories[i] = categories[i + 1];
            descriptions[i] = descriptions[i + 1];
            amounts[i] = amounts[i + 1];
            isIncome[i] = isIncome[i + 1];
            accounts[i] = accounts[i + 1];
        }
        count--;

        Console.WriteLine("Операцію видалено.");
    }
}