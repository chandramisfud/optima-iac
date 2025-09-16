<?php

namespace App\Modules\Configuration\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Request;
use App\Helpers\CallApi;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;

class PromoCalculator extends Controller
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
        $title = "Promo Calculator Configuration";
        return view('Configuration::promo-calculator.index', compact('title'));
    }

    public function formPage(): Factory|View|Application
    {
        $title = "Promo Calculator Configuration";
        return view('Configuration::promo-calculator.form', compact('title'));
    }

    public function formUploadPage(): Factory|View|Application
    {
        $title = "Promo Calculator Configuration";
        return view('Configuration::promo-calculator.form-upload', compact('title'));
    }

    public function getList(Request $request): bool|string
    {
        $api = '/config/promocalculator';
        $query = [
            'mainActivityId' => (int) ($request['mainActivityId'] == null ? 0 : $request['mainActivityId']),
            'channelId'     => (int) ($request['channelId'] == null ? 0 : $request['channelId']),
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function getCoverage(): bool|string
    {
        $api = '/config/promocalculator/filterandcoverage';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getChannel(): bool|string
    {
        $api = '/config/promocalculator/channel';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getFilter(): bool|string
    {
        $api = '/config/promocalculator/filter';
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api);
    }

    public function getData(Request $request): bool|string
    {
        $api = '/config/promocalculator/id';
        $query = [
            'mainActivityId'    => $request['mainActivityId'],
            'channelId'         => $request['channelId']
        ];
        $callApi = new CallApi();
        return $callApi->getUsingToken($this->token, $api, $query);
    }

    public function save(Request $request): bool|string
    {
        $api = '/config/promocalculator';
        $body = [
            'configCalculator'          => json_decode($request['calculatorConfiguration']),
            'subActivity'               => json_decode($request['subActivity']),
        ];
        $callApi = new CallApi();
        return $callApi->postUsingToken($this->token, $api, $body);
    }

    public function update(Request $request): bool|string
    {
        $api = '/config/promocalculator';
        $body = [
            'configCalculator'          => json_decode($request['calculatorConfiguration']),
            'subActivity'               => json_decode($request['subActivity']),
        ];
        $callApi = new CallApi();
        return $callApi->putUsingToken($this->token, $api, $body);
    }

    public function exportXls(Request $request): \Symfony\Component\HttpFoundation\BinaryFileResponse|array
    {
        $api = config('app.api'). '/config/promocalculator';
        $query = [
            'mainActivityId' => (int) ($request['mainActivityId'] == null ? 0 : $request['mainActivityId']),
            'channelId'     => (int) ($request['channelId'] == null ? 0 : $request['channelId']),
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

                    $arr[] = $fields->mainActivity;
                    $arr[] = $fields->channelLongDesc;
                    $arr[] = $fields->baseline;
                    $arr[] = $fields->uplift;
                    $arr[] = $fields->totalSales;
                    $arr[] = $fields->salesContribution;
                    $arr[] = $fields->storesCoverage;
                    $arr[] = $fields->redemptionRate;
                    $arr[] = $fields->cr;
                    $arr[] = $fields->cost;
                    $arr[] = $fields->baselineRecon;
                    $arr[] = $fields->upliftRecon;
                    $arr[] = $fields->totalSalesRecon;
                    $arr[] = $fields->salesContributionRecon;
                    $arr[] = $fields->storesCoverageRecon;
                    $arr[] = $fields->redemptionRateRecon;
                    $arr[] = $fields->crRecon;
                    $arr[] = $fields->costRecon;

                    $result[] = $arr;
                }
                $title = 'A1:S1'; //Report Title Bold and merge
                $header = 'A4:S4'; //Header Column Bold and color
                $heading = [
                    ['Promo Calculator Configuration'],
                    ['Main Activity : ' . $request['mainActivity'] ],
                    ['Channel : ' . $request['channel'] ],
                    ['Main Activity', 'Channel', 'Baseline', 'Uplift', 'Total Sales', 'Sales Contribution', 'Stores Coverage', 'Redemption Rate', 'CR', 'Cost', 'Baseline Recon', 'Uplift Recon', 'Total Sales Recon', 'Sales Contribution Recon', 'Stores Coverage Recon', 'Redemption Rate Recon', 'CR Recon', 'Cost Recon']
                ];
                $formatCell =  [];
                $filename = 'PromoCalculatorConfiguration-' . $request['mainActivity'] . '-' . $request['channel'] . '-';
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
}
