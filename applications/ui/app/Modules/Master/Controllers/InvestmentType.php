<?php

namespace App\Modules\Master\Controllers;

use App\Exports\ExportMasterInvestmentType;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use App\Exports\Export;
use App\Exports\Master\ExportViewMasterMappingInvestmentType;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

use Wording;

class InvestmentType extends Controller
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
        return view('Master::investment-type.index');
    }

    public function getListPaginateFilter(Request $request) {
        $api = config('app.api'). '/master/investmenttype';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $i=$request['order'][0]['column'];
            $order = $request['columns'][$i]['data'];
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            if($request['search']['value']==null){ $search=""; }else{ $search=$request['search']['value']; }
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
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => $response->status(),
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function investmentTypeFormPage(Request $request) {
        return view('Master::investment-type.form');
    }

    public function getListMapping(Request $request)
    {
        $api = config('app.api'). '/master/investmenttype/mapping';
        $query = [
            'activityid'        => $request->activityId,
            'subactivity'       => $request->subActivityId,
            'categoryid'        => $request->categoryId,
            'subcategoryid'     => $request->subCategoryId,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $data = json_decode($response)->values;
                return array(
                    'error'     => false,
                    'data'      => $data
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

    public function investmentTypeMappingPage(Request $request) {
        $title = "Investment Type";
        return view('Master::investment-type.mapping', compact('title'));
    }

    public function getListCategory(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping/category';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getListInvestmentType(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping/investmenttype';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getDataSubCategory(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping/subcategory';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'CategoryId'  => $request->CategoryId
            ]);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getDataActivity(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping/activity';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'subCategoryId'  => $request->subCategoryId
            ]);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getDataSubActivity(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping/subactivity';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'ActivityId'  => $request->ActivityId
            ]);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getDataByID(Request $request) {
        $api = config('app.api'). '/master/investmenttype/id';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'id'  => $request->id
            ]);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function saveMapping(Request $request) {
        $api = config('app.api') . '/master/investmenttype/mapping';
        $data = [
            'investmentMap'     => json_decode($request->investmentMap),
            'userid'            => '0'
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

    public function removeMapping(Request $request) {
        $api = config('app.api') . '/master/investmenttype/mapping';
        $data = [
            'investmentMap'     => json_decode($request->investmentMap),
            'userid'            => '0'
        ];
        Log::info('remove API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Remove Investment Type Mapping success",
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
                'message'   => "error : Remove Investment Type Mapping failed"
            );
        }
    }

    public function save(Request $request) {
        $api = config('app.api') . '/master/investmenttype';
        $data = [
            'refId'     => $request->refId,
            "longDesc"  => $request->longDesc,
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

    public function update(Request $request) {
        $api = config('app.api') . '/master/investmenttype';
        $data = [
            'id'        => $request->id,
            'refId'     => $request->refId,
            "longDesc"  => $request->longDesc,
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function deactivate(Request $request) {
        $api = config('app.api') . '/master/investmenttype';
        $data = [
            'id'            => $request['id']
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'Deactivate success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('deactivate API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('deactivate API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Deactivate Failed"
            );
        }
    }

    public function activate(Request $request) {
        $api = config('app.api') . '/master/investmenttype/activate';
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, [
                'id'  => $request->id
            ]);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'Activate success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('activate API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('activate API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Activate Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/master/investmenttype';
        $query = [
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
                    $arr[] = $fields->longDesc;

                    if($fields->isDeleted === 0){
                        $status = 'Active';
                    } else {
                        $status = 'Inactive';
                    }
                    $arr[] = $status;

                    if ($fields->isDeleted === 1){
                        if($fields->deleteOn == null || $fields->deleteOn == ''){
                            $updateOn = '';
                        } else {
                            $updateOn = date('Y-m-d' , strtotime($fields->deleteOn));
                        }
                    } else {
                        if($fields->modifiedOn == null || $fields->modifiedOn == ''){
                            $updateOn = '';
                        } else {
                            $updateOn = date('Y-m-d' , strtotime($fields->modifiedOn));
                        }
                    }
                    $arr[] = $updateOn;

                    if ($fields->isDeleted === 1){
                        if($fields->deleteBy == null || $fields->deleteBy == ''){
                            $modifiedBy = '';
                        } else {
                            $modifiedBy = $fields->deleteBy;
                        }
                    } else {
                        if($fields->modifiedBy == null || $fields->modifiedBy == ''){
                            $modifiedBy = '';
                        } else {
                            $modifiedBy = $fields->modifiedBy;
                        }
                    }
                    $arr[] = $modifiedBy;

                    $result[] = $arr;
                }

                $filename = 'Master Investment Type - ';
                $title = 'A1:E1'; //Report Title Bold and merge
                $header = 'A3:E3'; //Header Column Bold and color
                $heading = [
                    ['Master Investment Type'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Investment Type Code', 'Investment Type', 'Status', 'Status Update On', 'Status Update By']
                ];

                $formatCell =  [

                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
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

    public function mappingExportXls(Request $request) {
        $api = config('app.api'). '/master/investmenttype/mapping';
        $query = [
            'activityid'        => (int) (($request->activity == '' || $request->activity == null) ? 0 : $request->activity),
            'subactivity'       => (int) (($request->subActivity == '' || $request->subActivity == null) ? 0 : $request->subActivity),
            'categoryid'        => (int) (($request->category == '' || $request->category == null) ? 0 : $request->category),
            'subcategoryid'     => (int) (($request->subCategory == '' || $request->subCategory == null) ? 0 : $request->subCategory)
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
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subactivity;
                    $arr[] = $fields->investmentTypeCode_bfr;
                    $arr[] = $fields->investmentType_bfr;
                    $arr[] = $fields->investmentTypeCode;
                    $arr[] = $fields->investmentType;
                    if ($fields->createOn == '0001-01-01T00:00:00') {
                        $arr[] = '';
                    } else {
                        $arr[] = date('d-m-Y', strtotime($fields->createOn));
                    }
                    $arr[] = $fields->createBy;

                    $result[] = $arr;
                }

                $filename = 'MappingInvestmentType-';
                $title = 'A1:J1'; //Report Title Bold and merge
                $header = 'A5:J5'; //Header Column Bold and color
                $merge1 = 'E4:F4';
                $merge2 = 'G4:H4';
                $heading = [
                    ['Mapping Investment Type'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [],
                    ['', '', '', '', 'Before', '', 'After', ''],
                    ['Category', 'Sub Category', 'Activity', 'Sub Activity', 'Investment Type Code', 'Investment Type', 'Investment Type Code', 'Investment Type','Last Update On', 'Last Update By']
                ];

                $formatCell =  [
                    'E' => NumberFormat::FORMAT_TEXT,
                    'G' => NumberFormat::FORMAT_TEXT,
                ];

                $export = new ExportMasterInvestmentType($result, $heading, $title, $header, $merge1, $merge2, $formatCell);
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

//    public function mappingExportXls(Request $request)
//    {
//        $api = config('app.api'). '/master/investmenttype/mapping';
//        try {
//            $query = [
//                'activityid'        => (int) (($request->activity == '' || $request->activity == null) ? 0 : $request->activity),
//                'subactivity'       => (int) (($request->subActivity == '' || $request->subActivity == null) ? 0 : $request->subActivity),
//                'categoryid'        => (int) (($request->category == '' || $request->category == null) ? 0 : $request->category),
//                'subcategoryid'     => (int) (($request->subCategory == '' || $request->subCategory == null) ? 0 : $request->subCategory)
//            ];
//
//            Log::info('get API ' . $api);
//            Log::info('payload ' . json_encode($query));
//            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
//            if ($response->status() == 200) {
//                $resVal = json_decode($response)->values;
//
//                $filename = 'Mapping Investment Type - ';
//                $export = new ExportViewMasterMappingInvestmentType($resVal);
//                $mc = microtime(true);
//                return Excel::download($export, $filename . date('Y-m-d His') . $mc . '.xlsx');
//            } else {
//                $message = json_decode($response)->message;
//                $result = array(
//                    'error' => $response->status(),
//                    'data' => [],
//                    'message' => $message
//                );
//                Log::warning('get API ' . $api);
//                Log::warning($message);
//                return $result;
//            }
//        } catch (\Exception $e) {
//            Log::error('get API ' . $api);
//            Log::error($e->getMessage());
//            return [];
//        }
//    }
}
