<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Barryvdh\Snappy\Facades\SnappyPdf;
use Illuminate\Support\Facades\Storage;
use Session;
use File;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Wording;
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

    private function encCategoryShortDesc ($categoryShortDesc)
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }

    public function landingPage()
    {
        $title = "Promo Display";
        return view('FinanceReport::promo-display.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/promodisplay';
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
                'profileId'                 => 0,
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'BudgetParentId'            => 0,
                'ChannelId'                 => 0,
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
                $resVal = json_decode($response)->values->data;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
                }
                return json_encode([
                    "draw" => (int) $request->draw,
                    "data" => $resVal,
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

    public function formPage(Request $request): \Illuminate\Contracts\View\Factory|\Illuminate\Contracts\View\View|\Illuminate\Contracts\Foundation\Application
    {
        $category = MyEncrypt::decrypt($request['c']);
        if($category === 'DC') {
            $title = "Promo Display (Distributor Cost)";
            if ($request['sy'] >= 2025 ) {
                if (!$request['recon']) {
                    return view('FinanceReport::promo-display.dc-form-revamp', compact('title'));
                } else {
                    $title = "Promo Reconciliation Form (Distributor Cost)";
                    return view('FinanceReport::promo-display.dc-form-recon', compact('title'));
                }
            } else {
                return view('FinanceReport::promo-display.dc-form', compact('title'));
            }
        } else {
            $title = "Promo Display (Retailer Cost)";
            if ($request['sy'] >= 2025 ) {
                if (!$request['recon']) {
                    return view('FinanceReport::promo-display.form-revamp', compact('title'));
                } else {
                    $title = "Promo Reconciliation Form (Retailer Cost)";
                    return view('FinanceReport::promo-display.form-recon', compact('title'));
                }
            } else {
                return view('FinanceReport::promo-display.form', compact('title'));
            }
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

    public function getDataByID(Request $request): array
    {
        $api = config('app.api'). '/finance-report/promodisplay/id';
        $query = [
            'id'  => $request->id,
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

    public function getListEntity(): array
    {
        $api = config('app.api'). '/finance-report/promodisplay/entity';
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

    public function getListDistributorByEntityId(Request $request): array
    {
        $api = config('app.api'). '/finance-report/promodisplay/distributor';
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

    public function viewFile(Request $request): \Illuminate\Contracts\View\Factory|\Illuminate\Contracts\View\View|\Illuminate\Contracts\Foundation\Application
    {
        $promoId = $request->promoId;
        $docLink = $request->docLink;

        $pathPromo = '/assets/media/promo/' . $promoId . '/' . $docLink . '/';
        $filename = $request->fileName;
        Log::info('preview path ' . $pathPromo);
        Log::info('preview filename ' . $filename);

        return view('FinanceReport::promo-display.view', compact('promoId', 'pathPromo', 'filename'));
    }

    public function previewAttachment(Request $request): \Illuminate\Contracts\View\Factory|\Illuminate\Contracts\View\View|\Illuminate\Contracts\Foundation\Application
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-display.attachment-preview", compact('path', 'isExist', 'title'));
    }

    public function exportPdf(Request $request): \Illuminate\Http\Response|array
    {
        $yearPromo = $request['sy'];
        if ($yearPromo > '2024') {
            $api = config('app.api'). '/finance-report/promodisplaypdf/id';
            if ($request['statusMechanismList'] === 'true'){
                $viewPdfPromo = 'FinanceReport::promo-display.printout-revamp-mechanism-list-pdf';
            } else {
                $viewPdfPromo = 'FinanceReport::promo-display.printout-revamp-mechanism-text-pdf';
            }
        } else {
            $api = config('app.api'). '/finance-report/promodisplay/export-pdf';
            $viewPdfPromo = 'FinanceReport::promo-display.printout-pdf';
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
}
