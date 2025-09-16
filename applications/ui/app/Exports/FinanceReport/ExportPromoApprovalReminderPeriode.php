<?php
namespace App\Exports\FinanceReport;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\FromCollection;
use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\BeforeExport;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use Maatwebsite\Excel\Concerns\WithDrawings;

use PhpOffice\PhpSpreadsheet\Shared\Date;
use PhpOffice\PhpSpreadsheet\Style\NumberFormat;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

// use Maatwebsite\Excel\Concerns\WithStartRow;
// use Maatwebsite\Excel\Concerns\WithHeadingRow;
// use \Maatwebsite\Excel\Writer;
use Illuminate\Support\Facades\Log;
// class Export implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithDrawings
class ExportPromoApprovalReminderPeriode implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected $data;
    protected $heading;
    protected $headingcell1;
    protected $headingcell2;
    protected $headingcell3;
    protected $headingcell4;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading, $headingcell1, $headingcell2, $headingcell3, $headingcell4, $headingcell5,  $formatcell)
    {
        $this->data = $data;
        $this->heading = $heading;
        $this->headingcell1 = $headingcell1;
        $this->headingcell2 = $headingcell2;
        $this->formatcell = $formatcell;
        $this->headingcell3 = $headingcell3;
        $this->headingcell4 = $headingcell4;
        $this->headingcell5 = $headingcell5;
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
                $event->sheet->getStyle('A4:A6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('B4:B6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('C4:C6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('D4:D6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('E4:E6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('F6:I6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle('J4:M6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);

                $event->sheet->getStyle('N4:N6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],

                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);

                $event->sheet->getStyle('O4:O6')->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);

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
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell3)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell4)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->getStyle($this->headingcell5)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                        'vertical' => \PhpOffice\PhpSpreadsheet\Style\Alignment::VERTICAL_CENTER,
                    ],
                    'fill' => array(
                        'fillType' => Fill::FILL_SOLID,
                        'startColor' => array('argb' => 'FF0CA4FF')
                    ),
                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
                ]);
                $event->sheet->mergeCells('A4:A6');
                $event->sheet->mergeCells('B4:B6');
                $event->sheet->mergeCells('C4:C6');
                $event->sheet->mergeCells('D4:D6');
                $event->sheet->mergeCells('E4:E6');
                $event->sheet->mergeCells('J4:M4');
                $event->sheet->mergeCells('J5:K5');
                $event->sheet->mergeCells('L5:M5');
                $event->sheet->mergeCells('N4:N6');
                $event->sheet->mergeCells('O4:O6');
                $event->sheet->mergeCells($this->headingcell1);
                $event->sheet->mergeCells($this->headingcell3);
                $event->sheet->mergeCells($this->headingcell4);
                $event->sheet->mergeCells($this->headingcell5);
                // $event->sheet->getStyle("A1")->getNumberFormat()->setFormatCode('#,##0');
                $event->sheet->getStyle($this->headingcell1)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ]
                ]);
            },
        ];
    }
    public function drawings()
    {
        $drawing = new \PhpOffice\PhpSpreadsheet\Worksheet\Drawing();
        $drawing->setName('Logo');
        $drawing->setDescription('Logo');
        $drawing->setPath(public_path('/assets/media/logos/Logo.png'));
        $drawing->setHeight(20);

        return $drawing;
    }
}
