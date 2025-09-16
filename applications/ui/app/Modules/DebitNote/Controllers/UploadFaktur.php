<?php

namespace App\Modules\DebitNote\Controllers;

use Session;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class UploadFaktur extends Controller
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
        $title = "Debit Note";
        return view('DebitNote::upload-faktur.index', compact('title'));
    }

    public function upload(Request $request)
    {
        $api = config('app.api') . '/dn/upload-faktur';
        Log::info('post API ' . $api);
        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);
                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'message'   => "Upload success",
                    ]);
                } else {
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
