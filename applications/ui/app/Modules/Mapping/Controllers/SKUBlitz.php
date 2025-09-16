<?php

namespace App\Modules\Mapping\Controllers;

use App\Exports\Export;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class SKUBlitz extends Controller
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
        $title = "Mapping SKU Blitz";
        return view('Mapping::sku-blitz.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/mapping/sku-blitz';
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
                'SortDirection'             => $sort
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

    public function SKUBlitzFormPage (Request $request)
    {
        $title = "Mapping SKU Blitz";
        return view('Mapping::sku-blitz.form', compact('title'));
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/mapping/sku-blitz/entity';
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

    public function getListBrandByEntityId(Request $request)
    {
        $api = config('app.api'). '/mapping/sku-blitz/brand';
        $data = [
            'EntityId'           => $request->entityId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $data);
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

    public function getListSKUByBrandId(Request $request)
    {
        $api = config('app.api'). '/mapping/sku-blitz/sku';
        $data = [
            'BrandId'           => $request->brandId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $data);
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
        $api = config('app.api') . '/mapping/sku-blitz';
        $data = [
            'skuId'         => $request->skuId,
            'sapCode'       => $request->sapCode,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Save success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
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

    public function delete(Request $request)
    {
        $api = config('app.api') . '/mapping/sku-blitz';
        $data = [
            'id'           => $request->id
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Mapping Removed",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => "Remove Failed"
                );
            }
        } catch (\Exception $e) {
            Log::error('delete API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Remove Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/mapping/sku-blitz/download';
        Log::info('get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->entity;
                    $arr[] = $fields->brand;
                    $arr[] = $fields->sku;
                    $arr[] = $fields->sapCode;
                    $arr[] = (($fields->createOn) ? date('Y-m-d H:i:s' , strtotime($fields->createOn)) : null);
                    $arr[] = $fields->createBy;
                    $arr[] = (($fields->deleteOn) ? date('Y-m-d H:i:s' , strtotime($fields->deleteOn)) : null);
                    $arr[] = $fields->deleteBy;

                    $result[] = $arr;
                }

                $filename = 'Mapping SKU Blitz -';
                $title = 'A1:H1'; //Report Title Bold and merge
                $header = 'A3:H3'; //Header Column Bold and color
                $heading = [
                    ['Mapping SKU Blitz'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Brand', 'SKU', 'Blitz SKU Code', 'Created On', 'Created By', 'Removed On', 'Removed By']
                ];

                $formatCell =  [

                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' . $mc .  '.xlsx');
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
