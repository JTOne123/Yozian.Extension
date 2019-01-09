﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Yozian.Extension
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string @this, bool enableFailOverWithDefaultValue = true)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), @this, true);
            }
            catch (Exception)
            {
                if (!enableFailOverWithDefaultValue)
                {
                    throw;
                }
                return default(T);
            }
        }

        public static string LimitLength(this string @this, int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            if (@this.Length > length)
            {
                return @this.Substring(0, length);
            }
            return @this;
        }

        public static string Repeate(
            this string @this,
            int count,
            string seperator = ""
            )
        {
            return string.Join(seperator, Enumerable.Repeat(@this, count));
        }
    }
}