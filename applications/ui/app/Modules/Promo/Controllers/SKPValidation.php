<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Report\ExportSKPValidation;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;

class SKPValidation extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "SKP Validation";
        return view('Promo::skp-validation.index', compact('title'));
    }

    public function form(Request $request)
    {
        $category = MyEncrypt::decrypt($request['c']);
        if ($category === 'DC') {
            $title = "SKP Validation (Distributor Cost)";
            return view('Promo::skp-validation.dc-form', compact('title'));
        } else {
            $title = "SKP Validation (Retailer Cost)";
            return view('Promo::skp-validation.form', compact('title'));
        }
    }

    public function getFlagging()
    {
        $title = "SKP Validation";
        return view('Promo::skp-validation.flagging', compact('title'));
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

    public function getList(Request $request)
    {
        $api = config('app.api'). '/promo/skpvalidation';
        try {
            $query = [
                'Period'            => $request['period'],
                'EntityId'          => ($request['entityId'] ?? 0),
                'DistributorId'     => ($request['distributorId'] ?? 0),
                'BudgetParentId'    => 0,
                'ChannelId'         => ($request['channelId'] ?? 0),
                'CancelStatus'      => 'false',
                'StartFrom'         => $request['startFrom'],
                'StartTo'           => $request['startTo'],
                'Status'            => ($request['SKPstatus'] ?? -1),
                'SortColumn'        => 'skpstatus',
                'SortDirection'     => 'asc'
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryId($data->categoryShortDesc));
                }
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    "data"      => $resVal,
                    'message'   => $message
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
        $api = config('app.api'). '/promo/skpvalidation/id';
        $query = [
            'id'        => $request['id'],
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

    public function getListEntity()
    {
        $api = config('app.api') . '/promo/entity';
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

    public function getListChannel()
    {
        $api = config('app.api') . '/promo/creation/channel';
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/promo/skpvalidation';
        $promoSKPHeader = [
            "skpDraftAvail"             => $request['skpDraftAvail'] == 'on',
            "skpDraftAvailBfrAct60"     => $request['skpDraftAvailBfrAct60'] == 'on',
            "entityDraft"               => $request['entityDraftMatch'] == 'on',
            "brandDraft"                => $request['brandDraftMatch'] == 'on',
            "periodDraft"               => $request['periodDraftMatch'] == 'on',
            "activityDescDraft"         => $request['activityDescDraftMatch'] == 'on',
            "mechanismDraft"            => $request['mechanismDraftMatch'] == 'on',
            "investmentDraft"           => $request['investmentDraftMatch'] == 'on',
            "distributorDraft"          => $request['distributorDraftMatch'] == 'on',
            "channelDraft"              => $request['channelDraftMatch'] == 'on',
            "storeNameDraft"            => $request['storeNameDraftMatch'] == 'on',

            "entity"                    => $request['entityMatch'] == 'on',
            "brand"                     => $request['brandMatch'] == 'on',
            "periodMatch"               => $request['periodMatch'] == 'on',
            "activityDesc"              => $request['activityDescMatch'] == 'on',
            "investmentMatch"           => $request['investmentMatch'] == 'on',
            "mechanismMatch"            => $request['mechanismMatch'] == 'on',
            "skpSign7"                  => $request['skpSign7Match'] == 'on',
            "distributor"               => $request['distributorMatch'] == 'on',
            "channel"                   => $request['channelMatch'] == 'on',
            "storeName"                 => $request['storeNameMatch'] == 'on',

            "skpstatus"                 => $request['skpStatus'],
            "skp_notes"                 => $request['skpNotes']
        ];

        $data = [
            'promoSKPHeader'    => $promoSKPHeader,
            'promoId'           => $request['promoId'],
            'notes'             => "",
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error' => false,
                    'data' => [],
                    'message' => 'Data save success',
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/promo/skpvalidation/download';

        $query = [
            'Period'                    => $request['period'],
            'EntityId'                  => ($request['entityId'] ?? 0),
            'DistributorId'             => ($request['distributorId'] ?? 0),
            'BudgetParentId'            => 0,
            'ChannelId'                 => ($request['channelId'] ?? 0),
            'CancelStatus'              => 0,
            'StartFrom'                 => $request['startFrom'],
            'StartTo'                   => $request['startTo'],
            'SubmissionParam'           => 0,
            'Status'                    => ($request['SKPstatus'] ?? -1),
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

                    $arr[] = $fields->skpStatus;
                    $arr[] = $fields->refId;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = date('d-m-Y', strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = date('d-m-Y', strtotime($fields->lastStatusDate));

                    $arr[] = $fields->skpDraftAvail;
                    $arr[] = $fields->skpDraftAvailBfrAct60;
                    $arr[] = $fields->skpEntityDraft;
                    $arr[] = $fields->skpBrandDraft;
                    $arr[] = $fields->skpPeriodDraft;
                    $arr[] = $fields->skpActivityDescDraft;
                    $arr[] = $fields->skpMechanismDraft;
                    $arr[] = $fields->skpInvestmentDraft;
                    $arr[] = $fields->skpDistributorDraft;
                    $arr[] = $fields->skpChannelDraft;
                    $arr[] = $fields->skpStoreNameDraft;

                    $arr[] = $fields->skpSign7;
                    $arr[] = $fields->skpEntity;
                    $arr[] = $fields->skpBrand;
                    $arr[] = $fields->skpPeriodMatch;
                    $arr[] = $fields->skpActivityDesc;
                    $arr[] = $fields->skpMechanismMatch;
                    $arr[] = $fields->skpInvestmentMatch;
                    $arr[] = $fields->skpDistributor;
                    $arr[] = $fields->skpChannel;
                    $arr[] = $fields->skpStoreName;
                    if ($fields->storeNameon == '0001-01-01T00:00:00') {
                        $arr[] = '';
                    } else {
                        $arr[] = date('d-m-Y H:m:s', strtotime($fields->storeNameon));
                    }

                    $arr[] = $fields->storeNameby;
                    $arr[] = $fields->skP_Notes;

                    $result[] = $arr;
                }
                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A7:AL7'; //Header Column Bold and color
                $merge1 = 'A5:N6';
                $merge2 = 'O5:AI5';
                $merge3 = 'O6:Y6';
                $merge4 = 'Z6:AI6';
                $merge5 = 'O7:AI7';
                $heading = [
                    ['SKP Validation Report'],
                    ['Period : ' . $request->period],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [''],
                    ['PROMO','','','','','','','','','','','','','','SKP'],
                    ['','','','','','','','','','','','','','','DRAFT','','','','','','','','','','','FINAL'],
                    [
                        'SKP Status',
                        'Promo ID',
                        'Investment',
                        'Distributor',
                        'Initiator',
                        'Channel',
                        'Sub Account',
                        'Activity Name',
                        'Mechanism',
                        'Promo Start',
                        'Promo End',
                        'Creation Date',
                        'Status',
                        'Status Date',
                        'SKP Draft Availability',
                        'SKP Draft Availability Before Activity Start H-30',
                        'Entity',
                        'SKU',
                        'Period',
                        'Activity Desc',
                        'Mechanism',
                        'Investment',
                        'Distributor',
                        'Channel',
                        'StoreName',
                        'SKP Signed H-7',
                        'Entity',
                        'SKU',
                        'Period',
                        'Activity Desc',
                        'Mechanism',
                        'Investment',
                        'Distributor',
                        'Channel',
                        'Store Name',
                        'Validate on',
                        'Validate by',
                        'Remarks'
                    ]
                ];
                $formatCell =  [
                    'C' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'SKP Validation Report-';
                $export = new ExportSKPValidation($result, $heading, $title, $header, $merge1, $merge2, $merge3, $merge4, $merge5, $formatCell);
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
}
