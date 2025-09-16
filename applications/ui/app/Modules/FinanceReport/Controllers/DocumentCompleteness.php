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

class DocumentCompleteness extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/documentcompleteness';
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
                'EntityId'                  => $request->entityId,
                'profileId'                 => 'admin',
                'DistributorId'             => $request->distributorId,
                'TaxLevel'                  => '0',
                'Status'                    => '0',
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

    public function landingPage()
    {
        $title = "Debit Note [Doc Completeness]";
        return view('FinanceReport::document-completeness.index', compact('title'));
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/finance-report/documentcompleteness/entity';
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
        $api = config('app.api'). '/finance-report/documentcompleteness/distributor';
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

    public function exportXls(Request $request)
    {
        $api = config('app.api') . '/finance-report/documentcompleteness';
        $query = [
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'profileId'                 => 'admin',
            'TaxLevel'                  => '0',
            'Status'                    => '0',
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'promoRefId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result = [];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->refId;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->totalClaim;
                    $arr[] = $fields->lastStatus;
                    $arr[] = ((!$fields->lastUpdate) ? '' : date('d-m-Y H:i:s', strtotime($fields->lastUpdate)));
                    $arr[] = $fields->materialNumber;
                    $arr[] = $fields->notes;
                    $arr[] = $fields->sp_principal;
                    $arr[] = $fields->remarkSales;
                    $arr[] = $fields->salesValidationStatus;
                    $arr[] = $fields->fpNumber;
                    $arr[] = ((!$fields->fpDate) ? '' : date('d-m-Y H:i:s', strtotime($fields->fpDate)));
                    $arr[] = $fields->validate_by_finance_by;
                    $arr[] = $fields->validate_by_finance_username;
                    $arr[] = ((!$fields->validate_by_finance_on) ? '' : date('d-m-Y H:i:s', strtotime($fields->validate_by_finance_on)));
                    $arr[] = (($fields->original_Invoice_from_retailers) ? 'Y' : 'N');
                    $arr[] = (($fields->tax_Invoice) ? 'Y' : 'N');
                    $arr[] = (($fields->promotion_Agreement_Letter) ? 'Y' : 'N');
                    $arr[] = (($fields->trading_Term) ? 'Y' : 'N');
                    $arr[] = (($fields->sales_Data) ? 'Y' : 'N');
                    $arr[] = (($fields->copy_of_Mailer) ? 'Y' : 'N');
                    $arr[] = (($fields->copy_of_Photo_Doc) ? 'Y' : 'N');
                    $arr[] = (($fields->list_of_Transfer) ? 'Y' : 'N');

                    $result[] = $arr;
                }

                $filename = 'DN Doc Completeness Report-';
                $title = 'A1:X1'; //Report Title Bold and merge
                $header = 'A3:X3'; //Header Column Bold and color
                $heading = [
                    ['Debet Note Doc Completeness'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'DN Number',
                        'Promo ID',
                        'Activity',
                        'Total Claim',
                        'Last Status',
                        'Last Update',
                        'TaxLevel',
                        'Rejection Remarks by Finance',
                        'SP No',
                        'Remark by Sales',
                        'Sales Validation Status',
                        'FP Number',
                        'FP Date',
                        'Validate By Finance By',
                        'Validate By Finance By Username',
                        'Validate By Finance On',
                        'Original Invoice from retailers',
                        'Tax Invoice',
                        'Promotion Agreement Letter',
                        'Trading Term',
                        'Sales Data (sell in/sell out)',
                        'Copy of mailer',
                        'Copy of photo documentation',
                        'List of Transfer'
                    ]
                ];

                $formatCell =  [
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc . '.xlsx');
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
