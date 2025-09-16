<?php

namespace App\Modules\Promo\Controllers;

use App\Helpers\CallApi;
use Barryvdh\Snappy\Facades\SnappyPdf;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;

class PromoWorkflow extends Controller
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
        $title = "Promo ID Workflow";
        return view('Promo::promo-workflow.index', compact('title'));
    }

    public function getDataPromo(Request $request)
    {
        $api = config('app.api'). '/promo/workflow';
        $query = [
            'RefId'       => $request->refId,
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

    public function getDataPromoByRefId(Request $request): bool|string
    {
        $api = '/promo/workflowv2';
        $callApi = new CallApi();
        $query = [
            'refid'            => $request['refid'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListWorkflowHistory(Request $request)
    {
        $api = config('app.api'). '/promo/workflow/history';
        $query = [
            'refId'       => $request->refId,
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

    public function getListChangeHistory(Request $request)
    {
        $api = config('app.api'). '/promo/workflow/changes';
        $query = [
            'refId'       => $request->refId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                if (json_decode($response)->code === 200) {
                    return array(
                        'error'     => false,
                        'data'      => json_decode($response)->values
                    );
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                    );
                }
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

    public function getListDN(Request $request)
    {
        $api = config('app.api'). '/promo/workflow/dn';
        $query = [
            'refId'       => $request->refId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                if (json_decode($response)->code === 200) {
                    return array(
                        'error'     => false,
                        'data'      => json_decode($response)->values
                    );
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                    );
                }
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

    public function getPromoApprovalWorkflow(Request $request)
    {
        $api = config('app.api'). '/promo/workflow/timeline';
        $query = [
            'refId'       => $request['refId'],
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                if (json_decode($response)->code === 200) {
                    return array(
                        'error'     => false,
                        'data'      => json_decode($response)->values
                    );
                } else {
                    return array(
                        'error'     => true,
                        'data'      => [],
                    );
                }
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

    public function printPdf(Request $request)
    {
        $yearPromo = $request['sy'];
        if ($yearPromo >= 2025) {
            $api = config('app.api'). '/promo/workflowv2pdf/id';
            if ($request['statusMechanismList'] === 'true'){
                $viewPdfPromo = 'Promo::promo-workflow.printout-pdf-promo-revamp-mechanism-list';
            } else {
                $viewPdfPromo = 'Promo::promo-workflow.printout-pdf-promo-revamp-mechanism-text';
            }
        } else {
            $api = config('app.api'). '/promo/workflow/id';
            $viewPdfPromo = 'Promo::promo-workflow.printout-pdf-promo';
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

    public function previewAttachment(Request $request)
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-display.attachment-preview", compact('path', 'isExist', 'title'));
    }
}
