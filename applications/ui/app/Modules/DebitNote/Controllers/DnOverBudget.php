<?php

namespace App\Modules\DebitNote\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class DnOverBudget extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "Debit Note [Refresh]";
        return view('DebitNote::dn-over-budget.index', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/dn/overbudget';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values
                );
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function refresh(Request $request) {
        $api = config('app.api') . '/dn/overbudget';
        $data = [
            'promoId'     => $request->promoId,
        ];
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

}
