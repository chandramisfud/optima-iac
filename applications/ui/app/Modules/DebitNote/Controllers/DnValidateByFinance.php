<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use ZipArchive;
use Wording;
use File;

class DnValidateByFinance extends Controller
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
        $title = "Debit Note [Validate by Finance]";
        return view('DebitNote::dn-validate-by-finance.index', compact('title'));
    }

    public function approvalPage()
    {
        $title = "Debit Note [Validate By Finance]";
        return view('DebitNote::dn-validate-by-finance.form', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/dn/validation-byfinance';
        $query = [
            'entityid'          => (int) $request->entityId,
            'distributorid'     => (int) $request->distributorId,
            'TaxLevel'          => 0
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/dn/validation-byfinance/entity';
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

    public function getListWHTType(): bool|string
    {
        $api = '/mapping/distributor-wht/whttype';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getDataWHTTypeByPromoId(Request $request): bool|string
    {
        $api = '/dn/creation/whttype';
        $query = [
            'promoId'  => $request['promoId']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListDistributorByEntityId(Request $request)
    {
        $api = config('app.api'). '/dn/validation-byfinance/distributor';
        $query = [
            'budgetId'  => 0,
            'entityId'  => $request->entityId,
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

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/validation-byfinance/id';
        $query = [
            'id'       => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function getDataPromo(Request $request)
    {
        $api = config('app.api'). '/dn/validation-byfinance/promo/id';
        $query = [
            'id'       => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function viewFile(Request $request)
    {
        $title = 'Debit Note [Validate by Finance]';
        $id = $request->id;
        try {
            $no = str_replace("row","",$request->row);
            $path_promo = '/assets/media/debitnote/' . $id . '/row' . $request->row . '/' . $request->fileName;

            return view('DebitNote::dn-validate-by-finance.view', compact('id', 'title','path_promo'));

        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function downloadZip(Request $request) {
        $api = config('app.api'). '/dn/creation/id';
        $query = [
            'id'  => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                if ($data) {
                    $zip = new ZipArchive;

                    $path = 'assets/media/debitnote/' . $request->id;
                    if (!File::isDirectory($path)) {
                        File::makeDirectory($path,0777,true);
                    }

                    $filename_zip = $data->refId . '.zip';
                    if(file_exists($path . '/' . $filename_zip)){
                        Log::info('delete file zip');
                        unlink($path . '/' . $filename_zip);
                    }
                    if(count($data->dnattachment)>0){
                        if (true === ($zip->open($path . '/' . $filename_zip, ZipArchive::CREATE | ZipArchive::OVERWRITE))) {
                            for ($i = 0; $i < count($data->dnattachment); $i++)  {
                                if($data->dnattachment[$i]->docLink=='row1')
                                    if(file_exists($path . '/row1/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row1/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row2')
                                    if(file_exists($path . '/row2/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row2/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row3')
                                    if(file_exists($path . '/row3/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row3/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row4')
                                    if(file_exists($path . '/row4/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row4/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row5')
                                    if(file_exists($path . '/row5/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row5/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row6')
                                    if(file_exists($path . '/row6/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row6/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row7')
                                    if(file_exists($path . '/row7/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row7/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row8')
                                    if(file_exists($path . '/row8/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row8/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                                if($data->dnattachment[$i]->docLink=='row9')
                                    if(file_exists($path . '/row9/' . $data->dnattachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row9/' . $data->dnattachment[$i]->fileName), $data->dnattachment[$i]->fileName);
                                    }
                            }
                            $fp = fopen($path . '/' . $filename_zip, 'w');
                            fwrite($fp, '');
                            fclose($fp);
                            chmod($path . '/' . $filename_zip, 0755);

                            $zip->close();
                            return response()->download(public_path($path . '/' . $filename_zip));
                        } else {
                            return array(
                                'error'     => true,
                                'data'      => [],
                                'message'   => 'Error : Can not create Zip File'
                            );
                        }
                    } else {
                        return array(
                            'error'     => true,
                            'data'      => [],
                            'message'   => 'Error : No File'
                        );
                    }
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

    public function submit(Request $request) {
        $api = config('app.api') . '/dn/validation-byfinance/doc-completeness';

        if($request->taxLevel==null){ $taxLevel=""; }else{ $taxLevel=$request->taxLevel; }
        if($request->Original_Invoice_from_retailers=='on'){ $Original_Invoice_from_retailers=true; }else{ $Original_Invoice_from_retailers=false; }
        if($request->Tax_Invoice=='on'){ $Tax_Invoice=true; }else{ $Tax_Invoice=false; }
        if($request->Promotion_Agreement_Letter=='on'){ $Promotion_Agreement_Letter=true; }else{ $Promotion_Agreement_Letter=false; }
        if($request->Trading_Term=='on'){ $Trading_Term=true; }else{ $Trading_Term=false; }
        if($request->Sales_Data=='on'){ $Sales_Data=true; }else{ $Sales_Data=false; }
        if($request->Copy_of_mailer=='on'){ $Copy_of_mailer=true; }else{ $Copy_of_mailer=false; }
        if($request->Copy_of_photo_doc=='on'){ $Copy_of_photo_doc=true; }else{ $Copy_of_photo_doc=false; }
        if($request->List_of_Transfer=='on'){ $List_of_Transfer=true; }else{ $List_of_Transfer=false; }

        $dnDocCompletenessHeader = [
            "dnId"                              => (int) $request->dnid,
            "original_Invoice_from_retailers"   => $Original_Invoice_from_retailers,
            "tax_Invoice"                       => $Tax_Invoice,
            "promotion_Agreement_Letter"        => $Promotion_Agreement_Letter,
            "trading_Term"                      => $Trading_Term,
            "sales_Data"                        => $Sales_Data,
            "copy_of_Mailer"                    => $Copy_of_mailer,
            "copy_of_Photo_Doc"                 => $Copy_of_photo_doc,
            "list_of_Transfer"                  => $List_of_Transfer
        ];
        if($request->promoId == null){ $promoId = 0; } else { $promoId = $request->promoId; }
        if($request->entityId == null){ $entityId = 0; } else { $entityId = $request->entityId; }
        $data = [
            "dnId"              => (int) $request->dnid,
            "status"            => $request->approvalStatusCode,
            "notes"             => $request->notes,
            "taxlevel"          => $taxLevel,
            "entityId"          => $entityId,
            "promoId"           => $promoId,
            "isDNPromo"         => $request->isDNPromo === "true",
            "wHTType"           => $request['whtType'] ?? "",
            "statusPPH"         => $request['statusPPH'],
            "pphPct"            => str_replace(',', '', $request['pphPct']),
            "pphAmt"            => str_replace(',', '', $request['pphAmt']),
            "dnDocCompletenessHeader" => $dnDocCompletenessHeader
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                if (json_decode($response)->code === 200) {
                    return json_encode([
                        'error'     => false,
                        'message'   => 'DN Validated'
                    ]);
                } else {
                    return json_encode([
                        'error'     => true,
                        'message'   => $message
                    ]);
                }
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

    public function save(Request $request) {
        $api = config('app.api') . '/dn/validation-byfinance/save-doc-completeness';

        $dnDocCompletenessHeader = [
            "original_Invoice_from_retailers"   => (($request['original_Invoice_from_retailers'] === "true") ? true : false),
            "tax_Invoice"                       => (($request['tax_Invoice'] === "true") ? true : false),
            "promotion_Agreement_Letter"        => (($request['promotion_Agreement_Letter'] === "true") ? true : false),
            "trading_Term"                      => (($request['trading_Term'] === "true") ? true : false),
            "sales_Data"                        => (($request['sales_Data'] === "true") ? true : false),
            "copy_of_Mailer"                    => (($request['copy_of_Mailer'] === "true") ? true : false),
            "copy_of_Photo_Doc"                 => (($request['copy_of_Photo_Doc'] === "true") ? true : false),
            "list_of_Transfer"                  => (($request['list_of_Transfer'] === "true") ? true : false)
        ];

        $data = [
            "dnId"                      => (int) $request['dnid'],
            "dnDocCompletenessHeader"   => $dnDocCompletenessHeader
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                if (json_decode($response)->code === 200) {
                    return json_encode([
                        'error'     => false,
                        'message'   => $message
                    ]);
                } else {
                    return json_encode([
                        'error'     => true,
                        'message'   => $message
                    ]);
                }
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

    public function vatExpiredUpdate(Request $request) {
        $api = config('app.api') . '/dn/vatexpired?id=' . $request->id . '&VATExpired=' . $request->VATExpired;
        $query = [
            'id'              => $request->id,
            'VATExpired'      => $request->VATExpired,
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($query));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $query);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => $message
                ]);
            } else {
                Log::error($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            $message = $e->getMessage();
            Log::error($message);
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $message
            );
        }
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/dn/validation-byfinance/filter';
        Log::info('post API ' . $api);

        $data_multipart = [];
        array_push($data_multipart, [
            'name'  => 'status',
            'contents' => 'validate_by_finance'
        ], [
            'name'  => 'entity',
            'contents' => 0
        ], [
            'name'  => 'taxLevel',
            'contents' => '0'
        ], [
            'name'     => 'formFile',
            'contents' => $request->file('file')->getContent(),
            'filename' => $request->file('file')->getClientOriginalName()
        ]);
        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, $data_multipart);
                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
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
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }
}
