<?php

namespace App\Modules\Promo\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;
use Wording;


class PromoClosure extends Controller
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
        $title = "Promo Closure";
        return view('Promo::promo-closure.index', compact('title'));
    }

    public function Form(Request $request)
    {
        $method = $request['m'];
        $category = MyEncrypt::decrypt($request['c']);
        $yearPromo = $request['sy'];

        if($category === 'DC') {
            $title = "Promo Closure (Distributor Cost)";
            if ($yearPromo > '2024' ) {
                if (!$request['recon']) {
                    return view('Promo::promo-closure.dc-form-revamp', compact('method','title'));
                } else {
                    $title = "Promo Reconciliation Form (Distributor Cost)";
                    return view('Promo::promo-closure.dc-form-recon', compact('method','title'));
                }
            } else {
                return view('Promo::promo-closure.dc-form', compact('method', 'title'));
            }
        } else {
            $title = "Promo Closure (Retailer Cost)";
            if ($yearPromo > '2024' ) {
                if (!$request['recon']) {
                    return view('Promo::promo-closure.form-revamp', compact('method','title'));
                } else {
                    $title = "Promo Reconciliation Form (Retailer Cost)";
                    return view('Promo::promo-closure.form-recon', compact('method','title'));
                }
            } else {
                return view('Promo::promo-closure.form', compact('method','title'));
            }
        }
    }

    public function uploadPage()
    {
        $title = "Promo Closure";
        return view('Promo::promo-closure.upload-xls', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api') . '/promo/closure';
        try {
            $query = [
                'entity'            => ($request['entityId'] ?? 0),
                'distributor'       => ($request['distributorId'] ?? 0),
                'channel'           => ($request['channelId'] ?? 0),
                'start_from'        => $request['startFrom'],
                'start_to'          => $request['startTo'],
                'remaining_budget'  => ($request['remainingBudget'] ?? 'all'),
                'start'             => 0,
                'length'            => -1,
                'txtsearch'         => ($request['search'] ?? ''),
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
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

    public function getDataById(Request $request)
    {
        $api = config('app.api'). '/promo/closure/id';
        $query = [
            'id'        => $request->promoId,
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

    public function getDataV2ById(Request $request): bool|string
    {
        $api = '/promo/displayv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
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

    public function open(Request $request)
    {
        $api = config('app.api') . '/promo/closure/reopen';

        $arr_promo = array();
        array_push($arr_promo, $request->promoId);

        $data = [
            'promoId'       => $arr_promo,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Open
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $message = json_decode($response)->message;
                return array(
                    'error'     => false,
                    'message'   => $message
                );
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
                'message'   => "Open Promo Failed"
            );
        }
    }

    public function close(Request $request)
    {
        $api = config('app.api') . '/promo/closure/close';

        $arr_promo = array();
        array_push($arr_promo, $request->promoId);

        $data = [
            'promoId'       => $arr_promo,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Open
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                if($resVal[0]->status==='successfully closed') {
                    return array(
                        'error' => false,
                        'doc' => $resVal[0]->doc,
                        'message' => $resVal[0]->status,
                    );
                } else {
                    return array(
                        'error' => true,
                        'doc' => $resVal[0]->doc,
                        'message' => $resVal[0]->status,
                    );
                }
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
                'message'   => "Close Promo Failed"
            );
        }
    }

    public function uploadXls(Request $request) {
        $api = config('app.api'). '/promo/closure/import';
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

    public function exportXls(Request $request)
    {
        $api = config('app.api') . '/promo/closure';
        $query = [
            'entity'            => $request['entityId'],
            'distributor'       => $request['distributorId'],
            'channel'           => $request['channelId'],
            'start_from'        => $request['startFrom'],
            'start_to'          => $request['startTo'],
            'remaining_budget'  => ($request['remainingBudget'] ?? 'all'),
            'start'             => 0,
            'length'            => -1,
            'filter'            => null,
            'txtsearch'         => null,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result = [];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->initiator;
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));
                    $arr[] = date('d-m-Y', strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y', strtotime($fields->endPromo));
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = $fields->mechanisme2;
                    $arr[] = $fields->mechanisme3;
                    $arr[] = $fields->mechanisme4;
                    $arr[] = $fields->promoStatus;
                    $arr[] = $fields->reconStatus;
                    if($fields->closureStatus){
                        $arr[] = $fields->dnPaid;
                    }else{
                        $arr[] = $fields->investment;
                    }
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->aging;
                    if($fields->lastDNCreationDate=='0001-01-01T00:00:00'){
                        $arr[] = '';
                    }else{
                        $arr[] = date('Y-m-d' , strtotime($fields->lastDNCreationDate));
                    }

                    $arr[] = $fields->remainingInvestment_DN;
                    if($fields->closureStatus){
                        $arr[] = 'Closed';
                    }else{
                        $arr[] = 'Open';
                    }
                    $arr[] = $fields->closeBy;
                    if($fields->closeOn=='0001-01-01T00:00:00'){
                        $arr[] = '';
                    }else{
                        $arr[] = date('Y-m-d' , strtotime($fields->closeOn));
                    }

                    $result[] = $arr;
                }
                $title = 'A1:AC1'; //Report Title Bold and merge
                $header = 'A6:AC6'; //Header Column Bold and color
                $heading = [
                    ['Listing Promo Closure'],
                    ['Entity : ' . $request->entity],
                    ['Distributor : ' . $request->distributor],
                    ['Channel : ' . $request->channel],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    [
                        'Promo ID', 'Entity', 'Distributor', 'Initiator', 'Creation Date', 'Start Promo', 'End Promo', 'Channel', 'Sub Channel', 'Account', 'Sub Account',
                        'Sub Brand', 'Sub Category', 'Activity Description', 'Mechanism 1', 'Mechanism 2', 'Mechanism 3', 'Mechanism 4', 'Promo ID Status', 'Promo Reconciliation Status',
                        'Investment', 'DN Paid', 'DN Claim', 'Aging', 'Last DN Creation', 'Balance', 'Closure Status', 'Close By', 'CLose On'
                    ]
                ];
                $formatCell = [
                    'E' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'F' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'G' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'U' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'V' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'W' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'X' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Y' => NumberFormat::FORMAT_DATE_DDMMYYYY,
                    'Z' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];
                $filename = 'ListingPromoClosure-';
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
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function downloadTemplate(Request $request)
    {
        $api = config('app.api') . '/promo/closure';
        $query = [
            'entity'            => $request->entityId,
            'distributor'       => $request->distributorId,
            'channel'           => $request->channelId,
            'start_from'        => $request->startFrom,
            'start_to'          => $request->startTo,
            'remaining_budget'  => $request->remainingBudget,
            'start'             => 0,
            'length'            => -1,
            'filter'            => null,
            'txtsearch'         => null,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result = [];

                foreach ($resVal as $fields) {
                    $arr = [];
                    if (!$fields->closureStatus) {
                        $arr[] = $fields->promoNumber;
                        $arr[] = $fields->channelDesc;
                    }
                    $result[] = $arr;
                }
                $title = 'A1'; //Report Title Bold and merge
                $header = 'A1:B1'; //Header Column Bold and color
                $heading = [
                    ['Promo ID', 'Channel']
                ];
                $formatCell =  [];
                $filename = 'TemplatePromoClosure-';
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
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function previewAttachment(Request $request)
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-approval.attachment-preview", compact('path', 'isExist', 'title'));
    }

    public function encCategoryShortDesc ($categoryShortDesc)
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }
}
