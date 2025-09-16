<?php

namespace App\Modules\Mapping\Controllers;

use App\Exports\Export;
use App\Exports\Mapping\ExportTemplateMappingWHT;
use App\Helpers\CallApi;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Exception;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class WHTType extends Controller
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
        $title = "Mapping WHT Type";
        return view('Mapping::wht-type.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): bool|string
    {
        $api = '/mapping/distributor-wht';

        $page = 0;
        if ($request['length'] > -1) { $page = ($request['start'] / $request['length']); }
        ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];

        $short = $request['order'][0]['dir'];
        $column = $request['order'][0]['column'];
        $length = $request['length'];
        $SortColumn = $request['columns'][(int) $column]['data'];

        $query = [
            'Search'                    => $search,
            'PageNumber'                => $page,
            'PageSize'                  => (int) $length,
            'SortColumn'                => $SortColumn,
            'SortDirection'             => $short,
            'distributor'               => $request['distributor'] ?? '',
            'subActivity'               => $request['subActivity'] ?? '',
            'subAccount'                => $request['subAccount'] ?? '',
            'WHTType'                   => $request['WHTType'] ?? '',
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            if ($request['length'] > -1) {
                if ($request->has('Search')) {
                    $recordsFiltered = json_decode($res)->data->filteredCount;
                } else {
                    $recordsFiltered = json_decode($res)->data->totalCount;
                }
            } else {
                $recordsFiltered = json_decode($res)->data->totalCount;
            }
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => json_decode($res)->data->data,
                "recordsTotal" => json_decode($res)->data->totalCount,
                "recordsFiltered" => $recordsFiltered
            ]);
        } else {
            return $res;
        }
    }

    public function WHTTypeFormPage (): Factory|View|Application
    {
        $title = "Mapping WHT Type";
        return view('Mapping::wht-type.form', compact('title'));
    }

    public function WHTTypeFormUploadPage (): Factory|View|Application
    {
        $title = "Upload Mapping WHT Type";
        return view('Mapping::wht-type.upload-xls', compact('title'));
    }

    public function getListDistributor(): bool|string
    {
        $api = '/mapping/distributor-wht/distributor';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubActivity(): bool|string
    {
        $api = '/mapping/distributor-wht/subactivity';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubAccount(): bool|string
    {
        $api = '/mapping/distributor-wht/subaccount';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListWHTType(): bool|string
    {
        $api = '/mapping/distributor-wht/whttype';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getData(Request $request): bool|string
    {
        $api = '/mapping/distributor-wht/id';

        $query = [
            'id'   => $request['id'],
        ];

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function save(Request $request): bool|array|string
    {
        $api = '/mapping/distributor-wht';
        $body = [
            'distributor'   => $request['distributor'],
            'subActivity'   => $request['subActivity'],
            'subAccount'    => $request['subAccount'],
            'whtType'       => $request['WHTType'],
        ];
        $callApi = new CallApi();
        return $callApi->postUsingToken($this->token, $api, $body);
    }

    public function update(Request $request): bool|array|string
    {
        $api = '/mapping/distributor-wht';
        $body = [
            'id'        => $request['id'],
            'whtType'   => $request['WHTType'],
        ];
        $callApi = new CallApi();
        return $callApi->putUsingToken($this->token, $api, $body);
    }

    public function uploadXls(Request $request): bool|array|string
    {
        $api = config('app.api'). '/mapping/distributor-wht/upload';
        try {
            if ($request->file('file')) {
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, [
                        'name'      => 'formFile',
                        'contents'  => $request->file('file')->getContent()
                    ]);

                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response),
                        'error'     => false,
                        'message'   => "Upload success",
                    ]);
                } else {
                    return array(
                        'error' => true,
                        'message' => "Upload failed"
                    );
                }
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function delete(Request $request): bool|string
    {
        $api = '/mapping/distributor-wht';
        $body = [
            'id'         => $request['id']
        ];

        $callApi = new CallApi();
        return $callApi->deleteUsingToken($this->token, $api, $body);
    }

    public function downloadTemplate(Request $request): BinaryFileResponse|bool|string
    {
        $api = '/mapping/distributor-wht/download';

        $query = [
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'distributor',
            'SortDirection'             => 'asc',
            'distributor'               => $request['distributor'] ?? '',
            'subActivity'               => $request['subActivity'] ?? '',
            'subAccount'                => $request['subAccount'] ?? '',
            'WHTType'                   => $request['WHTType'] ?? '',
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->data;

            $result=[];
            foreach ($resVal as $fields) {
                $arr = [];
                $arr[] = $fields->distributor;
                $arr[] = $fields->subActivity;
                $arr[] = $fields->subAccount;
                $arr[] = $fields->WHTType;

                $result[] = $arr;
            }

            $filename = 'Template_Upload_WHT';
            $header = 'A1:D1';
            $heading = [
                ['Distributor', 'Sub Activity', 'Sub Account', 'WHT Type Mapping']
            ];

            $export = new ExportTemplateMappingWHT($result, $heading, $header);
            return Excel::download($export, $filename . '.xlsx');
        } else {
            return $res;
        }
    }

    public function exportXls(Request $request): BinaryFileResponse|bool|string
    {
        $api = '/mapping/distributor-wht';

        $query = [
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'distributor',
            'SortDirection'             => 'asc',
            'distributor'               => $request['distributor'] ?? '',
            'subActivity'               => $request['subActivity'] ?? '',
            'subAccount'                => $request['subAccount'] ?? '',
            'WHTType'                   => $request['WHTType'] ?? '',
        ];

        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->data;

            $result=[];
            foreach ($resVal as $fields) {
                $arr = [];
                $arr[] = $fields->distributor;
                $arr[] = $fields->subActivity;
                $arr[] = $fields->subAccount;
                $arr[] = $fields->WHTType;
                $arr[] = (($fields->modifiedOn) ? date('Y-m-d H:i:s' , strtotime($fields->modifiedOn)) : null);
                $arr[] = $fields->modifiedBy;

                $result[] = $arr;
            }

            $filename = 'Mapping WHT Type -';
            $title = 'A1:F1'; //Report Title Bold and merge
            $header = 'A3:F3'; //Header Column Bold and color
            $heading = [
                ['Mapping WHT Type'],
                ['Date Retrieved : ' . date('Y-m-d')],
                ['Distributor', 'Sub Activity', 'Sub Account', 'WHT Type', 'Modified On', 'Modified By']
            ];

            $formatCell =  [

            ];

            $export = new Export($result, $heading, $title, $header, $formatCell);
            $mc = microtime(true);
            return Excel::download($export, $filename . date('Y-m-d') . ' ' . $mc .  '.xlsx');
        } else {
            return $res;
        }
    }
}
