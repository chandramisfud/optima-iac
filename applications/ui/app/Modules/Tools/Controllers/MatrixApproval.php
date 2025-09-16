<?php

namespace App\Modules\Tools\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use App\Http\Controllers\Controller;
use Exception;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class MatrixApproval extends Controller
{
    protected mixed $token;

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage()
    {
        return view('Tools::matrix-approval.index');
    }

    public function processPage(Request $request)
    {
        return view('Tools::matrix-approval.process');
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/tools/upload/matrixapproval';
        Log::info('post API ' . $api);

        $data_multipart = [];
        array_push($data_multipart, [
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
                    if (isset(json_decode($response)->values)) {
                        $resVal =  json_decode($response)->values[0];
                        return json_encode([
                            'error' => false,
                            'message' => "Upload success",
                            'processId' => encrypt($resVal->processId)
                        ]);
                    } else {
                        return json_encode([
                            'error' => false,
                            'message' => "Upload success",
                            'processId' => 0
                        ]);
                    }
                } else {
                    $message = json_decode($response)->message;
                    Log::error($message);
                    return array(
                        'error' => true,
                        'message' => "Upload failed"
                    );
                }
            } else {
                return array(
                    'error' => true,
                    'message' => "File doesn't exist"
                );
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }

    public function getListMatrix(Request $request): bool|string
    {
        $api = '/tools/upload/matrixapprovalbyprocess';
        $query = [
            'processId'  => decrypt($request['processId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListPromo(Request $request): bool|string
    {
        $api = '/tools/upload/matrixapprovalpromobymatrix';
        $query = [
            'matrixId'  => $request['matrixId']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function sendEmail(Request $request): bool|string
    {
        try {
            $recon = 0;
            $title = "Promo Display";
            $subtitle = "Promo Id ";
            $promoId = $request['promoId'];
            $userApprover = $request['profileApprover'];
            $nameApprover = $request['nameApprover'];
            $email = $request['emailApprover'];
            $subject = "[APPROVAL NOTIF] Promo requires approval (" . $request['refId'] . ")";
            $api_promo = config('app.api') . '/promo/display/email/id';
            if ($request['cycle'] == 2) {
                $api_promo = config('app.api') . '/promo/display/id';
                $subject = "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $request['refId'] . ")";
            }

            $query_promo = [
                'id'           => $promoId,
            ];
            Log::info('get API ' . $api_promo);
            Log::info('payload ' . json_encode($query_promo));

            $api_email = config('app.api') . '/tools/email';


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
                    'nameApprover'  => $nameApprover
                ]);
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view('Tools::matrix-approval.email-approval-cycle1', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();
                if ($request['cycle'] == 2) {
                    $message = view('Tools::matrix-approval.email-approval-cycle2', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                        ->toHtml();
                }

                $data = [
                    'email'     => $email,
                    'subject'   => $subject,
                    'body'      => $message
                ];

                Log::info('post API ' . $api_email);
                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $data);
                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success " . $email);
                    return json_encode([
                        'error'     => false,
                        'data'      => [],
                        'message'   => "Send Email Success " . $email
                    ]);
                } else {
                    Log::info("error : Send Email Failed " . $email);
                    return json_encode([
                        'error'     => true,
                        'data'      => [],
                        'message'   => "error : Send Email Failed " . $email
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
