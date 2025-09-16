<?php

namespace App\Console;

use Illuminate\Console\Scheduling\Schedule;
use Illuminate\Foundation\Console\Kernel as ConsoleKernel;

class Kernel extends ConsoleKernel
{
    /**
     * Define the application's command schedule.
     *
     * @param Schedule $schedule
     * @return void
     */
    protected function schedule(Schedule $schedule)
    {
        // Approval Reminder
        $schedule->call(new SendEmail())->dailyAt('06:00');

        // Auto Cancel Promo
//        $schedule->call(new AutoCancelPromo())->dailyAt('06:00');

        // Reminder Pending Approval
        $schedule->call(new ReminderPendingApproval())->weekly()->fridays()->at('08:00');

        // Blitz Transfer Notif
        $schedule->call(new BlitzTransferNotification())->dailyAt('07:00');

        // Auto Force Close Promo
        $schedule->call(new AutoForceClosePromo())->dailyAt('01:00');

        // Approval Regular
        $schedule->call(new SendEmailApprovalRegular())->dailyAt('08:00');
    }

    /**
     * Register the commands for the application.
     *
     * @return void
     */
    protected function commands()
    {
        $this->load(__DIR__.'/Commands');

        require base_path('routes/console.php');
    }
}
