<?php

namespace App\Modules\UserAccess\Controllers;

use App\Exports\Export;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Maatwebsite\Excel\Facades\Excel;

use Wording;

class UserController extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function myProfilePage()
    {
        return view('UserAccess::user.myprofile');
    }

    public function getUserCoverage(Request $request)
    {
        $api = config('app.api'). '/user/ditributor';
        Log::info('get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return array(
                    'error' => false,
                    'data' => $resVal,
                    'message' => "Get Data Coverage success"
                );
            } else {
                $message = json_decode($response)->message;
                Log::warning('get api ' . $api);
                Log::warning($message);
                return array(
                    'error' => false,
                    'data' => [],
                    'message' => $message
                );

            }
        } catch (\Exception $e) {
            Log::error('get api ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => false,
                'data' => [],
                'message' => $e->getMessage()
            );

        }
    }

    public function landingPage()
    {
        return view('UserAccess::user.index');
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/useraccess/users';
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
                'Status'                    => $request->active,
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
                Log::warning('get api ' . $api);
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

    public function userFormPage(Request $request)
    {
        return view('UserAccess::user.form');
    }

    public function getListProfile(Request $request)
    {
        $api = config('app.api'). '/useraccess/users/userprofile';
        Log::info('get API ' . $api);
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
                Log::warning('get api ' . $api);
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

    public function save(Request $request)
    {
        $api = config('app.api') . '/useraccess/users';
        $data = [
            'email'         => $request->email,
            'username'      => $request->username,
            'contactinfo'   => $request->contactinfo,
            'profile'       => json_decode($request->profile)
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Save success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('post API ' . $api);
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

    public function update(Request $request)
    {
        $api = config('app.api') . '/useraccess/users';
        $data = [
            'id'            => $request->id,
            'email'         => $request->email,
            'username'      => $request->username,
            'contactinfo'   => $request->contactinfo,
            'profile'       => json_decode($request->profile)
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Update success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('put API ' . $api);
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

    public function delete(Request $request)
    {
        $api = config('app.api') . '/useraccess/users';
        $data = [
            'id'            => $request->id
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "User Deactivated",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
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
                'message'   => "error : Deactivate Failed"
            );
        }
    }

    public function activate(Request $request)
    {
        $api = config('app.api') . '/useraccess/users/activate';
        $data = [
            'id'            => $request->id
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "User activated",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
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
                'message'   => "error : Activate Failed"
            );
        }
    }

    public function resetPassword(Request $request)
    {
        $api = config('app.api') . '/auth/resetpassword';
        $data = [
            'userId'            => $request->id
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->asForm()->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'message'   => "Reset password success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
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
                'message'   => "error : Reset password failed"
            );
        }
    }

    public function getDataByID(Request $request)
    {
        $api = config('app.api'). '/useraccess/users/id';
        $query = [
            'userid'           => $request->userid,
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

    public function exportXls(Request $request)
    {
        $api = config('app.api'). '/useraccess/users';
        try {
            $query = [
                'Status'                    => $request->active,
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
                'SortColumn'                => 'userName',
                'SortDirection'             => 'asc'
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->email;
                    $arr[] = $fields->userName;
                    $arr[] = $fields->contactInfo;
                    $arr[] = $fields->status;
                    $arr[] = $fields->profileuser;

                    $result[] = $arr;
                }

                $filename = 'User Management -';
                $title = 'A1:E1'; //Report Title Bold and merge
                $header = 'A3:E3'; //Header Column Bold and color
                $heading = [
                    ['User Management'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Email', 'User Name', 'Contact Info', 'Status', 'User Profile']
                ];

                $formatCell =  [

                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' . $mc .  '.xlsx');
            } else {
                $message = json_decode($response)->message;
                $result = array(
                    'error' => $response->status(),
                    'data' => [],
                    'message' => $message
                );
                Log::warning('get API ' . $api);
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
