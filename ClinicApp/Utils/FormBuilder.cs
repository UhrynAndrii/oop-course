using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class FormBuilder
    {
        public static T Build<T>() where T : new()
        {
            T obj = new T();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetSetMethod() == null)
                {
                    continue;
                }

                string prompt = prop.Name;

                RequiredAttribute? required =
                    prop.GetCustomAttribute<RequiredAttribute>();

                MaxLengthAttribute? maxLength =
                    prop.GetCustomAttribute<MaxLengthAttribute>();

                MinValueAttribute? minValue =
                    prop.GetCustomAttribute<MinValueAttribute>();

                if (required != null || maxLength != null)
                {
                    prompt += " (";

                    if (required != null)
                    {
                        prompt += "обов'язкове";
                    }

                    if (maxLength != null)
                    {
                        if (required != null)
                        {
                            prompt += ", ";
                        }

                        prompt += $"макс. {maxLength.Length}";
                    }

                    prompt += ")";
                }

                if (minValue != null)
                {
                    prompt += $" (мін. {minValue.Min})";
                }

                while (true)
                {
                    Console.Write($"  {prompt}: ");
                    string? input = Console.ReadLine();

                    try
                    {
                        object converted = Convert.ChangeType(
                            input, prop.PropertyType) ?? throw new FormatException();

                        if (required != null &&
                            (converted is string text &&
                             string.IsNullOrWhiteSpace(text)))
                        {
                            throw new FormatException();
                        }

                        if (maxLength != null &&
                            converted is string stringValue &&
                            stringValue.Length > maxLength.Length)
                        {
                            throw new FormatException();
                        }

                        if (minValue != null &&
                            (double)Convert.ChangeType(converted, typeof(double)) < minValue.Min)
                        {
                            throw new FormatException();
                        }

                        prop.SetValue(obj, converted);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(
                            "  Некоректне значення, спробуйте ще раз.");
                    }
                }
            }

            return obj;
        }
    }
}