<?php

namespace App\Modules\Mapping\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class SubActivityPromoRecon extends Controller
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
        $title = "Mapping Sub Activity for Allow Edit Promo Recon";
        return view('Mapping::sub-activity-promo-recon.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/mapping/promorecon-subactivity';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            $search = ($request['search']['value'] ?? "");
            $length = $request['length'];
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

    public function subActivityPromoReconFormPage (Request $request)
    {
        $title = "Mapping Sub Activity for Allow Edit Promo Recon";
        return view('Mapping::sub-activity-promo-recon.form', compact('title'));
    }

    public function subActivityPromoReconUploadPage (Request $request)
    {
        $title = "Mapping Sub Activity for Allow Edit Promo Recon";
        return view('Mapping::sub-activity-promo-recon.upload-xls', compact('title'));
    }

    public function getListCategory(Request $request)
    {
        $api = config('app.api'). '/mapping/promorecon-subactivity/category';
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

    public function getListSubCategoryByCategoryId(Request $request)
    {
        $api = config('app.api'). '/mapping/promorecon-subactivity/subcategory';
        $data = [
            'CategoryId'           => $request->categoryId,
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

    public function getListActivityBySubCategoryId(Request $request)
    {
        $api = config('app.api'). '/mapping/promorecon-subactivity/activity';
        $data = [
            'SubCategoryId'           => $request->subCategoryId,
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

    public function getListSubActivityByActivityId(Request $request)
    {
        $api = config('app.api'). '/mapping/promorecon-subactivity/subactivity';
        $data = [
            'ActivityId'           => $request->activityId,
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
        $api = config('app.api') . '/mapping/promorecon-subactivity';
        $data = [
            'subActivityId'      => $request->subActivityId,
            'allowEdit'          => $request->allowEdit === "1",
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

    public function getDataById(Request $request) {
        $api = config('app.api'). '/mapping/promorecon-subactivity/subactivityid';
        $query = [
            'id'           => $request->id,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/mapping/promorecon-subactivity';
        $data = [
            'subActivityId'      => $request->subActivityId,
            'allowEdit'          => $request->allowEdit === "1",
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Update success",
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
            Log::error('put API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Update Failed"
            );
        }
    }

    public function delete(Request $request)
    {
        $api = config('app.api') . '/mapping/promorecon-subactivity';
        $data = [
            'subActivityId'      => $request->id,
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Delete success",
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
            Log::error('delete API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Delete Failed"
            );
        }
    }

    public function uploadXls(Request $request) {
        $api = config('app.api'). '/mapping/promorecon-subactivity/import';
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
                        'data'      => json_decode($response),
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
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Upload Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/mapping/promorecon-subactivity/download';
        Log::info('get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->refid;
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->subActivity;
                    $arr[] = (($fields->allowEdit === 1) ? "During promo period: allow edit" : "During promo period: not allow edit (still allow cancel)");

                    $result[] = $arr;
                }

                $filename = 'Mapping Sub Activity Promo Recon -';
                $title = 'A1:G1'; //Report Title Bold and merge
                $header = 'A3:G3'; //Header Column Bold and color
                $heading = [
                    ['Mapping Sub Activity Promo Recon'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Sub Activity ID', 'Category', 'Sub Category', 'Activity', 'Type', 'Sub Activity', 'Action']
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

    public function downloadTemplate(Request $request) {
        $api = config('app.api'). '/mapping/promorecon-subactivity/template';
        Log::info('get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];
                $raw = 0;
                foreach ($resVal as $fields) {
                    $raw ++;
                    $arr = [];
                    $arr = [];
                    $arr[] = $fields->refId;
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activityLongDesc;
                    $arr[] = $fields->subActivityTypeLongDesc;
                    $arr[] = $fields->longDesc;
                    if ($raw % 2 == 0){
                        $arr[] = 'During promo period: allow edit';
                    }else{
                        $arr[] = 'During promo period: not allow edit (still allow cancel)';
                    }

                    $result[] = $arr;
                }

                $filename = 'Template Mapping Sub Activity Promo Recon -';
                $title = 'A1:A1'; //Report Title Bold and merge
                $header = 'A1:G1'; //Header Column Bold and color
                $heading = [
                    ['Sub Activity ID',
                        'Category',
                        'Sub Category',
                        'Activity',
                        'Type',
                        'Sub Activity',
                        'Action',
                    ]
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
}
