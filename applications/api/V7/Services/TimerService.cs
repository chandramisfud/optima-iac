using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace V7.Services
{
    public class TimerService : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly TimerTask _timerTask;
        private readonly LoggerService.LoggerManager _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly string _sourceFolderPath;
        private readonly string _destinationFolderPath;
        private readonly string _localFolderPath;
        private readonly string _ftpFolderPath;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private DateTime? _lastRunDateBlitz;
        private DateTime? _lastRunDateEmail;
        private static readonly TimeZoneInfo JakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jakarta");

        public TimerService(IConfiguration configuration, TimerTask timerTask, IServiceScopeFactory scopeFactory)
        {
            _timerTask = timerTask;
            _logger = new LoggerService.LoggerManager();
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _sourceFolderPath = configuration["UploadPath"]!;
            _destinationFolderPath = configuration["UploadBackupPath"]!;
            _localFolderPath = configuration["localBlitzPath"]!;
            _ftpFolderPath = configuration["ftpBlitzPath"]!;
            _host = configuration["ftpHost"]!;
            _username = configuration["ftpUser"]!;
            _password = configuration["ftpPass"]!;

            // Schedule first execution at 6 AM
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, JakartaTimeZone);
            DateTime target = now.Date + new TimeSpan(6, 0, 0); // 6 AM today
            if (now > target && (!_lastRunDateEmail.HasValue || _lastRunDateEmail?.Date < now.Date))
            {
                // Run missed task immediately
                Task.Run(async () => await ExecuteTaskAsync());
                target = target.AddDays(1); // Schedule next run for tomorrow
            }
            TimeSpan dueTime = target - now;
            _timer = new Timer(async state => await ExecuteTaskAsync(), null, dueTime, TimeSpan.FromDays(1));

            _logger.LogInfo($"Timer INIT, Target Time: {target:yyyyMMMdd HH:mm}, TimeZone: {JakartaTimeZone.Id}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo($"TimerService Started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
            return Task.CompletedTask; // Timer already started in constructor
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _logger.LogInfo($"TimerService Stopped at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _semaphore.Dispose();
        }

        public void SetTargetTime(int hour, int minute)
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, JakartaTimeZone);
            DateTime target = now.Date + new TimeSpan(hour, minute, 0);
            if (now > target)
                target = target.AddDays(1);
            TimeSpan dueTime = target - now;
            _timer.Change(dueTime, TimeSpan.FromDays(1));

            _logger.LogInfo($"Timer RESTART, Target Time: {target:yyyyMMMdd HH:mm}, TimeZone: {JakartaTimeZone.Id}");
        }

        public DateTime GetTargetTime()
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, JakartaTimeZone);
            return now.Date + new TimeSpan(6, 0, 0); // Default 6 AM
        }

        private async Task ExecuteTaskAsync()
        {
            if (!await _semaphore.WaitAsync(0))
            {
                _logger.LogInfo($"Task is already running. Skipping at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                return;
            }

            try
            {
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, JakartaTimeZone);
                _logger.LogInfo($"***TASK Executed*** at {now:yyyy-MM-dd HH:mm:ss.fff} WIB");

                await ExecuteBlitzUpdateAsync(now);
                await ExecuteEmailReminderAsync(now);

                // Reschedule for tomorrow at 6 AM
                DateTime tomorrow = now.Date.AddDays(1) + new TimeSpan(6, 0, 0);
                TimeSpan dueTime = tomorrow - TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, JakartaTimeZone);
                _timer.Change(dueTime, TimeSpan.FromDays(1));
                _logger.LogInfo($"Next run scheduled for {tomorrow:yyyyMMMdd HH:mm}, TimeZone: {JakartaTimeZone.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Task Error: {ex.Message}, StackTrace: {ex.StackTrace}, Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task ExecuteBlitzUpdateAsync(DateTime now)
        {
            if (now.TimeOfDay >= new TimeSpan(1, 0, 0) && (!_lastRunDateBlitz.HasValue || _lastRunDateBlitz?.Date < now.Date))
            {
                _logger.LogInfo($"BLITZ Update Task STARTED at {now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                if (await _timerTask.BlitzUpdate(_host, _username, _password, _localFolderPath, _ftpFolderPath))
                {
                    _logger.LogInfo($"BLITZ Update Task FINISHED at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                    _lastRunDateBlitz = now;
                }
                else
                {
                    _logger.LogError($"BLITZ Update Task FAILED at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                }
            }
        }

        private async Task ExecuteEmailReminderAsync(DateTime now)
        {
            using var scope = _scopeFactory.CreateScope();
            var emailRepo = scope.ServiceProvider.GetRequiredService<IToolsEmailRepository>();

            if (now.TimeOfDay >= new TimeSpan(6, 0, 0) && (!_lastRunDateEmail.HasValue || _lastRunDateEmail?.Date < now.Date))
            {
                _logger.LogInfo($"SEND Reminder Promo Approval Email Task STARTED at {now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                if (await _timerTask.SendEmailApprovalReminder())
                {
                    _logger.LogInfo($"SEND Reminder Promo Approval Email Task FINISHED at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                    _lastRunDateEmail = now;
                }
                else
                {
                    _logger.LogError($"SEND Reminder Promo Approval Email Task FAILED at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} WIB");
                }
            }
        }
    }
}