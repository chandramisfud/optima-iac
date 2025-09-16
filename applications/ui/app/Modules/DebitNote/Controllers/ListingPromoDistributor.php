<?php

namespace App\Modules\DebitNote\Controllers;

use App\Exports\Export;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Wording;

class ListingPromoDistributor extends Controller
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
        $title = "Listing Promo Distributor";
        return view('DebitNote::listing-promo-distributor.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/dn/listing-promo-distributor';
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
                'CategoryId'                => (int) $request->categoryId,
                'EntityId'                  => (int) $request->entityId,
                'DistributorId'             => (int) $this->getDistributorId($request->session()->get('profile')),
                'BudgetParentId'            => 0,
                'ChannelId'                 => 0,
                'CreateFrom'                => $request->createFrom,
                'CreateTo'                  => $request->createTo,
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

    public function getDistributorId($id)
    {
        $api = config('app.api'). '/dn/listing-promo-distributor/userprofile/id';
        $query = [
            'id'    => $id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return json_decode($response)->values->distributorlist[0]->distributorId;
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

    public function getListCategory(Request $request)
    {
        $api = config('app.api'). '/dn/listing-promo-distributor/category';
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
        $api = config('app.api'). '/dn/listing-promo-distributor/entity';
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
        $api = config('app.api'). '/dn/listing-promo-distributor';
        $query = [
            'Period'                    => $request->period,
            'CategoryId'                => (int) $request->categoryId,
            'EntityId'                  => $request->entityId,
            'DistributorId'             => (int) $this->getDistributorId($request->session()->get('profile')),
            'BudgetParentId'            => 0,
            'ChannelId'                 => 0,
            'CreateFrom'                => $request->createFrom,
            'CreateTo'                  => $request->createTo,
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

                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->groupBrandDesc;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('Y-m-d' , strtotime($fields->startPromo));
                    $arr[] = date('Y-m-d' , strtotime($fields->endPromo));
                    $arr[] = $fields->lastStatus;


                    $result[] = $arr;
                }

                if($request->entity == "0" || $request->entity == 0){
                    $entity = 'All';
                }else{
                    $entity = $request->entity;
                }

                $heading = [
                    ['Listing Promo Distributor'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo ID', 'Category', 'Brand', 'Distributor', 'Account', 'Sub Account', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Status' ]
                ];
                $formatCell =  [

                ];

                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A5:K5'; //Header Column Bold and color
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
}
