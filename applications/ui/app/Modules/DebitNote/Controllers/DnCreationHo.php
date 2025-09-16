<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use Barryvdh\Snappy\Facades\SnappyPdf;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Exports\Export;
use ZipArchive;
use Session;
use Wording;
use File;

class DnCreationHo extends Controller
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
        return view('DebitNote::dn-creation-ho.index', compact('title'));
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
        return view('DebitNote::dn-creation-ho.form', compact('title'));
    }

    public function dnDisplay()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-creation-ho.dn-display', compact('title'));
    }

    public function uploadAttachPage()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-creation-ho.upload-attach', compact('title'));
    }

    public function cancelPage()
    {
        $title = "Debit Note";
        return view('DebitNote::dn-creation-ho.cancel', compact('title'));
    }

    public function getListSubAccount(Request $request)
    {
        $api = config('app.api'). '/dn/creation/ho/attribute/user';
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
        $api = config('app.api'). '/dn/creation/ho/attribute/user';
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
        $api = config('app.api'). '/dn/creation/ho/attribute/user';
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
        $api = config('app.api'). '/dn/creation/ho/sellingpoint/user';
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

    public function getDataTaxLevelByEntityId(Request $request)
    {
        $api = config('app.api'). '/dn/creation/taxlevel/entityid';
        $query = [
            'entityid'  => $request->entityId,
            'whtType'   => $request->whtType,
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
        $api = config('app.api'). '/dn/creation/ho/id';
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

    public function getListWHTType(): bool|string
    {
        $api = '/mapping/distributor-wht/whttype';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getDataWHTTypeByPromoId(Request $request): bool|string
    {
        $api = '/dn/creation/ho/whttype';
        $query = [
            'promoId'  => $request['promoId']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function save(Request $request)
    {
        $api = config('app.api') . '/dn/creation/ho';

        $arFile = array();
        if($request['file_name_attachment1']!=""){
            $fileAttach = [
                'doclink' => 'row1',
                'filename'  => $request['file_name_attachment1']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment2']!=""){
            $fileAttach = [
                'doclink' => 'row2',
                'filename'  => $request['file_name_attachment2']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment3']!=""){
            $fileAttach = [
                'doclink' => 'row3',
                'filename'  => $request['file_name_attachment3']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment4']!=""){
            $fileAttach = [
                'doclink' => 'row4',
                'filename'  => $request['file_name_attachment4']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment5']!=""){
            $fileAttach = [
                'doclink' => 'row5',
                'filename'  => $request['file_name_attachment5']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment6']!=""){
            $fileAttach = [
                'doclink' => 'row6',
                'filename'  => $request['file_name_attachment6']
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
        if($request->memDocNo == null){ $memDocNo = ""; } else { $memDocNo = $request->memDocNo; }
        if($request->promoId == null){ $promoId = 0; } else { $promoId = $request->promoId; }
        if($request->isDnPromo === "0"){ $isDNPromo = false; } else { $isDNPromo = true; }
        if($request->fpDate == null){ $FPDate = "1901-01-01"; } else { $FPDate = $request->fpDate; }

        $data = [
            'isDNPromo'         => $isDNPromo,
            'id'                => 0,
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
            "statusPPH"         => $request->statusPPH,
            "FPNumber"          => $request->fpNumber,
            "FPDate"            => date('Y-m-d' , strtotime($FPDate)),
            "statusPPN"         => $request->statusPPN,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                if (json_decode($response)->code === 666) {
                    $values = json_decode($response)->values;

                    $dataResult = array(
                        'error'         => true,
                        'status'        => 666,
                        'message'       => 'DN with similar data already exists',
                        'refId'         =>  $values->refId,
                        'dpp'           =>  $values->dpp,
                        'distributor'   =>  $values->distributor,
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
                    if ($request->isDNPromo === true && $request->promoId == 0){
                        $values = json_decode($response)->values;
                        $dataResult = array();

                        //DN Manual -> send email
                        $api_email = config('app.api') . '/tools/email';
                        $message = 'This email was sent automatically by Optima System in responses assign promo to DN. You can login into Optima System using:<br>Username : ' . ($values->approverUserid ?? "") . '<br><p>To login using this user, login into Optima System, either click on the button or copy and paste the follow link<p><a href="' . config('app.url') . '">Optima System</a><br><p>Thank you,<br>Optima System';

                        $dataEmail = [
                            'email'     => ($values->approverEmail ?? ""),
                            'subject'   => 'Optima no-reply [Assign Promo to DN]',
                            'body'      => $message
                        ];
                        Log::info('post API ' . $api_email);
                        Log::info('payload ' . json_encode($dataEmail));
                        $responseEmail =  Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $dataEmail);
                        if ($responseEmail->status() === 200) {
                            $dataResult = array_merge($dataResult, [
                                'error'     => false,
                                'refId'     => $values->refId,
                                'message'   => "Data Save Success"
                            ]);
                        } else {
                            Log::warning($api_email);
                            Log::warning($responseEmail);
                            $messageEmail = "Save Debet Note ID " . ($values->refId ?? "") . " has been successfuly but can't send email notification to " . ($values->approverUserid ?? "") . " - " . ($values->approverUserName ?? "") . " (" . ($values->approverEmail ?? "").")";
                            $dataResult = array_merge($dataResult, [
                                'error'     => true,
                                'message'   => $messageEmail
                            ]);
                        }
                    }

                    $values = json_decode($response)->values;
                    $path_user = public_path().'/assets/media/debitnote/' . $request->session()->get('profile');
                    if (!File::isDirectory($path_user)) {
                        File::makeDirectory($path_user,0777,true);
                    }

                    $pathid = $values->id;
                    $path_id = public_path().'/assets/media/debitnote/' . $pathid;

                    if (File::move($path_user, $path_id)) {
                        Log::info(json_encode([
                            'error'     => false,
                            'data'      => $values,
                            'message'   => "Upload Successfully",
                            'userid'    => $request->session()->get('profile')
                        ]));
                    } else {
                        Log::warning(json_encode([
                            'error'     => true,
                            'message'   => 'Upload Failed',
                            'userid'    => $request->session()->get('profile')
                        ]));
                    }
                    $dataResult = [
                        'refId'     => $values->refId,
                    ];
                    return $dataResult;
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
                'message'   => "error : Save Failed"
            );
        }
    }

    public function update(Request $request)
    {
        $api = config('app.api') . '/dn/creation/ho';

        $arFile = array();
        if($request['file_name_attachment1']!=""){
            $fileAttach = [
                'doclink' => 'row1',
                'filename'  => $request['file_name_attachment1']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment2']!=""){
            $fileAttach = [
                'doclink' => 'row2',
                'filename'  => $request['file_name_attachment2']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment3']!=""){
            $fileAttach = [
                'doclink' => 'row3',
                'filename'  => $request['file_name_attachment3']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment4']!=""){
            $fileAttach = [
                'doclink' => 'row4',
                'filename'  => $request['file_name_attachment4']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment5']!=""){
            $fileAttach = [
                'doclink' => 'row5',
                'filename'  => $request['file_name_attachment5']
            ];
            array_push($arFile, $fileAttach);
        }
        if($request['file_name_attachment6']!=""){
            $fileAttach = [
                'doclink' => 'row6',
                'filename'  => $request['file_name_attachment6']
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
        if($request->isDnPromo === "0"){ $isDNPromo = false; } else { $isDNPromo = true; }
        if($request->fpDate == null){ $FPDate = "1901-01-01"; } else { $FPDate = $request->fpDate; }

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
            'isDNPromo'         => $isDNPromo,
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
        Log::info('put API ' . $api);
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
                        'distributor'   =>  $values->distributor,
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
        $api = config('app.api'). '/dn/creation/ho/dnattachment';
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

    public function deleteFile(Request $request) {
        $api = config('app.api'). '/dn/creation/ho/dnattachment';
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
        $api = config('app.api') . '/dn/creation/ho/cancel';
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
        $api = config('app.api'). '/dn/creation/ho/print';
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/dn/creation';
        $query = [
            'Period'                    => $request->period,
            'EntityId'                  => 0,
            'DistributorId'             => 0,
            'ChannelId'                 => 0,
            'AccountId'                 => $request->subAccountId ?? 0,
            'ClosingDate'               => $request->closingDate,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'refId',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];

                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->refId;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->dpp;
                    $arr[] = $fields->ppnAmt;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->salesValidationStatus;

                    $salesValidationStatusOn='';
                    if ((date('Y-m-d' , strtotime($fields->salesValidationStatusOn))) == '0001-01-01'){
                        $salesValidationStatusOn = '';
                    } else {
                        $salesValidationStatusOn = date('Y-m-d H:i:s' , strtotime($fields->salesValidationStatusOn));
                    };
                    $arr[] = $salesValidationStatusOn;

                    $arr[] = $fields->remarkSales;
                    $arr[] = $fields->notes;

                    $validateByFinance = '';
                    if ((date('Y-m-d' , strtotime($fields->validate_by_finance_on))) == '0001-01-01'){
                        $validateByFinance = '';
                    } else {
                        $validateByFinance = date('Y-m-d H:i:s' , strtotime($fields->validate_by_finance_on));
                    };
                    $arr[] = $validateByFinance;

                    $validateBySales = '';
                    if ((date('Y-m-d' , strtotime($fields->validate_by_sales_on))) == '0001-01-01'){
                        $validateBySales = '';
                    } else {
                        $validateBySales = date('Y-m-d H:i:s' , strtotime($fields->validate_by_sales_on));
                    };
                    $arr[] = $validateBySales;

                    $confirmPaid = '';
                    if ((date('Y-m-d' , strtotime($fields->confirm_paid_on))) == '0001-01-01'){
                       $confirmPaid = '';
                    } else {
                        $confirmPaid = date('Y-m-d H:i:s' , strtotime($fields->confirm_paid_on));
                    };

                    $arr[] = $confirmPaid;
                    $arr[] = $fields->subAccount;


                    $result[] = $arr;
                }

                $heading = [
                    ['Debet Note'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['DN Number', 'Promo ID', 'Activity', 'DPP', 'VAT', 'Last Status', 'Sales Validation Status', 'Sales Validation Last Status', 'Remark by Sales', 'Rejection Remarks', 'Validated by Finance On', 'Validated by Sales On', 'Payment Date', 'Sub Account' ]
                ];
                $formatCell =  [
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'E' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $title = 'A1:N1'; //Report Title Bold and merge
                $header = 'A3:N3'; //Header Column Bold and color
                $filename = 'DebetNote-';
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
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
