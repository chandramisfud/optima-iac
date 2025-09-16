<?php

namespace App\Modules\Promo\Controllers;

use App\Helpers\MyEncrypt;
use App\Exports\Export;
use Exception;
use GuzzleHttp\Client;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;
use File;

class PromoReconSendBack extends Controller
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
        $title = "Promo Send Back Reconciliation";
        return view('Promo::promo-recon-send-back.index', compact('title'));
    }

    public function form(Request $request)
    {
        $category = MyEncrypt::decrypt($request['c']);
        if($category === 'DC') {
            $title = "Promo Send Back Reconciliation (Distributor Cost)";
            return view('Promo::promo-recon-send-back.dc-form', compact('title'));
        } else {
            $title = "Promo Send Back Reconciliation (Retailer Cost)";
            return view('Promo::promo-recon-send-back.form', compact('title'));
        }
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/promo/sendbackrecon';
        try {
            $query = [
                'year'                      => $request['period'],
                'categoryId'                => $request['categoryId'] ?? 0,
                'entity'                    => $request['entityId'] ?? 0,
                'distributor'               => $request['distributorId'] ?? 0,
                'budgetparent'              => $request['budgetparent'] ?? 0,
                'channel'                   => $request['channel'] ?? 0,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
                }
                return json_encode([
                    "data" => $resVal
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function getDataById(Request $request)
    {
        $api = config('app.api'). '/promo/sendbackrecon/id';
        $query = [
            'id'        => $request->id,
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
        } catch (Exception $e) {
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
        $api = config('app.api') . '/promo/sendbackrecon/category';
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api') . '/promo/sendbackrecon/entity';
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

    public function getListDistributor(Request $request)
    {
        $api = config('app.api') . '/promo/sendbackrecon/distributor';
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api') . '/promo/creation/channel';
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

    public function getListSubCategory(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            $dataActive = ($request['isDeleted'] ?? '0');
            $ar_parent = array();
            array_push($ar_parent, $request['categoryId']);

            $query = [
                'budgetId' => 0,
                'attribute' => 'subcategory',
                'arrayParent' => $ar_parent,
                'isDeleted' => $dataActive,
            ];
            Log::info('get API subCategory ' . $api);
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

    public function getListActivity(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['subCategoryId']);

            $query = [
                'budgetId'      => 0,
                'attribute'     => 'activity',
                'arrayParent'   => $ar_parent,
                'isDeleted'     => $request['isDeleted'],
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

    public function getListSubActivity(Request $request)
    {
        $api = config('app.api') . '/promo/recon/mapping/subactivity/activityid';
        try {
            $query = [
                'activityId'    => $request['activityId'],
            ];
            Log::info('get Sub Activity ' . $api);
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

    public function getListSubActivityBySubCategoryId(Request $request)
    {
        $api = config('app.api') . '/promo/activity/subactivity/subcategoryid';
        try {
            $query = [
                'subCategoryId' => $request['subCategoryId'],
            ];
            Log::info('get ' . $api);
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

    public function getCategoryByCategoryShortDesc(Request $request)
    {
        $api = config('app.api') . '/master/category/shortdesc';
        try {
            if (MyEncrypt::decrypt($request['categoryShortDesc']) === 'TS') {
                $categoryShortDesc = 'RC';
            } else if (MyEncrypt::decrypt($request['categoryShortDesc']) === 'RC') {
                $categoryShortDesc = 'RC';
            } else {
                $categoryShortDesc = 'DC';
            }

            $query = [
                'categoryShortDesc' => $categoryShortDesc,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values
                );
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

    public function getBaseline(Request $request)
    {
        $api = config('app.api') . '/promo/baseline';
        try {
            $body = [
                'promoId'           => $request['promoId'],
                'period'            => $request['period'],
                'dateCreation'      => date('Y-m-d'),
                'typePromo'         => 3,
                'subCategoryId'     => $request['subCategoryId'],
                'subActivityId'     => $request['subActivityId'],
                'distributorId'     => $request['distributorId'],
                'startPromo'        => $request['startPromo'],
                'endPromo'          => $request['endPromo'],
                'arrayRegion'       => $request['region'],
                'arrayChannel'      => $request['channel'],
                'arraySubChannel'   => $request['subChannel'],
                'arrayAccount'      => $request['account'],
                'arraySubAccount'   => $request['subAccount'],
                'arrayBrand'        => $request['brand'],
                'arraySKU'          => $request['sku'],
            ];

            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($body));
            $response = Http::timeout(180)->withToken($this->token)->post($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
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
                'message' => $e->getMessage()
            );
        }
    }

    public function getListRegion(Request $request)
    {
        $api = config('app.api') . '/promo/attribute/region';
        try {
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
                'budgetId'      => 0,
                'attribute'     => 'product',
                'arrayParent'   => $request['brandId'],
                'isDeleted'     => $dataActive,
            ];
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

    public function getListMechanism(Request $request)
    {
        $api = config('app.api') . '/promo/mechanism';
        try {
            $query = [
                'entityId' => $request['entityId'],
                'subCategoryId' => $request['subCategoryId'],
                'activityId' => $request['activityId'],
                'subActivityId' => $request['subActivityId'],
                'skuId' => $request['skuId'],
                'channelId' => $request['channelId'],
                'startDate' => $request['startDate'],
                'endDate' => $request['endDate'],
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

    public function getListSourceBudget(Request $request)
    {
        $api = config('app.api') . '/promo/sendbackrecon/sourcebudget';
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/promo/sendbackrecon';
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
            "activityDesc"              => ($request['activityDesc'] ?? ""),
            "startPromo"                => $request['startPromo'],
            "endPromo"                  => $request['endPromo'],
            "mechanisme1"               => '',
            "mechanisme2"               => '',
            "mechanisme3"               => '',
            "mechanisme4"               => '',
            "investment"                => str_replace(',', '', $request['investmentRecon']),
            "normalSales"               => str_replace(',', '', $request['baselineSalesRecon']),
            "incrSales"                 => str_replace(',', '', $request['incrementSalesRecon']),
            "roi"                       => str_replace(',', '', $request['roiRecon']),
            "costRatio"                 => str_replace(',', '', $request['costRatioRecon']),
            "statusApproval"            => ($request['statusApproval'] ?? ''),
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
            'mechanisms'        => json_decode($request['dataMechanism']),
            'promoAttachment'   => $arr_Files,
            "reconciled"        => false,
            "reconciledUpd"     => false
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                if(json_decode($response)->error){
                    $message = json_decode($response)->message;
                    Log::warning('post API ' . $api);
                    Log::warning($message);
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => $message
                    );
                } else {
                    $values = json_decode($response)->values;
                    if ($values->major_changes) {
                        // send email to approver
                        $emailApprover = $this->sendEmailApprover($values->userid_approver, $values->username_approver, $values->id, $values->email_approver, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $values->refId . ")");
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
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Save Failed"
            );
        }
    }

    protected function sendEmailApprover($userApprover, $nameApprover, $id, $email, $subject): bool|string
    {
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/display/id';
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
                    'nameApprover'  => $nameApprover
                ]);
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view('Promo::promo-recon.email-approval', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $email,
                    'subject'   => $subject,
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
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
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/promo/sendbackrecon';
            $query = [
                'year'                      => $request['period'],
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
                    $arr[] = $fields->activityDesc;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->approveOn));
                    $arr[] = $fields->approvalNotes;

                    $result[] = $arr;
                }
                $title = 'A1:F1'; //Report Title Bold and merge
                $header = 'A7:F7'; //Header Column Bold and color
                $heading = [
                    ['Promo Sendback Reconciliation'],
                    ['Budget Year : ' . $request['period']],
                    ['Category : ' . $request['category']],
                    ['Entity : ' . $request['entity']],
                    ['Distributor : ' . $request['distributor']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo ID', 'Activity Description', 'Promo Start', 'Promo End', 'Sendback Date', 'Notes']
                ];
                $formatCell =  [
                    'C' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'D' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'E' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                ];
                $filename = 'PromoSendbackRecon-';
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
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

    public function encCategoryShortDesc ($categoryShortDesc)
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }
}
