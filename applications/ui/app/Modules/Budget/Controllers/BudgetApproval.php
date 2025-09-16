<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Wording;

class BudgetApproval extends Controller
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
        $title = "Budget Approval";
        return view('Budget::budget-approval.index', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/budget/approval';
        try {
            $query = [
                'year'          => $request->period,
                'entity'        => $request->entityId,
                'distributor'   => $request->distributorId,
                'channel'       => $request->channelId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "data" => json_decode($response)->values
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
                Log::warning($message);
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
        $api = config('app.api'). '/budget/approval/entity';
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

    public function getListDistributorByEntityId(Request $request)
    {
        $api = config('app.api'). '/budget/approval/distributor';
        $query = [
            'budgetId'  => 0,
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api'). '/budget/approval/channel';
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

    public function approve(Request $request) {
        $api = config('app.api') . '/budget/approval/approve';
        $data = [
            'budgetId'     => $request->id,
        ];

        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->getStatusCode() === 200) {
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

    public function unApprove(Request $request) {
        $api = config('app.api') . '/budget/approval/unapprove';
        $data = [
            'budgetId'      => $request->id,
            'notes'         => $request->notes,
        ];

        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->getStatusCode() === 200) {
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

    public function exportXls(Request $request)
    {
        $api = config('app.api'). '/budget/approval';
        $query = [
            'year'          => $request->period,
            'entity'        => $request->entityId,
            'distributor'   => $request->distributorId,
            'channel'       => $request->channelId,
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
                    $arr[] = $fields->periode;
                    $arr[] = $fields->refId;
                    $arr[] = $fields->longDesc;
                    $arr[] = $fields->budgetAmount;
                    $arr[] = $fields->distributorName;
                    $arr[] = (($fields->statusApproval === "AP2") ? "Approved" : "Waiting Approval");

                    $result[] = $arr;
                }

                $filename = 'BudgetApproval-';
                $title = 'A1:F1'; //Report Title Bold and merge
                $header = 'A5:F5'; //Header Column Bold and color
                $heading = [
                    ['Budget Approval'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Period', 'Ref ID', 'Description', 'Amount', 'Distributor', 'Status Approval']
                ];

                $formatCell =  [
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' .$mc . '.xlsx');
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
