using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Simple.Net.Core.Data.Mapper;

namespace Simple.Net.Core.Data.Helpers
{
    public static class DataTableHelper
    {

        public static IEnumerable<T> MapToList<T>(this DataTable table, IRowMapper<T> map)
        {
            var reader = table.CreateDataReader();

            while (reader.Read())
            {
                yield return map.MapRow(reader);
            }
        }

        public static IList<T> ToList<T>(this DataTable table, string[] propertiesToExclude = null) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, propertiesToExclude ?? new string[] { }, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, string[] propertiesToExclude, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties)
            {
                if (propertiesToExclude.Contains(property.Name)) continue;

                try
                {
                    var value = row[property.Name];

                    if (value is DBNull)
                    {
                        property.SetValue(item, null, null);
                    }
                    else
                    {
                        property.SetValue(item, row[property.Name], null);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Unable to add value to property:{0} for type:{1} Message:{2}"
                        , property.Name
                        , item.GetType()
                        , ex.Message)
                        );
                }

            }
            return item;
        }

    }

}
