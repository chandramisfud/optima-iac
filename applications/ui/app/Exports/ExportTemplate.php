<?php
namespace App\Exports;

use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\FromArray;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;

use Maatwebsite\Excel\Concerns\WithEvents;
use Maatwebsite\Excel\Events\AfterSheet;
use PhpOffice\PhpSpreadsheet\Style\Fill;
use Maatwebsite\Excel\Concerns\WithColumnFormatting;
use Maatwebsite\Excel\Concerns\WithStrictNullComparison;
use Maatwebsite\Excel\Concerns\WithMultipleSheets;
use Maatwebsite\Excel\Concerns\WithTitle;
use Illuminate\Support\Facades\Log;

class ExportTemplate implements WithMultipleSheets
{
    use Exportable;

    // Mechanism
    protected $dataMechanism;
    protected $headingMechanism;
    protected $headerMechanism;

    // Activity
    protected $dataActivity;
    protected $headingActivity;
    protected $headerActivity;

    // Sku
    protected $dataSku;
    protected $headingSku;
    protected $headerSku;

    // Channel
    protected $dataChannel;
    protected $headingChannel;
    protected $headerChannel;

    public function __construct($dataMechanism, $headingMechanism = [], $headerMechanism = '',
                                $dataActivity, $headingActivity = [], $headerActivity = '',
                                $dataSku, $headingSku = [], $headerSku = '',
                                $dataChannel, $headingChannel = [], $headerChannel = '')
    {
        //Mechanism
        $this->dataMechanism = $dataMechanism;
        $this->headingMechanism = $headingMechanism;
        $this->headerMechanism = $headerMechanism;

        //Activity
        $this->dataActivity = $dataActivity;
        $this->headingActivity = $headingActivity;
        $this->headerActivity = $headerActivity;

        //Sku
        $this->dataSku = $dataSku;
        $this->headingSku = $headingSku;
        $this->headerSku = $headerSku;

        //Channel
        $this->dataChannel = $dataChannel;
        $this->headingChannel = $headingChannel;
        $this->headerChannel = $headerChannel;
    }

    public function sheets(): array
    {
        $sheets = [];

        $sheets[] = new ExportMassUploadTemplate($this->dataMechanism, $this->headingMechanism, $this->headerMechanism);
        $sheets[] = new ExportReferenceActivityTemplate($this->dataActivity, $this->headingActivity, $this->headerActivity);
        $sheets[] = new ExportReferenceSkuTemplate($this->dataSku, $this->headingSku, $this->headerSku);
        $sheets[] = new ExportReferenceChannelTemplate($this->dataChannel, $this->headingChannel, $this->headerChannel);

        return $sheets;
    }

//    public function columnFormats(): array
//    {
//        return $this->formatcell;
//    }
//
//    public function array(): array
//    {
//        return $this->data;
//    }
//
//    public function headings(): array
//    {
//        return $this->heading;
//    }
}
