<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use App\Exports\Report\ExportViewInvestment;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class PromoApprovalAging extends Controller
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
        $title = "Promo Approval Aging Report";
        return view('FinanceReport::promo-approval-aging.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/promoapprovalaging';
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
                'profileId'                 => 'admin',
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
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
        $api = config('app.api'). '/finance-report/promoapprovalaging/entity';
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
        $api = config('app.api'). '/finance-report/promoapprovalaging/distributor';
        $query = [
            'budgetId'  => 0,
            'entityId'  => $request->entityId,
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
        $api = config('app.api'). '/finance-report/promoapprovalaging';
        $query = [
            'Period'                    => $request->period,
            'profileId'                 => 'admin',
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
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

                    $arr[] = $fields->initiatorName;
                    $arr[] = $fields->promoNo;
                    $arr[] = $fields->channelDesc;
                    if($fields->promoNo=='Subtotal'){
                        $arr[] = "";
                    }else{
                        $arr[] = date('Y-m-d' , strtotime($fields->entryDate));
                    }
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->promoPeriode;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->approver1;
                    $arr[] = $fields->approver2;
                    $arr[] = $fields->approver3;
                    $arr[] = $fields->approver4;
                    $arr[] = $fields->approver5;
                    $arr[] = $fields->aging;
                    $arr[] = $fields->approver;

                    $result[] = $arr;
                }

                $filename = 'Promo-Aging-Approval-';
                $title = 'A1:S1'; //Report Title Bold and merge
                $header = 'A4:S4'; //Header Column Bold and color
                $heading = [
                    ['Promo Approval Aging Report'],
                    ['Budget Year : ' . $request->period],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'Promo Initiator', 'Promo No', 'Channel', 'Entry Date', 'Activity', 'Sub Activity', 'Promo Period',
                        'Activity Description', 'Mechanism', 'Promo Value Cost', 'Last Status',
                        'Initiator', 'Approve 1', 'Approver 2', 'Approver 3', 'Approver 4', 'Approver 5', 'Aging',
                        'Approver Name'
                    ]
                ];

                $formatCell =  [
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Q' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'R' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'S' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'T' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

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
