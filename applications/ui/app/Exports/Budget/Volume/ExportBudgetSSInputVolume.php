<?php
namespace App\Exports\Budget\Volume;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

// class Export implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithDrawings
class ExportBudgetSSInputVolume implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected $data;
    protected $heading;
    protected $headingcell1;
    protected $headingcell2;
    protected $cellNotInput;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading = [], $headingcell1 = '', $headingcell2 = '', $formatcell = [], $cellNotInput = '')
    {
        $this->data = $data;
        $this->heading = $heading;
        $this->headingcell1 = $headingcell1;
        $this->headingcell2 = $headingcell2;
        $this->formatcell = $formatcell;
        $this->cellNotInput = $cellNotInput;
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
                $event->sheet->mergeCells('A1:G1')->setCellValue('A1','SS Input Upload Template');
                $event->sheet->mergeCells('H1:T1')->setCellValue('H1','SS in Tons');
                $event->sheet->mergeCells('U1:AF1')->setCellValue('U1','Conversion Rate');
                $event->sheet->mergeCells('AG1:AR1')->setCellValue('AG1','SS Value');
                $event->sheet->mergeCells('AS1:AS1')->setCellValue('AS1','SS Value');

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

                $event->sheet->getStyle('A1:G1')->applyFromArray([
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

                $event->sheet->getStyle('A2:A2')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('H1:T2')->applyFromArray([
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

                $event->sheet->getStyle('H1:T1')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('H2:T2')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('U1:AF2')->applyFromArray([
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

                $event->sheet->getStyle('U1:AF1')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('U2:AF2')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('AG1:AR2')->applyFromArray([
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

                $event->sheet->getStyle('AG1:AR1')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $event->sheet->getStyle('AG2:AR2')->applyFromArray([
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                ]);

                $event->sheet->getStyle('AS1:AS2')->applyFromArray([
                    'font' => [
                        'name'      => 'Aptos Narrow',
                        'size'      => 10,
                        'bold'      => true,
                        'color' => [
                            'rgb' => 'ffffff'
                        ]
                    ],
                    'alignment' => [
                        'horizontal' => \PhpOffice\PhpSpreadsheet\Style\Alignment::HORIZONTAL_RIGHT,
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '5b687c'
                        ]
                    ]
                ]);

                $event->sheet->getStyle($this->cellNotInput)->applyFromArray([
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => 'CBD1D7'
                        ]
                    ],
                ]);

                //Hide Column
                $sheet->getColumnDimension('U')->setVisible(false);
                $sheet->getColumnDimension('V')->setVisible(false);
                $sheet->getColumnDimension('W')->setVisible(false);
                $sheet->getColumnDimension('X')->setVisible(false);
                $sheet->getColumnDimension('Y')->setVisible(false);
                $sheet->getColumnDimension('Z')->setVisible(false);
                $sheet->getColumnDimension('AA')->setVisible(false);
                $sheet->getColumnDimension('AB')->setVisible(false);
                $sheet->getColumnDimension('AC')->setVisible(false);
                $sheet->getColumnDimension('AD')->setVisible(false);
                $sheet->getColumnDimension('AE')->setVisible(false);
                $sheet->getColumnDimension('AF')->setVisible(false);
                $sheet->getColumnDimension('AG')->setVisible(false);
                $sheet->getColumnDimension('AH')->setVisible(false);
                $sheet->getColumnDimension('AI')->setVisible(false);
                $sheet->getColumnDimension('AJ')->setVisible(false);
                $sheet->getColumnDimension('AK')->setVisible(false);
                $sheet->getColumnDimension('AL')->setVisible(false);
                $sheet->getColumnDimension('AM')->setVisible(false);
                $sheet->getColumnDimension('AN')->setVisible(false);
                $sheet->getColumnDimension('AO')->setVisible(false);
                $sheet->getColumnDimension('AP')->setVisible(false);
                $sheet->getColumnDimension('AQ')->setVisible(false);
                $sheet->getColumnDimension('AR')->setVisible(false);
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
