<?php

namespace App\Http\Middleware;

use Closure;
use Exception;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class CheckUserSession
{
    /**
     * Handle an incoming request.
     *
     * @param Request $request
     * @param Closure $next
     * @return mixed
     */
    public function handle(Request $request, Closure $next): mixed
    {
        if (!$request->session()->exists('role')) {
            return redirect('/login-page');
        } else {
            $islogin = $request->session()->get('islogin');
            $token = $request->session()->get('token');

            $api = config('app.api') . '/auth/islogin';
            try {
                $response = Http::withToken($token)->get($api);
                if ($response->status() === 401) {
                    $message = json_decode($response)->message;
                    Log::info($message);
                    $request->session()->flush();
                    if (!$request->ajax()) {
                        return redirect('/login-page?islogin=1');
                    } else {
                        return abort(403);
                    }
                } else {
                    $resValue = json_decode($response)->values;
                    if ($islogin !== $resValue) {
                        if (!$request->ajax()) {
                            $request->session()->flush();
                            return redirect('/login-page?islogin=1');
                        } else {
                            $request->session()->flush();
                            return abort(403);
                        }
                    }
                }
            } catch (Exception $e) {
                Log::error('get API ' . $api);
                Log::error($e->getMessage());
            }
        }

        return $next($request);
    }
}
