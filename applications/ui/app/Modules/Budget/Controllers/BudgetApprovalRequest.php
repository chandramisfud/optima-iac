<?php

namespace App\Modules\Budget\Controllers;

use App\Exports\Budget\ApprovalRequest\ExportApprovalRequestPromo;
use App\Exports\Budget\ApprovalRequest\ExportBudgetApprovalRequest;
use App\Exports\Budget\ApprovalRequest\ExportBudgetApprovalRequestDC;
use App\Helpers\CallApi;
use App\Helpers\Formatted;
use Barryvdh\Snappy\Facades\SnappyPdf;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Symfony\Component\HttpFoundation\BinaryFileResponse;

class BudgetApprovalRequest extends Controller
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
        $title = "Budget Mass Approval";
        return view('Budget::budget-approval-request.index', compact('title'));
    }

    public function getListFilter(): bool|string
    {
        $api = '/budget/approvalrequest/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getListSummary(Request $request): array
    {
        try {
            $api = '/budget/approvalrequest/list';
            $query = [
                'period'                => $request['period'],
                'month'                 => json_decode($request['month']),
                'channelId'             => json_decode($request['channelId']),
                'groupBrand'            => json_decode($request['groupBrand']),
                'is5Bio'                => $request['is5Bio'],
                'budgetApprovalStatus'  => json_decode($request['budgetApprovalStatus']),
                'category'              => json_decode($request['categoryId']),
                'Search'                => '',
                'PageSize'              => -1,
                'PageNumber'            => 0,
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                return [
                    'error'     => false,
                    'data'      => json_decode($res)->data->budgetApproval
                ];
            } else {
                return [
                    'error'     => false,
                    'data'      => [],
                    'message'   => json_decode($res)->message
                ];
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ];
        }
    }

    public function getListDetail(Request $request): bool|string
    {
        $api = '/budget/approvalrequest/list';
        $page = ($request['length'] > -1 ? ($request['start'] / $request['length']) : 0);
        $query = [
            'period'                => $request['period'],
            'month'                 => json_decode($request['month']),
            'channelId'             => json_decode($request['channelId']),
            'groupBrand'            => json_decode($request['groupBrand']),
            'is5Bio'                => $request['is5Bio'],
            'budgetApprovalStatus'  => json_decode($request['budgetApprovalStatus']),
            'category'              => json_decode($request['categoryId']),
            'txtSearch'             => ($request['search']['value'] ?? ""),
            'order'                 => $request['columns'][$request['order'][0]['column']]['data'],
            'sort'                  => $request['order'][0]['dir'],
            'PageSize'              => $request['length'],
            'PageNumber'            => $page,
        ];
        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $resVal = json_decode($res)->data->budgetApprovalDetail->data;
            return json_encode([
                "draw" => (int)$request['draw'],
                "data" => $resVal,
                "recordsTotal" => json_decode($res)->data->budgetApprovalDetail->totalCount ?? 0,
                "recordsFiltered" => json_decode($res)->data->budgetApprovalDetail->filteredCount ?? 0
            ]);
        } else {
            return $res;
        }
    }

    public function checkBatchId(Request $request): array
    {
        try {
            $api = '/budget/approvalrequest/batchid';
            $query = [
                'batchId' => $request['batchId'],
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;
                if (isset($data->period)) {
                    return [
                        'error'     => false,
                        'data'      => $data,
                        'message'   => 'Batch Id is exist'
                    ];
                } else {
                    return [
                        'error'     => true,
                        'message'   => 'Data not found'
                    ];
                }
            } else {
                return [
                    'error'     => true,
                    'message'   => 'Batch Id is not exist'
                ];
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ];
        }
    }

    public function downloadPdfAbove(Request $request): Response
    {
        try {
            $api = '/budget/approvalrequest/batchid';
            $query = [
                'batchId' => $request['batchId'],
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            $fileNameAbove = 'Approval Sheet Above 5 bio.pdf';
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;
                return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-above-five-bio', compact('data'))
                    ->setPaper('A4')
                    ->setOrientation('landscape')
                    ->setOption('margin-top',2)
                    ->setOption('margin-bottom',2)
                    ->setOption('margin-left',2)
                    ->setOption('margin-right',2)
                    ->setOption("footer-center", "Page [page] from [toPage]")
                    ->setOption('footer-font-size', 6)
                    ->download($fileNameAbove);
            } else {
                $title = $fileNameAbove;
                return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-error-data', compact('title'))
                    ->setPaper('A4')
                    ->setOrientation('landscape')
                    ->setOption('margin-top',2)
                    ->setOption('margin-bottom',2)
                    ->setOption('margin-left',2)
                    ->setOption('margin-right',2)
                    ->download($fileNameAbove);
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            $title = 'Approval Sheet Above 5 bio.pdf';
            return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-error-data', compact('title'))
                ->setPaper('A4')
                ->setOrientation('landscape')
                ->setOption('margin-top',2)
                ->setOption('margin-bottom',2)
                ->setOption('margin-left',2)
                ->setOption('margin-right',2)
                ->download($title);
        }
    }

    public function downloadPdfBelow(Request $request): Response
    {
        try {
            $api = '/budget/approvalrequest/batchid';
            $query = [
                'batchId' => $request['batchId'],
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            $fileNameBelow = 'Approval Sheet Below 5 bio.pdf';
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;
                if ($request['category'] === "DC") {
                    return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-below-five-bio-dc', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->download($fileNameBelow);
                } else {
                    return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-below-five-bio', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->download($fileNameBelow);
                }
            } else {
                $title = $fileNameBelow;
                return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-error-data', compact('title'))
                    ->setPaper('A4')
                    ->setOrientation('landscape')
                    ->setOption('margin-top',2)
                    ->setOption('margin-bottom',2)
                    ->setOption('margin-left',2)
                    ->setOption('margin-right',2)
                    ->download($fileNameBelow);
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            $title = 'Approval Sheet Below 5 bio.pdf';
            return SnappyPdf::loadView('Budget::budget-approval-request.export-pdf-error-data', compact('title'))
                ->setPaper('A4')
                ->setOrientation('landscape')
                ->setOption('margin-top',2)
                ->setOption('margin-bottom',2)
                ->setOption('margin-left',2)
                ->setOption('margin-right',2)
                ->download($title);
        }
    }

    public function downloadExcel(Request $request): BinaryFileResponse|bool|array|string
    {
        try {
            $api = '/budget/approvalrequest/download';
            $query = [
                'period'                => $request['period'],
                'month'                 => json_decode($request['month']),
                'channelId'             => json_decode($request['channelId']),
                'groupBrand'            => json_decode($request['groupBrand']),
                'budgetApprovalStatus'  => json_decode($request['budgetApprovalStatus']),
                'category'              => json_decode($request['categoryId']),
                'is5Bio'                => $request['fiveBio'],
            ];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;

                $resultDetails=[];
                foreach ($data as $fields) {
                    $arr = [];
                    $arr[] = $fields->period;
                    $arr[] = $fields->promoId;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->activityName;
                    $arr[] = $fields->channel;
                    $arr[] = $fields->account;
                    $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                    $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investment5bio;
                    $arr[] = $fields->brand;
                    $arr[] = $fields->tradingTermAdhoc;
                    $arr[] = $fields->monthStart;
                    $arr[] = $fields->monthEnd;
                    $arr[] = $fields->statusDesc;
                    $arr[] = ($fields->deployed ? 'Deployed' : ($fields->approved ? 'Waiting to Deployed' : ''));
                    $arr[] = ($fields->approvalRequestOn ? date('d-m-Y', strtotime($fields->approvalRequestOn)) : '');
                    $arr[] = $fields->approvalRequestBy;
                    $arr[] = ($fields->approved1On ? date('d-m-Y', strtotime($fields->approved1On)) : '');
                    $arr[] = $fields->approved1By;
                    $arr[] = ($fields->approved2On ? date('d-m-Y', strtotime($fields->approved2On)) : '');
                    $arr[] = $fields->approved2By;
                    $arr[] = ($fields->approved3On ? date('d-m-Y', strtotime($fields->approved3On)) : '');
                    $arr[] = $fields->approved3By;
                    $arr[] = ($fields->approved4On ? date('d-m-Y', strtotime($fields->approved4On)) : '');
                    $arr[] = $fields->approved4By;
                    $arr[] = ($fields->approved5On ? date('d-m-Y', strtotime($fields->approved5On)) : '');
                    $arr[] = $fields->approved5By;
                    $arr[] = ($fields->rejectedOn ? date('d-m-Y', strtotime($fields->rejectedOn)) : '');
                    $arr[] = $fields->rejectedBy;

                    $resultDetails[] = $arr;
                }

                $fileNameExcel = 'Approval Sheet Details '. date('Y-m-d His') . ' ' . microtime(true) .  '.xlsx';
                $headerDetails = [
                    ['Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost', 'Cost >5bio', 'Brand', 'Trading Term / Adhoc', 'Month Start', 'Month End'
                        , 'Status', 'Status Deployed', 'Approval Request On', 'Approval Request By', 'Approved 1 On', 'Approved 1 By', 'Approved 2 On', 'Approved 2 By'
                        , 'Approved 3 On', 'Approved 3 By', 'Approved 4 On', 'Approved 4 By', 'Approved 5 On', 'Approved 5 By', 'Rejected On', 'Rejected By']
                ];
                $headerStyleDetails = 'A1:AF1';
                $formatCellDetails = [
                    'K'     => NumberFormat::builtInFormatCode(37)
                ];

                $export = new ExportApprovalRequestPromo($resultDetails, $headerDetails, $headerStyleDetails, $formatCellDetails);
                return Excel::download($export, $fileNameExcel);
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

    public function sendEmailRequest(Request $request): Response|bool|array|string
    {
        try {
            $api = '/budget/approvalrequest/data';
            $query = [
                'period'        => $request['period'],
                'month'         => json_decode($request['month']),
                'channelId'     => json_decode($request['channelId']),
                'groupBrand'    => json_decode($request['groupBrand']),
                'category'      => $request['categoryId'],
                'is5Bio'        => $request['fiveBio'],
            ];
            $period = $request['period'];
            $callApi = new CallApi();
            $res = $callApi->getUsingToken($this->token, $api, $query);
            if (!json_decode($res)->error) {
                $data = json_decode($res)->data;
                $data->period = $request['period'];
                $batchId = $data->batchId;
                $periodDesc = ($data->periodDesc ?? '');


                $path = 'assets/media/budget/approval-request/email/' . $batchId . '/';
                if (!Storage::disk('optima')->exists($path)) {
                    Storage::disk('optima')->makeDirectory($path);
                }
                $uuid = date('Y-m-d His') . ' ' . microtime(true);

                if ($request['categoryDesc'] === 'DC') {
                    //<editor-fold desc="Generate PDF Above 5 bio">
                    $fileNameAbove = 'Approval Sheet Above 5 bio (PDF)';
                    SnappyPdf::loadView('Budget::budget-approval-request.pdf-above-five-bio-dc', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->setOption("footer-center", "Page [page] from [toPage]")
                        ->setOption('footer-font-size', 6)
                        ->save($path . $fileNameAbove . ' ' . $uuid . '.pdf');
                    //</editor-fold>

                    //<editor-fold desc="Generate PDF Below 5 bio">
                    $fileNameBelow = 'Approval Sheet Below 5 bio (PDF)';
                    SnappyPdf::loadView('Budget::budget-approval-request.pdf-below-five-bio-dc', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->save($path . $fileNameBelow  . ' ' . $uuid .'.pdf');
                    //</editor-fold>

                    //<editor-fold desc="Generate Excel">
                    //<editor-fold desc="Above">
                    $resultAbove=[];
                    $startRowAbove = 7;
                    foreach ($data->budgetOver5Bio as $fields) {
                        $arr = [];
                        $arr[] = '';
                        $arr[] = $fields->period;
                        $arr[] = $fields->promoId;
                        $arr[] = $fields->entity;
                        $arr[] = $fields->distributor;
                        $arr[] = $fields->activity;
                        $arr[] = $fields->activityName;
                        $arr[] = $fields->channel;
                        $arr[] = $fields->account;
                        $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                        $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                        $arr[] = $fields->investment;

                        $startRowAbove++;
                        $resultAbove[] = $arr;
                    }

                    $centerTextAbove = 'A' . $startRowAbove;
                    for ($i=0; $i<17; $i++) {
                        $arr = [];

                        if ($i === 6) {
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = 'Approved by,';
                        } else if ($i === 11) {
                            $signatureData = $data->emailApprovalSigned;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                    $arr[] = ($signatureData[3]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[3]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[4]->approvedOn)) : "");
                                }
                            } else {
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            }
                        } else if ($i === 15) {
                            $signatureData = $data->emailApproval;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = ($signatureData[2]->username ?? '');
                                $arr[] = ($signatureData[3]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[4]->username ?? '');
                            } else {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = ($signatureData[2]->username ?? '');
                                $arr[] = '';
                                $arr[] = '';
                            }
                        } else if ($i === 16) {
                            $signatureData = $data->emailApproval;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                                $arr[] = ($signatureData[3]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[4]->jobtitle ?? '');
                            } else {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = '';
                            }
                        } else {
                            $arr[] = '';
                        }

                        $startRowAbove = $startRowAbove + 1;
                        $resultAbove[] = $arr;
                    }
                    $centerTextAbove = $centerTextAbove . ':L' . $startRowAbove;

                    $headerAbove = [
                        [''],
                        [''],
                        ['', 'APPROVAL SHEET'],
                        ['', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Above IDR 5bio)'],
                        [''],
                        ['', 'Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost']
                    ];
                    $headerStyleAbove = 'B6:L6'; //Header Column Bold and color
                    $formatCellAbove = [
                        'L'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    //<editor-fold desc="Below">
                    $resultBelow=[];
                    $arrSubHeaderMerged = [];
                    $arrHeaderFill = [];
                    $arrHeaderBorder = [];
                    $arrBodyBorder = [];
                    $arrFooterBorder = [];
                    $startRowSubHeaderMerged = 4;
                    $formatted = new Formatted();

                    //<editor-fold desc="All">
                    //<editor-fold desc="Header All">
                    $arrHeaderAllBelow = [];
                    $arrHeaderAllBelow[] = '';
                    $arrHeaderAllBelow[] = '';
                    $arrHeaderAllBelow[] = '';
                    $startColumnNumberOfAlphabet = 5;
                    $startColumnNumberOfAlphabetFill = 4;
                    $startColumnNumberOfAlphabetBorder = 5;
                    foreach ($data->budgetBellow5BioAll->header as $fields) {
                        $arrHeaderAllBelow[] = $fields->headerName;
                        if ($fields->headerName !== '') {
                            $dataHeaderMerged = [
                                'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                'value'         =>  $fields->headerName
                            ];
                            array_push($arrSubHeaderMerged, $dataHeaderMerged);
                            $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                            $arrHeaderAllBelow[] = '';

                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                        }
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }

                    $arrHeaderAllBelow[] = 'Total Count of Promo ID';
                    $arrHeaderAllBelow[] = 'Sum of Total Cost';
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);

                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                        'value'         => 'Total Count of Promo ID'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                        'value'         => 'Sum of Total Cost'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);

                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrHeaderAllBelow;
                    //</editor-fold>

                    //<editor-fold desc="Sub Header All">
                    $arrSubHeaderAllBelow = [];
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = 'Distributor';
                    $startColumnNumberOfAlphabetFill = 4;

                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    foreach ($data->budgetBellow5BioAll->subHeader[0]->valueSubHeader as $fields) {
                        $arrSubHeaderAllBelow[] = $fields->headerName;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrSubHeaderAllBelow;
                    //</editor-fold>

                    //<editor-fold desc="Body All">
                    foreach ($data->budgetBellow5BioAll->body as $body) {
                        $startColumnNumberOfAlphabetBodyBorder = 4;
                        $arrBodyAllBelow = [];
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = $body->text;
                        array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                        foreach ($body->value as $value) {
                            $arrBodyAllBelow[] = $value->value;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrBodyAllBelow;
                    }
                    //</editor-fold>

                    //<editor-fold desc="Footer All">
                    $startColumnNumberOfAlphabetFooterBorder = 4;
                    foreach ($data->budgetBellow5BioAll->footer as $footer) {
                        $arrFooterAllBelow = [];
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = $footer->text;
                        array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        foreach ($footer->value as $value) {
                            $arrFooterAllBelow[] = $value->value;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrFooterAllBelow;
                    }
                    //</editor-fold>

                    for ($i=0; $i<3; $i++) {
                        $arrSeparatorAll = [];
                        $arrSeparatorAll[] = '';

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSeparatorAll;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    //</editor-fold>

                    $centerTextBelow = 'A' . $startRowSubHeaderMerged;
                    for ($i=0; $i<17; $i++) {
                        $arr = [];

                        if ($i === 6) {
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = 'Approved by,';
                        } else if ($i === 11) {
                            $signatureData = $data->emailApprovalSigned;
                            $arr[] = '';
                            if ($signatureData) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                            }
                        } else if ($i === 15) {
                            $signatureData = $data->emailApproval;
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = ($signatureData[0]->username ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[1]->username ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[2]->username ?? '');
                        } else if ($i === 16) {
                            $signatureData = $data->emailApproval;
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = ($signatureData[0]->jobtitle ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[1]->jobtitle ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[2]->jobtitle ?? '');
                        } else {
                            $arr[] = '';
                        }


                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arr;
                    }
                    $centerTextBelow = $centerTextBelow . ':L' . $startRowSubHeaderMerged;

                    $headerBelow = [
                        ['', '', '', 'APPROVAL SHEET'],
                        ['', '', '', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Below IDR 5bio)'],
                        ['']
                    ];
                    $formatCellBelow = [
                        'E'     => NumberFormat::builtInFormatCode(37),
                        'F'     => NumberFormat::builtInFormatCode(37),
                        'G'     => NumberFormat::builtInFormatCode(37),
                        'H'     => NumberFormat::builtInFormatCode(37),
                        'J'     => NumberFormat::builtInFormatCode(37),
                        'I'     => NumberFormat::builtInFormatCode(37),
                        'K'     => NumberFormat::builtInFormatCode(37),
                        'L'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    //<editor-fold desc="Details">
                    $resultDetails=[];
                    foreach ($data->budgetPromoList as $fields) {
                        $arr = [];
                        $arr[] = $fields->period;
                        $arr[] = $fields->promoId;
                        $arr[] = $fields->entity;
                        $arr[] = $fields->distributor;
                        $arr[] = $fields->activity;
                        $arr[] = $fields->activityName;
                        $arr[] = $fields->channel;
                        $arr[] = $fields->account;
                        $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                        $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                        $arr[] = $fields->investment;
                        $arr[] = $fields->investment5bio;
                        $arr[] = $fields->brand;
                        $arr[] = $fields->tradingTermAdhoc;
                        $arr[] = $fields->monthStart;
                        $arr[] = $fields->monthEnd;

                        $resultDetails[] = $arr;
                    }
                    $headerDetails = [
                        ['Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost', 'Cost >5bio', 'Brand', 'Trading Term / Adhoc', 'Month Start', 'Month End']
                    ];
                    $headerStyleDetails = 'A1:P1'; //Header Column Bold and color
                    $formatCellDetails = [
                        'K'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    $fileNameExcel = 'Approval Sheet Details (XLS)';
                    $export = new ExportBudgetApprovalRequestDC($data, $resultAbove, $resultBelow, $resultDetails,
                        $headerAbove, $headerBelow, $headerDetails, $centerTextAbove,
                        $headerStyleAbove, $headerStyleDetails, $arrSubHeaderMerged, $arrHeaderFill, $arrHeaderBorder, $arrBodyBorder, $arrFooterBorder, $centerTextBelow,
                        $formatCellAbove, $formatCellBelow, $formatCellDetails);
                    Excel::store($export, $path . $fileNameExcel  . ' ' . $uuid . '.xlsx', 'optima');
                    //</editor-fold>

                    $linkAttachment = [
                        'linkDownloadFileAbove' => config('app.url') . '/' . $path . $fileNameAbove . ' ' . $uuid . '.pdf',
                        'fileNameAbove'         => $fileNameAbove,
                        'linkDownloadFileBelow' => config('app.url') . '/' . $path . $fileNameBelow  . ' ' . $uuid .'.pdf',
                        'fileNameBelow'         => $fileNameBelow,
                        'linkDownloadFileExcel' => config('app.url') . '/' . $path . $fileNameExcel  . ' ' . $uuid . '.xlsx',
                        'fileNameExcel'         => $fileNameExcel,
                    ];
                } else {
                    //<editor-fold desc="Generate PDF Above 5 bio">
                    $fileNameAbove = 'Approval Sheet Above 5 bio (PDF)';
                    SnappyPdf::loadView('Budget::budget-approval-request.pdf-above-five-bio', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->setOption("footer-center", "Page [page] from [toPage]")
                        ->setOption('footer-font-size', 6)
                        ->save($path . $fileNameAbove . ' ' . $uuid . '.pdf');
                    //</editor-fold>

                    //<editor-fold desc="Generate PDF Below 5 bio">
                    $fileNameBelow = 'Approval Sheet Below 5 bio (PDF)';
                    SnappyPdf::loadView('Budget::budget-approval-request.pdf-below-five-bio', compact('data'))
                        ->setPaper('A4')
                        ->setOrientation('landscape')
                        ->setOption('margin-top',5)
                        ->setOption('margin-bottom',5)
                        ->setOption('margin-left',2)
                        ->setOption('margin-right',2)
                        ->save($path . $fileNameBelow  . ' ' . $uuid .'.pdf');
                    //</editor-fold>

                    //<editor-fold desc="Generate Excel">
                    //<editor-fold desc="Above">
                    $resultAbove=[];
                    $startRowAbove = 7;
                    foreach ($data->budgetOver5Bio as $fields) {
                        $arr = [];
                        $arr[] = '';
                        $arr[] = $fields->period;
                        $arr[] = $fields->promoId;
                        $arr[] = $fields->entity;
                        $arr[] = $fields->distributor;
                        $arr[] = $fields->activity;
                        $arr[] = $fields->activityName;
                        $arr[] = $fields->channel;
                        $arr[] = $fields->account;
                        $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                        $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                        $arr[] = $fields->investment;

                        $startRowAbove++;
                        $resultAbove[] = $arr;
                    }

                    $centerTextAbove = 'A' . $startRowAbove;
                    for ($i=0; $i<17; $i++) {
                        $arr = [];

                        if ($i === 6) {
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = 'Approved by,';
                        } else if ($i === 11) {
                            $signatureData = $data->emailApprovalSigned;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                    $arr[] = ($signatureData[3]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[3]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[4]->approvedOn)) : "");
                                }
                            } else {
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            }
                        } else if ($i === 15) {
                            $signatureData = $data->emailApproval;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = ($signatureData[2]->username ?? '');
                                $arr[] = ($signatureData[3]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[4]->username ?? '');
                            } else {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = ($signatureData[2]->username ?? '');
                                $arr[] = '';
                                $arr[] = '';
                            }
                        } else if ($i === 16) {
                            $signatureData = $data->emailApproval;
                            if (count($signatureData) > 3) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                                $arr[] = ($signatureData[3]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[4]->jobtitle ?? '');
                            } else {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = '';
                            }
                        } else {
                            $arr[] = '';
                        }

                        $startRowAbove = $startRowAbove + 1;
                        $resultAbove[] = $arr;
                    }
                    $centerTextAbove = $centerTextAbove . ':L' . $startRowAbove;

                    $headerAbove = [
                        [''],
                        [''],
                        ['', 'APPROVAL SHEET'],
                        ['', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Above IDR 5bio)'],
                        [''],
                        ['', 'Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost']
                    ];
                    $headerStyleAbove = 'B6:L6'; //Header Column Bold and color
                    $formatCellAbove = [
                        'L'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    //<editor-fold desc="Below">
                    $resultBelow=[];
                    $arrSubHeaderMerged = [];
                    $arrHeaderFill = [];
                    $arrHeaderBorder = [];
                    $arrBodyBorder = [];
                    $arrFooterBorder = [];
                    $startRowSubHeaderMerged = 5;
                    $formatted = new Formatted();

                    //<editor-fold desc="All">
                    //<editor-fold desc="Filter All">
                    $arrHeaderAll = [];
                    $arrHeaderAll[] = '';
                    $arrHeaderAll[] = '';
                    $arrHeaderAll[] = '';
                    $arrHeaderAll[] = 'All';
                    $resultBelow[] = $arrHeaderAll;
                    //</editor-fold>

                    //<editor-fold desc="Header All">
                    $arrHeaderAllBelow = [];
                    $arrHeaderAllBelow[] = '';
                    $arrHeaderAllBelow[] = '';
                    $arrHeaderAllBelow[] = '';
                    $startColumnNumberOfAlphabet = 5;
                    $startColumnNumberOfAlphabetFill = 4;
                    $startColumnNumberOfAlphabetBorder = 5;
                    foreach ($data->budgetBellow5BioAll->header as $fields) {
                        $arrHeaderAllBelow[] = $fields->headerName;
                        if ($fields->headerName !== '') {
                            $dataHeaderMerged = [
                                'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                'value'         =>  $fields->headerName
                            ];
                            array_push($arrSubHeaderMerged, $dataHeaderMerged);
                            $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                            $arrHeaderAllBelow[] = '';

                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                        }
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }

                    $arrHeaderAllBelow[] = 'Total Count of Promo ID';
                    $arrHeaderAllBelow[] = 'Sum of Total Cost';
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);

                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                        'value'         => 'Total Count of Promo ID'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                        'value'         => 'Sum of Total Cost'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);

                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrHeaderAllBelow;
                    //</editor-fold>

                    //<editor-fold desc="Sub Header All">
                    $arrSubHeaderAllBelow = [];
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = '';
                    $arrSubHeaderAllBelow[] = 'Channel';
                    $startColumnNumberOfAlphabetFill = 4;

                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    foreach ($data->budgetBellow5BioAll->subHeader[0]->valueSubHeader as $fields) {
                        $arrSubHeaderAllBelow[] = $fields->headerName;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrSubHeaderAllBelow;
                    //</editor-fold>

                    //<editor-fold desc="Body All">
                    foreach ($data->budgetBellow5BioAll->body as $body) {
                        $startColumnNumberOfAlphabetBodyBorder = 4;
                        $arrBodyAllBelow = [];
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = '';
                        $arrBodyAllBelow[] = $body->text;
                        array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                        foreach ($body->value as $value) {
                            $arrBodyAllBelow[] = $value->value;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrBodyAllBelow;
                    }
                    //</editor-fold>

                    //<editor-fold desc="Footer All">
                    $startColumnNumberOfAlphabetFooterBorder = 4;
                    foreach ($data->budgetBellow5BioAll->footer as $footer) {
                        $arrFooterAllBelow = [];
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = '';
                        $arrFooterAllBelow[] = $footer->text;
                        array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        foreach ($footer->value as $value) {
                            $arrFooterAllBelow[] = $value->value;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrFooterAllBelow;
                    }
                    //</editor-fold>

                    for ($i=0; $i<3; $i++) {
                        $arrSeparatorAll = [];
                        $arrSeparatorAll[] = '';

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSeparatorAll;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    //</editor-fold>

                    //<editor-fold desc="Trading Term">
                    //<editor-fold desc="Filter Trading Term">
                    $arrHeaderTradingTerm = [];
                    $arrHeaderTradingTerm[] = '';
                    $arrHeaderTradingTerm[] = '';
                    $arrHeaderTradingTerm[] = '';
                    $arrHeaderTradingTerm[] = 'Trading Term';
                    $resultBelow[] = $arrHeaderTradingTerm;
                    //</editor-fold>

                    //<editor-fold desc="Header Trading Term">
                    $arrHeaderTradingTermBelow = [];
                    $arrHeaderTradingTermBelow[] = '';
                    $arrHeaderTradingTermBelow[] = '';
                    $arrHeaderTradingTermBelow[] = '';
                    $startColumnNumberOfAlphabet = 5;
                    $startColumnNumberOfAlphabetFill = 4;
                    $startColumnNumberOfAlphabetBorder = 5;
                    foreach ($data->budgetBellow5BioContractual->header as $fields) {
                        $arrHeaderTradingTermBelow[] = $fields->headerName;
                        if ($fields->headerName !== '') {
                            $dataHeaderMerged = [
                                'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                'value'         =>  $fields->headerName
                            ];
                            array_push($arrSubHeaderMerged, $dataHeaderMerged);
                            $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                            $arrHeaderTradingTermBelow[] = '';

                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                        }
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $arrHeaderTradingTermBelow[] = 'Total Count of Promo ID';
                    $arrHeaderTradingTermBelow[] = 'Sum of Total Cost';
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                        'value'         => 'Total Count of Promo ID'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                        'value'         => 'Sum of Total Cost'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrHeaderTradingTermBelow;
                    //</editor-fold>

                    //<editor-fold desc="Sub Header Trading Term">
                    $arrSubHeaderTradingTermBelow = [];
                    $arrSubHeaderTradingTermBelow[] = '';
                    $arrSubHeaderTradingTermBelow[] = '';
                    $arrSubHeaderTradingTermBelow[] = '';
                    $arrSubHeaderTradingTermBelow[] = 'Channel';
                    $startColumnNumberOfAlphabetFill = 4;

                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    foreach ($data->budgetBellow5BioContractual->subHeader[0]->valueSubHeader as $fields) {
                        $arrSubHeaderTradingTermBelow[] = $fields->headerName;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrSubHeaderTradingTermBelow;
                    //</editor-fold>

                    //<editor-fold desc="Body Trading Term">
                    foreach ($data->budgetBellow5BioContractual->body as $body) {
                        $startColumnNumberOfAlphabetBodyBorder = 4;
                        $arrBodyTradingTermBelow = [];
                        $arrBodyTradingTermBelow[] = '';
                        $arrBodyTradingTermBelow[] = '';
                        $arrBodyTradingTermBelow[] = '';
                        $arrBodyTradingTermBelow[] = $body->text;
                        array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                        foreach ($body->value as $value) {
                            $arrBodyTradingTermBelow[] = $value->value;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrBodyTradingTermBelow;
                    }
                    //</editor-fold>

                    //<editor-fold desc="Footer Trading Term">
                    $startColumnNumberOfAlphabetFooterBorder = 4;
                    foreach ($data->budgetBellow5BioContractual->footer as $footer) {
                        $arrFooterTradingTermBelow = [];
                        $arrFooterTradingTermBelow[] = '';
                        $arrFooterTradingTermBelow[] = '';
                        $arrFooterTradingTermBelow[] = '';
                        $arrFooterTradingTermBelow[] = $footer->text;
                        array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        foreach ($footer->value as $value) {
                            $arrFooterTradingTermBelow[] = $value->value;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrFooterTradingTermBelow;
                    }
                    //</editor-fold>

                    for ($i=0; $i<3; $i++) {
                        $arrSeparatorTradingTerm = [];
                        $arrSeparatorTradingTerm[] = '';

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSeparatorTradingTerm;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    //</editor-fold>

                    //<editor-fold desc="Adhoc">
                    //<editor-fold desc="Filter Adhoc">
                    $arrHeaderAdhoc = [];
                    $arrHeaderAdhoc[] = '';
                    $arrHeaderAdhoc[] = '';
                    $arrHeaderAdhoc[] = '';
                    $arrHeaderAdhoc[] = 'Adhoc';
                    $resultBelow[] = $arrHeaderAdhoc;
                    //</editor-fold>

                    //<editor-fold desc="Header Adhoc">
                    $arrHeaderAdhocBelow = [];
                    $arrHeaderAdhocBelow[] = '';
                    $arrHeaderAdhocBelow[] = '';
                    $arrHeaderAdhocBelow[] = '';
                    $startColumnNumberOfAlphabet = 5;
                    $startColumnNumberOfAlphabetFill = 4;
                    $startColumnNumberOfAlphabetBorder = 5;
                    foreach ($data->budgetBellow5BioNonContractual->header as $fields) {
                        $arrHeaderAdhocBelow[] = $fields->headerName;
                        if ($fields->headerName !== '') {
                            $dataHeaderMerged = [
                                'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                'value'         =>  $fields->headerName
                            ];
                            array_push($arrSubHeaderMerged, $dataHeaderMerged);
                            $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                            $arrHeaderAdhocBelow[] = '';

                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                            $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                        }
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $arrHeaderAdhocBelow[] = 'Total Count of Promo ID';
                    $arrHeaderAdhocBelow[] = 'Sum of Total Cost';
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                        'value'         => 'Total Count of Promo ID'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $dataHeaderMerged = [
                        'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                        'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                        'value'         => 'Sum of Total Cost'
                    ];
                    array_push($arrSubHeaderMerged, $dataHeaderMerged);
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrHeaderAdhocBelow;
                    //</editor-fold>

                    //<editor-fold desc="Sub Header Adhoc">
                    $arrSubHeaderAdhocBelow = [];
                    $arrSubHeaderAdhocBelow[] = '';
                    $arrSubHeaderAdhocBelow[] = '';
                    $arrSubHeaderAdhocBelow[] = '';
                    $arrSubHeaderAdhocBelow[] = 'Channel';
                    $startColumnNumberOfAlphabetFill = 4;

                    array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                    $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    foreach ($data->budgetBellow5BioNonContractual->subHeader[0]->valueSubHeader as $fields) {
                        $arrSubHeaderAdhocBelow[] = $fields->headerName;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                    }
                    $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                    $resultBelow[] = $arrSubHeaderAdhocBelow;
                    //</editor-fold>

                    //<editor-fold desc="Body Adhoc">
                    foreach ($data->budgetBellow5BioNonContractual->body as $body) {
                        $startColumnNumberOfAlphabetBodyBorder = 4;
                        $arrBodyAdhocBelow = [];
                        $arrBodyAdhocBelow[] = '';
                        $arrBodyAdhocBelow[] = '';
                        $arrBodyAdhocBelow[] = '';
                        $arrBodyAdhocBelow[] = $body->text;
                        array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                        foreach ($body->value as $value) {
                            $arrBodyAdhocBelow[] = $value->value;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrBodyAdhocBelow;
                    }
                    //</editor-fold>

                    //<editor-fold desc="Footer Adhoc">
                    $startColumnNumberOfAlphabetFooterBorder = 4;
                    foreach ($data->budgetBellow5BioNonContractual->footer as $footer) {
                        $arrFooterAdhocBelow = [];
                        $arrFooterAdhocBelow[] = '';
                        $arrFooterAdhocBelow[] = '';
                        $arrFooterAdhocBelow[] = '';
                        $arrFooterAdhocBelow[] = $footer->text;
                        array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        foreach ($footer->value as $value) {
                            $arrFooterAdhocBelow[] = $value->value;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                        }

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrFooterAdhocBelow;
                    }
                    //</editor-fold>

                    for ($i=0; $i<3; $i++) {
                        $arrSeparatorAdhoc = [];
                        $arrSeparatorAdhoc[] = '';

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSeparatorAdhoc;
                    }
                    //</editor-fold>

                    $centerTextBelow = 'A' . $startRowSubHeaderMerged;
                    for ($i=0; $i<17; $i++) {
                        $arr = [];

                        if ($i === 6) {
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = 'Approved by,';
                        } else if ($i === 11) {
                            $signatureData = $data->emailApprovalSigned;
                            $arr[] = '';
                            if ($signatureData) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                            }
                        } else if ($i === 15) {
                            $signatureData = $data->emailApproval;
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = ($signatureData[0]->username ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[1]->username ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[2]->username ?? '');
                        } else if ($i === 16) {
                            $signatureData = $data->emailApproval;
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = '';
                            $arr[] = ($signatureData[0]->jobtitle ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[1]->jobtitle ?? '');
                            $arr[] = '';
                            $arr[] = ($signatureData[2]->jobtitle ?? '');
                        } else {
                            $arr[] = '';
                        }


                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arr;
                    }
                    $centerTextBelow = $centerTextBelow . ':L' . $startRowSubHeaderMerged;

                    $headerBelow = [
                        ['', '', '', 'APPROVAL SHEET'],
                        ['', '', '', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Below IDR 5bio)'],
                        ['']
                    ];
                    $formatCellBelow = [
                        'E'     => NumberFormat::builtInFormatCode(37),
                        'F'     => NumberFormat::builtInFormatCode(37),
                        'G'     => NumberFormat::builtInFormatCode(37),
                        'H'     => NumberFormat::builtInFormatCode(37),
                        'J'     => NumberFormat::builtInFormatCode(37),
                        'I'     => NumberFormat::builtInFormatCode(37),
                        'K'     => NumberFormat::builtInFormatCode(37),
                        'L'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    //<editor-fold desc="Details">
                    $resultDetails=[];
                    foreach ($data->budgetPromoList as $fields) {
                        $arr = [];
                        $arr[] = $fields->period;
                        $arr[] = $fields->promoId;
                        $arr[] = $fields->entity;
                        $arr[] = $fields->distributor;
                        $arr[] = $fields->activity;
                        $arr[] = $fields->activityName;
                        $arr[] = $fields->channel;
                        $arr[] = $fields->account;
                        $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                        $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                        $arr[] = $fields->investment;
                        $arr[] = $fields->investment5bio;
                        $arr[] = $fields->brand;
                        $arr[] = $fields->tradingTermAdhoc;
                        $arr[] = $fields->monthStart;
                        $arr[] = $fields->monthEnd;

                        $resultDetails[] = $arr;
                    }
                    $headerDetails = [
                        ['Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost', 'Cost >5bio', 'Brand', 'Trading Term / Adhoc', 'Month Start', 'Month End']
                    ];
                    $headerStyleDetails = 'A1:P1'; //Header Column Bold and color
                    $formatCellDetails = [
                        'K'     => NumberFormat::builtInFormatCode(37)
                    ];
                    //</editor-fold>

                    $fileNameExcel = 'Approval Sheet Details (XLS)';
                    $export = new ExportBudgetApprovalRequest($data, $resultAbove, $resultBelow, $resultDetails,
                        $headerAbove, $headerBelow, $headerDetails, $centerTextAbove,
                        $headerStyleAbove, $headerStyleDetails, $arrSubHeaderMerged, $arrHeaderFill, $arrHeaderBorder, $arrBodyBorder, $arrFooterBorder, $centerTextBelow,
                        $formatCellAbove, $formatCellBelow, $formatCellDetails);
                    Excel::store($export, $path . $fileNameExcel  . ' ' . $uuid . '.xlsx', 'optima');
                    //</editor-fold>

                    $linkAttachment = [
                        'linkDownloadFileAbove' => config('app.url') . '/' . $path . $fileNameAbove . ' ' . $uuid . '.pdf',
                        'fileNameAbove'         => $fileNameAbove,
                        'linkDownloadFileBelow' => config('app.url') . '/' . $path . $fileNameBelow  . ' ' . $uuid .'.pdf',
                        'fileNameBelow'         => $fileNameBelow,
                        'linkDownloadFileExcel' => config('app.url') . '/' . $path . $fileNameExcel  . ' ' . $uuid . '.xlsx',
                        'fileNameExcel'         => $fileNameExcel,
                    ];
                }

                $apiEmail = config('app.api') . '/tools/email';
                $email = $data->emailApproval;
                $dataUrl = json_encode([
                    'batchId'       => $batchId,
                    'profileId'     => $email[0]->profileid,
                    'profileEmail'  => $email[0]->email
                ]);
                $category = $request['categoryDesc'];
                $url = encrypt($dataUrl);
                $linkApproval = config('app.url') . '/budget/approval-request/action?a=approve&c='. $request['categoryDesc'] .'&i=' . $url;
                $linkReject = config('app.url') . '/budget/approval-request/action?a=reject&i=' . $url;
                $message = view('Budget::budget-approval-request.email-body', compact('data', 'period', 'periodDesc', 'linkApproval', 'linkAttachment', 'linkReject', 'category'))->toHtml();

                $bodyEmail = [
                    'email'         => $email[0]->email,
                    'subject'       => '[Approval Needed] - OPTIMA Promo ID '. $periodDesc . ' ' . $period,
                    'body'          => $message,
                    'cc'            => ($email[0]->ccEmail ?? "")
                ];
                Log::info('post API ' . $apiEmail);
                Log::info('email ' . $email[0]->email);
                $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($apiEmail, $bodyEmail);
                Log::info($responseEmail->status());
                Log::info($responseEmail->reason());
                if ($responseEmail->status() === 200) {
                    Log::info("Send Email Success " . $email[0]->email);
                } else {
                    Log::info("error : Send Email Failed " . $email[0]->email);
                }

                return json_encode([
                    'error'     => false,
                    'data'      => [],
                    'message'   => "Send Email Success"
                ]);
            } else {
                return $res;
            }
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            );
        }
    }

    public function actionPage(): Factory|View|Application
    {
        return view('Budget::budget-approval-request.approved-page');
    }

    public function approve(Request $request): array
    {
        $api = config('app.api') . '/budget/approvalrequest/approve';
        try {
            $data = json_decode(decrypt($request['i']));
            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($data));
            $response = Http::timeout(180)->post($api, $data);
            if ($response->status() === 200) {
                $data = json_decode($response)->values;
                $batchId = $data->batchId;
                $period = ($data->period ?? '2025');
                $periodDesc = ($data->periodDesc ?? '');
                $email = $data->nextApproval;

                if (count($data->nextApproval) > 0) {
                    //<editor-fold desc="Create Attachment">
                    $path = 'assets/media/budget/approval-request/email/' . $batchId . '/';
                    if (!Storage::disk('optima')->exists($path)) {
                        Storage::disk('optima')->makeDirectory($path);
                    }

                    $uuid = date('Y-m-d His') . ' ' . microtime(true);

                    if ($request['c'] === 'DC') {
                        //<editor-fold desc="Generate PDF Above 5 bio">
                        $fileNameAbove = 'Approval Sheet Above 5 bio (PDF)';
                        SnappyPdf::loadView('Budget::budget-approval-request.pdf-above-five-bio-dc', compact('data'))
                            ->setPaper('A4')
                            ->setOrientation('landscape')
                            ->setOption('margin-top',5)
                            ->setOption('margin-bottom',5)
                            ->setOption('margin-left',2)
                            ->setOption('margin-right',2)
                            ->setOption("footer-center", "Page [page] from [toPage]")
                            ->setOption('footer-font-size', 6)
                            ->save($path . $fileNameAbove . ' ' . $uuid . '.pdf');
                        //</editor-fold>

                        //<editor-fold desc="Generate PDF Below 5 bio">
                        $fileNameBelow = 'Approval Sheet Below 5 bio (PDF)';
                        SnappyPdf::loadView('Budget::budget-approval-request.pdf-below-five-bio-dc', compact('data'))
                            ->setPaper('A4')
                            ->setOrientation('landscape')
                            ->setOption('margin-top',5)
                            ->setOption('margin-bottom',5)
                            ->setOption('margin-left',2)
                            ->setOption('margin-right',2)
                            ->save($path . $fileNameBelow  . ' ' . $uuid .'.pdf');
                        //</editor-fold>

                        //<editor-fold desc="Generate Excel">
                        //<editor-fold desc="Above">
                        $resultAbove=[];
                        $startRowAbove = 7;
                        foreach ($data->budgetOver5Bio as $fields) {
                            $arr = [];
                            $arr[] = '';
                            $arr[] = $fields->period;
                            $arr[] = $fields->promoId;
                            $arr[] = $fields->entity;
                            $arr[] = $fields->distributor;
                            $arr[] = $fields->activity;
                            $arr[] = $fields->activityName;
                            $arr[] = $fields->channel;
                            $arr[] = $fields->account;
                            $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                            $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                            $arr[] = $fields->investment;

                            $startRowAbove++;
                            $resultAbove[] = $arr;
                        }

                        $centerTextAbove = 'A' . $startRowAbove;
                        for ($i=0; $i<17; $i++) {
                            $arr = [];

                            if ($i === 6) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = 'Approved by,';
                            } else if ($i === 11) {
                                $signatureData = $data->emailApprovalSigned;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    if ($signatureData) {
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                        $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                        $arr[] = ($signatureData[3]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[3]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = ($signatureData[4]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[4]->approvedOn)) : "");
                                    }
                                } else {
                                    $arr[] = '';
                                    if ($signatureData) {
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                        $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                        $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = '';
                                    }
                                }
                            } else if ($i === 15) {
                                $signatureData = $data->emailApproval;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->username ?? '');
                                    $arr[] = ($signatureData[2]->username ?? '');
                                    $arr[] = ($signatureData[3]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->username ?? '');
                                } else {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->username ?? '');
                                    $arr[] = ($signatureData[1]->username ?? '');
                                    $arr[] = ($signatureData[2]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            } else if ($i === 16) {
                                $signatureData = $data->emailApproval;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->jobtitle ?? '');
                                    $arr[] = ($signatureData[2]->jobtitle ?? '');
                                    $arr[] = ($signatureData[3]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->jobtitle ?? '');
                                } else {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->jobtitle ?? '');
                                    $arr[] = ($signatureData[1]->jobtitle ?? '');
                                    $arr[] = ($signatureData[2]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            } else {
                                $arr[] = '';
                            }

                            $startRowAbove = $startRowAbove + 1;
                            $resultAbove[] = $arr;
                        }
                        $centerTextAbove = $centerTextAbove . ':L' . $startRowAbove;

                        $headerAbove = [
                            [''],
                            [''],
                            ['', 'APPROVAL SHEET'],
                            ['', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Above IDR 5bio)'],
                            [''],
                            ['', 'Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost']
                        ];
                        $headerStyleAbove = 'B6:L6'; //Header Column Bold and color
                        $formatCellAbove = [
                            'L'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        //<editor-fold desc="Below">
                        $resultBelow=[];
                        $arrSubHeaderMerged = [];
                        $arrHeaderFill = [];
                        $arrHeaderBorder = [];
                        $arrBodyBorder = [];
                        $arrFooterBorder = [];
                        $startRowSubHeaderMerged = 4;
                        $formatted = new Formatted();

                        //<editor-fold desc="All">
                        //<editor-fold desc="Header All">
                        $arrHeaderAllBelow = [];
                        $arrHeaderAllBelow[] = '';
                        $arrHeaderAllBelow[] = '';
                        $arrHeaderAllBelow[] = '';
                        $startColumnNumberOfAlphabet = 5;
                        $startColumnNumberOfAlphabetFill = 4;
                        $startColumnNumberOfAlphabetBorder = 5;
                        foreach ($data->budgetBellow5BioAll->header as $fields) {
                            $arrHeaderAllBelow[] = $fields->headerName;
                            if ($fields->headerName !== '') {
                                $dataHeaderMerged = [
                                    'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                    'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                    'value'         =>  $fields->headerName
                                ];
                                array_push($arrSubHeaderMerged, $dataHeaderMerged);
                                $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                                $arrHeaderAllBelow[] = '';

                                array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            }
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }

                        $arrHeaderAllBelow[] = 'Total Count of Promo ID';
                        $arrHeaderAllBelow[] = 'Sum of Total Cost';
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);

                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                            'value'         => 'Total Count of Promo ID'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                            'value'         => 'Sum of Total Cost'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrHeaderAllBelow;
                        //</editor-fold>

                        //<editor-fold desc="Sub Header All">
                        $arrSubHeaderAllBelow = [];
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = 'Distributor';
                        $startColumnNumberOfAlphabetFill = 4;

                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        foreach ($data->budgetBellow5BioAll->subHeader[0]->valueSubHeader as $fields) {
                            $arrSubHeaderAllBelow[] = $fields->headerName;
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSubHeaderAllBelow;
                        //</editor-fold>

                        //<editor-fold desc="Body All">
                        foreach ($data->budgetBellow5BioAll->body as $body) {
                            $startColumnNumberOfAlphabetBodyBorder = 4;
                            $arrBodyAllBelow = [];
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = $body->text;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                            foreach ($body->value as $value) {
                                $arrBodyAllBelow[] = $value->value;
                                array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrBodyAllBelow;
                        }
                        //</editor-fold>

                        //<editor-fold desc="Footer All">
                        $startColumnNumberOfAlphabetFooterBorder = 4;
                        foreach ($data->budgetBellow5BioAll->footer as $footer) {
                            $arrFooterAllBelow = [];
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = $footer->text;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            foreach ($footer->value as $value) {
                                $arrFooterAllBelow[] = $value->value;
                                array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrFooterAllBelow;
                        }
                        //</editor-fold>

                        for ($i=0; $i<3; $i++) {
                            $arrSeparatorAll = [];
                            $arrSeparatorAll[] = '';

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrSeparatorAll;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        //</editor-fold>

                        $centerTextBelow = 'A' . $startRowSubHeaderMerged;
                        for ($i=0; $i<17; $i++) {
                            $arr = [];

                            if ($i === 6) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = 'Approved by,';
                            } else if ($i === 11) {
                                $signatureData = $data->emailApprovalSigned;
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                }
                            } else if ($i === 15) {
                                $signatureData = $data->emailApproval;
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->username ?? '');
                            } else if ($i === 16) {
                                $signatureData = $data->emailApproval;
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                            } else {
                                $arr[] = '';
                            }


                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arr;
                        }
                        $centerTextBelow = $centerTextBelow . ':L' . $startRowSubHeaderMerged;

                        $headerBelow = [
                            ['', '', '', 'APPROVAL SHEET'],
                            ['', '', '', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Below IDR 5bio)'],
                            ['']
                        ];
                        $formatCellBelow = [
                            'E'     => NumberFormat::builtInFormatCode(37),
                            'F'     => NumberFormat::builtInFormatCode(37),
                            'G'     => NumberFormat::builtInFormatCode(37),
                            'H'     => NumberFormat::builtInFormatCode(37),
                            'J'     => NumberFormat::builtInFormatCode(37),
                            'I'     => NumberFormat::builtInFormatCode(37),
                            'K'     => NumberFormat::builtInFormatCode(37),
                            'L'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        //<editor-fold desc="Details">
                        $resultDetails=[];
                        foreach ($data->budgetPromoList as $fields) {
                            $arr = [];
                            $arr[] = $fields->period;
                            $arr[] = $fields->promoId;
                            $arr[] = $fields->entity;
                            $arr[] = $fields->distributor;
                            $arr[] = $fields->activity;
                            $arr[] = $fields->activityName;
                            $arr[] = $fields->channel;
                            $arr[] = $fields->account;
                            $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                            $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                            $arr[] = $fields->investment;
                            $arr[] = $fields->investment5bio;
                            $arr[] = $fields->brand;
                            $arr[] = $fields->tradingTermAdhoc;
                            $arr[] = $fields->monthStart;
                            $arr[] = $fields->monthEnd;

                            $resultDetails[] = $arr;
                        }
                        $headerDetails = [
                            ['Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost', 'Cost >5bio', 'Brand', 'Trading Term / Adhoc', 'Month Start', 'Month End']
                        ];
                        $headerStyleDetails = 'A1:P1'; //Header Column Bold and color
                        $formatCellDetails = [
                            'K'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        $fileNameExcel = 'Approval Sheet Details (XLS)';
                        $export = new ExportBudgetApprovalRequestDC($data, $resultAbove, $resultBelow, $resultDetails,
                            $headerAbove, $headerBelow, $headerDetails, $centerTextAbove,
                            $headerStyleAbove, $headerStyleDetails, $arrSubHeaderMerged, $arrHeaderFill, $arrHeaderBorder, $arrBodyBorder, $arrFooterBorder, $centerTextBelow,
                            $formatCellAbove, $formatCellBelow, $formatCellDetails);
                        Excel::store($export, $path . $fileNameExcel  . ' ' . $uuid . '.xlsx', 'optima');
                        //</editor-fold>

                        $linkAttachment = [
                            'linkDownloadFileAbove' => config('app.url') . '/' . $path . $fileNameAbove . ' ' . $uuid . '.pdf',
                            'fileNameAbove'         => $fileNameAbove,
                            'linkDownloadFileBelow' => config('app.url') . '/' . $path . $fileNameBelow  . ' ' . $uuid .'.pdf',
                            'fileNameBelow'         => $fileNameBelow,
                            'linkDownloadFileExcel' => config('app.url') . '/' . $path . $fileNameExcel  . ' ' . $uuid . '.xlsx',
                            'fileNameExcel'         => $fileNameExcel,
                        ];
                    } else {
                        //<editor-fold desc="Generate PDF Above 5 bio">
                        $fileNameAbove = 'Approval Sheet Above 5 bio (PDF)';
                        SnappyPdf::loadView('Budget::budget-approval-request.pdf-above-five-bio', compact('data'))
                            ->setPaper('A4')
                            ->setOrientation('landscape')
                            ->setOption('margin-top',5)
                            ->setOption('margin-bottom',5)
                            ->setOption('margin-left',2)
                            ->setOption('margin-right',2)
                            ->setOption("footer-center", "Page [page] from [toPage]")
                            ->setOption('footer-font-size', 6)
                            ->save($path . $fileNameAbove . ' ' . $uuid . '.pdf');
                        //</editor-fold>

                        //<editor-fold desc="Generate PDF Below 5 bio">
                        $fileNameBelow = 'Approval Sheet Below 5 bio (PDF)';
                        SnappyPdf::loadView('Budget::budget-approval-request.pdf-below-five-bio', compact('data'))
                            ->setPaper('A4')
                            ->setOrientation('landscape')
                            ->setOption('margin-top',5)
                            ->setOption('margin-bottom',5)
                            ->setOption('margin-left',2)
                            ->setOption('margin-right',2)
                            ->save($path . $fileNameBelow  . ' ' . $uuid .'.pdf');
                        //</editor-fold>

                        //<editor-fold desc="Generate Excel">
                        //<editor-fold desc="Above">
                        $resultAbove=[];
                        $startRowAbove = 7;
                        foreach ($data->budgetOver5Bio as $fields) {
                            $arr = [];
                            $arr[] = '';
                            $arr[] = $fields->period;
                            $arr[] = $fields->promoId;
                            $arr[] = $fields->entity;
                            $arr[] = $fields->distributor;
                            $arr[] = $fields->activity;
                            $arr[] = $fields->activityName;
                            $arr[] = $fields->channel;
                            $arr[] = $fields->account;
                            $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                            $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                            $arr[] = $fields->investment;

                            $startRowAbove++;
                            $resultAbove[] = $arr;
                        }

                        $centerTextAbove = 'A' . $startRowAbove;
                        for ($i=0; $i<17; $i++) {
                            $arr = [];

                            if ($i === 6) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = 'Approved by,';
                            } else if ($i === 11) {
                                $signatureData = $data->emailApprovalSigned;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    if ($signatureData) {
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                        $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                        $arr[] = ($signatureData[3]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[3]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = ($signatureData[4]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[4]->approvedOn)) : "");
                                    }
                                } else {
                                    $arr[] = '';
                                    if ($signatureData) {
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = '';
                                        $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                        $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[1]->approvedOn)) : "");
                                        $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[2]->approvedOn)) : "");
                                        $arr[] = '';
                                        $arr[] = '';
                                    }
                                }
                            } else if ($i === 15) {
                                $signatureData = $data->emailApproval;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->username ?? '');
                                    $arr[] = ($signatureData[2]->username ?? '');
                                    $arr[] = ($signatureData[3]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->username ?? '');
                                } else {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->username ?? '');
                                    $arr[] = ($signatureData[1]->username ?? '');
                                    $arr[] = ($signatureData[2]->username ?? '');
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            } else if ($i === 16) {
                                $signatureData = $data->emailApproval;
                                if (count($signatureData) > 3) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->jobtitle ?? '');
                                    $arr[] = ($signatureData[2]->jobtitle ?? '');
                                    $arr[] = ($signatureData[3]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = ($signatureData[4]->jobtitle ?? '');
                                } else {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->jobtitle ?? '');
                                    $arr[] = ($signatureData[1]->jobtitle ?? '');
                                    $arr[] = ($signatureData[2]->jobtitle ?? '');
                                    $arr[] = '';
                                    $arr[] = '';
                                }
                            } else {
                                $arr[] = '';
                            }

                            $startRowAbove = $startRowAbove + 1;
                            $resultAbove[] = $arr;
                        }
                        $centerTextAbove = $centerTextAbove . ':L' . $startRowAbove;

                        $headerAbove = [
                            [''],
                            [''],
                            ['', 'APPROVAL SHEET'],
                            ['', 'OPTIMA PROMO ID [' . strtoupper($data->periodDesc). ' ' . $data->period . '] (Above IDR 5bio)'],
                            [''],
                            ['', 'Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost']
                        ];
                        $headerStyleAbove = 'B6:L6'; //Header Column Bold and color
                        $formatCellAbove = [
                            'L'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        //<editor-fold desc="Below">
                        $resultBelow=[];
                        $arrSubHeaderMerged = [];
                        $arrHeaderFill = [];
                        $arrHeaderBorder = [];
                        $arrBodyBorder = [];
                        $arrFooterBorder = [];
                        $startRowSubHeaderMerged = 5;
                        $formatted = new Formatted();

                        //<editor-fold desc="All">
                        //<editor-fold desc="Filter All">
                        $arrHeaderAll = [];
                        $arrHeaderAll[] = '';
                        $arrHeaderAll[] = '';
                        $arrHeaderAll[] = '';
                        $arrHeaderAll[] = 'All';
                        $resultBelow[] = $arrHeaderAll;
                        //</editor-fold>

                        //<editor-fold desc="Header All">
                        $arrHeaderAllBelow = [];
                        $arrHeaderAllBelow[] = '';
                        $arrHeaderAllBelow[] = '';
                        $arrHeaderAllBelow[] = '';
                        $startColumnNumberOfAlphabet = 5;
                        $startColumnNumberOfAlphabetFill = 4;
                        $startColumnNumberOfAlphabetBorder = 5;
                        foreach ($data->budgetBellow5BioAll->header as $fields) {
                            $arrHeaderAllBelow[] = $fields->headerName;
                            if ($fields->headerName !== '') {
                                $dataHeaderMerged = [
                                    'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                    'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                    'value'         =>  $fields->headerName
                                ];
                                array_push($arrSubHeaderMerged, $dataHeaderMerged);
                                $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                                $arrHeaderAllBelow[] = '';

                                array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            }
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }

                        $arrHeaderAllBelow[] = 'Total Count of Promo ID';
                        $arrHeaderAllBelow[] = 'Sum of Total Cost';
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);

                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                            'value'         => 'Total Count of Promo ID'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                            'value'         => 'Sum of Total Cost'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);

                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrHeaderAllBelow;
                        //</editor-fold>

                        //<editor-fold desc="Sub Header All">
                        $arrSubHeaderAllBelow = [];
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = '';
                        $arrSubHeaderAllBelow[] = 'Channel';
                        $startColumnNumberOfAlphabetFill = 4;

                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        foreach ($data->budgetBellow5BioAll->subHeader[0]->valueSubHeader as $fields) {
                            $arrSubHeaderAllBelow[] = $fields->headerName;
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSubHeaderAllBelow;
                        //</editor-fold>

                        //<editor-fold desc="Body All">
                        foreach ($data->budgetBellow5BioAll->body as $body) {
                            $startColumnNumberOfAlphabetBodyBorder = 4;
                            $arrBodyAllBelow = [];
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = '';
                            $arrBodyAllBelow[] = $body->text;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                            foreach ($body->value as $value) {
                                $arrBodyAllBelow[] = $value->value;
                                array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrBodyAllBelow;
                        }
                        //</editor-fold>

                        //<editor-fold desc="Footer All">
                        $startColumnNumberOfAlphabetFooterBorder = 4;
                        foreach ($data->budgetBellow5BioAll->footer as $footer) {
                            $arrFooterAllBelow = [];
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = '';
                            $arrFooterAllBelow[] = $footer->text;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            foreach ($footer->value as $value) {
                                $arrFooterAllBelow[] = $value->value;
                                array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrFooterAllBelow;
                        }
                        //</editor-fold>

                        for ($i=0; $i<3; $i++) {
                            $arrSeparatorAll = [];
                            $arrSeparatorAll[] = '';

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrSeparatorAll;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        //</editor-fold>

                        //<editor-fold desc="Trading Term">
                        //<editor-fold desc="Filter Trading Term">
                        $arrHeaderTradingTerm = [];
                        $arrHeaderTradingTerm[] = '';
                        $arrHeaderTradingTerm[] = '';
                        $arrHeaderTradingTerm[] = '';
                        $arrHeaderTradingTerm[] = 'Trading Term';
                        $resultBelow[] = $arrHeaderTradingTerm;
                        //</editor-fold>

                        //<editor-fold desc="Header Trading Term">
                        $arrHeaderTradingTermBelow = [];
                        $arrHeaderTradingTermBelow[] = '';
                        $arrHeaderTradingTermBelow[] = '';
                        $arrHeaderTradingTermBelow[] = '';
                        $startColumnNumberOfAlphabet = 5;
                        $startColumnNumberOfAlphabetFill = 4;
                        $startColumnNumberOfAlphabetBorder = 5;
                        foreach ($data->budgetBellow5BioContractual->header as $fields) {
                            $arrHeaderTradingTermBelow[] = $fields->headerName;
                            if ($fields->headerName !== '') {
                                $dataHeaderMerged = [
                                    'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                    'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                    'value'         =>  $fields->headerName
                                ];
                                array_push($arrSubHeaderMerged, $dataHeaderMerged);
                                $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                                $arrHeaderTradingTermBelow[] = '';

                                array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            }
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $arrHeaderTradingTermBelow[] = 'Total Count of Promo ID';
                        $arrHeaderTradingTermBelow[] = 'Sum of Total Cost';
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                            'value'         => 'Total Count of Promo ID'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                            'value'         => 'Sum of Total Cost'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrHeaderTradingTermBelow;
                        //</editor-fold>

                        //<editor-fold desc="Sub Header Trading Term">
                        $arrSubHeaderTradingTermBelow = [];
                        $arrSubHeaderTradingTermBelow[] = '';
                        $arrSubHeaderTradingTermBelow[] = '';
                        $arrSubHeaderTradingTermBelow[] = '';
                        $arrSubHeaderTradingTermBelow[] = 'Channel';
                        $startColumnNumberOfAlphabetFill = 4;

                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        foreach ($data->budgetBellow5BioContractual->subHeader[0]->valueSubHeader as $fields) {
                            $arrSubHeaderTradingTermBelow[] = $fields->headerName;
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSubHeaderTradingTermBelow;
                        //</editor-fold>

                        //<editor-fold desc="Body Trading Term">
                        foreach ($data->budgetBellow5BioContractual->body as $body) {
                            $startColumnNumberOfAlphabetBodyBorder = 4;
                            $arrBodyTradingTermBelow = [];
                            $arrBodyTradingTermBelow[] = '';
                            $arrBodyTradingTermBelow[] = '';
                            $arrBodyTradingTermBelow[] = '';
                            $arrBodyTradingTermBelow[] = $body->text;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                            foreach ($body->value as $value) {
                                $arrBodyTradingTermBelow[] = $value->value;
                                array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrBodyTradingTermBelow;
                        }
                        //</editor-fold>

                        //<editor-fold desc="Footer Trading Term">
                        $startColumnNumberOfAlphabetFooterBorder = 4;
                        foreach ($data->budgetBellow5BioContractual->footer as $footer) {
                            $arrFooterTradingTermBelow = [];
                            $arrFooterTradingTermBelow[] = '';
                            $arrFooterTradingTermBelow[] = '';
                            $arrFooterTradingTermBelow[] = '';
                            $arrFooterTradingTermBelow[] = $footer->text;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            foreach ($footer->value as $value) {
                                $arrFooterTradingTermBelow[] = $value->value;
                                array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrFooterTradingTermBelow;
                        }
                        //</editor-fold>

                        for ($i=0; $i<3; $i++) {
                            $arrSeparatorTradingTerm = [];
                            $arrSeparatorTradingTerm[] = '';

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrSeparatorTradingTerm;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        //</editor-fold>

                        //<editor-fold desc="Adhoc">
                        //<editor-fold desc="Filter Adhoc">
                        $arrHeaderAdhoc = [];
                        $arrHeaderAdhoc[] = '';
                        $arrHeaderAdhoc[] = '';
                        $arrHeaderAdhoc[] = '';
                        $arrHeaderAdhoc[] = 'Adhoc';
                        $resultBelow[] = $arrHeaderAdhoc;
                        //</editor-fold>

                        //<editor-fold desc="Header Adhoc">
                        $arrHeaderAdhocBelow = [];
                        $arrHeaderAdhocBelow[] = '';
                        $arrHeaderAdhocBelow[] = '';
                        $arrHeaderAdhocBelow[] = '';
                        $startColumnNumberOfAlphabet = 5;
                        $startColumnNumberOfAlphabetFill = 4;
                        $startColumnNumberOfAlphabetBorder = 5;
                        foreach ($data->budgetBellow5BioNonContractual->header as $fields) {
                            $arrHeaderAdhocBelow[] = $fields->headerName;
                            if ($fields->headerName !== '') {
                                $dataHeaderMerged = [
                                    'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                                    'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                                    'value'         =>  $fields->headerName
                                ];
                                array_push($arrSubHeaderMerged, $dataHeaderMerged);
                                $startColumnNumberOfAlphabet = $startColumnNumberOfAlphabet + 2;
                                $arrHeaderAdhocBelow[] = '';

                                array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;

                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                                array_push($arrHeaderBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBorder) . $startRowSubHeaderMerged );
                                $startColumnNumberOfAlphabetBorder = $startColumnNumberOfAlphabetBorder + 1;
                            }
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $arrHeaderAdhocBelow[] = 'Total Count of Promo ID';
                        $arrHeaderAdhocBelow[] = 'Sum of Total Cost';
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet) . $startRowSubHeaderMerged,
                            'value'         => 'Total Count of Promo ID'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $dataHeaderMerged = [
                            'range'         => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged . ':' . $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged + 1,
                            'coordinate'    => $formatted->getNameFromNumber($startColumnNumberOfAlphabet + 1) . $startRowSubHeaderMerged,
                            'value'         => 'Sum of Total Cost'
                        ];
                        array_push($arrSubHeaderMerged, $dataHeaderMerged);
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrHeaderAdhocBelow;
                        //</editor-fold>

                        //<editor-fold desc="Sub Header Adhoc">
                        $arrSubHeaderAdhocBelow = [];
                        $arrSubHeaderAdhocBelow[] = '';
                        $arrSubHeaderAdhocBelow[] = '';
                        $arrSubHeaderAdhocBelow[] = '';
                        $arrSubHeaderAdhocBelow[] = 'Channel';
                        $startColumnNumberOfAlphabetFill = 4;

                        array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                        $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        foreach ($data->budgetBellow5BioNonContractual->subHeader[0]->valueSubHeader as $fields) {
                            $arrSubHeaderAdhocBelow[] = $fields->headerName;
                            array_push($arrHeaderFill, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFill) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFill = $startColumnNumberOfAlphabetFill + 1;
                        }
                        $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                        $resultBelow[] = $arrSubHeaderAdhocBelow;
                        //</editor-fold>

                        //<editor-fold desc="Body Adhoc">
                        foreach ($data->budgetBellow5BioNonContractual->body as $body) {
                            $startColumnNumberOfAlphabetBodyBorder = 4;
                            $arrBodyAdhocBelow = [];
                            $arrBodyAdhocBelow[] = '';
                            $arrBodyAdhocBelow[] = '';
                            $arrBodyAdhocBelow[] = '';
                            $arrBodyAdhocBelow[] = $body->text;
                            array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder) . $startRowSubHeaderMerged);
                            foreach ($body->value as $value) {
                                $arrBodyAdhocBelow[] = $value->value;
                                array_push($arrBodyBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetBodyBorder + 1) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetBodyBorder = $startColumnNumberOfAlphabetBodyBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrBodyAdhocBelow;
                        }
                        //</editor-fold>

                        //<editor-fold desc="Footer Adhoc">
                        $startColumnNumberOfAlphabetFooterBorder = 4;
                        foreach ($data->budgetBellow5BioNonContractual->footer as $footer) {
                            $arrFooterAdhocBelow = [];
                            $arrFooterAdhocBelow[] = '';
                            $arrFooterAdhocBelow[] = '';
                            $arrFooterAdhocBelow[] = '';
                            $arrFooterAdhocBelow[] = $footer->text;
                            array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                            $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            foreach ($footer->value as $value) {
                                $arrFooterAdhocBelow[] = $value->value;
                                array_push($arrFooterBorder, $formatted->getNameFromNumber($startColumnNumberOfAlphabetFooterBorder) . $startRowSubHeaderMerged);
                                $startColumnNumberOfAlphabetFooterBorder = $startColumnNumberOfAlphabetFooterBorder + 1;
                            }

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrFooterAdhocBelow;
                        }
                        //</editor-fold>

                        for ($i=0; $i<3; $i++) {
                            $arrSeparatorAdhoc = [];
                            $arrSeparatorAdhoc[] = '';

                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arrSeparatorAdhoc;
                        }
                        //</editor-fold>

                        $centerTextBelow = 'A' . $startRowSubHeaderMerged;
                        for ($i=0; $i<17; $i++) {
                            $arr = [];

                            if ($i === 6) {
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = 'Approved by,';
                            } else if ($i === 11) {
                                $signatureData = $data->emailApprovalSigned;
                                $arr[] = '';
                                if ($signatureData) {
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = '';
                                    $arr[] = ($signatureData[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                    $arr[] = '';
                                    $arr[] = ($signatureData[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($signatureData[0]->approvedOn)) : "");
                                }
                            } else if ($i === 15) {
                                $signatureData = $data->emailApproval;
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->username ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->username ?? '');
                            } else if ($i === 16) {
                                $signatureData = $data->emailApproval;
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = '';
                                $arr[] = ($signatureData[0]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[1]->jobtitle ?? '');
                                $arr[] = '';
                                $arr[] = ($signatureData[2]->jobtitle ?? '');
                            } else {
                                $arr[] = '';
                            }


                            $startRowSubHeaderMerged = $startRowSubHeaderMerged + 1;
                            $resultBelow[] = $arr;
                        }
                        $centerTextBelow = $centerTextBelow . ':L' . $startRowSubHeaderMerged;

                        $headerBelow = [
                            ['', '', '', 'APPROVAL SHEET'],
                            ['', '', '', 'OPTIMA PROMO ID ' . strtoupper($data->periodDesc). ' ' . $data->period . ' (Below IDR 5bio)'],
                            ['']
                        ];
                        $formatCellBelow = [
                            'E'     => NumberFormat::builtInFormatCode(37),
                            'F'     => NumberFormat::builtInFormatCode(37),
                            'G'     => NumberFormat::builtInFormatCode(37),
                            'H'     => NumberFormat::builtInFormatCode(37),
                            'J'     => NumberFormat::builtInFormatCode(37),
                            'I'     => NumberFormat::builtInFormatCode(37),
                            'K'     => NumberFormat::builtInFormatCode(37),
                            'L'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        //<editor-fold desc="Details">
                        $resultDetails=[];
                        foreach ($data->budgetPromoList as $fields) {
                            $arr = [];
                            $arr[] = $fields->period;
                            $arr[] = $fields->promoId;
                            $arr[] = $fields->entity;
                            $arr[] = $fields->distributor;
                            $arr[] = $fields->activity;
                            $arr[] = $fields->activityName;
                            $arr[] = $fields->channel;
                            $arr[] = $fields->account;
                            $arr[] = date('d-m-Y', strtotime($fields->promoStart));
                            $arr[] = date('d-m-Y', strtotime($fields->promoEnd));
                            $arr[] = $fields->investment;
                            $arr[] = $fields->investment5bio;
                            $arr[] = $fields->brand;
                            $arr[] = $fields->tradingTermAdhoc;
                            $arr[] = $fields->monthStart;
                            $arr[] = $fields->monthEnd;

                            $resultDetails[] = $arr;
                        }
                        $headerDetails = [
                            ['Period', 'Promo ID', 'Entity', 'Distributor', 'Activity', 'Activity Name', 'Channel', 'Account', 'Promo Start', 'Promo End', 'Cost', 'Cost >5bio', 'Brand', 'Trading Term / Adhoc', 'Month Start', 'Month End']
                        ];
                        $headerStyleDetails = 'A1:P1'; //Header Column Bold and color
                        $formatCellDetails = [
                            'K'     => NumberFormat::builtInFormatCode(37)
                        ];
                        //</editor-fold>

                        $fileNameExcel = 'Approval Sheet Details (XLS)';
                        $export = new ExportBudgetApprovalRequest($data, $resultAbove, $resultBelow, $resultDetails,
                            $headerAbove, $headerBelow, $headerDetails, $centerTextAbove,
                            $headerStyleAbove, $headerStyleDetails, $arrSubHeaderMerged, $arrHeaderFill, $arrHeaderBorder, $arrBodyBorder, $arrFooterBorder, $centerTextBelow,
                            $formatCellAbove, $formatCellBelow, $formatCellDetails);
                        Excel::store($export, $path . $fileNameExcel  . ' ' . $uuid . '.xlsx', 'optima');
                        //</editor-fold>

                        if ($email[0]->seq > 3) {
                            $linkAttachment = [
                                'linkDownloadFileAbove' => config('app.url') . '/' . $path . $fileNameAbove . ' ' . $uuid . '.pdf',
                                'fileNameAbove'         => $fileNameAbove,
                                'linkDownloadFileExcel' => config('app.url') . '/' . $path . $fileNameExcel  . ' ' . $uuid . '.xlsx',
                                'fileNameExcel'         => $fileNameExcel,
                            ];
                        } else {
                            $linkAttachment = [
                                'linkDownloadFileAbove' => config('app.url') . '/' . $path . $fileNameAbove . ' ' . $uuid . '.pdf',
                                'fileNameAbove'         => $fileNameAbove,
                                'linkDownloadFileBelow' => config('app.url') . '/' . $path . $fileNameBelow  . ' ' . $uuid .'.pdf',
                                'fileNameBelow'         => $fileNameBelow,
                                'linkDownloadFileExcel' => config('app.url') . '/' . $path . $fileNameExcel  . ' ' . $uuid . '.xlsx',
                                'fileNameExcel'         => $fileNameExcel,
                            ];
                        }
                    }
                    //</editor-fold>

                    $apiEmail = config('app.api') . '/tools/email';
                    $dataUrl = json_encode([
                        'batchId'       => $batchId,
                        'profileId'     => $email[0]->approvedBy,
                        'profileEmail'  => $email[0]->email
                    ]);
                    $url = encrypt($dataUrl);
                    $linkApproval = config('app.url') . '/budget/approval-request/action?a=approve&c='. $request['c'] .'&i=' . $url;
                    $linkReject = config('app.url') . '/budget/approval-request/action?a=reject&i=' . $url;
                    $category = $request['c'];

                    $message = view('Budget::budget-approval-request.email-body', compact('data', 'period', 'periodDesc', 'linkApproval', 'linkAttachment', 'linkReject', 'category'))->toHtml();

                    $bodyEmail = [
                        'email'         => $email[0]->email,
                        'subject'       => '[Approval Needed] - OPTIMA Promo ID '. $periodDesc . ' ' . $period,
                        'body'          => $message,
                        'cc'            => ($email[0]->ccEmail ?? "")
                    ];
                    Log::info('post API ' . $apiEmail);
                    Log::info('email ' . $email[0]->email);
                    $responseEmail = Http::timeout(180)->asForm()->withToken($this->token)->post($apiEmail, $bodyEmail);
                    Log::info($responseEmail->status());
                    Log::info($responseEmail->reason());
                    if ($responseEmail->status() === 200) {
                        Log::info("Send Email Success " . $email[0]->email);
                    } else {
                        Log::info("error : Send Email Failed " . $email[0]->email);
                    }
                }

                $res = [
                    'error'     => false,
                    'message'   => (json_decode($response)->message ?? "")
                ];
            } else {
                $res = [
                    'error'     => true,
                    'message'   => (json_decode($response)->message ?? "")
                ];
            }

            return $res;
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [
                'error'     => true,
                'message'   => $e->getMessage()
            ];
        }
    }

    public function reject(Request $request): array
    {
        $api = config('app.api') . '/budget/approvalrequest/reject';
        try {
            $data = json_decode(decrypt($request['i']));
            Log::info('post API ' . $api);
            Log::info('payload ' . json_encode($data));
            $response = Http::timeout(180)->post($api, $data);
            if ($response->status() === 200) {
                $res = [
                    'error'     => false,
                    'message'   => (json_decode($response)->message ?? "")
                ];
            } else {
                $res = [
                    'error'     => true,
                    'message'   => (json_decode($response)->message ?? "")
                ];
            }
            return $res;
        } catch (Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [
                'error'     => true,
                'message'   => $e->getMessage()
            ];
        }
    }
}
