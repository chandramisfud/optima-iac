<?php

namespace App\Modules\Report\Controllers;

use App\Exports\Export;
use App\Helpers\Formatted;
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

class ListingPromoReporting extends Controller
{
    protected mixed $token;
    protected mixed $formatFunction;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');
            $this->formatFunction = new Formatted();

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "Listing Promo Reporting";
        return view('Report::listing-promo-reporting.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/report/listingpromoreporting';
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
                'CategoryId'                => $request->categoryId,
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

    public function getListCategory(Request $request)
    {
        $api = config('app.api'). '/report/listingpromoreporting/category';
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/report/listingpromoreporting/entity';
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
        $api = config('app.api'). '/report/listingpromoreporting/distributor';
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
        $api = config('app.api'). '/report/listingpromoreporting/channel';
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
        $api = config('app.api'). '/report/listingpromoreporting';
        $query = [
            'Period'                    => $request->period,
            'CategoryId'                => $request->categoryId,
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
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->groupBrandDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subactivityType;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = ($this->formatFunction->isNotDate($fields->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($fields->lastStatusDate)));
                    $arr[] = (($fields->sendback_notes=='') ? '' : $fields->sendback_notes . ' on ' . $fields->sendback_notes_date);
                    $arr[] = $fields->target;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    $arr[] = (($fields->investmentTypeRefId_promo=='----' || $fields->investmentTypeRefId_promo=='' || $fields->investmentTypeRefId_promo==null)
                        ? '----' : $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo);
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->investmentBfrClose;
                    $arr[] = $fields->investmentClosedBalance;
                    $arr[] = $fields->remainingBalance;
                    $arr[] = (($fields->closureStatus) ? 'Closed' : 'Open');
                    $arr[] = $fields->reconStatus;
                    $arr[] = ((!$fields->lastReconStatus) ? '----' : $fields->lastReconStatus);
                    $arr[] = $fields->cancelReason;
                    $arr[] = $fields->actual_sales;
                    $arr[] = $fields->initiator_notes;
                    $arr[] = $fields->budgetApprovalStatus;
                    $arr[] = ($fields->budgetApprovalStatusOn ? date('d-m-Y', strtotime($fields->budgetApprovalStatusOn)) : '');
                    $arr[] = $fields->budgetApprovalStatusBy;
                    $arr[] = $fields->budgetDeployStatus;
                    $arr[] = ($fields->budgetDeployStatusOn ? date('d-m-Y', strtotime($fields->budgetDeployStatusOn)) : '');
                    $arr[] = $fields->budgetDeployStatusBy;
                    $arr[] = $fields->batchId;
                    $arr[] = $fields->mainActivity;


                    $result[] = $arr;
                }
                $heading = [
                    ['Listing Promo Reporting'],
                    ['Budget Year : ' . $request->period],
                    ['Category : ' . $request->category],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo Plan ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Account', 'Sub Account', 'Brand', 'Sub Brand', 'SKU',
                        'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism',
                        'Promo Start', 'Promo End', 'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Target', 'Investment', 'Investment Code', 'Investment Type',
                        'Last Promo Investment Type', 'DN Claim', 'DN Paid','Investment Before Closure','Closed Balance', 'Remaining Balance', 'Closure Status', 'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                        'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval by', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By', 'Batch ID', 'Main Activity'
                    ]
                ];
                $formatCell =  [
                    'AC' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AK' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AL' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AY' => NumberFormat::FORMAT_TEXT
                ];
                $title = 'A1:AZ1'; //Report Title Bold and merge
                $header = 'A6:AZ6'; //Header Column Bold and color
                $filename = 'ListingPromoReporting-';
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

    public function exportCsv(Request $request) {
        $api = config('app.api'). '/report/listingpromoreporting';
        $query = [
            'Period'                    => $request->period,
            'CategoryId'                => $request->categoryId,
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
            'SortColumn'                => 'promoNumber',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;

                $mc = microtime(true);
                $fileName = 'ListingPromoReporting-' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columns = array(
                    'Promo Plan ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Account', 'Sub Account',
                    'Brand', 'Sub Brand', 'SKU', 'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Status',
                    'Status Date', 'Last Sendback Note', 'Target', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type',
                    'DN Claim', 'DN Paid','Investment Before Closure','Closed Balance', 'Remaining Balance', 'Closure Status', 'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                    'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval by', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By', 'Batch ID', 'Main Activity'
                );

                $period = $request['period'];
                $category = $request['category'];
                $entity = $request['entity'];

                $callback = function () use ($resVal, $columns, $period, $category, $entity) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["Listing Promo Reporting"], ",");
                    fputcsv($file, ['Budget Year : ' . $period], ",");
                    fputcsv($file, ['Category : ' . $category], ",");
                    fputcsv($file, ['Entity : ' . $entity], ",");
                    fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')], ",");
                    fputcsv($file, $columns, ",");

                    foreach ($resVal as $task) {
                        $row['Planning ID']                 = $task->promoPlanRefId;
                        $row['TS Code']                     = $task->tsCoding;
                        $row['Category']                    = $task->categoryLongDesc;
                        $row['Promo ID']                    = $task->promoNumber;
                        $row['Entity']                      = $task->entity;
                        $row['Distributor']                 = $task->distributor;
                        $row['Budget']                      = $task->budgetSource;
                        $row['Initiator']                   = $task->initiator;
                        $row['Last Update']                 = date('Y-m-d' , strtotime($task->lastUpdate));
                        $row['Region']                      = $task->regionDesc;
                        $row['Channel']                     = $task->channelDesc;
                        $row['Account']                     = $task->accountDesc;
                        $row['Sub Account']                 = $task->subAccountDesc;
                        $row['Brand']                       = $task->groupBrandDesc;
                        $row['Sub Brand']                   = $task->brandDesc;
                        $row['SKU']                         = $task->skuDesc;
                        $row['Sub Category']                = $task->subCategory;
                        $row['Sub Activity Type']           = $task->subactivityType;
                        $row['Activity']                    = $task->activity;
                        $row['Sub Activity']                = $task->subActivity;
                        $row['Activity Name']               = $task->activityDesc;
                        $row['Mechanism']                   = $task->mechanism;
                        $row['Promo Start']                 = date('Y-m-d' , strtotime($task->startPromo));
                        $row['Promo End']                   = date('Y-m-d' , strtotime($task->endPromo));
                        $row['Creation Date']               = date('Y-m-d' , strtotime($task->createOn));
                        $row['Status']                      = $task->lastStatus;
                        $row['Status Date']                 = ($this->formatFunction->isNotDate($task->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($task->lastStatusDate)));
                        $row['Last Sendback Note']          = ($task->sendback_notes === "" ? '' : $task->sendback_notes . ' on ' . $task->sendback_notes_date);
                        $row['Target']                      = $task->target;
                        $row['Investment']                  = $task->investment;
                        $row['Investment Code']             = $task->investmentTypeRefId;
                        $row['Investment Type']             = $task->investmentTypeDesc;
                        $row['Last Promo Investment Type']  = (($task->investmentTypeRefId_promo=='----' || $task->investmentTypeRefId_promo=='' || $task->investmentTypeRefId_promo==null) ? '----' : $task->investmentTypeRefId_promo. ' - ' . $task->investmentTypeDesc_promo);
                        $row['DN Claim']                    = $task->dnClaim;
                        $row['DN Paid']                     = $task->dnPaid;
                        $row['Investment Before Closure']   = $task->investmentBfrClose;
                        $row['Closed Balance']              = $task->investmentClosedBalance;
                        $row['Remaining Balance']           = $task->remainingBalance;
                        $row['Closure Status']              = ($task->closureStatus ? 'Closed' : 'Open');
                        $row['Recon Status']                = $task->reconStatus;
                        $row['Approval Recon Status']       = ($task->lastReconStatus ?? '---');
                        $row['Cancel Reason']               = $task->cancelReason;
                        $row['Actual Sales']                = $task->actual_sales;
                        $row['Initiator Notes']             = $task->initiator_notes;
                        $row['Budget Mass Approval Status'] = $task->budgetApprovalStatus;
                        $row['Budget Mass Approval Date']   = $task->budgetApprovalStatusOn;
                        $row['Budget Mass Approval By']     = $task->budgetApprovalStatusBy;
                        $row['Budget Deploy Status']        = $task->budgetDeployStatus;
                        $row['Budget Deploy Date']          = $task->budgetDeployStatusOn;
                        $row['Budget Deploy By']            = $task->budgetDeployStatusBy;
                        $row['Batch ID']                    = $task->batchId;
                        $row['Main Activity']               = $task->mainActivity;

                        fputcsv($file, array(
                            $row['Planning ID'],
                            $row['TS Code'],
                            $row['Category'],
                            $row['Promo ID'],
                            $row['Entity'],
                            $row['Distributor'],
                            $row['Budget'],
                            $row['Initiator'],
                            $row['Last Update'],
                            $row['Region'],
                            $row['Channel'],
                            $row['Account'],
                            $row['Sub Account'],
                            $row['Brand'],
                            $row['Sub Brand'],
                            $row['SKU'],
                            $row['Sub Category'],
                            $row['Sub Activity Type'],
                            $row['Activity'],
                            $row['Sub Activity'],
                            $row['Activity Name'],
                            $row['Mechanism'],
                            $row['Promo Start'],
                            $row['Promo End'],
                            $row['Creation Date'],
                            $row['Status'],
                            $row['Status Date'],
                            $row['Last Sendback Note'],
                            $row['Target'],
                            $row['Investment'],
                            $row['Investment Code'],
                            $row['Investment Type'],
                            $row['Last Promo Investment Type'],
                            $row['DN Claim'],
                            $row['DN Paid'],
                            $row['Investment Before Closure'],
                            $row['Closed Balance'],
                            $row['Remaining Balance'],
                            $row['Closure Status'],
                            $row['Recon Status'],
                            $row['Approval Recon Status'],
                            $row['Cancel Reason'],
                            $row['Actual Sales'],
                            $row['Initiator Notes'],
                            $row['Budget Mass Approval Status'],
                            $row['Budget Mass Approval Date'],
                            $row['Budget Mass Approval By'],
                            $row['Budget Deploy Status'],
                            $row['Budget Deploy Date'],
                            $row['Budget Deploy By'],
                            $row['Batch ID'],
                            $row['Main Activity'],
                        ), ",");
                    }
                    fclose($file);
                };

                return response()->stream($callback, 200, $headers);
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
