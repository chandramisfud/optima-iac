<?php

namespace App\Http\Controllers;

use App\Exports\Export;
use App\Helpers\MyEncrypt;
use App\Http\Controllers\Controller;
use App\Http\Requests;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;

class Search extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function search(Request $request)
    {
        $api = config('app.api'). '/dashboard/main/searchdesktop';
        $data = [
            'keyword'           => $request->keyword,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::withToken($this->token)->asForm()->post($api, $data);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                foreach ($resVal as $data) {
                    if ($data->tipe === "Promo") {
                        $data->categoryShortDesc = urlencode($this->encCategoryId($data->categoryShortDesc));
                    }
                }
                return json_encode([
                    'error' => false,
                    'data'  => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function encCategoryId ($categoryId)
    {
        try {
            return MyEncrypt::encrypt($categoryId);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }

}
