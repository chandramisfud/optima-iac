<?php

namespace App\Providers;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\ServiceProvider;
use Illuminate\Support\Facades\View;
use \Illuminate\Support\Facades\Log;
use Session;

class MenuServiceProvider extends ServiceProvider
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
        view()->composer('panels.menu', function($view) {

            $api_menu = config('app.api'). '/auth/menu';

            $token = Session::get('token');
            $response_menu =  Http::withToken($token)->get($api_menu);
            if ($response_menu->getStatusCode() != 200) {
                $message = json_decode($response_menu)->message;
                Log::warning($api_menu);
                Log::warning($message);

                $result= array(
                    'error'     => true,
                    'message'   => $message
                );
                return $result;
            } else {
                $resVal = json_decode($response_menu)->values;
                $menuData = $resVal->menu;
                Session::put('user_access', $resVal->user_access);
                View::share('menuData',[$menuData]);
            }
        });
    }
}
