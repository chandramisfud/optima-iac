<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class BudgetDeployment extends Controller
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
        $title = "Budget Deployment";
        return view('Budget::budget-deployment.index', compact('title'));
    }

    public function getListFilter(): bool|string
    {
        $api = '/budget/deployment/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getList(Request $request): bool|string
    {
        $api = '/budget/deployment/list';
        $query = [
            'period'            => $request['period'],
            'channelId'         => json_decode($request['channelId']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'subActivityTypeId' => json_decode($request['subActivityType'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function processPage(): Factory|View|Application
    {
        return view('Budget::budget-deployment.process');
    }

    public function getListPromo(Request $request): bool|string
    {
        try {
            $api = config('app.api') . '/budget/deployment/batchid';
            $body = [
                'batchId'   => decrypt($request['batchId'])
            ];
            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($body));
            $response = Http::timeout(180)->withToken($this->token)->post($api, $body);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'Budget success deployed',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => "Deploy budget failed"
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "Deploy budget failed"
            ]);
        }
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
                    'nameApprover'  => $nameApprover,
                    'sy'            => date('Y', strtotime($data->promoHeader->startPromo))
                ]);
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view('Budget::budget-deployment.new-email-approval', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

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

    public function deploy(Request $request): bool|string
    {
        try {
            $api = config('app.api') . '/budget/deployment/request';
            $body = [
                'promoid'   => json_decode($request['promoId'])
            ];
            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($body));
            $response = Http::timeout(180)->withToken($this->token)->post($api, $body);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'batchId'   => encrypt(json_decode($response)->values->batchId),
                    'message'   => 'Budget success deployed',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
                Log::warning($message);
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => "Deploy budget failed"
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "Deploy budget failed"
            ]);
        }

    }

    public function downloadExcel(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/budget/deployment/list';
            $query = [
                'period'            => $request['period'],
                'channelId'         => json_decode($request['channelId']),
                'groupBrand'        => json_decode($request['groupBrand']),
                'subActivityTypeId' => json_decode($request['subActivityType'])
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;

                $result=[];
                foreach ($data->budgetDeploymentDetail as $fields) {
                    $arr = [];
                    $arr[] = $fields->promoId;
                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityName;
                    $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                    $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                    $arr[] = $fields->investment;

                    $result[] = $arr;
                }

                $fileName = 'Budget Deployment - ' . date('Y-m-d His') . microtime() . '.xlsx';
                $title = 'A1:L1'; //Report Title Bold and merge
                $header = 'A3:L3'; //Header Column Bold and color
                $heading = [
                    ['Budget Deployment'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Promo ID', 'Brand', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Activity', 'Sub Activity', 'Activity Name', 'Promo Start', 'Promo End', 'Cost']
                ];

                $formatCell =  [
                    'L' => NumberFormat::builtInFormatCode(37),
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                return Excel::download($export, $fileName);
            } else {
                return $res;
            }
        } catch (Exception $e) {
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
