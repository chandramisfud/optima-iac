<?php
namespace App\Exports\Tools;

use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\AfterSheet;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;
use Illuminate\Support\Facades\Log;

// class Export implements FromArray, WithHeadings, ShouldAutoSize, WithEvents, WithDrawings
class ExportSAPAccrual implements FromArray, ShouldAutoSize, WithEvents, WithColumnFormatting, WithStrictNullComparison
{
    protected $data;
    protected $headingall;
    protected $formatcell;
    protected $startRow;

    public function __construct($data,$headingall,$formatcell)
    {
        $this->data = $data;
        $this->headingall = $headingall;
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

    public function registerEvents(): array
    {
        return [
            AfterSheet::class => function (AfterSheet $event) {

                $event->sheet->getStyle($this->headingall)->applyFromArray([

                    'borders' => array(
                        'allBorders' => array(
                            'borderStyle' => \PhpOffice\PhpSpreadsheet\Style\Border::BORDER_THIN,
                            'color' => array('argb' => '0000'),
                        ),
                    ),
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
