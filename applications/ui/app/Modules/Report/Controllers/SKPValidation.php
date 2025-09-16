<?php

namespace App\Modules\Report\Controllers;

use App\Exports\Report\ExportSKPValidation;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class SKPValidation extends Controller
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
        $title = "SKP Validation Report";
        return view('Report::skp-validation.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/report/skpvalidation';
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
                'DistributorId'             => $request->distributorId,
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request->channelId,
                'CancelStatus'              => 0,
                'StartFrom'                 => $request->startFrom,
                'StartTo'                   => $request->startTo,
                'SubmissionParam'           => 0,
                'Status'                    => 0,
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
        $api = config('app.api'). '/report/skpvalidation/entity';
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
        $api = config('app.api'). '/report/skpvalidation/distributor';
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
        $api = config('app.api'). '/report/skpvalidation/channel';
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
        $api = config('app.api'). '/report/skpvalidation';
        $query = [
            'Period'                    => $request->period,
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => $request->channelId,
            'CancelStatus'              => 0,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Status'                    => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'refId',
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

                    $arr[] = $fields->skpStatus;
                    $arr[] = $fields->refId;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = date('d-m-Y', strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = date('d-m-Y', strtotime($fields->lastStatusDate));

                    $arr[] = $fields->skpDraftAvail;
                    $arr[] = $fields->skpDraftAvailBfrAct60;
                    $arr[] = $fields->skpEntityDraft;
                    $arr[] = $fields->skpBrandDraft;
                    $arr[] = $fields->skpPeriodDraft;
                    $arr[] = $fields->skpActivityDescDraft;
                    $arr[] = $fields->skpMechanismDraft;
                    $arr[] = $fields->skpInvestmentDraft;
                    $arr[] = $fields->skpDistributorDraft;
                    $arr[] = $fields->skpChannelDraft;
                    $arr[] = $fields->skpStoreNameDraft;

                    $arr[] = $fields->skpSign7;
                    $arr[] = $fields->skpEntity;
                    $arr[] = $fields->skpBrand;
                    $arr[] = $fields->skpPeriodMatch;
                    $arr[] = $fields->skpActivityDesc;
                    $arr[] = $fields->skpMechanismMatch;
                    $arr[] = $fields->skpInvestmentMatch;
                    $arr[] = $fields->skpDistributor;
                    $arr[] = $fields->skpChannel;
                    $arr[] = $fields->skpStoreName;
                    if ($fields->storeNameon == '0001-01-01T00:00:00') {
                        $arr[] = '';
                    } else {
                        $arr[] = date('d-m-Y H:m:s', strtotime($fields->storeNameon));
                    }

                    $arr[] = $fields->storeNameby;
                    $arr[] = $fields->skP_Notes;

                    $result[] = $arr;
                }
                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A7:AL7'; //Header Column Bold and color
                $merge1 = 'A5:N6';
                $merge2 = 'O5:AI5';
                $merge3 = 'O6:Y6';
                $merge4 = 'Z6:AI6';
                $merge5 = 'O7:AI7';
                $heading = [
                    ['SKP Validation Report'],
                    ['Period : ' . $request->period],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [''],
                    ['PROMO','','','','','','','','','','','','','','SKP'],
                    ['','','','','','','','','','','','','','','DRAFT','','','','','','','','','','','FINAL'],
                    [
                        'SKP Status',
                        'Promo ID',
                        'Investment',
                        'Distributor',
                        'Initiator',
                        'Channel',
                        'Sub Account',
                        'Activity Name',
                        'Mechanism',
                        'Promo Start',
                        'Promo End',
                        'Creation Date',
                        'Status',
                        'Status Date',
                        'SKP Draft Availability',
                        'SKP Draft Availability Before Activity Start H-30',
                        'Entity',
                        'SKU',
                        'Period',
                        'Activity Desc',
                        'Mechanism',
                        'Investment',
                        'Distributor',
                        'Channel',
                        'StoreName',
                        'SKP Signed H-7',
                        'Entity',
                        'SKU',
                        'Period',
                        'Activity Desc',
                        'Mechanism',
                        'Investment',
                        'Distributor',
                        'Channel',
                        'Store Name',
                        'Validate on',
                        'Validate by',
                        'Remarks'
                    ]
                ];
                $formatCell =  [

                    'C' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'SKP Validation Report-';
                $export = new ExportSKPValidation($result, $heading, $title, $header, $merge1, $merge2, $merge3, $merge4, $merge5, $formatCell);
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
