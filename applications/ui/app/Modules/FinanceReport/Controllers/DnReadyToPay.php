<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use App\Helpers\CallApi;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Symfony\Component\HttpFoundation\BinaryFileResponse;
use Symfony\Component\HttpFoundation\StreamedResponse;

class DnReadyToPay extends Controller
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
        $title = "DN Ready to Pay";
        return view('FinanceReport::dn-ready-to-pay.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): array|bool|string
    {
        try {
            $api = '/finance-report/dnreadytopay';
            $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
            $query = [
                'period'                    => $request['period'],
                'category'                  => $request['categoryId'],
                'entity'                    => $request['entityId'],
                'distributor'               => $request['distributorId'],
                'search'                    => ($request['search']['value'] ?? ""),
                'pageSize'                  => $request['length'],
                'pageNumber'                => $page,
            ];

            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $resVal = json_decode($res)->data->data;
                return json_encode([
                    "draw" => (int)$request['draw'],
                    "data" => $resVal,
                    "recordsTotal" => json_decode($res)->data->totalCount,
                    "recordsFiltered" => json_decode($res)->data->filteredCount
                ]);
            } else {
                return $res;
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

    public function getListFilter(): bool|string
    {
        $api = '/finance-report/dnreadytopay/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function exportXls(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/finance-report/dnreadytopay';
            $query = [
                'period'                    => $request['period'],
                'category'                  => $request['categoryId'],
                'entity'                    => $request['entityId'],
                'distributor'               => $request['distributorId'],
                'search'                    => '',
                'pageNumber'                => 0,
                'pageSize'                  => -1,
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data->data;

                $result = [];

                foreach ($data as $fields) {
                    $arr = [];

                    $arr[] = $fields->distributorDesc;
                    $arr[] = $fields->refId;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->validateByDanoneOn;
                    $arr[] = $fields->feePct / 100;
                    $arr[] = $fields->feeAmount;
                    $arr[] = $fields->dpp;
                    $arr[] = $fields->ppnPct / 100;
                    $arr[] = $fields->ppnAmt;
                    $arr[] = $fields->pphPct / 100;
                    $arr[] = $fields->pphAmt;
                    $arr[] = $fields->intDocNo;
                    $arr[] = $fields->taxLevel;


                    $result[] = $arr;
                }
                $heading = [
                    ['DN Ready to Pay'],
                    ['Period : ' . $request['period']],
                    ['Category : ' . $request['category']],
                    ['Entity : ' . $request['entity']],
                    ['Distributor : ' . $request['distributor']],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Distributor', 'DN Number', 'Promo ID', 'DN Description', 'Last Status', 'Validate by Danone On', 'Fee (%)', 'Fee Amount', 'DPP', 'PPN (%)', 'PPN Amount', 'PPH (%)', 'PPH Amount', 'Internal Doc. Number', 'Tax Level']
                ];
                $formatCell =  [
                    'G' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'H' => NumberFormat::builtInFormatCode(37),
                    'I' => NumberFormat::builtInFormatCode(37),
                    'J' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'K' => NumberFormat::builtInFormatCode(37),
                    'L' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'M' => NumberFormat::builtInFormatCode(37),
                ];
                $title = 'A1:O1'; //Report Title Bold and merge
                $header = 'A7:O7'; //Header Column Bold and color
                $filename = 'DN Ready to Pay - ';
                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportCsv(Request $request): StreamedResponse|bool|array|string
    {
        try {
            $api = '/finance-report/dnreadytopay';
            $query = [
                'period'                    => $request['period'],
                'category'                  => $request['categoryId'],
                'entity'                    => $request['entityId'],
                'distributor'               => $request['distributorId'],
                'search'                    => '',
                'pageNumber'                => 0,
                'pageSize'                  => -1,
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $resVal = json_decode($res)->data->data;

                $mc = microtime(true);
                $fileName = 'DN Ready to Pay - ' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columns = ['Distributor', 'DN Number', 'Promo ID', 'DN Description', 'Last Status', 'Validate by Danone On', 'Fee (%)', 'Fee Amount', 'DPP', 'PPN (%)', 'PPN Amount', 'PPH (%)', 'PPH Amount', 'Internal Doc. Number', 'Tax Level'];

                $period = $request['period'];
                $category = $request['category'];
                $entity = $request['entity'];
                $distributor = $request['distributor'];

                $callback = function () use ($resVal, $columns, $period, $category, $entity, $distributor) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["DN Ready to Pay"]);
                    fputcsv($file, ['Period : ' . $period]);
                    fputcsv($file, ['Category : ' . $category]);
                    fputcsv($file, ['Entity : ' . $entity]);
                    fputcsv($file, ['Distributor : ' . $distributor]);
                    fputcsv($file, ['Date Retrieved : ' . date('d-m-Y')]);
                    fputcsv($file, $columns);

                    foreach ($resVal as $task) {
                        $row['Distributor']             = $task->distributorDesc;
                        $row['DN Number']               = $task->refId;
                        $row['Promo ID']                = $task->promoRefId;
                        $row['DN Description']          = $task->activityDesc;
                        $row['Last Status']             = $task->lastStatus;
                        $row['Validate by Danone On']   = $task->validateByDanoneOn;
                        $row['Fee (%)']                 = $task->feePct;
                        $row['DPP']                     = $task->feeAmount;
                        $row['Fee Amount']              = $task->dpp;
                        $row['PPN (%)']                 = $task->ppnPct;
                        $row['PPN Amount']              = $task->ppnAmt;
                        $row['PPH (%)']                 = $task->pphPct;
                        $row['PPH Amount']              = $task->pphAmt;
                        $row['Internal Doc. Number']    = $task->intDocNo;
                        $row['Tax Level']               = $task->taxLevel;

                        fputcsv($file, array(
                            $row['Distributor'],
                            $row['DN Number'],
                            $row['Promo ID'],
                            $row['DN Description'],
                            $row['Last Status'],
                            $row['Validate by Danone On'],
                            $row['Fee (%)'],
                            $row['DPP'],
                            $row['Fee Amount'],
                            $row['PPN (%)'],
                            $row['PPN Amount'],
                            $row['PPH (%)'],
                            $row['PPH Amount'],
                            $row['Internal Doc. Number'],
                            $row['Tax Level']
                        ));
                    }
                    fclose($file);
                };

                return response()->stream($callback, 200, $headers);
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

}
