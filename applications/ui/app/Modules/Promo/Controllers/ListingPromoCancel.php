<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Wording;


class ListingPromoCancel extends Controller
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
        $title = "Listing Promo Cancel";
        return view('Promo::listing-promo-cancel.index', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api') . '/promo/cancel';
        try {
            $query = [
                'year'          => $request->period,
                'entity'        => $request->entityId,
                'budgetparent'  => 0,
                'channel'       => 0,
                'distributor'   => $request->distributorId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "data" => json_decode($response)->values
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api') . '/promo/entity';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListDistributor(Request $request)
    {
        $api = config('app.api') . '/promo/distributor';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request->entityId);

            $query = [
                'budgetId' => 0,
                'entityId' => $ar_parent,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api') . '/promo/cancel';

        $query = [
            'year'          => $request->period,
            'entity'        => $request->entityId,
            'budgetparent'  => 0,
            'channel'       => 0,
            'distributor'   => $request->distributorId,
        ];

        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->refId;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->allocation;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->cancelNotes;

                    $result[] = $arr;
                }
                $title = 'A1:G1'; //Report Title Bold and merge
                $header = 'A6:G6'; //Header Column Bold and color
                $heading = [
                    ['Listing Promo Cancel'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Distributor : ' . $request->distributor],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo ID', 'Last Status', 'Activity Description', 'TS Coding', 'Allocation', 'Investment', 'Cancel Notes']
                ];
                $formatCell =  [
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'PromoCancel-';
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
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

}
