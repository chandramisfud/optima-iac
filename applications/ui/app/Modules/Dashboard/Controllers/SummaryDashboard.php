<?php

namespace App\Modules\Dashboard\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

class SummaryDashboard extends Controller
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
        $title = "Summary Dashboard";
        return view('Dashboard::summary-dashboard.index', compact('title'));
    }

    public function getDataSummary(Request $request)
    {
        $api = config('app.api'). '/dashboard/summary/kpiscoring/dashboard';
        $query = [
            'create_from'       => $request->create_from,
            'create_to'         => $request->create_to,
            'userid'            => '0',
            'period_monitoring' => (($request->period_monitoring == 1) ? 'true' : 'false'),
            'date_monitoring'   => $request->date_monitoring,
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

    public function getDataCreatorLeaguesSummary(Request $request)
    {
        $api = config('app.api'). '/dashboard/summary/kpiscoring/leagues';
        $query = [
            'create_from'       => $request->create_from,
            'create_to'         => $request->create_to,
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

    public function getDataCreatorStanding(Request $request)
    {
        $api = config('app.api'). '/dashboard/summary/kpiscoring/standing';
        $query = [
            'create_from'       => $request->create_from,
            'create_to'         => $request->create_to,
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

    public function getDataApproverStanding(Request $request)
    {
        $api = config('app.api'). '/dashboard/summary/kpiscoring/approver';
        $query = [
            'promostart'       => $request->create_from,
            'promoend'         => $request->create_to,
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
        $api = config('app.api'). '/dashboard/summary/kpiscoring/summaries';
        $query = [
            'create_from'       => $request->create_from,
            'create_to'         => $request->create_to
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->jobTitle1;
                    $arr[] = $fields->creator;
                    $arr[] = $fields->jobTitle2;

                    $arr[] = $fields->promoPlanSubmittedBfrQuarterStart90_Score;
                    $arr[] = $fields->optimaPeriodMatch_Score;
                    $arr[] = $fields->optimaDescAndMechanismMatch_Score;
                    $arr[] = $fields->optimaAmountMatch_Score;
                    $arr[] = $fields->optimaCreationBrfActivityStart60_Score;

                    $arr[] = $fields->skpPeriodMatch_Score;
                    $arr[] = $fields->skpMechanismMatch_Score;
                    $arr[] = $fields->skpAmountMatch_Score;
                    $arr[] = $fields->skpSigned7_Score;

                    $arr[] = $fields->activityAuditCompliance_Total;
                    $arr[] = $fields->activityAuditCompliance_Score;

                    $arr[] = $fields->reconNKA_Total;
                    $arr[] = $fields->adjustBudgetRecon_Score;
                    $arr[] = $fields->feedbackOnPostROI_Score;
                    $arr[] = $fields->agingDNBySales_Score;
                    $arr[] = $fields->total_Score;

                    $result[] = $arr;
                }

                $filename = 'Summary Scoreboard-';
                $title = 'A1:C1'; //Report Title Bold and merge
                $header = 'A3:S3'; //Header Column Bold and color
                $heading = [
                    ['Scoreboard'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Job Title',
                        'User Name',
                        'Job Title',
                        'Detailed Promo Plan Submitted 90 days before Quarterly Start',
                        'Activity Period Matching',
                        'Activity Desc & Mechanism Matching',
                        'Budget Amount Matching',
                        'Optima & SKP Draft Created H-60 from Period start',
                        'Activity Period Matching',
                        'Activity Mechanism Matching',
                        'Budget Amount Matching',
                        'SKP Signed Provided H-7 from Period Start',
                        'KPI Audit Compliance ( % )',
                        'Score',
                        'Reconcilliation 60 days for NKA, 90 days for Non NKA after promo end',
                        'Adjust Budget in Optima 3 days after reconcillation',
                        'Perform Feedback on Post ROI',
                        'Aging DN after received by Sales : 3 days for DN need approval, 7 days for DN incomplete',
                        'Total']
                ];

                $formatCell =  [

                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' .$mc . '.xlsx');
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

    public function exportXlsDetail(Request $request)
    {
        $api = config('app.api'). '/dashboard/summary/kpiscoring/detail';
        $query = [
            'create_from'       => $request->create_from,
            'create_to'         => $request->create_to,
            'userid'            => '0',
            'period_monitoring' => (($request->period_monitoring == 1) ? 'true' : 'false'),
            'date_monitoring'   => $request->date_monitoring,
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;

                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->period;
                    $arr[] = $fields->promoPlanRefID;
                    $arr[] = $fields->tsCode;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;
                    $arr[] = $fields->mechanisme1;
                    $arr[] = date('d/m/Y' , strtotime($fields->startPlanning));
                    $arr[] = date('d/m/Y' , strtotime($fields->endPlanning));
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->salesInc;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->roi . ' %';
                    $arr[] = $fields->costRatio .' %';
                    $arr[] = $fields->creator;
                    $arr[] = date('d/m/Y' , strtotime($fields->creationDate));
                    $arr[] = $fields->lastStatus;
                    $arr[] = $fields->promoRefId;
                    $arr[] = $fields->cancelNotes;

                    $arr[] = $fields->pic;
                    $arr[] = $fields->statusPromo;
                    $arr[] = $fields->periodStart;

                    $arr[] = $fields->periodEnd;
                    $arr[] = $fields->investmentCreation;
                    $arr[] = date('d/m/Y' , strtotime($fields->promoCreation));

                    $arr[] = $fields->promoPlanCreation;
                    $arr[] = date('d/m/Y' , strtotime($fields->startPromo));
                    $arr[] = date('d/m/Y' , strtotime($fields->endPromo));

                    $arr[] = $fields->promoActivityDesc;
                    $arr[] = $fields->promoPlanSubmittedBfrQuarterStart90;
                    $arr[] = $fields->periodOfStart;

                    $arr[] = $fields->periodOfEnd;
                    $arr[] = $fields->check;
                    $arr[] = $fields->optimaPeriodMatch;

                    $arr[] = $fields->optimaMechanismMatch;
                    $arr[] = $fields->optimaDescMatch;
                    $arr[] = $fields->optimaDescAndMechanismMatch;

                    $arr[] = $fields->optimaAmountMatch;
                    $arr[] = $fields->optimaCreationBrfActivityStart60;

                    $result[] = $arr;
                }

                $filename = 'Summary Scoreboard Detail-';
                $title = 'A1:C1'; //Report Title Bold and merge
                $header = 'A3:AW3'; //Header Column Bold and color
                $heading = [
                    ['Scoreboard-Detail'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Period',
                        'Planning ID',
                        'TS Code',
                        'Entity',
                        'Distributor',
                        'Sub Category',
                        'Activity',
                        'Sub Activity',
                        'Activity Name',
                        'Region',
                        'Channel',
                        'Sub Channel',
                        'Account',
                        'Sub Account',
                        'Brand',
                        'SKU',
                        'Mechanism',
                        'Promo Start',
                        'Promo End',
                        'Baseline Sales',
                        'Sales Increment',
                        'Investment',
                        'ROI',
                        'Cost Ratio',
                        'Initiator',
                        'Create On',
                        'Last Status',
                        'Promo ID',
                        'Cancel Notes',
                        'PIC',
                        'Status in Listing Promo',
                        'Period Start',
                        'Period End',
                        'Investment Creation',
                        'Promo Creation',
                        'Promo Plan Creation ',
                        'Promo Start',
                        'Promo End',
                        'Activity Desc',
                        'Promo Plan Submitted 60 Days Before Period Start',
                        'Period Start',
                        'Period End',
                        'Check',
                        'Activity Period Matching',
                        'Mechanism Matching',
                        'Activity Matching',
                        'Activity & Mechanism Matching',
                        'Budget Amount Matching',
                        'Optima Created H-60',
                    ]
                ];

                $formatCell =  [
                    'T' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'U' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'V' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'W' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'X' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AH' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d His') . ' ' .$mc . '.xlsx');
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
