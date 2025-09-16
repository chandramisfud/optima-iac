<?php
namespace App\Exports\Budget;

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
class ExportXlsBudgetVolume implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected $data;
    protected $heading;
    protected $headingcell1;
    protected $headingcell2;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading = [], $headingcell1 = '', $headingcell2 = '', $formatcell = [])
    {
        $this->data = $data;
        $this->heading = $heading;
        $this->headingcell1 = $headingcell1;
        $this->headingcell2 = $headingcell2;
        $this->formatcell = $formatcell;
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
                $event->sheet->mergeCells('A9:G9')->setCellValue('A9','SS Input');
                $event->sheet->mergeCells('H9:T9')->setCellValue('H9','SS in Tons');
                $event->sheet->mergeCells('U9:AF9')->setCellValue('U9','Conversion Rate');
                $event->sheet->mergeCells('AG9:AS9')->setCellValue('AG9','SS Value');

                // Make Center and Color
                $event->sheet->getStyle($this->headingcell2)->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '9bc2e6'
                        ]
                    ],
                ]);

                $event->sheet->getStyle($this->headingcell1)->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true
                    ]
                ]);
                $sheet = $event->sheet->getDelegate();

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    $sheet->getStyle($col)->applyFromArray([
                        'font' => [
                            'name'      => 'Aptos Narrow',
                            'size'      => 10,
                        ],
                    ]);

                    $sheet->getColumnDimension($col)->setAutoSize(true);
                }

                $event->sheet->getStyle('A9:G9')->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '9bc2e6'
                        ]
                    ],
                ]);

                $event->sheet->getStyle('A10:A10')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('H9:T10')->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true,
                        'color' => [
                            'rgb' => 'ffffff'
                        ]
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '0e2841'
                        ]
                    ]
                ]);

                $event->sheet->getStyle('H9:T9')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('H10:T10')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('U9:AF10')->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true,
                        'color' => [
                            'rgb' => 'ffffff'
                        ]
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '36475e'
                        ]
                    ]
                ]);

                $event->sheet->getStyle('U9:AF9')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('U10:AF10')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('AG9:AS10')->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true,
                        'color' => [
                            'rgb' => 'ffffff'
                        ]
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '5b687c'
                        ]
                    ]
                ]);

                $event->sheet->getStyle('AG9:AS9')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('AG10:AS10')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
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
