<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use Illuminate\Support\Carbon;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class PromoPlanningRepoting extends Controller
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
        $title = "Promo Planning Reporting";
        return view('FinanceReport::promo-planning-reporting.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/promoplanningreporting';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $page = $page;
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Period'                    => $request->period,
                'profileId'                 => '0',
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'ChannelId'                 => $request->channelId,
                'CreateFrom'                => $request->startFrom,
                'CreateTo'                  => $request->startTo,
                'StartFrom'                 => $request->startFrom,
                'StartTo'                   => $request->startTo,
                'Search'                    => $search,
                'PageNumber'                => $page,
                'PageSize'                  => (int) $length,
                'SortColumn'                => $SortColumn,
                'SortDirection'             => $sort
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "draw" => (int) $request->draw,
                    "data" => json_decode($response)->values->data,
                    "recordsTotal" => json_decode($response)->values->totalCount,
                    "recordsFiltered" => json_decode($response)->values->filteredCount
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
                Log::warning($message);
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/finance-report/promoplanningreporting/entity';
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

    public function getListDistributorByEntityId(Request $request)
    {
        $api = config('app.api'). '/finance-report/promoplanningreporting/distributor';
        $query = [
            'EntityId'           => $request->entityId,
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api'). '/finance-report/promoplanningreporting/channel';
        $query = [
            'userid'           => 0,
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/finance-report/promoplanningreporting';
        $query = [
            'Period'                    => $request->period,
            'profileId'                 => '0',
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'ChannelId'                 => $request->channelId,
            'CreateFrom'                => $request->startFrom,
            'CreateTo'                  => $request->startTo,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'promoNumber',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->periode;
                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCode;

                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = ($fields->groupBrandDesc ?? "");
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;

                    $mechanism = "";
                    if($fields->mechanisme1 != "" && $fields->mechanisme1 != null ) $mechanism = $mechanism . $fields->mechanisme1;
                    if($fields->mechanisme2 != "" && $fields->mechanisme2 != null) $mechanism = $mechanism . ', ' . $fields->mechanisme2;
                    if($fields->mechanisme3 != "" && $fields->mechanisme3 != null) $mechanism = $mechanism . ', ' . $fields->mechanisme3;
                    if($fields->mechanisme4 != "" && $fields->mechanisme4 != null) $mechanism = $mechanism . ', ' . $fields->mechanisme4;
                    $arr[] = $mechanism;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    $arr[] = (($fields->investmentTypeRefId_promo ==='----' || $fields->investmentTypeRefId_promo ==='' || $fields->investmentTypeRefId_promo === null) ? '----' : $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo);

                    $arr[] = $fields->roi;
                    $arr[] = $fields->costRatio / 100;
                    $arr[] = $fields->totalSales;
                    $arr[] = $fields->initiator;
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->cancelNotes;
                    $dt = date('d/m/Y' , strtotime($fields->tsCodeOn));
                    $TSCodeOn = Carbon::createFromFormat('d/m/Y', $dt);
                    if($fields->tsCodeOn=='0001-01-01T00:00:00'){ $arr[] = ""; }else{ $arr[] = $TSCodeOn; }
                    $arr[] = $fields->tsCodeBy;

                    $result[] = $arr;
                }
                $title = 'A1:AJ1'; //Report Title Bold and merge
                $header = 'A6:AJ6'; //Header Column Bold and color
                $heading = [
                    ['Promo Planning'],
                    ['Period : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Distributor : ' . $request->distributor],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'Period', 'Planning ID', 'TSCode', 'Entity', 'Distributor', 'Sub Category', 'Activity', 'Sub Activity', 'Activity Name',
                        'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Brand', 'Sub Brand', 'SKU', 'Mechanism',
                        'Promo Start', 'Promo End', 'Baseline Sales', 'Sales Increment', 'Investment','Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio', 'Total Sales', 'Initiator', 'Create On',
                        'Last Status', 'Promo ID', 'Cancel Notes', 'TSCode On', 'TSCode By'
                    ]
                ];
                $formatCell =  [
                    'S' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'T' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'U' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'V' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'W' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'X' => NumberFormat::FORMAT_TEXT,
                    'AA' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AB' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AC' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AE' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                ];
                $filename = 'PromoPlanningReporting-';
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
