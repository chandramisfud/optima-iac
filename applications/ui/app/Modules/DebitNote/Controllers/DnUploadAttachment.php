<?php

namespace App\Modules\DebitNote\Controllers;

use Maatwebsite\Excel\Facades\Excel;
use Session;
use App\Http\Requests;
use Illuminate\Support\Facades\Http;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\File;
use App\Exports\DebitNote\ExportTemplateDnUploadAttachment;

class DnUploadAttachment extends Controller
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
        return view('DebitNote::dn-upload-attachment.index');
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
        $api = config('app.api'). '/dn/upload-attachment/attachment';
        $query = [
            'period'        => date('Y'),
            'distributor'   => 0,
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
                    $arr[] = $fields->refId;
                    $arr[] = $fields->row1;
                    $arr[] = $fields->row2;
                    $arr[] = $fields->row3;
                    $arr[] = $fields->row4;
                    $arr[] = $fields->row5;
                    $arr[] = $fields->row6;

                    $result[] = $arr;
                }

                $filename = 'DNUploadAttachments';
                $header = 'A1:J1'; //Header Column Bold and color
                $heading = [
                    ['dnnumber', 'attachment1', 'attachment2', 'attachment3', 'attachment4', 'attachment5',  'attachment6']
                ];

                $export = new ExportTemplateDnUploadAttachment($result, $heading, $header);
                return Excel::download($export, $filename .  '.xlsx');
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

    public function importData(Request $request) {
        $filename = "DNUploadAttachments.xlsx";
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
                $DNNumber = $result[0][$i][0];
                $attach1 = $result[0][$i][1];
                $attach2 = $result[0][$i][2];
                $attach3 = $result[0][$i][3];
                $attach4 = $result[0][$i][4];
                $attach5 = $result[0][$i][5];
                $attach6 = $result[0][$i][6];

                $resErr = 0;
                $resultDN = "OK";
                $api = config('app.api'). '/dn/upload-attachment/search/refid';

                try{
                    $dnId = 0;
                    $response = Http::timeout(180)->withToken($this->token)->get($api, [
                        'refId'  => $DNNumber,
                    ]);

                    if($response->status() !== 200){
                        $message = json_decode($response)->message;
                        Log::warning('get API ' . $api);
                        Log::warning($message);
                        $resultDN = $response;
                        $resErr = 1;
                    }else{
                        $resVal = json_decode($response)->values;

                        $dnId = $resVal[0]->id;
                        $resultDN = "OK";
                    }
                } catch (\Exception $e) {
                    Log::error('get API ' . $api);
                    Log::error($e->getMessage());
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => $e->getMessage()
                    );
                    $resultDN = $e->getMessage();
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

                // update data DN file attaxh
//                Log::info('DN : '. $DNNumber);
//                Log::info('attach1 : '. $attach1);
                $resultUpdate = "OK";
                $result11 = '';
                $result12 = '';
                $result13 = '';
                $result14 = '';
                $result15 = '';
                $result16 = '';

                if($resErr == 0){
                    // update file attach1
                    $apiUpdate = config('app.api'). '/dn/upload-attachment';
                    Log::info('file1 : '. $attach1);
                    if($attach1 !='' and $attach1 != null){
                        try{
                            // move file from temp to DN dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row1';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest1 = $path_row . "/" . $attach1;
                            File::copy($pathFile1, $pathDest1);
                            // store data to database
                            $responseUpdate1 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row1',
                                "fileName"  => $attach1,
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file1 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result11 = $e->getMessage();

                        }
                    }
                    // update file attach2
                    Log::info('file2 : '. $attach2);
                    if($attach2 !='' and $attach2 != null){
                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row2';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest2 = $path_row . "/" . $attach2;
                            File::copy($pathFile2, $pathDest2);

                            // store data to database
                            $responseUpdate2 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row2',
                                "fileName"  => $attach2,
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file2 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result12 = $e->getMessage();
                        }
                    }
                    // update file attach3
                    Log::info('file3 : '. $attach3);
                    if($attach3 !='' and $attach3 != null){
                        try{
                            // move file from temp to DN dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row3';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest3 = $path_row . "/" . $attach3;
                            File::copy($pathFile3, $pathDest3);

                            // store data to database
                            $responseUpdate3 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row3',
                                "fileName"  => $attach3
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file3 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result13 = $e->getMessage();
                        }
                    }
                    // update file attach4
                    Log::info('file4 : '. $attach4);
                    if($attach4 !='' and $attach4 != null){
                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row4';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest4 = $path_row . "/" . $attach4;
                            File::copy($pathFile4, $pathDest4);

                            // store data to database
                            $responseUpdate4 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row4',
                                "fileName"  => $attach4,
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file4 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result14 = $e->getMessage();

                        }
                    }
                    // update file attach5
                    Log::info('file5 : '. $attach5);
                    if($attach5 !='' and $attach5 != null){
                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row5';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest5 = $path_row . "/" . $attach5;
                            File::copy($pathFile5, $pathDest5);

                            // store data to database
                            $responseUpdate5 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row5',
                                "fileName"  => $attach5
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file5 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result15 = $e->getMessage();

                        }
                    }
                    // update file attach6
                    Log::info('file6 : '. $attach6);
                    if($attach6 !='' and $attach6 != null){
                        try{
                            // move file from temp to debetnote dest
                            $pathDN = public_path().'/assets/media/debitnote/' . $dnId;
                            if (!File::isDirectory($pathDN)) {
                                File::makeDirectory($pathDN,0777,true);
                            }

                            $path_row = public_path().'/assets/media/debitnote/' . $dnId . '/row6';
                            if (!File::isDirectory($path_row)) {
                                File::makeDirectory($path_row,0777,true);
                            }
                            $pathDest6 = $path_row . "/" . $attach6;
                            File::copy($pathFile6, $pathDest6);

                            // store data to database
                            $responseUpdate6 =  Http::timeout(180)->withToken($this->token)->post($apiUpdate, [
                                "dnId"   => $dnId,
                                "docLink"   => 'row6',
                                "fileName"  => $attach6,
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
                            Log::error('DN id : '. $dnId);
                            Log::error('file6 ' . $apiUpdate);
                            Log::error($e->getMessage());
                            $result16 = $e->getMessage();

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
                    'dnNumber'          => $DNNumber,
                    'attach1'           => $attach1,
                    'attach2'           => $attach2,
                    'attach3'           => $attach3,
                    'attach4'           => $attach4,
                    'attach5'           => $attach5,
                    'attach6'           => $attach6,
                    'resultDN'          => $resultDN,
                    'result1'           => $result1,
                    'result2'           => $result2,
                    'result3'           => $result3,
                    'result4'           => $result4,
                    'result5'           => $result5,
                    'result6'           => $result6,
                    'result11'          => $result11,
                    'result12'          => $result12,
                    'result13'          => $result13,
                    'result14'          => $result14,
                    'result15'          => $result15,
                    'result16'          => $result16,
                    'resultProgress'    => $resProgress,
                );
                array_push($data, $row);
            }
            // Delete Folder tmp/uploads
            $path = storage_path('tmp/uploads/' . $request->session()->get('userid'));
            File::deleteDirectory($path);

            Log::info('DN Upload Attachments ' . json_encode($data));
            $json['error']          = false;
            $json['message']        = 'Sukses';
            $json['data']           = $data;
            $json['resSuccesSum']   = $resSuccesSum;
            $json['resFailedSum']   = $resFailedSum;
        }else{
            return array(
                'error'     => true,
                'message'   => 'File DNUploadAttachments.xlsx doesn`t exist'
            );
        }
        return json_encode($json);
    }
}
