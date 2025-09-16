<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Export;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class PromoPlanningApproval extends Controller
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
        $title = "Promo Planning";
        return view('Promo::promo-planning-approval.index', compact('title'));
    }

    public function downloadTemplate(Request $request)
    {
        $api = config('app.api'). '/promo/planningapproval/promoplanning';
        $query = [
            'periode'                   => $request->period,
            'create_from'               => $request->period . '-01-01',
            'create_to'                 => $request->period . '-12-31',
            'start_from'                => $request->period . '-01-01',
            'start_to'                  => $request->period . '-12-31',
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;

                $result=[];
                foreach ($resVal as $fields) {
                    if ($fields->tsCode=="" && SUBSTR($fields->lastStatus, 0, 9)<>"Cancelled"){
                        $arr = [];
                        $arr[] = $fields->refId;

                        $result[] = $arr;
                    }
                }

                $filename = 'PromoApproval-';
                $title = 'A1:A1'; //Report Title Bold and merge
                $header = 'A1:A1'; //Header Column Bold and color
                $heading = [
                    ['Promo Plan']
                ];

                $formatCell =  [

                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d H:i:s') . ' ' .$mc . '.xlsx');
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error' => $response->status(),
                    'data' => [],
                    'message' => $message
                );
                Log::info('get API ' . $api);
                Log::warning($message);
                return $result;
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function uploadApprove(Request $request)
    {;
        $api = config('app.api') . '/promo/planningapproval';
        Log::info('post API ' . $api);
        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);
                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'message'   => "Upload success",
                    ]);
                } else if ($response->status() === 403) {
                    return json_encode([
                        'error'     => true,
                        'message'   => json_decode($response)->message,
                    ]);
                } else {
                    return array(
                        'error' => true,
                        'message' => "Upload failed"
                    );
                }
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }
}
