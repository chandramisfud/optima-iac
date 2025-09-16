<?php

namespace App\Modules\Tools\Controllers;

use App\Exports\Tools\ExportTemplateBudgetAdjustment;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Wording;

class BudgetAdjustment extends Controller
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
        $title = "Budget Adjustment";
        return view('Tools::budget-adjustment.index', compact('title'));
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/tools/budgetadjustment/entity' ;
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

    public function getDataBudgetMaster(Request $request)
    {
        $api = config('app.api'). '/tools/budgetadjustment/allocation' ;
        $query = [
            'period'   => $request->periode,
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

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/tools/upload/budgetadjustment';
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
                Log::info('status code upload budget adjustment :' . $response->status());
                if ($response->status() === 200) {
                    return json_encode([
                        'error' => false,
                        'code'      => json_decode($response)->code,
                        'message' => "Upload success",
                    ]);
                } elseif ($response->status() === 409) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'code'      => json_decode($response)->code,
                        'message'   => json_decode($response)->message,
                    ]);
                } else {
                    return array(
                        'error' => true,
                        'code'      => $response->status(),
                        'message'   => json_decode($response)->message,
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

    public function downloadTemplate(Request $request) {
        $api = config('app.api'). '/tools/budgetadjustment/hierarchy' ;

        $query = [
            'period'       => $request['period'],
            'entityId'     => $request['entityId'],
            'budgetName'   => urldecode($request['budgetName']),
        ];

        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->budgetParentDesc;
                    $arr[] = $fields->totalAssignmentAmount;
                    $arr[] = $fields->assignTo;
                    $arr[] = $fields->assignmentDesc;
                    $arr[] = $fields->assignmentAmount;
                    $arr[] = $fields->categoryDesc;
                    $arr[] = $fields->subCategoryDesc;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->subActivityDesc;
                    $arr[] = $fields->approval;

                    $result[] = $arr;
                }
                $header = 'A1:J1'; //Header Column Bold and color
                $heading = [
                    [
                        'BUDGET PARENT', 'TOTAL ASSIGNMENT AMOUNT', 'ASSIGN TO', 'ASSIGNMENT DESC', 'ASSIGNMENT_AMOUNT', 'CATEGORY', 'SUB CATEGORY', 'ACTIVITY', 'SUB ACTIVITY', 'Approval'
                    ]
                ];
                $formatCell =  [
                    'B' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'E' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'BudgetAdjustment-';
                $export = new ExportTemplateBudgetAdjustment($result, $heading, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error' => $response->status(),
                    'data' => [],
                    'message' => $message
                );
                Log::info('get API ' . $api);
                Log::warning($message);
                return $result;
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

}
