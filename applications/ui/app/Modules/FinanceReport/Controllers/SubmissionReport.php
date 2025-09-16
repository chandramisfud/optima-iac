<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use App\Exports\FinanceReport\ExportTemplatePromoSubmission;
use GuzzleHttp\Client;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class SubmissionReport extends Controller
{
    private $client;
    private $headers;
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
        $this->client = new Client([
            'base_uri' => config('app.api')
        ]);
        $this->headers = [
            'Content-Type'  => 'application/json',
        ];
    }

    public function landingPage()
    {
        $title = "Promo Submission report";
        return view('FinanceReport::submission-report.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission';
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
        $api = config('app.api'). '/finance-report/promosubmission/entity';
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
        $api = config('app.api'). '/finance-report/promosubmission/distributor';
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/channel';
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

    public function getDataLatePromo(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/email';
        $query = [
            'period'            => $request->period,
            'entity'            => $request->entity,
            'distributor'       => $request->distributor,
            'userid'            => 0,

        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->submissionlist2
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

    public function getDataException(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/exception';
        $query = [
            "idx"       => $request->session()->get('profile'),
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

    public function getListUserGroup(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/usergroup';
        $query = [
            'usergroupid'            => $request->usergroupid,

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

    public function getDatauserList(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/userlist';
        $query = [
            'usergroupid'       => $request->usergroupid,
            'userlevel'         => $request->userlevel,
            'isdeleted'         => $request->status
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

    public function uploadXls(Request $request) {
        $data_multipart = [];
        array_push($data_multipart, [
            'name'     => 'formFile',
            'contents' => $request->file('file')->getContent(),
            'filename' => $request->file('file')->getClientOriginalName(),
        ]);

        try {
            $response = $this->client->request('POST', '/api/finance-report/promosubmission/exception/upload?idx=' . $request->session()->get('profile'),
                [
                    'headers' => [
                        'Authorization' => 'Bearer ' . $this->token
                    ],
                    'multipart' => $data_multipart,
                ]);
            if ($response->getStatusCode() === 200) {
                return array(
                    'error' => false,
                    'message' => "upload success"
                );
            } else {
                return array(
                    'error' => true,
                    'message' => "Upload failed"
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/finance-report/promosubmission';
        $query = [
            'Period'                    => $request->period,
            'profileId'                 => 'admin',
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => $request->channelId,
            'CreateFrom'                => $request->startFrom,
            'CreateTo'                  => $request->startTo,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
//            'SortColumn'                => 'promoNumber',
//            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];

                $strEntitiy = "";
                foreach ($resVal as $fields) {
                    $arr = [];

                    $strEntitiy = $fields->entity;
                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    // $arr[] = $fields->principalDesc;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('Y-m-d' , strtotime($fields->lastUpdate));

                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;

                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subactivityType;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;

                    $arr[] = $fields->mechanisme1;
                    $arr[] = $fields->mechanisme2;
                    $arr[] = $fields->mechanisme3;
                    $arr[] = $fields->mechanisme4;

                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = date('Y-m-d' , strtotime($fields->lastStatusDate));

                    if($fields->sendback_notes==''){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->sendback_notes . ' on ' . $fields->sendback_notes_date;
                    }
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->target;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    if($fields->investmentTypeRefId_promo=='----' || $fields->investmentTypeRefId_promo=='' || $fields->investmentTypeRefId_promo==null)
                    {
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo;
                    }

                    $arr[] = $fields->roi;
                    $arr[] = $fields->costRatio / 100;
                    $arr[] = $fields->remainingBalance;

                    $arr[] = $fields->gap;
                    if($fields->onTime){
                        $arr[] = 'On-Time';
                    }else{
                        $arr[] = 'Late';
                    }

                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    if($fields->closureStatus){
                        $arr[] = 'Closed';
                    }else{
                        $arr[] = 'Open';
                    }
                    $arr[] = $fields->reconStatus;
                    if($fields->lastReconStatus==null){
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->lastReconStatus;
                    }
                    $arr[] = $fields->cancelReason;
                    $arr[] = $fields->actual_sales;
                    $arr[] = $fields->initiator_notes;
                    $arr[] = $fields->reason;

                    $result[] = $arr;
                }

                $entity = $request->entity;
                if($entity == "0"){
                    $entity = 'All';
                }else{
                    $entity = $strEntitiy;
                }
                $title = 'A1:AY1'; //Report Title Bold and merge
                $header = 'A6:AY6'; //Header Column Bold and color
                $heading = [
                    ['Promo Submission Report'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $entity], ['Date Retrieved : ' . date('Y-m-d')],
                    ['Submission Days : 30'],
                    ['Promo Plan ID','TS Code', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Brand', 'SKU',
                        'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism 1', 'Mechanism 2', 'Mechanism 3',
                        'Mechanism 4', 'Promo Start', 'Promo End', 'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio',
                        'Remaining Balance', 'Submission Configuration','Submission Status', 'DN Claim', 'DN Paid', 'Closure Status', 'Recon Status', 'Last Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes', 'Exception Reason']
                    ];
                $formatCell =  [
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_TEXT,
                    'AL' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AM' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AN' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AW' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                ];
                $filename = 'PromoSubmissionReport-';
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

    public function exportXlsAttachment(Request $request, $year, $entity, $distributor, $channel) {
        $api = config('app.api'). '/finance-report/promosubmission/download';

        $query = [
            'Period'                    => $year,
            'profileId'                 => 'admin',
            'EntityId'                  => $entity,
            'DistributorId'             => $distributor,
            'BudgetParentId'            => 0,
            'ChannelId'                 => $channel,
            'CreateFrom'                => $request->startFrom,
            'CreateTo'                  => $request->startTo,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
//            'SortColumn'                => 'promoNumber',
//            'SortDirection'             => 'asc'
        ];
        Log::info('payload ' . json_encode($api));
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];

                $strEntitiy = "";
                foreach ($resVal as $fields) {
                    $arr = [];

                    $strEntitiy = $fields->entity;
                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    // $arr[] = $fields->principalDesc;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('Y-m-d' , strtotime($fields->lastUpdate));

                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;

                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subactivityType;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;

                    $arr[] = $fields->mechanisme1;
                    $arr[] = $fields->mechanisme2;
                    $arr[] = $fields->mechanisme3;
                    $arr[] = $fields->mechanisme4;

                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = date('Y-m-d' , strtotime($fields->lastStatusDate));

                    if($fields->sendback_notes==''){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->sendback_notes . ' on ' . $fields->sendback_notes_date;
                    }
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->target;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    if($fields->investmentTypeRefId_promo=='----' || $fields->investmentTypeRefId_promo=='' || $fields->investmentTypeRefId_promo==null)
                    {
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo;
                    }

                    $arr[] = $fields->roi;
                    $arr[] = $fields->costRatio / 100;
                    $arr[] = $fields->remainingBalance;

                    $arr[] = $fields->gap;
                    if($fields->onTime){
                        $arr[] = 'On-Time';
                    }else{
                        $arr[] = 'Late';
                    }

                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    if($fields->closureStatus){
                        $arr[] = 'Closed';
                    }else{
                        $arr[] = 'Open';
                    }
                    $arr[] = $fields->reconStatus;
                    if($fields->lastReconStatus==null){
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->lastReconStatus;
                    }
                    $arr[] = $fields->cancelReason;
                    $arr[] = $fields->actual_sales;
                    $arr[] = $fields->initiator_notes;
                    $arr[] = $fields->reason;

                    $result[] = $arr;
                }

                $entity = $request->entity;
                if($entity == "0"){
                    $entity = 'All';
                }else{
                    $entity = $strEntitiy;
                }
                $title = 'A1:AY1'; //Report Title Bold and merge
                $header = 'A6:AY6'; //Header Column Bold and color
                $heading = [
                    ['Promo Submission Report'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $entity], ['Date Retrieved : ' . date('Y-m-d')],
                    ['Submission Days : 30'],
                    ['Promo Plan ID','TS Code', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Brand', 'SKU',
                        'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism 1', 'Mechanism 2', 'Mechanism 3',
                        'Mechanism 4', 'Promo Start', 'Promo End', 'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio',
                        'Remaining Balance', 'Submission Configuration','Submission Status', 'DN Claim', 'DN Paid', 'Closure Status', 'Recon Status', 'Last Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes', 'Exception Reason']
                ];
                $formatCell =  [
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_TEXT,
                    'AL' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AM' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AN' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AW' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,

                ];
                $filename = 'PromoSubmissionReport-';
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

    public function downloadTemplate(Request $request) {
        try {
            $result=[
                [
                    'NIS00588/AL20-01230/TS',
                    'Test Upload file Exception 1'
                ],
                [
                    'SGM01153/AL20-01650/TS',
                    'Test Upload file Exception 2'
                ],
                [

                    'SGM01161/AL20-01650/TS',
                    'Test Upload file Exception 3'
                ],
                [
                    'NIS00594/AL20-01230/TS',
                    'Test Upload file Exception 4'
                ],
                [
                    'NIS01363/AL20-01229/TS',
                    'Test Upload file Exception 5'
                ]
            ];
            $header = 'A1:B1'; //Header Column Bold and color
            $heading = [
                ['promorefid', 'reason']
            ];
            $filename = 'Template_Promo_Exception';
            $export = new ExportTemplatePromoSubmission($result, $heading, $header);
            return Excel::download($export, $filename . '.xlsx');

        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return [];
        }
    }

    public function sendEmail(Request $request) {
        $apiGetDataPromo = config('app.api') . '/finance-report/promosubmission/email';

        $year = $request->period;
        $entity = $request->entity;
        $distributor = $request->distributor;
        $channel = $request->channel;
        $emailSend = $request->emailSend;

        $data = [
            'period'        => $year,
            'entity'        => $entity,
            'distributor'   => $distributor,
            'channel'       => $channel,
            'userid'        => $request->session()->get('profile'),
        ];
        Log::info('post API ' . $apiGetDataPromo);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->get($apiGetDataPromo, $data);
            if ($response->status() === 200) {
                $submissionlist1 = json_decode($response)->values->submissionlist1;
                $submissionlist2 = json_decode($response)->values->submissionlist2;

                //submission list 1
                if ($submissionlist1) {
                    $month = array();
                    foreach ($submissionlist1 as $months) {
                        $month[] .= $months->monthName;
                    }

                    $chartontimepct = array();
                    for ($i = 0; $i < count($submissionlist1); $i++) {
                        if ($submissionlist1[$i]->onTimePCT == 0) {
                            $submissionlist1[$i]->onTimePCT = null;
                        }
                        $chartontimepct[] = $submissionlist1[$i]->onTimePCT;
                    }

                    $chartlatepct = array();
                    for ($i = 0; $i < count($submissionlist1); $i++) {
                        if ($submissionlist1[$i]->latePCT == 0) {
                            $submissionlist1[$i]->latePCT = null;
                        }
                        $chartlatepct[] = $submissionlist1[$i]->latePCT;
                    }
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => 'Send Mail Failed'
                    );
                }

                //submission list 2
                if ($submissionlist2) {
                    $totaloptima = 0;
                    $ontime = 0;
                    $sontimepct = 0;
                    $late = 0;
                    $slatepct = 0;

                    foreach($submissionlist2 as $value){

                        $totaloptima += $value->totOptimaCreated;
                        $ontime += $value->onTime;
                        $sontimepct += $value->onTimePCT;
                        $late += $value->late;
                        $slatepct += $value->latePCT;

                        $ontimepct = ($ontime/$totaloptima)*100;
                        $latepct = ($late/$totaloptima)*100;
                    }
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => 'Send Mail Failed'
                    );
                }

                $apiLatePromo = config('app.api') . '/finance-report/promosubmission/latepromo';

                $responseLatePromo =  Http::timeout(180)->withToken($this->token)->get($apiLatePromo);
                if ($responseLatePromo->status() === 200) {
                    $dataLatePromo = json_decode($responseLatePromo)->values;

                    $message = view('FinanceReport::submission-report.email', compact('submissionlist1','submissionlist2','month', 'totaloptima', 'ontime', 'ontimepct', 'late','latepct','dataLatePromo','chartontimepct','chartlatepct','year', 'entity', 'distributor', 'channel'))->render();
                    $emails = json_decode(stripslashes($emailSend));
                    foreach($emails as $mail) {
                        $data_multipart = [];
                        array_push($data_multipart, [
                            'name'  => 'email',
                            'contents' => $mail
                        ], [
                            'name'  => 'subject',
                            'contents' => "Promo Late Submission Report"
                        ], [
                            'name'  => 'body',
                            'contents' => $message
                        ], [
                            'name'  => 'cc',
                            'contents' => $mail
                        ], [
                            'name'  => 'bcc',
                            'contents' => $mail
                        ]);
                        $result_email = $this->client->request('POST', '/api/tools/email',
                            [
                                'headers' => [
                                    'Authorization' => 'Bearer ' . $this->token
                                ],
                                'http_errors' => false,
                                'multipart' => $data_multipart
                            ]);
                        if ($result_email->getStatusCode() === 200) {
                            return array(
                                'error'     => false,
                                'message'   => "Send Mail Success"
                            );
                        } else {
                            return array(
                                'error'     => true,
                                'message'   => "Send Mail Failed"
                            );
                        }
                    };
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => 'Send Mail Failed'
                    );
                }

            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $apiGetDataPromo);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $apiGetDataPromo);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "error : Send Email Failed"
            );
        }
    }
}
