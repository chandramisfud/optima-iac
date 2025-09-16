<?php

namespace App\Modules\UserAccess\Controllers;

use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Http\Client\Response;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Wording;

class UserAdminReport extends Controller
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
        $title = "User Administration Report";
        return view('UserAccess::user-admin-report.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/useraccess/useradminreport';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $page = $page;

            $query = [
                'Search'                    => $search,
                'PageNumber'                => $page,
                'PageSize'                  => (int) $length
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

    public function exportXls(Request $request) {
        $api = config('app.api'). '/useraccess/useradminreport';
        try {
            $query = [
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
                    $arr[] = $fields->contactinfo;
                    $arr[] = $fields->distributorid;
                    $arr[] = $fields->usergroupid;
                    $arr[] = $fields->usergroupname;
                    $arr[] = $fields->userlevel;
                    $arr[] = $fields->levelname;
                    $arr[] = $fields->menuid;
                    $arr[] = $fields->menu;
                    $arr[] = $fields->submenu;
                    $arr[] = $fields->flag;
                    $arr[] = $fields->crud;
                    $arr[] = $fields->approve;
                    $arr[] = $fields->create_rec;
                    $arr[] = $fields->read_rec;
                    $arr[] = $fields->update_rec;
                    $arr[] = $fields->delete_rec;

                    $result[] = $arr;
                }

                $filename = 'User Admin Report -';
                $title = 'A1:U1'; //Report Title Bold and merge
                $header = 'A3:U3'; //Header Column Bold and color
                $heading = [
                    ['User Admin Report'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['User ID', 'User Name', 'Email', 'Department', 'Job Title', 'Contact Info', 'Distributor', 'User Group ID',
                        'User Menu Group','User Level','User Menu Level','Menu ID','Menu','Sub Menu','Flag','CRUD','Approve','Create','Read','Update','Delete']
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

}
