<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use Barryvdh\Snappy\Facades\SnappyPdf;
use Illuminate\Support\Facades\View;
use Session;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class InvoiceCreation extends Controller
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
        $title = "Invoice Creation";
        return view('DebitNote::invoice-creation.index', compact('title'));
    }

    public function formPage()
    {
        $title = "Invoice Creation";
        return view('DebitNote::invoice-creation.form', compact('title'));
    }

    public function rejectPage()
    {
        $title = "Invoice Creation [Reject]";
        return view('DebitNote::invoice-creation.reject', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = '/dn/create-invoice/invoice';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'CreateDate'                => $request->filter_date,
            'Entity'                    => ($request->entityId ?? 0),
            'Distributor'               => ($request->distributorId ?? 0),
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/dn/create-invoice/entity';

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

    public function getListCategory(): bool|string
    {
        $api = '/dn/create-invoice/category';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getDataDistributorCompanyName(Request $request)
    {
        $api = config('app.api'). '/dn/create-invoice/userprofile/id';
        $query = [
            'id'  => $request->session()->get('profile'),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->distributorlist
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
        $api = config('app.api'). '/dn/create-invoice/userprofile/id';
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

    public function getListTaxLevel(Request $request)
    {
        $api = config('app.api'). '/dn/create-invoice/taxlevel';
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

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/create-invoice/invoice/id';
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

    public function getDataDnByTaxlevel(Request $request)
    {
        $api = config('app.api'). '/dn/create-invoice/invoice-taxlevel';

        $category = 0;
        if ($request['categoryId']) {
            $category =  ((int)$request['dnPeriod'] < 2024 ) ? 0 : $request['categoryId'];
        }

        $query = [
            'entityid'      => ($request->entityId ?? 0),
            'distributorid' => $this->getDistributorId($request->session()->get('profile')),
            'TaxLevel'      => ($request->TaxLevel ?? 0),
            'dnPeriod'      => ($request['dnPeriod'] ?? '0'),
            'categoryId'    => $category
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'         => false,
                    'data'          => json_decode($response)->values,
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
        $api = config('app.api') . '/dn/create-invoice';
        $category = 0;
        if ($request['categoryId']) {
            $category =  ((int)$request['dnPeriod'] < 2024 ) ? 0 : $request['categoryId'];
        }
        $data = [
            'desc'              => $request->invoiceDesc,
            'dppAmount'         => $request->dppAmount,
            'ppNpct'            => $request->ppn,
            'invoiceAmount'     => $request->invoiceAmount,
            'distributorId'     => $this->getDistributorId($request->session()->get('profile')),
            'dnPeriod'          => $request['dnPeriod'],
            'entityId'          => $request->entityId,
            'categoryId'        => $category,
            'taxLevel'          => $request->taxLevel,
            'dnid'              => json_decode($request->dnid),
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
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

    public function update(Request $request) {
        $api = config('app.api') . '/dn/create-invoice';
        $category = 0;
        if ($request['categoryId']) {
            $category =  ((int)$request['dnPeriod'] < 2024 ) ? 0 : $request['categoryId'];
        }
        $data = [
            'invoiceId'         => $request->id,
            'desc'              => $request->invoiceDesc,
            'dppAmount'         => $request->dppAmount,
            'ppNpct'            => $request->ppn,
            'invoiceAmount'     => $request->invoiceAmount,
            'distributorId'     => $this->getDistributorId($request->session()->get('profile')),
            'dnPeriod'          => $request['dnPeriod'],
            'entityId'          => $request->entityId,
            'categoryId'        => $category,
            'taxLevel'          => $request->taxLevel,
            'dnid'              => json_decode($request->dnid),
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
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

    public function reject(Request $request) {
        $api = config('app.api') . '/dn/create-invoice/reject';
        $data = [
            'dnid'              => $request->dnid,
            'reason'            => $request->notes,
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Reject DN Success'
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

    public function printPdf(Request $request) {
        $api = config('app.api'). '/dn/create-invoice/print-invoice/id';
        $query = [
            'id'  => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $user_print = $request->session()->get('name');

                $footer = View::make('DebitNote::invoice-creation.printout-pdf-footer', compact('user_print'));

                $pdf = SnappyPdf::loadView('DebitNote::invoice-creation.printout-pdf', compact('data'))
                    ->setOption('footer-html', $footer)
                    ->inline('Invoice_'.date('Y-m-d_H-i-s').'.pdf');
                return $pdf;
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

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/dn/create-invoice/filter';
        Log::info('post API ' . $api);

        $data_multipart = [];
        array_push($data_multipart, [
            'name'  => 'status',
            'contents' => 'ready_to_invoice'
        ], [
            'name'  => 'entity',
            'contents' => $request['entity']
        ], [
            'name'  => 'TaxLevel',
            'contents' => $request['taxLevel']
        ], [
            'name'  => 'invoiceId',
            'contents' => ($request['invoiceId'] ?? 0)
        ], [
            'name'  => 'dnPeriod',
            'contents' => ($request['dnPeriod'] ?? 0)
        ], [
            'name'  => 'categoryId',
            'contents' => ($request['categoryId'] ?? 0)
        ], [
            'name'     => 'formFile',
            'contents' => $request->file('file')->getContent(),
            'filename' => $request->file('file')->getClientOriginalName()
        ]);

        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, $data_multipart);
                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'message'   => "Upload success",
                    ]);
                } else {
                    Log::warning($response->body());
                    return array(
                        'error' => true,
                        'message' => "Upload failed"
                    );
                }
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }
}
