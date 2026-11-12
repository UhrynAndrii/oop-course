
using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class ModelValidator
    {
        public static ValidationResult Validate(object obj)
        {
            ValidationResult result = new ValidationResult();

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                object? value = prop.GetValue(obj);

                RequiredAttribute? required =
                    prop.GetCustomAttribute<RequiredAttribute>();

                if (required != null)
                {
                    if (value == null ||
                        (value is string text && string.IsNullOrWhiteSpace(text)))
                    {
                        result.AddError($"{prop.Name}: {required.ErrorMessage}");
                    }
                }

                MaxLengthAttribute? maxLength =
                    prop.GetCustomAttribute<MaxLengthAttribute>();

                if (maxLength != null && value is string stringValue)
                {
                    if (stringValue.Length > maxLength.Length)
                    {
                        result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
                    }
                }

                MinValueAttribute? minValue =
                    prop.GetCustomAttribute<MinValueAttribute>();

                if (minValue != null && value != null)
                {
                    if (Convert.ToDouble(value) < minValue.Min)
                    {
                        result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
                    }
                }
            }

            return result;
        }

        public static void PrintInfo(Type type)
        {
            Console.WriteLine($"Type: {type.Name}");

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                bool hasAttributes = false;

                foreach (object attribute in prop.GetCustomAttributes())
                {
                    if (!hasAttributes)
                    {
                        Console.WriteLine($"Property: {prop.Name}");
                        hasAttributes = true;
                    }

                    Console.WriteLine($"  Attribute: {attribute.GetType().Name}");
                }
            }
        }
    }
}