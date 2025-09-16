<?php

namespace App\Modules\Promo\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Barryvdh\Snappy\Facades\SnappyPdf;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;

use Wording;
use File;
use PDF;


class PromoDisplay extends Controller
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
        $title = "Promo Display";
        return view('Promo::promo-display.index', compact('title'));
    }

    public function form(Request $request)
    {
        $category = MyEncrypt::decrypt($request['c']);

        if($category === 'DC') {
            $title = "Promo Display (Distributor Cost)";
            if ($request['sy'] >= 2025 ) {
                if (!$request['recon']) {
                    return view('Promo::promo-display.dc-form-revamp', compact('title'));
                } else {
                    $title = "Promo Reconciliation Form (Distributor Cost)";
                    return view('Promo::promo-display.dc-form-recon', compact('title'));
                }
            } else {
                return view('Promo::promo-display.dc-form', compact('title'));
            }
        } else {
            $title = "Promo Display (Retailer Cost)";
            if ($request['sy'] >= 2025 ) {
                if (!$request['recon']) {
                    return view('Promo::promo-display.form-revamp', compact('title'));
                } else {
                    $title = "Promo Reconciliation Form (Retailer Cost)";
                    return view('Promo::promo-display.form-recon', compact('title'));
                }
            } else {
                return view('Promo::promo-display.form', compact('title'));
            }
        }
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/promo/display';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $query = [
                'year'              => $request->period,
                'entity'            => $request->entityId,
                'budgetParent'      => 0,
                'channel'           => 0,
                'distributor'       => $request->distributorId,
                'Search'            => $search,
                'PageNumber'        => $page,
                'PageSize'          => (int) $length,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
                }
                return json_encode([
                    "draw" => (int) $request->draw,
                    "data" => $resVal,
                    "recordsTotal" => json_decode($response)->values->recordsTotal,
                    "recordsFiltered" => json_decode($response)->values->recordsFiltered
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

    public function getDataV2ById(Request $request): bool|string
    {
        $api = '/promo/displayv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataById(Request $request)
    {
        $api = config('app.api'). '/promo/display/id';
        $query = [
            'id'        => $request->promoId,
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api') . '/promo/entity';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListDistributor(Request $request)
    {
        $api = config('app.api') . '/promo/distributor';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request->entityId);

            $query = [
                'budgetId' => 0,
                'entityId' => $ar_parent,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListSubactivityType(Request $request)
    {
        $api = config('app.api'). '/promo/subcategory/categoryid';
        $query = [
            'CategoryId'  => $request['CategoryId'],
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

    public function previewAttachment(Request $request)
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-display.attachment-preview", compact('path', 'isExist', 'title'));
    }

    public function exportPdf(Request $request) {
        $yearPromo = $request['sy'];

        if ($yearPromo >= 2025) {
            $api = config('app.api'). '/promo/displayv2pdf/id';
            if ($request['statusMechanismList'] === 'true'){
                $viewPdfPromo = 'Promo::promo-display.printout-revamp-mechanism-list-pdf';
            } else {
                $viewPdfPromo = 'Promo::promo-display.printout-revamp-mechanism-text-pdf';
            }
        } else {
            $api = config('app.api') . '/promo/display/id';
            $viewPdfPromo = 'Promo::promo-display.printout-pdf';
        }

        $query = [
            'id'  => $request['id'],
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $pdf = SnappyPdf::loadView($viewPdfPromo, compact('data'))
                    ->setOption('page-size', 'letter')
                    ->inline('Promo_'.date('Y-m-d_H-i-s').'.pdf');
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

    public function encCategoryShortDesc ($categoryShortDesc)
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }
}
