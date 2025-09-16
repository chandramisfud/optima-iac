<?php

namespace App\Modules\Tools\Controllers;

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

class BlitzRawData extends Controller
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
        $title = "BLITZ Raw Data";
        return view('Tools::blitz-rawdata.index', compact('title'));
    }

    public function getDataRaw(Request $request)
    {
        $api = config('app.api'). '/tools/baseline' ;

        $query = [
            'refid'         => ($request->refid ?? "null"),
            'promoplan'     => ($request->promoplan ?? 0),
        ];

        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
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

    public function exportXlsActualSales(Request $request) {
        $api = config('app.api'). '/tools/baseline' ;

        $query = [
            'refid'         => ($request->refid ?? ""),
            'promoplan'     => ($request->promoplan ?? ""),
        ];

        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values[0]->rawActualSales;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->distributor_Id;
                    $arr[] = $fields->distributor_Name;
                    $arr[] = $fields->region_Code;
                    $arr[] = $fields->region_Desc;
                    $arr[] = $fields->year;
                    $arr[] = $fields->month_Name;
                    $arr[] = $fields->accountCode;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountCode;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->product_Code;
                    $arr[] = $fields->product_Name;
                    $arr[] = $fields->qty_In_Ton;
                    $arr[] = $fields->qty_In_Car;
                    $arr[] = $fields->sS_DBP;
                    $arr[] = $fields->sS_RBP;
                    $arr[] = $fields->source;
                    $arr[] = $fields->month_int;
                    $arr[] =  date("Y-m-d", strtotime($fields->created_at));
                    $arr[] =  date("Y-m-d", strtotime($fields->updated_at));
                    $arr[] = $fields->ym;

                    $result[] = $arr;
                }
                $title = 'A1:U1'; //Report Title Bold and merge
                $header = 'A3:U3'; //Header Column Bold and color
                $heading = [
                    ['Actual Sales Raw Data'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['distributor_Id',
                        'distributor_Name',
                        'region_Code',
                        'region_Desc',
                        'year',
                        'month_Name',
                        'accountCode',
                        'accountDesc',
                        'subAccountCode',
                        'subAccountDesc',
                        'product_Code',
                        'product_Name',
                        'qty_In_Ton',
                        'qty_In_Car',
                        'sS_DBP',
                        'sS_RBP',
                        'source',
                        'month_int',
                        'created_at',
                        'updated_at',
                        'ym']
                ];
                $formatCell =  [
                    'A' => NumberFormat::FORMAT_TEXT,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'Actual Sales Raw Data - ';
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

    public function exportXlsRawBaseline(Request $request) {
        $api = config('app.api'). '/tools/baseline' ;

        $query = [
            'refid'         => ($request->refid ?? ""),
            'promoplan'     => ($request->promoplan ?? ""),
        ];

        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values[0]->rawBaseLine;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->distributor_Id;
                    $arr[] = $fields->distributor_Name;
                    $arr[] = $fields->region_Code;
                    $arr[] = $fields->region_Desc;
                    $arr[] = $fields->year;
                    $arr[] = $fields->month_Name;
                    $arr[] = $fields->accountCode;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountCode;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->product_Code;
                    $arr[] = $fields->product_Name;
                    $arr[] = $fields->qty_In_Ton;
                    $arr[] = $fields->qty_In_Car;
                    $arr[] = $fields->sS_DBP;
                    $arr[] = $fields->sS_RBP;
                    $arr[] = $fields->source;
                    $arr[] = $fields->month_int;
                    $arr[] =  date("Y-m-d", strtotime($fields->created_at));
                    $arr[] =  date("Y-m-d", strtotime($fields->updated_at));
                    $arr[] = $fields->ym;

                    $result[] = $arr;
                }
                $title = 'A1:U1'; //Report Title Bold and merge
                $header = 'A3:U3'; //Header Column Bold and color
                $heading = [
                    ['Baseline Raw Data'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['distributor_Id',
                        'distributor_Name',
                        'region_Code',
                        'region_Desc',
                        'year',
                        'month_Name',
                        'accountCode',
                        'accountDesc',
                        'subAccountCode',
                        'subAccountDesc',
                        'product_Code',
                        'product_Name',
                        'qty_In_Ton',
                        'qty_In_Car',
                        'sS_DBP',
                        'sS_RBP',
                        'source',
                        'month_int',
                        'created_at',
                        'updated_at',
                        'ym']
                ];
                $formatCell =  [
                    'A' => NumberFormat::FORMAT_TEXT,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'Baseline Raw Data - ';
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
