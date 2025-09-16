<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Budget\ExportXlsBudgetVolume;
use App\Exports\Budget\Volume\ExportTemplateBudgetVolume;
use App\Helpers\MyEncrypt;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Maatwebsite\Excel\Facades\Excel;
use App\Helpers\CallApi;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Symfony\Component\HttpFoundation\BinaryFileResponse;
use Symfony\Component\HttpFoundation\StreamedResponse;

class BudgetVolume extends Controller
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
        $title = "Budget [SS Input]";
        return view('Budget::budget-volume.index', compact('title'));
    }

    public function uploadPage(): Factory|View|Application
    {
        $title = "Budget [SS Input]";
        return view('Budget::budget-volume.upload-xls', compact('title'));
    }

    public function getListPaginateFilter(Request $request): bool|array|string
    {
        $api = config('app.api'). '/budget/ssvolume/list';
        try {
            $page = 0;
            if ($request['length'] > -1) {
                $page = ($request['start'] / $request['length']);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $SortColumn = $request['columns'][(int) $column]['data'];

            $query = [
                'period'            => $request['period'],
                'channel'           => json_decode($request['channel']),
                'subChannel'        => json_decode($request['subChannel']),
                'account'           => json_decode($request['account']),
                'subAccount'        => json_decode($request['subAccount']),
                'region'            => json_decode($request['region']),
                'groupBrand'        => json_decode($request['groupBrand']),
                'txtSearch'         => $search,
                'PageNumber'        => $page,
                'PageSize'          => (int) $length,
                'sort'              => $sort,
                'order'             => $SortColumn
            ];
            Log::info('get API ' . $api);
            Log::info('payload ' . json_encode($query));
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                return json_encode([
                    "draw" => (int) $request['draw'],
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

    public function getListChannel(): bool|string
    {
        $api = '/budget/ssvolume/channellist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListRegion(): bool|string
    {
        $api = '/budget/ssvolume/regionlist';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSubChannel(Request $request): bool|string
    {
        $api = '/budget/ssvolume/subchannellist';
        $query = [
            'channelId'  => json_decode($request['channelId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListAccount(Request $request): bool|string
    {
        $api = '/budget/ssvolume/accountlist';
        $query = [
            'subChannelId'  => json_decode($request['subChannelId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListSubAccount(Request $request): bool|string
    {
        $api = '/budget/ssvolume/subaccountlist';
        $query = [
            'accountId'  => json_decode($request['accountId'])
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getListGroupBrand(): bool|string
    {
        $api = '/budget/ssvolume/groupbrandlist';

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function uploadXls(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/ssvolume/upload';
        Log::info('post API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)
                ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                ->post($api, [
                    'name'      => 'formFile',
                    'contents'  => $request->file('file')->getContent()
                ]);

            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return json_encode([
                    'error' => false,
                    'data' => $resVal,
                    'message' => json_decode($response)->message,
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::error($message);
                return array(
                    'error' => true,
                    'message' => "Upload failed"
                );
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload error"
            );
        }
    }

    public function downloadTemplate(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/ssvolume';
        $query = [
            'period'            => $request['period'],
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'region'            => json_decode($request['region']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
            'sort'              => 'ASC',
            'order'             => 'channel'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {

                //Volume
                $resValVolume = json_decode($response)->values->data;
                $resultVolume=[];
                $row = 2;
                foreach ($resValVolume as $fields) {
                    $arr = [];
                    $row++;

                    $arr[] = $fields->period;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->region;
                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->m1;
                    $arr[] = $fields->m2;
                    $arr[] = $fields->m3;
                    $arr[] = $fields->m4;
                    $arr[] = $fields->m5;
                    $arr[] = $fields->m6;
                    $arr[] = $fields->m7;
                    $arr[] = $fields->m8;
                    $arr[] = $fields->m9;
                    $arr[] = $fields->m10;
                    $arr[] = $fields->m11;
                    $arr[] = $fields->m12;
                    $arr[] = '=SUM(H' . $row . ':S' . $row . ')';
                    $arr[] = $fields->rate1;
                    $arr[] = $fields->rate2;
                    $arr[] = $fields->rate3;
                    $arr[] = $fields->rate4;
                    $arr[] = $fields->rate5;
                    $arr[] = $fields->rate6;
                    $arr[] = $fields->rate7;
                    $arr[] = $fields->rate8;
                    $arr[] = $fields->rate9;
                    $arr[] = $fields->rate10;
                    $arr[] = $fields->rate11;
                    $arr[] = $fields->rate12;
                    $arr[] = '=H' . $row . '*U' . $row;
                    $arr[] = '=I' . $row . '*V' . $row;
                    $arr[] = '=J' . $row . '*W' . $row;
                    $arr[] = '=K' . $row . '*X' . $row;
                    $arr[] = '=L' . $row . '*Y' . $row;
                    $arr[] = '=M' . $row . '*Z' . $row;
                    $arr[] = '=N' . $row . '*AA' . $row;
                    $arr[] = '=O' . $row . '*AB' . $row;
                    $arr[] = '=P' . $row . '*AC' . $row;
                    $arr[] = '=Q' . $row . '*AD' . $row;
                    $arr[] = '=R' . $row . '*AE' . $row;
                    $arr[] = '=S' . $row . '*AF' . $row;
                    $arr[] = '=SUM(AG' . $row . ':AR' . $row . ')';

                    $resultVolume[] = $arr;
                }

                // Region
                $resValRegion = json_decode($response)->values->region;
                $resultRegion = [];
                foreach ($resValRegion as $fields) {
                    $arr = [];
                    $arr[] = $fields->longDesc;

                    $resultRegion[] = $arr;
                }


                $filename = 'Template Budget SS Input - ';

                //Volume
                $titleVolume = 'A1:AS1'; //Report Title Bold and merge
                $headerVolume = 'A2:AS2'; //Header Column Bold and color
                $cellNotInput = 'T3:AS'.$row; // Cell Mark Not Input
                $headingVolume = [
                    ['SS Input Upload Template', 'SS in Tons', 'Conversion Rate', 'SS Value', 'SS Value'],
                    ['Periode', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Region', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS in Tons', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS Value']
                ];

                $formatCellVolume =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Q' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'R' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'S' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'T' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'U' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'V' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'W' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'X' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Y' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Z' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AA' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AB' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AC' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AK' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AL' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AN' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AP' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AS' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                //Region
                $headerRegion = 'A1:A1'; //Header Column Bold and color
                $headingRegion = [
                    ['Region']
                ];

                $export = new ExportTemplateBudgetVolume($resultVolume,$headingVolume, $titleVolume, $headerVolume, $formatCellVolume,
                    $resultRegion, $headingRegion, $headerRegion, $cellNotInput
                );

                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportXls(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/ssvolume/list';
        $query = [
            'period'            => $request['period'],
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'region'            => json_decode($request['region']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
            'sort'              => 'ASC',
            'order'             => 'channel'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->period;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->subChannel;
                    $arr[] = $fields->account;
                    $arr[] = $fields->subAccount;
                    $arr[] = $fields->region;
                    $arr[] = $fields->groupBrand;
                    $arr[] = $fields->m1;
                    $arr[] = $fields->m2;
                    $arr[] = $fields->m3;
                    $arr[] = $fields->m4;
                    $arr[] = $fields->m5;
                    $arr[] = $fields->m6;
                    $arr[] = $fields->m7;
                    $arr[] = $fields->m8;
                    $arr[] = $fields->m9;
                    $arr[] = $fields->m10;
                    $arr[] = $fields->m11;
                    $arr[] = $fields->m12;
                    $arr[] = $fields->fy;
                    $arr[] = $fields->rate1;
                    $arr[] = $fields->rate2;
                    $arr[] = $fields->rate3;
                    $arr[] = $fields->rate4;
                    $arr[] = $fields->rate5;
                    $arr[] = $fields->rate6;
                    $arr[] = $fields->rate7;
                    $arr[] = $fields->rate8;
                    $arr[] = $fields->rate9;
                    $arr[] = $fields->rate10;
                    $arr[] = $fields->rate11;
                    $arr[] = $fields->rate12;
                    $arr[] = $fields->value1;
                    $arr[] = $fields->value2;
                    $arr[] = $fields->value3;
                    $arr[] = $fields->value4;
                    $arr[] = $fields->value5;
                    $arr[] = $fields->value6;
                    $arr[] = $fields->value7;
                    $arr[] = $fields->value8;
                    $arr[] = $fields->value9;
                    $arr[] = $fields->value10;
                    $arr[] = $fields->value11;
                    $arr[] = $fields->value12;
                    $arr[] = $fields->valueFY;

                    $result[] = $arr;
                }

                $filename = 'Budget SS Input - ';
                $title = 'A1:AS1'; //Report Title Bold and merge
                $header = 'A10:AS10'; //Header Column Bold and color
                $heading = [
                    ['Period : ' . $request['period'] ],
                    ['Channel : ' . (($request['channelText'] == '') ? 'ALL' : $request['channelText'])],
                    ['Sub Channel : ' . (($request['subChannelText'] == '') ? 'ALL' : $request['subChannelText'])],
                    ['Account : ' . (($request['accountText'] == '') ? 'ALL' : $request['accountText'])],
                    ['Sub Account : ' . (($request['subAccountText'] == '') ? 'ALL' : $request['subAccountText'])],
                    ['Region : ' . (($request['regionText'] == '') ? 'ALL' : $request['regionText'])],
                    ['Brand : ' . (($request['groupBrandText'] == '') ? 'ALL' : $request['groupBrandText'])],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['SS Input', 'SS in Tons', 'Conversion Rate', 'SS Value'],
                    ['Periode', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Region', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS in Tons', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS Value']
                ];

                $formatCell =  [
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Q' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'R' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'S' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'T' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'U' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'V' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'W' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'X' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Y' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'Z' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AA' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AB' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AC' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AK' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AL' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AN' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AP' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AS' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new ExportXlsBudgetVolume($result, $heading, $title, $header, $formatCell);

                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportCsv(Request $request): StreamedResponse|array
    {
        $api = config('app.api'). '/budget/ssvolume/list';
        $query = [
            'period'            => $request['period'],
            'channel'           => json_decode($request['channel']),
            'subChannel'        => json_decode($request['subChannel']),
            'account'           => json_decode($request['account']),
            'subAccount'        => json_decode($request['subAccount']),
            'region'            => json_decode($request['region']),
            'groupBrand'        => json_decode($request['groupBrand']),
            'PageNumber'        => 0,
            'PageSize'          => -1,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;
                $mc = microtime(true);
                $fileName = 'Budget SS Input - ' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columnsHeader = ['', '', '', '', '', '', '', 'SS in Tons', '', '', '', '', '', '', '', '', '', '', '', '',
                    'Conversion Rate', '', '', '', '', '', '', '', '', '', '', '',
                    'SS Value', '', '', '', '', '', '', '', '', '', '', '', ''];
                $columns = ['Period', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Region', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS in Tons',
                    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY SS Value'];

                $callback = function () use ($resVal, $columns, $columnsHeader) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["Budget SS Input"]);

                    fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')]);
                    fputcsv($file, $columnsHeader);
                    fputcsv($file, $columns);

                    foreach ($resVal as $fields) {
                        $row['Period'] = $fields->period;
                        $row['Channel'] = $fields->channel;
                        $row['Sub Channel'] = $fields->subChannel;
                        $row['Account'] = $fields->account;
                        $row['Sub Account'] = $fields->subAccount;
                        $row['Region'] = $fields->region;
                        $row['Brand'] = $fields->groupBrand;
                        $row['Jan'] = $fields->m1;
                        $row['Feb'] = $fields->m2;
                        $row['Mar'] = $fields->m3;
                        $row['Apr'] = $fields->m4;
                        $row['May'] = $fields->m5;
                        $row['Jun'] = $fields->m6;
                        $row['Jul'] = $fields->m7;
                        $row['Aug'] = $fields->m8;
                        $row['Sep'] = $fields->m9;
                        $row['Oct'] = $fields->m10;
                        $row['Nov'] = $fields->m11;
                        $row['Dec'] = $fields->m12;
                        $row['FY SS in Tons'] = $fields->fy;
                        $row['Jan'] = $fields->rate1;
                        $row['Feb'] = $fields->rate2;
                        $row['Mar'] = $fields->rate3;
                        $row['Apr'] = $fields->rate4;
                        $row['May'] = $fields->rate5;
                        $row['Jun'] = $fields->rate6;
                        $row['Jul'] = $fields->rate7;
                        $row['Aug'] = $fields->rate8;
                        $row['Sep'] = $fields->rate9;
                        $row['Oct'] = $fields->rate10;
                        $row['Nov'] = $fields->rate11;
                        $row['Dec'] = $fields->rate12;
                        $row['Jan'] = $fields->value1;
                        $row['Feb'] = $fields->value2;
                        $row['Mar'] = $fields->value3;
                        $row['Apr'] = $fields->value4;
                        $row['May'] = $fields->value5;
                        $row['Jun'] = $fields->value6;
                        $row['Jul'] = $fields->value7;
                        $row['Aug'] = $fields->value8;
                        $row['Sep'] = $fields->value9;
                        $row['Oct'] = $fields->value10;
                        $row['Nov'] = $fields->value11;
                        $row['Dec'] = $fields->value12;
                        $row['FY SS Value'] = $fields->valueFY;

                        fputcsv($file, array(
                            $row['Period'],
                            $row['Channel'],
                            $row['Sub Channel'],
                            $row['Account'],
                            $row['Sub Account'],
                            $row['Region'],
                            $row['Brand'],
                            $row['Jan'],
                            $row['Feb'],
                            $row['Mar'],
                            $row['Apr'],
                            $row['May'],
                            $row['Jun'],
                            $row['Jul'],
                            $row['Aug'],
                            $row['Sep'],
                            $row['Oct'],
                            $row['Nov'],
                            $row['Dec'],
                            $row['FY SS in Tons'],
                            $row['Jan'],
                            $row['Feb'],
                            $row['Mar'],
                            $row['Apr'],
                            $row['May'],
                            $row['Jun'],
                            $row['Jul'],
                            $row['Aug'],
                            $row['Sep'],
                            $row['Oct'],
                            $row['Nov'],
                            $row['Dec'],
                            $row['Jan'],
                            $row['Feb'],
                            $row['Mar'],
                            $row['Apr'],
                            $row['May'],
                            $row['Jun'],
                            $row['Jul'],
                            $row['Aug'],
                            $row['Sep'],
                            $row['Oct'],
                            $row['Nov'],
                            $row['Dec'],
                            $row['FY SS Value'],
                        ));
                    }
                    fclose($file);
                };
                return response()->stream($callback, 200, $headers);
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
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    protected function sendEmailApprover(Request $request): bool|string
    {
        // Get Data Request
        $userApprover = $request['userApprover'];
        $nameApprover = $request['nameApprover'];
        $id = $request['promoId'];
        $email = $request['emailApprover'];
        $subject = $request['subject'];

        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/displayv2email/id';

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
                if ($data->promo->mechanismInputMethod) {
                    $viewEmail = 'Budget::budget-volume.email-approval-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-volume.email-approval-revamp-mechanism-text';
                }

                $attach = array();
                for ($i=0; $i<count($data->attachment); $i++) {
                    if ($data->attachment[$i]->docLink =='row1') $attach['row1'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row2') $attach['row2'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row3') $attach['row3'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row4') $attach['row4'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row5') $attach['row5'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row6') $attach['row6'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row7') $attach['row7'] = $data->attachment[$i]->fileName;
                }
                if (!empty($data->attachment)) array_push($ar_fileattach, $attach);

                $urlid = $data->promo->id;
                $urlrefid = urlencode(MyEncrypt::encrypt($data->promo->refId));
                $yearPromo = date( 'Y', strtotime($data->promo->startPromo));

                $dataUrl = json_encode([
                    'promoId'       => $promoId,
                    'refId'         => $data->promo->refId,
                    'profileId'     => $userApprover,
                    'nameApprover'  => $nameApprover,
                    'sy'            => $yearPromo,
                ]);

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
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }

    protected function sendEmailApproverRecon(Request $request): bool|string
    {
        // Get Data Request
        $userApprover = $request['userApprover'];
        $nameApprover = $request['nameApprover'];
        $id = $request['promoId'];
        $email = $request['emailApprover'];
        $subject = $request['subject'];

        $recon = 0;
        $title = "Promo Display";
        $subtitle = "Promo Id ";
        $promoId = $id;

        $api_promo = config('app.api') . '/promo/displayv2email/id';

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

                if ($data->promo->mechanismInputMethod) {
                    $viewEmail = 'Budget::budget-volume.email-approval-recon-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-volume.email-approval-recon-revamp-mechanism-text';
                }

                $attach = array();
                for ($i=0; $i<count($data->attachment); $i++) {
                    if ($data->attachment[$i]->docLink =='row1') $attach['row1'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row2') $attach['row2'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row3') $attach['row3'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row4') $attach['row4'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row5') $attach['row5'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row6') $attach['row6'] = $data->attachment[$i]->fileName;
                    if ($data->attachment[$i]->docLink =='row7') $attach['row7'] = $data->attachment[$i]->fileName;
                }
                if (!empty($data->attachment)) array_push($ar_fileattach, $attach);

                $urlid = $data->promo->id;
                $urlrefid = urlencode(MyEncrypt::encrypt($data->promo->refId));
                $yearPromo = date( 'Y', strtotime($data->promo->startPromo));

                $dataUrl = json_encode([
                    'promoId'       => $promoId,
                    'refId'         => $data->promo->refId,
                    'profileId'     => $userApprover,
                    'nameApprover'  => $nameApprover,
                    'sy'            => $yearPromo
                ]);

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
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error'     => true,
                'data'      => [],
                'message'   => "error : Send Email To Approver Failed"
            ]);
        }
    }
}
