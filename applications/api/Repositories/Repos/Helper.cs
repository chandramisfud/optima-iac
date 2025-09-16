using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Repos
{
    /// <summary>
    /// Helper
    /// </summary>
    public static class Helper
    {
        private static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
        /// <summary>
        /// generateStrongPassword
        /// </summary>
        public static string generateStrongPassword(int length = 8, bool add_dashes = false, string available_sets = "luds")
        {
            List<string> sets = new();
            if (available_sets.Contains('l'))
                sets.Add("abcdefghjkmnpqrstuvwxyz");
            if (available_sets.Contains('u'))
                sets.Add("ABCDEFGHJKMNPQRSTUVWXYZ");
            if (available_sets.Contains('d'))
                sets.Add("23456789");
            if (available_sets.Contains('s'))
                sets.Add("!@#$%&*?");


            string all = "";
            string password = "";
            var random = new Random();

            foreach (string set in sets)
            {
                password += set[random.Next(set.Length)];
                all += set;
            }

            //all = str_split($all);
            for (int i = 0; i < length - sets.Count; i++)
            {
                password += all[random.Next(all.Length)];
            }
            password = Shuffle(password);

            //if (!add_dashes)
            return password;
        }

        public static List<object> GetPagedData(List<object> data, int page, int pageSize)
        {
            //based zero request
            page++;
            // Melakukan paging berdasarkan nomor halaman dan ukuran halaman yang diminta
            var startIndex = (page - 1) * pageSize;
            var endIndex = startIndex + pageSize;

            if (startIndex >= data.Count)
            {
                // Jika nomor halaman melebihi jumlah total data yang ada, kembalikan koleksi kosong
                return new List<object>();
            }

            if (endIndex > data.Count)
            {
                // Jika indeks akhir melebihi jumlah total data yang ada, batasi indeks akhir
                endIndex = data.Count;
            }

            // Mengambil data yang sesuai dengan halaman yang diminta
            return data.GetRange(startIndex, endIndex - startIndex);
        }


        public static DataTable _castToDataTable<T>(T __type, List<T> __contents)
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

        public static DataTable _castToDataTableV2<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new()
            {
                TableName = typeof(T).FullName
            };
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity)!;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable ConvertObjectToDataTable(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "The object cannot be null.");

            DataTable table = new DataTable(obj.GetType().Name);

            // Use reflection to get properties
            PropertyInfo[] properties = obj.GetType().GetProperties();

            // Add columns based on properties
            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Add a single row with the values of the object's properties
            var row = table.NewRow();
            foreach (var prop in properties)
            {
                row[prop.Name] = prop.GetValue(obj) ?? DBNull.Value;
            }
            table.Rows.Add(row);

            return table;
        }
    }
}
