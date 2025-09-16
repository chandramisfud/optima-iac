<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Budget\ExportBudgetTTConsole;
use App\Exports\Budget\TTConsole\ExportTemplateBudgetTTConsole;
use App\Exports\Export;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Maatwebsite\Excel\Facades\Excel;
use App\Helpers\CallApi;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class BudgetTTConsole extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage(): Factory|View|Application
    {
        $title = "Budget [TT Consol]";
        return view('Budget::budget-tt-console.index', compact('title'));
    }

    public function uploadPageRC(): Factory|View|Application
    {
        $title = "Upload Budget [TT Consol] - RC";
        return view('Budget::budget-tt-console.upload-xls-rc', compact('title'));
    }

    public function uploadPageDC(): Factory|View|Application
    {
        $title = "Upload Budget [TT Consol] - DC";
        return view('Budget::budget-tt-console.upload-xls-dc', compact('title'));
    }

    public function formPage (Request $request): Factory|View|Application
    {
        $category = decrypt($request['c']);
        if ($category === 'Retailer Cost') {
            $title = "Budget TT Consol Form (Retailer Cost)";
            return view('Budget::budget-tt-console.form', compact('title'));
        } else {
            $title = "Budget TT Consol Form (Distributor Cost)";
            return view('Budget::budget-tt-console.dc-form', compact('title'));
        }
    }

    public function getListPaginateFilter(Request $request): bool|array|string
    {
        $api = config('app.api'). '/budget/ttconsole/list';
        try {
            $page = 0;
            if ($request['length'] > -1) {
                $page = ($request['start'] / $request['length']);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $SortColumn = $request['columns'][(int) $column]['data'];

            $query = [
                'period'            => (int) $request['period'],
                'category'          => json_decode($request['category']),
                'subCategory'       => json_decode($request['subCategory']),
                'subActivityType'   => json_decode($request['subActivityType']),
                'activity'          => json_decode($request['activity']),
                'subActivity'       => json_decode($request['subActivity']),
                'channel'           => json_decode($request['channel']),
                'subChannel'        => json_decode($request['subChannel']),
                'account'           => json_decode($request['account']),
                'subAccount'        => json_decode($request['subAccount']),
                'distributor'        => json_decode($request['distributor']),
                'groupBrand'        => json_decode($request['groupBrand']),
                'txtSearch'         => $search,
                'PageNumber'        => $page,
                'PageSize'          => (int) $length,
                'sort'              => $sort,
                'order'             => $SortColumn
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                foreach ($resVal as $data) {
                    $data->categoryEnc = encrypt($data->category);
                }
                return json_encode([
                    "draw" => (int) $request['draw'],
                    "data" => $resVal,
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

    public function getListCategory(): bool|string
    {
        $api = '/budget/ttconsole/categorylist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubCategory(Request $request): bool|string
    {
        $api = '/budget/ttconsole/subcategorylist';
        $query = [
            'categoryId'         => json_decode($request['categoryId']),
            'subActivityTypeId'  => json_decode($request['subActivityTypeId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListChannel(): bool|string
    {
        $api = '/budget/ttconsole/channellist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubChannel(Request $request): bool|string
    {
        $api = '/budget/ttconsole/subchannellist';
        $query = [
            'channelId'  => json_decode($request['channelId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListAccount(Request $request): bool|string
    {
        $api = '/budget/ttconsole/accountlist';
        $query = [
            'subChannelId'  => json_decode($request['subChannelId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListSubAccount(Request $request): bool|string
    {
        $api = '/budget/ttconsole/subaccountlist';
        $query = [
            'accountId'  => json_decode($request['accountId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListDistributor(): bool|string
    {
        $api = '/budget/ttconsole/distributorlist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubActivityType(Request $request): bool|string
    {
        $api = '/budget/ttconsole/subactivitytypelist';
        $query = [
            'categoryId'  => json_decode($request['categoryId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListActivity(Request $request): bool|string
    {
        $api = '/budget/ttconsole/activitylist';
        $query = [
            'categoryId'        => json_decode($request['categoryId']),
            'subCategoryId'     => json_decode($request['subCategoryId']),
            'subActivityTypeId' => json_decode($request['subActivityTypeId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListSubActivity(Request $request): bool|string
    {
        $api = '/budget/ttconsole/subactivitylist';
        $query = [
            'categoryId'        => json_decode($request['categoryId']),
            'activityId'        => json_decode($request['activityId']),
            'subActivityTypeId' => json_decode($request['subActivityTypeId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListGroupBrand(): bool|string
    {
        $api = '/budget/ssconversionrate/groupbrandlist';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getDataByID(Request $request): bool|string
    {
        $api = '/budget/ttconsole/id';
        $query = [
            'id'  => json_decode($request['id']),
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function uploadXls(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ssconversionrate/upload';
        Log::info('post API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);

            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal,
                    'message' => json_decode($response)->message,
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
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

    public function downloadTemplateRC(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/ttconsole';
        $query = [
            'period'            => (int) $request['period'],
            'category'          => json_decode($request['category']),
            'subCategory'       => json_decode($request['subCategory']),
            'subActivityType'   => json_decode($request['subActivityType']),
            'activity'          => json_decode($request['activity']),
            'subActivity'       => json_decode($request['subActivity']),
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'distributor'       => json_decode($request['distributor']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
            'sort'              => 'ASC',
            'order'             => 'Id'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $resultTTConsole=[];
                $row = 1;
                $separator = '&';
                foreach ($resVal as $fields) {
                    $arr = [];

                    $row++;
                    $arr[] = $fields->period;
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->distributorShortDesc;
                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->tt / 100;
                    $arr[] = '=CONCAT(A' . $row . $separator . '" "' . $separator . 'J' . $row . $separator . '" - "' . $separator . 'I' . $row . $separator . '" - "' . $separator . 'G' .$row . $separator . '"  "' . $separator . 'M' . $row . ')';

                    $resultTTConsole[] = $arr;
                }

                $filename = 'Template Budget TT Consol Retailer Cost - ';
                $titleTTConsole = 'A1:O1'; //Report Title Bold and merge
                $headerTTConsole = 'A1:O1'; //Header Column Bold and color
                $headingTTConsole = [
                    ['Budget Year', 'Category', 'Sub Category', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Distributor', 'Distributor Short Desc', 'Brand', 'Sub Activity Type', 'Sub Activity', 'Activity', 'TT % (in %)', 'Budget Name (Auto)']
                ];
                $cellNoInput = 'O2:O'.$row;

                $formatCellTTConsole =  [
                    'N' => NumberFormat::FORMAT_PERCENTAGE_00,
                ];

                //Activity
                $resValActivity = json_decode($response)->values->activity;
                $resultActivity = [];
                foreach ($resValActivity as $fields) {
                    $arr = [];
                    $arr[] = $fields->CategoryDesc;
                    $arr[] = $fields->SubCategoryDesc;
                    $arr[] = $fields->SubActivityTypeDesc;
                    $arr[] = $fields->SubActivityDesc;
                    $arr[] = $fields->ActivityDesc;

                    $resultActivity[] = $arr;
                }

                $headerActivity = 'A1:E1'; //Header Column Bold and color
                $headingActivity = [
                    ['Category', 'Sub Category', 'Sub Activity Type', 'Sub Activity', 'Activity']
                ];

                //Account
                $resValAccount = json_decode($response)->values->account;
                $resultAccount = [];
                foreach ($resValAccount as $fields) {
                    $arr = [];
                    $arr[] = $fields->ChannelDesc;
                    $arr[] = $fields->SubChannelDesc;
                    $arr[] = $fields->AccountDesc;
                    $arr[] = $fields->SubAccountDesc;

                    $resultAccount[] = $arr;
                }

                $headerAccount = 'A1:D1'; //Header Column Bold and color
                $headingAccount = [
                    ['Channel', 'Sub Channel', 'Account', 'Sub Account']
                ];

                //Distributor
                $resValDistributor = json_decode($response)->values->distributor;
                $resultDistributor = [];
                foreach ($resValDistributor as $fields) {
                    $arr = [];
                    $arr[] = $fields->Distributor;
                    $arr[] = $fields->DistributorShortDesc;

                    $resultDistributor[] = $arr;
                }

                $headerDistributor = 'A1:B1'; //Header Column Bold and color
                $headingDistributor = [
                    ['Distributor', 'Distributor Short Desc']
                ];

                //Brand
                $resValBrand = json_decode($response)->values->brand;
                $resultBrand = [];
                foreach ($resValBrand as $fields) {
                    $arr = [];
                    $arr[] = $fields->Brand;

                    $resultBrand[] = $arr;
                }

                $headerBrand = 'A1:A1'; //Header Column Bold and color
                $headingBrand = [
                    ['Brand']
                ];


                $export = new ExportTemplateBudgetTTConsole($resultTTConsole, $headingTTConsole, $titleTTConsole, $headerTTConsole, $formatCellTTConsole,
                    $resultActivity, $headingActivity, $headerActivity,
                    $resultAccount, $headingAccount, $headerAccount,
                    $resultDistributor, $headingDistributor, $headerDistributor,
                    $resultBrand, $headingBrand, $headerBrand, $cellNoInput
                );


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

    public function downloadTemplateDC(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/ttconsole';
        $query = [
            'period'            => (int) $request['period'],
            'category'          => json_decode($request['category']),
            'subCategory'       => json_decode($request['subCategory']),
            'subActivityType'   => json_decode($request['subActivityType']),
            'activity'          => json_decode($request['activity']),
            'subActivity'       => json_decode($request['subActivity']),
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'distributor'       => json_decode($request['distributor']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
            'sort'              => 'ASC',
            'order'             => 'period'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];
                $row = 1;
                $separator = '&';
                foreach ($resVal as $fields) {
                    $arr = [];
                    $row++;

                    $arr[] = $fields->period;
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->distributorShortDesc;
                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->tt / 100;
                    $arr[] = '=CONCAT(A' . $row . $separator . '" "' . $separator . 'G' . $row . $separator . '" - "' . $separator . 'F' . $row . $separator . '" - "' . $separator . 'C' . $row . $separator . '" "' . $separator . 'H' .$row . ')';

                    $result[] = $arr;
                }

                $filename = 'Template Budget TT Consol Distributor Cost - ';
                $header = 'A1:L1'; //Header Column Bold and color
                $heading = [
                    ['Budget Year', 'Category', 'Sub Category', 'Channel', 'Distributor', 'Distributor Short Desc', 'Brand', 'Sub Activity Type', 'Sub Activity', 'Activity', 'TT % (in %)', 'Budget Name (Auto)']
                ];
                $cellNoInput = 'L2:L'.$row;

                $formatCell =  [
                    'K' => NumberFormat::FORMAT_PERCENTAGE_00,
                ];

                $export = new ExportBudgetTTConsole($result, $heading, $header, $formatCell, $cellNoInput);

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

    public function exportXls(Request $request): BinaryFileResponse|array
    {
        if($request['typeDataText'] == 'Current'){
            $api = config('app.api'). '/budget/ttconsole/list';
            $filename = 'Budget TT Consol ' . $request['categoryText'] . ' - ';
        } else {
            $api = config('app.api'). '/budget/ttconsole/history';
            $filename = 'Budget TT Consol Historical ' . $request['categoryText'] . ' - ';
        }

        $query = [
            'period'            => (int) $request['period'],
            'category'          => json_decode($request['category']),
            'subCategory'       => json_decode($request['subCategory']),
            'subActivityType'   => json_decode($request['subActivityType']),
            'activity'          => json_decode($request['activity']),
            'subActivity'       => json_decode($request['subActivity']),
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'distributor'       => json_decode($request['distributor']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
            'sort'              => 'ASC',
            'order'             => 'period'
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

                    $arr[] = $fields->period;
                    $arr[] = $fields->category;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->distributorShortDesc;
                    $arr[] = $fields->groupbrand;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->tt / 100;
                    $arr[] = $fields->budgetName;

                    if($request['typeDataText'] != 'Current') {
                        $arr[] = $fields->action;
                        $arr[] = date('d-m-Y', strtotime($fields->actionOn));
                        $arr[] = $fields->actionBy;
                        $arr[] = $fields->actionByEmail;
                    }

                    $result[] = $arr;
                }

                if($request['typeDataText'] != 'Current') {
                    $title = 'A1:S1'; //Report Title Bold and merge
                    $header = 'A5:S5'; //Header Column Bold and color
                } else {
                    $title = 'A1:O1'; //Report Title Bold and merge
                    $header = 'A5:O5'; //Header Column Bold and color
                }


                if($request['typeDataText'] == 'Current'){
                    $heading = [
                        ['TT Consol'],
                        ['Budget Year : ' . $request['period'] ],
                        ['Account : ' . (($request['accountText'] == '') ? 'ALL' : $request['accountText'])],
                        ['Date Retrieved : ' . date('d-m-Y')],
                        ['Budget Year', 'Category', 'Sub Category', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Distributor', 'Distributor Short Desc', 'Brand', 'Sub Activity Type', 'Sub Activity', 'Activity', 'TT % (in %)', 'Budget Name (Auto)']
                    ];
                } else {
                    $heading = [
                        ['TT Consol'],
                        ['Budget Year : ' . $request['period'] ],
                        ['Account : ' . (($request['accountText'] == '') ? 'ALL' : $request['accountText'])],
                        ['Date Retrieved : ' . date('d-m-Y')],
                        ['Budget Year', 'Category', 'Sub Category', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Distributor', 'Distributor Short Desc', 'Brand', 'Sub Activity Type', 'Sub Activity', 'Activity', 'TT % (in %)', 'Budget Name (Auto)', 'Action', 'Action On', 'Action By', 'Action By Email']
                    ];
                }



                $formatCell =  [
                    'N' => NumberFormat::FORMAT_PERCENTAGE_00,
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function uploadXlsRC(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ttconsole/uploadrc';
        Log::info('post API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);

            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal,
                    'message' => json_decode($response)->message,
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
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

    public function uploadXlsDC(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ttconsole/uploaddc';
        Log::info('post API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);

            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal,
                    'message' => json_decode($response)->message,
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
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

    public function save(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ttconsole';
        $data = [
            'period'                => $request['period'],
            'category'              => (int) $request['category'],
            'subCategory'           => (int) ($request['sub_category'] != null ? $request['sub_category'] : 0),
            'channel'               => (int) $request['channel'],
            'subChannel'            => (int) ($request['sub_channel'] != null ? $request['sub_channel'] : 0),
            'account'               => (int) ($request['account'] != null ? $request['account'] : 0),
            'subAccount'            => (int) ($request['sub_account'] != null ? $request['sub_account'] : 0),
            'distributor'           => (int) $request['distributor'],
            'distributorShortDesc'  => $request['distributorShortDesc'],
            'groupBrand'            => (int) $request['groupbrand'],
            'subActivityType'       => (int) $request['sub_activity_type'],
            'activity'              => (int) $request['activity'],
            'subActivity'           => (int) $request['sub_activity'],
            'tt'                    => str_replace(',', '', $request['ttPercent']),
            'budgetName'            => $request['budget_name'],
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => json_decode($response)->message,
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
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Save Failed"
            );
        }
    }

    public function update(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ttconsole';
        $data = [
            'id'                    => $request['id'],
            'period'                => $request['period'],
            'category'              => (int) $request['category'],
            'subCategory'           => (int) ($request['sub_category'] != null ? $request['sub_category'] : 0),
            'channel'               => (int) $request['channel'],
            'subChannel'            => (int) ($request['sub_channel'] != null ? $request['sub_channel'] : 0),
            'account'               => (int) ($request['account'] != null ? $request['account'] : 0),
            'subAccount'            => (int) ($request['sub_account'] != null ? $request['sub_account'] : 0),
            'distributor'           => (int) $request['distributor'],
            'distributorShortDesc'  => $request['distributorShortDesc'],
            'groupBrand'            => (int) $request['groupbrand'],
            'subActivityType'       => (int) $request['sub_activity_type'],
            'activity'              => (int) $request['activity'],
            'subActivity'           => (int) $request['sub_activity'],
            'tt'                    => str_replace(',', '', $request['ttPercent']),
            'budgetName'            => $request['budget_name'],
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => json_decode($response)->message,
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
        } catch (Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Save Failed"
            );
        }
    }

    protected function sendEmailApprover(Request $request): bool|string
    {
        // Get Data Request
        $userApprover = $request['userApprover'];
        $nameApprover = $request['nameApprover'];
        $id = $request['promoId'];
        $email = $request['emailApprover'];
        $subject = $request['subject'];

        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/displayv2email/id';

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
                if ($data->promo->mechanismInputMethod) {
                    $viewEmail = 'Budget::budget-tt-console.email-approval-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-tt-console.email-approval-revamp-mechanism-text';
                }

                $attach = array();
                for ($i=0; $i<count($data->attachment); $i++) {
                    if ($data->attachment[$i]->docLink =='row1') $attach['row1'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row2') $attach['row2'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row3') $attach['row3'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row4') $attach['row4'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row5') $attach['row5'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row6') $attach['row6'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row7') $attach['row7'] = $data->attachment[$i]->fileName;
                }
                if (!empty($data->attachment)) array_push($ar_fileattach, $attach);

                $urlid = $data->promo->id;
                $urlrefid = urlencode(MyEncrypt::encrypt($data->promo->refId));
                $yearPromo = date( 'Y', strtotime($data->promo->startPromo));

                $dataUrl = json_encode([
                    'promoId'       => $promoId,
                    'refId'         => $data->promo->refId,
                    'profileId'     => $userApprover,
                    'nameApprover'  => $nameApprover,
                    'sy'            => $yearPromo,
                ]);

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

    protected function sendEmailApproverRecon(Request $request): bool|string
    {
        // Get Data Request
        $userApprover = $request['userApprover'];
        $nameApprover = $request['nameApprover'];
        $id = $request['promoId'];
        $email = $request['emailApprover'];
        $subject = $request['subject'];

        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/displayv2email/id';

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

                if ($data->promo->mechanismInputMethod) {
                    $viewEmail = 'Budget::budget-tt-console.email-approval-recon-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-tt-console.email-approval-recon-revamp-mechanism-text';
                }

                $attach = array();
                for ($i=0; $i<count($data->attachment); $i++) {
                    if ($data->attachment[$i]->docLink =='row1') $attach['row1'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row2') $attach['row2'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row3') $attach['row3'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row4') $attach['row4'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row5') $attach['row5'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row6') $attach['row6'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row7') $attach['row7'] = $data->attachment[$i]->fileName;
                }
                if (!empty($data->attachment)) array_push($ar_fileattach, $attach);

                $urlid = $data->promo->id;
                $urlrefid = urlencode(MyEncrypt::encrypt($data->promo->refId));
                $yearPromo = date( 'Y', strtotime($data->promo->startPromo));

                $dataUrl = json_encode([
                    'promoId'       => $promoId,
                    'refId'         => $data->promo->refId,
                    'profileId'     => $userApprover,
                    'nameApprover'  => $nameApprover,
                    'sy'            => $yearPromo
                ]);

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
