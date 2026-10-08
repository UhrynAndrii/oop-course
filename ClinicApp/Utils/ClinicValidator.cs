using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhoneRegex =
        new Regex(@"^(?:[0-9]{10}|\+38[0-9]{10})\z");

    private static readonly Regex EmailRegex =
        new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException(
                "Значення не може бути порожнім або містити більше 50 символів.",
                fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) ||
            !PhoneRegex.IsMatch(phone))
        {
            throw new ArgumentException(
                "Номер телефону повинен містити 10 цифр або мати формат +38 та 10 цифр.",
                nameof(phone));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Дата повинна бути не раніше 1900 року і не пізніше сьогодні.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Значення повинно бути більшим за 0.");
        }
    }

    public static void ValidateEmail(string email)
    {
        if (email.Length == 0)
        {
            return;
        }

        if (!EmailRegex.IsMatch(email))
        {
            throw new ArgumentException(
                "Некоректний формат email.",
                nameof(email));
        }
    }
}