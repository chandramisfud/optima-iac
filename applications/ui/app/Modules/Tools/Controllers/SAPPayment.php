<?php

namespace App\Modules\Tools\Controllers;

use Illuminate\Support\Facades\Storage;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Response;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use sys;

class SAPPayment extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage()
    {
        return view('Tools::sap-payment.index');
    }

    public function getListUploadHistory()
    {
        $api = config('app.api'). '/tools/xmlgenerate/xmlupload';
        $query = [
            'uploadtype'        => 'payment'
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

    public function uploadXml(Request $request) {
        try {
            if ($request->file('file')) {
                $filenametostore = $request->file('file')->getClientOriginalName();

                $api = config('app.api') . '/tools/xmlgenerate/xmlupload';

                $data = [
                    "userid"        => $request->session()->get('profile'),
                    "useremail"     => $request->session()->get('email'),
                    "filename"      => $filenametostore,
                    "uploadtype"    => 'payment',
                ];

                Log::info($api);
                Log::info(json_encode($data));

                $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
                if ($response->status() === 200) {
                    Log::info($response);
                } else {
                    Log::info($response);
                    return array(
                        'error'     => true,
                        'message'   => 'Upload Failed',
                    );
                }
                $upload = Storage::disk('sftp')->put($filenametostore, file_get_contents($request->file));
                if ($upload) {
                    return array(
                        'error'     => false,
                        'message'   => 'Upload Success',
                        'file'      => $filenametostore
                    );
                } else {
                    return array(
                        'error'     => true,
                        'message'   => 'Upload Failed',
                    );
                }
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }

    public function getListEntity()
    {
        $api = config('app.api') . '/tools/xmlgenerate/entity';
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

    public function getDataDistributor(Request $request)
    {
        $api = config('app.api') . '/tools/xmlgenerate/distributor';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'PrincipalId' => $request->PrincipalId
            ]);
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

    public function cekFlag(Request $request)
    {
        $api = config('app.api') . '/tools/xmlgenerate';
        $query = [
            'entity'    => $request->entity,
            'id'        => $request->distributor,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'data'=> $resVal,
                    'error' => false
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return json_encode([
                    'error' => true,
                ]);

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

    public function generateBatchName(Request $request)
    {
        $api = config('app.api') . '/tools/xmlgenerate/batchname';

        $query = [
            "entitylist"    => $request->entity,
            "userid"        => $request->session()->get('profile'),
            "id"            => json_decode($request->distributor)
        ];

        $entity_code = "";
        switch ($request->entity) {
            case 2:
                $entity_code = "4450";
                break;
            case 3:
                $entity_code = "5600";
                break;
            case 4:
                $entity_code = "5610";
                break;
        }

        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $data = json_decode($response)->values;

                $entity = $request->entityname;
                $xNo = 0;
                $result= [];
                foreach ($data as $fields) {
                    $xNo += 1;
                    $arr = [];
                    $arr[] = $xNo;
                    $arr[] = $fields->dnNumber;
                    $arr[] = $fields->dnDesc;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->dnAmount;
                    $arr[] = $fields->feeAmount;
                    $arr[] = $fields->ppnAmt;
                    $arr[] = $fields->pphAmt;
                    $arr[] = $fields->totalToPay;
                    $arr[] = $fields->batchName;
                    $arr[] = $fields->taxlevel;

                    $result[] = $arr;
                }

                $title = 'A1:K1'; //Report Title Bold and merge
                $header = 'A4:K4'; //Header Column Bold and color
                $heading = [
                    ['PAYMENT BATCH LIST'],
                    ['Entity: ' . $entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['NO', 'DN NO', 'DN DESCRIPTION', 'PROMO ID', 'DN AMOUNT', 'FEE', 'VAT', 'WHT', 'TOTAL', 'BATCH NAME', 'TAXLEVEL']
                ];
                $formatCell =  [
                    'A' => NumberFormat::FORMAT_NUMBER,
                    'B' => NumberFormat::FORMAT_TEXT,
                    'C' => NumberFormat::FORMAT_TEXT,
                    'D' => NumberFormat::FORMAT_TEXT,
                    'E' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_TEXT,
                    'K' => NumberFormat::FORMAT_TEXT,
                ];
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                $filename = 'ID9_DUMBCP_'. $entity_code . '_PAYMENT_BATCH ' . date('Y-m-d His') . $mc . '.xlsx';

                return Excel::download($export, $filename);
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error' => $response->status(),
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

    public function loopKosong(){
        $arr = [];
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        $arr[] = '';
        return $arr;
    }

    public function getDataDistributorPaymentAloneNonBatching(Request $request)
    {
        $api = config('app.api'). '/tools/xmlgenerate/batch-payment';
        $query = [
            "entity"    => $request['entity'],
            "batching"  => 0,
            "id"        => json_decode($request['distributor'])
        ];
        Log::info('get API PaymentAloneNonBatching ' . $api);
        Log::info('payload PaymentAloneNonBatching ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                if (!Storage::disk('optima')->exists('/assets/media/tools')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools');
                }
                if (!Storage::disk('optima')->exists('/assets/media/tools/sap-payment')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools/sap-payment');
                }
                $path_row = '/assets/media/tools/sap-payment/' . $request['uuid'];
                Storage::disk('optima')->put($path_row . '/dataPaymentAloneNonBatching.json', json_encode(json_decode($response)->values));
                $result = array(
                    'error'     => false,
                    'message'   => "Get Data PaymentAloneNonBatching Success"
                );
                Log::warning(json_encode($result));
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error'     => true,
                    'message'   => "Get Data PaymentAloneNonBatching Failed"
                );
                Log::info('get API ' . $api);
                Log::warning($message);
            }
            return $result;
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Get Data PaymentAloneNonBatching Failed"
            );
        }
    }

    public function getDataDistributorPaymentAloneBatching(Request $request)
    {
        $api = config('app.api'). '/tools/xmlgenerate/batch-payment';
        $query = [
            "entity"    => $request['entity'],
            "batching"  => 1,
            "id"        => json_decode($request['distributor'])
        ];
        Log::info('get API PaymentAloneBatching ' . $api);
        Log::info('payload PaymentAloneBatching ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                if (!Storage::disk('optima')->exists('/assets/media/tools')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools');
                }
                if (!Storage::disk('optima')->exists('/assets/media/tools/sap-payment')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools/sap-payment');
                }
                $path_row = '/assets/media/tools/sap-payment/' . $request['uuid'];
                Storage::disk('optima')->put($path_row . '/dataPaymentAloneBatching.json', json_encode(json_decode($response)->values));
                $result = array(
                    'error'     => false,
                    'message'   => "Get Data PaymentAloneBatching Success"
                );
                Log::info(json_encode($result));
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error'     => true,
                    'message'   => "Get Data PaymentAloneBatching Failed"
                );
                Log::info('get API ' . $api);
                Log::warning($message);
            }
            return $result;
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Get Data PaymentAloneBatching Failed"
            );
        }
    }

    public function getDataXmlNonBatching(Request $request)
    {
        $api = config('app.api'). '/tools/xmlgenerate/batch-payment';
        $query = [
            "entity"    => $request['entity'],
            "batching"  => 0,
            "id"        => json_decode($request['distributor'])
        ];
        Log::info('get API XmlNonBatching ' . $api);
        Log::info('payload XmlNonBatching ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                if (!Storage::disk('optima')->exists('/assets/media/tools')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools');
                }
                if (!Storage::disk('optima')->exists('/assets/media/tools/sap-payment')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools/sap-payment');
                }
                $path_row = '/assets/media/tools/sap-payment/' . $request['uuid'];
                Storage::disk('optima')->put($path_row . '/dataXmlNonBatching.json', json_encode(json_decode($response)->values));
                $result = array(
                    'error'     => false,
                    'message'   => "Get Data XmlNonBatching Success"
                );
                Log::info(json_encode($result));
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error'     => true,
                    'message'   => "Get Data XmlNonBatching Failed"
                );
                Log::info('get API ' . $api);
                Log::warning($message);
            }
            return $result;
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Get Data XmlNonBatching Failed"
            );
        }
    }

    public function getDataXmlBatching(Request $request)
    {
        $api = config('app.api'). '/tools/xmlgenerate/batch-payment';
        $query = [
            "entity"    => $request['entity'],
            "batching"  => 1,
            "id"        => json_decode($request['distributor'])
        ];
        Log::info('get API XmlBatching ' . $api);
        Log::info('payload XmlBatching ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                if (!Storage::disk('optima')->exists('/assets/media/tools')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools');
                }
                if (!Storage::disk('optima')->exists('/assets/media/tools/sap-payment')) {
                    Storage::disk('optima')->makeDirectory('/assets/media/tools/sap-payment');
                }
                $path_row = '/assets/media/tools/sap-payment/' . $request['uuid'];
                Storage::disk('optima')->put($path_row . '/dataXmlBatching.json', json_encode(json_decode($response)->values));
                $result = array(
                    'error'     => false,
                    'message'   => "Get Data XmlBatching Success"
                );
                Log::info(json_encode($result));
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error'     => true,
                    'message'   => "Get Data XmlBatching Failed"
                );
                Log::info('get API ' . $api);
                Log::warning($message);
            }
            return $result;
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Get Data XmlBatching Failed"
            );
        }
    }

    public function downloadDistributorPaymentAloneNonBatching(Request $request)
    {
        try {
            $entity_code = match ($request['entity']) {
                '2' => "4450",
                '3' => "5600",
                '4' => "5610",
                default => "",
            };

            Log::info('downloadDistributorPaymentAloneNonBatching');
            $path = Storage::disk('optima')->get('/assets/media/tools/sap-payment/' . $request['uuid'] . '/dataPaymentAloneNonBatching.json');

            $dataJson = json_decode($path, true);
            $data = $dataJson['distributorpayment'];
            $entity = $dataJson['xmlgenerate'][0]['entityDesc'];

            $result=[];
            $xNo = 0;
            foreach ($data as $fields) {
                $xNo += 1;
                $arr = [];
                $arr[] = $xNo;
                $arr[] = $fields['batchName'];
                $arr[] = $fields['dnDescription'];
                $arr[] = $fields['distributor'];
                $arr[] = $fields['budgetBucket'];
                $arr[] = $fields['invoiceDNNumber'];
                $arr[] = $fields['dnAmount'];
                $arr[] = $fields['feeAmount'];
                $arr[] = $fields['feePct'] / 100;
                $arr[] = $fields['ppnAmt'];
                $arr[] = $fields['ppnPct'] / 100;
                $arr[] = $fields['pphAmt'];
                $arr[] = $fields['pphPct'] / 100;
                $arr[] = $fields['totalToPay'];
                $arr[] = $fields['taxLevel'];
                $arr[] = $fields['fpNumber'];
                if(date('Y-m-d' , strtotime($fields['fpDate']))=='1970-01-01' || date('Y-m-d' , strtotime($fields['fpDate']))=='0001-01-01'){
                    $arr[] = "";
                }else{
                    $arr[] = date('d-m-Y' , strtotime($fields['fpDate']));
                }
                $arr[] = $fields['originalId'];

                $result[] = $arr;
            }

            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'PROPOSED BY';
            $arr[] = '';
            $arr[] = 'REVIEWED BY';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'APPROVED BY';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'SALES FINANCE ADMIN';
            $arr[] = '';
            $arr[] = 'CUSTOMER ACTIVATION MANAGER - MS';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'REVENUE GROWTH CONTROLLER';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            // $arr[] = 'FATTAH';
            $arr[] = $request->session()->get('name');;
            $arr[] = '';
            $arr[] = 'ARIF FADILLAH';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'LOLY ANDASARI';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $filename = 'ID9_DUMBCP_'. $entity_code . '_DIST_PAYMENT ' . date('Y-m-d His') . '_Non Batching.xlsx';
            $title = 'A1:R1'; //Report Title Bold and merge
            $header = 'A4:R4'; //Header Column Bold and color
            $heading = [
                ['DISTRIBUTOR PAYMENT LIST'],
                ['Entity: ' . $entity],
                ['Date Retrieved : ' . date('Y-m-d')],
                [
                    'NO', 'BATCH NAME', 'DN DESCRIPTION', 'DISTRIBUTOR', 'BUDGET BUCKET', 'PONumber (XML)', 'DN AMOUNT', 'FEE', 'FEE (%)', 'VAT', 'VAT (%)', 'WHT', 'WHT (%)', 'TOTAL TO PAY',
                    'TAXLEVEL', 'FP NUMBER', 'FP DATE', 'ORIGINAL DN NUMBER'
                ]
            ];
            $formatCell =  [
                'A' => NumberFormat::FORMAT_NUMBER,
                'B' => NumberFormat::FORMAT_TEXT,
                'C' => NumberFormat::FORMAT_TEXT,
                'D' => NumberFormat::FORMAT_TEXT,
                'E' => NumberFormat::FORMAT_TEXT,
                'F' => NumberFormat::FORMAT_TEXT,
                'G' => '#,##0',
                'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'I' => NumberFormat::FORMAT_PERCENTAGE_00,
                'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'K' => NumberFormat::FORMAT_PERCENTAGE_00,
                'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'M' => NumberFormat::FORMAT_PERCENTAGE_00,
                'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'O' => NumberFormat::FORMAT_TEXT,
                'P' => NumberFormat::FORMAT_TEXT,
                'R' => NumberFormat::FORMAT_TEXT,
            ];
            $export = new Export($result, $heading, $title, $header, $formatCell);
            return Excel::download($export, $filename);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Generate PaymentAloneNonBatching Failed"
            );
        }

    }

    public function downloadDistributorPaymentAloneBatching(Request $request)
    {
        try {
            $entity_code = match ($request['entity']) {
                '2' => "4450",
                '3' => "5600",
                '4' => "5610",
                default => "",
            };

            Log::info('downloadDistributorPaymentAloneBatching');
            $path = Storage::disk('optima')->get('/assets/media/tools/sap-payment/' . $request['uuid'] . '/dataPaymentAloneBatching.json');

            $dataJson = json_decode($path, true);
            $data = $dataJson['distributorpayment'];
            $entity = $dataJson['xmlgenerate'][0]['entityDesc'];

            $result=[];
            $xNo = 0;
            foreach ($data as $fields) {
                $xNo += 1;
                $arr = [];
                $arr[] = $xNo;
                $arr[] = $fields['batchName'];
                $arr[] = $fields['dnDescription'];
                $arr[] = $fields['distributor'];
                $arr[] = $fields['budgetBucket'];
                $arr[] = $fields['invoiceDNNumber'];
                $arr[] = $fields['dnAmount'];
                $arr[] = $fields['feeAmount'];
                $arr[] = $fields['feePct'] / 100;
                $arr[] = $fields['ppnAmt'];
                $arr[] = $fields['ppnPct'] / 100;
                $arr[] = $fields['pphAmt'];
                $arr[] = $fields['pphPct'] / 100;
                $arr[] = $fields['totalToPay'];
                $arr[] = $fields['taxLevel'];
                $arr[] = $fields['fpNumber'];
                if(date('Y-m-d' , strtotime($fields['fpDate']))=='1970-01-01' || date('Y-m-d' , strtotime($fields['fpDate']))=='0001-01-01'){
                    $arr[] = "";
                }else{
                    $arr[] = date('d-m-Y' , strtotime($fields['fpDate']));
                }
                $arr[] = $fields['originalId'];

                $result[] = $arr;
            }

            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'PROPOSED BY';
            $arr[] = '';
            $arr[] = 'REVIEWED BY';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'APPROVED BY';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();
            $result[] = $this->loopKosong();

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'SALES FINANCE ADMIN';
            $arr[] = '';
            $arr[] = 'CUSTOMER ACTIVATION MANAGER - MS';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'REVENUE GROWTH CONTROLLER';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $arr = [];
            $arr[] = '';
            $arr[] = '';
            // $arr[] = 'FATTAH';
            $arr[] = $request->session()->get('name');;
            $arr[] = '';
            $arr[] = 'ARIF FADILLAH';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = '';
            $arr[] = 'LOLY ANDASARI';
            $arr[] = '';
            $arr[] = '';
            $result[] = $arr;

            $filename = 'ID9_DUMBCP_'. $entity_code . '_DIST_PAYMENT ' . date('Y-m-d His') . '_Batching.xlsx';
            $title = 'A1:R1'; //Report Title Bold and merge
            $header = 'A4:R4'; //Header Column Bold and color
            $heading = [
                ['DISTRIBUTOR PAYMENT LIST'],
                ['Entity: ' . $entity],
                ['Date Retrieved : ' . date('Y-m-d')],
                [
                    'NO', 'BATCH NAME', 'DN DESCRIPTION', 'DISTRIBUTOR', 'BUDGET BUCKET', 'PONumber (XML)', 'DN AMOUNT', 'FEE', 'FEE (%)', 'VAT', 'VAT (%)', 'WHT', 'WHT (%)', 'TOTAL TO PAY',
                    'TAXLEVEL', 'FP NUMBER', 'FP DATE', 'ORIGINAL DN NUMBER'
                ]
            ];
            $formatCell =  [
                'A' => NumberFormat::FORMAT_NUMBER,
                'B' => NumberFormat::FORMAT_TEXT,
                'C' => NumberFormat::FORMAT_TEXT,
                'D' => NumberFormat::FORMAT_TEXT,
                'E' => NumberFormat::FORMAT_TEXT,
                'F' => NumberFormat::FORMAT_TEXT,
                'G' => '#,##0',
                'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'I' => NumberFormat::FORMAT_PERCENTAGE_00,
                'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'K' => NumberFormat::FORMAT_PERCENTAGE_00,
                'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'M' => NumberFormat::FORMAT_PERCENTAGE_00,
                'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                'O' => NumberFormat::FORMAT_TEXT,
                'P' => NumberFormat::FORMAT_TEXT,
                'R' => NumberFormat::FORMAT_TEXT,
            ];
            $export = new Export($result, $heading, $title, $header, $formatCell);
            return Excel::download($export, $filename);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Generate PaymentAloneBatching Failed"
            );
        }

    }

    public function downloadXmlNonBatching(Request $request)
    {
        try {
            $entity_code = match ($request['entity']) {
                '2' => "4450",
                '3' => "5600",
                '4' => "5610",
                default => "",
            };

            Log::info('downloadXmlNonBatching');
            $path = Storage::disk('optima')->get('/assets/media/tools/sap-payment/' . $request['uuid'] . '/dataXmlNonBatching.json');
            $dataJson = json_decode($path, true);
            $data = $dataJson['xmlgenerate'];

            if ($data) {
                $domTree = new \DOMDocument('1.0', 'UTF-8');
                $domTree->xmlStandalone = true;
                $domTree->preserveWhiteSpace = true;
                $domTree->formatOutput = true;
                $rootXML = $domTree->createElement( 'TO_COR_BCP_ORDER' );
                $rootXML = $domTree->appendChild($rootXML);

                $ar_PONumber = array();
                for ($i=0; $i<count($data); $i++) {
                    $BCPOrder = $domTree->createElement('BCPOrder');
                    $BCPOrder = $rootXML->appendChild($BCPOrder);

                    foreach ($data[$i] as $key => $item) {
                        if ($item !== " ") {
                            if ($key == "poNumber") {
                                $child = $domTree->createElement('PONumber');
                                $child = $BCPOrder->appendChild($child)->nodeValue = $item;
                            } else if ($key == "poDate") {
                                $child = $domTree->createElement('PODate');
                                $child = $BCPOrder->appendChild($child)->nodeValue = $item;
                            } else if ($key == "requestedDeliveryDate") {
                                $child = $domTree->createElement('RequestedDeliveryDate');
                                $child = $BCPOrder->appendChild($child)->nodeValue = date('Ymd');
                            } else if ($key ==  "entityId" || $key == "entityDesc" || $key == "originalId") {
                            } else {
                                $child = $domTree->createElement(ucfirst($key));
                                $child = $BCPOrder->appendChild($child)->nodeValue =  str_replace("&", "&amp;", $item);
                            }
                        }
                    }

                    array_push($ar_PONumber, $data[$i]['poNumber']);
                }

                $response = Response::make( preg_replace("/[\n]/", "\r\n", $domTree->saveXML()), 200 );
                $response->header('Cache-Control', 'public');
                $response->header('Content-Description', 'File Transfer');
                $response->header('Content-Disposition', 'attachment; filename=ID9_DUMBCP_'. $entity_code .'_'. date('Ymd') . '_PAYMENT_Non_Batching.xml');
                $response->header('Content-Transfer-Encoding', 'binary');
                $response->header('Content-Type', 'text/xml');

                return $response;
            } else {
                Log::info("XmlNonBatching no Data");
                return "No data";
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Generate XmlNonBatching Failed"
            );
        }
    }

    public function downloadXmlBatching(Request $request)
    {
        try {
            $entity_code = match ($request['entity']) {
                '2' => "4450",
                '3' => "5600",
                '4' => "5610",
                default => "",
            };

            Log::info('downloadXmlBatching');
            $path = Storage::disk('optima')->get('/assets/media/tools/sap-payment/' . $request['uuid'] . '/dataXmlBatching.json');
            $dataJson = json_decode($path, true);
            $data = $dataJson['xmlgenerate'];

            if ($data) {
                $domTree = new \DOMDocument('1.0', 'UTF-8');
                $domTree->xmlStandalone = true;
                $domTree->preserveWhiteSpace = true;
                $domTree->formatOutput = true;
                $rootXML = $domTree->createElement('TO_COR_BCP_ORDER');
                $rootXML = $domTree->appendChild($rootXML);

                $ar_PONumber = array();
                for ($i = 0; $i < count($data); $i++) {
                    $BCPOrder = $domTree->createElement('BCPOrder');
                    $BCPOrder = $rootXML->appendChild($BCPOrder);

                    foreach ($data[$i] as $key => $item) {
                        if ($item !== " ") {
                            if ($key == "poNumber") {
                                $child = $domTree->createElement('PONumber');
                                $child = $BCPOrder->appendChild($child)->nodeValue = $item;
                            } else if ($key == "poDate") {
                                $child = $domTree->createElement('PODate');
                                $child = $BCPOrder->appendChild($child)->nodeValue = $item;
                            } else if ($key == "requestedDeliveryDate") {
                                $child = $domTree->createElement('RequestedDeliveryDate');
                                $child = $BCPOrder->appendChild($child)->nodeValue = date('Ymd');
                            } else if ($key == "entityId" || $key == "entityDesc" || $key == "originalId") {
                            } else {
                                $child = $domTree->createElement(ucfirst($key));
                                $child = $BCPOrder->appendChild($child)->nodeValue = str_replace("&", "&amp;", $item);
                            }
                        }
                    }
                }

                $response = Response::make(preg_replace("/[\n]/", "\r\n", $domTree->saveXML()), 200);
                $response->header('Cache-Control', 'public');
                $response->header('Content-Description', 'File Transfer');
                $response->header('Content-Disposition', 'attachment; filename=ID9_DUMBCP_' . $entity_code . '_' . date('Ymd') . '_PAYMENT.xml');
                $response->header('Content-Transfer-Encoding', 'binary');
                $response->header('Content-Type', 'text/xml');

                return $response;
            } else {
                Log::info("XmlBatching no Data");
                return "No data";
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Generate XmlBatching Failed"
            );
        }
    }

    public function flaggingPayment(Request $request)
    {
        try {
            // clear directory sap payment generate yesterday
            $this->removeDirGeneratePayment();

            $path = Storage::disk('optima')->get('/assets/media/tools/sap-payment/' . $request['uuid'] . '/dataXmlBatching.json');
            $dataJson = json_decode($path, true);
            $data = $dataJson['xmlgenerate'];

            if ($data) {
                $ar_PONumber = array();
                for ($i = 0; $i < count($data); $i++) {
                    $poNumber = [
                        'poNumber'      => $data[$i]['poNumber'],
                        "originalId"    => $data[$i]['originalId'],
                        "entityId"      => $data[$i]['entityId'],
                    ];

                    array_push($ar_PONumber, $poNumber);
                }

                $apiFlagging = config('app.api') . '/tools/xmlgenerate/flagging';
                $data = [
                    "userid"    => $request->session()->get('profile'),
                    "useremail" => $request->session()->get('email'),
                    "poNumber"  => $ar_PONumber,
                ];
                Log::info('get api Flagging Payment ' . $apiFlagging);
                Log::info('Payload Flagging Payment ' . json_encode($data));
                $responseFlagging = Http::timeout(180)->withToken($this->token)->post($apiFlagging, $data);
                if ($responseFlagging->status() === 200) {
                    return array(
                        'error'     => false,
                        'message'   => "Flagging Payment Success"
                    );
                } else {
                    Log::warning('api Flagging Payment ' . $apiFlagging);
                    Log::warning('Payload Flagging Payment ' . $responseFlagging);
                    return array(
                        'error'     => true,
                        'message'   => "Flagging Payment Failed"
                    );
                }
            } else {
                Log::info("XmlBatching no Data");
                return array(
                    'error'     => true,
                    'message'   => "Flagging Payment Failed"
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'message'   => "Flagging Payment Failed"
            );
        }
    }

    private function removeDirGeneratePayment()
    {
        try {
            if (Storage::disk('optima')->exists('/assets/media/tools/sap-payment')) {
                $dirUUID = Storage::disk('optima')->allDirectories('/assets/media/tools/sap-payment');

                if (count($dirUUID) > 0) {
                    for ($i=0; $i<count($dirUUID); $i++) {
                        $dir = str_replace('assets/media/tools/sap-payment/', '', $dirUUID[$i]);
                        $year = substr($dir, 0, 4);
                        $month = substr($dir, 4, 2);
                        $day = substr($dir, 6, 2);
                        $dateDir = $year . '-' . $month . '-' . $day;

                        if ($dateDir < date('Y-m-d')) {
                            Storage::disk('optima')->deleteDirectory($dirUUID[$i]);
                        }
                    }
                }
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
        }

    }

}
