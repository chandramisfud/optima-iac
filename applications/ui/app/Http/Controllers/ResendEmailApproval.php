<?php

namespace App\Http\Controllers;

use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class ResendEmailApproval extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage(): Factory|View|Application
    {
        $title = "Resend Email Approval";
        return view('resend-email-approval.index', compact('title'));
    }

    public function getList(Request $request): bool|string
    {
        $api = config('app.api') . '/tools/xva/resend_email_approval';
        Log::info('GET ' . $api);
        try {
            $response = Http::withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;

                if ($request['cycle'] === '1') {
                    $cycle1 = $resVal->dataCycle1;
                    return json_encode([
                        "data" => $cycle1
                    ]);
                } else {
                    $cycle2 = $resVal->dataCycle2;
                    return json_encode([
                        "data" => $cycle2
                    ]);
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function confirmKey(Request $request): bool
    {
        $key = (string) config('app.key_resend_email_approval');
        $req_key = $request['key'];
        if ($req_key == $key) {
            return true;
        } else {
            return false;
        }
    }

    public function sendEmailCycle1(Request $request): bool|string
    {
        Log::info('RESEND EMAIL APPROVAL CYCLE 1 by ' . ($request->session()->get('email') ?? ""));
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";

        $yearPromo = $request['period'];
        $promoId = $request['promoId'];
        $userApprover = $request['UserApprover'];
        $nameApprover = $request['UserApproverName'];

        if ($yearPromo >= 2025) {
            $api_promo = config('app.api') . '/promo/display/email/id';
        } else {
            $api_promo = config('app.api') . '/promo/display/email/id';
        }

        $query_promo = [
            'id'           => $promoId,
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
                if ($yearPromo >= 2025){
                    $viewEmail = 'resend-email-approval.new-email-approval';

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
                    $refId = $data->promoHeader->refId;

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                } else {
                    $viewEmail = 'resend-email-approval.email-approval';

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
                    $refId = $data->promoHeader->refId;

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                }
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view($viewEmail, compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $request['email'],
                    'subject'   => '[APPROVAL NOTIF] Promo requires approval (' . $refId . ')',
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
                Log::info('payload ' . json_encode($data));

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
                Log::warning('Get Data Promo '. $request['promoId'] .' Failed');
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

    public function sendEmailCycle2(Request $request): bool|string
    {
        Log::info('RESEND EMAIL APPROVAL CYCLE 2 by ' . ($request->session()->get('email') ?? ""));
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";

        $yearPromo = $request['period'];
        $promoId = $request['promoId'];
        $userApprover = $request['UserApprover'];
        $nameApprover = $request['UserApproverName'];

        if ($yearPromo >= 2025) {
            $api_promo = config('app.api') . '/promo/display/email/id';
        } else {
            $api_promo = config('app.api') . '/promo/display/email/id';
        }

        $query_promo = [
            'id'           => $request['promoId'],
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
                if ($yearPromo >= 2025){
                    $viewEmail = 'resend-email-approval.email-approval-recon';

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
                    $refId = $data->promoHeader->refId;

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                } else {
                    $viewEmail = 'resend-email-approval.email-approval-recon';

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
                    $refId = $data->promoHeader->refId;

                    $dataUrl = json_encode([
                        'promoId'       => $promoId,
                        'refId'         => $data->promoHeader->refId,
                        'profileId'     => $userApprover,
                        'nameApprover'  => $nameApprover,
                        'sy'            => $yearPromo,
                    ]);
                }
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view($viewEmail, compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $request['email'],
                    'subject'   => '[APPROVAL NOTIF] Promo Reconciliation requires approval (' . $refId . ')',
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
                Log::info('payload ' . json_encode($data));

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
                Log::warning('Get Data Promo '. $request['promoId'] .' Failed');
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
