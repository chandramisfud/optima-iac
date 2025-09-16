<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\FinanceReport\ExportTTControl;
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

class TTControl extends Controller
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
        $title = "Channel Summary";
        return view('FinanceReport::tt-control.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): array|bool|string
    {
        try {
            $api = '/finance-report/ttcontrolrcdc';
            $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
            $query = [
                'period'                    => $request['period'],
                'categoryId'                => json_decode($request['categoryId']),
                'groupBrandId'              => json_decode($request['groupBrandId']),
                'channelId'                 => json_decode($request['channelId']),
                'subActivityTypeId'         => json_decode($request['subActivityTypeId']),
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
                    "recordsTotal" => json_decode($res)->data->totalCount ?? 0,
                    "recordsFiltered" => json_decode($res)->data->filteredCount ?? 0
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
        $api = '/finance-report/ttcontrolrcdc/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function exportXls(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/finance-report/ttcontrolrcdc';
            $query = [
                'period'                    => $request['period'],
                'categoryId'                => json_decode($request['categoryId']),
                'groupBrandId'              => json_decode($request['groupBrandId']),
                'channelId'                 => json_decode($request['channelId']),
                'subActivityTypeId'         => json_decode($request['subActivityTypeId']),
                'search'                    => '',
                'pageNumber'                => 0,
                'pageSize'                  => -1,
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);

            if (!json_decode($res)->error) {
                $data = json_decode($res)->data->data;
                $groupBrand = json_decode($res)->data->brand;
                $categoryTitleHeader = (count(json_decode($request['categoryId'])) == 1) ? $request['categoryText'] : 'ALL';
                $subActivityTypeTitleHeader = (count(json_decode($request['subActivityTypeId'])) == 0) ? 'ALL' : $request['subActivityTypeText'];

                $result = [];
                $result[] = ['Channel Summary'];
                $arrRowHeader = [];
                $arrRowSubTotal = [];
                $arrRowSubTotalBrand = [];
                $arrRowSubTotalChannel = [];
                $arrRowSubTotalSubAccount = [];
                $arrRowSubTotalActivity = [];
                $startRowCell = 8;
                foreach ($groupBrand as $valGroupBrand) {
                    $result[] = [''];
                    $result[] = ['Period ' . $request['period']];
                    $result[] = ['Category ' . $categoryTitleHeader];
                    $result[] = ['Sub Activity Type ' . $subActivityTypeTitleHeader];
                    $result[] = ['Brand ' . $valGroupBrand];
                    $result[] = ['Channel', 'Sub Channel', 'Account', 'Sub Account', 'FY SS', 'Activity', 'Sub Activity', 'TT Rate', 'FY Budget', 'FY Cost Submitted', 'Returned Balance From Closure', 'Remaining Budget Include Closure', 'DN Claim', 'DN Paid'];
                    array_push($arrRowHeader, 'A'.($startRowCell-1).':N'.($startRowCell-1));

                    foreach ($data as $fields) {
                        if ($valGroupBrand === $fields->brand) {
                            $arr = [];

                            $arr[] = $fields->channelDesc;
                            $arr[] = $fields->subChannelDesc;
                            $arr[] = $fields->accountDesc;
                            $arr[] = $fields->subAccountDesc;
                            $arr[] = $fields->fySs;
                            $arr[] = $fields->activityDesc;
                            $arr[] = $fields->subActivityDesc;
                            $arr[] = $fields->ttRate / 100;
                            $arr[] = $fields->fyTtBudget;
                            $arr[] = $fields->fyTtCostSubmitted;
                            $arr[] = $fields->returnedBalanceFromClosure;
                            $arr[] = $fields->remainingBudgetIncludeClosure;
                            $arr[] = $fields->dnClaim;
                            $arr[] = $fields->dnPaid;

                            if ($fields->isSubTotal) {
                                array_push($arrRowSubTotal, 'A'.$startRowCell.':N'.$startRowCell);
                            }

                            if ($fields->isSubTotal) {
                                if ($fields->tBrand) {
                                    array_push($arrRowSubTotalBrand, 'A'.$startRowCell.':N'.$startRowCell);
                                } else if ($fields->tChannel) {
                                    array_push($arrRowSubTotalChannel, 'A'.$startRowCell.':N'.$startRowCell);
                                } else if ($fields->tSubAccount) {
                                    array_push($arrRowSubTotalSubAccount, 'A'.$startRowCell.':N'.$startRowCell);
                                } else if ($fields->tActivity) {
                                    array_push($arrRowSubTotalActivity, 'A'.$startRowCell.':N'.$startRowCell);
                                }
                            }
                            $startRowCell = $startRowCell + 1;

                            $result[] = $arr;
                        }
                    }
                    $startRowCell = $startRowCell + 6;
                }
                $formatCell =  [
                    'E' => NumberFormat::builtInFormatCode(37),
                    'H' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'I' => NumberFormat::builtInFormatCode(37),
                    'J' => NumberFormat::builtInFormatCode(37),
                    'K' => NumberFormat::builtInFormatCode(37),
                    'L' => NumberFormat::builtInFormatCode(37),
                    'M' => NumberFormat::builtInFormatCode(37),
                    'N' => NumberFormat::builtInFormatCode(37),
                ];
                $filename = 'Channel Summary ' . $categoryTitleHeader;
                $export = new ExportTTControl($result, $formatCell, $arrRowHeader, $arrRowSubTotal, $arrRowSubTotalBrand, $arrRowSubTotalChannel, $arrRowSubTotalSubAccount,$arrRowSubTotalActivity);
                $mc = microtime(true);
                return Excel::download($export, $filename . ' ' . date('Y-m-d His') . ' ' . $mc .  '.xlsx');
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
