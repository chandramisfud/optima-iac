<?php

namespace App\Modules\Report\Controllers;

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

class Accrual extends Controller
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
        $title = "Accrual Report";
        return view('Report::accrual.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/report/accrual';
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
                'EntityId'                  => $request->entityId,
                'DistributorId'             => 0,
                'BudgetParentId'            => 0,
                'ClosingDate'               => $request->closingDate,
                'Download'                  => 0,
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
        $api = config('app.api'). '/report/accrual/entity';
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/report/accrual';
        $query = [
            'Period'                    => $request->period,
            'EntityId'                  => (($request->entityId) ?? 0),
            'DistributorId'             => 0,
            'BudgetParentId'            => 0,
            'ClosingDate'               => $request->closingDate,
            'Download'                  => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'createOn',
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
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('Y-m-d' , strtotime($fields->createOn));
                    $arr[] = date('Y-m-d' , strtotime($fields->lastUpdate));
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = $fields->mechanisme2;
                    $arr[] = $fields->mechanisme3;
                    $arr[] = $fields->mechanisme4;
                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo));
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->approvalNotes;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->accrueYTD;

                    $result[] = $arr;
                }

                $filename = 'Accrual Report';
                $title = 'A1:Z1'; //Report Title Bold and merge
                $header = 'A5:Z5'; //Header Column Bold and color
                $heading = [
                    ['Accrual Report'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Budget Source', 'Initiator', 'Created Data', 'Last Update', 'Promo Number', 'Channel', 'Account', 'Sub Account',
                        'Sub Category', 'Activity', 'Sub Activity', 'Activity Description', 'Mechanism 1', 'Mechanism 2',
                        'Mechanism 3', 'Mechanism 4', 'Start Promo', 'End Promo', 'Last Status', 'Approval Notes',
                        'Investment', 'DN Claim', 'DN Paid', 'Accrual YTD']
                    ];

                $formatCell =  [
                    'W' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'X' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Y' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Z' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AA' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AB' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . '.xlsx');
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
