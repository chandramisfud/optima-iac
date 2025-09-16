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

class DNDetailReporting extends Controller
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
        $title = "DN Detail Reporting";
        return view('FinanceReport::dn-detail-reporting.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/dndetailreporting';
        try {
            $page = 0;
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Period'                    => $request->period,
                'profileId'                 => 'admin',
                'CategoryId'                => ($request->categoryId ?? 0),
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'BudgetParentId'            => 0,
                'ChannelId'                 => 0,
                'SubAccountId'              => $request->subAccountId,
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
        $api = config('app.api'). '/finance-report/dndetailreporting/category';
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
        $api = config('app.api'). '/finance-report/dndetailreporting/entity';
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
        $api = config('app.api'). '/finance-report/dndetailreporting/distributor';
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

    public function getListSubAccount(Request $request)
    {
        $api = config('app.api'). '/finance-report/dndetailreporting/subaccount';
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

    public function getDataGap(Request $request)
    {
        $api = config('app.api'). '/finance-report/dndetailreporting/investmentnotif';
        $query = [
            'periode'   => $request->periode,
            'userid'    => 'admin',
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

    public function exportCsv(Request $request) {
        $api = config('app.api'). '/finance-report/dndetailreporting';

        $query = [
            'Period'                    => $request->period,
            'profileId'                 => 'admin',
            'CategoryId'                => ($request->categoryId ?? 0),
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => 0,
            'SubAccountId'              => $request->subAccountId,
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

                $mc = microtime(true);
                $fileName = 'DNDetailReporting-' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                if ($request->session()->get('role') == '102' || $request->session()->get('role') == '110') {
                    $columns = array(
                        'Distributor', 'Entity', 'DN Number', 'Promo ID', 'Category', 'Activity', 'channel', 'Sub Account', 'Selling Point', 'Profit Center', 'Fee Description', 'Fee (%)',
                        'Fee Amount', 'DPP', 'PPN (%)','PPN Amount', 'PPH (%)', 'PPH Amount', 'Payment Date', 'Last Status', 'Surat Pengantar Cabang', 'Surat Pengantar HO', 'Invoice No',
                        'Send To Danone On', 'Received By Danone By', 'Received by Danone On', 'Invoice On','Confirm Paid On',
                        'Over Budget Status', 'Internal Doc. No', 'Memorial Doc. No', 'TaxLevel', 'WHT Type', 'Rejection Remarks by Finance','Initiator', 'Sales Validation Status', 'FP Number', 'FP Date', 'Remark by Sales','PIC DN Manual', 'Batch Name',
                        'Mechanism', 'Promo Start', 'Promo End', 'SKU', 'Approval Status', 'Activity Name', 'Closure Status', 'Remaining Balance', 'Sub Activity Type', 'Sub Activity'
                    );

                    $callback = function () use ($resVal, $columns) {
                        $file = fopen('php://output', 'w');
                        fputcsv($file, ["Debet Note Detail Reporting"], ",");

                        fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')], ",");
                        fputcsv($file, $columns, ",");

                        foreach ($resVal as $task) {
                            $row['Distributor']             = $task->distributorName;
                            $row['Entity']                  = $task->entityDesc;
                            $row['DN Number']               = $task->refId;
                            $row['Promo ID']                = $task->promoRefId;
                            $row['Category']                = $task->dnCategory;
                            $row['Activity']                = $task->activityDesc;
                            $row['channel']                 = $task->channel;
                            $row['Sub Account']             = $task->subAccount;
                            $row['Selling Point']           = $task->sellingpoint;
                            $row['Profit Center']           = $task->profitCenter;
                            $row['Fee Description']         = $task->feeDesc;
                            $row['Fee (%)']                 = $task->feePct;
                            $row['Fee Amount']              = $task->feeAmount;
                            $row['DPP']                     = $task->dpp;
                            $row['PPN (%)']                 = $task->ppnPct;
                            $row['PPN Amount']              = $task->ppnAmt;
                            $row['PPH (%)']                 = $task->pphPct;
                            $row['PPH Amount']              = $task->pphAmt;
                            if($task->paymentDate == null){
                                $row['Payment Date']        = "";
                            }else{
                                $row['Payment Date']        = date('Y-m-d' , strtotime($task->paymentDate));
                            }
                            $row['Last Status']             = $task->lastStatus;
                            $row['Surat Pengantar Cabang']  = $task->suratPengantarCabang;
                            $row['Surat Pengantar HO']      = $task->suratPengantarHO;
                            $row['Invoice No']              = $task->invoiceNo;
                            if($task->send_to_danone_on == null || $task->send_to_danone_on == ''){
                                $row['Send To Danone On']   = "";
                            }else{
                                $row['Send To Danone On']   = date('Y-m-d' , strtotime($task->send_to_danone_on));
                            }
                            $row['Received By Danone By']   = $task->received_by_danone_by;
                            if($task->receivedByDanoneOn == null || $task->receivedByDanoneOn == ''){
                                $row['Received by Danone On']  = "";
                            }else{
                                $row['Received by Danone On']  = date('Y-m-d' , strtotime($task->receivedByDanoneOn));
                            }
                            if($task->invoice_on == null || $task->invoice_on == ''){
                                $row['Invoice On'] = "";
                            }else{
                                $row['Invoice On'] = date('Y-m-d' , strtotime($task->invoice_on));
                            }
                            if($task->confirm_paid_on == null || $task->confirm_paid_on == ''){
                                $row['Confirm Paid On'] = "";
                            }else{
                                $row['Confirm Paid On'] = date('Y-m-d' , strtotime($task->confirm_paid_on));
                            }
                            $row['Over Budget Status']      = $task->overBudgetStatus;
                            $row['Internal Doc. No']        = $task->intDocNo;
                            $row['Memorial Doc. No']        = $task->memDocNo;
                            $row['TaxLevel']                = $task->materialNumber . ' - ' . $task->taxLevel;
                            $row['WHT Type']        = $task->whtType;
                            $row['Rejection Remarks by Finance']    = $task->notes;
                            $row['Initiator']               = $task->initiator;
                            $row['Sales Validation Status'] = $task->salesValidationStatus;
                            $row['FP Number']               = $task->fpNumber;
                            if($task->fpNumber==null || $task->fpNumber==""){
                                $row['FP Date'] = "";
                            }else{
                                $row['FP Date'] = date('Y-m-d' , strtotime($task->fpDate));
                            }
                            $row['Remark by Sales']         = $task->statusSalesNotes;
                            $row['PIC DN Manual']           = $task->pic;
                            $row['Batch Name']              = $task->batchname;
                            $row['Mechanism']               = $task->mechanism;
                            if($task->startPromo == null || $task->startPromo == ''){
                                $row['Promo Start']  = "";
                            }else{
                                $row['Promo Start']  = date('Y-m-d' , strtotime($task->startPromo));
                            }
                            if($task->endPromo == null || $task->endPromo == ''){
                                $row['Promo End']  = "";
                            }else{
                                $row['Promo End']  = date('Y-m-d' , strtotime($task->endPromo));
                            }
                            $row['SKU']                     = $task->sku;
                            $row['Approval Status']         = $task->approvalStatus;
                            $row['Activity Name']           = $task->promo_activity_name;
                            if($task->isClose) { $row['CLosure Status'] = "Closed"; }else{ $row['CLosure Status'] = "Open";}
                            $row['Remaining Balance']       = $task->remainingBalance;
                            $row['Sub Activity Type']       = $task->subActivityType;
                            $row['Sub Activity']            = $task->subActivityDesc;

                            fputcsv($file, array(
                                $row['Distributor'],
                                $row['Entity'],
                                $row['DN Number'],
                                $row['Promo ID'],
                                $row['Category'],
                                $row['Activity'],
                                $row['channel'],
                                $row['Sub Account'],
                                $row['Selling Point'],
                                $row['Profit Center'],
                                $row['Fee Description'],
                                $row['Fee (%)'],
                                $row['Fee Amount'],
                                $row['DPP'],
                                $row['PPN (%)'],
                                $row['PPN Amount'],
                                $row['PPH (%)'],
                                $row['PPH Amount'],
                                $row['Payment Date'],
                                $row['Last Status'],
                                $row['Surat Pengantar Cabang'],
                                $row['Surat Pengantar HO'],
                                $row['Invoice No'],
                                $row['Send To Danone On'],
                                $row['Received By Danone By'],
                                $row['Received by Danone On'],
                                $row['Invoice On'],
                                $row['Confirm Paid On'],
                                $row['Over Budget Status'],
                                $row['Internal Doc. No'],
                                $row['Memorial Doc. No'],
                                $row['TaxLevel'],
                                $row['WHT Type'],
                                $row['Rejection Remarks by Finance'],
                                $row['Initiator'],
                                $row['Sales Validation Status'],
                                $row['FP Number'],
                                $row['FP Date'],
                                $row['Remark by Sales'],
                                $row['PIC DN Manual'],
                                $row['Batch Name'],
                                $row['Mechanism'],
                                $row['Promo Start'],
                                $row['Promo End'],
                                $row['SKU'],
                                $row['Approval Status'],
                                $row['Activity Name'],
                                $row['CLosure Status'],
                                $row['Remaining Balance'],
                                $row['Sub Activity Type'],
                                $row['Sub Activity']
                            ), ",");
                        }
                        fclose($file);
                    };
                } else {
                    $columns = array(
                        'Distributor', 'Entity', 'DN Number', 'Promo ID', 'Category', 'Activity', 'Channel', 'Account', 'Sub Account', 'Selling Point', 'Profit Center', 'Fee Description', 'Fee (%)',
                        'Fee Amount', 'DPP', 'PPN (%)', 'PPN Amount', 'PPH (%)', 'PPH Amount', 'Total Claim', 'Total Paid', 'Payment Date', 'Last Status', 'Surat Pengantar Cabang', 'Surat Pengantar HO', 'Invoice No',
                        'Create By', 'Create On', 'Send To Danone By', 'Send To Danone On', 'Received By Danone By', 'Received by Danone On', 'Validate By Finance By', 'Validate By Finance By (User Name)', 'Validate By Finance On', 'Validate By Sales By', 'Validate By Sales On',
                        'Invoice Notif By', 'Invoice Notif On', 'Invoice By', 'Invoice On', 'Confirm Paid By', 'Confirm Paid On',
                        'Aging', 'Last Update', 'Over Budget Status', 'Internal Order Number', 'Internal Doc. No', 'Memorial Doc. No', 'TaxLevel', 'WHT Type', 'Rejection Remarks by Finance', 'Initiator', 'Sales Validation Status', 'FP Number', 'FP Date', 'Remark by Sales',
                        'VAT Expired', 'PO Number', 'PIC DN Manual', 'Batch Name', 'Closure Status', 'Remaining Balance', 'Sub Activity Type', 'Sub Activity'
                    );

                    $callback = function () use ($resVal, $columns) {
                        $file = fopen('php://output', 'w');
                        fputcsv($file, ["Debet Note Detail Reporting"], ",");

                        fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')], ",");
                        fputcsv($file, $columns, ",");

                        foreach ($resVal as $val) {
                            $row['Distributor'] = $val->distributorName;
                            $row['Entity'] = $val->entityDesc;
                            $row['DN Number'] = $val->refId;
                            $row['Promo ID'] = $val->promoRefId;
                            $row['Category'] = $val->dnCategory;
                            $row['Activity'] = $val->activityDesc;
                            $row['Channel'] = $val->channel;
                            $row['Account'] = $val->account;
                            $row['Sub Account'] = $val->subAccount;
                            $row['Selling Point'] = $val->sellingpoint;
                            $row['Profit Center'] = $val->profitCenter;
                            $row['Fee Description'] = $val->feeDesc;
                            $row['Fee (%)'] = $val->feePct;
                            $row['Fee Amount'] = $val->feeAmount;
                            $row['DPP'] = $val->dpp;
                            $row['PPN (%)'] = $val->ppnPct;
                            $row['PPN Amount'] = $val->ppnAmt;
                            $row['PPH (%)'] = $val->pphPct;
                            $row['PPH Amount'] = $val->pphAmt;
                            $row['Total Claim'] = $val->totalClaim;
                            $row['Total Paid'] = $val->totalPaid;
                            $row['Payment Date'] = (($val->paymentDate) ? date('Y-m-d', strtotime($val->paymentDate)) : '');
                            $row['Last Status'] = $val->lastStatus;
                            $row['Surat Pengantar Cabang'] = $val->suratPengantarCabang;
                            $row['Surat Pengantar HO'] = $val->suratPengantarHO;
                            $row['Invoice No'] = $val->invoiceNo;
                            $row['Create By'] = $val->createBy;
                            $row['Create On'] = (($val->createOn) ? date('Y-m-d', strtotime($val->createOn)) : '');
                            $row['Send To Danone By'] = $val->send_to_danone_by;
                            $row['Send To Danone On'] = (($val->send_to_danone_on) ? date('Y-m-d', strtotime($val->send_to_danone_on)) : '');
                            $row['Received By Danone By'] = $val->received_by_danone_by;
                            $row['Received by Danone On'] = (($val->receivedByDanoneOn) ? date('Y-m-d', strtotime($val->receivedByDanoneOn)) : '');
                            $row['Validate By Finance By'] = $val->validate_by_finance_by;
                            $row['Validate By Finance By (User Name)'] = $val->validate_by_finance_by_username;
                            $row['Validate By Finance On'] = (($val->validate_by_finance_on) ? date('Y-m-d', strtotime($val->validate_by_finance_on)) : '');
                            $row['Validate By Sales By'] = $val->validate_by_sales_by;
                            $row['Validate By Sales On'] = (($val->validate_by_sales_on) ? date('Y-m-d', strtotime($val->validate_by_sales_on)) : '');
                            $row['Invoice Notif By'] = $val->invoiceNotifBy;
                            $row['Invoice Notif On'] = (($val->invoiceNotifOn) ? date('Y-m-d', strtotime($val->invoiceNotifOn)) : '');
                            $row['Invoice By'] = $val->invoice_by;
                            $row['Invoice On'] = (($val->invoice_on) ? date('Y-m-d', strtotime($val->invoice_on)) : '');
                            $row['Confirm Paid By'] = $val->confirm_paid_by;
                            $row['Confirm Paid On'] = (($val->confirm_paid_on) ? date('Y-m-d', strtotime($val->confirm_paid_on)) : '');
                            $row['Aging'] = $val->aging;
                            $row['Last Update'] = date('Y-m-d', strtotime($val->lastUpdate));
                            $row['Over Budget Status'] = $val->overBudgetStatus;
                            $row['Internal Order Number'] = $val->internalOrderNumber;
                            $row['Internal Doc. No'] = $val->intDocNo;
                            $row['Memorial Doc. No'] = $val->memDocNo;
                            $row['TaxLevel'] = $val->materialNumber . ' - ' . $val->taxLevel;
                            $row['WHT Type'] = $val->whtType;
                            $row['Rejection Remarks by Finance'] = $val->notes;
                            $row['Initiator'] = $val->initiator;
                            $row['Sales Validation Status'] = $val->salesValidationStatus;
                            $row['FP Number'] = $val->fpNumber;
                            $row['FP Date'] = (($val->fpNumber === null || $val->fpNumber === "") ? '' : date('Y-m-d', strtotime($val->fpDate)));
                            $row['Remark by Sales'] = $val->statusSalesNotes;
                            $row['VAT Expired'] = (($val->vatExpired) ? "Y" : "N");
                            $row['PO Number'] = $val->ponumber;
                            $row['PIC DN Manual'] = $val->pic;
                            $row['Batch Name'] = $val->batchname;
                            $row['Closure Status'] = (isset($val->isClose) ? (($val->isClose) ? "Closed" : "Open") : "");
                            $row['Remaining Balance'] = ($val->remainingBalance ?? "");
                            $row['Sub Activity Type'] = ($val->subActivityType ?? "");
                            $row['Sub Activity'] = ($val->subActivityDesc ?? "");

                            fputcsv($file, array(
                                $row['Distributor'],
                                $row['Entity'],
                                $row['DN Number'],
                                $row['Promo ID'],
                                $row['Category'],
                                $row['Activity'],
                                $row['Channel'],
                                $row['Account'],
                                $row['Sub Account'],
                                $row['Selling Point'],
                                $row['Profit Center'],
                                $row['Fee Description'],
                                $row['Fee (%)'],
                                $row['Fee Amount'],
                                $row['DPP'],
                                $row['PPN (%)'],
                                $row['PPN Amount'],
                                $row['PPH (%)'],
                                $row['PPH Amount'],
                                $row['Total Claim'],
                                $row['Total Paid'],
                                $row['Payment Date'],
                                $row['Last Status'],
                                $row['Surat Pengantar Cabang'],
                                $row['Surat Pengantar HO'],
                                $row['Invoice No'],
                                $row['Create By'],
                                $row['Create On'],
                                $row['Send To Danone By'],
                                $row['Send To Danone On'],
                                $row['Received By Danone By'],
                                $row['Received by Danone On'],
                                $row['Validate By Finance By'],
                                $row['Validate By Finance By (User Name)'],
                                $row['Validate By Finance On'],
                                $row['Validate By Sales By'],
                                $row['Validate By Sales On'],
                                $row['Invoice Notif By'],
                                $row['Invoice Notif On'],
                                $row['Invoice By'],
                                $row['Invoice On'],
                                $row['Confirm Paid By'],
                                $row['Confirm Paid On'],
                                $row['Aging'],
                                $row['Last Update'],
                                $row['Over Budget Status'],
                                $row['Internal Order Number'],
                                $row['Internal Doc. No'],
                                $row['Memorial Doc. No'],
                                $row['TaxLevel'],
                                $row['WHT Type'],
                                $row['Rejection Remarks by Finance'],
                                $row['Initiator'],
                                $row['Sales Validation Status'],
                                $row['FP Number'],
                                $row['FP Date'],
                                $row['Remark by Sales'],
                                $row['VAT Expired'],
                                $row['PO Number'],
                                $row['PIC DN Manual'],
                                $row['Batch Name'],
                                $row['Closure Status'],
                                $row['Remaining Balance'],
                                $row['Sub Activity Type'],
                                $row['Sub Activity']
                            ), ",");
                        }
                        fclose($file);
                    };
                }
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

    public function exportXlsGap(Request $request) {
        $api = config('app.api'). '/finance-report/dndetailreporting/investmentnotif';
        $query = [
            'periode'   => $request->periode,
            'userid'    => 'admin',
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->gaP_list;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->dnClaim_promo;
                    $arr[] = $fields->dnPaid_promo;
                    $arr[] = $fields->dnNumber;
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->gaP_DNClaim;
                    $arr[] = $fields->gaP_DNPaid;

                    $result[] = $arr;
                }

                $filename = 'GAPDNClaim-';
                $title = 'A1:I1'; //Report Title Bold and merge
                $header = 'A4:I4'; //Header Column Bold and color
                $heading = [
                    ['GAP DN Claim'],
                    ['Budget Year : ' . $request->periode],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Budget', 'Promo Number', 'DN Claim', 'DN Paid', 'DN Number', 'Total Claim', 'Total Paid', 'GAP Claim', 'GAP Paid']
                ];
                $formatcell =  [
                    'C' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatcell);
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
