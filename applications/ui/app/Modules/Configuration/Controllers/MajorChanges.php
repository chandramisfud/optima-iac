<?php

namespace App\Modules\Configuration\Controllers;

use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class MajorChanges extends Controller
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
        $title = "Major Changes";
        return view('Configuration::major-changes.index', compact('title'));
    }

    public function getDataConfig(Request $request)
    {
        $api = config('app.api'). '/config/majorchanges';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data'  => $resVal
                ]);
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

    public function getDataConfigDC(Request $request)
    {
        $api = config('app.api'). '/config/majorchangesdc';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data'  => $resVal
                ]);
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

    public function submit(Request $request)
    {
        $api = config('app.api') . '/config/majorchanges';
        $data = [
            'id'                => 1,
            'budgetSources'     => (bool)$request->budgetSources,
            'activity'          => (bool)$request->activity,
            'subActivity'       => (bool)$request->subActivity,
            'startPromo'        => (bool)$request->startPromo,
            'endPromo'          => (bool)$request->endPromo,
            'activityDesc'      => (bool)$request->activityDesc,
            'initiatorNotes'    => (bool)$request->initiatorNotes,
            'incrSales'         => (bool)$request->incrSales,
            'investment'        => (bool)$request->investment,
            'roi'               => (bool)$request->roi,
            'cr'                => (bool)$request->cr,
            'channel'           => (bool)$request->channel,
            'subChannel'        => (bool)$request->subChannel,
            'account'           => (bool)$request->account,
            'subAccount'        => (bool)$request->subAccount,
            'region'            => (bool)$request->region,
            'brand'             => (bool)$request->brand,
            'sku'               => (bool)$request->sku,
            'mechanism'         => (bool)$request->mechanism,
            'attachment'        => (bool)$request->attachment,
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Save success",
                ]);
            } else {
                $message = json_decode($response)->message;
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
                'message'   => "error : Save Failed"
            );
        }
    }

    public function submitDC(Request $request)
    {
        $api = config('app.api') . '/config/majorchangesdc';
        $data = [
            'id'                => 1,
            'subCategory'       => (bool)$request['subCategory'],
            'year'              => (bool)($request['year'] ?? false),
            'entity'            => (bool)$request['entity'],
            'distributor'       => (bool)$request['distributor'],
            'activity'          => (bool)($request['year'] ?? false),
            'subActivity'       => (bool)$request['subActivity'],
            'startPromo'        => (bool)$request['startPromo'],
            'endPromo'          => (bool)$request['endPromo'],
            'activityDesc'      => (bool)($request['activityDesc'] ?? false),
            'initiatorNotes'    => (bool)$request['initiatorNotes'],
            'incrSales'         => (bool)($request['incrSales'] ?? false),
            'investment'        => (bool)$request['investment'],
            'roi'               => (bool)($request['roi'] ?? false),
            'cr'                => (bool)($request['cr'] ?? false),
            'channel'           => (bool)$request['channel'],
            'subChannel'        => (bool)($request['subChannel'] ?? false),
            'account'           => (bool)($request['account'] ?? false),
            'subAccount'        => (bool)($request['subAccount'] ?? false),
            'region'            => (bool)($request['region'] ?? false),
            'groupBrand'        => (bool)$request['groupBrand'],
            'brand'             => (bool)($request['brand'] ?? false),
            'sku'               => (bool)($request['sku'] ?? false),
            'mechanism'         => (bool)$request['mechanism'],
            'budgetSources'     => (bool)$request['budgetSources'],
            'promoPlan'         => (bool)($request['promoPlan'] ?? false),
            'attachment'        => (bool)($request['attachment'] ?? false),
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Save success",
                ]);
            } else {
                $message = json_decode($response)->message;
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
                'message'   => "error : Save Failed"
            );
        }
    }

    public function exportExcelHistory(Request $request) {
        $api = config('app.api'). '/config/majorchanges/history';
        $query = [
            'year'                    => $request['period'],
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
                    $arr[] = $fields->budgetSources;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->startPromo;
                    $arr[] = $fields->endPromo;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->initiatorNotes;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->roi;
                    $arr[] = $fields->cr;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->region;
                    $arr[] = $fields->brand;
                    $arr[] = $fields->sku;
                    $arr[] = $fields->mechanism;
                    $arr[] = $fields->attachment;
                    $arr[] = date('Y-m-d' , strtotime($fields->modifiedOn));
                    $arr[] = $fields->modifiedBy;
                    $arr[] = $fields->modifiedEmail;


                    $result[] = $arr;
                }

                $filename = 'Major Changes Hist-';
                $title = 'A1:W1'; //Report Title Bold and merge
                $header = 'A3:W3'; //Header Column Bold and color
                $heading = [
                    ['Major Changes History'],
                    ['Year : ' . $request->period],
                    ['Budget Source', 'Activity', 'Sub Activity', 'Start Promo', 'End Promo', 'Activity Description', 'Initiator Notes', 'Increment Sales', 'Investment', 'ROI', 'Cost Ratio',
                        'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Region', 'Brand', 'SKU', 'Mechanism', 'Attachment', 'Modified On', 'Modified By', 'Modified Email']
                ];

                $formatCell =  [];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
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

    public function exportExcelHistoryDC(Request $request) {
        $api = config('app.api'). '/config/majorchanges/historydc';
        $query = [
            'year'                    => $request['period'],
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

                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->budgetSources;
                    $arr[] = $fields->startPromo;
                    $arr[] = $fields->endPromo;
                    $arr[] = $fields->mechanism;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->initiatorNotes;
                    $arr[] = $fields->attachment;
                    $arr[] = date('Y-m-d' , strtotime($fields->modifiedOn));
                    $arr[] = $fields->modifiedBy;
                    $arr[] = $fields->modifiedEmail;


                    $result[] = $arr;
                }

                $filename = 'DC Major Changes History-';
                $title = 'A1:O1'; //Report Title Bold and merge
                $header = 'A3:O3'; //Header Column Bold and color
                $heading = [
                    ['Major Changes History (Distributor Cost)'],
                    ['Year : ' . $request['period']],
                    ['Brand', 'Distributor', 'Sub Activity Type', 'Sub Activity', 'Channel', 'Budget Source', 'Start Promo', 'End Promo',
                        'Mechanism', 'Investment', 'Initiator Notes', 'Attachment', 'Modified On', 'Modified By', 'Modified Email']
                ];

                $formatCell =  [];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
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
