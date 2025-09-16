<?php
namespace App\Exports\Tools;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\FromCollection;
use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\BeforeExport;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Border;
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
class ExportTemplateBudgetAdjustment implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithColumnFormatting,  WithStrictNullComparison
{
    protected $data;
    protected $heading;
    protected $headingcell1;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading = [], $headingcell1 = '', $formatcell = [])
    {
        $this->data = $data;
        $this->heading = $heading;
        $this->headingcell1 = $headingcell1;
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
                $event->sheet->getStyle($this->headingcell1)->applyFromArray([
                    'font' => [
                        'bold' => false
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => '0CA4FF'
                        ]
                    ]
                ]);
                $sheet = $event->sheet->getDelegate();

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    $sheet->getColumnDimension($col)->setAutoSize(true);
                }
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
