<?php

namespace App\Modules\Promo\Controllers;

use Barryvdh\Snappy\Facades\SnappyPdf;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Wording;

class PromoMultiPrint extends Controller
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
        $title = "Promo Multi Print";
        return view('Promo::promo-multi-print.index', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/promo/multiprint';

        $query = [
            'year'                    => $request->period,
            'entity'                  => (int) $request->entityId,
            'distributor'             => 0,
            'budgetparent'            => 0,
            'channel'                 => 0,
            'cancelstatus'            => 'false',
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
        $api = config('app.api'). '/dn/multiprint-promo/entity';
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
        $dataPromo = json_decode($request['id']);
        if ($dataPromo[0]->yearPromo >= 2025) {
            $api = config('app.api') . '/promo/displayv2/id';
            $viewPrint = 'Promo::promo-multi-print.printout-revamp-pdf';
        } else {
            $api = config('app.api') . '/promo/display/id';
            $viewPrint = 'Promo::promo-multi-print.printout-pdf';
        }

        try {
            $arrData = array();
            if (count($dataPromo) > 0) {
                for ($i = 0; $i < count($dataPromo); $i++)  {
                    $query = [
                        'id'  =>  $dataPromo[$i]->id,
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
                    $pdf = SnappyPdf::loadView($viewPrint, compact('arrData'))
                        ->setOption('page-size', 'letter')
                        ->inline('Promo_Multiprint_'.date('Y-m-d_H-i-s').'.pdf');
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
