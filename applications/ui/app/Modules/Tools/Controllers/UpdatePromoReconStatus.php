<?php

namespace App\Modules\Tools\Controllers;

use App\Http\Controllers\Controller;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class UpdatePromoReconStatus extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage(): Factory|View|Application
    {
        $title = "Update Promo Reconciliation Status";
        return view('Tools::update-promo-recon-status.index', compact('title'));
    }

    public function uploadTemplate(Request $request): bool|string
    {
        $api = config('app.api') . '/tools/promorecon/status';
        Log::info('post API ' . $api);
        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);
                if ($response->status() === 200) {
                    return json_encode([
                        'error'     => false,
                        'message'   => "Upload success",
                    ]);
                } else if ($response->status() === 403) {
                    return json_encode([
                        'error'     => true,
                        'message'   => json_decode($response)->message,
                    ]);
                } else {
                    return json_encode([
                        'error'     => true,
                        'message'   => "Upload failed"
                    ]);
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => "File does not exist"
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => "Upload error"
            ]);
        }
    }

}
