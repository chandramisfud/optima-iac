<?php
namespace App\Console;

use App\Exports\Export;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class ReminderPendingApproval
{
    public function __invoke()
    {
        Log::info('running job schedule');
        $this->sendingEmailPendingApproval();
    }

    private function sendingEmailPendingApproval()
    {
        Log::info('Email Pending Approval');
        $api = config('app.api'). '/tools/scheduler/reminder/pendingapproval';

        $response = Http::get($api);
        if ($response->status() === 200) {
            $data = json_decode($response)->values[0]->aging;
            $data2 = json_decode($response)->values[0]->emailPending[0];
            $dataAttachment = json_decode($response)->values[0]->pendingPromo;

            if (!$data) {
                Log::info($api ." no Data");
                $values= array();
                $result= array(
                    'values' => $values
                );
                $data = $result['values'];
            }

            Log::info('send email promo display');

            $fileName = $this->generateAttachment($dataAttachment);
            $fileAttachment = '/assets/media/promo/pendingapproval/' . $fileName;
            Log::info('File Attachment : ' . $fileAttachment);
            $message = view('Promo::email-reminder.promo-display.email_aging_body', compact('data', 'fileAttachment'))->render();

            $query = [
                'email'     => (explode(";",$data2->email_to)),
                'subject'   => "[OPTIMA REMINDER] Pending Approval and Sendback Aging Summary",
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

    private function generateAttachment($dataAttachment): BinaryFileResponse
    {
        Log::info('Generate File Attachment Reminder Pending Approval');
        $path = '/assets/media/promo/pendingapproval';
        if (!Storage::disk('optima')->exists($path)) {
            Storage::disk('optima')->makeDirectory($path);
        }

        $result = [];
        foreach ($dataAttachment as $fields) {
            $arr = [];

            $arr[] = $fields->promo_id;
            $arr[] = $fields->last_status;
            $arr[] = $fields->promo_initiator;
            $arr[] = $fields->initiator_name;
            $arr[] = $fields->creation_date;
            $arr[] = $fields->channel;
            $arr[] = $fields->sub_account;
            $arr[] = $fields->promo_start;
            $arr[] = $fields->promo_end;
            $arr[] = $fields->activity_name;
            $arr[] = $fields->mechanism_1;
            $arr[] = $fields->investment;
            $arr[] = $fields->aging;

            $result[] = $arr;
        }

        $filename = 'Pending Approval-';
        $title = 'A1:M1';
        $header = 'A3:M3';
        $heading = [
            ['Pending Approval'],
            ['Date Retrieved : ' . date('Y-m-d')],
            [
                'Promo ID',
                'Last Status',
                'Promo Initiator',
                'Initiator Name',
                'Creation Date',
                'Channel',
                'Sub Account',
                'Promo Start',
                'Promo End',
                'Activity Name',
                'Mechanism',
                'Investment',
                'Aging',
            ]
        ];
        $formatCell =  [
            'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'M' => NumberFormat::FORMAT_NUMBER,
        ];

        $export = new Export($result, $heading, $title, $header, $formatCell);
        $mc = microtime(true);

        Excel::store($export, $filename . date('Y-m-d His') . $mc . '.xlsx','local');
        Storage::disk('optima')->move($filename . date('Y-m-d His') . $mc . '.xlsx', '/assets/media/promo/pendingapproval/'.$filename . date('Y-m-d His') . $mc . '.xlsx');
        return Excel::download($export, $filename . date('Y-m-d') . ' ' . $mc .  '.xlsx');
    }

}
