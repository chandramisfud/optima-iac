<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;
use File;

class AppServiceProvider extends ServiceProvider
{
    /**
     * Register any application services.
     *
     * @return void
     */
    public function register()
    {
        //
    }

    /**
     * Bootstrap any application services.
     *
     * @return void
     */
    public function boot()
    {
        $path = File::allFiles(storage_path('logs'));
        $last_date = date('Y-m-d', strtotime('-' . config('app.log_aging') . ' days'));
        foreach($path as $list) {
            $file = pathinfo($list);
            $removePrefix = substr(str_replace('optima-', '', $file['filename']), 0, 10);
            $datetimeFile = date('Y-m-d', strtotime($removePrefix));
            if ($datetimeFile <= $last_date) {
                if (storage_path('logs/' . $file['basename'])) {
                    unlink(storage_path('logs/' . $file['basename']));
                }
            }
        }
    }
}
