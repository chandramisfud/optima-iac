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

class ExportSummaryDSN implements FromArray, WithTitle, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected mixed $data;
    protected mixed $title;
    protected mixed $formatColumn;

    public function __construct($data, $title, $formatColumn = [])
    {
        $this->data = $data;
        $this->title = $title;
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
        return 'Total Summary DSN';
    }

    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $sheet = $event->sheet->getDelegate();

                $sheet->getColumnDimension('E')->setWidth(28);
                $sheet->getColumnDimension('F')->setWidth(12);
                $sheet->getColumnDimension('H')->setWidth(10);
                $sheet->getColumnDimension('I')->setWidth(14);

                $event->sheet->mergeCells('C3:I3')->setCellValue('C3', $this->title);
                $event->sheet->getStyle('C3:I3')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_CENTER,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                    'font' => [
                        'size'  => 11,
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

                $event->sheet->getStyle('E8:E8')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_LEFT,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                    'font' => [
                        'size'  => 11,
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

                $event->sheet->getStyle('F8:G8')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_CENTER,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                    'font' => [
                        'size'  => 11,
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

                $event->sheet->getStyle('E13:G13')->applyFromArray([
                    'font' => [
                        'size'  => 11,
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

                $event->sheet->mergeCells('G16:I16')->setCellValue('G16', 'Approved by');
                $event->sheet->getStyle('C16:I16')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_CENTER,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                    'font' => [
                        'bold'  => true
                    ],
                ]);

                $event->sheet->getStyle('C22:I22')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_CENTER,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                    'font' => [
                        'bold'  => true
                    ],
                ]);

                $event->sheet->getStyle('C23:I23')->applyFromArray([
                    'alignment' => [
                        'horizontal'    => Alignment::HORIZONTAL_CENTER,
                        'vertical'      => Alignment::VERTICAL_CENTER
                    ],
                ]);
            },
        ];
    }

}
