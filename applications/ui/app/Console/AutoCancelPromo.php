<?php
namespace App\Console;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Exception;

class AutoCancelPromo {

    public function __invoke()
    {
        Log::info('jalankan job schedule Auto Cancel');
        $this->autoCancelPromoProgress();
    }

    public function getAutoCancelPromoList(){
        $api = config('app.api') . '/tools/scheduler/promo/autoclosing';
        try {
            Log::info('get API ' . $api);
            $response = Http::get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                if ($resVal) {
                    return ($resVal);
                }else{
                    Log::info($api. "no Data");
                    return array(
                        'values' => []
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'values' => []
                );
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function autoCancelPromoProgress(){
        $apiEmail = config('app.api') . '/tools/email';
        $count = 1;

        foreach($this->getAutoCancelPromoList() as $r) {
            $cancelPromo = FALSE;
            $apiPromo = config('app.api'). '/tools/scheduler/promo/cancel';

            $dataPromo = [
                "promoId"               => $r->id,
                "userId"                => "system",
                "statusCode"            => "TP2",
            ];

            $responsePromo = Http::asForm()->post($apiPromo, $dataPromo);
            if ($responsePromo->status() === 200) {
                Log::info('Cancel Promo by System ('.$r->refId.')');
                $resValPromo = json_decode($responsePromo);
                Log::info($apiPromo);
                Log::info($resValPromo);

                $cancelPromo = TRUE;
            } else {
                $resValPromo = json_decode($responsePromo);
                Log::error($apiPromo);
                Log::error($resValPromo);
            }

            // cacnel promo Planning
            $cancelPlanning = FALSE;
            $apiPlanning = config('app.api'). '/tools/scheduler/promo/planning/cancel';

            // ip_promo_planning_cancel
            $dataPlanning = [
                "promoPlanId"     => $r->promoPlanId,
                "notes"           => "Auto Cancel by System",
                "userId"          => "system"
            ];

            $responsePlanning = Http::asForm()->post($apiPlanning, $dataPlanning);
            if ($responsePlanning->status() === 200) {
                Log::info('Cancel Planning by System ('.$r->promoPlanId.')');
                $resValPlanning = json_decode($responsePlanning);
                Log::info($apiPromo);
                Log::info($resValPlanning);

                $cancelPlanning = TRUE;
            } else {
                $resValPlanning = json_decode($responsePlanning);
                Log::error($apiPromo);
                Log::error($resValPlanning);
            }

            if($cancelPromo && $cancelPlanning){
                // email notif
                Log::info('send notif Auto Cancel by System for ' . $r->refId);
                Log::info('send email Auto Cancel by System to Initiator (' . $r->createBy . ' - ' . $r->emailInitiator . ')');

                $queryInitiator = [
                    'email'     => $r->emailInitiator,
                    'subject'   => "[AUTO CLOSE PROMO] ".$r->refId,
                    'message'   => "Your Promo ID <b>is automatically cancelled by system</b> because there is no approval or sendback from first approver over 45 days from creation date.<br><p><p>Promo Number : " . $r->refId ."<br>Aging Day : " . $r->dayfreeze ." days<br><p><p>Thank you,<br>Optima System"
                ];

                $responseEmail = Http::asForm()->post($apiEmail, $queryInitiator);
                if ($responseEmail->status() === 200) {
                    Log::info('send email to initiator success ' . $count);
                    $resValEmail = json_decode($responseEmail);
                    Log::info($apiEmail);
                    Log::info($resValEmail);

                } else {
                    $resValEmail = json_decode($responseEmail);
                    Log::error($apiEmail);
                    Log::error($resValEmail);
                }
            }

            if($count==40) {
                sleep(60 * 35);
                Log::info('waiting for 60 * 35 second');
            }
            $count += 1;
        }
    }
}
