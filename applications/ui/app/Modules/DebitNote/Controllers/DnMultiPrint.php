<?php

namespace App\Modules\DebitNote\Controllers;

use Barryvdh\Snappy\Facades\SnappyPdf;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Wording;

class DnMultiPrint extends Controller
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
        $title = "Multi Print Debit Note";
        return view('DebitNote::dn-multi-print.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/dn/multiprint';
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
                'EntityId'                  => 0,
                'DistributorId'             => 0,
                'BudgetParentId'            => 0,
                'ChannelId'                 => 0,
                'AccountId'                 => $request->subAccountId,
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

    public function getListSubAccount(Request $request)
    {
        $api = config('app.api'). '/dn/multiprint/subaccount';
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

    public function printPdf(Request $request) {
        $api = config('app.api') . '/dn/creation/print';
        $dataDN = json_decode($request->id);

        try {
            $arrData = array();
            if (count($dataDN) > 0) {
                for ($i = 0; $i < count($dataDN); $i++)  {
                    $query = [
                        'id'  =>  $dataDN[$i],
                    ];

                    Log::info('Get API ' . $api);
                    Log::info('Payload ' . json_encode($query));
                    $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
                    $resVal = json_decode($response)->values;
                    $data = json_encode($resVal);
                    $message = json_decode($response)->message;
                    if ($response->status() === 200) {
                        if ($data) {
                            array_push($arrData, $data);
                        } else {
                            Log::info($api ." no Data");
                            return array(
                                'error'     => true,
                                'data'      => [],
                                'message'   => 'no Data'
                            );
                        }
                    } else {
                        return array(
                            'error'     => true,
                            'data'      => [],
                            'message'   => $message
                        );
                    }
                }
                if (count($arrData) > 0 ) {
                    $pdf = SnappyPdf::loadView('DebitNote::dn-multi-print.printout-pdf-dn', compact('arrData'))
                        ->setOption('page-size', 'letter')
                        ->inline('DN_'.date('Y-m-d_H-i-s').'.pdf');
                    return $pdf;
                }
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
}
