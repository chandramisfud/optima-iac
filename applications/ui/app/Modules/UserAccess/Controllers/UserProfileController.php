<?php

namespace App\Modules\UserAccess\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use Maatwebsite\Excel\Facades\Excel;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;


class UserProfileController extends Controller
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
        $title = "User Profile Management";
        return view('UserAccess::user-profile.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request) {
        $api = config('app.api'). '/useraccess/userprofile';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $SortColumn = $request->columns[(int) $column]['data'];

            ($request->status==null) ? $status="ALL" : $status=$request->status;
            $query = [
                'Status'                    => $status,
                'usergroupid'               => $request->usergroupid,
                'userlevel'                 => $request->userlevel,
                'SortColumn'                => $SortColumn,
                'SortDirection'             => $sort,
                'Search'                    => $search,
                'PageNumber'                => (int) $page,
                'PageSize'                  => (int) $length,
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

    public function formPage(Request $request) {
        $title = "User Profile Management";
        return view('UserAccess::user-profile.form', compact('title'));
    }

    public function getDataByID(Request $request) {
        $api = config('app.api'). '/useraccess/userprofile/id';
        $query = [
            'id'           => $request->profileId,
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

    public function getListUserGroup(Request $request)
    {
        $api = config('app.api'). '/useraccess/userprofile/usergroup';
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

    public function getListUserRightsByUserGroupId(Request $request)
    {
        $api = config('app.api'). '/useraccess/userprofile/userrights';
        $query = [
            'UserGroupId'           => $request->usergroupid,
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

    public function getListDistributor(Request $request) {
        $api = config('app.api'). '/useraccess/userprofile/distributor';
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

    public function getListCategory(Request $request) {
        $api = config('app.api'). '/useraccess/userprofile/category';
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

    public function getListChannel() {
        $api = '/useraccess/userprofile/channellist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function save(Request $request) {
        $api = config('app.api') . '/useraccess/userprofile';
        $distributorlist = json_decode($request->distributorid);
        $categoryList = json_decode($request->categoryId);
        $channelList = json_decode($request->channelId);
        $data = [
            'id'                => $request->profileid,
            'username'          => $request->profilename,
            'email'             => $request->email,
            'department'        => $request->department,
            'jobtitle'          => $request->jobtitle,
            'usergroupid'       => $request->usergroupid,
            'userlevel'         => $request->usergrouplevel,
            'distributorlist'   => $distributorlist,
            'categoryId'        => $categoryList,
            'channelId'         => $channelList,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function update(Request $request) {
        $api = config('app.api') . '/useraccess/userprofile';
        $distributorlist = json_decode($request->distributorid);
        $categoryList = json_decode($request->categoryId);
        $channelList = json_decode($request->channelId);
        $data = [
            'id'                => $request->profileid,
            'username'          => $request->profilename,
            'email'             => $request->email,
            'department'        => $request->department,
            'jobtitle'          => $request->jobtitle,
            'usergroupid'       => $request->usergroupid,
            'userlevel'         => $request->usergrouplevel,
            'distributorlist'   => $distributorlist,
            'categoryId'        => $categoryList,
            'channelId'         => $channelList,
        ];
        Log::info('put API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
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

    public function delete(Request $request) {
        $api = config('app.api') . '/useraccess/userprofile';
        $data = [
            'id'           => $request->id
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => "Profile Deactivated",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => "Delete Failed"
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

    public function activate(Request $request)
    {
        $api = config('app.api') . '/useraccess/userprofile/activate';
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
                    'message'   => "Profile activated",
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
                'message'   => "error : Activate Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/useraccess/userprofile';
        ($request->status==null) ? $status="ALL" : $status=$request->status;
        try {
            $query = [
                'Status'                    => $status,
                'usergroupid'               => $request->usergroupid,
                'userlevel'                 => $request->userlevel,
                'SortColumn'                => 'id',
                'SortDirection'             => 'asc',
                'Search'                    => '',
                'PageNumber'                => 0,
                'PageSize'                  => -1,
            ];

            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->id;
                    $arr[] = $fields->username;
                    $arr[] = $fields->email;
                    $arr[] = $fields->department;
                    $arr[] = $fields->jobtitle;
                    $arr[] = (($fields->usergroupid) ? $fields->usergroupid  . ' - ' . $fields->usergroupname : null);
                    $arr[] = (($fields->userlevel) ? $fields->userlevel  . ' - ' . $fields->levelname : null);
                    $arr[] = $fields->profileCategory;
                    $arr[] = $fields->status;

                    $result[] = $arr;
                }

                $filename = 'User Profile -';
                $title = 'A1:I1'; //Report Title Bold and merge
                $header = 'A3:I3'; //Header Column Bold and color
                $heading = [
                    ['User Profile'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Profile ID', 'Profile Name', 'Email', 'Department', 'Job Title', 'User Group Menu', 'User Group Rights', 'Category', 'Status']
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
