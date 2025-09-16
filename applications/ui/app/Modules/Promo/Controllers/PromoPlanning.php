<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Export;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Support\Carbon;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;

class PromoPlanning extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    function checkAccess(Request $request)
    {
        try {
            $list_access = $request->session()->get('user_access');
            $menu_exist = $this->findObjectById($list_access, $request['menuid']);
            if ($menu_exist) {
                foreach ($list_access as $list) {
                    if ($list->menuid == $request['menuid']) {
                        $access_name = $request['access_name'];
                        if ($list->$access_name) {
                            return json_encode([
                                "error"     => false
                            ]);
                        } else {
                            return json_encode([
                                'error'     => true,
                                'message'   => "Feature is not allowed for this user"
                            ]);
                        }
                    }
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => "Feature is not allowed for this user"
                ]);
            }
        } catch (Exception $e) {
            Log::error('Auth->checkAccess ' . $e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => $e->getMessage()
            ]);
        }
    }

    private function findObjectById($array, $id): bool
    {
        foreach ( $array as $element ) {
            if ( $id == $element->menuid ) {
                return true;
            }
        }

        return false;
    }

    public function landingPage()
    {
        $title = "Promo Planning";
        return view('Promo::promo-planning.index', compact('title'));
    }

    public function toBeCreated()
    {
        $title = "Promo To Be Created";
        return view('Promo::promo-planning.to-be-created', compact('title'));
    }

    private function encCategoryId($categoryId): string
    {
        try {
            return MyEncrypt::encrypt($categoryId);
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api') . '/promo/planning';
        try {
            $page = 0;
            if ($request['length'] > -1) {
                $page = ($request['start'] / $request['length']);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];
            $search = ($request['search']['value'] ?? "");
            $length = $request['length'];
            $SortColumn = $request['columns'][(int)$column]['data'];

            $query = [
                'year'          => $request['period'],
                'entity'        => $request['entityId'],
                'distributor'   => $request['distributorId'],
                'createFrom'    => $request['startFrom'],
                'createTo'      => $request['startTo'],
                'startFrom'     => $request['startFrom'],
                'startTo'       => $request['startTo'],
                'Search'        => $search,
                'PageNumber'    => $page,
                'PageSize'      => (int)$length,
                'SortColumn'    => $SortColumn,
                'SortDirection' => $sort
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $data = json_decode($response)->values->data;
                foreach ($data as $r) {
                    $r->categoryShortDesc = urlencode($this->encCategoryId('RC'));
                }
                return json_encode([
                    "draw" => (int)$request['draw'],
                    "data" => $data,
                    "recordsTotal" => json_decode($response)->values->recordsTotal,
                    "recordsFiltered" => json_decode($response)->values->recordsFiltered
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
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

    public function getListToBeCreated(Request $request)
    {
        $api = config('app.api') . '/promo/tobecreated';
        try {
            $page = 0;
            if ($request['length'] > -1) {
                $page = ($request['start'] / $request['length']);
            }
            $search = ($request['search']['value'] ?? "");
            $length = $request['length'];

            $query = [
                'search' => $search,
                'pageNumber' => $page,
                'pageSize' => (int)$length,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "draw" => (int)$request['draw'],
                    "data" => json_decode($response)->values->data,
                    "recordsTotal" => json_decode($response)->values->recordsTotal,
                    "recordsFiltered" => json_decode($response)->values->recordsFiltered
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
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

    public function getListEntity()
    {
        $api = config('app.api') . '/promo/entity';
        Log::info('get API ' . $api);
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
        $api = config('app.api') . '/promo/distributor';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['entityId']);

            $query = [
                'budgetId' => 0,
                'entityId' => $ar_parent,
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

    public function getListSubCategory(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            $ar_parent = array();
            array_push($ar_parent, 3);
            $query = [
                'budgetId'      => 0,
                'attribute'     => 'subcategory',
                'arrayParent'   => $ar_parent,
                'isDeleted'     => $request['isDeleted'],
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
        $api = config('app.api') . '/promo/attribute';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['activityId']);

            $query = [
                'budgetId'      => 0,
                'attribute'     => 'subactivity',
                'arrayParent'   => $ar_parent,
                'isDeleted'     => $request['isDeleted'],
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

    public function getListChannel()
    {
        $api = config('app.api') . '/promo/planning/channel';
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

    public function getListEditChannel(Request $request)
    {
        $api = config('app.api') . '/promo/planning/channel/promoplanningid';
        try {
            $query = [
                'promoPlanningId' => $request['promoPlanningId'],
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

    public function getListSubChannel(Request $request)
    {
        $api = config('app.api') . '/promo/planning/subchannel';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['channelId']);

            $query = [
                'arrayChannel' => $ar_parent,
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

    public function getListEditSubChannel(Request $request)
    {
        $api = config('app.api') . '/promo/planning/subchannel/promoplanningid';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['channelId']);

            $query = [
                'promoPlanningId' => $request['promoPlanningId'],
                'arrayChannel' => $ar_parent,
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

    public function getListAccount(Request $request)
    {
        $api = config('app.api') . '/promo/planning/account';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['subChannelId']);

            $query = [
                'arraySubChannel' => $ar_parent,
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

    public function getListEditAccount(Request $request)
    {
        $api = config('app.api') . '/promo/planning/account/promoplanningid';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['subChannelId']);

            $query = [
                'promoPlanningId' => $request['promoPlanningId'],
                'arraySubChannel' => $ar_parent,
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

    public function getListSubAccount(Request $request)
    {
        $api = config('app.api') . '/promo/planning/subaccount';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['accountId']);

            $query = [
                'arrayAccount' => $ar_parent,
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

    public function getListEditSubAccount(Request $request)
    {
        $api = config('app.api') . '/promo/planning/subaccount/promoplanningid';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request['accountId']);

            $query = [
                'promoPlanningId' => $request['promoPlanningId'],
                'arrayAccount' => $ar_parent,
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

    public function getListRegion()
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
            $ar_parent = array();
            array_push($ar_parent, $request['entityId']);

            $query = [
                'budgetId'      => 0,
                'attribute'     => 'brand',
                'arrayParent'   => $ar_parent,
                'isDeleted'     => ($request['isDeleted'] ?? "0"),
            ];
            Log::info('get API ' . $api . ' Brand');
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

    public function getListSKU(Request $request)
    {
        $api = config('app.api') . '/promo/attribute';
        try {
            $query = [
                'budgetId' => 0,
                'attribute' => 'product',
                'arrayParent' => $request['brandId'],
                'isDeleted' => ($request['isDeleted'] ?? "0"),
            ];
            Log::info('get API ' . $api . ' SKU');
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

    public function getListMechanism(Request $request)
    {
        $api = config('app.api') . '/promo/mechanism';
        try {
            $query = [
                'entityId'          => $request['entityId'],
                'subCategoryId'     => $request['subCategoryId'],
                'activityId'        => $request['activityId'],
                'subActivityId'     => $request['subActivityId'],
                'skuId'             => $request['skuId'],
                'channelId'         => $request['channelId'],
                'startDate'         => $request['startDate'],
                'endDate'           => $request['endDate'],
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

    public function getCutOff()
    {
        $app_cutoff_mechanism = config('app.app_cutoff_mechanism');
        $app_cutoff_hierarchy = config('app.app_cutoff_hierarchy');
        return array(
            'app_cutoff_mechanism' => $app_cutoff_mechanism,
            'app_cutoff_hierarchy' => $app_cutoff_hierarchy
        );
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

    public function getBaseline(Request $request)
    {
        $api = config('app.api') . '/promo/baseline';
        try {
            $body = [
                'promoId'           => ($request['promoId'] ?? 0),
                'period'            => $request['period'],
                'dateCreation'      => date('Y-m-d'),
                'typePromo'         => 1,
                'subCategoryId'     => ($request['subCategoryId'] ?? 0),
                'subActivityId'     => ($request['subActivityId'] ?? 0),
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

    public function getConfigRoiCR(Request $request)
    {
        $api = config('app.api') . '/promo/config-roi-cr';
        try {
            $query = [
                'subActivityId' => $request['subActivityId'],
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

    public function getPromoPlanningExist(Request $request)
    {
        $api = config('app.api') . '/promo/planning/exist';
        try {
            $arr_channels = array();
            array_push($arr_channels, $request['channelId']);

            $arr_subAccounts = array();
            array_push($arr_subAccounts, $request['subAccountId']);

            $query = [
                'period'            => $request['period'],
                'activityDesc'      => $request['activityDesc'],
                'arrayChannel'      => $arr_channels,
                'arrayAccount'      => $arr_subAccounts,
                "startPromo"        => $request['startPromo'],
                "endPromo"          => $request['endPromo'],
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

    public function promoPlanningFormPage()
    {
        $title = 'Form Promo Planning';
        return view('Promo::promo-planning.form', compact('title'));
    }

    public function getDataByID(Request $request)
    {
        $api = config('app.api') . '/promo/planning/id';
        try {
            $query = [
                'id' => $request['id'],
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

    public function save(Request $request)
    {
        $api = config('app.api') . '/promo/planning';
        $promoHeader = [
            "promoPlanId"           => 0,
            "periode"               => $request['period'],
            "distributorId"         => $request['distributorId'],
            "entityId"              => $request['entityId'],
            "categoryShortDesc"     => '',
            "principalShortDesc"    => '',
            "categoryId"            => 0,
            "subCategoryId"         => $request['subCategoryId'],
            "activityId"            => $request['activityId'],
            "subActivityId"         => $request['subActivityId'],
            "activityDesc"          => $request['activityDesc'],
            "startPromo"            => $request['startPromo'],
            "endPromo"              => $request['endPromo'],
            "mechanisme1"           => '',
            "mechanisme2"           => '',
            "mechanisme3"           => '',
            "mechanisme4"           => '',
            "investment"            => str_replace(',', '', $request['investment']),
            "normalSales"           => str_replace(',', '', $request['baselineSales']),
            "incrSales"             => str_replace(',', '', $request['incrementSales']),
            "roi"                   => str_replace(',', '', $request['roi']),
            "costRatio"             => str_replace(',', '', $request['costRatio']),
            "notes"                 => "",
            "initiator_notes"       => $request['initiatorNotes'],
            "modifReason"           => $request['modifReason'],
        ];

        $dataRegion = json_decode($request['dataRegion'], TRUE);
        $arr_regions = array();
        for ($i = 0; $i < count($dataRegion); $i++) {
            $regions['id'] = $dataRegion[$i];
            array_push($arr_regions, $regions);
        }

        $dataChannel = json_decode($request['channelId'], TRUE);
        $arr_channels = array();
        $channels['id'] = $dataChannel;
        array_push($arr_channels, $channels);


        $dataSubChannel = json_decode($request['subChannelId'], TRUE);
        $arr_subChannels = array();
        $subChannels['id'] = $dataSubChannel;
        array_push($arr_subChannels, $subChannels);

        $dataAccount = json_decode($request['accountId'], TRUE);
        $arr_accounts = array();
        $accounts['id'] = $dataAccount;
        array_push($arr_accounts, $accounts);

        $dataSubAccount = json_decode($request['subAccountId'], TRUE);
        $arr_subAccounts = array();
        $subAccounts['id'] = $dataSubAccount;
        array_push($arr_subAccounts, $subAccounts);

        $dataBrand = json_decode($request['dataBrand'], TRUE);
        $arr_brands = array();
        for ($i = 0; $i < count($dataBrand); $i++) {
            $brands['id'] = $dataBrand[$i];
            array_push($arr_brands, $brands);
        }

        $dataSKU = json_decode($request['dataSKU'], TRUE);
        $arr_skus = array();
        for ($i = 0; $i < count($dataSKU); $i++) {
            $skus['id'] = $dataSKU[$i];
            array_push($arr_skus, $skus);
        }

        // Get Latest Baseline Sales
        $paramBaseLine = [
            'promoId'           => 0,
            'period'            => $request['period'],
            'dateCreation'      => date('Y-m-d'),
            'typePromo'         => 1,
            'subCategoryId'     => ($request['subCategoryId'] ?? 0),
            'subActivityId'     => ($request['subActivityId'] ?? 0),
            'distributorId'     => $request['distributorId'],
            'startPromo'        => $request['startPromo'],
            'endPromo'          => $request['endPromo'],
            'arrayRegion'       => $dataRegion,
            'arrayChannel'      => [$dataChannel],
            'arraySubChannel'   => [$dataSubChannel],
            'arrayAccount'      => [$dataAccount],
            'arraySubAccount'   => [$dataSubAccount],
            'arrayBrand'        => $dataBrand,
            'arraySKU'          => $dataSKU,
        ];

        $baseLineLatest = $this->getLatestBaseline($paramBaseLine);
        $promoHeader['normalSales'] = $baseLineLatest;
        // Get Latest Baseline Sales

        $data = [
            'promoPlanningHeader'   => $promoHeader,
            'regions'               => $arr_regions,
            'channels'              => $arr_channels,
            'subChannels'           => $arr_subChannels,
            'accounts'              => $arr_accounts,
            'subAccounts'           => $arr_subAccounts,
            'brands'                => $arr_brands,
            'skus'                  => $arr_skus,
            'mechanisms'            => json_decode($request['dataMechanism'])
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error' => false,
                    'data' => json_decode($response)->values,
                    'message' => 'Promo Plan ID ' . json_decode($response)->values->refId . ' Has been saved successfully',
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
                'message' => "error : Save Failed"
            );
        }
    }

    public function update(Request $request)
    {
        $api = config('app.api') . '/promo/planning';
        $promoHeader = [
            "promoPlanId"           => $request['promoPlanningId'],
            "periode"               => $request['period'],
            "distributorId"         => $request['distributorId'],
            "entityId"              => $request['entityId'],
            "categoryShortDesc"     => '',
            "principalShortDesc"    => '',
            "categoryId"            => 0,
            "subCategoryId"         => $request['subCategoryId'],
            "activityId"            => $request['activityId'],
            "subActivityId"         => $request['subActivityId'],
            "activityDesc"          => $request['activityDesc'],
            "startPromo"            => $request['startPromo'],
            "endPromo"              => $request['endPromo'],
            "mechanisme1"           => '',
            "mechanisme2"           => '',
            "mechanisme3"           => '',
            "mechanisme4"           => '',
            "investment"            => str_replace(',', '', $request['investment']),
            "normalSales"           => str_replace(',', '', $request['baselineSales']),
            "incrSales"             => str_replace(',', '', $request['incrementSales']),
            "roi"                   => str_replace(',', '', $request['roi']),
            "costRatio"             => str_replace(',', '', $request['costRatio']),
            "notes"                 => "",
            "initiator_notes"       => $request['initiatorNotes'],
            "modifReason"           => $request['modifReason'],
        ];

        $dataRegion = json_decode($request['dataRegion'], TRUE);
        $arr_regions = array();
        for ($i = 0; $i < count($dataRegion); $i++) {
            $regions['id'] = $dataRegion[$i];
            array_push($arr_regions, $regions);
        }

        $dataChannel = json_decode($request['channelId'], TRUE);
        $arr_channels = array();
        $channels['id'] = $dataChannel;
        array_push($arr_channels, $channels);


        $dataSubChannel = json_decode($request['subChannelId'], TRUE);
        $arr_subChannels = array();
        $subChannels['id'] = $dataSubChannel;
        array_push($arr_subChannels, $subChannels);

        $dataAccount = json_decode($request['accountId'], TRUE);
        $arr_accounts = array();
        $accounts['id'] = $dataAccount;
        array_push($arr_accounts, $accounts);

        $dataSubAccount = json_decode($request['subAccountId'], TRUE);
        $arr_subAccounts = array();
        $subAccounts['id'] = $dataSubAccount;
        array_push($arr_subAccounts, $subAccounts);

        $dataBrand = json_decode($request['dataBrand'], TRUE);
        $arr_brands = array();
        for ($i = 0; $i < count($dataBrand); $i++) {
            $brands['id'] = $dataBrand[$i];
            array_push($arr_brands, $brands);
        }

        $dataSKU = json_decode($request['dataSKU'], TRUE);
        $arr_skus = array();
        for ($i = 0; $i < count($dataSKU); $i++) {
            $skus['id'] = $dataSKU[$i];
            array_push($arr_skus, $skus);
        }

        // Get Latest Baseline Sales
        $paramBaseLine = [
            'promoId'           => 0,
            'period'            => $request['period'],
            'dateCreation'      => date('Y-m-d'),
            'typePromo'         => 1,
            'subCategoryId'     => ($request['subCategoryId'] ?? 0),
            'subActivityId'     => ($request['subActivityId'] ?? 0),
            'distributorId'     => $request['distributorId'],
            'startPromo'        => $request['startPromo'],
            'endPromo'          => $request['endPromo'],
            'arrayRegion'       => $dataRegion,
            'arrayChannel'      => [$dataChannel],
            'arraySubChannel'   => [$dataSubChannel],
            'arrayAccount'      => [$dataAccount],
            'arraySubAccount'   => [$dataSubAccount],
            'arrayBrand'        => $dataBrand,
            'arraySKU'          => $dataSKU,
        ];

        $baseLineLatest = $this->getLatestBaseline($paramBaseLine);
        $promoHeader['normalSales'] = $baseLineLatest;
        // Get Latest Baseline Sales

        $data = [
            'promoPlanningHeader'   => $promoHeader,
            'regions'               => $arr_regions,
            'channels'              => $arr_channels,
            'subChannels'           => $arr_subChannels,
            'accounts'              => $arr_accounts,
            'subAccounts'           => $arr_subAccounts,
            'brands'                => $arr_brands,
            'skus'                  => $arr_skus,
            'mechanisms'            => json_decode($request['dataMechanism'])
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error' => false,
                    'data' => json_decode($response)->values,
                    'message' => 'Promo Plan ID ' . json_decode($response)->values->refId . ' Has been saved successfully',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('put API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (Exception $e) {
            Log::error('put API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => "error : Save Failed"
            );
        }
    }

    public function cancel(Request $request)
    {
        $api = config('app.api') . '/promo/planning/cancel';

        $data = [
            'promoPlanningId' => $request['promoPlanningId'],
            'reason' => $request['reason'],
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error' => false,
                    'data' => json_decode($response)->values,
                    'message' => 'Promo Plan ID ' . json_decode($response)->values->refId . ' cancelled',
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
                'message' => "error : Cancel Failed"
            );
        }
    }

    private function getLatestBaseline($param)
    {
        $api = config('app.api') . '/promo/baseline';
        try {
            $body = [
                'promoId'           => ($param['promoId'] ?? 0),
                'period'            => $param['period'],
                'dateCreation'      => date('Y-m-d'),
                'typePromo'         => 1,
                'subCategoryId'     => ($param['subCategoryId'] ?? 0),
                'subActivityId'     => ($param['subActivityId'] ?? 0),
                'distributorId'     => $param['distributorId'],
                'startPromo'        => $param['startPromo'],
                'endPromo'          => $param['endPromo'],
                'arrayRegion'       => $param['arrayRegion'],
                'arrayChannel'      => $param['arrayChannel'],
                'arraySubChannel'   => $param['arraySubChannel'],
                'arrayAccount'      => $param['arrayAccount'],
                'arraySubAccount'   => $param['arraySubAccount'],
                'arrayBrand'        => $param['arrayBrand'],
                'arraySKU'          => $param['arraySKU'],
            ];

            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($body));
            $response = Http::timeout(180)->withToken($this->token)->post($api, $body);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                return $resVal->baseline_sales;
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return 0;
            }
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return 0;
        }
    }

    public function uploadXls(Request $request)
    {
        $api = '/promo/planning/upload';
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Upload success",
                    'data'      => json_decode($response)->values
                ]);
            } else {
                return array(
                    'error' => true,
                    'message' => "Upload failed"
                );
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }

    public function exportXls(Request $request)
    {
        $api = config('app.api') . '/promo/planning/download';
        $query = [
            'year'          => $request['period'],
            'entity'        => $request['entityId'],
            'distributor'   => $request['distributorId'],
            'createFrom'    => $request['startFrom'],
            'createTo'      => $request['startTo'],
            'startFrom'     => $request['startFrom'],
            'startTo'       => $request['startTo'],
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result = [];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->refId;
                    // $arr[] = $fields->promoPlanId;
                    $arr[] = $fields->tsCode;
                    $arr[] = $fields->entityShortDesc;
                    $arr[] = $fields->distributorShortDesc;
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('d-m-Y', strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->endPromo));
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->cancelNotes;
                    $arr[] = $fields->initiator_notes;
                    $dt = date('d/m/Y  H:m:s', strtotime($fields->tsCodeOn));
                    $TSCodeOn = Carbon::createFromFormat('d/m/Y  H:m:s', $dt);
                    if ($fields->tsCodeOn == '0001-01-01T00:00:00') {
                        $arr[] = "";
                    } else {
                        $arr[] = $TSCodeOn;
                    }
                    $arr[] = $fields->tsCodeBy;

                    $result[] = $arr;
                }
                $title = 'A1:T1'; //Report Title Bold and merge
                $header = 'A6:T6'; //Header Column Bold and color
                $heading = [
                    ['Promo Planning'],
                    ['Period : ' . $request['period']],
                    ['Entity : ' . $request['entity']],
                    ['Distributor : ' . $request['distributor']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Planning ID', 'TSCode', 'Entity', 'Distributor', 'Region', 'Sub Account', 'Sub Brand', 'Mechanism', 'Promo Start', 'Promo End', 'Activity Description', 'Investment', 'Investment Code', 'Investment Type', 'Last Status', 'Promo ID', 'Cancel Notes', 'Initiator Notes', 'TSCode On', 'TSCode By']
                ];
                $formatCell = [
                    'I' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'J' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_TEXT,
                ];
                $filename = 'PromoPlanning-';
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc . '.xlsx');
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

}
