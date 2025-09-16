<?php

namespace App\Modules\Master\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use App\Exports\ExportTemplate;
use Maatwebsite\Excel\Facades\Excel;

class PromoMechanism extends Controller
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
        return view('Master::promo-mechanism.index');
    }

    public function getListPaginateFilter(Request $request) {
        $api = config('app.api'). '/master/mechanism';
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

    public function promoMechanismFormPage(Request $request) {
        return view('Master::promo-mechanism.form');
    }

    public function promoMechanismUploadPage(Request $request) {
        return view('Master::promo-mechanism.upload');
    }

    public function getListEntity(Request $request) {
        $api = config('app.api'). '/master/mechanism/entity';
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

    public function getDataAttribute(Request $request) {
        $api = config('app.api'). '/master/mechanism/attribute';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'attribute'     => $request->attribute,
                'longdesc'      => $request->longDesc
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

    public function getListChannel(Request $request) {
        $api = config('app.api'). '/master/mechanism/channel';
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

    public function getDataByID(Request $request) {
        $api = config('app.api'). '/master/mechanism/id';
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

    public function save(Request $request) {
        $api = config('app.api') . '/master/mechanism';
        $data = [
            "entity"        => $request->entity,
            "subCategoryId" => $request->subcategory,
            "subCategory"   => $request->subCategoryText,
            "activity"      => $request->activity,
            "subActivity"   => $request->subactivity,
            "productId"     => $request->sku,
            "product"       => $request->skuText,
            "mechanism"     => $request->mechanism,
            "channelId"     => $request->channel,
            "channel"       => $request->channelText,
            "startDate"    => date('Y-m-d', strtotime($request->start_date)),
            "endDate"      => date('Y-m-d', strtotime($request->end_date)),
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
        $api = config('app.api') . '/master/mechanism';
        $data = [
            'id'            => $request->id,
            "entity"        => $request->entity,
            "subCategoryId" => $request->subcategory,
            "subCategory"   => $request->subCategoryText,
            "activity"      => $request->activity,
            "subActivity"   => $request->subactivity,
            "productId"     => $request->sku,
            "product"       => $request->skuText,
            "mechanism"     => $request->mechanism,
            "channelId"     => $request->channel,
            "channel"       => $request->channelText,
            "startDate"    => date('Y-m-d', strtotime($request->start_date)),
            "endDate"      => date('Y-m-d', strtotime($request->end_date)),
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

    public function delete(Request $request) {
        $api = config('app.api') . '/master/mechanism';
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
                    'message'   => 'Delete success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('delete API ' . $api);
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

    public function downloadTemplate(Request $request) {
        //Get Data Mechanism
        $apiMechanism = config('app.api'). '/master/mechanism/download';
        Log::info('get API ' . $apiMechanism);

        //Get Data Activity
        $apiActivity = config('app.api'). '/master/mechanism/subactivity';
        $queryActivity = [
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'activityRefId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $apiActivity);
        Log::info('payload ' . json_encode($queryActivity));

        //Get Data SKU
        $apiSku = config('app.api'). '/master/mechanism/product';
        $querySku = [
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'productRefId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $apiSku);
        Log::info('payload ' . json_encode($querySku));

        //Get Data Channel
        $apiChannel = config('app.api'). '/master/mechanism/subaccount';
        $queryChannel = [
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'subAccountRefId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $apiChannel);
        Log::info('payload ' . json_encode($queryChannel));

        try {
            $responseMechanism = Http::timeout(180)->withToken($this->token)->get($apiMechanism);
            $responseActivity = Http::timeout(180)->withToken($this->token)->get($apiActivity, $queryActivity);
            $responseSku = Http::timeout(180)->withToken($this->token)->get($apiSku, $querySku);
            $responseChannel = Http::timeout(180)->withToken($this->token)->get($apiChannel, $queryChannel);

            if ($responseMechanism->status() == 200 && $responseActivity->status() == 200 && $responseSku->status() == 200 && $responseChannel->status() == 200) {

                // Mechanism
                $resValMechanism = json_decode($responseMechanism)->values;
                $resultMechanism=[];
                $rowRequirement = 2;
                $rowSKU = 2;
                $rowDiscount = 2;
                foreach ($resValMechanism as $fields) {
                    $arr = [];
                    $arr[] = $fields->entity;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->product;
                    $arr[] = $fields->requirement;
                    $arr[] = $fields->discount;
                    $arr[] = '=F'. $rowRequirement++ . '&" "&' . 'E' . $rowSKU++ . '&" "&' . 'G' . $rowDiscount++;
                    $arr[] = $fields->channel;
                    $arr[] = date("d-m-Y", strtotime($fields->startDate));
                    $arr[] = date("d-m-Y", strtotime($fields->endDate));

                    $resultMechanism[] = $arr;
                }

                // Activity
                $resValActivity = json_decode($responseActivity)->values->data;
                $resultActivity = [];
                foreach ($resValActivity as $fields) {
                    $arr = [];
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->subCategoryLongDesc;
                    $arr[] = $fields->activityLongDesc;
                    $arr[] = $fields->subActivityLongDesc;

                    $resultActivity[] = $arr;
                }

                // SKU
                $resValSku = json_decode($responseSku)->values->data;
                $resultSku = [];
                foreach ($resValSku as $fields) {
                    $arr = [];
                    $arr[] = $fields->entityLongDesc;
                    $arr[] = $fields->brandLongDesc;
                    $arr[] = $fields->productLongDesc;

                    $resultSku[] = $arr;
                }

                // Channel
                $resValChannel = json_decode($responseChannel)->values->data;
                $resultChannel = [];
                foreach ($resValChannel as $fields) {
                    $arr = [];
                    $arr[] = $fields->channelLongDesc;
                    $arr[] = $fields->subChannelLongDesc;
                    $arr[] = $fields->accountLongDesc;
                    $arr[] = $fields->subAccountLongDesc;

                    $resultChannel[] = $arr;
                }

                $filename = 'Template_mechanism_mass_upload';

                // Mechanism
                $headerMechanism = 'A1:K1'; //Header Column Bold and color
                $headingMechanism = [
                    ['Entity', 'Sub Category', 'Activity', 'Sub Activity', 'SKU', 'Requirement', 'Discount', 'Mechanism Master Data', 'CHANNEL', 'START PROMO', 'END PROMO']
                ];

                // Activity
                $headerActivity = 'A1:D1'; //Header Column Bold and color
                $headingActivity = [
                    ['Category', 'Sub Category', 'Activity', 'Sub Activity']
                ];

                // Sku
                $headerSku = 'A1:C1'; //Header Column Bold and color
                $headingSku = [
                    ['Entity', 'Brand', 'SKU', ]
                ];

                // Channel
                $headerChannel = 'A1:D1'; //Header Column Bold and color
                $headingChannel = [
                    ['Channel', 'Sub Channel', 'Account', 'Sub Account']
                ];

                $export = new ExportTemplate($resultMechanism, $headingMechanism, $headerMechanism,
                    $resultActivity, $headingActivity, $headerActivity,
                    $resultSku, $headingSku, $headerSku,
                    $resultChannel, $headingChannel, $headerChannel,
                );



                return Excel::download($export, $filename . '.xlsx');
            } else {
                return array(
                    'error' => '500',
                    'message' => 'Download Template failed'
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                    'error' => '500',
                    'message' => 'Download Template failed'
                );
        }
    }

    public function exportXls(Request $request)
    {
        $api = config('app.api'). '/master/mechanism';
        try {
            $query = [
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
                'SortColumn'                => 'id',
                'SortDirection'             => 'asc'
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->entity;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->product;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('d-m-Y' , strtotime($fields->startDate));
                    $arr[] = date('d-m-Y' , strtotime($fields->endDate));
                    $arr[] = $fields->createBy;
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));

                    $result[] = $arr;
                }

                $filename = 'Master Mechanism-';
                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A3:K3'; //Header Column Bold and color
                $heading = [
                    ['Master Mechanism'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity','Sub Category','Activity', 'Sub Activity','SKU', 'Channel', 'Mechanism', 'Start Date', 'End Date', 'Created By', 'Created On']
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
                Log::warning('get API ' . $api);
                Log::warning($message);
                return $result;
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/master/mechanism/upload';
        Log::info('post API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);

            if ($response->status() === 200) {
                $resVal = json_decode($response)->result;
                return json_encode([
                    'error' => false,
                    'data' => $resVal,
                    'message' => "Upload success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error' => true,
                    'message' => "Upload failed"
                );
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
