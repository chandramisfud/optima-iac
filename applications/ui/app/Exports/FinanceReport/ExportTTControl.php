<?php
namespace App\Exports\FinanceReport;

use JetBrains\PhpStorm\ArrayShape;
use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Alignment;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Fill;

use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

class ExportTTControl implements FromArray, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected mixed $data;
    protected mixed $formatCell;
    protected mixed $arrHeader;
    protected mixed $arrSubTotalRow;
    protected mixed $arrSubTotalRowBrand;
    protected mixed $arrSubTotalRowChannel;
    protected mixed $arrSubTotalRowSubAccount;
    protected mixed $arrSubTotalRowActivity;

    public function __construct($data,  $formatCell = [], $arrHeader = [], $arrSubTotalRow = [], $arrSubTotalRowBrand = [], $arrSubTotalRowChannel = [], $arrSubTotalRowSubAccount = [], $arrSubTotalRowActivity = [])
    {
        $this->data = $data;
        $this->formatCell = $formatCell;
        $this->arrHeader = $arrHeader;
        $this->arrSubTotalRow = $arrSubTotalRow;
        $this->arrSubTotalRowBrand = $arrSubTotalRowBrand;
        $this->arrSubTotalRowChannel = $arrSubTotalRowChannel;
        $this->arrSubTotalRowSubAccount = $arrSubTotalRowSubAccount;
        $this->arrSubTotalRowActivity = $arrSubTotalRowActivity;
    }

    public function columnFormats(): array
    {
        return $this->formatCell;
    }

    public function array(): array
    {
        return $this->data;
    }

    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $sheet = $event->sheet->getDelegate();

                $event->sheet->getStyle('A1')->applyFromArray([
                    'font' => [
                        'size'      => 14,
                        'bold'      => true
                    ],
                ]);

                $event->sheet->getStyle('A2')->applyFromArray([
                    'font' => [
                        'bold'      => true
                    ],
                ]);

                // Make Center and Color
//                $event->sheet->getStyle($this->headerStyle)->applyFromArray([
//                    'font' => [
//                        'bold'      => true,
//                        'color'     =>  [
//                            'rgb' => 'ffffff'
//                        ],
//                    ],
//                    'fill' => [
//                        'fillType' => Fill::FILL_SOLID,
//                        'color' => [
//                            'rgb' => '002060'
//                        ]
//                    ],
//                    'alignment' => [
//                        'horizontal' => Alignment::HORIZONTAL_CENTER,
//                    ],
//                ]);

                $arrHeader = $this->arrHeader;
                for ($i=0; $i<count($arrHeader); $i++) {
                    $event->sheet->getStyle($arrHeader[$i])->applyFromArray([
                        'font' => [
                            'bold'      => true,
                            'color'     =>  [
                                'rgb' => 'ffffff'
                            ],
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                        'alignment' => [
                            'horizontal' => Alignment::HORIZONTAL_CENTER,
                        ],
                    ]);
                }

                $arrSubTotalRow = $this->arrSubTotalRow;
                for ($i=0; $i<count($arrSubTotalRow); $i++) {
                    $event->sheet->getStyle($arrSubTotalRow[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => 'b4c6e7'
                            ]
                        ],
                        'font' => [
                            'bold'  => true
                        ]
                    ]);
                }

                //Brand
                $arrSubTotalRowBrand = $this->arrSubTotalRowBrand;
                for ($i=0; $i<count($arrSubTotalRowBrand); $i++) {
                    $event->sheet->getStyle($arrSubTotalRowBrand[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '469FF7'
                            ]
                        ],
                        'font' => [
                            'bold'  => true
                        ]
                    ]);
                }

                //Channel
                $arrSubTotalRowChannel = $this->arrSubTotalRowChannel;
                for ($i=0; $i<count($arrSubTotalRowChannel); $i++) {
                    $event->sheet->getStyle($arrSubTotalRowChannel[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '59A6F3'
                            ]
                        ],
                        'font' => [
                            'bold'  => true
                        ]
                    ]);
                }

                //Sub Account
                $arrSubTotalRowSubAccount = $this->arrSubTotalRowSubAccount;
                for ($i=0; $i<count($arrSubTotalRowSubAccount); $i++) {
                    $event->sheet->getStyle($arrSubTotalRowSubAccount[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '7DBAF5'
                            ]
                        ],
                        'font' => [
                            'bold'  => true
                        ]
                    ]);
                }

                //Activity
                $arrSubTotalRowActivity = $this->arrSubTotalRowActivity;
                for ($i=0; $i<count($arrSubTotalRowActivity); $i++) {
                    $event->sheet->getStyle($arrSubTotalRowActivity[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '9AC8F5'
                            ]
                        ],
                        'font' => [
                            'bold'  => true
                        ]
                    ]);
                }

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    $sheet->getColumnDimension($col)->setAutoSize(true);
                }
            },
        ];
    }
}
