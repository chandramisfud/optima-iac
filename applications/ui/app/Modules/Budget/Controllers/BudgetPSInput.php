<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Budget\ExportBudgetSSInputPSInput;
use App\Exports\Export;
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

class BudgetPSInput extends Controller
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
        $title = "Budget [PS Input]";
        return view('Budget::budget-ps-input.index', compact('title'));
    }

    public function uploadPage(): Factory|View|Application
    {
        $title = "Budget [PS Input]";
        return view('Budget::budget-ps-input.upload-xls', compact('title'));
    }

    public function getListPaginateFilter(Request $request): bool|string
    {
        $page = 0;
        if ($request['length'] > -1) $page = ($request['start'] / $request['length']);

        $api = '/budget/psinput/list';
        $query = [
            'period'                => (int) $request['period'],
            'distributor'           => json_decode($request['distributor']),
            'groupBrand'            => json_decode($request['groupBrand']),
            'txtSearch'             => ($request['search']['value'] ?? ""),
            'SortColumn'            => $request['columns'][$request['order'][0]['column']]['data'],
            'SortDirection'         => $request['order'][0]['dir'],
            'PageSize'              => $request['length'],
            'PageNumber'            => $page,
        ];
        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $data = json_decode($res)->data->data;
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => $data,
                "recordsTotal" => json_decode($res)->data->totalCount,
                "recordsFiltered" => json_decode($res)->data->filteredCount
            ]);
        } else {
            return $res;
        }
    }

    public function getListFilter(): bool|string
    {
        $api = '/budget/psinput/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function exportXls(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/psinput/list';
        $query = [
            'period'            => (int) $request['period'],
            'distributor'       => json_decode($request['distributor']),
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
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->period;
                    $arr[] = $fields->distributor;
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

                    $result[] = $arr;
                }

                $filename = 'Budget PS Input - ';
                $title = 'A1:P1'; //Report Title Bold and merge
                $header = 'A5:P5'; //Header Column Bold and color
                $heading = [
                    ['Period : ' . $request['period'] ],
                    ['Distributor : ' . (($request['distributorText'] == '') ? 'ALL' : $request['distributorText'])],
                    ['Brand : ' . (($request['groupBrandText'] == '') ? 'ALL' : $request['groupBrandText'])],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Period', 'Distributor', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY']
                ];

                $formatCell =  [
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'E' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);

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
        $api = config('app.api'). '/budget/psinput/list';
        $query = [
            'period'            => (int) $request['period'],
            'distributor'       => json_decode($request['distributor']),
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
                $fileName = 'Budget PS Input - ' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columns = ['Period', 'Distributor', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY'];

                $callback = function () use ($resVal, $columns) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["Budget PS Input"]);

                    fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')]);
                    fputcsv($file, $columns);

                    foreach ($resVal as $fields) {

                        $row['Period'] = $fields->period;
                        $row['Distributor'] = $fields->distributor;
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
                        $row['FY'] = $fields->fy;

                        fputcsv($file, array(
                            $row['Period'],
                            $row['Distributor'],
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
                            $row['FY'] ,
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

    public function downloadTemplate(Request $request): BinaryFileResponse|array
    {
        $api = config('app.api'). '/budget/psinput/list';
        $query = [
            'period'            => (int) $request['period'],
            'distributor'       => json_decode($request['distributor']),
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
                $result=[];
                $row = 1;
                foreach ($resVal as $fields) {
                    $arr = [];

                    $row++;
                    $arr[] = $fields->period;
                    $arr[] = $fields->distributor;
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
                    $arr[] = '=SUM(D' . $row . ':O' . $row . ')';

                    $result[] = $arr;
                }

                $filename = 'Template Budget PS Input - ';
                $header = 'A1:P1'; //Header Column Bold and color
                $heading = [
                    ['Period', 'Distributor', 'Brand', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'FY']
                ];
                $cellNoInput = 'P2:P'.$row;

                $formatCell =  [
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'E' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'L' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'M' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'N' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'O' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'P' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new ExportBudgetSSInputPSInput($result, $heading, $header, $formatCell, $cellNoInput);

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

    public function uploadXls(Request $request): bool|array|string
    {
        $api = config('app.api') . '/budget/psinput/upload';
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
                    $viewEmail = 'Budget::budget-ps-input.email-approval-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-ps-input.email-approval-revamp-mechanism-text';
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
                    $viewEmail = 'Budget::budget-ps-input.email-approval-recon-revamp-mechanism-list';
                } else {
                    $viewEmail = 'Budget::budget-ps-input.email-approval-recon-revamp-mechanism-text';
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
