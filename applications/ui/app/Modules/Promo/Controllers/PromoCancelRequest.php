<?php

namespace App\Modules\Promo\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Illuminate\Support\Facades\Storage;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Wording;

class PromoCancelRequest extends Controller
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
        $title = "Promo Cancel Request";
        return view('Promo::promo-cancel-request.index', compact('title'));
    }

    public function form(Request $request)
    {
        $category = MyEncrypt::decrypt($request['c']);
        if($category === 'DC' || $request['promoPlanId']) {
            $title = "Promo Cancel Request (Distributor Cost)";
            if ($request['sy'] >= 2025) {
                if ($request['recon']) {
                    $title = "Promo Cancel Request Reconciliation (Distributor Cost)";
                    return view('Promo::promo-cancel-request.dc-form-recon', compact('title'));
                } else {
                    return view('Promo::promo-cancel-request.dc-form-revamp', compact('title'));
                }
            } else {
                return view('Promo::promo-cancel-request.dc-form', compact('title'));
            }
            return view('Promo::promo-cancel-request.dc-form', compact('title'));
        } else {
            $title = "Promo Cancel Request (Retailer Cost)";
            if ($request['sy'] >= 2025) {
                if ($request['recon']) {
                    $title = "Promo Cancel Request Reconciliation (Retailer Cost)";
                    return view('Promo::promo-cancel-request.form-recon', compact('title'));
                } else {
                    return view('Promo::promo-cancel-request.form-revamp', compact('title'));
                }
            } else {
                return view('Promo::promo-cancel-request.form', compact('title'));
            }
        }
    }

    public function getList(Request $request)
    {
        $api = config('app.api') . '/promo/cancel/request';
        try {
            $query = [
                'year' => $request->period,
                'entity' => ($request->entityId ?? 0),
                'distributor' => ($request->distributorId ?? 0),
                'budgetparent' => ($request->budgetparent ?? 0),
                'channel' => ($request->channel ?? 0),
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
                }
                return json_encode([
                    "data" => $resVal,
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getDataById(Request $request)
    {
        $api = config('app.api') . '/promo/cancel/request/id';
        $query = [
            'id' => $request->promoId,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                return array(
                    'error' => false,
                    'data' => json_decode($response)->values
                );
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getDataV2ById(Request $request): bool|string
    {
        $api = '/promo/displayv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataPromo(Request $request): bool|string
    {
        $api = '/promo/approvalv2/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataPromoRecon(Request $request): bool|string
    {
        $api = '/promo/approvalv2recon/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api') . '/promo/entity';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListDistributor(Request $request)
    {
        $api = config('app.api') . '/promo/distributor';
        try {
            $ar_parent = array();
            array_push($ar_parent, $request->entityId);

            $query = [
                'budgetId' => 0,
                'entityId' => $ar_parent,
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'error' => false,
                    'data' => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function getListSubactivityType(Request $request)
    {
        $api = config('app.api'). '/promo/subcategory/categoryid';
        $query = [
            'CategoryId'  => $request['CategoryId'],
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

    public function previewAttachment(Request $request)
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-cancel-request.attachment-preview", compact('path', 'isExist', 'title'));
    }

    public function approve(Request $request)
    {
        $api = config('app.api') . '/promo/cancel/request/approve';
        $data = [
            'promoId'       => $request->promoId,
            'planningId'    => ($request->promoPlanningId ?? 0),
            'notes'         => $request->notes,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Approval
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $message_email = "This email was sent automatically by Optima System in responses to notif about cancel request promo.<br>Promo Ref ID : " . $request->promoRefId ."<br>Promo Initiator : " . $request->createdBy ."<br><p>The promo cancellation request has been approved by " . $request->session()->get('profile') . "<p><a href='" . config('app.url') . "'>Optima System</a><p>Thank you,<br>Optima System";
                $subject = "Approved Promo Cancel";
                $emailCancel = $this->sendEmailCancel($message_email, $subject, $request->createdBy);
                if (!json_decode($emailCancel)->error) {
                    return array(
                        'error'     => false,
                        'message'   => "Cancel promo success"
                    );
                } else {
                    Log::warning("Cancel promo success but can't send email notification to " . $request->createdBy . " (" . json_decode($emailCancel)->data->emailto .")");
                    return array(
                        'error'     => true,
                        'message'   => "Cancel promo success but can't send email notification to " . $request->createdBy. " (" . json_decode($emailCancel)->data->emailto .")"
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "Cancel Promo Failed"
            );
        }
    }

    public function reject(Request $request)
    {
        $api = config('app.api') . '/promo/cancel/request/sendback';
        $data = [
            'promoId'       => $request->promoId,
            'planningId'    => ($request->promoPlanningId ?? 0),
            'notes'         => $request->notes,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Reject
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $message_email = "This email was sent automatically by Optima System in responses to notif about cancel request promo.<br>Promo Ref ID : " . $request->promoRefId ."<br>Promo Initiator : " . $request->createdBy ."<br><p>The promo cancellation request has been rejected by " . $request->session()->get('profile') . ".<p><a href='" . config('app.url') . "'>Optima System</a><p>Thank you,<br>Optima System";
                $subject = "Rejected Promo Cancel";

                $emailCancel = $this->sendEmailCancel($message_email, $subject, $request->createdBy);
                if (!json_decode($emailCancel)->error) {
                    return array(
                        'error'     => false,
                        'message'   => "Reject promo success"
                    );
                } else {
                    Log::warning("Reject promo success but can't send email notification to " . $request->createdBy . " (" . json_decode($emailCancel)->data->emailto .")");
                    return array(
                        'error'     => true,
                        'message'   => "Reject promo success but can't send email notification to " . $request->createdBy. " (" . json_decode($emailCancel)->data->emailto .")"
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "Reject promo Failed"
            );
        }
    }

    protected function sendEmailCancel($messageEmail, $subject, $createBy): bool|string
    {
        $apiConfig = config('app.api') . '/tools/email/getconfig';
        $queryConfig = [
            'id'        => 'cancel submit',
            'param'     => $createBy
        ];
        Log::info('get API ' . $apiConfig);
        Log::info('payload ' . json_encode($queryConfig));

        $api_email = config('app.api') . '/tools/email';
        try {
            $responseConfig =  Http::timeout(180)->withToken($this->token)->get($apiConfig, $queryConfig);
            if ($responseConfig->status() === 200) {
                $data = json_decode($responseConfig)->values;
                $dataEmailTo = $data[0]->email_to;
                $dataEmailCC = $data[0]->email_cc;
                $emailAddressTo = explode(",",$dataEmailTo);
                $emailAddressCC = explode(",",$dataEmailCC);

                $data = [
                    'email'     => $emailAddressTo,
                    'subject'   => $subject,
                    'body'      => $messageEmail,
                    'cc'        => $emailAddressCC
                ];

                Log::info('post API ' . $api_email);
                Log::info('payload ' . json_encode($data));

                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $data);
                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success");
                    return json_encode([
                        'error'     => false,
                        'data'      => [
                            'emailto'   => $dataEmailTo
                        ],
                        'message'   => "Send Email Success"
                    ]);
                } else {
                    Log::info("error : Send Email Failed");
                    return json_encode([
                        'error'     => true,
                        'data'      => [
                            'emailto'   => $dataEmailTo
                        ],
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

    public function encCategoryShortDesc ($categoryShortDesc)
    {
        try {
            return MyEncrypt::encrypt($categoryShortDesc);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return '';
        }
    }
}
