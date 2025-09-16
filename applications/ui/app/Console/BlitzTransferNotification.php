<?php
namespace App\Console;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class BlitzTransferNotification
{
    public function __invoke()
    {
        Log::info('running job schedule');
        $this->sendingEmailBlitzTransferNotification();
    }

    private function sendingEmailBlitzTransferNotification ()
    {
        Log::info('Email Blitz Notification');
        $api = config('app.api'). '/tools/scheduler/blitznotif';
        $response = Http::get($api);

        if ($response->status() === 200) {
            $data = json_decode($response)->values[0]->itemtype;
            $data2 = json_decode($response)->values[0]->blitzemail[0];

            if (!$data) {
                $values= array();
                $result= array(
                    'values' => $values
                );
                $data = $result['values'];
            }
            $message = view('Promo::email-reminder.blitz-transfer-notif.view_email_blitz', compact('data'))->render();

            $query = [
                'email'     => (explode(";",$data2->email_to)),
                'subject'   => $data2->email_subject,
                'body'      => $message,
                'cc'        => (explode(";",$data2->email_cc)),
                'bcc'       => []
            ];

            $apiEmail = config('app.api'). '/tools/email';
            $responseEmail = Http::asForm()->post($apiEmail, $query);
            if ($responseEmail->status() === 200) {
                Log::info('Email send success');
            } else {
                $resValEmail = json_decode($responseEmail);
                Log::error($apiEmail);
                Log::error(json_encode($resValEmail));
            }

        } else {
            Log::error($api);
            Log::error($response->status());
            Log::error($response->reason());
        }
    }

}
