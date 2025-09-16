<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Wording;

class PromoSendBack extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function getDataById(Request $request)
    {
        $api = config('app.api'). '/promo/sendback/id';
        $query = [
            'id'        => $request['promoId'],
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                if($resVal->promoHeader->createBy === $request->session()->get('profile')) {
                    return json_encode([
                        'error'     => false,
                        'data'      => $resVal,
                        'errCode'   => 200,
                        'message'   => 'Success get data'
                    ]);
                } else {
                    return json_encode([
                        'error'     => false,
                        'data'      => $resVal,
                        'errCode'   => 404,
                        'message'   => 'This promo is not yours'
                    ]);
                }
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

    public function getListCategory(Request $request)
    {
        $api = config('app.api') . '/promo/sendback/category';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api') . '/promo/entity';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListDistributor(Request $request)
    {
        $api = config('app.api') . '/promo/distributor';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request->entityId);

            $query = [
                'budgetId' => 0,
                'entityId' => $ar_parent,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListSubCategory(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            if (isset($request->isDeleted)) {
                $dataActive = $request->isDeleted; //Semua Data
            } else {
                $dataActive = '0'; //Data Active saja
            }
            $ar_parent = array();
            array_push($ar_parent, $request->categoryId);

            $query = [
                'budgetId' => 0,
                'attribute' => 'subcategory',
                'arrayParent' => $ar_parent,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API ' . $api . ' subCategory');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListActivity(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            if (isset($request->isDeleted)) {
                $dataActive = $request->isDeleted; //Semua Data
            } else {
                $dataActive = '0'; //Data Active saja
            }
            $ar_parent = array();
            array_push($ar_parent, $request->subCategoryId);

            $query = [
                'budgetId' => 0,
                'attribute' => 'activity',
                'arrayParent' => $ar_parent,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API ' . $api . ' Activity');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListSubActivity(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            if (isset($request->isDeleted)) {
                $dataActive = $request->isDeleted; //Semua Data
            } else {
                $dataActive = '0'; //Data Active saja
            }
            $ar_parent = array();
            array_push($ar_parent, $request->activityId);

            $query = [
                'budgetId' => 0,
                'attribute' => 'subactivity',
                'arrayParent' => $ar_parent,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListSubactivityType(Request $request)
    {
        $api = config('app.api'). '/promo/subcategory/categoryid';
        $query = [
            'CategoryId'  => $request['CategoryId'],
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

    public function getListEditChannel(Request $request)
    {
        $api = config('app.api') . '/promo/creation/channel/promoid';
        try {
            $query = [
                'promoId' => $request->promoId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListEditSubChannel(Request $request)
    {
        $api = config('app.api') . '/promo/creation/subchannel/promoid';
        try {
            $dataChannel = json_decode($request->dataChannel, TRUE);
            $arr_channels = array();
            for ($i = 0; $i < count($dataChannel); $i++) {
                array_push($arr_channels, $dataChannel[$i]);
            }

            $query = [
                'promoId' => $request->promoId,
                'arrayChannel' => $arr_channels,
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListEditAccount(Request $request)
    {
        $api = config('app.api') . '/promo/creation/account/promoid';
        try {
            $dataSubChannel = json_decode($request->dataSubChannel, TRUE);
            $arr_subChannels = array();
            for ($i = 0; $i < count($dataSubChannel); $i++) {
                array_push($arr_subChannels, $dataSubChannel[$i]);
            }

            $query = [
                'promoId' => $request->promoId,
                'arraySubChannel' => $arr_subChannels,
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListEditSubAccount(Request $request)
    {
        $api = config('app.api') . '/promo/creation/subaccount/promoid';
        try {
            $dataAccount = json_decode($request->dataAccount, TRUE);
            $arr_accounts = array();
            for ($i = 0; $i < count($dataAccount); $i++) {
                array_push($arr_accounts, $dataAccount[$i]);
            }

            $query = [
                'promoId' => $request->promoId,
                'arrayAccount' => $arr_accounts,
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getInvestmentType(Request $request)
    {
        $api = config('app.api') . '/promo/investmenttype';
        try {
            $query = [
                'subActivityId' => $request->subActivityId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListChannelMaster(Request $request)
    {
        $api = config('app.api') . '/master/channel';
        try {
            $query = [
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
                'SortColumn'                => 'LongDesc',
                'SortDirection'             => 'asc'
            ];

            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal->data
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getConfigRoiCR(Request $request)
    {
        $api = config('app.api') . '/promo/config-roi-cr';
        try {
            $query = [
                'subActivityId' => $request->subActivityId,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListSourceBudget(Request $request)
    {
        $api = config('app.api') . '/promo/sendback/sourcebudget';
        try {
            $query = [
                'year'              => $request['period'],
                'entityId'          => ($request['entityId'] ?? 0),
                'distributorId'     => ($request['distributorId'] ?? 0),
                'subCategoryId'     => ($request['subCategoryId'] ?? 0),
                'activityId'        => ($request['activityId'] ?? 0),
                'subActivityId'     => ($request['subActivityId'] ?? 0),
                'arrayChannel'      => ($request['channel'][0] ?? [0]),
                'arraySubChannel'   => ($request['subChannel'][0] ?? [0]),
                'arrayAccount'      => ($request['account'][0] ?? [0]),
                'arraySubAccount'   => ($request['subAccount'][0] ??  [0]),
                'arrayRegion'       => ($request['region'] ?? [0]),
                'arrayBrand'        => ($request['brand'] ?? [0]),
                'arraySKU'          => ($request['sku'] ?? [0]),
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getCutOff(Request $request)
    {
        $app_cutoff_mechanism = config('app.app_cutoff_mechanism');
        $app_cutoff_hierarchy = config('app.app_cutoff_hierarchy');
        return array(
            'app_cutoff_mechanism' => $app_cutoff_mechanism,
            'app_cutoff_hierarchy' => $app_cutoff_hierarchy
        );
    }

    public function getListRegion(Request $request)
    {
        $api = config('app.api') . '/promo/attribute/region';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListGroupBrand(Request $request)
    {
        $api = config('app.api') . '/promo/groupbrand/entityid';
        try {
            $query = [
                'entityid' => $request->entityId,
            ];
            Log::info('get API ' . $api . ' Group Brand');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListBrandByGroupBrandId(Request $request)
    {
        $api = config('app.api') . '/promo/brand/groupbrandid';
        try {
            $query = [
                'groupbrandid' => $request->groupBrandId,
            ];
            Log::info('get API ' . $api . ' Group Brand');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListBrand(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            if (isset($request->isDeleted)) {
                $dataActive = $request->isDeleted; //Semua Data
            } else {

                $dataActive = '0'; //Data Active saja
            }
            $ar_parent = array();
            array_push($ar_parent, $request->entityId);

            $query = [
                'budgetId' => 0,
                'attribute' => 'brand',
                'arrayParent' => $ar_parent,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API ' . $api . ' Brand');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListSKU(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            if (isset($request->isDeleted)) {
                $dataActive = $request->isDeleted; //Semua Data
            } else {
                $dataActive = '0'; //Data Active saja
            }

            $query = [
                'budgetId' => 0,
                'attribute' => 'product',
                'arrayParent' => $request->brandId,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API ' . $api . ' SKU');
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function getListMechanism(Request $request)
    {
        $api = config('app.api') . '/promo/mechanism';
        try {
            $query = [
                'entityId' => $request->entityId,
                'subCategoryId' => $request->subCategoryId,
                'activityId' => $request->activityId,
                'subActivityId' => $request->subActivityId,
                'skuId' => $request->skuId,
                'channelId' => $request->channelId,
                'startDate' => $request->startDate,
                'endDate' => $request->endDate,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/promo/sendback';
        $promoHeader = [
            "promoId"                   => $request['promoId'],
            "promoPlanId"               => $request['promoPlanId'],
            "allocationId"              => $request['allocationId'],
            "allocationRefId"           => $request['allocationRefId'],
            "categoryShortDesc"         => $request['categoryShortDesc'],
            "principalShortDesc"        => $request['entityShortDesc'],
            "budgetMasterId"            => $request['allocationId'],
            "categoryId"                => $request['categoryId'],
            "subCategoryId"             => $request['subCategoryId'],
            "activityId"                => $request['activityId'],
            "subActivityId"             => $request['subActivityId'],
            "activityDesc"              => $request['activityDesc'],
            "startPromo"                => $request['startPromo'],
            "endPromo"                  => $request['endPromo'],
            "mechanisme1"               => '',
            "mechanisme2"               => '',
            "mechanisme3"               => '',
            "mechanisme4"               => '',
            "investment"                => str_replace(',', '', $request['investment']),
            "normalSales"               => str_replace(',', '', $request['baselineSales']),
            "incrSales"                 => str_replace(',', '', $request['incrementSales']),
            "roi"                       => str_replace(',', '', $request['roi']),
            "costRatio"                 => str_replace(',', '', $request['costRatio']),
            "statusApproval"            => $request['statusApproval'],
            "notes"                     => ($request['notes_message'] ?? ''),
            "tsCoding"                  => $request['tsCode'],
            "initiator_notes"           => $request['initiatorNotes'],
            "modifReason"               => $request['modifReason'],
        ];

        $dataRegion = json_decode($request->dataRegion, TRUE);
        $arr_regions = array();
        for ($i = 0; $i < count($dataRegion); $i++) {
            $regions['id'] = $dataRegion[$i];
            array_push($arr_regions, $regions);
        }

        $dataChannel = json_decode($request->dataChannel, TRUE);
        $arr_channels = array();
        for ($i = 0; $i < count($dataChannel); $i++) {
            $channels['id'] = (int) $dataChannel[$i];
            array_push($arr_channels, $channels);
        }

        $dataSubChannel = json_decode($request->dataSubChannel, TRUE);
        $arr_subChannels = array();
        for ($i = 0; $i < count($dataSubChannel); $i++) {
            $subChannels['id'] = $dataSubChannel[$i];
            array_push($arr_subChannels, $subChannels);
        }

        $dataAccount = json_decode($request->dataAccount, TRUE);
        $arr_accounts = array();
        for ($i = 0; $i < count($dataAccount); $i++) {
            $accounts['id'] = $dataAccount[$i];
            array_push($arr_accounts, $accounts);
        }

        $dataSubAccount = json_decode($request->dataSubAccount, TRUE);
        $arr_subAccounts = array();
        for ($i = 0; $i < count($dataSubAccount); $i++) {
            $subAccounts['id'] = $dataSubAccount[$i];
            array_push($arr_subAccounts, $subAccounts);
        }

        $dataBrand = json_decode($request->dataBrand, TRUE);
        $arr_brands = array();
        for ($i = 0; $i < count($dataBrand); $i++) {
            $brands['id'] = $dataBrand[$i];
            array_push($arr_brands, $brands);
        }

        $dataSKU = json_decode($request->dataSKU, TRUE);
        $arr_skus = array();
        for ($i = 0; $i < count($dataSKU); $i++) {
            $skus['id'] = $dataSKU[$i];
            array_push($arr_skus, $skus);
        }
        $arr_Files = array();
        if($request['fileName1']) {
            if ($request['fileName1'] !== 'For SKP Draft') {
                $fileAttach = [
                    'docLink' => 'row1',
                    'fileName' => $request['fileName1']

                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName2']) {
            if ($request['fileName2'] !== 'For SKP Fully Approved') {
                $fileAttach = [
                    'docLink' => 'row2',
                    'fileName' => $request['fileName2']

                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName3']) {
            if ($request['fileName3'] !== '') {
                $fileAttach = [
                    'docLink' => 'row3',
                    'fileName' => $request['fileName3']

                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName4']) {
            if ($request['fileName4'] !== '') {
                $fileAttach = [
                    'docLink' => 'row4',
                    'fileName' => $request['fileName4']

                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName5']) {
            if ($request['fileName5'] !== '') {
                $fileAttach = [
                    'docLink' => 'row5',
                    'fileName' => $request['fileName5']
                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName6']) {
            if ($request['fileName6'] !== '') {
                $fileAttach = [
                    'docLink' => 'row6',
                    'fileName' => $request['fileName6']
                ];
                array_push($arr_Files, $fileAttach);
            }
        }
        if($request['fileName7']) {
            if ($request['fileName7'] !== '') {
                $fileAttach = [
                    'docLink' => 'row7',
                    'fileName' => $request['fileName7']
                ];
                array_push($arr_Files, $fileAttach);
            }
        }

        $data = [
            'promoHeader'       => $promoHeader,
            'regions'           => $arr_regions,
            'channels'          => $arr_channels,
            'subChannels'       => $arr_subChannels,
            'accounts'          => $arr_accounts,
            'subAccounts'       => $arr_subAccounts,
            'brands'            => $arr_brands,
            'skus'              => $arr_skus,
            'mechanisms'        => json_decode($request->dataMechanism),
            'promoAttachment'   => $arr_Files,
            'budgetAmount'      => str_replace(',', '', $request['budgetDeployed']),
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;
                if (json_decode($response)->error) {
                    return json_encode([
                        'error' => true,
                        'data' => $values,
                        'message' => json_decode($response)->message,
                    ]);
                } else {
                    if (!$values->isFullyApproved) {
                        // send email to approver
                        $emailApprover = $this->sendEmailApprover($values->userid_approver, $values->username_approver, $values->id, $values->email_approver, "[APPROVAL NOTIF] Promo requires approval (" . $values->refId . ")", date("Y", strtotime($request['startPromo'])));
                        if (!json_decode($emailApprover)->error) {
                            Log::info("Promo ID " . json_decode($response)->values->refId . " Has been saved successfully and send email notification to " . $values->userid_approver . " (" . $values->email_approver . ")");
                            return array(
                                'error' => false,
                                'message' => 'Promo ID ' . json_decode($response)->values->refId . ' Has been saved successfully'
                            );
                        } else {
                            Log::warning("Promo ID " . json_decode($response)->values->refId . " Has been saved successfully but can't send email notification to " . $values->userid_approver . " (" . $values->email_approver . ")");
                            return array(
                                'error' => true,
                                'message' => "Promo ID " . json_decode($response)->values->refId . " Has been saved successfully but can't send email notification to " . $values->userid_approver . " (" . $values->email_approver . ")"
                            );
                        }
                    } else {
                        return json_encode([
                            'error' => false,
                            'data' => $values,
                            'message' => 'Promo ID ' . json_decode($response)->values->refId . ' Has been saved successfully',
                        ]);
                    }
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Save Failed"
            );
        }
    }

    protected function sendEmailApprover($userApprover, $nameApprover, $id, $email, $subject, $yearPromo): bool|string
    {
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        if ($yearPromo >= 2025){
            $api_promo = config('app.api') . '/promo/display/email/id';
        } else {
            $api_promo = config('app.api') . '/promo/display/email/id';
        }

        $query_promo = [
            'id'           => $id,
        ];
        Log::info('get API ' . $api_promo);
        Log::info('payload ' . json_encode($query_promo));

        $api_email = config('app.api') . '/tools/email';
        try {
            $responsePromo =  Http::timeout(180)->withToken($this->token)->get($api_promo, $query_promo);
            if ($responsePromo->status() === 200) {
                $data = json_decode($responsePromo)->values;
                if (!$data) $data = array();

                $ar_fileattach = array();
                if ($yearPromo >= 2025){

                    $viewEmail = 'Promo::promo-send-back.new-email-approval';

                    $attach = array();
                    for ($i=0; $i<count($data->attachments); $i++) {
                        if ($data->attachments[$i]->docLink =='row1') $attach['row1'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row2') $attach['row2'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row3') $attach['row3'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row4') $attach['row4'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row5') $attach['row5'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row6') $attach['row6'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row7') $attach['row7'] = $data->attachments[$i]->fileName;
                    }
                    if (!empty($data->attachments)) array_push($ar_fileattach, $attach);

                    $urlid = $data->promoHeader->id;
                    $urlrefid = urlencode(MyEncrypt::encrypt($data->promoHeader->refId));

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                } else {
                    $viewEmail = 'Promo::promo-send-back.email-approval';

                    $attach = array();
                    for ($i=0; $i<count($data->attachments); $i++) {
                        if ($data->attachments[$i]->docLink =='row1') $attach['row1'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row2') $attach['row2'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row3') $attach['row3'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row4') $attach['row4'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row5') $attach['row5'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row6') $attach['row6'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row7') $attach['row7'] = $data->attachments[$i]->fileName;
                    }
                    if (!empty($data->attachments)) array_push($ar_fileattach, $attach);

                    $urlid = $data->promoHeader->id;
                    $urlrefid = urlencode(MyEncrypt::encrypt($data->promoHeader->refId));

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                }

                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view($viewEmail, compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $email,
                    'subject'   => $subject,
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
                Log::info('payload ' . json_encode($data));

                $responseEmail = Http::asForm()->withToken($this->token)->post($api_email, $data);
                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success");
                    return json_encode([
                        'error'     => false,
                        'data'      => [],
                        'message'   => "Send Email Success"
                    ]);
                } else {
                    Log::info("error : Send Email Failed");
                    return json_encode([
                        'error'     => true,
                        'data'      => [],
                        'message'   => "error : Send Email Failed"
                    ]);
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => "error : Get Data Promo Failed"
                ]);
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }

    public function uploadFile(Request $request)
    {
        $api = config('app.api') . '/promo/creation/attachment';
        $FileId = $request['uuid'];
        $promoId = 0;
        if ($request['mode'] ==='edit') {
            $FileId = $request['promoId'];
            $promoId = $request['promoId'];
        }
        $path = '/assets/media/promo/' . $FileId;
        $path_row = '/assets/media/promo/' . $FileId . '/' . $request['row'];
        try {
            if (!Storage::disk('optima')->exists($path)) {
                Storage::disk('optima')->makeDirectory($path);
            }
            if (!Storage::disk('optima')->exists($path_row)) {
                Storage::disk('optima')->makeDirectory($path_row);
            }
            $fileSize = $request->file('file')->getSize();
            if (Storage::disk('optima')->put($path_row . '/' . $request->file('file')->getClientOriginalName(), file_get_contents($request['file']))) {
                $filePath = $path_row . '/' . $request->file('file')->getClientOriginalName();
                $fileExist = Storage::disk('optima')->exists($filePath);
                $fileSizeA = Storage::disk('optima')->size($filePath);
                if ($fileExist && ($fileSize===$fileSizeA)) {
                    if ($request['promoId'] === '0') {
                        Log::info(json_encode([
                            'error'     => false,
                            'promoId'   => $request['promoId'],
                            'message'   => "Upload Successfully",
                        ]));
                        return json_encode([
                            'error' => false,
                            'message' => "Upload Successfully",
                        ]);
                    } else {
                        $data = [
                            'promoId' => (int)$promoId,
                            'docLink' => $request['row'],
                            'fileName' => $request->file('file')->getClientOriginalName(),
                        ];
                        Log::info('post API ' . $api);
                        Log::info('payload ' . json_encode($data));

                        $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
                        if ($response->status() === 200) {
                            Log::info(json_encode([
                                'error'     => false,
                                'promoId'   => $request['promoId'],
                                'message'   => "Upload Successfully",
                            ]));
                            return json_encode([
                                'error' => false,
                                'message' => "Upload Successfully",
                            ]);
                        } else {
                            $message = json_decode($response)->message;
                            Log::warning($message);
                            return array(
                                'error' => true,
                                'message' => "Upload Failed",
                            );
                        }
                    }
                } else {
                    Log::warning('promoid ' . $promoId . ' Upload attachment Failed');
                    return array(
                        'error' => true,
                        'message' => "Upload Failed",
                    );
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => "Upload Failed"
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => "Upload Failed"
            ]);
        }
    }

    public function deleteFile(Request $request) {
        $api = config('app.api') . '/promo/creation/attachment';
        try {
            $FileId = $request['uuid'];
            $promoId = 0;
            if ($request['mode'] ==='edit') {
                $FileId = $request['promoId'];
                $promoId = ($request['promoId'] ?? 0);
            }

            $file_name = $request['fileName'];
            $path_row = '/assets/media/promo/' . $FileId . '/' . $request['row'];

            $fileDate = date('Y-m-d');
            $newFileName = $path_row . '/' . $fileDate . '_' . $file_name;

            if (Storage::disk('optima')->move($path_row . '/' . $file_name, $newFileName)) {
                if ($request['promoId'] === '0') {
                    return json_encode([
                        'error'     => false,
                        'message'   => "Delete Successfully",
                        'userid'    => $request->session()->get('profile'),

                    ]);
                } else {
                    $data = [
                        'promoId' => (int) $promoId,
                        'docLink' => $request['row'],
                        'fileName' => $file_name,
                    ];
                    Log::info('delete API ' . $api);
                    Log::info('payload ' . json_encode($data));

                    $response = Http::timeout(180)->withToken($this->token)->delete($api, $data);
                    if ($response->status() === 200) {
                        return json_encode([
                            'error' => false,
                            'userid' => $request->session()->get('profile'),
                            'message' => "Delete Successfully",
                        ]);
                    } else {
                        $message = json_decode($response)->message;
                        Log::warning($message);
                        return array(
                            'error' => true,
                            'message' => "Delete Failed",
                        );
                    }
                }
            } else {
                $data = [
                    'promoId' => (int)$promoId,
                    'docLink' => $request['row'],
                    'fileName' => $file_name,
                ];
                Log::info('delete API ' . $api);
                Log::info('payload ' . json_encode($data));

                $response = Http::timeout(180)->withToken($this->token)->delete($api, $data);
                if ($response->status() === 200) {
                    return json_encode([
                        'error' => false,
                        'userid' => $request->session()->get('profile'),
                        'message' => "Delete Successfully",
                    ]);
                } else {
                    $message = json_decode($response)->message;
                    Log::warning($message);
                    return array(
                        'error' => false,
                        'message' => "Delete Successfully",
                    );
                }
            }
        } catch (Exception $e) {
            Log::error('delete API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "Delete Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/promo/sendback';
        $query = [
            'year'                      => $request->period,
            'categoryId'                => $request['categoryId'] ?? 0,
            'entity'                    => $request['entityId'] ?? 0,
            'distributor'               => $request['distributorId'] ?? 0,
            'budgetparent'              => $request['budgetparent'] ?? 0,
            'channel'                   => $request['channel'] ?? 0,
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

                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->activityDesc;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->approveOn));
                    $arr[] = $fields->approvalNotes;

                    $result[] = $arr;
                }
                $title = 'A1:G1'; //Report Title Bold and merge
                $header = 'A7:G7'; //Header Column Bold and color
                $heading = [
                    ['Promo Sendback'],
                    ['Budget Year : ' . $request->period],
                    ['Category : ' . $request->category],
                    ['Entity : ' . $request->entity],
                    ['Distributor : ' . $request->distributor],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo ID', 'Last Status', 'Activity Description', 'Promo Start', 'Promo End', 'Sendback Date', 'Notes']
                ];
                $formatCell =  [
                    'D' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'E' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'F' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                ];
                $filename = 'PromoSendback-';
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


    public function landingPage(): Factory|View|Application
    {
        $title = "Promo Send Back";
        return view('Promo::promo-send-back.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): bool|string
    {
        $api = '/promo/sendback';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'year'                      => $request['period'],
            'entity'                    => $request['entityId'],
            'distributor'               => $request['distributorId'],
            'category'                  => ($request['categoryId'] ?? 0),
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data;
            foreach ($resVal as $data) {
                $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
            }
            return json_encode([
                "data" => $resVal,
            ]);
        } else {
            return $res;
        }
    }

    public function form(Request $request): Factory|View|Application
    {
        $category = MyEncrypt::decrypt($request['c']);
        $isOld = $request['old'];
        if($category === 'DC') {
            $title = "Promo Send Back (Distributor Cost)";
            if ($isOld) {
                return view('Promo::promo-send-back.dc-form', compact('title'));
            } else {
                if ($request['recon'] === "0") {
                    return view('Promo::promo-send-back.dc-form-revamp', compact('title'));
                } else {
                    $title = "Promo Send Back Reconciliation (Distributor Cost)";
                    return view('Promo::promo-send-back.dc-form-recon', compact('title'));
                }
            }
        } else {
            $title = "Promo Send Back (Retailer Cost)";
            if ($isOld) {
                return view('Promo::promo-send-back.form', compact('title'));
            } else {
                if ($request['recon'] === "0") {
                    return view('Promo::promo-send-back.form-revamp', compact('title'));
                } else {
                    $title = "Promo Send Back Reconciliation (Retailer Cost)";
                    return view('Promo::promo-send-back.form-recon', compact('title'));
                }
            }
        }
    }

    public function encCategoryShortDesc ($categoryShortDesc): string
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }

    public function getCategoryByCategoryShortDesc(Request $request): bool|string
    {
        $category = MyEncrypt::decrypt($request['categoryShortDesc']);
        if ($request['c']) {
            $category = MyEncrypt::decrypt($request['c']);
        }
        $api = '/master/category/shortdesc';
        $callApi = new CallApi();
        $query = [
            'categoryShortDesc' => $category,
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getPromoAttributeList(): bool|string
    {
        $api = '/promo/sendbackv2/attributlist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getDataBudget(Request $request): bool|string
    {
        $api = '/promo/creationv2/budget';
        $callApi = new CallApi();
        $query = [
            'period'                => $request['period'],
            'groupBrandId'           => $request['groupBrandId'],
            'categoryId'            => $request['categoryId'],
            'subCategoryId'         => $request['subCategoryId'],
            'activityId'            => $request['activityId'],
            'subActivityId'         => $request['subActivityId'],
            'subActivityTypeId'     => $request['subActivityTypeId'],
            'distributorId'         => $request['distributorId'],
            'channelId'             => $request['channelId'],
            'subChannelId'          => $request['subChannelId'],
            'accountId'             => $request['accountId'],
            'subAccountId'          => $request['subAccountId'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataMechanism(Request $request): bool|string
    {
        $api = '/promo/sendbackv2/mechanismwithstatus';
        $callApi = new CallApi();
        $query = [
            'entityId'          => ($request['entityId'] ?? 0),
            'subCategoryId'     => 0,
            'activityId'        => ($request['activityId'] ?? 0),
            'subActivityId'     => ($request['subActivityId'] ?? 0),
            'skuId'             => 0,
            'channelId'         => ($request['channelId'] ?? 0),
            'startDate'         => ($request['startDate'] ?? 0),
            'endDate'           => ($request['endDate'] ?? 0),
            'brandId'           => ($request['brandId'] ?? 0),
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataBaseline(Request $request): bool|string
    {
        $api = '/promo/sendbackv2/baseline';
        $callApi = new CallApi();
        $query = [
            'promoId'           => ($request['promoId'] ?? 0),
            'period'            => $request['period'],
            'date'              => date('Y-m-d H:i:s'),
            'pType'             => 1,
            'grpBrand'          => ($request['groupBrandId'] ?? 0),
            'subCategory'       => ($request['subCategoryId'] ?? 0),
            'subActivity'       => ($request['subActivityId'] ?? 0),
            'promoStart'        => $request['startDate'],
            'promoEnd'          => $request['endDate'],
            'distributor'       => ($request['distributorId'] ?? 0),
            'channel'           => $request['channelId'],
            'subChannel'        => $request['subChannelId'],
            'account'           => $request['accountId'],
            'subAccount'        => $request['subAccountId'],
            'region'            => json_decode($request['regions']),
            'product'           => json_decode($request['skus']),
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataTotalSales(Request $request): bool|string
    {
        $api = '/promo/sendbackv2/ssvalue';
        $callApi = new CallApi();
        $query = [
            'period'            => $request['period'],
            'groupBrandId'      => ($request['groupBrandId'] ?? 0),
            'promoStart'        => $request['startDate'],
            'promoEnd'          => $request['endDate'],
            'channelId'         => $request['channelId'],
            'subChannelId'      => $request['subChannelId'],
            'accountId'         => $request['accountId'],
            'subAccountId'      => $request['subAccountId'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataCR(Request $request): bool|string
    {
        $api = '/promo/sendbackv2/cr';
        $callApi = new CallApi();
        $query = [
            'period'            => $request['period'],
            'grpBrandId'        => ($request['groupBrandId'] ?? 0),
            'subActivityId'     => ($request['subActivityId'] ?? 0),
            'distributorId'     => ($request['distributorId'] ?? 0),
            'subAccountId'      => ($request['subAccountId'] ?? 0),
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getData(Request $request): bool|string
    {
        $api = '/promo/sendbackv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataDC(Request $request): bool|string
    {
        $api = '/promo/sendbackv2dc/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataRecon(Request $request): bool|string
    {
        $api = '/promo/reconv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataReconDC(Request $request): bool|string
    {
        $api = '/promo/reconv2dc/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function updatePromo(Request $request): array
    {
        $api = config('app.api') . '/promo/sendbackv2';

        $arrFiles = array();
        if($request['fileName1']) {
            if ($request['fileName1'] !== '') {
                $fileAttach = [
                    'docLink' => 'row1',
                    'fileName' => $request['fileName1']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName2']) {
            if ($request['fileName2'] !== '') {
                $fileAttach = [
                    'docLink' => 'row2',
                    'fileName' => $request['fileName2']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName3']) {
            if ($request['fileName3'] !== '') {
                $fileAttach = [
                    'docLink' => 'row3',
                    'fileName' => $request['fileName3']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName4']) {
            if ($request['fileName4'] !== '') {
                $fileAttach = [
                    'docLink' => 'row4',
                    'fileName' => $request['fileName4']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName5']) {
            if ($request['fileName5'] !== '') {
                $fileAttach = [
                    'docLink' => 'row5',
                    'fileName' => $request['fileName5']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName6']) {
            if ($request['fileName6'] !== '') {
                $fileAttach = [
                    'docLink' => 'row6',
                    'fileName' => $request['fileName6']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName7']) {
            if ($request['fileName7'] !== '') {
                $fileAttach = [
                    'docLink' => 'row7',
                    'fileName' => $request['fileName7']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }

        $promo = [
            'promoId'               => $request['promoId'],
            'periode'               => $request['period'],
            'entityId'              => $request['entityId'],
            'groupBrandId'          => $request['groupBrandId'],
            'categoryId'            => $request['categoryId'],
            'subCategoryId'         => $request['subCategoryId'],
            'activityId'            => $request['activityId'],
            'subActivityId'         => $request['subActivityId'],
            'subActivityTypeId'     => $request['subActivityTypeId'],
            'activityDesc'          => $request['activityDesc'],
            'startPromo'            => $request['startPromo'],
            'endPromo'              => $request['endPromo'],
            'distributorId'         => $request['distributorId'],
            'channelId'             => $request['channelId'],
            'subChannelId'          => $request['subChannelId'],
            'accountId'             => $request['accountId'],
            'subAccountId'          => $request['subAccountId'],
            'baseline'              => str_replace(',', '', $request['baseline']),
            'upLift'                => str_replace(',', '', $request['upLift']),
            'totalSales'            => str_replace(',', '', $request['totalSales']),
            'salesContribution'     => str_replace(',', '', $request['salesContribution']),
            'storesCoverage'        => str_replace(',', '', $request['storesCoverage']),
            'redemptionRate'        => str_replace(',', '', $request['redemptionRate']),
            'cr'                    => str_replace(',', '', $request['cr']),
            'cost'                  => str_replace(',', '', $request['cost']),
            "notes"                 => ($request['notes_message'] ?? ''),
            "initiator_notes"       => $request['initiatorNotes'],
            "modifReason"           => $request['modifReason'],
        ];

        $body = [
            'promo'                 => $promo,
            'region'                => json_decode($request['region']),
            'sku'                   => json_decode($request['sku']),
            'mechanism'             => json_decode($request['mechanism']),
            'attachment'            => $arrFiles,
        ];
        Log::info('PUT API ' . $api);
        Log::info('payload ' . json_encode($body));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                if (!Storage::disk('optima')->exists('/assets/media/promo')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/promo');
                }

                $pathPromoId = '/assets/media/promo/' . $resVal->id;
                $pathTemp = '/assets/media/promo/' . $request['uuid'];

                if (!Storage::disk('optima')->exists($pathTemp)) {
                    Storage::disk('optima')->makeDirectory($pathTemp);
                }

                if (Storage::disk('optima')->move($pathTemp, $pathPromoId)) {
                    Log::info(json_encode([
                        'error'     => false,
                        'message'   => "Upload Successfully",
                        'userid'    => $request->session()->get('profile')
                    ]));
                } else {
                    Log::warning(json_encode([
                        'error'     => true,
                        'message'   => 'Upload Failed',
                        'userid'    => $request->session()->get('profile')
                    ]));
                }

                //upload attachment1
                $path_row1 = '/assets/media/promo/' . $resVal->id . '/row1/';
                $fileUpload1 = "No File Upload attachment1";
                if($request['fileName1']) {
                    if ($request['fileName1'] !== '') {
                        if (Storage::disk('optima')->exists($path_row1 . $request['fileName1'])) {
                            $fileUpload1 = "Upload Successfully";
                        } else {
                            $fileUpload1 = "Upload Failed";
                        }
                    }
                }

                //upload attachment2
                $path_row2 = '/assets/media/promo/' . $resVal->id . '/row2/';
                $fileUpload2 = "No File Upload attachment2";
                if($request['fileName2']) {
                    if ($request['fileName2'] !== '') {
                        if (Storage::disk('optima')->exists($path_row2 . $request['fileName2'])) {
                            $fileUpload2 = "Upload Successfully";
                        } else {
                            $fileUpload2 = "Upload Failed";
                        }
                    }
                }

                //upload attachment3
                $path_row3 = '/assets/media/promo/' . $resVal->id . '/row3/';
                $fileUpload3 = "No File Upload attachment3";
                if($request['fileName3']) {
                    if ($request['fileName3'] !== '') {
                        if (Storage::disk('optima')->exists($path_row3 . $request['fileName3'])) {
                            $fileUpload3 = "Upload Successfully";
                        } else {
                            $fileUpload3 = "Upload Failed";
                        }
                    }
                }

                //upload attachment4
                $path_row4 = '/assets/media/promo/' . $resVal->id . '/row4/';
                $fileUpload4 = "No File Upload attachment4";
                if($request['fileName4']) {
                    if ($request['fileName4'] !== '') {
                        if (Storage::disk('optima')->exists($path_row4 . $request['fileName4'])) {
                            $fileUpload4 = "Upload Successfully";
                        } else {
                            $fileUpload4 = "Upload Failed";
                        }
                    }
                }

                //upload attachment5
                $path_row5 = '/assets/media/promo/' . $resVal->id . '/row5/';
                $fileUpload5 = "No File Upload attachment5";
                if($request['fileName5']) {
                    if ($request['fileName5'] !== '') {
                        if (Storage::disk('optima')->exists($path_row5 . $request['fileName5'])) {
                            $fileUpload5 = "Upload Successfully";
                        } else {
                            $fileUpload5 = "Upload Failed";
                        }
                    }
                }

                //upload attachment6
                $path_row6 = '/assets/media/promo/' . $resVal->id . '/row6/';
                $fileUpload6 = "No File Upload attachment6";
                if($request['fileName6']) {
                    if ($request['fileName6'] !== '') {
                        if (Storage::disk('optima')->exists($path_row6 . $request['fileName6'])) {
                            $fileUpload6 = "Upload Successfully";
                        } else {
                            $fileUpload6 = "Upload Failed";
                        }
                    }
                }

                //upload attachment7
                $path_row7 = '/assets/media/promo/' . $resVal->id . '/row7/';
                $fileUpload7 = "No File Upload attachment7";
                if($request['fileName7']) {
                    if ($request['fileName7'] !== '') {
                        if (Storage::disk('optima')->exists($path_row7 . $request['fileName7'])) {
                            $fileUpload7 = "Upload Successfully";
                        } else {
                            $fileUpload7 = "Upload Failed";
                        }
                    }
                }

                if ($resVal->isSendEmail) {
                    // send email to approver
                    $emailApprover = $this->sendEmailApprover($resVal->dataEmail->userIdApprover, $resVal->dataEmail->userNameApprover, $resVal->id, $resVal->dataEmail->emailApprover, "[APPROVAL NOTIF] Promo requires approval (" . $resVal->refId . ")", date("Y", strtotime($request['startPromo'])));
                    if (!json_decode($emailApprover)->error) {
                        Log::info("Promo ID " . $resVal->refId . " Has been updated successfully and send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                            'attachment5'=> $fileUpload5,
                            'attachment6'=> $fileUpload6,
                            'attachment7'=> $fileUpload7,
                        );
                    } else {
                        Log::warning("Promo ID " . $resVal->refId . " Has been updated successfully but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => "Promo ID " . $resVal->refId . " Has been updated successfully  but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")",
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                            'attachment5'=> $fileUpload5,
                            'attachment6'=> $fileUpload6,
                            'attachment7'=> $fileUpload7,
                        );
                    }
                } else {
                    return array(
                        'error'     => false,
                        'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                        'attachment1'=> $fileUpload1,
                        'attachment2'=> $fileUpload2,
                        'attachment3'=> $fileUpload3,
                        'attachment4'=> $fileUpload4,
                        'attachment5'=> $fileUpload5,
                        'attachment6'=> $fileUpload6,
                        'attachment7'=> $fileUpload7,
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Update Failed"
            );
        }
    }

    public function updatePromoDC(Request $request): array
    {
        $api = config('app.api') . '/promo/sendbackv2dc';

        $arrFiles = array();
        if($request['fileName1']) {
            if ($request['fileName1'] !== '') {
                $fileAttach = [
                    'docLink' => 'row1',
                    'fileName' => $request['fileName1']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName2']) {
            if ($request['fileName2'] !== '') {
                $fileAttach = [
                    'docLink' => 'row2',
                    'fileName' => $request['fileName2']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName3']) {
            if ($request['fileName3'] !== '') {
                $fileAttach = [
                    'docLink' => 'row3',
                    'fileName' => $request['fileName3']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName4']) {
            if ($request['fileName4'] !== '') {
                $fileAttach = [
                    'docLink' => 'row4',
                    'fileName' => $request['fileName4']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }

        $promo = [
            'promoId'               => $request['promoId'],
            'periode'               => $request['period'],
            'entityId'              => $request['entityId'],
            'groupBrandId'          => $request['groupBrandId'],
            'categoryId'            => $request['categoryId'],
            'subCategoryId'         => ($request['subCategoryId'] ?? 0),
            'activityId'            => ($request['activityId'] ?? 0),
            'subActivityId'         => $request['subActivityId'],
            'subActivityTypeId'     => $request['subActivityTypeId'],
            'activityDesc'          => $request['activityDesc'],
            'startPromo'            => $request['startPromo'],
            'endPromo'              => $request['endPromo'],
            'distributorId'         => $request['distributorId'],
            'channelId'             => $request['channelId'],
            'subChannelId'          => ($request['subChannelId'] ?? 0),
            'accountId'             => ($request['accountId'] ?? 0),
            'subAccountId'          => ($request['subAccountId'] ?? 0),
            'baseline'              => str_replace(',', '', $request['baseline']),
            'upLift'                => str_replace(',', '', $request['upLift']),
            'totalSales'            => str_replace(',', '', $request['totalSales']),
            'salesContribution'     => str_replace(',', '', $request['salesContribution']),
            'storesCoverage'        => str_replace(',', '', $request['storesCoverage']),
            'redemptionRate'        => str_replace(',', '', $request['redemptionRate']),
            'cr'                    => str_replace(',', '', $request['cr']),
            'cost'                  => str_replace(',', '', $request['cost']),
            "modifReason"           => $request['modifReason'],
            "notes"                 => ($request['notes_message'] ?? ''),
            "initiator_notes"       => ($request['initiatorNotes'] ?? "")
        ];

        $body = [
            'promo'                 => $promo,
            'region'                => json_decode($request['region']),
            'sku'                   => json_decode($request['sku']),
            'mechanism'             => json_decode($request['mechanism']),
            'attachment'            => $arrFiles,
        ];
        Log::info('PUT API ' . $api);
        Log::info('payload ' . json_encode($body));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                if (!Storage::disk('optima')->exists('/assets/media/promo')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/promo');
                }

                //upload attachment1
                $path_row1 = '/assets/media/promo/' . $resVal->id . '/row1/';
                $fileUpload1 = "No File Upload attachment1";
                if($request['fileName1']) {
                    if ($request['fileName1'] !== '') {
                        if (Storage::disk('optima')->exists($path_row1 . $request['fileName1'])) {
                            $fileUpload1 = "Upload Successfully";
                        } else {
                            $fileUpload1 = "Upload Failed";
                        }
                    }
                }

                //upload attachment2
                $path_row2 = '/assets/media/promo/' . $resVal->id . '/row2/';
                $fileUpload2 = "No File Upload attachment2";
                if($request['fileName2']) {
                    if ($request['fileName2'] !== '') {
                        if (Storage::disk('optima')->exists($path_row2 . $request['fileName2'])) {
                            $fileUpload2 = "Upload Successfully";
                        } else {
                            $fileUpload2 = "Upload Failed";
                        }
                    }
                }

                //upload attachment3
                $path_row3 = '/assets/media/promo/' . $resVal->id . '/row3/';
                $fileUpload3 = "No File Upload attachment3";
                if($request['fileName3']) {
                    if ($request['fileName3'] !== '') {
                        if (Storage::disk('optima')->exists($path_row3 . $request['fileName3'])) {
                            $fileUpload3 = "Upload Successfully";
                        } else {
                            $fileUpload3 = "Upload Failed";
                        }
                    }
                }

                //upload attachment4
                $path_row4 = '/assets/media/promo/' . $resVal->id . '/row4/';
                $fileUpload4 = "No File Upload attachment4";
                if($request['fileName4']) {
                    if ($request['fileName4'] !== '') {
                        if (Storage::disk('optima')->exists($path_row4 . $request['fileName4'])) {
                            $fileUpload4 = "Upload Successfully";
                        } else {
                            $fileUpload4 = "Upload Failed";
                        }
                    }
                }

                if ($resVal->isSendEmail) {
                    // send email to approver
                    $emailApprover = $this->sendEmailApprover($resVal->dataEmail->userIdApprover, $resVal->dataEmail->userNameApprover, $resVal->id, $resVal->dataEmail->emailApprover, "[APPROVAL NOTIF] Promo requires approval (" . $resVal->refId . ")", date("Y", strtotime($request['startPromo'])));
                    if (!json_decode($emailApprover)->error) {
                        Log::info("Promo ID " . $resVal->refId . " Has been updated successfully and send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                        );
                    } else {
                        Log::warning("Promo ID " . $resVal->refId . " Has been updated successfully but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => "Promo ID " . $resVal->refId . " Has been updated successfully  but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")",
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                        );
                    }
                } else {
                    return array(
                        'error'     => false,
                        'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                        'attachment1'=> $fileUpload1,
                        'attachment2'=> $fileUpload2,
                        'attachment3'=> $fileUpload3,
                        'attachment4'=> $fileUpload4,
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('PUT API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('PUT API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Update Failed"
            );
        }
    }

    public function updatePromoRecon(Request $request): array
    {
        $api = config('app.api') . '/promo/reconv2';

        $arrFiles = array();
        if($request['fileName1']) {
            if ($request['fileName1'] !== '') {
                $fileAttach = [
                    'docLink' => 'row1',
                    'fileName' => $request['fileName1']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName2']) {
            if ($request['fileName2'] !== '') {
                $fileAttach = [
                    'docLink' => 'row2',
                    'fileName' => $request['fileName2']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName3']) {
            if ($request['fileName3'] !== '') {
                $fileAttach = [
                    'docLink' => 'row3',
                    'fileName' => $request['fileName3']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName4']) {
            if ($request['fileName4'] !== '') {
                $fileAttach = [
                    'docLink' => 'row4',
                    'fileName' => $request['fileName4']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName5']) {
            if ($request['fileName5'] !== '') {
                $fileAttach = [
                    'docLink' => 'row5',
                    'fileName' => $request['fileName5']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName6']) {
            if ($request['fileName6'] !== '') {
                $fileAttach = [
                    'docLink' => 'row6',
                    'fileName' => $request['fileName6']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName7']) {
            if ($request['fileName7'] !== '') {
                $fileAttach = [
                    'docLink' => 'row7',
                    'fileName' => $request['fileName7']
                ];
                array_push($arrFiles, $fileAttach);
            }
        }

        $promo = [
            'promoId'               => $request['promoId'],
            'periode'               => $request['period'],
            'entityId'              => $request['entityId'],
            'groupBrandId'          => $request['groupBrandId'],
            'categoryId'            => $request['categoryId'],
            'subCategoryId'         => $request['subCategoryId'],
            'activityId'            => $request['activityId'],
            'subActivityId'         => $request['subActivityId'],
            'subActivityTypeId'     => $request['subActivityTypeId'],
            'activityDesc'          => $request['activityDesc'],
            'startPromo'            => $request['startPromo'],
            'endPromo'              => $request['endPromo'],
            'distributorId'         => $request['distributorId'],
            'channelId'             => $request['channelId'],
            'subChannelId'          => $request['subChannelId'],
            'accountId'             => $request['accountId'],
            'subAccountId'          => $request['subAccountId'],
            'baseline'              => str_replace(',', '', $request['baseline']),
            'upLift'                => str_replace(',', '', $request['upLift']),
            'totalSales'            => str_replace(',', '', $request['totalSales']),
            'salesContribution'     => str_replace(',', '', $request['salesContribution']),
            'storesCoverage'        => str_replace(',', '', $request['storesCoverage']),
            'redemptionRate'        => str_replace(',', '', $request['redemptionRate']),
            'cr'                    => str_replace(',', '', $request['cr']),
            'cost'                  => str_replace(',', '', $request['cost']),
            "notes"                 => ($request['notes_message'] ?? ''),
            "initiator_notes"       => $request['initiatorNotes'],
            "modifReason"           => $request['modifReason'],
        ];

        $body = [
            'promo'                 => $promo,
            'region'                => json_decode($request['region']),
            'sku'                   => json_decode($request['sku']),
            'mechanism'             => json_decode($request['mechanism']),
            'attachment'            => $arrFiles,
        ];
        Log::info('PUT API ' . $api);
        Log::info('payload ' . json_encode($body));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                if (!Storage::disk('optima')->exists('/assets/media/promo')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/promo');
                }

                $pathPromoId = '/assets/media/promo/' . $resVal->id;
                $pathTemp = '/assets/media/promo/' . $request['uuid'];

                if (!Storage::disk('optima')->exists($pathTemp)) {
                    Storage::disk('optima')->makeDirectory($pathTemp);
                }

                if (Storage::disk('optima')->move($pathTemp, $pathPromoId)) {
                    Log::info(json_encode([
                        'error'     => false,
                        'message'   => "Upload Successfully",
                        'userid'    => $request->session()->get('profile')
                    ]));
                } else {
                    Log::warning(json_encode([
                        'error'     => true,
                        'message'   => 'Upload Failed',
                        'userid'    => $request->session()->get('profile')
                    ]));
                }

                //upload attachment1
                $path_row1 = '/assets/media/promo/' . $resVal->id . '/row1/';
                $fileUpload1 = "No File Upload attachment1";
                if($request['fileName1']) {
                    if ($request['fileName1'] !== '') {
                        if (Storage::disk('optima')->exists($path_row1 . $request['fileName1'])) {
                            $fileUpload1 = "Upload Successfully";
                        } else {
                            $fileUpload1 = "Upload Failed";
                        }
                    }
                }

                //upload attachment2
                $path_row2 = '/assets/media/promo/' . $resVal->id . '/row2/';
                $fileUpload2 = "No File Upload attachment2";
                if($request['fileName2']) {
                    if ($request['fileName2'] !== '') {
                        if (Storage::disk('optima')->exists($path_row2 . $request['fileName2'])) {
                            $fileUpload2 = "Upload Successfully";
                        } else {
                            $fileUpload2 = "Upload Failed";
                        }
                    }
                }

                //upload attachment3
                $path_row3 = '/assets/media/promo/' . $resVal->id . '/row3/';
                $fileUpload3 = "No File Upload attachment3";
                if($request['fileName3']) {
                    if ($request['fileName3'] !== '') {
                        if (Storage::disk('optima')->exists($path_row3 . $request['fileName3'])) {
                            $fileUpload3 = "Upload Successfully";
                        } else {
                            $fileUpload3 = "Upload Failed";
                        }
                    }
                }

                //upload attachment4
                $path_row4 = '/assets/media/promo/' . $resVal->id . '/row4/';
                $fileUpload4 = "No File Upload attachment4";
                if($request['fileName4']) {
                    if ($request['fileName4'] !== '') {
                        if (Storage::disk('optima')->exists($path_row4 . $request['fileName4'])) {
                            $fileUpload4 = "Upload Successfully";
                        } else {
                            $fileUpload4 = "Upload Failed";
                        }
                    }
                }

                //upload attachment5
                $path_row5 = '/assets/media/promo/' . $resVal->id . '/row5/';
                $fileUpload5 = "No File Upload attachment5";
                if($request['fileName5']) {
                    if ($request['fileName5'] !== '') {
                        if (Storage::disk('optima')->exists($path_row5 . $request['fileName5'])) {
                            $fileUpload5 = "Upload Successfully";
                        } else {
                            $fileUpload5 = "Upload Failed";
                        }
                    }
                }

                //upload attachment6
                $path_row6 = '/assets/media/promo/' . $resVal->id . '/row6/';
                $fileUpload6 = "No File Upload attachment6";
                if($request['fileName6']) {
                    if ($request['fileName6'] !== '') {
                        if (Storage::disk('optima')->exists($path_row6 . $request['fileName6'])) {
                            $fileUpload6 = "Upload Successfully";
                        } else {
                            $fileUpload6 = "Upload Failed";
                        }
                    }
                }

                //upload attachment7
                $path_row7 = '/assets/media/promo/' . $resVal->id . '/row7/';
                $fileUpload7 = "No File Upload attachment7";
                if($request['fileName7']) {
                    if ($request['fileName7'] !== '') {
                        if (Storage::disk('optima')->exists($path_row7 . $request['fileName7'])) {
                            $fileUpload7 = "Upload Successfully";
                        } else {
                            $fileUpload7 = "Upload Failed";
                        }
                    }
                }

                if ($resVal->isSendEmail) {
                    // send email to approver
                    $emailApprover = $this->sendEmailApproverRecon($resVal->dataEmail->userIdApprover, $resVal->dataEmail->userNameApprover, $resVal->id, $resVal->dataEmail->emailApprover, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $resVal->refId . ")", date("Y", strtotime($request['startPromo'])));
                    if (!json_decode($emailApprover)->error) {
                        Log::info("Promo ID " . $resVal->refId . " Has been updated successfully and send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                            'attachment5'=> $fileUpload5,
                            'attachment6'=> $fileUpload6,
                            'attachment7'=> $fileUpload7,
                        );
                    } else {
                        Log::warning("Promo ID " . $resVal->refId . " Has been updated successfully but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => "Promo ID " . $resVal->refId . " Has been updated successfully  but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")",
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                            'attachment5'=> $fileUpload5,
                            'attachment6'=> $fileUpload6,
                            'attachment7'=> $fileUpload7,
                        );
                    }
                } else {
                    return array(
                        'error'     => false,
                        'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                        'attachment1'=> $fileUpload1,
                        'attachment2'=> $fileUpload2,
                        'attachment3'=> $fileUpload3,
                        'attachment4'=> $fileUpload4,
                        'attachment5'=> $fileUpload5,
                        'attachment6'=> $fileUpload6,
                        'attachment7'=> $fileUpload7,
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Update Failed"
            );
        }
    }

    public function updatePromoReconDC(Request $request): array
    {
        $api = config('app.api') . '/promo/reconv2dc';

        $arrFiles = array();
        if($request['fileName1']) {
            if ($request['fileName1'] !== '') {
                $fileAttach = [
                    'docLink' => 'row1',
                    'fileName' => $request['fileName1']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName2']) {
            if ($request['fileName2'] !== '') {
                $fileAttach = [
                    'docLink' => 'row2',
                    'fileName' => $request['fileName2']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName3']) {
            if ($request['fileName3'] !== '') {
                $fileAttach = [
                    'docLink' => 'row3',
                    'fileName' => $request['fileName3']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }
        if($request['fileName4']) {
            if ($request['fileName4'] !== '') {
                $fileAttach = [
                    'docLink' => 'row4',
                    'fileName' => $request['fileName4']

                ];
                array_push($arrFiles, $fileAttach);
            }
        }

        $promo = [
            'promoId'               => $request['promoId'],
            'periode'               => $request['period'],
            'entityId'              => $request['entityId'],
            'groupBrandId'          => $request['groupBrandId'],
            'categoryId'            => $request['categoryId'],
            'subCategoryId'         => ($request['subCategoryId'] ?? 0),
            'activityId'            => ($request['activityId'] ?? 0),
            'subActivityId'         => $request['subActivityId'],
            'subActivityTypeId'     => $request['subActivityTypeId'],
            'activityDesc'          => $request['activityDesc'],
            'startPromo'            => $request['startPromo'],
            'endPromo'              => $request['endPromo'],
            'distributorId'         => $request['distributorId'],
            'channelId'             => $request['channelId'],
            'subChannelId'          => ($request['subChannelId'] ?? 0),
            'accountId'             => ($request['accountId'] ?? 0),
            'subAccountId'          => ($request['subAccountId'] ?? 0),
            'baseline'              => str_replace(',', '', $request['baseline']),
            'upLift'                => str_replace(',', '', $request['upLift']),
            'totalSales'            => str_replace(',', '', $request['totalSales']),
            'salesContribution'     => str_replace(',', '', $request['salesContribution']),
            'storesCoverage'        => str_replace(',', '', $request['storesCoverage']),
            'redemptionRate'        => str_replace(',', '', $request['redemptionRate']),
            'cr'                    => str_replace(',', '', $request['cr']),
            'cost'                  => str_replace(',', '', $request['cost']),
            "modifReason"           => $request['modifReason'],
            "notes"                 => ($request['notes_message'] ?? ''),
            "initiator_notes"       => ($request['initiatorNotes'] ?? "")
        ];

        $body = [
            'promo'                 => $promo,
            'region'                => json_decode($request['region']),
            'sku'                   => json_decode($request['sku']),
            'mechanism'             => json_decode($request['mechanism']),
            'attachment'            => $arrFiles,
        ];
        Log::info('PUT API ' . $api);
        Log::info('payload ' . json_encode($body));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                if (!Storage::disk('optima')->exists('/assets/media/promo')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/promo');
                }

                //upload attachment1
                $path_row1 = '/assets/media/promo/' . $resVal->id . '/row1/';
                $fileUpload1 = "No File Upload attachment1";
                if($request['fileName1']) {
                    if ($request['fileName1'] !== '') {
                        if (Storage::disk('optima')->exists($path_row1 . $request['fileName1'])) {
                            $fileUpload1 = "Upload Successfully";
                        } else {
                            $fileUpload1 = "Upload Failed";
                        }
                    }
                }

                //upload attachment2
                $path_row2 = '/assets/media/promo/' . $resVal->id . '/row2/';
                $fileUpload2 = "No File Upload attachment2";
                if($request['fileName2']) {
                    if ($request['fileName2'] !== '') {
                        if (Storage::disk('optima')->exists($path_row2 . $request['fileName2'])) {
                            $fileUpload2 = "Upload Successfully";
                        } else {
                            $fileUpload2 = "Upload Failed";
                        }
                    }
                }

                //upload attachment3
                $path_row3 = '/assets/media/promo/' . $resVal->id . '/row3/';
                $fileUpload3 = "No File Upload attachment3";
                if($request['fileName3']) {
                    if ($request['fileName3'] !== '') {
                        if (Storage::disk('optima')->exists($path_row3 . $request['fileName3'])) {
                            $fileUpload3 = "Upload Successfully";
                        } else {
                            $fileUpload3 = "Upload Failed";
                        }
                    }
                }

                //upload attachment4
                $path_row4 = '/assets/media/promo/' . $resVal->id . '/row4/';
                $fileUpload4 = "No File Upload attachment4";
                if($request['fileName4']) {
                    if ($request['fileName4'] !== '') {
                        if (Storage::disk('optima')->exists($path_row4 . $request['fileName4'])) {
                            $fileUpload4 = "Upload Successfully";
                        } else {
                            $fileUpload4 = "Upload Failed";
                        }
                    }
                }

                if ($resVal->isSendEmail) {
                    // send email to approver
                    $emailApprover = $this->sendEmailApproverRecon($resVal->dataEmail->userIdApprover, $resVal->dataEmail->userNameApprover, $resVal->id, $resVal->dataEmail->emailApprover, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $resVal->refId . ")", date("Y", strtotime($request['startPromo'])));
                    if (!json_decode($emailApprover)->error) {
                        Log::info("Promo ID " . $resVal->refId . " Has been updated successfully and send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                        );
                    } else {
                        Log::warning("Promo ID " . $resVal->refId . " Has been updated successfully but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")");
                        return array(
                            'error'     => false,
                            'message'   => "Promo ID " . $resVal->refId . " Has been updated successfully  but can't send email notification to " . $resVal->dataEmail->userIdApprover. " (" . $resVal->dataEmail->emailApprover.")",
                            'attachment1'=> $fileUpload1,
                            'attachment2'=> $fileUpload2,
                            'attachment3'=> $fileUpload3,
                            'attachment4'=> $fileUpload4,
                        );
                    }
                } else {
                    return array(
                        'error'     => false,
                        'message'   => 'Promo ID ' . $resVal->refId . ' Has been updated successfully',
                        'attachment1'=> $fileUpload1,
                        'attachment2'=> $fileUpload2,
                        'attachment3'=> $fileUpload3,
                        'attachment4'=> $fileUpload4,
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Update Failed"
            );
        }
    }

    protected function sendEmailApproverRecon($userApprover, $nameApprover, $id, $email, $subject, $yearPromo): bool|string
    {
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        if ($yearPromo >= 2025){
            $api_promo = config('app.api') . '/promo/display/email/id';
        } else {
            $api_promo = config('app.api') . '/promo/display/email/id';
        }

        $query_promo = [
            'id'           => $id,
        ];
        Log::info('get API ' . $api_promo);
        Log::info('payload ' . json_encode($query_promo));

        $api_email = config('app.api') . '/tools/email';
        try {
            $responsePromo =  Http::timeout(180)->withToken($this->token)->get($api_promo, $query_promo);
            if ($responsePromo->status() === 200) {
                $data = json_decode($responsePromo)->values;
                if (!$data) $data = array();

                $ar_fileattach = array();
                if ($yearPromo >= 2025){
                    $viewEmail = 'Promo::promo-send-back.email-approval-recon';

                    $attach = array();
                    for ($i=0; $i<count($data->attachments); $i++) {
                        if ($data->attachments[$i]->docLink =='row1') $attach['row1'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row2') $attach['row2'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row3') $attach['row3'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row4') $attach['row4'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row5') $attach['row5'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row6') $attach['row6'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row7') $attach['row7'] = $data->attachments[$i]->fileName;
                    }
                    if (!empty($data->attachments)) array_push($ar_fileattach, $attach);

                    $urlid = $data->promoHeader->id;
                    $urlrefid = urlencode(MyEncrypt::encrypt($data->promoHeader->refId));

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                } else {
                    $viewEmail = 'Promo::promo-send-back.email-approval-recon';

                    $attach = array();
                    for ($i=0; $i<count($data->attachments); $i++) {
                        if ($data->attachments[$i]->docLink =='row1') $attach['row1'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row2') $attach['row2'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row3') $attach['row3'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row4') $attach['row4'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row5') $attach['row5'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row6') $attach['row6'] = $data->attachments[$i]->fileName;
                        if ($data->attachments[$i]->docLink =='row7') $attach['row7'] = $data->attachments[$i]->fileName;
                    }
                    if (!empty($data->attachments)) array_push($ar_fileattach, $attach);

                    $urlid = $data->promoHeader->id;
                    $urlrefid = urlencode(MyEncrypt::encrypt($data->promoHeader->refId));

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                }

                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view($viewEmail, compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $email,
                    'subject'   => $subject,
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $data);
                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success");
                    return json_encode([
                        'error'     => false,
                        'data'      => [],
                        'message'   => "Send Email Success"
                    ]);
                } else {
                    Log::info("error : Send Email Failed");
                    return json_encode([
                        'error'     => true,
                        'data'      => [],
                        'message'   => "error : Send Email Failed"
                    ]);
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => "error : Get Data Promo Failed"
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }
}
