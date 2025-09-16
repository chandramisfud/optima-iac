using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public class BGLogger
    {
        private static readonly object _lock = new object();

        public static void WriteLog(string message)
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

                // Buat folder Logs jika belum ada
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                // Format nama file berdasarkan tanggal dan jam
                string logFileName = $"logs_{DateTime.UtcNow.AddHours(7):yyyyMMdd_HH}.txt";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Format pesan log
                string logMessage = $"{DateTime.UtcNow.AddHours(7):HH:mm:ss} - {message}";

                // Tulis ke file dengan lock untuk mencegah race condition
                lock (_lock)
                {
                    File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw new Exception("Failed to write log: {ex.Message}");
            }
        }
    }
}
