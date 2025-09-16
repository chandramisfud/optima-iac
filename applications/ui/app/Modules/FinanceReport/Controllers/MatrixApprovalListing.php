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

class MatrixApprovalListing extends Controller
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
        $title = "Matrix Approval Listing";
        return view('FinanceReport::matrix-approval-listing.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/matrixapprovallisting';
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
                'CategoryId'                => ($request->categoryId ?? 0),
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

    public function getListCategory(Request $request)
    {
        $api = config('app.api'). '/finance-report/matrixapprovallisting/category';
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
        $api = config('app.api'). '/finance-report/matrixapprovallisting/entity';
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
        $api = config('app.api'). '/finance-report/matrixapprovallisting/distributor';
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
        $api = config('app.api'). '/finance-report/matrixapprovallisting';
        $query = [
            'CategoryId'                => ($request->categoryId ?? 0),
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'entity',
            'SortDirection'             => 'desc'
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
                    $arr[] = ($fields->categoryLongDesc ?? "");
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->minInvestment;
                    $arr[] = $fields->maxInvestment;
                    $arr[] = $fields->matrixApprover;
                    $arr[] = ((!$fields->modifiedOn) ? '' : date('Y-m-d' , strtotime($fields->modifiedOn)));

                    $result[] = $arr;
                }

                $filename = 'Listing Matrix Approval Current -';
                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A4:K4';
                $heading = [
                    ['Listing Matrix Approval Current'],
                    ['Entity : ' . $request['entity']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Category', 'Initiator', 'Channel', 'Sub Channel', 'Sub Activity Type', 'Min. Investment', 'Max. Investment', 'Matrix Approver','Last Update']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
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

    public function exportXlsHistorical(Request $request) {
        $api = config('app.api'). '/finance-report/matrixapprovallisting/history';
        $query = [
            'category'                  => ($request->categoryId ?? 0),
            'entity'                    => ($request->entityId ?? 0),
            'distributor'               => ($request->distributorId ?? 0),
            'userid'                    => '0',
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
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = ($fields->categoryLongDesc ?? "");
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->minInvestment;
                    $arr[] = $fields->maxInvestment;
                    $arr[] = $fields->matrixApprover;
                    $arr[] = $fields->actionStatus;
                    $arr[] = $fields->actionBy;
                    $arr[] = $fields->actionEmail;
                    $arr[] = $fields->actionOn;

                    $result[] = $arr;
                }

                $filename = 'Listing Matrix Approval Historical -';
                $title = 'A1:N1'; //Report Title Bold and merge
                $header = 'A4:N4';
                $heading = [
                    ['Listing Matrix Approval Historical'],
                    ['Entity : ' . $request['entity']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Category', 'Initiator', 'Channel', 'Sub Channel', 'Sub Activity Type', 'Min. Investment', 'Max. Investment', 'Matrix Approver'
                        , 'Action Status', 'Action Profile', 'Action Email', 'Action On']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
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
