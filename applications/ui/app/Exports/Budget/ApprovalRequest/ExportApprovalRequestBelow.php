<?php
namespace App\Exports\Budget\ApprovalRequest;

use Illuminate\Support\Facades\Log;
use JetBrains\PhpStorm\ArrayShape;
use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Concerns\WithTitle;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Alignment;
use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Fill;

use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

class ExportApprovalRequestBelow implements FromArray, WithHeadings, WithTitle, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected mixed $data;
    protected mixed $header;
    protected mixed $formatCell;
    protected mixed $arrSubHeaderMerged;
    protected mixed $arrHeaderFill;
    protected mixed $arrHeaderBorder;
    protected mixed $arrBodyBorder;
    protected mixed $arrFooterBorder;
    protected mixed $centerTextBelow;

    public function __construct($data, $header = [], $formatCell = [], $arrSubHeaderMerged = [], $arrHeaderFill = [], $arrHeaderBorder = [], $arrBodyBorder = [], $arrFooterBorder = [], $centerTextBelow = '')
    {
        $this->data = $data;
        $this->header = $header;
        $this->formatCell = $formatCell;
        $this->arrSubHeaderMerged = $arrSubHeaderMerged;
        $this->arrHeaderFill = $arrHeaderFill;
        $this->arrHeaderBorder = $arrHeaderBorder;
        $this->arrBodyBorder = $arrBodyBorder;
        $this->arrFooterBorder = $arrFooterBorder;
        $this->centerTextBelow = $centerTextBelow;
    }

    public function columnFormats(): array
    {
        return $this->formatCell;
    }
    public function array(): array
    {
        return $this->data;
    }
    public function headings(): array{
        return $this->header;
    }
    public function title(): string {
        return 'Approval Sheet Below 5bio';
    }
    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $event->sheet->getStyle('D1:D2')->applyFromArray([
                    'font' => [
                        'size'  => 20,
                        'bold'  => true
                    ],
                ]);

                $sheet = $event->sheet->getDelegate();

                $arraySubHeaderMergedData = $this->arrSubHeaderMerged;
                for ($i=0; $i<count($arraySubHeaderMergedData); $i++) {
                    $event->sheet->mergeCells($arraySubHeaderMergedData[$i]['range'])->setCellValue($arraySubHeaderMergedData[$i]['coordinate'],$arraySubHeaderMergedData[$i]['value']);
                    $event->sheet->getStyle($arraySubHeaderMergedData[$i]['range'])->applyFromArray([
                        'alignment' => [
                            'horizontal'    => Alignment::HORIZONTAL_CENTER,
                            'vertical'      => Alignment::VERTICAL_CENTER
                        ],
                    ]);
                }

                $arrHeaderFillData = $this->arrHeaderFill;
                for ($i=0; $i<count($arrHeaderFillData); $i++) {
                    $event->sheet->getStyle($arrHeaderFillData[$i])->applyFromArray([
                        'fill' => [
                            'fillType' => Fill::FILL_SOLID,
                            'color' => [
                                'rgb' => '366092'
                            ]
                        ],
                        'font' => [
                            'color' => [
                                'rgb' => 'ffffff'
                            ],
                            'bold'  => true
                        ]
                    ]);
                }

                $arrHeaderBorderData = $this->arrHeaderBorder;
                for ($i=0; $i<count($arrHeaderBorderData); $i++) {
                    $event->sheet->getStyle($arrHeaderBorderData[$i])->applyFromArray([
                        'borders' => [
                            'bottom' => [
                                'borderStyle' => Border::BORDER_THIN,
                                'color' => [
                                    'rgb' => 'ffffff'
                                    ]
                            ],
                        ],
                    ]);
                }

                $arrBodyBorderData = $this->arrBodyBorder;
                for ($i=0; $i<count($arrBodyBorderData); $i++) {
                    $event->sheet->getStyle($arrBodyBorderData[$i])->applyFromArray([
                        'borders' => [
                            'top' => [
                                'borderStyle' => Border::BORDER_THIN,
                                'color' => [
                                    'rgb' => 'dce6f1'
                                ]
                            ],
                        ],
                    ]);
                }

                $event->sheet->getStyle($this->centerTextBelow)->applyFromArray([
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $arrFooterBorderData = $this->arrFooterBorder;
                for ($i=0; $i<count($arrFooterBorderData); $i++) {
                    $event->sheet->getStyle($arrFooterBorderData[$i])->applyFromArray([
                        'borders' => [
                            'top' => [
                                'borderStyle' => Border::BORDER_DOUBLE,
                                'color' => [
                                    'rgb' => '000000'
                                ]
                            ],
                        ],
                    ]);
                }

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    if ($col === 'D') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                        $sheet->getColumnDimension($col)->setWidth(32);
                    } else {
                        $sheet->getColumnDimension($col)->setAutoSize(true);
                    }
                }
            },
        ];
    }
}
