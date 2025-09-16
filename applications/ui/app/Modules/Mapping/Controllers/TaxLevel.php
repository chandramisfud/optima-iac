<?php

namespace App\Modules\Mapping\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

class TaxLevel extends Controller
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
        $title = "Mapping Material Tax Level";
        return view('Mapping::tax-level.index', compact('title'));
    }

    public function getListPaginateFilter(Request $request)
    {
        $api = config('app.api'). '/mapping/taxlevel';
        try {
            $page = 0;
            if ($request->length > -1) {
                $page = ($request->start / $request->length);
            }
            $sort = $request['order'][0]['dir'];
            $column = $request['order'][0]['column'];;
            $search = ($request['search']['value'] ?? "");
            $length = $request['length'];
            $SortColumn = $request->columns[(int) $column]['data'];

            $query = [
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

    public function taxLevelFormPage (Request $request)
    {
        $title = "Mapping Material Tax Level";
        return view('Mapping::tax-level.form', compact('title'));
    }

    public function getListEntity(Request $request)
    {
        $api = config('app.api'). '/mapping/taxlevel/entity';
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

    public function save(Request $request)
    {
        $api = config('app.api') . '/mapping/taxlevel';
        $data = [
            'materialNumber'    => $request->materialNumber,
            'description'       => $request->description,
            'whT_Type'          => $request->whT_Type,
            'whT_Code'          => $request->whT_Code,
            'purpose'           => $request->purpose,
            'entity'            => $request->entity,
            'entityId'          => $request->entityId,
            'ppnPct'            => $request->ppnPct,
            'pphPct'            => $request->pphPct,
        ];
        Log::info('post API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response =  Http::timeout(180)->withToken($this->token)->post($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Save success",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('post API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "Save Failed"
            );
        }
    }

    public function delete(Request $request)
    {
        $api = config('app.api') . '/mapping/taxlevel';
        $data = [
            'id'           => $request->id
        ];
        Log::info('delete API ' . $api);
        Log::info('payload ' . json_encode($data));
        try {
            $response = Http::timeout(180)->withToken($this->token)->delete($api, $data);
            if ($response->status() === 200) {
                return json_encode([
                    'error'     => false,
                    'data'      => json_decode($response)->values,
                    'message'   => "Mapping Removed",
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::info('delete API ' . $api);
                Log::warning($message);
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => "Remove Failed"
                );
            }
        } catch (\Exception $e) {
            Log::error('delete API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => "error : Remove Failed"
            );
        }
    }

    public function exportXls(Request $request) {
        $api = config('app.api'). '/mapping/taxlevel/download';
        Log::info('get API ' . $api);
        try {
            $response = Http::timeout(180)->withToken($this->token)->get($api);
            if ($response->status() == 200) {
                $resVal = json_decode($response)->values;
                $result=[];
                foreach ($resVal as $fields) {
                    $arr = [];
                    $arr[] = $fields->materialNumber;
                    $arr[] = $fields->description;
                    $arr[] = $fields->whT_Type;
                    $arr[] = $fields->whT_Code;
                    $arr[] = $fields->purpose;
                    $arr[] = $fields->entity;
                    $arr[] = $fields->ppnPct;
                    $arr[] = $fields->pphPct;
                    $arr[] = (($fields->createOn) ? date('Y-m-d H:i:s' , strtotime($fields->createOn)) : null);
                    $arr[] = $fields->createBy;
                    $arr[] = (($fields->deleteOn) ? date('Y-m-d H:i:s' , strtotime($fields->deleteOn)) : null);
                    $arr[] = $fields->deleteBy;

                    $result[] = $arr;
                }

                $filename = 'Mapping Material Tax Level -';
                $title = 'A1:L1'; //Report Title Bold and merge
                $header = 'A3:L3'; //Header Column Bold and color
                $heading = [
                    ['Mapping Material Tax Level'],
                    ['Date Retrieved : ' . date('Y-m-d')],
                    ['Material Number', 'Description', 'WHT Type', 'WHT Code', 'Purpose', 'Entity', 'PPN (%)', 'PPH (%)', 'Created On', 'Created By', 'Removed On', 'Removed By']
                ];

                $formatCell =  [
                    'G' => NumberFormat::FORMAT_NUMBER_00,
                    'H' => NumberFormat::FORMAT_NUMBER_00,
                ];

                $export = new Export($result, $heading, $title, $header, $formatCell);
                $mc = microtime(true);
                return Excel::download($export, $filename . date('Y-m-d') . ' ' . $mc .  '.xlsx');
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
