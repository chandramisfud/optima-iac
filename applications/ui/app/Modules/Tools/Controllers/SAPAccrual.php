<?php

namespace App\Modules\Tools\Controllers;

use Illuminate\Support\Facades\Response;
use Illuminate\Support\Facades\Storage;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use App\Exports\Tools\ExportSAPAccrual;
use Excel;
use sys;
class SAPAccrual extends Controller
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
        return view('Tools::sap-accrual.index');
    }

    public function getListUploadHistory(Request $request)
    {
        $api = config('app.api'). '/tools/xmlgenerate/xmlupload';
        $query = [
            'uploadtype'        => 'accrual'
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

    public function getListReportHeader(Request $request)
    {
        $api = config('app.api'). '/tools/sap-accrual/promo-accrual';
        $query = [
            'periode'   => $request->period,
            'entityId'    => ($request->entityId ?? 0),
            'closingDate' => $request->closingDate,
        ];
        Log::info('Get API ' . $api);
        Log::info('Payload ' . json_encode($query));
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

    public function uploadXml(Request $request) {
        try {
            if ($request->file('file')) {
                $filenametostore = $request->file('file')->getClientOriginalName();

                $api = config('app.api') . '/tools/xmlgenerate/xmlupload';

                $data = [
                    "userid"        => $request->session()->get('profile'),
                    "useremail"     => $request->session()->get('email'),
                    "filename"      => $filenametostore,
                    "uploadtype"    => 'accrual',
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/tools/xmlgenerate/nmn/id';
        $query = [
            "id"    => (int)$request->id
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $entity_code = "5610";
                $resVal = json_decode($response)->values;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->nomor;
                    $arr[] = $fields->glaw;
                    $arr[] = $fields->dateretrieve1;
                    $arr[] = $fields->dateretrieve2;
                    $arr[] = $fields->entitycode;
                    $arr[] = $fields->idr;
                    $arr[] = $fields->blank1;
                    $arr[] = $fields->nama;
                    $arr[] = $fields->tc;
                    $arr[] = $fields->dateretrieve10;
                    $arr[] = str_replace("'","",$fields->noldua);
                    $arr[] = $fields->cbufin;
                    $arr[] = $fields->topline;
                    $arr[] = $fields->blank2;
                    $arr[] = $fields->fix1;
                    $arr[] = $fields->blank3;
                    $arr[] = $fields->fix2;
                    $arr[] = $fields->blank4;
                    $arr[] = $fields->blank5;
                    $arr[] = $fields->ytdvalue;
                    $arr[] = $fields->fix3;
                    $arr[] = $fields->blank6;
                    $arr[] = $fields->blank7;
                    $arr[] = $fields->accruts;
                    $arr[] = $fields->promoid;
                    $arr[] = $fields->blank8;
                    $arr[] = $fields->my;
                    $arr[] = $fields->blank10;
                    $arr[] = $fields->blank11;
                    $arr[] = $fields->sh;
                    $arr[] = $fields->blank12;
                    $arr[] = $fields->blank13;
                    $arr[] = $fields->blank14;
                    $arr[] = $fields->blank15;
                    $arr[] = $fields->blank16;
                    $arr[] = $fields->blank17;
                    $arr[] = $fields->blank18;

                    $result[] = $arr;
                }

                $all = 'A1:AK'.count($resVal);
                $formatCell =  [
                ];

                $export = new ExportSAPAccrual($result,$all,$formatCell);
                return Excel::download($export, 'ID9_'. $entity_code .'_accr_'. $request->session()->get('profile') . '_' . date('Ymd') . '_' . date('His'). '.xlsx');
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error' => $response->status(),
                    'data' => [],
                    'message' => $message
                );
                Log::warning($message);
                return $result;
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportXML(Request $request) {
        $api = config('app.api'). '/tools/xmlgenerate/accrual/id';

        $query = [
            "id"    => (int)$request->id
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
                if ($data) {
                    // Create Accrual
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
                                } else if ($key ==  "entityId" || $key == "entityDesc" || $key=='principalId' || $key=='principalDesc') {
                                } else {
                                    $child = $domTree->createElement(ucfirst($key));
                                    $child = $BCPOrder->appendChild($child)->nodeValue =  str_replace("&", "&amp;", $item);
                                }
                            }
                        }
                        array_push($ar_PONumber, $data[$i]->poNumber);
                    }
                    $filename = 'ID9_'. $entity_code .'_accr_'. $request->session()->get('profile')  . '_' . date('Ymd') . '_' . date('His'). '.xml';
                    $response = Response::make( preg_replace("/[\n]/", "\r\n", $domTree->saveXML()), 200 );
                    $response->header('Cache-Control', 'public');
                    $response->header('Content-Description', 'File Transfer');
                    $response->header('Content-Disposition', 'attachment; filename='.$filename);
                    $response->header('Content-Transfer-Encoding', 'binary');
                    $response->header('Content-Type', 'text/xml');

                    return $response;
                }else{
                    Log::info($api. " no Data");
                    return "No data";
                }
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

    public function exportXMLReversal(Request $request) {
        $api = config('app.api'). '/tools/xmlgenerate/accrual/id';

        $query = [
            "id"    => (int)$request->id
        ];

        $entity_code = "";
        switch ($request->entity) {
            case 0:
                $entity_code = "0000";
                break;
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
                if ($data) {
                    // Create Reversal
                    $domTree = new \DOMDocument('1.0', 'UTF-8');
                    $domTree->xmlStandalone = true;
                    $domTree->preserveWhiteSpace = true;
                    $domTree->formatOutput = true;
                    $rootXML = $domTree->createElement( 'TO_COR_BCP_ORDER' );
                    $rootXML = $domTree->appendChild($rootXML);

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
                                } else if ($key == "orderType") {
                                    $child = $domTree->createElement(ucfirst($key));
                                    $child = $BCPOrder->appendChild($child)->nodeValue =  "ZTD6";
                                } else if ($key ==  "entityId" || $key == "entityDesc" || $key=='principalId' || $key=='principalDesc') {
                                } else {
                                    $child = $domTree->createElement(ucfirst($key));
                                    $child = $BCPOrder->appendChild($child)->nodeValue =  str_replace("&", "&amp;", $item);
                                }
                            }
                        }

                    }
                    $filename = 'ID9_'. $entity_code .'_accr rvs_'. $request->session()->get('profile') . '_' . date('Ymd') . '_' . date('His'). '.xml';
                    $response = Response::make( preg_replace("/[\n]/", "\r\n", $domTree->saveXML()), 200 );
                    $response->header('Cache-Control', 'public');
                    $response->header('Content-Description', 'File Transfer');
                    $response->header('Content-Disposition', 'attachment; filename='.$filename);
                    $response->header('Content-Transfer-Encoding', 'binary');
                    $response->header('Content-Type', 'text/xml');

                    return $response;
                }else{
                    Log::info($api. " no Data");
                    return "No data";
                }
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
