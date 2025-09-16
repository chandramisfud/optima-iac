<?php

namespace App\Modules\FinanceReport\Controllers;

use App\Exports\Export;
use App\Exports\FinanceReport\ExportPromoApprovalReminderPeriode;
use App\Exports\FinanceReport\ExportPromoApprovalReminderSingle;
use App\Helpers\Formatted;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use Wording;
use File;

class ListingPromoReporting extends Controller
{
    protected mixed $token;
    protected mixed $formatFunction;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');
            $this->formatFunction = new Formatted();

            return $next($request);
        });
    }

    public function landingPage()
    {
        $title = "Listing Promo Reporting";
        return view('FinanceReport::listing-promo-reporting.index', compact('title'));
    }

    public function promoApprovalReminderPage()
    {
        $title = "Promo Approval Reminder";
        return view('FinanceReport::listing-promo-reporting.promoApprovalReminder-form', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            ($request['search']['value']==null) ? $search="" : $search=$request['search']['value'];
            $length = $request['length'];
            $page = $page;
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
                'Period'                    => $request->period,
                'profileId'                 => 'admin',
                'CategoryId'                => $request->categoryId,
                'EntityId'                  => $request->entityId,
                'DistributorId'             => $request->distributorId,
                'BudgetParentId'            => 0,
                'ChannelId'                 => $request->channelId,
                'CreateFrom'                => $request->startFrom,
                'CreateTo'                  => $request->startTo,
                'StartFrom'                 => $request->startFrom,
                'StartTo'                   => $request->startTo,
                'SubmissionParam'           => 0,
                'Search'                    => $search,
                'PageNumber'                => $page,
                'PageSize'                  => (int) $length,
                'SortColumn'                => $SortColumn,
                'SortDirection'             => $sort
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

    public function getListPromoApprovalReminder(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder';
        $query = [
            'year'          => $request->year,
            'monthStart'    => $request->month_start,
            'monthEnd'      => $request->month_end,
        ];
        Log::info('Get API ' . $api);
        Log::info('payload' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            $message = json_decode($response)->message;
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values->data,
                    'gap'       => json_decode($response)->values->gap,
                    'message'   => $message,
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

    public function getListUserGroup(Request $request)
    {
        $api = config('app.api'). '/finance-report/promosubmission/usergroup';
        $query = [
            'usergroupid'            => $request->usergroupid,
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

    public function getDatauserList(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder/sourceemail';
        $query = [
            'usergroupid'       => $request->usergroupid,
            'userlevel'         => $request->userlevel,
            'isdeleted'         => $request->status
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

    public function getDatauserRegular(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder/regularemail/data';

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

    public function getListCategory(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/category';
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

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/entity';
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

    public function getListDistributorByEntityId(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/distributor';
        $query = [
            'EntityId'           => $request->entityId,
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

    public function getListChannel(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/channel';
        $query = [
            'userid'           => 'admin',
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

    public function getDataGap(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/investmentnotif';
        $query = [
            'periode'   => $request->periode,
            'userid'    => 'admin',
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

    public function getDataPromoApprovalReminderConfig(Request $request)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder/autoconfig';
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

    public function getDataPromoApprovalReminder($year, $month_start, $month_end)
    {
        $api = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder';
        $query = [
            'year'          => $year,
            'monthStart'    => $month_start,
            'monthEnd'      => $month_end,
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

    public function createListingPromo($data, $subjectMonth)
    {
        $result = [];
        foreach ($data as $fields) {
            $arr = [];

            $arr[] = $fields->PromoPlanRefId;
            $arr[] = $fields->TsCoding;
            $arr[] = $fields->PromoNumber;
            $arr[] = $fields->Entity;
            $arr[] = $fields->Distributor;
            $arr[] = $fields->BudgetSource;
            $arr[] = $fields->Initiator;
            $arr[] = date('Y-m-d' , strtotime($fields->LastUpdate));

            $arr[] = $fields->RegionDesc;
            $arr[] = $fields->ChannelDesc;
            $arr[] = $fields->SubChannelDesc;
            $arr[] = $fields->AccountDesc;
            $arr[] = $fields->SubAccountDesc;
            $arr[] = $fields->BrandDesc;
            $arr[] = $fields->SKUDesc;

            $arr[] = $fields->SubCategory;
            $arr[] = $fields->SubactivityType;
            $arr[] = $fields->Activity;
            $arr[] = $fields->SubActivity;
            $arr[] = $fields->ActivityDesc;

            $arr[] = $fields->Mechanism;
            $arr[] = date('Y-m-d' , strtotime($fields->StartPromo));
            $arr[] = date('Y-m-d' , strtotime($fields->EndPromo));
            $arr[] = date('Y-m-d' , strtotime($fields->CreateOn));
            $arr[] = $fields->LastStatus;
            $arr[] = ($this->formatFunction->isNotDate($fields->LastStatusDate) ? '---' : date('d-m-Y' , strtotime($fields->LastStatusDate)));
            if($fields->sendback_notes==''){
                $arr[] = '';
            }else{
                $arr[] = $fields->sendback_notes . ' on ' . $fields->sendback_notes_date;
            }
            $arr[] = $fields->NormalSales;
            $arr[] = $fields->IncrSales;
            $arr[] = $fields->Target;
            $arr[] = $fields->Investment;
            $arr[] = $fields->InvestmentTypeRefId;
            $arr[] = $fields->InvestmentTypeDesc;
            if($fields->InvestmentTypeRefId_promo=='----' || $fields->InvestmentTypeRefId_promo=='' || $fields->InvestmentTypeRefId_promo==null)
            {
                $arr[] = '----';
            }else{
                $arr[] = $fields->InvestmentTypeRefId_promo. ' - ' . $fields->InvestmentTypeDesc_promo;
            }

            $arr[] = $fields->Roi;
            $arr[] = $fields->CostRatio / 100;
            $arr[] = $fields->RemainingBalance;
            $arr[] = $fields->Gap;
            if($fields->submission_deadline == null){
                $arr[] = '';
            }else{
                $arr[] = $fields->submission_deadline;
            }
            if($fields->OnTime){
                $arr[] = 'On-Time';
            }else{
                $arr[] = 'Late';
            }

            $arr[] = $fields->DnClaim;
            $arr[] = $fields->DnPaid;
            $arr[] = $fields->investmentBfrClose;
            $arr[] = $fields->investmentClosedBalance;
            if($fields->ClosureStatus){
                $arr[] = 'Closed';
            }else{
                $arr[] = 'Open';
            }
            $arr[] = $fields->ReconStatus;
            // $arr[] = $fields->lastReconStatus;
            if($fields->LastReconStatus==null){
                $arr[] = '----';
            }else{
                $arr[] = $fields->LastReconStatus;
            }
            $arr[] = $fields->CancelReason;
            $arr[] = $fields->actual_sales;
            $arr[] = $fields->initiator_notes;

            $result[] = $arr;
        }

        $heading = [
            ['Listing Promo Reporting'],
            ['Budget Year : ' . $subjectMonth ],
            ['Promo Plan ID','TS Code', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Brand', 'SKU',
                'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio',
                'Remaining Balance', 'GAP', 'Submission Deadline','Status Submission', 'DN Claim', 'DN Paid','Investment Before Closure','Closed Balance', 'Closure Status', 'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes']];
        $formatcell =  [
            'AB' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AC' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AF' => NumberFormat::FORMAT_TEXT,
            'AI' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AJ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AK' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AO' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AP' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AW' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'AU' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
        ];

        $title = 'A1:AX1'; //Report Title Bold and merge
        $header = 'A3:AX3'; //Header Column Bold and color

        $path = public_path().'/assets/media/promoreminder/';
        if (!File::isDirectory($path)) {
            File::makeDirectory($path,0775,true);
        }

        $export = new Export($result, $heading, $title, $header, $formatcell);
        $mc = microtime(true);
        $fileName = 'ListingPromoReporting-' . date('Y-m-d His') . $mc . '.xlsx';
        Excel::store($export, $fileName, 'promoreminder');
        return config('app.url') . '/assets/media/promoreminder/' . $fileName;
    }

    public function sendEmail(Request $request) {
        $api_email = config('app.api'). '/tools/email';
        $api_saveEmail = config('app.api'). '/finance-report/listingpromoreporting/promoapprovalreminder/regularemail';
        try {
            $data = [
                "configEmail"   => json_decode($request->emailSave),
            ];
            Log::info('Put API ' . $api_saveEmail);
            Log::info('payload ' . json_encode($data));
            $responseSaveEmail =  Http::timeout(180)->withToken($this->token)->put($api_saveEmail, $data);
            if ($responseSaveEmail->status() === 200) {
                if(json_decode($responseSaveEmail)->error){
                    return array(
                        'error'         => true,
                        'message'       => 'Update Promo Approval Reminder Send Email Failed',
                    );
                }else{
                    $dataApprovalReminder = $this->getDataPromoApprovalReminder($request->year, $request->month_start, $request->month_end)['data'];
                    $data = $dataApprovalReminder->data;
                    $strMonthStart = $request->strMonthStart;
                    $strMonthEnd = $request->strMonthEnd;
                    $dataListing = $dataApprovalReminder->lsPromo;

                    if($strMonthStart === $strMonthEnd){
                        $subjectMonth = $strMonthStart . ' ' . $request->year;
                        $fileName = $this->createListingPromo($dataListing, $subjectMonth);
                        $message = view('FinanceReport::listing-promo-reporting.view_email1', compact('data', 'strMonthStart', 'strMonthEnd', 'fileName'))->render();
                    }else{
                        $subjectMonth = $strMonthStart . ' & '. $strMonthEnd . ' ' .  $request->year;
                        $fileName = $this->createListingPromo($dataListing, $subjectMonth);
                        $message = view('FinanceReport::listing-promo-reporting.view_email2', compact('data', 'strMonthStart', 'strMonthEnd', 'fileName'))->render();
                    }

                    $dataEmail = [
                        'email'     => json_decode($request->emailSend),
                        'subject'   => "Promo Approval Reminder " . $subjectMonth,
                        'body'      => $message,
                        'cc'        => [],
                        'bcc'       => []
                    ];
                    $result_email =  Http::timeout(180)->asForm()->withToken($this->token)->post($api_email, $dataEmail);
                    if ($result_email->status() === 200) {
                        return array(
                            'error'     => false,
                            'message'   => "Send Mail Success"
                        );
                    } else {
                        return array(
                            'error'     => true,
                            'message'   => "Send Mail Failed"
                        );
                    }
                }
            } else {
                $message = json_decode($responseSaveEmail)->message;
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('Post API ' . $api_email);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function configuration(Request $request)
    {
        $api = config('app.api') . '/finance-report/listingpromoreporting/promoapprovalreminder/autoconfig';

        if($request->eod=='on'){ $eod=true; }else{ $eod=false; }
        if($request->autorun=='on'){ $autorun=true; }else{ $autorun=false; }

        $data = [
            "id"            => $request->id,
            "dt1"           => $request->dt1,
            "dt2"           => $request->dt2,
            "eod"           => $eod,
            "autoRun"       => $autorun,
            "configEmail"   => json_decode($request->emailsave),
        ];
        Log::info('PUT API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->put($api, $data);
            if ($response->status() === 200) {
                if(json_decode($response)->error){
                    $error = true;
                    $messageResponse = 'Update Promo Approval Reminder Configuration Failed';
                }else{
                    $error = false;
                    $messageResponse = 'Update Promo Approval Reminder Configuration has been successfuly';
                }

                return array(
                    'error'         => $error,
                    'message'       => $messageResponse,
                );
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error'     => true,
                    'message'   => $message,
                );
            }
        } catch (\Exception $e) {
            Log::error('PUT API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Update Failed"
            );
        }
    }

    public function exportXlsGap(Request $request) {
        $api = config('app.api'). '/finance-report/listingpromoreporting/investmentnotif';
        $query = [
            'periode'   => $request->periode,
            'userid'    => 'admin',
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->gaP_list;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];

                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->dnClaim_promo;
                    $arr[] = $fields->dnPaid_promo;
                    $arr[] = $fields->dnNumber;
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->gaP_DNClaim;
                    $arr[] = $fields->gaP_DNPaid;

                    $result[] = $arr;
                }

                $filename = 'GAPDNClaim-';
                $title = 'A1:I1'; //Report Title Bold and merge
                $header = 'A4:I4'; //Header Column Bold and color
                $heading = [
                    ['GAP DN Claim'],
                    ['Budget Year : ' . $request->periode],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Budget', 'Promo Number', 'DN Claim', 'DN Paid', 'DN Number', 'Total Claim', 'Total Paid', 'GAP Claim', 'GAP Paid']
                ];
                $formatcell =  [
                    'C' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'D' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                ];

                $export = new Export($result, $heading, $title, $header, $formatcell);
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
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/finance-report/listingpromoreporting';
        $query = [
            'Period'                    => $request->period,
            'profileID'                 => 'admin',
            'CategoryId'                => $request->categoryId,
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => $request->channelId,
            'CreateFrom'                => $request->startFrom,
            'CreateTo'                  => $request->startTo,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'promoNumber',
            'SortDirection'             => 'desc'
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

                    $arr[] = $fields->promoPlanRefId;
                    $arr[] = $fields->tsCoding;
                    $arr[] = $fields->categoryLongDesc;
                    $arr[] = $fields->promoNumber;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->distributor;
                    $arr[] = $fields->budgetSource;
                    $arr[] = $fields->initiator;
                    $arr[] = date('d-m-Y' , strtotime($fields->lastUpdate));
                    $arr[] = $fields->regionDesc;
                    $arr[] = $fields->channelDesc;
                    $arr[] = $fields->subChannelDesc;
                    $arr[] = $fields->accountDesc;
                    $arr[] = $fields->subAccountDesc;
                    $arr[] = $fields->groupBrandDesc;
                    $arr[] = $fields->brandDesc;
                    $arr[] = $fields->skuDesc;
                    $arr[] = $fields->subCategory;
                    $arr[] = $fields->subactivityType;
                    $arr[] = $fields->activity;
                    $arr[] = $fields->subActivity;
                    $arr[] = $fields->activityDesc;
                    $arr[] = $fields->mechanism;
                    $arr[] = date('d-m-Y' , strtotime($fields->startPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->endPromo));
                    $arr[] = date('d-m-Y' , strtotime($fields->createOn));
                    $arr[] = $fields->lastStatus;
                    $arr[] = ($this->formatFunction->isNotDate($fields->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($fields->lastStatusDate)));
                    if($fields->sendback_notes==''){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->sendback_notes . ' on ' . $fields->sendback_notes_date;
                    }
                    $arr[] = $fields->normalSales;
                    $arr[] = $fields->incrSales;
                    $arr[] = $fields->target;
                    $arr[] = $fields->investment;
                    $arr[] = $fields->investmentTypeRefId;
                    $arr[] = $fields->investmentTypeDesc;
                    if($fields->investmentTypeRefId_promo=='----' || $fields->investmentTypeRefId_promo=='' || $fields->investmentTypeRefId_promo==null)
                    {
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->investmentTypeRefId_promo. ' - ' . $fields->investmentTypeDesc_promo;
                    }
                    $arr[] = $fields->roi / 100;
                    $arr[] = $fields->costRatio / 100;
                    $arr[] = $fields->remainingBalance;
                    $arr[] = $fields->gap;
                    if($fields->submission_deadline == null){
                        $arr[] = '';
                    }else{
                        $arr[] = $fields->submission_deadline;
                    }
                    if($fields->onTime){
                        $arr[] = 'On-Time';
                    }else{
                        $arr[] = 'Late';
                    }
                    $arr[] = $fields->dnClaim;
                    $arr[] = $fields->dnPaid;
                    $arr[] = $fields->investmentBfrClose;
                    $arr[] = $fields->investmentClosedBalance;
                    if($fields->closureStatus){
                        $arr[] = 'Closed';
                    }else{
                        $arr[] = 'Open';
                    }
                    $arr[] = $fields->reconStatus;
                    if($fields->lastReconStatus==null){
                        $arr[] = '----';
                    }else{
                        $arr[] = $fields->lastReconStatus;
                    }
                    $arr[] = $fields->cancelReason;
                    $arr[] = $fields->actual_sales;
                    $arr[] = $fields->initiator_notes;
                    $arr[] = $fields->budgetApprovalStatus;
                    $arr[] = (($fields->budgetApprovalStatusOn === '---') ? '---' : date('d-m-Y', strtotime($fields->budgetApprovalStatusOn)));
                    $arr[] = $fields->budgetApprovalStatusBy;
                    $arr[] = $fields->budgetDeployStatus;
                    $arr[] = (($fields->budgetDeployStatusOn === '---')  ? '---' : date('d-m-Y', strtotime($fields->budgetDeployStatusOn)));
                    $arr[] = $fields->budgetDeployStatusBy;
                    $arr[] = $fields->batchId;
                    $arr[] = $fields->mainActivity;

                    $result[] = $arr;
                }
                $submissionParam = 30;
                $heading = [
                    ['Listing Promo Reporting'],
                    ['Budget Year : ' . $request->period],
                    ['Category : ' . $request->category],
                    ['Entity : ' . $request->entity],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Submission Days : ' . $submissionParam],
                    ['Planning ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Brand', 'Sub Brand', 'SKU', 'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Status', 'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI', 'Cost Ratio', 'Remaining Balance', 'GAP', 'Submission Deadline', 'Status Submission', 'DN Claim', 'DN Paid', 'Investment Before Closure', 'Closed Balance', 'Closure Status', 'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                        'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval by', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By', 'Batch ID', 'Main Activity']
                ];
                $formatCell =  [
                    'AD' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AE' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AF' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AG' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AK' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AL' => NumberFormat::FORMAT_PERCENTAGE_00,
                    'AM' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AQ' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AR' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AS' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AT' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'AY' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
                    'BG' => NumberFormat::FORMAT_TEXT,
                ];
                $title = 'A1:BH1'; //Report Title Bold and merge
                $header = 'A7:BH7'; //Header Column Bold and color
                $filename = 'ListingPromoReporting-';
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
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportCsv(Request $request) {
        $api = config('app.api'). '/finance-report/listingpromoreporting';
        $query = [
            'Period'                    => $request->period,
            'profileID'                 => 'admin',
            'CategoryId'                => $request->categoryId,
            'EntityId'                  => $request->entityId,
            'DistributorId'             => $request->distributorId,
            'BudgetParentId'            => 0,
            'ChannelId'                 => $request->channelId,
            'CreateFrom'                => $request->startFrom,
            'CreateTo'                  => $request->startTo,
            'StartFrom'                 => $request->startFrom,
            'StartTo'                   => $request->startTo,
            'SubmissionParam'           => 0,
            'Search'                    => '',
            'PageNumber'                => 0,
            'PageSize'                  => -1,
            'SortColumn'                => 'promoNumber',
            'SortDirection'             => 'asc'
        ];
        Log::info('get API ' . $api);
        Log::info('payload ' . json_encode($query));
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api, $query);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values->data;

                $mc = microtime(true);
                $fileName = 'ListingPromoReporting-' . date('Y-m-d His') . $mc . '.csv';

                $headers = array(
                    "Content-type"        => "text/csv",
                    "Content-Disposition" => "attachment; filename=$fileName",
                    "Pragma"              => "no-cache",
                    "Cache-Control"       => "must-revalidate, post-check=0, pre-check=0",
                    "Expires"             => "0"
                );

                $columns = array(
                    'Planning ID', 'TS Code', 'Category', 'Promo ID', 'Entity', 'Distributor', 'Budget', 'Initiator', 'Last Update', 'Region', 'Channel', 'Sub Channel', 'Account', 'Sub Account',
                    'Brand', 'Sub Brand', 'SKU', 'Sub Category', 'Sub Activity Type', 'Activity', 'Sub Activity', 'Activity Name', 'Mechanism', 'Promo Start', 'Promo End', 'Creation Date', 'Status',
                    'Status Date', 'Last Sendback Note', 'Baseline Sales', 'Incr Sales', 'Total Sales', 'Investment', 'Investment Code', 'Investment Type', 'Last Promo Investment Type', 'ROI',
                    'Cost Ratio', 'Remaining Balance', 'GAP', 'Submission Deadline', 'Status Submission', 'DN Claim', 'DN Paid', 'Investment Before Closure', 'Closed Balance', 'Closure Status',
                    'Recon Status', 'Approval Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes',
                    'Budget Mass Approval Status', 'Budget Mass Approval Date', 'Budget Mass Approval By', 'Budget Deploy Status', 'Budget Deploy Date', 'Budget Deploy By', 'Batch ID', 'Main Activity'
                );

                $period = $request['period'];
                $category = $request['category'];
                $entity = $request['entity'];
                $submissionParam = 30;
                $callback = function () use ($resVal, $columns, $period, $category, $entity, $submissionParam) {
                    $file = fopen('php://output', 'w');
                    fputcsv($file, ["Listing Promo Reporting"], ",");
                    fputcsv($file, ['Budget Year : ' . $period], ",");
                    fputcsv($file, ['Category : ' . $category], ",");
                    fputcsv($file, ['Entity : ' . $entity], ",");
                    fputcsv($file, ['Date Retrieved : ' . date('Y-m-d')], ",");
                    fputcsv($file, ['Submission Days : ' . $submissionParam], ",");
                    fputcsv($file, $columns, ",");

                    foreach ($resVal as $task) {
                        $row['Planning ID']                 = $task->promoPlanRefId;
                        $row['TS Code']                     = $task->tsCoding;
                        $row['Category']                    = $task->categoryLongDesc;
                        $row['Promo ID']                    = $task->promoNumber;
                        $row['Entity']                      = $task->entity;
                        $row['Distributor']                 = $task->distributor;
                        $row['Budget']                      = $task->budgetSource;
                        $row['Initiator']                   = $task->initiator;
                        $row['Last Update']                 = date('Y-m-d' , strtotime($task->lastUpdate));
                        $row['Region']                      = $task->regionDesc;
                        $row['Channel']                     = $task->channelDesc;
                        $row['Sub Channel']                 = $task->subChannelDesc;
                        $row['Account']                     = $task->accountDesc;
                        $row['Sub Account']                 = $task->subAccountDesc;
                        $row['Brand']                       = $task->groupBrandDesc;
                        $row['Sub Brand']                   = $task->brandDesc;
                        $row['SKU']                         = $task->skuDesc;
                        $row['Sub Category']                = $task->subCategory;
                        $row['Sub Activity Type']           = $task->subactivityType;
                        $row['Activity']                    = $task->activity;
                        $row['Sub Activity']                = $task->subActivity;
                        $row['Activity Name']               = $task->activityDesc;
                        $row['Mechanism']                   = $task->mechanism;
                        $row['Promo Start']                 = date('Y-m-d' , strtotime($task->startPromo));
                        $row['Promo End']                   = date('Y-m-d' , strtotime($task->endPromo));
                        $row['Creation Date']               = date('Y-m-d' , strtotime($task->createOn));
                        $row['Status']                      = $task->lastStatus;
                        $row['Status Date']                 = ($this->formatFunction->isNotDate($task->lastStatusDate) ? '---' : date('d-m-Y' , strtotime($task->lastStatusDate)));
                        $row['Last Sendback Note']          = ($task->sendback_notes === "" ? '' : $task->sendback_notes . ' on ' . $task->sendback_notes_date);
                        $row['Baseline Sales']              = $task->normalSales;
                        $row['Incr Sales']                  = $task->incrSales;
                        $row['Total Sales']                 = $task->target;
                        $row['Investment']                  = $task->investment;
                        $row['Investment Code']             = $task->investmentTypeRefId;
                        $row['Investment Type']             = $task->investmentTypeDesc;
                        $row['Last Promo Investment Type']  = ($task->investmentTypeRefId_promo === '---' || $task->investmentTypeRefId_promo === '' || $task->investmentTypeRefId_promo==null ? '---' : $task->investmentTypeRefId_promo. ' - ' . $task->investmentTypeDesc_promo);
                        $row['ROI']                         = $task->roi;
                        $row['Cost Ratio']                  = $task->costRatio;
                        $row['Remaining Balance']           = $task->remainingBalance;
                        $row['GAP']                         = $task->gap;
                        $row['Submission Deadline']         = ($task->submission_deadline ?? '');
                        $row['Status Submission']           = ($task->onTime ? 'On-Time' : 'Late');
                        $row['DN Claim']                    = $task->dnClaim;
                        $row['DN Paid']                     = $task->dnPaid;
                        $row['Investment Before Closure']   = $task->investmentBfrClose;
                        $row['Closed Balance']              = $task->investmentClosedBalance;
                        $row['Closure Status']              = ($task->closureStatus ? 'Closed' : 'Open');
                        $row['Recon Status']                = $task->reconStatus;
                        $row['Approval Recon Status']       = ($task->lastReconStatus ?? '---');
                        $row['Cancel Reason']               = $task->cancelReason;
                        $row['Actual Sales']                = $task->actual_sales;
                        $row['Initiator Notes']             = $task->initiator_notes;
                        $row['Budget Mass Approval Status'] = $task->budgetApprovalStatus;
                        $row['Budget Mass Approval Date']   = $task->budgetApprovalStatusOn;
                        $row['Budget Mass Approval By']     = $task->budgetApprovalStatusBy;
                        $row['Budget Deploy Status']        = $task->budgetDeployStatus;
                        $row['Budget Deploy Date']          = $task->budgetDeployStatusOn;
                        $row['Budget Deploy By']            = $task->budgetDeployStatusBy;
                        $row['Batch ID']                    = $task->batchId;
                        $row['Main Activity']               = $task->mainActivity;

                        fputcsv($file, array(
                            $row['Planning ID'],
                            $row['TS Code'],
                            $row['Category'],
                            $row['Promo ID'],
                            $row['Entity'],
                            $row['Distributor'],
                            $row['Budget'],
                            $row['Initiator'],
                            $row['Last Update'],
                            $row['Region'],
                            $row['Channel'],
                            $row['Sub Channel'],
                            $row['Account'],
                            $row['Sub Account'],
                            $row['Brand'],
                            $row['Sub Brand'],
                            $row['SKU'],
                            $row['Sub Category'],
                            $row['Sub Activity Type'],
                            $row['Activity'],
                            $row['Sub Activity'],
                            $row['Activity Name'],
                            $row['Mechanism'],
                            $row['Promo Start'],
                            $row['Promo End'],
                            $row['Creation Date'],
                            $row['Status'],
                            $row['Status Date'],
                            $row['Last Sendback Note'],
                            $row['Baseline Sales'],
                            $row['Incr Sales'],
                            $row['Total Sales'],
                            $row['Investment'],
                            $row['Investment Code'],
                            $row['Investment Type'],
                            $row['Last Promo Investment Type'],
                            $row['ROI'],
                            $row['Cost Ratio'],
                            $row['Remaining Balance'],
                            $row['GAP'],
                            $row['Submission Deadline'],
                            $row['Status Submission'],
                            $row['DN Claim'],
                            $row['DN Paid'],
                            $row['Investment Before Closure'],
                            $row['Closed Balance'],
                            $row['Closure Status'],
                            $row['Recon Status'],
                            $row['Approval Recon Status'],
                            $row['Cancel Reason'],
                            $row['Actual Sales'],
                            $row['Initiator Notes'],
                            $row['Budget Mass Approval Status'],
                            $row['Budget Mass Approval Date'],
                            $row['Budget Mass Approval By'],
                            $row['Budget Deploy Status'],
                            $row['Budget Deploy Date'],
                            $row['Budget Deploy By'],
                            $row['Batch ID'],
                            $row['Main Activity']
                        ), ",");
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
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return [];
        }
    }

    public function exportXlsPromoApprovalReminder(Request $request) {
        $dataApprovalReminder = $this->getDataPromoApprovalReminder($request->year, $request->month_start, $request->month_end)['data'];
        $data = $dataApprovalReminder->data;
        $strMonthStart = $request->strMonthStart;
        $strMonthEnd = $request->strMonthEnd;

        if($strMonthStart === $strMonthEnd){
            return $this->downloadxlsSingle($data, $request->strMonthStart);
        }else{
            return $this->downloadxlsPeriode($data, $request->strMonthStart, $request->strMonthEnd);
        }
    }

    public function downloadxlsSingle($data, $strMonthStart) {
        $result = [];

        foreach ($data as $fields) {
            $arr = [];

            if(str_contains($fields->channel, 'Total')){
                $arr[] = '';
            }else{
                $arr[] = $fields->channelhead;
            }

            $arr[] = $fields->channel;
            $arr[] = $fields->kamfcmcem;
            $arr[] = $fields->status2;
            $arr[] = $fields->sb_group;
            $arr[] = $fields->w11;
            $arr[] = $fields->inv11;
            $arr[] = $fields->w12;
            $arr[] = $fields->inv12;
            $arr[] = $fields->wtot;
            $arr[] = $fields->invTot;

            $result[] = $arr;
        }
        $filename = 'PromoApprovalReminder-';
        $title = 'A1:K1'; //Report Title Bold and merge
        $header = 'A4:K4'; //Header Column Bold and color
        $merge1 = 'F4:I4';
        $merge2 = 'F5:G5';
        $merge3 = 'H5:I5';
        $heading = [
            ['Promo Approval Reminder'],
            ['Date Retrieved : ' . date('Y-m-d')],
            [],
            ['Channel Head', 'Channel', 'PIC', 'Status Group', 'Sendback Grouping', $strMonthStart, '', '', '', 'Total Count of Promo ID', 'Total Sum of Investment'],
            ['', '', '', '', '', 'Biweekly 1 (Start 1-15)', '', 'Biweekly 2 (Start 16-31)', ''],
            ['', '', '', '', '', 'Count of Promo ID', 'Sum of Investment', 'Count of Promo ID', 'Sum of Investment', 'Total Count of Promo ID', 'Total Sum of Investment']
        ];
        $formatcell =  [
            'A' => NumberFormat::FORMAT_TEXT,
            'B' => NumberFormat::FORMAT_TEXT,
            'C' => NumberFormat::FORMAT_TEXT,
            'D' => NumberFormat::FORMAT_TEXT,
            'E' => NumberFormat::FORMAT_TEXT,
            'F' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'G' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'H' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'I' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'J' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
            'K' => NumberFormat::FORMAT_NUMBER_COMMA_SEPARATED1,
        ];

        $export = new ExportPromoApprovalReminderSingle($result, $heading, $title, $header, $merge1, $merge2, $merge3, $formatcell);
        $mc = microtime(true);
        return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc . '.xlsx');
    }

    public function downloadxlsPeriode($data, $strMonthStart, $strMonthEnd) {
        $result = [];

        foreach ($data as $fields) {
            $arr = [];

            if(str_contains($fields->channel, 'Total')){
                $arr[] = '';
            }else{
                $arr[] = $fields->channelhead;
            }

            $arr[] = $fields->channel;
            $arr[] = $fields->kamfcmcem;
            $arr[] = $fields->status2;
            $arr[] = $fields->sb_group;

            $arr[] = $fields->w11;
            $arr[] = $fields->inv11;
            $arr[] = $fields->w12;
            $arr[] = $fields->inv12;

            $arr[] = $fields->w21;
            $arr[] = $fields->inv21;
            $arr[] = $fields->w22;
            $arr[] = $fields->inv22;

            $arr[] = $fields->wtot;
            $arr[] = $fields->invTot;

            $result[] = $arr;
        }
        $filename = 'PromoApprovalReminder-';
        $title = 'A1:O1'; //Report Title Bold and merge
        $header = 'A4:O4'; //Header Column Bold and color
        $merge1 = 'F4:I4';
        $merge2 = 'F5:G5';
        $merge3 = 'H5:I5';
        $heading = [
            ['Promo Approval Reminder'],
            ['Date Retrieved : ' . date('Y-m-d')],
            [],
            ['Channel Head', 'Channel', 'PIC', 'Status Group', 'Sendback Grouping', $strMonthStart, '', '', '', $strMonthEnd, '', '', '', 'Total Count of Promo ID', 'Total Sum of Investment'],
            ['', '', '', '', '', 'Biweekly 1 (Start 1-15)', '', 'Biweekly 2 (Start 16-31)', '','Biweekly 1 (Start 1-15)', '', 'Biweekly 2 (Start 16-31)', '', '', ''],
            ['', '', '', '', '', 'Count of Promo ID', 'Sum of Investment', 'Count of Promo ID', 'Sum of Investment', 'Count of Promo ID', 'Sum of Investment', 'Count of Promo ID', 'Sum of Investment', 'Total Count of Promo ID', 'Total Sum of Investment']
        ];
        $formatcell =  [
            'A' => NumberFormat::FORMAT_TEXT,
            'B' => NumberFormat::FORMAT_TEXT,
            'C' => NumberFormat::FORMAT_TEXT,
            'D' => NumberFormat::FORMAT_TEXT,
            'E' => NumberFormat::FORMAT_TEXT,
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
        ];

        $export = new ExportPromoApprovalReminderPeriode($result, $heading, $title, $header, $merge1, $merge2, $merge3, $formatcell);
        $mc = microtime(true);
        return Excel::download($export, $filename . date('Y-m-d His') . ' ' . $mc . '.xlsx');
    }
}
