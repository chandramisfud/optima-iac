<?php
namespace App\Console;

use App\Exports\Export;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Exception;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

class SendEmailApprovalRegular
{
    public function __invoke()
    {
        Log::info('running job schedule');
        $this->sendingEmailApprovalRegular();
    }

    private function sendingEmailApprovalRegular()
    {
        Log::info('Send Email Approval Regular');
        $apiEmail = config('app.api'). '/tools/email';
        try {
            $dataRegular = $this->getDataRegular();
            $year1 = date('Y');
            $year2 = date('Y');
            $monthNum1  = date('m');
            $monthNum2  = (int) $monthNum1 + 1;
            if($monthNum2 === 13) {
                $year2 = (int) $year2 + 1;
                $monthNum2 = 1;
            }

            $monthName1 = date("F", mktime(0, 0, 0, $monthNum1, 10));
            $monthName2 = date("F", mktime(0, 0, 0, $monthNum2, 10));

            $strMonth1 = $monthName1;
            $strMonth2 = $monthName2;

            $subjectMonth = $strMonth1 . ' ' . $year1 . ' & '. $strMonth2 . ' ' . $year2;

            if(!empty($dataRegular['data']->data)){
                $dataEmail = $dataRegular['data']->email;
                $dataListing = $dataRegular['data']->lsPromo;
                $data = $dataRegular['data']->data;

                $fileName = $this->createListingPromo($dataListing, $subjectMonth);

                $message = view('Promo::email-reminder.promo-approval-reminder-regular.view_email2', compact('data', 'strMonth1', 'strMonth2', 'fileName'))->render();

                $reqData = [
                    'email'     => $dataEmail,
                    'subject'   => "Promo ID Approval Reminder : " . $subjectMonth,
                    'body'      => $message,
                    'cc'        => [],
                    'bcc'       => []
                ];

                $response = Http::asForm()->post($apiEmail, $reqData);
                if ($response->status() === 200) {
                    Log::info($apiEmail);
                    Log::info($response);
                    Log::info('Send Mail Success');
                } else {
                    Log::error($apiEmail);
                    Log::error($response->status());
                    Log::error($response->reason());
                    Log::error('Send Mail Failed');
                }
            } else {
                Log::warning('No Data to Send Email');
            }
        } catch (Exception $e) {
            Log::error('get API ' . $apiEmail);
            Log::error($e->getMessage());
        }
    }

    private function getDataRegular(): array
    {
        Log::info('Get Data Regular');
        $api = config('app.api'). '/tools/scheduler/listingpromoreporting/promoapprovalreminder/regularemail';

        try {
            $response = Http::get($api);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                return array(
                    'error'     => true,
                    'data'      => $resVal
                );
            } else {
                Log::error($response->status());
                Log::error($response->reason());
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $response->reason()
                );
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

    private function createListingPromo($data, $subjectMonth): bool|string
    {
        try {
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
                $arr[] = date('Y-m-d' , strtotime($fields->LastStatusDate));
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
                    'Remaining Balance', 'GAP', 'Submission Deadline','Status Submission', 'DN Claim', 'DN Paid','Investment Before Closure','Closed Balance', 'Closure Status', 'Recon Status', 'Last Recon Status', 'Cancel Reason', 'Actual Sales', 'Initiator Notes']];
            $formatCell =  [
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

            $mc = microtime(true);
            $fileName = 'ListingPromoReporting-' . date('Y-m-d His') . $mc . '.xlsx';

            $path = '/assets/media/promoreminder/';
            if (!Storage::disk('optima')->exists($path)) {
                Storage::disk('optima')->makeDirectory($path);
            }

            $export = new Export($result, $heading, $title, $header, $formatCell);
            Excel::store($export, $fileName, 'promoreminder');
            return config('app.url') . '/assets/media/promoreminder/' . $fileName;
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'error' => true,
                'data' => [],
                'message' => $e->getMessage()
            ]);
        }
    }

}
