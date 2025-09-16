<?php

namespace App\Modules\Promo\Controllers;

use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use App\Http\Controllers\Controller;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;

class PromoApprovalRecon extends Controller
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
        $title = "Promo Approval Reconcile";
        return view('Promo::promo-approval-recon.index', compact('title'));
    }

    public function formApprove(Request $request): Factory|View|Application
    {
        $category = MyEncrypt::decrypt($request['c']);
        if($category === 'DC') {
            $title = "Promo Approval (Distributor Cost)";
            if ($request['sy'] >= 2025) {
                return view('Promo::promo-approval-recon.dc-form-revamp', compact('title'));
            } else {
                return view('Promo::promo-approval-recon.dc-form', compact('title'));
            }
        } else {
            $title = "Promo Approval (Retailer Cost)";
            if ($request['sy'] >= 2025) {
                return view('Promo::promo-approval-recon.form-revamp', compact('title'));
            } else {
                return view('Promo::promo-approval-recon.form', compact('title'));
            }
        }
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/promo/approvalrecon';
        try {
            $query = [
                'PageNumber'    => 0,
                'PageSize'      => -1,
                'Search'        => ($request['search']['value'] ?? ""),
                'category'      => $request['categoryId'],
                'entity'        => $request['entityId'],
                'distributor'   => $request['distributorId'],
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                foreach ($resVal as $data) {
                    $data->categoryShortDesc = urlencode($this->encCategoryShortDesc($data->categoryShortDesc));
                }

                return json_encode([
                    "data" => $resVal
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

    public function getListCategory(Request $request)
    {
        $api = config('app.api') . '/promo/approvalrecon/category';
        try {
            Log::info('get API ' . $api);
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/promo/approvalrecon/entity';
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

    public function getListDistributor(Request $request)
    {
        $api = config('app.api'). '/promo/approvalrecon/distributor';
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

    public function getPromoById(Request $request)
    {
        $api = config('app.api'). '/promo/approvalrecon/id';
        $query = [
            'id'           => $request->id,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data'  => $resVal
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
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

    public function getDataPromo(Request $request): bool|string
    {
        $api = '/promo/approvalv2recon/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getDataPromoDC(Request $request): bool|string
    {
        $api = '/promo/approvalv2dcrecon/id';
        $callApi = new CallApi();
        $query = [
            'id'            => $request['id'],
        ];
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListDNClaimed(Request $request): bool|string
    {
        $api = '/promo/approvalrecon/dn/claim/id';
        $query = [
            'id'  => $request['id']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListDNPaid(Request $request): bool|string
    {
        $api = '/promo/approvalrecon/dn/paid/id';
        $query = [
            'id'  => $request['id']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function dnPage(): Factory|View|Application
    {
        $title = "Promo Approval Reconcile | DN Information";
        return view('Promo::promo-approval-recon.form-dn', compact('title'));
    }

    public function getDataDN(Request $request): bool|string
    {
        $api = '/promo/approvalrecon/dn/id';
        $query = [
            'id'  => $request['id']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListTaxLevel(Request $request): bool|string
    {
        $api = '/promo/approvalrecon/dn/taxlevel';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function approve(Request $request)
    {
        $api = config('app.api') . '/promo/approvalrecon/approve';
        $api_email = config('app.api') . '/tools/email';
        $promoSKPHeader = [
            "skpDraftAvail"         => (($request->skpDraftAvail) ? false : true),
            "skpDraftAvailBfrAct60" => (($request->skpDraftAvailBfrAct60) ? false : true),
            "periodMatch"           => (($request->periodMatch) ? false : true),
            "investmentMatch"       => (($request->investmentMatch) ? false : true),
            "mechanismMatch"        => (($request->mechanismMatch) ? false : true),
            "skpSign7"              => (($request->skpSign7) ? false : true),
            "entityDraft"           => (($request->entityDraft) ? false : true),
            "brandDraft"            => (($request->brandDraft) ? false : true),
            "periodDraft"           => (($request->periodDraft) ? false : true),
            "activityDescDraft"     => (($request->activityDescDraft) ? false : true),
            "mechanismDraft"        => (($request->mechanismDraft) ? false : true),
            "investmentDraft"       => (($request->investmentDraft) ? false : true),
            "entity"                => (($request->entity) ? false : true),
            "brand"                 => (($request->brand) ? false : true),
            "activityDesc"          => (($request->activityDesc) ? false : true),
            "distributorDraft"      => (($request->distributorDraft) ? false : true),
            "distributor"           => (($request->distributor) ? false : true),
            "channelDraft"          => (($request->channelDraft) ? false : true),
            "channel"               => (($request->channel) ? false : true),
            "storeNameDraft"        => (($request->storeNameDraft) ? false : true),
            "storeName"             => (($request->storeName) ? false : true),
            "skpstatus"             => (($request->skpstatus) ? 0 : 1),
            "skp_notes"             => "",
        ];
        $data = [
            'promoSKPHeader'=> $promoSKPHeader,
            'promoId'       => $request->promoId,
            'notes'         => "",
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Approval
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;
                if($values->isFullyApprovedRecon){
                    // send email to initiator
                    $subject = "Promo Reconciliation fully approved (". $values->refId .")";
                    $emailInitiator = [
                        'email'     => $values->email_initiator,
                        'subject'   => $subject,
                        'body'   => "This email was sent automatically by Optima System in responses to approve promo.<br><p><p>Promo Number : " . $values->refId ."<br>Approved By : " . $request->session()->get('profile') ."<br><p><p>Thank you,<br>Optima System"
                    ];

                    $responseEmailInitiator = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $emailInitiator);
                    if ($responseEmailInitiator->status() === 200) {
                        Log::info("Send Email Success");
                        return json_encode([
                            'error'     => false,
                            'message'   => "Promo Approved Success"
                        ]);
                    } else {
                        Log::warning("Promo approved success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_Initiator.")");
                        return array(
                            'error'     => true,
                            'message'   => "Promo approved success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_Initiator.")"
                        );
                    }
                } else {
                    // send email to other approver
                    $emailApprover = $this->sendEmailApprover($values->userid_approver, $values->username_approver, $request->promoId, $values->email_approver, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $values->refId . ")", $request['sy']);
                    if (!json_decode($emailApprover)->error) {
                        return array(
                            'error'     => false,
                            'message'   => "Promo Approved Success"
                        );
                    } else {
                        Log::warning("Promo approved success but can't send email notification to " . $values->userid_approver. " (" . $values->email_approver.")");
                        return array(
                            'error'     => true,
                            'message'   => "Promo approved success but can't send email notification to " . $values->userid_approver. " (" . $values->email_approver.")"
                        );
                    }
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
                'message'   => "Promo Approve Failed"
            );
        }
    }

    public function approveLinkEmail(Request $request): array
    {
        $api = config('app.api') . '/promo/approvalreconbyemail/approve';
        $api_email = config('app.api') . '/tools/email';
        $promoSKPHeader = [
            "skpDraftAvail"         => (($request->skpDraftAvail) ? false : true),
            "skpDraftAvailBfrAct60" => (($request->skpDraftAvailBfrAct60) ? false : true),
            "periodMatch"           => (($request->periodMatch) ? false : true),
            "investmentMatch"       => (($request->investmentMatch) ? false : true),
            "mechanismMatch"        => (($request->mechanismMatch) ? false : true),
            "skpSign7"              => (($request->skpSign7) ? false : true),
            "entityDraft"           => (($request->entityDraft) ? false : true),
            "brandDraft"            => (($request->brandDraft) ? false : true),
            "periodDraft"           => (($request->periodDraft) ? false : true),
            "activityDescDraft"     => (($request->activityDescDraft) ? false : true),
            "mechanismDraft"        => (($request->mechanismDraft) ? false : true),
            "investmentDraft"       => (($request->investmentDraft) ? false : true),
            "entity"                => (($request->entity) ? false : true),
            "brand"                 => (($request->brand) ? false : true),
            "activityDesc"          => (($request->activityDesc) ? false : true),
            "distributorDraft"      => (($request->distributorDraft) ? false : true),
            "distributor"           => (($request->distributor) ? false : true),
            "channelDraft"          => (($request->channelDraft) ? false : true),
            "channel"               => (($request->channel) ? false : true),
            "storeNameDraft"        => (($request->storeNameDraft) ? false : true),
            "storeName"             => (($request->storeName) ? false : true),
            "skpstatus"             => (($request->skpstatus) ? 0 : 1),
            "skp_notes"             => "",
        ];
        $data = [
            'promoSKPHeader'=> $promoSKPHeader,
            'promoId'       => $request->promoId,
            'notes'         => "",
            'profileId'     => $request->profileId,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST Approval
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;

                if($values->isFullyApprovedRecon){
                    // send email to initiator
                    $subject = "Promo Reconciliation fully approved (". $values->refId .")";
                    $emailInitiator = [
                        'email'     => $values->email_initiator,
                        'subject'   => $subject,
                        'body'   => "This email was sent automatically by Optima System in responses to approve promo.<br><p><p>Promo Number : " . $values->refId ."<br>Approved By : " . $values->userid_approver ."<br><p><p>Thank you,<br>Optima System"
                    ];

                    $responseEmailInitiator = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $emailInitiator);
                    if ($responseEmailInitiator->status() === 200) {
                        Log::info("Send Email Success");
                        return array(
                            'error' => false,
                            'message' => "Promo " . $request->refId . " Approved"
                        );
                    } else {
                        Log::warning("Promo approved success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_Initiator.")");
                        return array(
                            'error'     => true,
                            'message'   => "Promo approved success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_Initiator.")"
                        );
                    }
                } else {
                    // send email to other approver
                    $emailApprover = $this->sendEmailApprover($values->userid_approver, $values->username_approver, $request->promoId, $values->email_approver, "[APPROVAL NOTIF] Promo Reconciliation requires approval (" . $values->refId . ")", $request['sy']);
                    if (!json_decode($emailApprover)->error) {
                        return array(
                            'error' => false,
                            'message' => "Promo " . $request->refId . " Approved"
                        );
                    } else {
                        Log::warning("Promo approved success but can't send email notification to " . $values->userid_approver . " (" . $values->email_approver . ")");
                        return array(
                            'error' => true,
                            'message' => "Promo approved success but can't send email notification to " . $values->userid_approver . " (" . $values->email_approver . ")"
                        );
                    }
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
                'message'   => "Promo Approve Failed"
            );
        }
    }

    protected function sendEmailApprover($userApprover, $nameApprover, $id, $email, $subject, $yearPromo): bool|string
    {
        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        if ($yearPromo >= 2025){
            $api_promo = config('app.api') . '/promo/display/email/id';
        } else {
            $api_promo = config('app.api') . '/promo/display/email/id';
        }

        $query_promo = [
            'id'           => $id,
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
                    $viewEmail = 'Promo::promo-approval-recon.email-approval';

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
                        'sy'            => $yearPromo
                    ]);
                } else {
                    $viewEmail = 'Promo::promo-approval-recon.email-approval';

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
                        'sy'            => $yearPromo
                    ]);
                }
                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));

                $message = view($viewEmail, compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();

                $data = [
                    'email'     => $email,
                    'subject'   => $subject,
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

    public function sendBack(Request $request)
    {
        $api = config('app.api') . '/promo/approvalrecon/sendback';
        $api_email = config('app.api') . '/tools/email';
        $promoSKPHeader = [
            "skpDraftAvail"         => (($request->skpDraftAvail) ? false : true),
            "skpDraftAvailBfrAct60" => (($request->skpDraftAvailBfrAct60) ? false : true),
            "periodMatch"           => (($request->periodMatch) ? false : true),
            "investmentMatch"       => (($request->investmentMatch) ? false : true),
            "mechanismMatch"        => (($request->mechanismMatch) ? false : true),
            "skpSign7"              => (($request->skpSign7) ? false : true),
            "entityDraft"           => (($request->entityDraft) ? false : true),
            "brandDraft"            => (($request->brandDraft) ? false : true),
            "periodDraft"           => (($request->periodDraft) ? false : true),
            "activityDescDraft"     => (($request->activityDescDraft) ? false : true),
            "mechanismDraft"        => (($request->mechanismDraft) ? false : true),
            "investmentDraft"       => (($request->investmentDraft) ? false : true),
            "entity"                => (($request->entity) ? false : true),
            "brand"                 => (($request->brand) ? false : true),
            "activityDesc"          => (($request->activityDesc) ? false : true),
            "distributorDraft"      => (($request->distributorDraft) ? false : true),
            "distributor"           => (($request->distributor) ? false : true),
            "channelDraft"          => (($request->channelDraft) ? false : true),
            "channel"               => (($request->channel) ? false : true),
            "storeNameDraft"        => (($request->storeNameDraft) ? false : true),
            "storeName"             => (($request->storeName) ? false : true),
            "skpstatus"             => (($request->skpstatus) ? 0 : 1),
            "skp_notes"             => "",
        ];
        $data = [
            'promoSKPHeader'=> $promoSKPHeader,
            'promoId'       => $request->promoId,
            'notes'         => $request->notes,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $categoryShortDescEnc = $request['categoryShortDescEnc'];
            if ($categoryShortDescEnc === "DC" || $categoryShortDescEnc === "RC") $categoryShortDescEnc = urlencode($this->encCategoryShortDesc($categoryShortDescEnc));

            // POST SendBack
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;

                // send email to initiator
                $bodyInitiator = [
                    'email'     => $values->email_initiator,
                    'subject'   => "Promo ID Sendback (" . $values->refId .") By " . $request->session()->get('profile'),
                    'body'      => 'This email was sent automatically by Optima System in responses to send back promo.<br><p><p>Promo Number : <a href="' . config('app.url') .
                        '/promo/recon-send-back/form?method=update&id='. $request->promoId . '&c='. $categoryShortDescEnc . '">' . $values->refId . '</a><br>Sendback By : ' . $request->session()->get('profile') . '-' . $request->session()->get('name') .
                        '<br>Reason : ' . $request->notes .'<br><p><p>Thank you,<br>Optima System'
                ];

                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $bodyInitiator);

                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success");
                    return array(
                        'error'     => false,
                        'data'      => [],
                        'message'   => "Promo Sendback Success"
                    );
                } else {
                    Log::info("Promo sendback success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_initiator.")");
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => "Promo sendback success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_initiator.")"
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
                'message'   => "Promo Senback Failed"
            );
        }
    }

    public function previewAttachment(Request $request)
    {
        $path = '/assets/media/promo/' . $request->i . '/row' . $request->r . '/' . $request->fileName;
        $title = $request->t;
        $isExist = false;
        if (Storage::disk('optima')->exists($path)) $isExist = true;

        return view("Promo::promo-approval-recon.attachment-preview", compact('path', 'isExist', 'title'));
    }

    public function approvePage(Request $request)
    {
        $dataRaw = MyEncrypt::decrypt($request->p);
        $data = json_decode($dataRaw);
        $title = "Approve Promo " . ($data->refId ?? "");
        return view('Promo::promo-approval-recon.approve-from-email', compact('title'));
    }

    public function sendBackPage(Request $request)
    {
        $dataRaw = MyEncrypt::decrypt($request->p);
        $data = json_decode($dataRaw);
        $title = "Send Back Promo " . ($data->refId ?? "");
        return view('Promo::promo-approval-recon.sendback-from-email', compact('title'));
    }

    public function getEncryptedPromo(Request $request)
    {
        if (!MyEncrypt::decrypt($request->param)) {
            return json_encode([
                'error' => true
            ]);
        } else {
            return MyEncrypt::decrypt($request->param);
        }
    }

    public function sendBackLinkEmail(Request $request)
    {
        $api = config('app.api') . '/promo/approvalreconbyemail/sendback';
        $api_email = config('app.api') . '/tools/email';
        $promoSKPHeader = [
            "skpDraftAvail"         => (($request->skpDraftAvail) ? false : true),
            "skpDraftAvailBfrAct60" => (($request->skpDraftAvailBfrAct60) ? false : true),
            "periodMatch"           => (($request->periodMatch) ? false : true),
            "investmentMatch"       => (($request->investmentMatch) ? false : true),
            "mechanismMatch"        => (($request->mechanismMatch) ? false : true),
            "skpSign7"              => (($request->skpSign7) ? false : true),
            "entityDraft"           => (($request->entityDraft) ? false : true),
            "brandDraft"            => (($request->brandDraft) ? false : true),
            "periodDraft"           => (($request->periodDraft) ? false : true),
            "activityDescDraft"     => (($request->activityDescDraft) ? false : true),
            "mechanismDraft"        => (($request->mechanismDraft) ? false : true),
            "investmentDraft"       => (($request->investmentDraft) ? false : true),
            "entity"                => (($request->entity) ? false : true),
            "brand"                 => (($request->brand) ? false : true),
            "activityDesc"          => (($request->activityDesc) ? false : true),
            "distributorDraft"      => (($request->distributorDraft) ? false : true),
            "distributor"           => (($request->distributor) ? false : true),
            "channelDraft"          => (($request->channelDraft) ? false : true),
            "channel"               => (($request->channel) ? false : true),
            "storeNameDraft"        => (($request->storeNameDraft) ? false : true),
            "storeName"             => (($request->storeName) ? false : true),
            "skpstatus"             => (($request->skpstatus) ? 0 : 1),
            "skp_notes"             => "",
        ];
        $data = [
            'promoSKPHeader'=> $promoSKPHeader,
            'promoId'       => $request->promoId,
            'notes'         => $request->notes,
            'profileId'     => $request->profileId
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            // POST SendBack
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;

                // send email to initiator
                $bodyInitiator = [
                    'email'     => $values->email_initiator,
                    'subject'   => "Promo ID Sendback (" . $values->refId .") By " . $request->profileId,
                    'body'      => "This email was sent automatically by Optima System in responses to send back promo.<br><p><p>Promo Number : <a href=" . config('app.url') .
                        "/promo/recon-send-back/form?method=update&id=". $request->promoId . ">" . $values->refId . "</a><br>Sendback By : " . $request->profileId . "-" . $request->nameApprover .
                        "<br>Reason : " . $request->notes ."<br><p><p>Thank you,<br>Optima System"
                ];

                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $bodyInitiator);

                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success");
                    return array(
                        'error'     => false,
                        'data'      => [],
                        'message'   => "Promo Sendback Success"
                    );
                } else {
                    Log::info("Promo sendback success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_initiator.")");
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => "Promo sendback success but can't send email notification to " . $values->userid_initiator. " (" . $values->email_initiator.")"
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
                'message'   => "Promo Senback Failed"
            );
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
