<?php
namespace App\Exports;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Concerns\WithTitle;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;
use Illuminate\Support\Facades\Log;

class ExportReferenceSkuTemplate implements FromArray, WithHeadings, ShouldAutoSize, WithColumnFormatting, WithTitle, WithEvents
{
    protected $data;
    protected $heading;
    protected $headingcell2;
    protected $formatcell;
    protected $startRow;

    public function __construct($data, $heading = [], $headingcell2 = '', $formatcell = [])
    {
        $this->data = $data;
        $this->heading = $heading;
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
                $event->sheet->getStyle($this->headingcell2)->applyFromArray([
                    'font' => [
                        'bold' => true
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
    public function title(): string
    {
        return 'Reference-Sku';
    }
}
