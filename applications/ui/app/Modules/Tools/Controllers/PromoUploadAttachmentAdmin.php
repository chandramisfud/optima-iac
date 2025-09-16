<?php

namespace App\Modules\Tools\Controllers;

use App\Exports\Tools\ExportTemplatePromoUploadAttachment;
use App\Helpers\MyEncrypt;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use Session;
use App\Http\Requests;
use Illuminate\Support\Facades\Http;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\File;

class PromoUploadAttachmentAdmin extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage()
    {
        return view('Tools::upload-attachment-promo-admin.index');
    }

    public function temp(Request $request)
    {
        $path = storage_path('tmp/uploads/' . $request->session()->get('userid'));

        if (!file_exists($path)) {
            mkdir($path, 0777, true);
        }

        $file = $request->file('file');
        $name = trim($file->getClientOriginalName());

        $file->move($path, $name);

        return response()->json([
            'name'          => $name,
            'original_name' => $file->getClientOriginalName(),
        ]);
    }

    public function tempDelete(Request $request)
    {
        $filename =  $request->name;
        $path = storage_path('tmp/uploads/' . $request->session()->get('userid')). '/' . $filename;

        if (File::isFile($path)) {
            File::delete($path);
        }
        return $filename;
    }

    public function downloadTemplate(Request $request) {
        try {
            $result=[];
            $header = 'A1:H1'; //Header Column Bold and color
            $heading = [
                ['promonumber', 'attachment1', 'attachment2', 'attachment3', 'attachment4', 'attachment5', 'attachment6', 'attachment7']
            ];
            $filename = 'PromoUploadAttachments';
            $export = new ExportTemplatePromoUploadAttachment($result, $heading, $header);
            return Excel::download($export, $filename . '.xlsx');

        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return [];
        }
    }

    public function importData(Request $request) {
        $filename = "PromoUploadAttachments.xlsx";
        $path = storage_path('tmp/uploads/' . $request->session()->get('userid')). '/';
        $pathFile = $path . $filename;

        if (file_exists($pathFile)) {
            $result = Excel::toArray([], $pathFile);
            $keys      = array_keys($result);
            $arraySize = count($result[0]);

            $data = [];
            $resSuccesSum = 0;
            $resFailedSum = 0;
            $attach1 ='';
            for( $i=1; $i < $arraySize; $i++ ) {
                // Cek DN by RefId via API
                $PromoNumber = $result[0][$i][0];
                $attach1 = $result[0][$i][1];
                $attach2 = $result[0][$i][2];
                $attach3 = $result[0][$i][3];
                $attach4 = $result[0][$i][4];
                $attach5 = $result[0][$i][5];
                $attach6 = $result[0][$i][6];
                $attach7 = $result[0][$i][7];

                $resErr = 0;
                $resultPromo = "OK";
                $api = config('app.api'). '/tools/search/promo/refid';

                try{
                    $promoId = 0;
                    $response = Http::timeout(180)->withToken($this->token)->get($api, [
                        'refid'  => $PromoNumber,
                    ]);

                    if($response->status() !== 200){
                        $message = json_decode($response)->message;
                        Log::warning('get API ' . $api);
                        Log::warning($message);
                        $resultPromo = $response;
                        $resErr = 1;
                    }else{
                        $resVal = json_decode($response)->values;
                        $promoId = $resVal->id;
                        $resultPromo = "OK";
                    }
                } catch (\Exception $e) {
                    Log::error('get API ' . $api);
                    Log::error($e->getMessage());
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => $e->getMessage()
                    );
                    $resultPromo = $e->getMessage();
                }

                // Cek file exist
                $result1 = "OK";
                $pathFile1 = $path . $attach1;
                if (!file_exists($pathFile1)) {
                    Log::info('File ' . $pathFile1 . " doesn't exist");
                    $result1 = 'File ' . $attach1 . " doesn't exist";
                    $resErr = 1;
                }
                $result2 = "OK";
                $pathFile2 = $path . $attach2;
                if (!file_exists($pathFile2)) {
                    Log::info('File ' . $pathFile2 . " doesn't exist");
                    $result2 = 'File ' . $attach2 . " doesn't exist";
                    $resErr = 2;
                }
                $result3 = "OK";
                $pathFile3 = $path . $attach3;
                if (!file_exists($pathFile3)) {
                    Log::info('File ' . $pathFile3 . " doesn't exist");
                    $result3 = 'File ' . $attach3 . " doesn't exist";
                    $resErr = 3;
                }

                $result4 = "OK";
                $pathFile4 = $path . $attach4;
                if (!file_exists($pathFile4)) {
                    Log::info('File ' . $pathFile4 . " doesn't exist");
                    $result4 = 'File ' . $attach4 . " doesn't exist";
                    $resErr = 4;
                }
                $result5 = "OK";
                $pathFile5 = $path . $attach5;
                if (!file_exists($pathFile5)) {
                    Log::info('File ' . $pathFile5 . " doesn't exist");
                    $result5 = 'File ' . $attach5 . " doesn't exist";
                    $resErr = 5;
                }
                $result6 = "OK";
                $pathFile6 = $path . $attach6;
                if (!file_exists($pathFile6)) {
                    Log::info('File ' . $pathFile6 . " doesn't exist");
                    $result6 = 'File ' . $attach6 . " doesn't exist";
                    $resErr = 6;
                }

                $result7 = "OK";
                $pathFile7 = $path . $attach7;
                if (!file_exists($pathFile7)) {
                    Log::info('File ' . $pathFile7 . " doesn't exist");
                    $result7 = 'File ' . $attach7 . " doesn't exist";
                    $resErr = 7;
                }

                // update data Prommo file attaxh
                Log::info('Promo : '. $PromoNumber);
                $resultUpdate = "OK";
                $result11 = $result12 = $result13 = $result14 = $result15 = $result16 = $result17 = '';
                if($resErr == 0){
                    // update file attach1
                    $apiDelete = config('app.api'). '/tools/promoattachment?PromoId=' . $promoId;
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file1 : '. $attach1);

                    if($attach1 !='' and $attach1 != null){
                        try{
                            // delete data to database
                            $responseDelete1 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row1');
                            $resValDelete1 = json_decode($responseDelete1);
                            if($responseDelete1->status() !== 200){
                                Log::error($responseDelete1);
                                Log::error($apiDelete);
                                Log::error($resValDelete1);
                                $result11 = $resValDelete1;
                                $resErr = 11;
                            }else{
                                Log::info($responseDelete1);
                                Log::info('File ' . $attach1 . " deleted");
                                $result11 = 'File ' . $attach1 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file1 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result12 = $e->getMessage();

                        }

                        try{
                            // move file from temp to promo dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row1';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest1 = $path_row . "/" . $attach1;
                            File::copy($pathFile1, $pathDest1);
                            // store data to database
                            $responseUpdate1 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row1',
                                "fileName"  => $attach1,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate1 = json_decode($responseUpdate1)->values;
                            if($responseUpdate1->status() !== 200){
                                Log::error($responseUpdate1);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate1);
                                $result11 = $resValUpdate1;
                                $resErr = 11;
                            }else{
                                $result11 = 'File ' . $attach1 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file1 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result11 = $e->getMessage();

                        }
                    }
                    // update file attach2
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file2 : '. $attach2);
                    if($attach2 !='' and $attach2 != null){
                        try{
                            // delete data to database
                            $responseDelete2 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row2');
                            $resValDelete2= json_decode($responseDelete2);

                            if($responseDelete2->status() !== 200){
                                Log::error($responseDelete2);
                                Log::error($apiDelete);
                                Log::error($resValDelete2);
                                $result12 = $resValDelete2;
                                $resErr = 12;
                            }else{
                                $result12 = 'File ' . $attach2 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file2 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result12 = $e->getMessage();
                        }

                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row2';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest2 = $path_row . "/" . $attach2;
                            File::copy($pathFile2, $pathDest2);

                            // store data to database
                            $responseUpdate2 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row2',
                                "fileName"  => $attach2,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate2= json_decode($responseUpdate2)->values;
                            if($responseUpdate2->status() !== 200){
                                Log::error($responseUpdate2);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate2);
                                $result12 = $resValUpdate2;
                                $resErr = 12;
                            }else{
                                $result12 = 'File ' . $attach2 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file2 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result12 = $e->getMessage();
                        }
                    }
                    // update file attach3
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file3 : '. $attach3);
                    if($attach3 !='' and $attach3 != null){
                        try{
                            // delete data to database
                            $responseDelete3 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row3');
                            $resValDelete3= json_decode($responseDelete3);

                            if($responseDelete3->status() !== 200){
                                Log::error($responseDelete3);
                                Log::error($apiDelete);
                                Log::error($resValDelete3);
                                $result13 = $responseDelete3;
                                $resErr = 13;
                            }else{
                                $result13 = 'File ' . $attach3 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file3 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result13 = $e->getMessage();

                        }

                        try{
                            // move file from temp to promo dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row3';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest3 = $path_row . "/" . $attach3;
                            File::copy($pathFile3, $pathDest3);

                            // store data to database
                            $responseUpdate3 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row3',
                                "fileName"  => $attach3,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate3= json_decode($responseUpdate3)->values;

                            if($responseUpdate3->status() !== 200){
                                Log::error($responseUpdate3);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate3);
                                $result13 = $resValUpdate3;
                                $resErr = 13;
                            }else{
                                $result13 = 'File ' . $attach3 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file3 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result13 = $e->getMessage();
                        }
                    }
                    // update file attach4
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file4 : '. $attach4);
                    if($attach4 !='' and $attach4 != null){
                        try{
                            // delete data to database
                            $responseDelete4 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row4');
                            $resValDelete4= json_decode($responseDelete4);

                            if($responseDelete4->status() !== 200){
                                Log::error($responseDelete4);
                                Log::error($apiDelete);
                                Log::error($resValDelete4);
                                $result14 = $responseDelete4;
                                $resErr = 14;

                            }else{
                                $result14 = 'File ' . $attach4 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file2 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result14 = $e->getMessage();

                        }

                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row4';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest4 = $path_row . "/" . $attach4;
                            File::copy($pathFile4, $pathDest4);

                            // store data to database
                            $responseUpdate4 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row4',
                                "fileName"  => $attach4,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate4= json_decode($responseUpdate4)->values;

                            if($responseUpdate4->status() !== 200){
                                Log::error($responseUpdate4);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate4);
                                $result14 = $resValUpdate4;
                                $resErr = 14;
                            }else{
                                $result14 = 'File ' . $attach4 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file4 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result14 = $e->getMessage();

                        }
                    }
                    // update file attach5
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file5 : '. $attach5);
                    if($attach5 !='' and $attach5 != null){
                        try{
                            // delete data to database
                            $responseDelete5 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row5');
                            $resValDelete5= json_decode($responseDelete5);

                            if($responseDelete5->status() !== 200){
                                Log::error($responseDelete5);
                                Log::error($apiDelete);
                                Log::error($resValDelete5);
                                $result15 = $responseDelete5;
                                $resErr = 15;

                            }else{
                                $result15 = 'File ' . $attach5 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file5 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result15 = $e->getMessage();

                        }

                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row5';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest5 = $path_row . "/" . $attach5;
                            File::copy($pathFile5, $pathDest5);

                            // store data to database
                            $responseUpdate5 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row5',
                                "fileName"  => $attach5,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate5= json_decode($responseUpdate5)->values;

                            if($responseUpdate5->status() !== 200){
                                Log::error($responseUpdate5);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate5);
                                $result15 = $resValUpdate5;
                                $resErr = 15;
                            }else{
                                $result15 = 'File ' . $attach5 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file5 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result15 = $e->getMessage();

                        }
                    }
                    // update file attach6
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file6 : '. $attach6);
                    if($attach6 !='' and $attach6 != null){
                        try{
                            // delete data to database
                            $responseDelete6 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row6');
                            $resValDelete6= json_decode($responseDelete6);

                            if($responseDelete6->status() !== 200){
                                Log::error($responseDelete6);
                                Log::error($apiDelete);
                                Log::error($resValDelete6);
                                $result16 = $responseDelete6;
                                $resErr = 16;

                            }else{
                                $result16 = 'File ' . $attach6 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file6 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result16 = $e->getMessage();

                        }

                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row6';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest6 = $path_row . "/" . $attach6;
                            File::copy($pathFile6, $pathDest6);

                            // store data to database
                            $responseUpdate6 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row6',
                                "fileName"  => $attach6,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate6= json_decode($responseUpdate6)->values;

                            if($responseUpdate6->status() !== 200){
                                Log::error($responseUpdate6);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate6);
                                $result16 = $resValUpdate6;
                                $resErr = 16;
                            }else{
                                $result16 = 'File ' . $attach6 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file6 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result16 = $e->getMessage();

                        }
                    }

                    // update file attach7
                    $apiUpdate = config('app.api'). '/tools/promoattachment';
                    // ip_dn_attachment_store
                    Log::info('file7 : '. $attach7);
                    if($attach7 !='' and $attach7 != null){
                        try{
                            // delete data to database
                            $responseDelete7 =  Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=row7');
                            $resValDelete7= json_decode($responseDelete7);

                            if($responseDelete7->status() !== 200){
                                Log::error($responseDelete7);
                                Log::error($apiDelete);
                                Log::error($responseDelete7);
                                $result17 = $resValDelete7;
                                $resErr = 17;

                            }else{
                                $result17 = 'File ' . $attach7 . " deleted";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file7 ' . $apiDelete);
                            Log::error($e->getMessage());
                            $result17 = $e->getMessage();

                        }

                        try{
                            // move file from temp to promo dest
                            $pathDN = public_path().'/assets/media/promo/' . $promoId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/promo/' . $promoId . '/row7';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest7 = $path_row . "/" . $attach7;
                            File::copy($pathFile7, $pathDest7);

                            // store data to database
                            $responseUpdate7 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "promoId"   => $promoId,
                                "docLink"   => 'row7',
                                "fileName"  => $attach7,
                                "createOn"  => date('Y-m-d H:m:s'),
                                "createBy"  => $request->session()->get('userid')
                            ]);

                            $resValUpdate7= json_decode($responseUpdate7)->values;

                            if($responseUpdate7->status() !== 200){
                                Log::error($responseUpdate7);
                                Log::error($apiUpdate);
                                Log::error($resValUpdate7);
                                $result17 = $resValUpdate7;
                                $resErr = 17;
                            }else{
                                $result17 = 'File ' . $attach7 . " updated";
                            }
                        } catch (\Exception $e) {
                            Log::error('promo id : '. $promoId);
                            Log::error('file7 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result17 = $e->getMessage();
                        }
                    }

                }

                Log::info('Res Error ' . $resErr);
                if($resErr == 0){
                    $resProgress = 'success';
                    $resSuccesSum += 1;
                }else{
                    $resProgress = 'failed';
                    $resFailedSum += 1;
                }

                $row = array(
                    'PromoNumber' => $PromoNumber,
                    'attach1' => $attach1,
                    'attach2' => $attach2,
                    'attach3' => $attach3,
                    'attach4' => $attach4,
                    'attach5' => $attach5,
                    'attach6' => $attach6,
                    'attach7' => $attach7,
                    'resultPromo'=> $resultPromo,
                    'result1' => $result1,
                    'result2' => $result2,
                    'result3' => $result3,
                    'result4' => $result4,
                    'result5' => $result5,
                    'result6' => $result6,
                    'result7' => $result7,
                    'result11' => $result11,
                    'result12' => $result12,
                    'result13' => $result13,
                    'result14' => $result14,
                    'result15' => $result15,
                    'result16' => $result16,
                    'result17' => $result17,
                    'resultProgress' => $resProgress,
                );
                array_push($data, $row);
            }
            // Delete Folder tmp/uploads
            $path = storage_path('tmp/uploads/' . $request->session()->get('userid'));
            File::deleteDirectory($path);

            Log::info('Promo Upload Attachments ' . json_encode($data));
            $json['error']    = false;
            $json['pesan']     = 'Sukses';
            $json['data']      = $data;
            $json['resSuccesSum']      = $resSuccesSum;
            $json['resFailedSum']      = $resFailedSum;
        }else{
            return array(
                'error'     => true,
                'message'   => 'File PromoUploadAttachments.xlsx doesn`t exist'
            );
        }
        return json_encode($json);
    }

    public function import(Request $request)
    {
        try {
            $filename = "PromoUploadAttachments.xlsx";
            $path = storage_path('tmp/uploads/' . $request->session()->get('userid')). '/';
            $pathFile = $path . $filename;
            if (file_exists($pathFile)) {
                $api = config('app.api') . "/tools/upload/promoattachmentget";
                Log::info('import data');
                Log::info($api);
                $data_multipart =  [
                    'name'     => 'formFile',
                    'contents' => file_get_contents($pathFile),
                    'filename' => $filename
                ];
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', file_get_contents($pathFile), $filename)
                    ->post($api, $data_multipart);
                if ($response->status() === 200) {
                    return json_encode([
                        'error'     => false,
                        'message'   => "Upload success",
                        'data'      => json_decode($response)->values
                    ]);
                } else {
                    return json_encode([
                        'error'     => true,
                        'message'   => "Upload error",
                        'data'      => []
                    ]);
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => $filename . " not found",
                    'data'      => []
                ]);
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error' => true,
                'message' => "Upload error",
            ]);
        }
    }

    public function readDataExcel(Request $request)
    {
        try {
            Log::info('Read Excel');

            $filename = "PromoUploadAttachments.xlsx";
            $path = storage_path('tmp/uploads/' . $request->session()->get('userid')). '/';
            $pathFile = $path . $filename;

            $result = Excel::toArray((object)[], $pathFile);
            $arraySize = count($result[0]);

            $data = [];
            for( $i=1; $i < $arraySize; $i++ ) {
                array_push($data, [
                    'promoNumber' => $result[0][$i][0],
                    'attachment1' => $result[0][$i][1],
                    'attachment2' => $result[0][$i][2],
                    'attachment3' => $result[0][$i][3],
                    'attachment4' => $result[0][$i][4],
                    'attachment5' => $result[0][$i][5],
                    'attachment6' => $result[0][$i][6],
                    'attachment7' => $result[0][$i][7]
                ]);
            }

            return json_encode([
                'error'     => false,
                'message'   => "Success to read excel",
                'data'      => $data
            ]);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'message'   => "Failed to read excel",
            ]);
        }
    }

    public function process(Request $request)
    {
        try {
            $api = config('app.api'). '/tools/search/promo/refid';
            Log::info('Process Promo');
            Log::info($api);
            Log::info(json_encode([
                'refid'  => $request['promoNumber'],
            ]));
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'refid'  => $request['promoNumber'],
            ]);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $promoId = $resVal->id;
                $path = storage_path('tmp/uploads/' . $request->session()->get('userid')). '/';

                $statusProcess = false;
                if ($this->processAttachment($promoId, $path, $request['attachment1'], 'row1', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment2'], 'row2', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment3'], 'row3', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment4'], 'row4', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment5'], 'row5', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment6'], 'row6', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }
                if ($this->processAttachment($promoId, $path, $request['attachment7'], 'row7', $request->session()->get('userid'))) {
                    $statusProcess = true;
                }

                Log::info('Promo ID ' . $promoId . ' status ' . $statusProcess);
                if ($statusProcess) {
                    // call api /api/tools/upload/promoattachment
                    $apiProcess = config('app.api'). '/tools/promo/promoattachment';
                    $responseProcess =  Http::timeout(180)->withToken($this->token)->post($apiProcess, [
                        "promoId"   => $promoId,
                    ]);
                    if ($responseProcess->status() === 200) {
                        $resVal = json_decode($responseProcess)->values[0];
                        if ($resVal->recon === 0) {
                            $this->sendEmailApproverCycle1($resVal->userid_approver, $resVal->username_approver, $resVal->id, $resVal->email_approver, "[APPROVAL NOTIF] Promo requires approval (" . $resVal->refid . ")");
                        } else {
                            $this->sendEmailApproverCycle2($resVal->userid_approver, $resVal->username_approver, $resVal->id, $resVal->email_approver, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $resVal->refid . ")");
                        }
                    } else {
                        Log::info('promo will not send email');
                    }
                }

                File::deleteDirectory($path);

                return json_encode([
                    'error'     => false,
                    'message'   => "Process finish",
                ]);
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error' => true,
                'message' => "Process error",
            ]);
        }
    }

    private function processAttachment($promoId, $path, $fileName, $row, $userid): bool
    {
        if (file_exists($path . $fileName)) {
            Log::info('File ' . $path . $fileName . " exist");
            // move attachment to dir promo id
            $pathPromo = '/assets/media/promo/' . $promoId;

            if (!Storage::disk('optima')->exists($pathPromo)) {
                Storage::disk('optima')->makeDirectory($pathPromo);
            }

            $pathRow = '/assets/media/promo/' . $promoId . '/' . $row;
            if (!Storage::disk('optima')->exists($pathRow)) {
                Storage::disk('optima')->makeDirectory($pathRow);
            }
            $pathDest = $pathRow . "/" . $fileName;
            File::copy($path . $fileName, public_path($pathDest));

            // delete data attachment
            $apiDelete = config('app.api'). '/tools/promoattachment?PromoId=' . $promoId;
            $responseDelete = Http::timeout(180)->withToken($this->token)->delete($apiDelete . '&DocLink=' . $row);
            if ($responseDelete->status() === 200) {
                Log::info('delete attachment ' . $row . ' success');
            } else {
                Log::info('delete attachment ' . $row . ' failed');
            }
            // insert (update) data attachment
            $apiUpdate = config('app.api'). '/tools/promoattachment';
            $responseUpdate =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                "promoId"   => $promoId,
                "docLink"   => $row,
                "fileName"  => $fileName,
                "createOn"  => date('Y-m-d H:m:s'),
                "createBy"  => $userid
            ]);
            if ($responseUpdate->status() === 200) {
                Log::info('insert new attachment ' . $row . ' success');
            } else {
                Log::info('insert new attachment ' . $row . ' failed');
            }

            return true;
        } else {
            return false;
        }
    }

    protected function sendEmailApproverCycle1($userApprover, $nameApprover, $id, $email, $subject)
    {
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/display/email/id';
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
                    'nameApprover'  => $nameApprover,
                    'sy'            => date('Y', strtotime($data->promoHeader->startPromo)),
                ]);
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view('Tools::upload-attachment-promo-admin.new-email-approval', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
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
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }

    protected function sendEmailApproverCycle2($userApprover, $nameApprover, $id, $email, $subject)
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
                    'nameApprover'  => $nameApprover,
                    'sy'            => date('Y', strtotime($data->promoHeader->startPromo)),
                ]);
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view('Tools::upload-attachment-promo-admin.new-email-approval', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
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
