<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class ListingDnOverBudget extends Controller
{
    private mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    private function encCategoryId($categoryId): string
    {
        try {
            return MyEncrypt::encrypt($categoryId);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }

    public function landingPage()
    {
        $title = "Debit Note Over Budget [Reassignment]";
        return view('DebitNote::listing-dn-over-budget.index', compact('title'));
    }

    public function landingPageToBeSettled()
    {
        $title = "DN Over Budget";
        return view('DebitNote::listing-dn-over-budget.to-be-settled', compact('title'));
    }

    public function formPage()
    {
        $title = "Debit Note Over Budget [Reassignment]";
        return view('DebitNote::listing-dn-over-budget.form', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = '/dn/listing-over-budget';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'periode'                   => $request->period,
            'entityId'                  => ($request->entityId ?? 0),
            'distributorId'             => (int) $this->getDistributorId($request->session()->get('profile')),
            'channelId'                 => ($request->channelId ?? "0"),
            'accountId'                 => ($request->accountId ?? "0"),
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
                "recordsTotal" => json_decode($res)->data->recordsTotal,
                "recordsFiltered" => json_decode($res)->data->recordsFiltered
            ]);
        } else {
            return $res;
        }
    }

    public function getListToBeSettled(Request $request)
    {
        $api = '/dashboard/main/dn/overbudget/tobesettled';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'search'                    => ($request['search']['value'] ?? ""),
            'sortColumn'                => $request['columns'][$request['order'][0]['column']]['data'],
            'sortDirection'             => $request['order'][0]['dir'],
            'pageSize'                  => $request['length'],
            'pageNumber'                => $page,
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->data;
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => $resVal,
                "recordsTotal" => json_decode($res)->data->recordsTotal,
                "recordsFiltered" => json_decode($res)->data->recordsFiltered
            ]);
        } else {
            return $res;
        }
    }

    public function getDataToBeSettledByPromoId(Request $request)
    {
        $api = config('app.api'). '/dashboard/main/dn/overbudget/tobesettled/promoid';
        $query = [
            'promoId'       => $request->promoId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return array(
                    'error'     => false,
                    'data'      => $resVal,
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

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/listing-over-budget/id';
        $query = [
            'id'       => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function getDataPromo(Request $request)
    {
        $api = config('app.api'). '/dn/listing-over-budget/approvedpromo-for-dn';
        $query = [
            'periode'       => $request->period,
            'entity'      => ($request->entityId ?? 0),
            'channel'     => ($request->channelId ?? 0),
            'account'     => ($request->accountId ?? 0),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function save(Request $request) {
        $api = config('app.api') . '/dn/listing-over-budget/assign';
        $data = [
            'dnId'        => $request->dnId,
            'promoId'     => $request->promoId,
            'userId'      => $request->session()->get('profile'),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }
}
