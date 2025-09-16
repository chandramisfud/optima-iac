<?php
namespace App\Console;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Exception;

class AutoForceClosePromo
{
    public function __invoke()
    {
        Log::info('running job schedule');
        $this->forceClosePromo();
    }

    private function forceClosePromo()
    {
        Log::info('Promo Auto Force Close');
        $api = config('app.api'). '/tools/scheduler/promo/autoclose';

        try {
            $response = Http::post($api);
            if ($response->status() === 200) {
                $data = json_decode($response);
                Log::info($api);
                Log::info(json_encode($data));
            } else {
                Log::error($response->status());
                Log::error($response->reason());
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
        }
    }
}
