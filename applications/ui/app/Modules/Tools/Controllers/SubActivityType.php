<?php

namespace App\Modules\Tools\Controllers;

use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class SubActivityType extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage()
    {
        return view('Tools::sub-activity-type.index');
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/tools/upload/subactivitytype';
        Log::info('post API ' . $api);

        $data_multipart = [];
        array_push($data_multipart, [
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
                        'error' => false,
                        'message' => "Upload success",
                    ]);
                } else {
                    $message = $response->message;
                    Log::error($message);
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
