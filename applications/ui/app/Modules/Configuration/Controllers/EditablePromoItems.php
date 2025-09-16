<?php

namespace App\Modules\Configuration\Controllers;

use App\Exports\Export;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use App\Helpers\CallApi;

class EditablePromoItems extends Controller
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
        $title = "Editable Promo Reconciliation Items";
        return view('Configuration::editable-promo-items.index', compact('title'));
    }

    public function getDataConfig(Request $request)
    {
        $api = '/config/promoitem';
        $query = [
            'categoryShortDesc'     => $request['categoryDesc'],
        ];

        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function submit(Request $request)
    {
        $api = '/config/promoItem';
        $configPromoItem = [
            'budgetYear'        => (int)(($request['budgetYear'] == true) ? 0 : 1),
            'promoPlanning'     => (int)(($request['promoPlanning'] == true) ? 0 : 1),
            'budgetSource'      => (int)(($request['budgetSource'] == true) ? 0 : 1),
            'entity'            => (int)(($request['entity'] == true) ? 0 : 1),
            'distributor'       => (int)(($request['distributor'] == true) ? 0 : 1),
            'subCategory'       => (int)(($request['subCategory'] == true) ? 0 : 1),
            'activity'          => (int)(($request['activity'] == true) ? 0 : 1),
            'subActivity'       => (int)(($request['subActivity'] == true) ? 0 : 1),
            'subActivityType'   => (int)(($request['subCategory'] == true) ? 0 : 1),
            'startPromo'        => (int)(($request['startPromo'] == true) ? 0 : 1),
            'endPromo'          => (int)(($request['endPromo'] == true) ? 0 : 1),
            'activityName'      => (int)(($request['activityName'] == true) ? 0 : 1),
            'initiatorNotes'    => (int)(($request['initiatorNotes'] == true) ? 0 : 1),
            'incrSales'         => (int)(($request['incrSales'] == true) ? 0 : 1),
            'investment'        => (int)(($request['investment'] == true) ? 0 : 1),
            'channel'           => (int)(($request['channel'] == true) ? 0 : 1),
            'subChannel'        => (int)(($request['subChannel'] == true) ? 0 : 1),
            'account'           => (int)(($request['account'] == true) ? 0 : 1),
            'subAccount'        => (int)(($request['subAccount'] == true) ? 0 : 1),
            'region'            => (int)(($request['region'] == true) ? 0 : 1),
            'groupBrand'        => (int)(($request['groupBrand'] == true) ? 0 : 1),
            'brand'             => (int)(($request['brand'] == true) ? 0 : 1),
            'sku'               => (int)(($request['sku'] == true) ? 0 : 1),
            'mechanism'         => (int)(($request['mechanism'] == true) ? 0 : 1),
            'attachment'        => (int)(($request['attachment'] == true) ? 0 : 1),
            'roi'               => (int)(($request['roi'] == true) ? 0 : 1),
            'cr'                => (int)(($request['cr'] == true) ? 0 : 1),
        ];

        $data = [
            'categoryId'        => (int)$request['categoryId'],
            'configPromoItem'   => $configPromoItem
        ];
        $callApi = new CallApi();
        return $callApi->putUsingToken($this->token, $api, $data);
    }

    public function submitDC(Request $request)
    {
        $api = '/config/promoItem';
        $configPromoItem = [
            'budgetYear'        => (int)(($request['budgetYear'] == true) ? 0 : 1),
            'promoPlanning'     => (int)(($request['promoPlanning'] == true) ? 0 : 1),
            'budgetSource'      => (int)(($request['budgetSource'] == true) ? 0 : 1),
            'entity'            => (int)(($request['entity'] == true) ? 0 : 1),
            'distributor'       => (int)(($request['distributor'] == true) ? 0 : 1),
            'subCategory'       => (int)(($request['subCategory'] == true) ? 0 : 1),
            'activity'          => (int)(($request['activity'] == true) ? 0 : 1),
            'subActivity'       => (int)(($request['subActivity'] == true) ? 0 : 1),
            'subActivityType'   => (int)(($request['subActivityType'] == true) ? 0 : 1),
            'startPromo'        => (int)(($request['startPromo'] == true) ? 0 : 1),
            'endPromo'          => (int)(($request['endPromo'] == true) ? 0 : 1),
            'activityName'      => (int)(($request['activityDesc'] == true) ? 0 : 1),
            'initiatorNotes'    => (int)(($request['initiatorNotes'] == true) ? 0 : 1),
            'incrSales'         => (int)(($request['incrSales'] == true) ? 0 : 1),
            'investment'        => (int)(($request['investment'] == true) ? 0 : 1),
            'channel'           => (int)(($request['channel'] == true) ? 0 : 1),
            'subChannel'        => (int)(($request['subChannel'] == true) ? 0 : 1),
            'account'           => (int)(($request['account'] == true) ? 0 : 1),
            'subAccount'        => (int)(($request['subAccount'] == true) ? 0 : 1),
            'region'            => (int)(($request['region'] == true) ? 0 : 1),
            'groupBrand'        => (int)(($request['groupBrand'] == true) ? 0 : 1),
            'brand'             => (int)(($request['brand'] == true) ? 0 : 1),
            'sku'               => (int)(($request['sku'] == true) ? 0 : 1),
            'mechanism'         => (int)(($request['mechanism'] == true) ? 0 : 1),
            'attachment'        => (int)(($request['attachment'] == true) ? 0 : 1),
            'roi'               => (int)(($request['roi'] == true) ? 0 : 1),
            'cr'                => (int)(($request['cr'] == true) ? 0 : 1),
        ];

        $data = [
            'categoryId'        => (int)$request['categoryId'],
            'configPromoItem'   => $configPromoItem
        ];
        $callApi = new CallApi();
        return $callApi->putUsingToken($this->token, $api, $data);
    }
    
    public function exportExcelHistory(Request $request) {
        $api = '/config/promoitemhistory';
        $query = [
            'year'               => (int)$request['period'],
            'categoryShortDesc'  => $request['categoryShortDesc'],
        ];
        $callApi = new CallApi();
        $res = $callApi->getUsingToken($this->token, $api, $query);
        if (!json_decode($res)->error) {
            $data = json_decode($res)->data;
            $result = [];
            foreach ($data as $fields) {
                $arr = [];
                $arr[] = $fields->categoryLongDesc;
                $arr[] = (!($fields->budgetYear));
                $arr[] = (!($fields->promoPlanning));
                $arr[] = (!($fields->budgetSource));
                $arr[] = (!($fields->entity));
                $arr[] = (!($fields->distributor));
                $arr[] = (!($fields->subCategory));
                $arr[] = (!($fields->activity));
                $arr[] = (!($fields->subActivity));
                $arr[] = (!($fields->subActivityType));
                $arr[] = (!($fields->startPromo));
                $arr[] = (!($fields->endPromo));
                $arr[] = (!($fields->activityName));
                $arr[] = (!($fields->initiatorNotes));
                $arr[] = (!($fields->incrSales));
                $arr[] = (!($fields->investment));
                $arr[] = (!($fields->ROI));
                $arr[] = (!($fields->CR));
                $arr[] = (!($fields->channel));
                $arr[] = (!($fields->subChannel));
                $arr[] = (!($fields->account));
                $arr[] = (!($fields->subAccount));
                $arr[] = (!($fields->region));
                if($fields->categoryShortDesc == 'RC'){
                    $arr[] = (!($fields->brand));
                } else {
                    $arr[] = (!($fields->groupBrand));
                }
                $arr[] = (!($fields->SKU));
                $arr[] = (!($fields->mechanism));
                $arr[] = (!($fields->attachment));
                $arr[] = date('Y-m-d' , strtotime($fields->createOn));
                $arr[] = $fields->createBy;
                $arr[] = $fields->createdEmail;
                $arr[] = $fields->status;
                $result[] = $arr;
            }

            $filename = 'Editable Promo Items ('. $request->categoryShortDesc .') -';
            $title = 'A1:AE1'; //Report Title Bold and merge
            $header = 'A3:AE3'; //Header Column Bold and color
            $heading = [
                ['Editable Promo Items ('. $request->categoryShortDesc .')'],
                ['Year : ' . $request->period],
                ['Category', 'Budget Year', 'Promo Planning', 'Budget Source', 'Entity', 'Distributor', 'Sub Category', 'Activity', 'Sub Activity', 'Sub Activity Type', 'Start Promo', 'End Promo', 'Activity Description', 'Initiator Notes', 'Increment Sales', 'Investment', 'ROI', 'Cost Ratio',
                    'Channel', 'Sub Channel', 'Account', 'Sub Account', 'Region', 'Brand', 'SKU', 'Mechanism', 'Attachment', 'Modified On', 'Modified By', 'Modified Email', 'Status']
            ];

            $formatCell =  [];

            $export = new Export($result, $heading, $title, $header, $formatCell);
            $mc = microtime(true);
            return Excel::download($export, $filename . date('Y-m-d') . ' ' .$mc . '.xlsx');
        } else {
            return $res;
        }
    }
}
