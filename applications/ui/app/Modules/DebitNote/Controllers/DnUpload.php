<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use Barryvdh\Snappy\Facades\SnappyPdf;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use ZipArchive;
use Session;
use Wording;
use File;

class DnUpload extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage(Request $request)
    {
        $title = "Debit Note";
        return view('DebitNote::dn-upload.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/dn/creation';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $page = $page;
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Period'                    => $request->period,
                'EntityId'                  => 0,
                'DistributorId'             => 0,
                'ChannelId'                 => 0,
                'AccountId'                 => $request->subAccountId,
                'ClosingDate'               => $request->closingDate,
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
        return view('DebitNote::dn-upload.form', compact('title'));
    }

    public function uploadPage()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-upload.upload-xls', compact('title'));
    }

    public function dnDisplay()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-upload.dn-display', compact('title'));
    }

    public function uploadAttachPage()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-upload.upload-attach', compact('title'));
    }

    public function cancelPage()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-upload.cancel', compact('title'));
    }

    public function getListSubAccount(Request $request)
    {
        $api = config('app.api'). '/dn/creation/attribute/user';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->subAccount
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
        $api = config('app.api'). '/dn/creation/attribute/user';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->entity
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api'). '/dn/creation/attribute/user';
        Log::info('Get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->channel
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
        $api = config('app.api'). '/dn/creation/taxlevel';
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

    public function getListSellingPoint(Request $request)
    {
        $api = config('app.api'). '/dn/creation/sellingpoint/user';
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

    public function getDataTaxLevelByEntityId(Request $request)
    {
        $api = config('app.api'). '/dn/creation/taxlevel/entityid';
        $query = [
            'entityid'                    => $request->entityId,
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

    public function getDataPromo(Request $request)
    {
        $api = config('app.api'). '/dn/creation/approvedpromo-for-dn';
        $query = [
            'periode'       => $request->period,
            'entity'        => (int) $request->entityId,
            'channel'       => (int) $request->channelId,
            'account'       => (int) $request->subAccountId,
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

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/creation/id';
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

    public function getDataWHTTypeByPromoId(Request $request): bool|string
    {
        $api = '/dn/creation/whttype';
        $query = [
            'promoId'  => $request['promoId']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
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
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return json_decode($response)->values->distributorlist[0]->distributorId;
            } else {
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

    public function getDataDistributorId(Request $request)
    {
        $api = config('app.api'). '/dn/listing-promo-distributor/userprofile/id';
        $query = [
            'id'    => $request->session()->get('profile'),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                if($request->ajax()){
                    return array(
                        'error'     => false,
                        'data'      => json_decode($response)->values->distributorlist[0]->distributorId,
                        'message'   => $message
                    );
                } else {
                    return json_decode($response)->values->distributorlist[0]->distributorId;
                }
//                return json_decode($response)->values->distributorlist[0]->distributorId;

            } else {
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/dn/creation';

        $attachment1 = $request->file('attachment1');
        $attachment2 = $request->file('attachment2');
        $attachment3 = $request->file('attachment3');
        $attachment4 = $request->file('attachment4');
        $attachment5 = $request->file('attachment5');
        $attachment6 = $request->file('attachment6');

        $arFile = array();
        if($attachment1!=""){
            $fileAttach = [
                'doclink' => 'row1',
                'filename'  => $attachment1->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }
        if($attachment2!=""){
            $fileAttach = [
                'doclink' => 'row2',
                'filename'  => $attachment2->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }
        if($attachment3!=""){
            $fileAttach = [
                'doclink' => 'row3',
                'filename'  => $attachment3->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }
        if($attachment4!=""){
            $fileAttach = [
                'doclink' => 'row4',
                'filename'  => $attachment4->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }
        if($attachment5!=""){
            $fileAttach = [
                'doclink' => 'row5',
                'filename'  => $attachment5->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }
        if($attachment6!=""){
            $fileAttach = [
                'doclink' => 'row6',
                'filename'  => $attachment6->getClientOriginalName()
            ];
            array_push($arFile, $fileAttach);
        }

        $sellpoints = json_decode($request->sellpoint, TRUE);
        $ar_sellpoint = array();
        for ($i = 0; $i < count($sellpoints); $i++)  {
            $sellpoint['flag']      = true;
            $sellpoint['sellpoint'] = $sellpoints[$i];
            $sellpoint['longDesc']  = "";
            array_push($ar_sellpoint, $sellpoint);
        }

        $intDocNo = ($request->intDocNo ?? "");
        $memDocNo = ($request->memDocNo ?? "");
        $promoId = ($request->promoId ?? 0);
        $FPDate = ($request->fpDate ?? "1901-01-01");

        $statusPPN = $request['statusPPN'];
        switch ($request['statusPPN']) {
            case 'PPN - DN Amount':
                $statusPPN = 'PPN DN AMOUNT';
                break;
            case 'PPN - Fee':
                $statusPPN = 'PPN FEE';
                break;
            case 'PPN - DPP':
                $statusPPN = 'PPN DPP';
                break;
        }

        $statusPPH = $request['statusPPH'];
        switch ($request['statusPPH']) {
            case 'PPH - DN Amount':
                $statusPPH = 'PPN DN AMOUNT';
                break;
            case 'PPH - Fee':
                $statusPPH = 'FEE PPH';
                break;
            case 'PPH - DPP':
                $statusPPH = 'DPP PPH';
                break;
        }

        $data = [
            'isDNPromo'         => true,
            'id'                => $request->id,
            'periode'           => $request->period,
            'entityId'          => (int) $request->entity,
            'distributorId'     => (int) $this->getDistributorId($request->session()->get('profile')),
            'activityDesc'      => $request->activityDesc,
            'accountId'         => $request->accountId ?? 0,
            'promoId'           => $promoId ?? 0,
            'intDocNo'          => $intDocNo,
            'memDocNo'          => $memDocNo,
            'DNAmount'          => (float) str_replace(',', '', $request->dnAmount),
            'FeeDesc'           => $request->feeDesc,
            'FeePct'            => (float) str_replace(',', '', $request->feePct),
            'FeeAmount'         => (float) str_replace(',', '', $request->feeAmount),
            'dpp'               => (float) str_replace(',', '', $request->dpp),
            'ppnPct'            => (float) str_replace(',', '', $request->ppnPct),
            'deductionDate'     => $request->deductionDate,
            "userId"            => $request->session()->get('profile'),
            "sellpoint"         => $ar_sellpoint,
            "dnattachment"      => $arFile,
            "taxLevel"          => $request->taxLevel,
            "whtType"           => $request['whtType'],
            "pphPct"            => (float) str_replace(',', '', $request->pphPct),
            "pphAmt"            => (float) str_replace(',', '', $request->pphAmount),
            "statusPPH"         => $statusPPH,
            "FPNumber"          => $request->fpNumber,
            "FPDate"            => date('Y-m-d' , strtotime($FPDate)),
            "statusPPN"         => $statusPPN,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                if (json_decode($response)->code === 666) {
                    $values = json_decode($response)->values;

                    $dataResult = array(
                        'error'         => true,
                        'status'        => 666,
                        'message'       => 'DN with similar data already exists',
                        'refId'         =>  $values->refId,
                        'dpp'           =>  $values->dpp,
                        'distributor'   =>  $values->refId,
                        'entity'        =>  $values->entity,
                    );
                    if ($request->activityDesc === $values->activityDesc) {
                        $dataResult = array_merge($dataResult, [
                            'activitdesc'  => $values->activityDesc
                        ]);
                    } else {
                        $dataResult = array_merge($dataResult, [
                            'activitdesc'  => "none"
                        ]);
                    }

                    if ($intDocNo === $values->intdocno) {
                        $dataResult = array_merge($dataResult, [
                            'intdocno'  => $values->intdocno
                        ]);
                    } else {
                        $dataResult = array_merge($dataResult, [
                            'intdocno'  => "none"
                        ]);
                    }

                    return $dataResult;
                } else if (json_decode($response)->code === 500) {
                    $message = json_decode($response)->message;
                    return array(
                        'error'     => true,
                        'message'   => $message,
                    );
                } else {
                    $values = json_decode($response)->values;
                    return array(
                        'error'     => false,
                        'refId'     => $values->refId,
                        'message'   => "Update Success"
                    );
                }
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error'     => true,
                    'message'   => $message,
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Update Failed"
            );
        }
    }

    public function uploadFile(Request $request) {
        $api = config('app.api'). '/dn/creation/dnattachment';
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

    public function uploadXls(Request $request) {
        $api = config('app.api'). '/dn/upload';
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

    public function deleteFile(Request $request) {
        $api = config('app.api'). '/dn/creation/dnattachment';
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

    public function cancel(Request $request) {
        $api = config('app.api') . '/dn/creation/cancel';
        $data = [
            'dnid'              => $request->dnid,
            'reason'            => $request->notes,
            'userid'            => $request->session()->get('profile')
        ];

        Log::info($api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Cancel DN Success'
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

    public function printPdf(Request $request) {
        $api = config('app.api'). '/dn/creation/print';
        $query = [
            'id'  => $request->id,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $pdf = SnappyPdf::loadView('DebitNote::dn-creation.printout-pdf-dn', compact('data'))
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
}
