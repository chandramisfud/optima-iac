<?php

namespace App\Modules\FinanceReport\Controllers;

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

class ListingPromoReconciliation extends Controller
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
        $title = "Listing Promo Reconciliation Reporting";
        return view('FinanceReport::listing-promo-reconciliation.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromorecon';
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
                'profileId'                 => json_decode($request->profileId),
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request->channelId,
                'CreateFrom'                => $request->startFrom,
                'CreateTo'                  => $request->startTo,
                'StartFrom'                 => $request->startFrom,
                'StartTo'                   => $request->startTo,
                'SubmissionParam'           => 0,
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
        $api = config('app.api'). '/finance-report/listingpromorecon/entity';
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
        $api = config('app.api'). '/finance-report/listingpromorecon/distributor';
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

    public function getDataPromoCreator(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromorecon/usergroups';
        $query = [
            'id'  => ['104', '106']
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
        $api = config('app.api'). '/finance-report/listingpromorecon/channel';
        $query = [
            'userid'  => 0,
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
        $api = config('app.api'). '/finance-report/listingpromorecon';
        $query = [
            'Period'                    => $request->period,
            'profileId'                 => 'ALL',
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => 0,
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

                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->approvalNotes;
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    $arr[] = (($fields->investmentTypeRefId_promo=='----' || $fields->investmentTypeRefId_promo=='' || $fields->investmentTypeRefId_promo==null) ?
                        '----' : $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo
                    );
                    $arr[] = $fields->roi;
                    $arr[] = $fields->costRatio;
                    $arr[] = $fields->remainingBalance;
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = "";
                    $arr[] = $fields->promoPlanRefId_after;
                    $arr[] = $fields->tsCoding_after;
                    $arr[] = $fields->promoNumber_after;
                    $arr[] = $fields->entity_after;
                    $arr[] = $fields->distributor_after;
                    $arr[] = $fields->budgetSource_after;
                    $arr[] = $fields->initiator_after;
                    $arr[] = $fields->regionDesc_after;
                    $arr[] = $fields->channelDesc_after;
                    $arr[] = $fields->subChannelDesc_after;
                    $arr[] = $fields->accountDesc_after;
                    $arr[] = $fields->subAccountDesc_after;
                    $arr[] = $fields->brandDesc_after;
                    $arr[] = $fields->skuDesc_after;
                    $arr[] = $fields->subCategory_after;
                    $arr[] = $fields->activity_after;
                    $arr[] = $fields->subActivity_after;
                    $arr[] = $fields->activityDesc_after;
                    $arr[] = $fields->mechanisme1_after;
                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo_after));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo_after));
                    $arr[] = date('Y-m-d' , strtotime($fields->createOn_after));
                    $arr[] = (($fields->lastStatus_after==null) ? '----' : $fields->lastStatus_after);
                    $arr[] = $fields->approvalNotes_after;
                    $arr[] = $fields->normalSales_after;
                    $arr[] = $fields->incrSales_after;
                    $arr[] = $fields->investment_after;
                    $arr[] = $fields->roi_after;
                    $arr[] = $fields->costRatio_after;
                    $arr[] = $fields->remainingBalance_after;
                    $arr[] = $fields->dnClaim_after;
                    $arr[] = $fields->dnPaid_after;
                    $arr[] = $fields->investmentBfrClose;
                    $arr[] = $fields->investmentClosedBalance;
                    $arr[] = $fields->actual_sales;

                    $result[] = $arr;
                }
                $title = 'A1:BR1'; //Report Title Bold and merge
                $header = 'A5:BR5'; //Header Column Bold and color
                $heading = [
                    ['Listing Promo Reconciliation Reporting'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'Planning ID', 'TS Code', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator','Region', 'Channel', 'Sub Channel',
                        'Account', 'Sub Account', 'Brand', 'SKU', 'Sub Category', 'Activity', 'Sub Activity', 'Activity Name', 'Promo Start', 'Promo End', 'Creation Date', 'Recon Status', 'Approval Notes', 'Baseline Sales',
                        'Incr Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio', 'Remaining Balance', 'DN Claim', 'DN Paid', "",

                        'Planning ID', 'TS Code', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator','Region', 'Channel', 'Sub Channel',
                        'Account', 'Sub Account', 'Brand', 'SKU', 'Sub Category', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Last Recon Status', 'Approval Notes', 'Baseline Sales',
                        'Incr Sales', 'Investment', 'ROI', 'Cost Ratio', 'Remaining Balance', 'DN Claim', 'DN Paid','Investment Before Closure','Closed Balance', 'Actual Sales'//, 'Closure Status',
                    ]
                ];
                $formatCell =  [
                    'X' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Y' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Z' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AD' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AE' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                    'BI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BN' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BK' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BL' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BP' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                ];
                $filename = 'PromoRecon-';
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
