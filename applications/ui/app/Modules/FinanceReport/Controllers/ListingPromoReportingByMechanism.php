<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use App\Helpers\Formatted;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Symfony\Component\HttpFoundation\BinaryFileResponse;
use Symfony\Component\HttpFoundation\StreamedResponse;

class ListingPromoReportingByMechanism extends Controller
{
    protected mixed $token;
    protected mixed $formatFunction;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');
            $this->formatFunction = new Formatted();

            return $next($request);
        });
    }

    public function landingPage(): Factory|View|Application
    {
        $title = "Listing Promo Reporting by Mechanism";
        return view('FinanceReport::listing-promo-reporting-by-mechanism.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): array|bool|string
    {
        try {
            $api = '/finance-report/listingpromoreportingbymechanism';
            $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
            $query = [
                'Period'                    => $request['period'],
                'CategoryId'                => $request['categoryId'],
                'EntityId'                  => $request['entityId'],
                'DistributorId'             => $request['distributorId'],
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request['channelId'],
                'CreateFrom'                => $request['startFrom'],
                'CreateTo'                  => $request['startTo'],
                'StartFrom'                 => $request['startFrom'],
                'StartTo'                   => $request['startTo'],
                'SubmissionParam'           => 0,
                'Search'                    => ($request['search']['value'] ?? ""),
                'SortColumn'                => $request['columns'][$request['order'][0]['column']]['data'],
                'SortDirection'             => $request['order'][0]['dir'],
                'PageSize'                  => $request['length'],
                'PageNumber'                => $page,
            ];

            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $resVal = json_decode($res)->data->data;
                return json_encode([
                    "draw" => (int)$request['draw'],
                    "data" => $resVal,
                    "recordsTotal" => json_decode($res)->data->totalCount,
                    "recordsFiltered" => json_decode($res)->data->filteredCount
                ]);
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function getListCategory(): bool|string
    {
        $api = '/finance-report/listingpromoreporting/category';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListEntity(): bool|string
    {
        $api = '/report/listingpromoreporting/entity';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListDistributor(Request $request): bool|string
    {
        $api = '/finance-report/listingpromoreporting/distributor';
        $callApi = new CallApi();
        $query = [
            'entityId'      => $request['entityId'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListChannel(): bool|string
    {
        $api = '/finance-report/listingpromoreporting/channel';
        $callApi = new CallApi();
        $query = [
            'userid'           => 'admin',
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function exportXls(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/finance-report/listingpromoreportingbymechanism';
            $query = [
                'Period'                    => $request['period'],
                'CategoryId'                => $request['categoryId'],
                'EntityId'                  => $request['entityId'],
                'DistributorId'             => $request['distributorId'],
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request['channelId'],
                'CreateFrom'                => $request['startFrom'],
                'CreateTo'                  => $request['startTo'],
                'StartFrom'                 => $request['startFrom'],
                'StartTo'                   => $request['startTo'],
                'SubmissionParam'           => 0,
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
                'SortColumn'                => 'promoNumber',
                'SortDirection'             => 'asc'
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data->data;

                $result = [];

                foreach ($data as $fields) {
                    $arr = [];

                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('d-m-Y' , strtotime($fields->lastUpdate));
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->groupBrandDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = ($this->formatFunction->isNotDate($fields->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($fields->lastStatusDate)));
                    if($fields->sendbackNotes==''){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->sendbackNotes . ' on ' . $fields->sendbackNotesDate;
                    }
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->target;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    if($fields->investmentTypeRefIdPromo=='---' || $fields->investmentTypeRefIdPromo=='' || $fields->investmentTypeRefIdPromo==null)
                    {
                        $arr[] = '---';
                    }else{
                        $arr[] = $fields->investmentTypeRefIdPromo. ' - ' . $fields->investmentTypeDescPromo;
                    }
                    $arr[] = $fields->roi;
                    $arr[] = $fields->costRatio / 100;
                    $arr[] = $fields->remainingBalance;
                    $arr[] = $fields->gap;
                    if($fields->submissionDeadline == null){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->submissionDeadline;
                    }
                    if($fields->onTime){
                        $arr[] = 'On-Time';
                    }else{
                        $arr[] = 'Late';
                    }
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->investmentBfrClose;
                    $arr[] = $fields->investmentClosedBalance;
                    if($fields->closureStatus){
                        $arr[] = 'Closed';
                    }else{
                        $arr[] = 'Open';
                    }
                    $arr[] = $fields->reconStatus;
                    if($fields->lastReconStatus==null){
                        $arr[] = '---';
                    }else{
                        $arr[] = $fields->lastReconStatus;
                    }
                    $arr[] = $fields->cancelReason;
                    $arr[] = $fields->actualSales;
                    $arr[] = $fields->initiatorNotes;
                    $arr[] = $fields->budgetApprovalStatus;
                    $arr[] = $fields->budgetApprovalStatusOn;
                    $arr[] = $fields->budgetApprovalStatusBy;
                    $arr[] = $fields->budgetDeployStatus;
                    $arr[] = $fields->budgetDeployStatusOn;
                    $arr[] = $fields->budgetDeployStatusBy;
                    $arr[] = $fields->smartCode;
                    $arr[] = $fields->baseline;
                    $arr[] = $fields->totalSales;
                    $arr[] = ($fields->uplift ? $fields->uplift / 100 : 0);
                    $arr[] = ($fields->salesContribution ? $fields->salesContribution / 100 : 0);
                    $arr[] = ($fields->storesCoverage ? $fields->storesCoverage / 100 : 0);
                    $arr[] = ($fields->redemptionRate ? $fields->redemptionRate / 100 : 0);
                    $arr[] = ($fields->cr ? $fields->cr / 100 : 0);
                    $arr[] = $fields->cost;
                    $arr[] = $fields->mainActivity;


                    $result[] = $arr;
                }
                $heading = [
                    ['Listing Promo Reporting by Mechanism'],
                    ['Budget Year : ' . $request['period']],
                    ['Category : ' . $request['category']],
                    ['Entity : ' . $request['entity']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'Planning ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account',
                        'Brand', 'Sub Brand', 'SKU', 'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End',
                        'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type',
                        'Last Promo Investment Type', 'ROI', 'Cost Ratio', 'Remaining Balance', 'GAP', 'Submission Deadline', 'Status Submission', 'DN Claim', 'DN Paid', 'Investment Before Closure',
                        'Closed Balance', 'Closure Status', 'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                        'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval by', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By',
                        'Smart Code', 'Baseline', 'Total Sales', 'Uplift (%)', 'Sales Contribution (%)', 'Stores Coverage (%)', 'Redemption Rate (%)', 'CR (%)', 'Cost', 'Main Activity'
                    ]
                ];
                $formatCell =  [
                    'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AK' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AL' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AS' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AT' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AY' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BH' => NumberFormat::builtInFormatCode(37),
                    'BI' => NumberFormat::builtInFormatCode(37),
                    'BJ' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BK' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BL' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BM' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BN' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'BO' => NumberFormat::builtInFormatCode(37),
                ];
                $title = 'A1:BP1'; //Report Title Bold and merge
                $header = 'A6:BP6'; //Header Column Bold and color
                $filename = 'Listing Promo Reporting by Mechanism - ';
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportCsv(Request $request): StreamedResponse|bool|array|string
    {
        try {
            $api = '/finance-report/listingpromoreportingbymechanism';
            $query = [
                'Period'                    => $request['period'],
                'CategoryId'                => $request['categoryId'],
                'EntityId'                  => $request['entityId'],
                'DistributorId'             => $request['distributorId'],
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request['channelId'],
                'CreateFrom'                => $request['startFrom'],
                'CreateTo'                  => $request['startTo'],
                'StartFrom'                 => $request['startFrom'],
                'StartTo'                   => $request['startTo'],
                'SubmissionParam'           => 0,
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
                'SortColumn'                => 'promoNumber',
                'SortDirection'             => 'asc'
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $resVal = json_decode($res)->data->data;

                $mc = microtime(true);
                $fileName = 'Listing Promo Reporting by Mechanism - ' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columns = array(
                    'Planning ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account',
                    'Brand', 'Sub Brand', 'SKU', 'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Status',
                    'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI',
                    'Cost Ratio', 'Remaining Balance', 'GAP', 'Submission Deadline', 'Status Submission', 'DN Claim', 'DN Paid', 'Investment Before Closure', 'Closed Balance', 'Closure Status',
                    'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                    'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval By', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By',
                    'Smart Code', 'Baseline', 'Total Sales', 'Uplift (%)', 'Sales Contribution (%)', 'Stores Coverage (%)', 'Redemption Rate (%)', 'CR (%)', 'Cost', 'Main Acitivty'
                );

                $period = $request['period'];
                $category = $request['category'];
                $entity = $request['entity'];

                $callback = function () use ($resVal, $columns, $period, $category, $entity) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["Listing Promo Reporting by Mechanism"], ",");
                    fputcsv($file, ['Budget Year : ' . $period], ",");
                    fputcsv($file, ['Category : ' . $category], ",");
                    fputcsv($file, ['Entity : ' . $entity], ",");
                    fputcsv($file, ['Date Retrieved : ' . date('d-m-Y')], ",");
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
                        $row['Sub Channel']                 = $task->subChannelDesc;
                        $row['Account']                     = $task->accountDesc;
                        $row['Sub Account']                 = $task->subAccountDesc;
                        $row['Brand']                       = $task->groupBrandDesc;
                        $row['Sub Brand']                   = $task->brandDesc;
                        $row['SKU']                         = $task->skuDesc;
                        $row['Sub Category']                = $task->subCategory;
                        $row['Sub Activity Type']           = $task->subActivityType;
                        $row['Activity']                    = $task->activity;
                        $row['Sub Activity']                = $task->subActivity;
                        $row['Activity Name']               = $task->activityDesc;
                        $row['Mechanism']                   = $task->mechanism;
                        $row['Promo Start']                 = date('Y-m-d' , strtotime($task->startPromo));
                        $row['Promo End']                   = date('Y-m-d' , strtotime($task->endPromo));
                        $row['Creation Date']               = date('Y-m-d' , strtotime($task->createOn));
                        $row['Status']                      = $task->lastStatus;
                        $row['Status Date']                 = ($this->formatFunction->isNotDate($task->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($task->lastStatusDate)));
                        $row['Last Sendback Note']          = ($task->sendbackNotes === "" ? '' : $task->sendbackNotes . ' on ' . $task->sendbackNotesDate);
                        $row['Baseline Sales']              = $task->normalSales;
                        $row['Incr Sales']                  = $task->incrSales;
                        $row['Total Sales']                 = $task->target;
                        $row['Investment']                  = $task->investment;
                        $row['Investment Code']             = $task->investmentTypeRefId;
                        $row['Investment Type']             = $task->investmentTypeDesc;
                        $row['Last Promo Investment Type']  = ($task->investmentTypeRefIdPromo === '---' || $task->investmentTypeRefIdPromo === '' || $task->investmentTypeRefIdPromo==null ? '---' : $task->investmentTypeRefIdPromo. ' - ' . $task->investmentTypeDescPromo);
                        $row['ROI']                         = $task->roi;
                        $row['Cost Ratio']                  = ($task->costRatio / 100);
                        $row['Remaining Balance']           = $task->remainingBalance;
                        $row['GAP']                         = $task->gap;
                        $row['Submission Deadline']         = ($task->submissionDeadline ?? '');
                        $row['Status Submission']           = ($task->onTime ? 'On-Time' : 'Late');
                        $row['DN Claim']                    = $task->dnClaim;
                        $row['DN Paid']                     = $task->dnPaid;
                        $row['Investment Before Closure']   = $task->investmentBfrClose;
                        $row['Closed Balance']              = $task->investmentClosedBalance;
                        $row['Closure Status']              = ($task->closureStatus ? 'Closed' : 'Open');
                        $row['Recon Status']                = $task->reconStatus;
                        $row['Approval Recon Status']       = ($task->lastReconStatus ?? '---');
                        $row['Cancel Reason']               = $task->cancelReason;
                        $row['Actual Sales']                = $task->actualSales;
                        $row['Initiator Notes']             = $task->initiatorNotes;
                        $row['Budget Mass Approval Status'] = $task->budgetApprovalStatus;
                        $row['Budget Mass Approval Date']   = $task->budgetApprovalStatusOn;
                        $row['Budget Mass Approval By']     = $task->budgetApprovalStatusBy;
                        $row['Budget Deploy Status']        = $task->budgetDeployStatus;
                        $row['Budget Deploy Date']          = $task->budgetDeployStatusOn;
                        $row['Budget Deploy By']            = $task->budgetDeployStatusBy;
                        $row['Smart Code']                  = $task->smartCode;
                        $row['Baseline']                    = $task->baseline;
                        $row['Total Sales']                 = $task->totalSales;
                        $row['Uplift (%)']                  = $task->uplift;
                        $row['Sales Contribution (%)']      = $task->salesContribution;
                        $row['Stores Coverage (%)']         = $task->storesCoverage;
                        $row['Redemption Rate (%)']         = $task->redemptionRate;
                        $row['CR (%)']                      = $task->cr;
                        $row['Cost']                        = $task->cost;
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
                            $row['Sub Channel'],
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
                            $row['Baseline Sales'],
                            $row['Incr Sales'],
                            $row['Total Sales'],
                            $row['Investment'],
                            $row['Investment Code'],
                            $row['Investment Type'],
                            $row['Last Promo Investment Type'],
                            $row['ROI'],
                            $row['Cost Ratio'],
                            $row['Remaining Balance'],
                            $row['GAP'],
                            $row['Submission Deadline'],
                            $row['Status Submission'],
                            $row['DN Claim'],
                            $row['DN Paid'],
                            $row['Investment Before Closure'],
                            $row['Closed Balance'],
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
                            $row['Smart Code'],
                            $row['Baseline'],
                            $row['Total Sales'],
                            $row['Uplift (%)'],
                            $row['Sales Contribution (%)'],
                            $row['Stores Coverage (%)'],
                            $row['Redemption Rate (%)'],
                            $row['CR (%)'],
                            $row['Cost'],
                            $row['Main Activity']
                        ), ",");
                    }
                    fclose($file);
                };

                return response()->stream($callback, 200, $headers);
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }
}
