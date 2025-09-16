<?php

namespace App\Modules\Report\Controllers;

use Session;
use File;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Barryvdh\Snappy\Facades\SnappyPdf;
use Wording;
use ZipArchive;

class DNDisplay extends Controller
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
        $title = "Debit Note";
        return view('Report::dn-display.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay';
        try {
            $page = 0;
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Period'                    => $request->period,
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'ChannelId'                 => 0,
                'AccountId'                 => 0,
                'IsDNManual'                => 0,
                'Search'                    => $search,
                'PageNumber'                => $page,
                'PageSize'                  => (int) $length,
                'SortColumn'                => $SortColumn,
                'SortDirection'             => $sort
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "draw" => (int) $request->draw,
                    "data" => json_decode($response)->values->data,
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

    public function formPage()
    {
        $title = "Debit Note";
        return view('Report::dn-display.form', compact('title'));
    }

    public function getDataDNById(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay/id';
        $query = [
            'id'  => $request->id,
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay/entity';
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

    public function getListDistributorByEntityId(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay/distributor';
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

    public function getListSellingPoint(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay/sellingpoint';
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

    public function getListTaxLevel(Request $request)
    {
        $api = config('app.api'). '/report/dndisplay/taxlevel';
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

    public function downloadZip(Request $request) {
        $api = config('app.api'). '/report/dndisplay/id';
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
                    if(count($data->dnAttachment)>0){
                        if (true === ($zip->open($path . '/' . $filename_zip, ZipArchive::CREATE | ZipArchive::OVERWRITE))) {
                            for ($i = 0; $i < count($data->dnAttachment); $i++)  {
                                if($data->dnAttachment[$i]->docLink=='row1')
                                    if(file_exists($path . '/row1/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row1/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row2')
                                    if(file_exists($path . '/row2/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row2/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row3')
                                    if(file_exists($path . '/row3/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row3/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row4')
                                    if(file_exists($path . '/row4/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row4/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row5')
                                    if(file_exists($path . '/row5/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row5/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row6')
                                    if(file_exists($path . '/row6/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row6/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row7')
                                    if(file_exists($path . '/row7/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row7/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row8')
                                    if(file_exists($path . '/row8/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row8/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
                                    }
                                if($data->dnAttachment[$i]->docLink=='row9')
                                    if(file_exists($path . '/row9/' . $data->dnAttachment[$i]->fileName)){
                                        $zip->addFile(public_path($path . '/row9/' . $data->dnAttachment[$i]->fileName), $data->dnAttachment[$i]->fileName);
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

    public function printPdf(Request $request) {
        $api = config('app.api'). '/report/dndisplay/print';
        $query = [
            'id'  => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $pdf = SnappyPdf::loadView('Report::dn-display.printout-pdf-dn', compact('data'))
                    ->setOption('page-size', 'letter')
                    ->inline('DN_'.date('Y-m-d_H-i-s').'.pdf');
                return $pdf;
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
}
