<?php
namespace App\Exports\Budget\ApprovalRequest;

use JetBrains\PhpStorm\ArrayShape;
use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Concerns\WithTitle;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;

use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

class ExportApprovalRequestPromo implements FromArray, WithHeadings, WithTitle, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected mixed $data;
    protected mixed $header;
    protected mixed $headerStyle;
    protected mixed $formatCell;

    public function __construct($data, $header = [], $headerStyle = '', $formatCell = [])
    {
        $this->data = $data;
        $this->header = $header;
        $this->headerStyle = $headerStyle;
        $this->formatCell = $formatCell;
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
        return 'Promo ID Optima';
    }
    #[ArrayShape([AfterSheet::class => "\Closure"])] public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {
                $event->sheet->getStyle($this->headerStyle)->applyFromArray([
                    'font' => [
                        'bold' => true
                    ],
                    'fill' => [
                        'fillType' => Fill::FILL_SOLID,
                        'color' => [
                            'rgb' => 'ffff00'
                        ]
                    ]
                ]);

                $event->sheet->getStyle($this->headerStyle)->applyFromArray([
                    'font' => [
                        'bold' => true
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
}
