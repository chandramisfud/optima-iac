<?php
namespace App\Exports\FinanceReport\SummaryBudgeting;

use JetBrains\PhpStorm\ArrayShape;
use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;
use Maatwebsite\Excel\Concerns\WithTitle;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Alignment;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Fill;

class ExportTotalSummary implements FromArray, WithEvents, ShouldAutoSize, WithColumnFormatting, WithStrictNullComparison, WithTitle
{
    protected mixed $data;
    protected mixed $arrMergeHeader;
    protected mixed $arrRowHeader;
    protected mixed $arrRowSubHeader;
    protected mixed $arrRowSubTotal;
    protected mixed $arrRowGrandTotal;
    protected mixed $formatColumn;

    public function __construct($data, $arrMergeHeader = [], $arrRowHeader = [], $arrRowSubHeader = [], $arrRowSubTotal = [], $arrRowGrandTotal = [], $formatColumn = [])
    {
        $this->data = $data;
        $this->arrMergeHeader = $arrMergeHeader;
        $this->arrRowHeader = $arrRowHeader;
        $this->arrRowSubHeader = $arrRowSubHeader;
        $this->arrRowSubTotal = $arrRowSubTotal;
        $this->arrRowGrandTotal = $arrRowGrandTotal;
        $this->formatColumn = $formatColumn;
    }

    public function array(): array
    {
        return $this->data;
    }

    public function columnFormats(): array
    {
        return $this->formatColumn;
    }

    public function title(): string {
        return 'Total Summary';
    }

    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $sheet = $event->sheet->getDelegate();

                $sheet->freezePane('H4');

                //<editor-fold desc="Merge Header">
                $arrMergeHeaderData = $this->arrMergeHeader;
                for ($i=0; $i<count($arrMergeHeaderData); $i++) {
                    $event->sheet->mergeCells($arrMergeHeaderData[$i]['range'])->setCellValue($arrMergeHeaderData[$i]['coordinate'],$arrMergeHeaderData[$i]['value']);
                    $event->sheet->getStyle($arrMergeHeaderData[$i]['range'])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                        'borders' => [
                            'bottom' => [
                                'borderStyle' => Border::BORDER_THIN,
                                'color' => [
                                    'rgb' => '000000'
                                ]
                            ],
                        ],
                    ]);
                }
                //</editor-fold>

                //<editor-fold desc="Row Header">
                $arrRowHeaderData = $this->arrRowHeader;
                for ($i=0; $i<count($arrRowHeaderData); $i++) {
                    $sheet->getRowDimension($arrRowHeaderData[$i])->setRowHeight(39.5);
                }
                //</editor-fold>

                //<editor-fold desc="Sub Header">
                $arrRowSubHeaderData = $this->arrRowSubHeader;
                for ($i=0; $i<count($arrRowSubHeaderData); $i++) {
                    $sheet->getRowDimension($arrRowSubHeaderData[$i])->setRowHeight(24);
                    $event->sheet->getStyle('B' . $arrRowSubHeaderData[$i] . ':F' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('H' . $arrRowSubHeaderData[$i] . ':K' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('M' . $arrRowSubHeaderData[$i] . ':V' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('X' . $arrRowSubHeaderData[$i] . ':AF' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AH' . $arrRowSubHeaderData[$i] . ':AI' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AK' . $arrRowSubHeaderData[$i] . ':AL' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AN' . $arrRowSubHeaderData[$i] . ':AO' . $arrRowSubHeaderData[$i])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                        'font' => [
                            'size'  => 9,
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '002060'
                            ]
                        ],
                    ]);
                }
                //</editor-fold>

                //<editor-fold desc="Sub Total">
                $arrRowSubTotalData = $this->arrRowSubTotal;
                for ($i=0; $i<count($arrRowSubTotalData); $i++) {
                    $event->sheet->getStyle('B' . $arrRowSubTotalData[$i] . ':F' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('H' . $arrRowSubTotalData[$i] . ':K' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('M' . $arrRowSubTotalData[$i] . ':V' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('X' . $arrRowSubTotalData[$i] . ':AF' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AH' . $arrRowSubTotalData[$i] . ':AI' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AK' . $arrRowSubTotalData[$i] . ':AL' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AN' . $arrRowSubTotalData[$i] . ':AO' . $arrRowSubTotalData[$i])->applyFromArray([
                        'font' => [
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '8ea9db'
                            ]
                        ],
                    ]);
                }
                //</editor-fold>

                //<editor-fold desc="Grand Total">
                $arrRowGrandTotalData = $this->arrRowGrandTotal;
                for ($i=0; $i<count($arrRowGrandTotalData); $i++) {
                    $event->sheet->getStyle('B' . $arrRowGrandTotalData[$i] . ':F' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('H' . $arrRowGrandTotalData[$i] . ':K' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('M' . $arrRowGrandTotalData[$i] . ':V' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('X' . $arrRowGrandTotalData[$i] . ':AF' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AH' . $arrRowGrandTotalData[$i] . ':AI' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AK' . $arrRowGrandTotalData[$i] . ':AL' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);

                    $event->sheet->getStyle('AN' . $arrRowGrandTotalData[$i] . ':AO' . $arrRowGrandTotalData[$i])->applyFromArray([
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ],
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '305496'
                            ]
                        ],
                    ]);
                }
                //</editor-fold>

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    if ($col === 'G' || $col === 'L' || $col === 'W'  || $col === 'AG'  || $col === 'AJ' || $col === 'AM') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(0.5);
                    } else if ($col === 'AN' || $col === 'AO') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(14);
                    } else {
                        $sheet->getColumnDimension($col)->setAutoSize(true);
                    }
                }
            },
        ];
    }

}
