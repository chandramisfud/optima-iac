using System.Data;
using System.Reflection;
using V7.Model.Promo;

namespace V7.Services
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
                (array[n], array[k]) = (array[k], array[n]);
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


        public static List<object> SortDataBy(List<object> data, string columnName, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("Column name cannot be null or empty.");
            }

            if (sortDirection != "asc" && sortDirection != "desc")
            {
                throw new ArgumentException("Invalid sort direction. Use 'asc' or 'desc'.");
            }

            PropertyInfo propertyInfo = typeof(object).GetProperty(columnName)! ?? throw new ArgumentException($"Invalid column name '{columnName}'.");
            if (sortDirection == "asc")
            {
                return data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
            }
            else
            {
                return data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
            }
        }

        public static DataTable ConvertParamToDataTable<T>(T __type, List<T> __contents)
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

        public static DataTable ArrayIntToKeyId(List<ArrayIntType> arrId)
        {
            DataTable dtKey = new("ArrayIntType");
            dtKey.Columns.Add("keyid");
            if (arrId != null)
            {
                foreach (var item in arrId)
                {
                    dtKey.Rows.Add(item.id);
                }
            }
            else
            {
                dtKey.Rows.Add(0);
            }

            return dtKey;
        }

        public static DataTable ListIntToKeyId(int[] arrId)
        {
            DataTable dtKey = new("ArrayIntType");
            dtKey.Columns.Add("keyid");
            if (arrId != null)
            {
                foreach (var item in arrId)
                {
                    dtKey.Rows.Add(item);
                }
            }
            else
            {
                dtKey.Rows.Add(0);
            }

            return dtKey;
        }

        public static DataTable ListStringToId(string[] arrId)
        {
            DataTable dtKey = new("ArrayIntType");
            dtKey.Columns.Add("id");
            if (arrId != null)
            {
                foreach (var item in arrId)
                {
                    dtKey.Rows.Add(item);
                }
            }
            else
            {
                dtKey.Rows.Add(0);
            }

            return dtKey;
        }
    }
}
