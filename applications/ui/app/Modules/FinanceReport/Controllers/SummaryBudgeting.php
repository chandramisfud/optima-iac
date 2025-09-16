<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\FinanceReport\SummaryBudgeting\ExportSummaryBudgeting;
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

class SummaryBudgeting extends Controller
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
        $title = "Summary Budgeting";
        return view('FinanceReport::summary-budgeting.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request): array|bool|string
    {
        try {
            $api = '/finance-report/summarybudget/list';
            $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
            $query = [
                'period'                    => $request['period'],
                'category'                  => $request['categoryId'],
                'grpBrand'                  => json_decode($request['groupBrand']),
                'channel'                   => json_decode($request['channel']),
                'txtSearch'                 => ($request['search']['value'] ?? ""),
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
        $api = '/finance-report/summarybudget/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function exportXls(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/finance-report/summarybudget';
            $query = [
                'period'                    => $request['period'],
                'category'                  => $request['categoryId'],
                'grpBrand'                  => json_decode($request['groupBrand']),
                'channel'                   => json_decode($request['channel']),
                'search'                    => '',
                'pageNumber'                => 0,
                'pageSize'                  => -1,
            ];
            $callApi = new CallApi();
            $callApi->getUsingToken($this->token, $api, $query);
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $dataSummaryDSN = json_decode($res)->data->budgetSign;
                $data = json_decode($res)->data->budgetSummary;
                $dataDC = json_decode($res)->data->budgetSummaryDC;

                $resultSummaryDSN = [];

                //<editor-fold desc="Summary DSN">
                $title = 'TRADE SUPPORT BUDGET ' . $request['period'];
                $titleSummaryDSN = [];
                $titleSummaryDSN[] = '';
                $titleSummaryDSN[] = '';
                $titleSummaryDSN[] = $title;

                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = $titleSummaryDSN;

                $resultSummaryDSN[] = [''];

                foreach ($dataSummaryDSN->dsDesc as $dsDesc) {
                    $dataDsDesc = [];

                    $dataDsDesc[] = '';
                    $dataDsDesc[] = '';
                    $dataDsDesc[] = '';
                    $dataDsDesc[] = '';
                    $dataDsDesc[] = $dsDesc->dsDesc;
                    $dataDsDesc[] = $dsDesc->dsValue;
                    $dataDsDesc[] = '';

                    $resultSummaryDSN[] = $dataDsDesc;
                }

                $resultSummaryDSN[] = [''];

                $titleDsType = [];
                $titleDsType[] = '';
                $titleDsType[] = '';
                $titleDsType[] = '';
                $titleDsType[] = '';
                $titleDsType[] = 'Type';
                $titleDsType[] = 'Budget (BIDR)';
                $titleDsType[] = '% of PS';
                $resultSummaryDSN[] = $titleDsType;

                foreach ($dataSummaryDSN->dsType as $dsType) {
                    $dataDsType = [];

                    $dataDsType[] = '';
                    $dataDsType[] = '';
                    $dataDsType[] = '';
                    $dataDsType[] = '';
                    $dataDsType[] = $dsType->dstype;
                    $dataDsType[] = $dsType->dsBudget;
                    $dataDsType[] = $dsType->dsPctOfPs / 100;

                    $resultSummaryDSN[] = $dataDsType;
                }

                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];

                $titleSign = [];
                $titleSign[] = '';
                $titleSign[] = '';
                $titleSign[] = 'Proposed by';
                $titleSign[] = '';
                $titleSign[] = 'Reviewed by';
                $titleSign[] = '';
                $titleSign[] = 'Approved by';
                $resultSummaryDSN[] = $titleSign;

                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];
                $resultSummaryDSN[] = [''];

                $signName = [];
                $signName[] = '';
                $signName[] = '';
                foreach ($dataSummaryDSN->dsSign as $dsSign) {
                    $signName[] = $dsSign->username;
                    $signName[] = '';
                }
                $resultSummaryDSN[] = $signName;

                $signJobTitle = [];
                $signJobTitle[] = '';
                $signJobTitle[] = '';
                foreach ($dataSummaryDSN->dsSign as $dsSign) {
                    $signJobTitle[] = $dsSign->jobtitle;
                    $signJobTitle[] = '';
                }
                $resultSummaryDSN[] = $signJobTitle;

                $formatColumnSummaryDSN = [
                    'F'     => NumberFormat::builtInFormatCode(39),
                    'G'     => NumberFormat::FORMAT_PERCENTAGE_00,
                ];
                //</editor-fold>

                //<editor-fold desc="Total Summary">
                $result = [];
                $startRowHeader = 2;
                $arrMergeHeader = [];
                $arrRowHeader = [];
                $arrRowSubHeader = [];
                $arrRowSubTotal = [];
                $arrRowGrandTotal = [];

                $result[] = [''];
                foreach ($data as $brand) {
                    $arrBrand = [];

                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = 'Distributor'; // K
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // T
                    $arrBrand[] = ''; // U
                    $arrBrand[] = 'Retailer'; // V
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // AA
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // AD
                    $arrBrand[] = '';
                    $arrBrand[] = 'Trend Spend Deployed';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // AH
                    $arrBrand[] = 'Warchest';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // AK
                    $arrBrand[] = 'Total TS (Deployed + Warchest)';
                    $arrBrand[] = '';

                    $result[] = $arrBrand;
                    array_push($arrRowHeader, $startRowHeader);

                    $arrChannel = [];

                    $arrChannel[] = '';
                    $arrChannel[] = 'Brand';
                    $arrChannel[] = 'Channel';
                    $arrChannel[] = 'Sub Channel';
                    $arrChannel[] = 'Account';
                    $arrChannel[] = 'Sub Account';
                    $arrChannel[] = '';
                    $arrChannel[] = 'SS Volume (tons)';
                    $arrChannel[] = 'PS Volume (tons)';
                    $arrChannel[] = 'SS';
                    $arrChannel[] = 'PS';
                    $arrChannel[] = '';
                    $arrChannel[] = 'KPI'; // M
                    $arrChannel[] = 'KPI %';
                    $arrChannel[] = 'RGA';
                    $arrChannel[] = 'RGA %';
                    $arrChannel[] = 'Transport';
                    $arrChannel[] = 'Transport %';
                    $arrChannel[] = 'Other Cost';
                    $arrChannel[] = 'Other Cost %';
                    $arrChannel[] = 'Total Distributor Cost';
                    $arrChannel[] = 'Total Distributor Cost %'; // V
                    $arrChannel[] = ''; // W
                    $arrChannel[] = 'TT'; // X
                    $arrChannel[] = '% TT to SS';
                    $arrChannel[] = '% TT to PS';
                    $arrChannel[] = 'Adhoc';
                    $arrChannel[] = '% Adhoc to SS';
                    $arrChannel[] = '% Adhoc to PS'; // AC
                    $arrChannel[] = 'TT + Adhoc';
                    $arrChannel[] = '% TT + Adhoc to SS';
                    $arrChannel[] = '% TT + Adhoc to PS'; // AF
                    $arrChannel[] = '';
                    $arrChannel[] = 'Total Trade Spend';
                    $arrChannel[] = '% to PS';
                    $arrChannel[] = ''; // AJ
                    $arrChannel[] = 'Warchest';
                    $arrChannel[] = '% to PS';
                    $arrChannel[] = ''; // AM
                    $arrChannel[] = 'Total TS';
                    $arrChannel[] = '% to PS';

                    $result[] = $arrChannel;
                    array_push($arrRowSubHeader, $startRowHeader + 1);

                    array_push($arrMergeHeader, [
                        'range'         => 'M' . $startRowHeader . ':V'. $startRowHeader,
                        'coordinate'    => 'M' . $startRowHeader,
                        'value'         => 'Distributor'
                    ]);
                    array_push($arrMergeHeader, [
                        'range'         => 'X' . $startRowHeader . ':AF'. $startRowHeader,
                        'coordinate'    => 'X' . $startRowHeader,
                        'value'         => 'Retailer'
                    ]);
                    array_push($arrMergeHeader, [
                        'range'         => 'AH' . $startRowHeader . ':AI'. $startRowHeader,
                        'coordinate'    => 'AH' . $startRowHeader,
                        'value'         => 'Trend Spend Deployed'
                    ]);
                    array_push($arrMergeHeader, [
                        'range'         => 'AK' . $startRowHeader . ':AL'. $startRowHeader,
                        'coordinate'    => 'AK' . $startRowHeader,
                        'value'         => 'Warchest'
                    ]);
                    array_push($arrMergeHeader, [
                        'range'         => 'AN' . $startRowHeader . ':AO'. $startRowHeader,
                        'coordinate'    => 'AN' . $startRowHeader,
                        'value'         => 'Total TS (Deployed + Warchest)'
                    ]);

                    foreach ($brand->channels as $channel) {

                        foreach ($channel->accounts as $account) {
                            $arrAccount = [];

                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->brand;
                            $arrAccount[] = $account->total->channel;
                            $arrAccount[] = $account->total->subChannel;
                            $arrAccount[] = $account->total->account;
                            $arrAccount[] = $account->total->subAccount;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->ssVolumeTons;
                            $arrAccount[] = $account->total->psVolumeTons;
                            $arrAccount[] = $account->total->ss;
                            $arrAccount[] = $account->total->ps;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->kpi;
                            $arrAccount[] = $account->total->kpiPct / 100;
                            $arrAccount[] = $account->total->rga;
                            $arrAccount[] = $account->total->rgaPct / 100;
                            $arrAccount[] = $account->total->transport;
                            $arrAccount[] = $account->total->transportPct / 100;
                            $arrAccount[] = $account->total->otherCost;
                            $arrAccount[] = $account->total->otherCostPct / 100;
                            $arrAccount[] = $account->total->totalDistributorCost;
                            $arrAccount[] = $account->total->totalDistributorCostPct / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->tt;
                            $arrAccount[] = $account->total->pctTtToSs / 100;
                            $arrAccount[] = $account->total->pctTtToPs / 100;
                            $arrAccount[] = $account->total->adhoc;
                            $arrAccount[] = $account->total->pctAdhocToSs / 100;
                            $arrAccount[] = $account->total->pctAdhocToPs / 100;
                            $arrAccount[] = $account->total->ttPlusAdhoc;
                            $arrAccount[] = $account->total->pctTtPlusAdhocTotSs / 100;
                            $arrAccount[] = $account->total->pctTtPlusAdhocTotPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->totalTradeSpend;
                            $arrAccount[] = $account->total->tradeSpendPctToPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->warChest;
                            $arrAccount[] = $account->total->warChestPctToPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->totalTS;
                            $arrAccount[] = $account->total->pctToPs / 100;

                            $result[] = $arrAccount;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->brand;
                            $arrAccount[] = $account->total->channel;
                            $arrAccount[] = $account->total->subChannel;
                            $arrAccount[] = $account->total->account;
                            $arrAccount[] = $account->total->subAccount;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->ssVolumeTons;
                            $arrAccount[] = $account->total->psVolumeTons;
                            $arrAccount[] = $account->total->ss;
                            $arrAccount[] = $account->total->ps;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->kpi;
                            $arrAccount[] = $account->total->kpiPct / 100;
                            $arrAccount[] = $account->total->rga;
                            $arrAccount[] = $account->total->rgaPct / 100;
                            $arrAccount[] = $account->total->transport;
                            $arrAccount[] = $account->total->transportPct / 100;
                            $arrAccount[] = $account->total->otherCost;
                            $arrAccount[] = $account->total->otherCostPct / 100;
                            $arrAccount[] = $account->total->totalDistributorCost;
                            $arrAccount[] = $account->total->totalDistributorCostPct / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->tt;
                            $arrAccount[] = $account->total->pctTtToSs / 100;
                            $arrAccount[] = $account->total->pctTtToPs / 100;
                            $arrAccount[] = $account->total->adhoc;
                            $arrAccount[] = $account->total->pctAdhocToSs / 100;
                            $arrAccount[] = $account->total->pctAdhocToPs / 100;
                            $arrAccount[] = $account->total->ttPlusAdhoc;
                            $arrAccount[] = $account->total->pctTtPlusAdhocTotSs / 100;
                            $arrAccount[] = $account->total->pctTtPlusAdhocTotPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->totalTradeSpend;
                            $arrAccount[] = $account->total->tradeSpendPctToPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->warChest;
                            $arrAccount[] = $account->total->warChestPctToPs / 100;
                            $arrAccount[] = '';
                            $arrAccount[] = $account->total->totalTS;
                            $arrAccount[] = $account->total->pctToPs / 100;

                            $startRowHeader = $startRowHeader + 1;
                        }

                        $arrSubTotal = [];

                        $arrSubTotal[] = '';
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->channel;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->ssVolumeTons;
                        $arrSubTotal[] = $channel->subTotal->psVolumeTons;
                        $arrSubTotal[] = $channel->subTotal->ss;
                        $arrSubTotal[] = $channel->subTotal->ps;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->kpi;
                        $arrSubTotal[] = $channel->subTotal->kpiPct / 100;
                        $arrSubTotal[] = $channel->subTotal->rga;
                        $arrSubTotal[] = $channel->subTotal->rgaPct / 100;
                        $arrSubTotal[] = $channel->subTotal->transport;
                        $arrSubTotal[] = $channel->subTotal->transportPct / 100;
                        $arrSubTotal[] = $channel->subTotal->otherCost;
                        $arrSubTotal[] = $channel->subTotal->otherCostPct / 100;
                        $arrSubTotal[] = $channel->subTotal->totalDistributorCost;
                        $arrSubTotal[] = $channel->subTotal->totalDistributorCostPct / 100;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->tt;
                        $arrSubTotal[] = $channel->subTotal->pctTtToSs / 100;
                        $arrSubTotal[] = $channel->subTotal->pctTtToPs / 100;
                        $arrSubTotal[] = $channel->subTotal->adhoc;
                        $arrSubTotal[] = $channel->subTotal->pctAdhocToSs / 100;
                        $arrSubTotal[] = $channel->subTotal->pctAdhocToPs / 100;
                        $arrSubTotal[] = $channel->subTotal->ttPlusAdhoc;
                        $arrSubTotal[] = $channel->subTotal->pctTtPlusAdhocTotSs / 100;
                        $arrSubTotal[] = $channel->subTotal->pctTtPlusAdhocTotPs / 100;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->totalTradeSpend;
                        $arrSubTotal[] = $channel->subTotal->tradeSpendPctToPs / 100;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->warChest;
                        $arrSubTotal[] = $channel->subTotal->warChestPctToPs / 100;
                        $arrSubTotal[] = '';
                        $arrSubTotal[] = $channel->subTotal->totalTS;
                        $arrSubTotal[] = $channel->subTotal->pctToPs / 100;


                        $result[] = $arrSubTotal;
                        $startRowHeader = $startRowHeader + 1;
                        array_push($arrRowSubTotal, $startRowHeader + 1);
                    }

                    $arrGrandTotal = [];

                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = 'Grand Total';
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->ssVolumeTons;
                    $arrGrandTotal[] = $brand->grandTotal->psVolumeTons;
                    $arrGrandTotal[] = $brand->grandTotal->ss;
                    $arrGrandTotal[] = $brand->grandTotal->ps;
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->kpi;
                    $arrGrandTotal[] = $brand->grandTotal->kpiPct / 100;
                    $arrGrandTotal[] = $brand->grandTotal->rga;
                    $arrGrandTotal[] = $brand->grandTotal->rgaPct / 100;
                    $arrGrandTotal[] = $brand->grandTotal->transport;
                    $arrGrandTotal[] = $brand->grandTotal->transportPct / 100;
                    $arrGrandTotal[] = $brand->grandTotal->otherCost;
                    $arrGrandTotal[] = $brand->grandTotal->otherCostPct / 100;
                    $arrGrandTotal[] = $brand->grandTotal->totalDistributorCost;
                    $arrGrandTotal[] = $brand->grandTotal->totalDistributorCostPct / 100;
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->tt;
                    $arrGrandTotal[] = $brand->grandTotal->pctTtToSs / 100;
                    $arrGrandTotal[] = $brand->grandTotal->pctTtToPs / 100;
                    $arrGrandTotal[] = $brand->grandTotal->adhoc;
                    $arrGrandTotal[] = $brand->grandTotal->pctAdhocToSs / 100;
                    $arrGrandTotal[] = $brand->grandTotal->pctAdhocToPs / 100;
                    $arrGrandTotal[] = $brand->grandTotal->ttPlusAdhoc;
                    $arrGrandTotal[] = $brand->grandTotal->pctTtPlusAdhocTotSs / 100;
                    $arrGrandTotal[] = $brand->grandTotal->pctTtPlusAdhocTotPs / 100;
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->totalTradeSpend;
                    $arrGrandTotal[] = $brand->grandTotal->tradeSpendPctToPs / 100;
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->warChest;
                    $arrGrandTotal[] = $brand->grandTotal->warChestPctToPs / 100;
                    $arrGrandTotal[] = '';
                    $arrGrandTotal[] = $brand->grandTotal->totalTS;
                    $arrGrandTotal[] = $brand->grandTotal->pctToPs / 100;

                    $result[] = $arrGrandTotal;
                    $startRowHeader = $startRowHeader + 1;
                    array_push($arrRowGrandTotal, $startRowHeader + 1);

                    $startRowHeader = $startRowHeader + 1;
                    $result[] = [''];
                    $startRowHeader = $startRowHeader + 2;
                }

                $formatColumn = [
                    'H'     => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I'     => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'J'     => NumberFormat::builtInFormatCode(37),
                    'K'     => NumberFormat::builtInFormatCode(37),
                    'M'     => NumberFormat::builtInFormatCode(37),
                    'N'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'O'     => NumberFormat::builtInFormatCode(37),
                    'P'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'Q'     => NumberFormat::builtInFormatCode(37),
                    'R'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'S'     => NumberFormat::builtInFormatCode(37),
                    'T'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'U'     => NumberFormat::builtInFormatCode(37),
                    'V'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'X'     => NumberFormat::builtInFormatCode(37),
                    'Y'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'Z'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AA'     => NumberFormat::builtInFormatCode(37),
                    'AB'     => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AC'    => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AD'    => NumberFormat::builtInFormatCode(37),
                    'AE'    => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AF'    => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AH'    => NumberFormat::builtInFormatCode(37),
                    'AI'    => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AK'    => NumberFormat::builtInFormatCode(37),
                    'AL'    => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AN'    => NumberFormat::builtInFormatCode(37),
                    'AO'    => NumberFormat::FORMAT_PERCENTAGE_00,
                ];
                //</editor-fold>

                //<editor-fold desc="DC Summary">
                $startRowHeaderDCSummary = 2;
                $resultDCSummary = [];
                $arrMergeHeaderDCSummary = [];
                $arrRowHeaderDCSummary = [];
                $arrRowSubHeaderDCSummary = [];
                $arrRowTotalDCSummary = [];
                $resultDCSummary[] = [''];
                foreach ($dataDC as $brand) {
                    $arrBrand = [];

                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = 'Running Rate'; // I
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // N
                    $arrBrand[] = 'Non Running Rate'; // O
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // T
                    $arrBrand[] = 'Total DC'; // U
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = '';
                    $arrBrand[] = ''; // Y

                    $resultDCSummary[] = $arrBrand;
                    array_push($arrRowHeaderDCSummary, $startRowHeaderDCSummary);

                    $arrSubActivityType = [];

                    $arrSubActivityType[] = '';
                    $arrSubActivityType[] = 'Brand';
                    $arrSubActivityType[] = 'Sub Activity Type';
                    $arrSubActivityType[] = '';
                    $arrSubActivityType[] = 'SS Volume (tons)';
                    $arrSubActivityType[] = 'SS Value';
                    $arrSubActivityType[] = 'PS Value';
                    $arrSubActivityType[] = '';
                    $arrSubActivityType[] = 'TRS';
                    $arrSubActivityType[] = 'PTT';
                    $arrSubActivityType[] = 'APL';
                    $arrSubActivityType[] = 'Aladdin';
                    $arrSubActivityType[] = 'Total';
                    $arrSubActivityType[] = '';
                    $arrSubActivityType[] = 'TRS';
                    $arrSubActivityType[] = 'PTT';
                    $arrSubActivityType[] = 'APL';
                    $arrSubActivityType[] = 'Aladdin';
                    $arrSubActivityType[] = 'Total';
                    $arrSubActivityType[] = '';
                    $arrSubActivityType[] = 'TRS';
                    $arrSubActivityType[] = 'PTT';
                    $arrSubActivityType[] = 'APL';
                    $arrSubActivityType[] = 'Aladdin';
                    $arrSubActivityType[] = 'Total';

                    $resultDCSummary[] = $arrSubActivityType;
                    array_push($arrRowSubHeaderDCSummary, $startRowHeaderDCSummary + 1);

                    array_push($arrMergeHeaderDCSummary, [
                        'range'         => 'I' . $startRowHeaderDCSummary . ':M'. $startRowHeaderDCSummary,
                        'coordinate'    => 'I' . $startRowHeaderDCSummary,
                        'value'         => 'Running Rate'
                    ]);
                    array_push($arrMergeHeaderDCSummary, [
                        'range'         => 'O' . $startRowHeaderDCSummary . ':S'. $startRowHeaderDCSummary,
                        'coordinate'    => 'O' . $startRowHeaderDCSummary,
                        'value'         => 'Non Running Rate'
                    ]);
                    array_push($arrMergeHeaderDCSummary, [
                        'range'         => 'U' . $startRowHeaderDCSummary . ':Y'. $startRowHeaderDCSummary,
                        'coordinate'    => 'U' . $startRowHeaderDCSummary,
                        'value'         => 'Total DC'
                    ]);

                    foreach ($brand->subActivityType as $subActivityType) {
                        $arrSubActivityType = [];

                        $arrSubActivityType[] = '';
                        $arrSubActivityType[] = $subActivityType->subTotal->brand;
                        $arrSubActivityType[] = $subActivityType->subTotal->subActivityType;
                        $arrSubActivityType[] = '';
                        $arrSubActivityType[] = $subActivityType->subTotal->ssVolumeTons;
                        $arrSubActivityType[] = $subActivityType->subTotal->ss;
                        $arrSubActivityType[] = $subActivityType->subTotal->ps;
                        $arrSubActivityType[] = '';
                        $arrSubActivityType[] = $subActivityType->subTotal->rrTrs;
                        $arrSubActivityType[] = $subActivityType->subTotal->rrPtt;
                        $arrSubActivityType[] = $subActivityType->subTotal->rrApl;
                        $arrSubActivityType[] = $subActivityType->subTotal->rrAld;
                        $arrSubActivityType[] = $subActivityType->subTotal->rrTotal;
                        $arrSubActivityType[] = '';
                        $arrSubActivityType[] = $subActivityType->subTotal->nrTrs;
                        $arrSubActivityType[] = $subActivityType->subTotal->nrPtt;
                        $arrSubActivityType[] = $subActivityType->subTotal->nrApl;
                        $arrSubActivityType[] = $subActivityType->subTotal->nrAld;
                        $arrSubActivityType[] = $subActivityType->subTotal->nrTotal;
                        $arrSubActivityType[] = '';
                        $arrSubActivityType[] = $subActivityType->subTotal->totTrs;
                        $arrSubActivityType[] = $subActivityType->subTotal->totPtt;
                        $arrSubActivityType[] = $subActivityType->subTotal->totApl;
                        $arrSubActivityType[] = $subActivityType->subTotal->totAld;
                        $arrSubActivityType[] = $subActivityType->subTotal->totTotal;

                        $resultDCSummary[] = $arrSubActivityType;
                        $startRowHeaderDCSummary = $startRowHeaderDCSummary + 1;
                    }

                    $arrTotal = [];

                    $arrTotal[] = '';
                    $arrTotal[] = '';
                    $arrTotal[] = 'Total';
                    $arrTotal[] = '';
                    $arrTotal[] = $brand->grandTotal->ssVolumeTons;
                    $arrTotal[] = $brand->grandTotal->ss;
                    $arrTotal[] = $brand->grandTotal->ps;
                    $arrTotal[] = '';
                    $arrTotal[] = $brand->grandTotal->rrTrs;
                    $arrTotal[] = $brand->grandTotal->rrPtt;
                    $arrTotal[] = $brand->grandTotal->rrApl;
                    $arrTotal[] = $brand->grandTotal->rrAld;
                    $arrTotal[] = $brand->grandTotal->rrTotal;
                    $arrTotal[] = '';
                    $arrTotal[] = $brand->grandTotal->nrTrs;
                    $arrTotal[] = $brand->grandTotal->nrPtt;
                    $arrTotal[] = $brand->grandTotal->nrApl;
                    $arrTotal[] = $brand->grandTotal->nrAld;
                    $arrTotal[] = $brand->grandTotal->nrTotal;
                    $arrTotal[] = '';
                    $arrTotal[] = $brand->grandTotal->totTrs;
                    $arrTotal[] = $brand->grandTotal->totPtt;
                    $arrTotal[] = $brand->grandTotal->totApl;
                    $arrTotal[] = $brand->grandTotal->totAld;
                    $arrTotal[] = $brand->grandTotal->totTotal;

                    $resultDCSummary[] = $arrTotal;
                    $startRowHeaderDCSummary = $startRowHeaderDCSummary + 1;
                    array_push($arrRowTotalDCSummary, $startRowHeaderDCSummary + 1);

                    $startRowHeaderDCSummary = $startRowHeaderDCSummary + 1;
                    $resultDCSummary[] = [''];
                    $startRowHeaderDCSummary = $startRowHeaderDCSummary + 2;
                }

                $formatColumnDCSummary = [
                    'E'     => NumberFormat::builtInFormatCode(37),
                    'F'     => NumberFormat::builtInFormatCode(37),
                    'G'     => NumberFormat::builtInFormatCode(37),
                    'H'     => NumberFormat::builtInFormatCode(37),
                    'I'     => NumberFormat::builtInFormatCode(37),
                    'J'     => NumberFormat::builtInFormatCode(37),
                    'K'     => NumberFormat::builtInFormatCode(37),
                    'L'     => NumberFormat::builtInFormatCode(37),
                    'M'     => NumberFormat::builtInFormatCode(37),
                    'O'     => NumberFormat::builtInFormatCode(37),
                    'P'     => NumberFormat::builtInFormatCode(37),
                    'Q'     => NumberFormat::builtInFormatCode(37),
                    'R'     => NumberFormat::builtInFormatCode(37),
                    'S'     => NumberFormat::builtInFormatCode(37),
                    'U'     => NumberFormat::builtInFormatCode(37),
                    'V'     => NumberFormat::builtInFormatCode(37),
                    'W'     => NumberFormat::builtInFormatCode(37),
                    'X'     => NumberFormat::builtInFormatCode(37),
                    'Y'     => NumberFormat::builtInFormatCode(37),
                ];
                //</editor-fold>

                $filename = 'Summary Budgeting';
                $export = new ExportSummaryBudgeting($resultSummaryDSN, $result, $resultDCSummary,
                    $title, $formatColumnSummaryDSN,
                    $arrMergeHeader, $arrRowHeader, $arrRowSubHeader, $arrRowSubTotal, $arrRowGrandTotal, $formatColumn,
                    $arrMergeHeaderDCSummary, $arrRowHeaderDCSummary, $arrRowSubHeaderDCSummary, $arrRowTotalDCSummary, $formatColumnDCSummary
                );
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
