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

class ExportDCSummary implements FromArray, WithEvents, ShouldAutoSize, WithColumnFormatting, WithStrictNullComparison, WithTitle
{
    protected mixed $data;
    protected mixed $arrMergeHeader;
    protected mixed $arrRowHeader;
    protected mixed $arrRowSubHeader;
    protected mixed $arrRowTotal;
    protected mixed $formatColumn;

    public function __construct($data, $arrMergeHeader = [], $arrRowHeader = [], $arrRowSubHeader = [], $arrRowTotal = [], $formatColumn = [])
    {
        $this->data = $data;
        $this->arrMergeHeader = $arrMergeHeader;
        $this->arrRowHeader = $arrRowHeader;
        $this->arrRowSubHeader = $arrRowSubHeader;
        $this->arrRowTotal = $arrRowTotal;
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
        return 'DC Summary';
    }

    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $sheet = $event->sheet->getDelegate();

                $sheet->getColumnDimension('D')->setWidth(0.5);
                $sheet->getColumnDimension('H')->setWidth(0.5);
                $sheet->getColumnDimension('N')->setWidth(0.5);
                $sheet->getColumnDimension('T')->setWidth(0.5);

                $sheet->freezePane('E4');

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
                    $sheet->getRowDimension($arrRowSubHeaderData[$i])->setRowHeight(27);
                    $event->sheet->getStyle('B' . $arrRowSubHeaderData[$i] . ':C' . $arrRowSubHeaderData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('E' . $arrRowSubHeaderData[$i] . ':G' . $arrRowSubHeaderData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('I' . $arrRowSubHeaderData[$i] . ':M' . $arrRowSubHeaderData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('O' . $arrRowSubHeaderData[$i] . ':S' . $arrRowSubHeaderData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('U' . $arrRowSubHeaderData[$i] . ':Y' . $arrRowSubHeaderData[$i])->applyFromArray([
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

                //<editor-fold desc="Total">
                $arrRowTotalData = $this->arrRowTotal;
                for ($i=0; $i<count($arrRowTotalData); $i++) {
                    $event->sheet->getStyle('B' . $arrRowTotalData[$i] . ':C' . $arrRowTotalData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('E' . $arrRowTotalData[$i] . ':G' . $arrRowTotalData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('I' . $arrRowTotalData[$i] . ':M' . $arrRowTotalData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('O' . $arrRowTotalData[$i] . ':S' . $arrRowTotalData[$i])->applyFromArray([
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

                    $event->sheet->getStyle('U' . $arrRowTotalData[$i] . ':Y' . $arrRowTotalData[$i])->applyFromArray([
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

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    if ($col === 'D' || $col === 'H' || $col === 'N'  || $col === 'T') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(0.5);
                    } else if ($col === 'A') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(8);
                    } else if ($col === 'B') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(12);
                    } else if ($col === 'C') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(27);
                    } else {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(20);
                    }
                }
            },
        ];
    }
}
