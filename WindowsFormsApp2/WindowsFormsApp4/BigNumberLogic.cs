using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2
{
    /// <summary>
    /// Класс для выполнения арифметических операций над многоразрядными целыми числами,
    /// представленными в виде строк.
    /// </summary>
    public class BigNumberLogic
    {
        private static bool IsValidNumber(string num)
        {
            if (string.IsNullOrWhiteSpace(num)) return false;
            int startIndex = (num[0] == '-' || num[0] == '+') ? 1 : 0;
            if (startIndex == num.Length) return false;

            for (int i = startIndex; i < num.Length; i++)
            {
                if (!char.IsDigit(num[i])) return false;
            }
            return true;
        }
        private static string GetAbs(string num)
        {
            if (num[0] == '-' || num[0] == '+') return num.Substring(1);
            return num;
        }
        private static bool IsNegative(string num)
        {
            return num.Length > 0 && num[0] == '-';
        }
        private static int CompareAbs(string a, string b)
        {
            a = a.TrimStart('0');
            b = b.TrimStart('0');
            if (a.Length > b.Length) return 1;
            if (a.Length < b.Length) return -1;
            return string.Compare(a, b);
        }
        private static string AddAbs(string a, string b)
        {
            StringBuilder result = new StringBuilder();
            int i = a.Length - 1, j = b.Length - 1, carry = 0;

            while (i >= 0 || j >= 0 || carry > 0)
            {
                int digitA = (i >= 0) ? a[i--] - '0' : 0;
                int digitB = (j >= 0) ? b[j--] - '0' : 0;
                int sum = digitA + digitB + carry;
                result.Append(sum % 10);
                carry = sum / 10;
            }

            char[] charArray = result.ToString().ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray).TrimStart('0') == "" ? "0" : new string(charArray).TrimStart('0');
        }
        private static string SubtractAbs(string a, string b)
        {
            StringBuilder result = new StringBuilder();
            int i = a.Length - 1, j = b.Length - 1, borrow = 0;

            while (i >= 0)
            {
                int digitA = a[i--] - '0' - borrow;
                int digitB = (j >= 0) ? b[j--] - '0' : 0;

                if (digitA < digitB)
                {
                    digitA += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result.Append(digitA - digitB);
            }

            char[] charArray = result.ToString().ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray).TrimStart('0') == "" ? "0" : new string(charArray).TrimStart('0');
        }

        /// <summary>
        /// Сложение двух многоразрядных чисел
        /// </summary>
        public static string Add(string num1, string num2)
        {
            if (!IsValidNumber(num1) || !IsValidNumber(num2))
                throw new ArgumentException("Некорректный формат числа");

            bool neg1 = IsNegative(num1);
            bool neg2 = IsNegative(num2);
            string abs1 = GetAbs(num1);
            string abs2 = GetAbs(num2);

            if (neg1 == neg2)
            {
                string res = AddAbs(abs1, abs2);
                return (neg1 && res != "0") ? "-" + res : res;
            }
            else
            {
                int cmp = CompareAbs(abs1, abs2);
                string res;
                if (cmp >= 0)
                {
                    res = SubtractAbs(abs1, abs2);
                    return (neg1 && res != "0") ? "-" + res : res;
                }
                else
                {
                    res = SubtractAbs(abs2, abs1);
                    return (neg2 && res != "0") ? "-" + res : res;
                }
            }
        }

        /// <summary>
        /// Вычитание двух многоразрядных чисел (num1 - num2)
        /// </summary>
        public static string Subtract(string num1, string num2)
        {
            if (!IsValidNumber(num1) || !IsValidNumber(num2))
                throw new ArgumentException("Некорректный формат числа");
            string negNum2;
            if (IsNegative(num2))
                negNum2 = num2.Substring(1);
            else
                negNum2 = "-" + num2;

            return Add(num1, negNum2);
        }

        /// <summary>
        /// Увеличение числа на 1
        /// </summary>
        public static string Increment(string num)
        {
            return Add(num, "1");
        }

        /// <summary>
        /// Уменьшение числа на 1
        /// </summary>
        public static string Decrement(string num)
        {
            return Subtract(num, "1");
        }
    }
}
