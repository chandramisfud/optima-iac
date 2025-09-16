<?php

namespace App\Http\Controllers;

use App\Exports\Export;
use App\Http\Controllers\Controller;
use App\Http\Requests;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Storage;
use Maatwebsite\Excel\Facades\Excel;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Wording;

class AppDrive extends Controller
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
        $title = "Application Drive";
        return view('drive-apps.index', compact('title'));
    }

    public function getListDrive(Request $request)
    {
        try {
            $pathSource = 'assets/media/' . $request->type . '/';
            $listDrive = [];
            $arrId = Storage::disk('optima')->directories($pathSource);

            for ($i=0; $i<count($arrId); $i++) {
                $id = str_replace($pathSource, '', $arrId[$i]);

                $arrRow = Storage::disk('optima')->directories($pathSource . $id);
                if (count($arrRow) > 0) {
                    for ($j=0; $j<count($arrRow); $j++) {
                        $row = str_replace($pathSource . $id . '/', '', $arrRow[$j]);

                        $arrFile = Storage::disk('optima')->files($pathSource . $id . '/' . $row);

                        for ($k=0; $k<count($arrFile); $k++) {
                            $fileName = str_replace($pathSource . $id . '/' . $row . '/', '', $arrFile[$k]);
                            $dateModified = Storage::disk('optima')->lastModified($arrFile[$k]);
                            $size = Storage::disk('optima')->size($arrFile[$k]);
                            $data = json_encode([
                                'id'            => $id,
                                'row'           => $row,
                                'fileName'      => $fileName,
                                'dateModified'  => date('Y-m-d H:i:s', $dateModified),
                                'size'          => $size
                            ]);
                            array_push($listDrive, json_decode($data));
                        }
                    }
                } else {
                    $data = json_encode([
                        'id'            => $id,
                        'row'           => '-',
                        'fileName'      => '-',
                        'dateModified'  => '-',
                        'size'          => '-'
                    ]);
                    array_push($listDrive, json_decode($data));
                }
            }

            return array(
                'error'     => false,
                'data'      => $listDrive
            );
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function exportExcel(Request $request)
    {
        try {
            $pathSource = 'assets/media/' . $request->type . '/';
            $listDrive = [];
            $arrId = Storage::disk('optima')->directories($pathSource);

            for ($i=0; $i<count($arrId); $i++) {
                $id = str_replace($pathSource, '', $arrId[$i]);

                $arrRow = Storage::disk('optima')->directories($pathSource . $id);
                if (count($arrRow) > 0) {
                    for ($j=0; $j<count($arrRow); $j++) {
                        $row = str_replace($pathSource . $id . '/', '', $arrRow[$j]);

                        $arrFile = Storage::disk('optima')->files($pathSource . $id . '/' . $row);

                        for ($k=0; $k<count($arrFile); $k++) {
                            $fileName = str_replace($pathSource . $id . '/' . $row . '/', '', $arrFile[$k]);
                            $dateModified = Storage::disk('optima')->lastModified($arrFile[$k]);
                            $size = Storage::disk('optima')->size($arrFile[$k]);
                            $data = json_encode([
                                'id'            => $id,
                                'row'           => $row,
                                'fileName'      => $fileName,
                                'dateModified'  => date('Y-m-d H:i:s', $dateModified),
                                'size'          => $size
                            ]);
                            array_push($listDrive, json_decode($data));
                        }
                    }
                } else {
                    $data = json_encode([
                        'id'            => $id,
                        'row'           => '-',
                        'fileName'      => '-',
                        'dateModified'  => '-',
                        'size'          => '-'
                    ]);
                    array_push($listDrive, json_decode($data));
                }
            }

            $result = [];
            foreach ($listDrive as $fields) {
                $arr = [];
                $arr[] = $fields->id;
                $arr[] = $fields->row;
                $arr[] = $fields->fileName;
                $arr[] = $fields->dateModified;
                $arr[] = ((int) $fields->size / 1000) . ' KB';

                $result[] = $arr;
            }

            $filename = 'Attachment List -';
            $title = 'A1:A5'; //Report Title Bold and merge
            $header = 'A4:E5'; //Header Column Bold and color
            $heading = [
                ['Attachment List'],
                ['Type : ' . $request->type],
                ['Date Retrieved : ' . date('Y-m-d')],
                ['ID', 'Row', 'File Name', 'Date Modified', 'Size']
            ];

            $formatCell =  [];

            $export = new Export($result, $heading, $title, $header, $formatCell);
            $mc = microtime(true);
            return Excel::download($export, $filename . date('Y-m-d H:i:s') . ' ' .$mc . '.xlsx');
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

}
