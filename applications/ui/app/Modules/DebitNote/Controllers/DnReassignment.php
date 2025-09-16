<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use ZipArchive;
use Wording;
use File;

class DnReassignment extends Controller
{
    private mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "Debit Note [Reassignment]";
        return view('DebitNote::dn-reassignment.index', compact('title'));
    }

    public function formPage()
    {
        $title = "Debit Note [Reassignment]";
        return view('DebitNote::dn-reassignment.form', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = '/dn/reassignment';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'Search'                    => ($request['search']['value'] ?? ""),
            'SortColumn'                => $request['columns'][$request['order'][0]['column']]['data'],
            'SortDirection'             => $request['order'][0]['dir'],
            'PageSize'                  => $request['length'],
            'PageNumber'                => $page,
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->data;
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => $resVal,
                "recordsTotal" => json_decode($res)->data->recordsTotal,
                "recordsFiltered" => json_decode($res)->data->recordsFiltered
            ]);
        } else {
            return $res;
        }
    }

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/reassignment/id';
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
        $api = config('app.api'). '/dn/reassingment/promo-approval';
        $query = [
            'periode'       => $request->period,
            'entityId'      => $request->entityId,
            'channelId'     => $request->channelId,
            'accountId'     => $request->accountId,

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

    public function downloadZip(Request $request) {
        $api = config('app.api'). '/dn/creation/ho/id';
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

    public function save(Request $request) {
        $api = config('app.api') . '/dn/reassignment/assign';
        $data = [
            'dnId'        => $request->dnId,
            'promoId'     => ($request->promoId ?? 0),
            'userId'      => $request->session()->get('profile'),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'DN Assignment Success',
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

}
