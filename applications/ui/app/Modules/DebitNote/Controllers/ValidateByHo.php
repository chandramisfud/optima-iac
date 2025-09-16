<?php

namespace App\Modules\DebitNote\Controllers;

use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Wording;

class ValidateByHo extends Controller
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
        $title = "Debit Note [Validate By HO]";
        return view('DebitNote::validate-by-ho.index', compact('title'));
    }

    public function approvalPage()
    {
        $title = "Debit Note [Validate By HO]";
        return view('DebitNote::validate-by-ho.approval', compact('title'));
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/dn/received-approved-byho/validate-bydistributor-ho';
        $query = [
            'entityid'          => 0,
            'distributorid'     => (int) $this->getDistributorId($request->session()->get('profile')),

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

    public function getDistributorId($id)
    {
        $api = config('app.api'). '/dn/listing-promo-distributor/userprofile/id';
        $query = [
            'id'    => $id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return json_decode($response)->values->distributorlist[0]->distributorId;
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
        $api = config('app.api'). '/dn/received-approved-byho/id';
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

    public function approved(Request $request) {
        $api = config('app.api') . '/dn/received-approved-byho/changestatus/distributor-multiapproval';
        $data = [
            'userId'            => $request->session()->get('profile'),
            'status'            => 'validate_by_dist_ho',
            'dnid'              => json_decode($request->dnid),
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Validate by HO success'
                ]);
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
        $api = config('app.api') . '/dn/received-approved-byho/multi-approval';
        $data = [
            "dnId"          => $request->dnid,
            "status"        => $request->approvalStatusCode,
            "notes"         => $request->notes,
        ];
        Log::info('api ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'DN Validated'
                ]);
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

    public function uploadFile(Request $request) {
        try {
            if($request->mode=='edit' || $request->mode=='uploadattach' || $request->mode=='approve'){
                $FileId = $request->dnId;
                $dnId = $request->dnId;
            }else{
                $FileId = $request->session()->get('profile');
                $dnId = 0;
            }

            $file = $request->file('file');
            $path = public_path().'/assets/media/debitnote/' . $FileId;
            if (!File::isDirectory($path)) {
                File::makeDirectory($path,0777,true);
            }

            $path_row = public_path().'/assets/media/debitnote/' . $FileId . '/' . $request->row;
            if (!File::isDirectory($path_row)) {
                File::makeDirectory($path_row,0777,true);
            }

            $file_name = $file->getClientOriginalName();

            if ($file->move($path_row, $file_name)) {
                if($request->dnId === '0'){
                    return json_encode([
                        'error'     => false,
                        'message'   => "Upload Successfuly",
                        'userid'    => $request->session()->get('profile'),

                    ]);
                }else{
                    $api = config('app.api'). '/dn/creation/dnattachment';
                    // ip_dn_attachment_store
                    $data = [
                        'dnId'    => (int) $dnId,
                        'docLink' => $request->row,
                        'fileName'=> $file_name,
                        'createBy'=> $request->session()->get('profile')
                    ];
                    $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
                    if ($response->status() === 200) {
                        return json_encode([
                            'error'     => false,
                            'userid'    => $request->session()->get('profile'),
                            'message'   => "Upload Successfuly",
                        ]);
                    } else {
                        $message = json_decode($response)->message;
                        Log::warning($message);
                        return array(
                            'error'     => true,
                            'message'   => "Upload Failed",
                        );
                    }
                }
            }else{
                return array(
                    'error'     => true,
                    'message'   => "Upload Failed",
                    'userid'    => $request->session()->get('profile'),
                );
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

    public function deleteFile(Request $request) {
        try {
            if($request->mode=='edit' || $request->mode=='uploadattach'){
                $FileId = $request->dnId;
            }else{
                $FileId = $request->session()->get('profile');
            }
            $file = $request->file('file');
            if($file==null){
                $file_name = $request->fileName;
            }else{
                $file_name = $file->getClientOriginalName();
            }

            if($request->mode=='edit' || $request->mode=='uploadattach') {
                $api = config('app.api'). '/dn/creation/dnattachment';
                $query = [
                    'DNId'    => (int) $FileId,
                    'DocLink' => $request->row,
                    'FileName'=> $file_name,
                ];
                Log::info('post API ' . $api);
                Log::info('payload ' . json_encode($query));
                $response =  Http::timeout(180)->withToken($this->token)->delete($api, $query);
                if ($response->status() === 200) {
                    return json_encode([
                        'error'     => false,
                        'userid'    => $request->session()->get('profile'),
                        'message'   => "Delete File Successfuly",
                    ]);
                } else {
                    $message = json_decode($response)->message;
                    Log::warning($message);
                    return array(
                        'error'     => true,
                        'message'   => "Delete File Failed, File Not Found",
                    );
                }
            } else {
                return json_encode([
                    'error'     => false,
                    'message'   => "Delete File Successfuly",
                ]);
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Delete File Failed"
            );
        }
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/dn/received-approved-byho/filter';
        Log::info('post API ' . $api);

        $data_multipart = [];
        array_push($data_multipart, [
            'name'  => 'status',
            'contents' => 'send_to_dist_ho'
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
