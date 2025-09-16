<?php

namespace App\Modules\DebitNote\Controllers;

use App\Helpers\CallApi;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class DnAssignment extends Controller
{
    private mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "Debit Note Manual [Assignment]";
        return view('DebitNote::dn-assignment.index', compact('title'));
    }

    public function formPage()
    {
        $title = "Debit Note Manual [Assignment]";
        return view('DebitNote::dn-assignment.form', compact('title'));
    }

    public function getList(Request $request): bool|string
    {
        $api = '/dn/manual-assignment';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'Search'                    => ($request['search']['value'] ?? ""),
            'SortColumn'                => $request['columns'][$request['order'][0]['column']]['data'],
            'SortDirection'             => $request['order'][0]['dir'],
            'PageSize'                  => $request['length'],
            'PageNumber'                => $page,
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->data;
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => $resVal,
                "recordsTotal" => json_decode($res)->data->recordsTotal,
                "recordsFiltered" => json_decode($res)->data->recordsFiltered
            ]);
        } else {
            return $res;
        }
    }

    public function getData(Request $request)
    {
        $api = config('app.api'). '/dn/manual-assignment/id';
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
                    'usergroupid'   => $request->session()->get('role')
                );
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error'     => true,
                    'data'      => [],
                    'usergroupid'   => $request->session()->get('role'),
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
        $api = config('app.api'). '/dn/manual-assignment/approvedpromo-for-dn';
        $query = [
            'periode'     => $request->period,
            'entity'      => $request->entityId,
            'channel'     => $request->channelId,
            'account'     => $request->subAccountId,

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

    public function save(Request $request) {
        $api = config('app.api') . '/dn/manual-assignment/assign';
        $data = [
            'dnId'        => $request->dnId,
            'promoId'     => $request->promoId,
            'userId'      => $request->session()->get('profile'),
        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $message = json_decode($response)->message;
                return json_encode([
                    'error'     => false,
                    'message'   => $message
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

    public function forward(Request $request) {
        $api = config('app.api') . '/dn/manual-assignment/assign-promo';
        $data = [
            'dnid'                      => $request->dnId,
            'approver_userid'           => $request->approverUserId,
            'internal_order_number'     => $request->internalOrderNumber,

        ];
        Log::info('Get API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                $values = json_decode($response)->values;
                $api_email = config('app.api') . '/tools/email';
                $message = "This email was sent automatically by Optima System in responses assign promo to DN. You can login into Optima System using:<br>Username : " . $values->approver_userid . "<br><p>To login using this user, login into Optima System, either click on the button or copy and paste the follow link<p><a href='" . config('app.url') . "'>Optima System</a><br><p>Thank you,<br>Optima System";

                $dataEmail = [
                    'email'     => ($values->approver_email ?? ""),
                    'subject'   => "Optima no-reply [Assign Promo to DN]",
                    'body'      => $message
                ];
                $responseEmail =  Http::asForm()->withToken($this->token)->post($api_email, $dataEmail);
                if ($responseEmail->status() === 200) {
                    return json_encode([
                        'error'     => false,
                        'message'   => 'DN Forward Success'
                    ]);
                } else {
                    Log::warning($api_email);
                    Log::warning($responseEmail);
                    return array(
                        'error'     => true,
                        'data'      => [],
                        'message'   => 'DN Forward Failed'
                    );
                }
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
}
