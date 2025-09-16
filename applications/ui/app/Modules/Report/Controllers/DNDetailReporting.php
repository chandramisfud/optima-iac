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
        $title = "Debit Note Detail";
        return view('Report::dn-detail-reporting.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/report/dndetailreporting';
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
        $api = config('app.api'). '/report/dndetailreporting/category';
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
        $api = config('app.api'). '/report/dndetailreporting/entity';
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
        $api = config('app.api'). '/report/dndetailreporting/distributor';
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
        $api = config('app.api'). '/report/dndetailreporting/subaccount';
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
        $api = config('app.api'). '/report/dndetailreporting';

        $query = [
            'Period'                    => $request->period,
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

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->refId;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->dnCategory;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->sellingpoint;
                    $arr[] = $fields->profitCenter;
                    $arr[] = $fields->feeDesc;
                    $arr[] = $fields->feePct;
                    $arr[] = $fields->feeAmount;
                    $arr[] = $fields->dpp;
                    $arr[] = $fields->ppnPct;
                    $arr[] = $fields->ppnAmt;
                    $arr[] = $fields->pphPct;
                    $arr[] = $fields->pphAmt;
                    $arr[] = $fields->totalClaim;
                    $arr[] = $fields->totalPaid;
                    $arr[] = (($fields->paymentDate) ? date('Y-m-d' , strtotime($fields->paymentDate)) : '');
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->suratPengantarCabang;
                    $arr[] = $fields->suratPengantarHO;
                    $arr[] = $fields->invoiceNo;
                    $arr[] = $fields->createBy;
                    $arr[] = (($fields->createOn) ? date('Y-m-d' , strtotime($fields->createOn)) : '');
                    $arr[] = $fields->received_by_danone_by;
                    $arr[] = (($fields->receivedByDanoneOn) ? date('Y-m-d' , strtotime($fields->receivedByDanoneOn)) : '');
                    $arr[] = $fields->validate_by_finance_by;
                    $arr[] = $fields->validate_by_finance_by_username;
                    $arr[] = (($fields->validate_by_finance_on) ? date('Y-m-d' , strtotime($fields->validate_by_finance_on)) : '');
                    $arr[] = $fields->validate_by_sales_by;
                    $arr[] = (($fields->validate_by_sales_on) ? date('Y-m-d' , strtotime($fields->validate_by_sales_on)) : '');
                    $arr[] = $fields->invoiceNotifBy;
                    $arr[] = (($fields->invoiceNotifOn) ? date('Y-m-d' , strtotime($fields->invoiceNotifOn)) : '');
                    $arr[] = $fields->invoice_by;
                    $arr[] = (($fields->invoice_on) ? date('Y-m-d' , strtotime($fields->invoice_on)) : '');
                    $arr[] = $fields->confirm_paid_by;
                    $arr[] = (($fields->confirm_paid_on) ? date('Y-m-d' , strtotime($fields->confirm_paid_on)) : '');
                    $arr[] = $fields->aging;
                    $arr[] = (($fields->lastUpdate) ? date('Y-m-d' , strtotime($fields->lastUpdate)) : '');
                    $arr[] = $fields->overBudgetStatus;
                    $arr[] = $fields->intDocNo;
                    $arr[] = $fields->memDocNo;
                    $arr[] = $fields->materialNumber . ' - ' . $fields->taxLevel;
                    $arr[] = $fields->whtType;
                    $arr[] = $fields->notes;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->salesValidationStatus;
                    $arr[] = $fields->fpNumber;
                    $arr[] = (($fields->fpNumber === null || $fields->fpNumber==="") ? '' :  date('Y-m-d' , strtotime($fields->fpDate)));
                    $arr[] = $fields->statusSalesNotes;
                    $arr[] =  (($fields->vatExpired) ? "Y" : "N");
                    $arr[] = $fields->ponumber;
                    $arr[] = $fields->pic;
                    $arr[] = $fields->batchname;
                    $arr[] = $fields->subActivityDesc;

                    $result[] = $arr;
                }

                $filename = 'DNDetailReporting-';
                $title = 'A1:BD1'; //Report Title Bold and merge
                $header = 'A3:BD3'; //Header Column Bold and color
                $heading = [
                    ['Debit Note Detail Reporting'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'DN Number', 'Promo ID', 'Category', 'Activity', 'Channel', 'Account', 'Sub Account', 'Selling Point', 'Profit Center', 'Fee Description', 'Fee (%)',
                        'Fee Amount', 'DPP', 'PPN (%)', 'PPN Amount', 'PPH (%)', 'PPH Amount', 'Total Claim', 'Total Paid','Payment Date', 'Last Status', 'Surat Pengantar Cabang', 'Surat Pengantar HO', 'Invoice No',
                        'Create By', 'Create On', 'Received By Danone By', 'Received by Danone On', 'Validate By Finance By', 'Validate By Finance By (User Name)', 'Validate By Finance On', 'Validate By Sales By', 'Validate By Sales On',
                        'Invoice Notif By', 'Invoice Notif On', 'Invoice By', 'Invoice On', 'Confirm Paid By', 'Confirm Paid On',
                        'Aging', 'Last Update', 'Over Budget Status', 'Internal Doc. No', 'Memorial Doc. No', 'TaxLevel', 'WHT Type', 'Rejection Remarks by Finance','Initiator','Sales Validation Status', 'FP Number', 'FP Date', 'Remark by Sales', 'VAT Expired', 'PO Number', 'PIC DN Manual', 'Batch Name', 'Sub Activity'
                    ]
                ];
                $formatCell =  [
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Q' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'R' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'S' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' .$mc . '.csv');
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
