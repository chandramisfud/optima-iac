<?php

namespace App\Modules\References\Controllers;

use Illuminate\Support\Facades\Storage;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class Distributor extends Controller
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
        $title = "References";
        return view('References::distributor.index', compact('title'));
    }

    public function checkFile(Request $request)
    {
        try {
            $path = 'assets/media/references/803/' . $request->distributorId . '/' .$request->row;
            if (!Storage::disk('optima')->exists($path)) {
                Storage::disk('optima')->makeDirectory($path);
            }
            $files = Storage::disk('optima')->files($path);
            if (count($files) > 0) {
                return json_encode([
                    'error'     => false,
                    'files'     => $files
                ]);
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => "File doesn't exist"
                ]);
            }

        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function getListDistributor(Request $request)
    {
        $api = config('app.api'). '/references/distributor';
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

    public function upload(Request $request)
    {
        $path = 'assets/media/references/803/' . $request->distributorId . '/' . $request->row;
        try {
            Storage::disk('optima')->move($path, $path.'_temp');

            if (!Storage::disk('optima')->exists($path)) {
                Storage::disk('optima')->makeDirectory($path);
            }
            if (Storage::disk('optima')->put($path . '/' . $request->file('file')->getClientOriginalName(), file_get_contents($request->file))) {
                Storage::disk('optima')->deleteDirectory($path.'_temp');
                return json_encode([
                    'error'     => false,
                    'message'   => 'Upload Success'
                ]);
            } else {
                Storage::disk('optima')->deleteDirectory($path);
                Storage::disk('optima')->move($path.'_temp', $path);
                return json_encode([
                    'error'     => true,
                    'message'   => "Upload Failed"
                ]);
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => $e->getMessage()
            ]);
        }
    }
}
