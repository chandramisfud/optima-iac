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

class BudgetAssignment extends Controller
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
        $title = "Budget Assignment";
        return view('Budget::budget-assignment.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/budget/assignment';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $page = $page;
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Search'                    => $search,
                'PageNumber'                => $page,
                'PageSize'                  => (int) $length,
                'SortColumn'                => $SortColumn,
                'SortDirection'             => $sort,
                'year'                      => $request->period,
                'entity'                    => $request->entityId,
                'distributor'               => $request->distributorId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "draw" => (int) $request->draw,
                    "data" => json_decode($response)->values->data,
                    "recordsTotal" => json_decode($response)->values->totalCount,
                    "recordsFiltered" => json_decode($response)->values->filteredCount
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

    public function budgetAssignmentFormPage()
    {
        $title = "Budget Assignment";
        return view('Budget::budget-assignment.form', compact('title'));
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/budget/assignment/entity';
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
        $api = config('app.api'). '/budget/assignment/distributor';
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

    public function getListBudgetSource(Request $request)
    {
        $api = config('app.api'). '/budget/assignment/allocation';
        $query = [
            'year'          => $request->period,
            'entity'        => $request->entityId,
            'distributor'   => $request->distributorId,
            'PageSize'      => -1,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->data
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

    public function getDataBudgetSourceById(Request $request)
    {
        $api = config('app.api'). '/budget/assignment/allocation/id';
        $query = [
            'id'  => $request->id,
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

    public function save(Request $request)
    {
        $api = config('app.api') . '/budget/assignment';
        $data = [
            'budgetId'          => $request->budgetId,
            'frownId'           => $request->frownId,
            'allocationId'      => $request->allocationId,
            'budgetAmount'      => str_replace(',', '', $request->budgetAssigned),
            'assignmentDetail'  => json_decode($request->assignmentDetail),
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'Save success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => "error : Save Failed"
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Save Failed"
            );
        }
    }

    public function getDataByID(Request $request) {
        $api = config('app.api'). '/budget/assignment/id';
        $query = [
            'id'           => $request->id,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data'  => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
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

    public function update(Request $request) {
        $api = config('app.api') . '/budget/assignment';
        $data = [
            'assignmentId'      => $request->id,
            'assignmentDetail'  => json_decode($request->assignmentDetail),
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Update success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('put API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('put API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Update Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/budget/assignment';
        $query = [
            'year'                      => $request->period,
            'entity'                    => $request->entityId,
            'distributor'               => $request->distributorId,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'refId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->refId;
                    $arr[] = $fields->frownId . " - " . $fields->frownName;
                    $arr[] = $fields->budgetAmount;

                    $result[] = $arr;
                }

                $filename = 'BudgetAssignment-';
                $title = 'A1:C1'; //Report Title Bold and merge
                $header = 'A5:C5'; //Header Column Bold and color
                $heading = [
                    ['Budget Assignment'],
                    ['Budget Year : ' . $request->period],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Ref (Budget Parent)', 'From Owner ID', 'Amount']
                ];

                $formatCell =  [
                    'C' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d H:i:s') . ' ' .$mc . '.xlsx');
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
