<?php

namespace App\Modules\Configuration\Controllers;

use App\Exports\Configuration\ExportTemplateMechanismInput;
use App\Http\Controllers\Controller;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Request;
use App\Helpers\CallApi;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class MechanismInputMethod extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage(): Factory|View|Application
    {
        $title = "Mechanism Input Method Configuration";
        return view('Configuration::mechanism-input-method.index', compact('title'));
    }

    public function getList(): array
    {
        $api = '/config/mechanisminputmethod';
        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api);
        if (!json_decode($res)->error) {
            $data = json_decode($res)->data;

            return [
                'data'  =>  $data
            ];
        } else {
            return [
                'data'  => []
            ];
        }
    }

    public function downloadTemplate(): BinaryFileResponse|array
    {
        try {
            $api = '/config/mechanisminputmethod';
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;

                $result=[];
                foreach ($data as $fields) {
                    $arr = [];
                    $arr[] = $fields->categoryDesc;
                    $arr[] = $fields->subCategoryDesc;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->subActivityDesc;
                    $arr[] = $fields->inputMethod;

                    $result[] = $arr;
                }

                $filename = 'Template Mechanism Input';
                $header = 'A1:E1'; //Header Column Bold and color
                $heading = [
                    ['Category', 'Sub Category', 'Activity', 'Sub Activity', 'Using Mechanism List']
                ];
                $export = new ExportTemplateMechanismInput($result, $heading, $header);
                return Excel::download($export, $filename . '.xlsx');
            } else {
                return [
                    'data'  => []
                ];
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return [];
        }
    }

    public function upload(Request $request): bool|string
    {
        $api = config('app.api') . '/config/mechanisminputmethod/upload';
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
