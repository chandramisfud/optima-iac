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

class Investment extends Controller
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
        $title = "Investment Report";
        return view('FinanceReport::investment.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/investment';
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
                'BudgetParentId'            => $request->budgetAllocationId,
                'ChannelId'                 => 0,
                'PageNumber'                => $page,
                'Search'                    => $search,
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

    public function getListBudgetAllocation(Request $request)
    {
        $api = config('app.api'). '/finance-report/investment/budgetallocation';
        $query = [
            'year'          => $request->year,
            'userid'        => 0,
            'entityId'      => 0,
            'distributorId' => 0,
            'budgetParentId'=> 0,
            'channelId'     => 0
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/finance-report/investment/entity';
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
        $api = config('app.api'). '/finance-report/investment/distributor';
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
        $api = config('app.api'). '/finance-report/investment';
        $query = [
            'Period'                    => $request->period,
            'profileId'                 => 'admin',
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => $request->budgetAllocationId,
            'ChannelId'                 => 0,
            'PageNumber'                => 0,
            'Search'                    => '',
            'PageSize'                  => -1,
            'SortColumn'                => 'budgetAllocationName',
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
                    $arr[] = $fields->categoryDesc;
                    $arr[] = $fields->budgetAllocationName;
                    $arr[] = $fields->activityType;
                    $arr[] = (($fields->isLastLayer === 1) ? "TRUE" : "FALSE");
                    $arr[] = $fields->channel;
                    $arr[] = $fields->budgetDeployed;
                    $arr[] = $fields->promoCreated;
                    $arr[] = $fields->dnClaimed;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->returnBalanceFromPromo;
                    $arr[] = $fields->remainingBudget;
                    $arr[] = $fields->gapBudgetDeployedvsPromoCreated;
                    $arr[] = $fields->gapPromoCreatedvsDNClaimed;
                    $arr[] = $fields->gapDNClaimedvsDNPaid;

                    $result[] = $arr;
                }

                $filename = 'Investment-';
                $title = 'A1:P1'; //Report Title Bold and merge
                $header = 'A5:P5'; //Header Column Bold and color
                $heading = [
                    ['Investment Report'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Category', 'Name', 'Sub Activity Type', 'Is Last Layer', 'Channel', 'Budget Deployed', 'Promo Created', 'DN Claim',
                        'DN Paid', 'Returned Balance From Promo Closure', 'Remaining Budget Balance', 'Budget Deployed vs Promo Created', 'Promo Created vs DN Claim', 'DN Claim vs DN Paid']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
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
