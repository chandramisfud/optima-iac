<?php
namespace App\Exports\Budget\Volume;

use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithMultipleSheets;

class ExportTemplateBudgetVolume implements WithMultipleSheets
{
    use Exportable;

    // Volume
    protected $dataVolume;
    protected $headingVolume;
    protected $headingVolumeCell1;
    protected $headingVolumeCell2;
    protected $formatCellVolume;
    protected $cellNotInput;

    // Region
    protected $dataRegion;
    protected $headingRegion;
    protected $headerRegion;

    public function __construct($dataVolume, $headingVolume = [], $headingVolumeCell1 = '', $headingVolumeCell2 = '', $formatCellVolume = [],
                                $dataRegion, $headingRegion = [], $headerRegion = '', $cellNotInput = '')
    {
        //Volume
        $this->dataVolume = $dataVolume;
        $this->headingVolume = $headingVolume;
        $this->headingVolumeCell1 = $headingVolumeCell1;
        $this->headingVolumeCell2 = $headingVolumeCell2;
        $this->formatCellVolume = $formatCellVolume;
        $this->cellNotInput = $cellNotInput;

        //Region
        $this->dataRegion = $dataRegion;
        $this->headingRegion = $headingRegion;
        $this->headerRegion = $headerRegion;
    }

    public function sheets(): array
    {
        $sheets = [];

        $sheets[] = new ExportBudgetSSInputVolume($this->dataVolume, $this->headingVolume, $this->headingVolumeCell1, $this->headingVolumeCell2, $this->formatCellVolume, $this->cellNotInput);
        $sheets[] = new ExportReferenceRegionTemplate($this->dataRegion, $this->headingRegion, $this->headerRegion);

        return $sheets;
    }
}
