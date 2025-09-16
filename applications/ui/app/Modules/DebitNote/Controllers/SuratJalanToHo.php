<?php

namespace App\Modules\DebitNote\Controllers;

use Barryvdh\Snappy\Facades\SnappyPdf;
use Illuminate\Support\Facades\View;
use Session;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class SuratJalanToHo extends Controller
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
        $title = "Surat Jalan [to HO]";
        return view('DebitNote::surat-jalan-to-ho.index', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/dn/suratjalan-tandaterima-toho';
        $query = [
            'senddate' => $request->senddate
        ];

        Log::info('Get API ' . $api);
        Log::info('Payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api,$query);
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
        $api = config('app.api'). '/dn/suratjalan-tandaterima-toho/id';
        $query = [
            'id'  => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $data->user_print = $request->session()->get('profile');

                $header = View::make('DebitNote::surat-jalan-to-ho.printout-pdf-header', compact('data'));
                $footer = View::make('DebitNote::surat-jalan-to-ho.printout-pdf-footer', compact('data'));

                $pdf = SnappyPdf::loadView('DebitNote::surat-jalan-to-ho.printout-pdf', compact('data'))
                    ->setOption('header-html', $header)
                    ->setOption('footer-html', $footer)
                    ->setOption('footer-center', 'Halaman [page] dari [topage]')
                    ->setOrientation('landscape')
                    ->inline('DN_'.date('Y-m-d_H-i-s').'.pdf');
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
