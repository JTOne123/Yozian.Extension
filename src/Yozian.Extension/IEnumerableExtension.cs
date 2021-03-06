﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yozian.Extension.Pagination;

namespace Yozian.Extension
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(
            this IEnumerable<T> @this,
            Action<T> processor,
            bool useNewList = false
            )
        {
            // clone & could modify on origin collection
            var cloned = useNewList ? @this.ToList() : @this;
            foreach (var item in cloned)
            {
                processor(item);
            }
        }

        public static void ForEach<T>(
            this IEnumerable<T> @this,
            Action<T, int> processor,
            bool useNewCollection = false)
        {
            // clone & could modify on origin collection dimension
            var collection = useNewCollection ? @this.ToList() : @this;
            var index = 0;
            foreach (var item in collection)
            {
                processor(item, index);
                index++;
            }
        }

        public static string FlattenToString<T>(this IEnumerable<T> @this, string seperator = "")
        {
            return string.Join(seperator, @this.Select(x => x.ToString()));
        }

        public static string FlattenToString<T>(this IEnumerable<T> @this, Func<T, string> converter, string seperator = "")
        {
            return string.Join(seperator, @this.Select(converter));
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> @this,
            Func<TSource, TKey> targetProperty
            )
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in @this)
            {
                var keys = targetProperty(element);
                if (seenKeys.Add(keys))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// loop for items by paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="limits"></param>
        /// <param name="processor"></param>
        public static void ForEachPage<T>(
              this IEnumerable<T> @this,
              int limits,
              Action<IEnumerable<T>, int> processor)
        {
            var pagination = @this.ToPagination(limits);

            pagination.Pages.ForEach((page, index) =>
            {
                // page start from 1
                processor(page, index + 1);
            });
        }

        private static Pagination<T> ToPagination<T>(
              this IEnumerable<T> @this,
              int limits
            )
        {
            if (limits <= 0)
            {
                throw new ArgumentException($"Limits should be greater than 0!");
            }

            var pageCount = 0;

            if (@this.Count() % limits == 0)
            {
                pageCount = @this.Count() / limits;
            }
            else
            {
                pageCount = @this.Count() / limits + 1;
            }

            var pagination = new Pagination<T>()
            {
                PageCount = pageCount,
                Limits = limits
            };

            var pages = new List<IEnumerable<T>>();

            var page = 0;

            while (page < pagination.PageCount)
            {
                var processList = @this
                        .Skip(limits * page)
                        .Take(limits)
                        .ToList();

                if (processList.Count() <= 0)
                {
                    break;
                }

                pages.Add(processList);

                page++;
            }

            pagination.Pages = pages;

            return pagination;
        }

        protected class Pagination<T>
        {
            public int PageCount { get; set; }
            public int Limits { get; set; }
            public IEnumerable<IEnumerable<T>> Pages { get; set; }

            internal Pagination()
            {
            }
        }
    }
}