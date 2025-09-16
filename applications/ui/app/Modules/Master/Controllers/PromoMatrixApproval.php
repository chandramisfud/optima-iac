<?php

namespace App\Modules\Master\Controllers;

use App\Exports\Master\ExportViewMasterPromoMatrixApproval;
use App\Helpers\CallApi;
use App\Helpers\MyEncrypt;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use App\Exports\Export;
use App\Exports\Master\ExportViewMasterPromoMechanism;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

use Wording;

class PromoMatrixApproval extends Controller
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
        return view('Master::promo-matrix-approval.index');
    }

    public function getList(Request $request)
    {
        $api = config('app.api'). '/master/matrixpromoapproval';
        $query = [
            'entity'        => $request->entity,
            'distributor'   => $request->distributor,
            'userid'        => '0',
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $data = json_decode($response)->values;
                return array(
                    'error'     => false,
                    'data'      => $data
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

    public function promoMatrixApprovalFormPage(Request $request) {
        return view('Master::promo-matrix-approval.form');
    }

    public function processPage(Request $request)
    {
        return view('Master::promo-matrix-approval.process');
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

                $message = view('Master::promo-matrix-approval.email-approval-cycle1', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
                    ->toHtml();
                if ($request['cycle'] == 2) {
                    $message = view('Master::promo-matrix-approval.email-approval-cycle2', compact('title', 'subtitle', 'data', 'promoId', 'urlid', 'urlrefid' , 'userApprover', 'recon', 'ar_fileattach', 'paramEncrypted'))
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

    public function getListPromo(Request $request): bool|string
    {
        $api = '/tools/upload/matrixapprovalpromobymatrix';
        $query = [
            'matrixId'  => decrypt($request['matrixId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListEntity(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/entity';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
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

    public function getDataDistributor(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/distributor';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'PrincipalId'  => $request->PrincipalId
            ]);
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

    public function getListCategory(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/category';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
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

    public function getListSubActivityType(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/subactivitytype/categoryid';
        $query = [
            'categoryId'    => $request['categoryId']
        ];
        Log::info('GET ' . $api);
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

    public function getListChannel(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/channel';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
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

    public function getDataSubChannel(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/subchannel';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, [
                'ChannelId'  => $request->ChannelId
            ]);
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

    public function getListProfile(Request $request)
    {
        $api = config('app.api'). '/master/matrixpromoapproval/initiator';
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() == 200) {
                $data = json_decode($response)->values;
                return array(
                    'error'     => false,
                    'data'      => $data
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

    public function getDataByID(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/id';
        $query = [
            'id'  => $request->id
        ];
        Log::info('GET ' . $api);
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

    public function save(Request $request) {
        $api = config('app.api') . '/master/matrixpromoapproval';
        $data = [
            "entityid"              => $request->entity,
            "distributorid"         => $request->distributor,
            "categoryId"            => $request->category,
            "subactivitytypeid"     => $request->sub_activity_type,
            "channelid"             => $request->channel,
            "subchannelid"          => ($request->sub_channel ?? 0),
            "initiator"             => $request->initiator,
            "mininvestment"         => $request->min_invest,
            "maxinvestment"         => $request->max_invest,
            "userid"                => "0",
            "useremail"             => "0",
            'matrixApprover'        => json_decode($request->matrixApprover)
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Save success',
                    'matrixId' => encrypt(json_decode($response)->values[0]->matrixid)
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('post API ' . $api);
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
                'message'   => "error : Save Failed"
            );
        }
    }

    public function update(Request $request) {
        $api = config('app.api') . '/master/matrixpromoapproval';
        $data = [
            'id'                    => $request->id,
            "entityid"              => $request->entity,
            "distributorid"         => $request->distributor,
            "categoryId"            => $request->category,
            "subactivitytypeid"     => $request->sub_activity_type,
            "channelid"             => $request->channel,
            "subchannelid"          => ($request->sub_channel ?? 0),
            "initiator"             => $request->initiator,
            "mininvestment"         => $request->min_invest,
            "maxinvestment"         => $request->max_invest,
            "userid"                => "0",
            "useremail"             => "0",
            'matrixApprover'        => json_decode($request->matrixApprover)
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => 'Update success',
                    'matrixId' => encrypt(json_decode($response)->values[0]->matrixid)
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('put API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('put API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Update Failed"
            );
        }
    }

    public function delete(Request $request) {
        $api = config('app.api') . '/master/matrixpromoapproval';
        $data = [
            'id'            => $request['id']
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => 'Delete success',
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning('delete API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('delete API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Delete Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval';
        $query = [
            'entity'        => (int) (($request->entity == '' || $request->entity == null) ? 0 : $request->entity),
            'distributor'   => (int) (($request->distributor == '' || $request->distributor == null) ? 0 : $request->distributor),
            'userid'        => 0,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        $entity = '';
        if($request->entity == ''){
            $entity = 'ALL';
        }else{
            $entity = $request->entityText;
        }
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->minInvestment;
                    $arr[] = $fields->maxInvestment;
                    $arr[] = $fields->matrixApprover;

                    $result[] = $arr;
                }

                $filename = 'Matrix Promo Approval Current -';
                $title = 'A1:J1'; //Report Title Bold and merge
                $header = 'A4:J4'; //Header Column Bold and color
                $heading = [
                    ['Promo Matrix Approval - Current'],
                    ['Entity : ' . $entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Category', 'Initiator', 'Channel', 'Sub Channel', 'Sub Activity Type', 'Min. Investment', 'Max. Investment', 'Matrix Approver']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
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

    public function exportXlsHistorical(Request $request) {
        $api = config('app.api'). '/master/matrixpromoapproval/history';
        $query = [
            'entity'        => ($request['entity'] ?? 0),
            'distributor'   => ($request['distributor'] ?? 0),
            'userid'        => 0,
            'PageNumber'    => 0,
            'PageSize'      => -1,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        $entity = $request['entityText'];
        if($request['entity'] == ''){
            $entity = 'ALL';
        }
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->initiator;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->subActivityType;
                    $arr[] = $fields->minInvestment;
                    $arr[] = $fields->maxInvestment;
                    $arr[] = $fields->matrixApprover;
                    $arr[] = $fields->actionStatus;
                    $arr[] = $fields->actionBy;
                    $arr[] = $fields->actionEmail;
                    $arr[] = $fields->actionOn;

                    $result[] = $arr;
                }

                $filename = 'Matrix Promo Approval Historical -';
                $title = 'A1:N1'; //Report Title Bold and merge
                $header = 'A4:N4'; //Header Column Bold and color
                $heading = [
                    ['Promo Matrix Approval - Historical'],
                    ['Entity : ' . $entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Entity', 'Distributor', 'Category', 'Initiator', 'Channel', 'Sub Channel', 'Sub Activity Type', 'Min. Investment', 'Max. Investment', 'Matrix Approver'
                    , 'Action Status', 'Action Profile', 'Action Email', 'Action On']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
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

    public function uploadPage(Request $request) {
        return view('Master::promo-matrix-approval.form-upload');
    }

    public function processUploadPage(Request $request)
    {
        return view('Master::promo-matrix-approval.process-upload');
    }

    public function uploadXls(Request $request)
    {
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
        } catch (\Exception $e) {
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
}
