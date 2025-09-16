using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public static class Tools
    {
        public static DataTable CastToDataTable<T>(T __type, List<T> __contents)
        {
            DataTable datas = new();
            try
            {
                PropertyInfo[] __columns = typeof(T).GetProperties();
                foreach (PropertyInfo v in __columns)
                {
                    if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                        datas.Columns.Add(v.Name, v.PropertyType);
                    else
                        datas.Columns.Add(__dispName.DisplayName, v.PropertyType);
                }
                if (__contents != null)
                {
                    foreach (var r in __contents)
                    {
                        DataRow __row = datas.NewRow();
                        foreach (PropertyInfo v in __columns)
                        {
                            if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                                __row[v.Name] = v.GetValue(r);
                            else
                                __row[__dispName.DisplayName] = v.GetValue(r);
                        }
                        datas.Rows.Add(__row);
                    }
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return datas;
        }
    }
}
