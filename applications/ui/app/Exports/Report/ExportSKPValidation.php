<?php
namespace App\Exports\Report;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

use PhpOffice\PhpSpreadsheet\Style\Border;
use PhpOffice\PhpSpreadsheet\Style\Alignment;

use Illuminate\Support\Facades\Log;

class ExportSKPValidation implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected $data;
    protected $heading;
    protected $headingcell1;
    protected $headingcell2;
    protected $headingcell3;
    protected $headingcell4;
    protected $headingcell5;
    protected $headingcell6;
    protected $headingcell7;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading, $headingcell1, $headingcell2, $headingcell3, $headingcell4, $headingcell5, $headingcell6, $headingcell7, $formatcell)
    {
        $this->data = $data;
        $this->heading = $heading;
        $this->headingcell1 = $headingcell1;
        $this->headingcell2 = $headingcell2;
        $this->formatcell = $formatcell;
        $this->headingcell3 = $headingcell3;
        $this->headingcell4 = $headingcell4;
        $this->headingcell5 = $headingcell5;
        $this->headingcell6 = $headingcell6;
        $this->headingcell7 = $headingcell7;
    }

    public function columnFormats(): array
    {
        return $this->formatcell;
    }
    public function array(): array
    {
        return $this->data;
    }
    public function headings(): array{
        return $this->heading;
    }

    public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {
                $event->sheet->getStyle($this->headingcell2)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell3)->applyFromArray([
                    'font' => [
                        'bold' => true,
                        'size' => 18
                    ],
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                        'vertical' => Alignment::VERTICAL_CENTER,
                    ],
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell4)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                        'vertical' => Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FFF9EA1F')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell5)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF006BAB')
                    ),
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                        'vertical' => Alignment::VERTICAL_CENTER,
                    ],
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),

                ]);
                $event->sheet->getStyle($this->headingcell6)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                        'vertical' => Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),

                ]);
                $event->sheet->getStyle($this->headingcell7)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FFF9EA1F')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),

                ]);

                $event->sheet->mergeCells($this->headingcell1);
                $event->sheet->mergeCells($this->headingcell3);
                $event->sheet->mergeCells($this->headingcell4);
                $event->sheet->mergeCells($this->headingcell5);
                $event->sheet->mergeCells($this->headingcell6);
                $event->sheet->getStyle($this->headingcell1)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ]
                ]);
            },
        ];
    }
}
