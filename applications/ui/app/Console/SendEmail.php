<?php
namespace App\Console;

use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class SendEmail
{
    public function __invoke()
    {
        Log::info('jalankan job schedule');
        $this->sendEmail();
    }

    public function getReminder(): array
    {
        $api = config('app.api') . '/tools/scheduler/reminder';
        try {
            Log::info('get API ' . $api);
            $response = Http::get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                if ($resVal) {
                    return ($resVal);
                }else{
                    Log::info($api. "no Data");
                    return array(
                        'values' => []
                    );
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning('get API ' . $api);
                Log::warning($message);
                return array(
                    'values' => []
                );
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function sendEmail (){
        $apiEmail = config('app.api') . '/tools/email';
        $count = 1;

        Log::info('Approval Reminder');
        $reminder = $this->getReminder();
        if (count($reminder) > 0) {
            foreach($reminder as $val) {
                if($val->send){
                    Log::info('send notif for ' . $val->refId);
                    Log::info('send email to approver (' . $val->userapprover . ' - ' . $val->userApproverEmail . ')');

                    if($val->reconStatus == '0'){
                        $subject = "[APPROVAL REMINDER] Promo requires approval (".$val->refId.") by " . $val->userapprover;
                        $this->email($val->refId, $val->userapprover, $val->userApproverName, $val->parentId, $val->userApproverEmail , "[APPROVAL REMINDER] Promo requires approval (".$val->refId.") from " . $val->createBy . " (Aging : " . $val->aging .")", 0, date('Y', strtotime($val->startPromo)));
                    } else {
                        $subject = "[APPROVAL REMINDER] Promo Reconciliation requires approval (".$val->refId.") by " . $val->userapprover;
                        $this->email($val->refId, $val->userapprover, $val->userApproverName, $val->parentId, $val->userApproverEmail , "[APPROVAL REMINDER] Promo Reconciliation requires approval (".$val->refId.") from " . $val->createBy . " (Aging : " . $val->aging .")", 1, date('Y', strtotime($val->startPromo)));
                    }

                    // send email initiator
                    Log::info('send email to initiator (' . $val->createBy . ' - ' . $val->initiatiorEmail . ')');

                    $dataInitiator = [
                        'email'     => $val->initiatiorEmail,
                        'subject'   => $subject,
                        'body'      => "This email was sent automatically by Optima System in responses to approve promo.<br><p><p>Promo ID : " . $val->refId ."<br>Approver : " . $val->userapprover ."<br>Aging : " . $val->aging ." days<br><p><p>Thank you,<br>Optima System"
                    ];

                    $responseEmailInitiator = Http::asForm()->post($apiEmail, $dataInitiator);
                    if ($responseEmailInitiator->status() === 200) {
                        Log::info('send email to initiator success ' . $count);
                    } else {
                        $resValInitiator = json_decode($responseEmailInitiator);
                        Log::error($apiEmail);
                        Log::error($resValInitiator);
                    }
                    if($count == 40) {
                        sleep(60 * 35);
                        Log::info('waiting for 60 * 35 second');
                    }
                    $count += 1;
                }
            }
        }
    }

    public function email ($refId, $userApprover, $nameApprover, $id, $email, $subject, $recon=0, $yearPromo): array|string
    {
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        if ($yearPromo >= 2025) {
            if ($recon == 0) {
                $api = config('app.api'). '/tools/scheduler/promo/id';
            } else {
                $api = config('app.api'). '/tools/scheduler/promorecon/id';
            }
        } else {
            if ($recon == 0) {
                $api = config('app.api'). '/tools/scheduler/promo/id';
            } else {
                $api = config('app.api'). '/tools/scheduler/promorecon/id';
            }
        }

        try {
            $query = [
                'id' => $id,
            ];

            $response = Http::asForm()->get($api,$query);
            Log::info($api);
            Log::info($response->status());

            if ($response->status() === 200) {
                $ar_fileattach = array();
                if ($yearPromo >= 2025) {
                    $data = json_decode($response)->values;
                    if ($data) {
                        $attach = [];
                        for ($i = 0; $i < count($data->attachments); $i++)  {
                            if($data->attachments[$i]->docLink=='row1')
                                $attach['row1']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row2')
                                $attach['row2']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row3')
                                $attach['row3']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row4')
                                $attach['row4']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row5')
                                $attach['row5']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row6')
                                $attach['row6']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row7')
                                $attach['row7']= $data->attachments[$i]->fileName;
                        }
                        if(!empty($data->attachments))
                            array_push($ar_fileattach, $attach);

                    }else{
                        Log::info($api ." no Data");
                        $result = array(
                            'values' => []
                        );
                        $data = $result['values'];
                    }
                } else {
                    $data = json_decode($response)->values;
                    if ($data) {
                        $attach = [];
                        for ($i = 0; $i < count($data->attachments); $i++)  {
                            if($data->attachments[$i]->docLink=='row1')
                                $attach['row1']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row2')
                                $attach['row2']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row3')
                                $attach['row3']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row4')
                                $attach['row4']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row5')
                                $attach['row5']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row6')
                                $attach['row6']= $data->attachments[$i]->fileName;
                            if($data->attachments[$i]->docLink=='row7')
                                $attach['row7']= $data->attachments[$i]->fileName;
                        }
                        if(!empty($data->attachments))
                            array_push($ar_fileattach, $attach);

                    }else{
                        Log::info($api ." no Data");
                        $result = array(
                            'values' => []
                        );
                        $data = $result['values'];
                    }
                }

                Log::info('send email promo display');

                $dataUrl = json_encode([
                    'promoId'       => $promoId,
                    'refId'         => $refId,
                    'profileId'     => $userApprover,
                    'nameApprover'  => $nameApprover,
                    'sy'            => $yearPromo
                ]);

                $paramEncrypted = urlencode(MyEncrypt::encrypt($dataUrl));
                if($yearPromo >= 2025) {
                    if($recon == 0) {
                        $message = view('Promo::email-reminder.promo-display.email-approval', compact('title', 'subtitle', 'data', 'promoId','recon', 'ar_fileattach', 'paramEncrypted'))->render();
                    } else {
                        $message = view('Promo::email-reminder.promo-display.email-approval-recon', compact('title', 'subtitle', 'data', 'promoId','recon', 'ar_fileattach', 'paramEncrypted'))->render();
                    }
                } else {
                    if($recon == 0) {
                        $message = view('Promo::email-reminder.promo-display.display-cycle1', compact('title', 'subtitle', 'data', 'promoId','recon', 'ar_fileattach', 'paramEncrypted'))->render();
                    } else {
                        $message = view('Promo::email-reminder.promo-display.display-cycle2', compact('title', 'subtitle', 'data', 'promoId','recon', 'ar_fileattach', 'paramEncrypted'))->render();
                    }
                }

                $queryEmail = [
                    'email'     => $email,
                    'subject'   => $subject,
                    'body'      => $message
                ];

                $apiEmail = config('app.api'). '/tools/email';

                $responseEmail = Http::asForm()->post($apiEmail, $queryEmail);
                if ($responseEmail->status() === 200) {
                    return 'Email send success';
                } else {
                    $resValEmail = json_decode($responseEmail);
                    Log::error($apiEmail);
                    Log::error($resValEmail);
                    return 'Error send Email';
                }
            } else {
                Log::error($api ." data");
                Log::error($response);
                Log::error('Fail Get Data Promo Display');
                return 'Error send Email';
            }

        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }
}
