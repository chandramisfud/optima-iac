<?php

namespace App\Modules\Dashboard\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Http;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;

class DashboardController extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage(Request $request)
    {
        $api_menu = config('app.api'). '/auth/menu';
        Log::info($api_menu);
        try {
            $response_menu =  Http::timeout(180)->withToken($this->token)->get($api_menu);
            if ($response_menu->status() === 200) {
                $resVal = json_decode($response_menu)->values;
                $menuData = $resVal->menu->menu;
                foreach ($menuData as $data) {
                    if ($data->menuid == 1) {
                        return redirect('/dashboard/main');
                    } else {
                        return view('Dashboard::blank');
                    }
                }
            } else {
                $message = json_decode($response_menu)->message;
                Log::warning($api_menu);
                Log::warning($message);
                return view('Dashboard::blank');
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api_menu);
            Log::error($e->getMessage());

            return view('Dashboard::blank');
        }
    }

    public function search(Request $request){
        $api = config('app.api') . '/tools/searchdesktop';
        $data = [
            'keyword'         => $request->search
        ];
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response = Http::timeout(180)->asForm()->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $resValue = json_decode($response)->values;
                $message = json_decode($response)->message;
                $result= array(
                    'error'     => false,
                    'message'   => $message,
                    'data'      => $resValue
                );
                return $result;
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                $err = [
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                ];
            } else {
                $err = [
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                ];
            }
            Log::error($err);
            return $err;
        }
    }
}
