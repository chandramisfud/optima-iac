<?php

namespace App\Http\Controllers;

use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;
use Wording;

class AppLog extends Controller
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
        $title = "Application Log";
        return view('log-apps.index', compact('title'));
    }

    public function getListLog (Request $request) {
        $storage = Storage::disk('log')->allFiles('/');
        $data = [];
        if ($storage) {
            for ($i=0; $i<count($storage); $i++) {
                if ($storage[$i] !== '.gitignore') {
                    if ($request->date) {
                        if (str_contains($storage[$i], $request->date)) {
                            array_push($data, [
                                'nameFile'  => $storage[$i]
                            ]);
                        }
                    } else {
                        array_push($data, [
                            'nameFile'  => $storage[$i]
                        ]);
                    }
                }
            }
        }

        return array(
            'error'     => false,
            'data'      => $data
        );
    }

    public function downloadLog (Request $request) {
        return Storage::disk('log')->download($request->f);
    }
}
