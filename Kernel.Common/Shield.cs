using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kernel.Common
{
    public static class Shield
    {
        /// <summary>
        /// Ensure that a string argument matches a regex pattern. Eg @"^\d{13}" will provide a 13 digit match for IDNumber 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="regexPattern"></param>
        public static void ArgumentMustMatchPattern(string argumentName, string value, string regexPattern)
        {
            Regex rx = new Regex(regexPattern);

            var error = new ArgumentException($"Value '{value}' does not match regex pattern '{regexPattern}'.", argumentName);
            if (!rx.IsMatch(value))
            {
                throw error;
            }
        }

        /// <summary>
        /// If a string is provided ensure that a string argument matches a regex pattern. Eg @"^\d{13}" will provide a 13 digit match for IDNumber 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="regexPattern"></param>
        public static void ArgumentMustMatchPatternIfNotNullOrEmpty(string argumentName, string value, string regexPattern)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var rx = new Regex(regexPattern);
            var error = new ArgumentException($"Value '{value}' does not match regex pattern '{regexPattern}'.", argumentName);
            if (!rx.IsMatch(value))
            {
                throw error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void ArgumentValueInBounds(string argumentName, DateTime value, DateTime? minValue = null, DateTime? maxValue = null)
        {
            if (minValue == null)
            {
                minValue = DateTime.MinValue;
            }
            if (maxValue == null)
            {
                maxValue = DateTime.MaxValue;
            }

            var error = new ArgumentException($"Value must be at least {minValue.ToString()} and at most {maxValue.ToString()}.", argumentName);
            if (value > maxValue || value < minValue)
            {
                throw error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        public static void NumberIsInRange(string argumentName, string value, int minNumber, int maxNumber)
        {
            var error = new ArgumentException($"Value must be between {minNumber} and  {maxNumber}.", argumentName);
            if (!Enumerable.Range(1, 3).Contains(int.Parse(value)))
            {
                throw error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        public static void ArgumentLength(string argumentName, string value, int minLength, int maxLength)
        {
            var error = new ArgumentException($"Length of value must be at least {minLength} and at most {maxLength}.", argumentName);
            if (value == null && minLength != 0)
            {
                throw error;
            }
            else if (value == null && minLength == 0)
            {
                return;
            }
            else if (value.Length > maxLength || value.Length < minLength)
            {
                throw error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ArgumentItemCount<T>(string argumentName, IEnumerable<T> value, int minLength = 0, int maxLength = 0)
        {
            ArgumentException error;
            if (minLength > 0 && maxLength > 0)
            {
                if (value.Count() < minLength || value.Count() > maxLength)
                {
                    throw new ArgumentException($"Number of items in collection must be at least {minLength} and at most {maxLength}.", argumentName); ;
                }
            }
            else if (minLength > 0)
            {
                if (value.Count() < minLength)
                {
                    throw new ArgumentException($"Number of items in collection must be at least {minLength}.", argumentName);
                }
            }
            else if (maxLength > 0)
            {
                if (value.Count() > maxLength)
                {
                    throw new ArgumentException($"Number of items in collection must be at most {maxLength}.", argumentName);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ArgumentIsOfType(string argumentName, Type value, Type c)
        {
            if (!value.IsAssignableFrom(c))
            {
                throw new ArgumentException($"{argumentName} is not of type {c.Name}");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ArgumentNotNull(string argumentName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }


        /// <summary>
        /// In some cases an argument MUST be null
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        public static void ArgumentIsNull(string argumentName, object value)
        {
            if (value != null)
            {
                throw new ArgumentException($"{argumentName} must be null"); ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentName"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void ArgumentNotNullOrEmpty(string argumentName, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be an empty string.", argumentName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumentName"></param>
        /// <param name="items"></param> // Use IReadOnlyCollection<T> instead of IEnumerable<T> to discourage double enumeration
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void ArgumentNotNullOrEmpty<T>(string argumentName, IReadOnlyCollection<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (items.Count == 0)
            {
                throw new ArgumentException("Collection must contain at least one item.", argumentName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string StringHasCleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\ .@)(-]", "",
                    RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="argumentName"></param>
        /// <param name="exceptionMessage"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ArgumentValid(bool valid, string argumentName, string exceptionMessage)
        {
            if (!valid)
            {
                throw new ArgumentException(exceptionMessage, argumentName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="exceptionMessage"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void OperationValid(bool valid, string exceptionMessage)
        {
            if (!valid)
            {
                throw new InvalidOperationException(exceptionMessage);
            }
        }
    }
}
