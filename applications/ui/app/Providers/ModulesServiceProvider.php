<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;
use Log;

class ModulesServiceProvider extends ServiceProvider
{
    /**
     * Register services.
     *
     * @return void
     */
    public function register()
    {
        //
    }

    /**
     * Bootstrap services.
     *
     * @return void
     */
    public function boot()
    {
        // For each of the registered modules, include their routes and Views
        $modules = config("module.modules");

        foreach($modules as $module) {
            // Load the routes for each of the modules
            if(file_exists(__DIR__.'/../Modules/'.$module.'/routes.php')) {
                include __DIR__.'/../Modules/'.$module.'/routes.php';
            }

            // Load the views
            if(is_dir(__DIR__.'/../Modules/'.$module.'/Views')) {
                $this->loadViewsFrom(__DIR__.'/../Modules/'.$module.'/Views', $module);
            }
        }
    }
}
