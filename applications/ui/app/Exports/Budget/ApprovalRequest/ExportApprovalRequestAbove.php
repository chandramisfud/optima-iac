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
use PhpOffice\PhpSpreadsheet\Style\Fill;

use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;

class ExportApprovalRequestAbove implements FromArray, WithHeadings, WithTitle, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected mixed $data;
    protected mixed $header;
    protected mixed $headerStyle;
    protected mixed $formatCell;
    protected mixed $centerTextAbove;

    public function __construct($data, $header = [], $headerStyle = '', $formatCell = [], $centerTextAbove = '')
    {
        $this->data = $data;
        $this->header = $header;
        $this->headerStyle = $headerStyle;
        $this->formatCell = $formatCell;
        $this->centerTextAbove = $centerTextAbove;
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
        return 'Approval Sheet Above 5bio';
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

                $event->sheet->getStyle('B3:B4')->applyFromArray([
                    'font' => [
                        'size'  => 20,
                        'bold'  => true
                    ],
                ]);

                $event->sheet->getStyle($this->centerTextAbove)->applyFromArray([
                    'alignment' => [
                        'horizontal' => Alignment::HORIZONTAL_CENTER,
                    ],
                ]);

                $sheet = $event->sheet->getDelegate();

                $cols = array_keys($sheet->getColumnDimensions());
                foreach ($cols as $col) {
                    if ($col === 'B') {
                        $sheet->getColumnDimension($col)->setAutoSize(false);
                    } else {
                        $sheet->getColumnDimension($col)->setAutoSize(true);
                    }
                }
            },
        ];
    }
}
