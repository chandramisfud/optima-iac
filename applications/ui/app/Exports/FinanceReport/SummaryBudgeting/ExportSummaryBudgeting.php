<?php
namespace App\Exports\FinanceReport\SummaryBudgeting;

use JetBrains\PhpStorm\Pure;
use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithMultipleSheets;

class ExportSummaryBudgeting implements WithMultipleSheets
{
    use Exportable;

    protected mixed $dataSummaryDSN;
    protected mixed $titleSummaryDSN;
    protected mixed $formatColumnSummaryDSN;

    protected mixed $data;
    protected mixed $arrMergeHeader;
    protected mixed $arrRowHeader;
    protected mixed $arrRowSubHeader;
    protected mixed $arrRowSubTotal;
    protected mixed $arrRowGrandTotal;
    protected mixed $formatColumn;

    protected mixed $dataDCSummary;
    protected mixed $arrMergeHeaderDCSummary;
    protected mixed $arrRowHeaderDCSummary;
    protected mixed $arrRowSubHeaderDCSummary;
    protected mixed $arrRowTotalDCSummary;
    protected mixed $formatColumnDCSummary;

    public function __construct($dataSummaryDSN, $data, $dataDCSummary,
                                $titleSummaryDSN = '', $formatColumnSummaryDSN = [],
                                $arrMergeHeader = [], $arrRowHeader = [], $arrRowSubHeader = [], $arrRowSubTotal = [], $arrRowGrandTotal = [], $formatColumn = [],
                                $arrMergeHeaderDCSummary = [], $arrRowHeaderDCSummary = [], $arrRowSubHeaderDCSummary = [], $arrRowTotalDCSummary = [], $formatColumnDCSummary = []
    )

    {

        $this->dataSummaryDSN = $dataSummaryDSN;
        $this->titleSummaryDSN = $titleSummaryDSN;
        $this->formatColumnSummaryDSN = $formatColumnSummaryDSN;

        $this->data = $data;
        $this->arrMergeHeader = $arrMergeHeader;
        $this->arrRowHeader = $arrRowHeader;
        $this->arrRowSubHeader = $arrRowSubHeader;
        $this->arrRowSubTotal = $arrRowSubTotal;
        $this->arrRowGrandTotal = $arrRowGrandTotal;
        $this->formatColumn = $formatColumn;

        $this->dataDCSummary = $dataDCSummary;
        $this->arrMergeHeaderDCSummary = $arrMergeHeaderDCSummary;
        $this->arrRowHeaderDCSummary = $arrRowHeaderDCSummary;
        $this->arrRowSubHeaderDCSummary = $arrRowSubHeaderDCSummary;
        $this->arrRowTotalDCSummary = $arrRowTotalDCSummary;
        $this->formatColumnDCSummary = $formatColumnDCSummary;
    }

    #[Pure] public function sheets(): array
    {
        $sheets = [];

        $sheets['Summary DSN'] = new ExportSummaryDSN($this->dataSummaryDSN, $this->titleSummaryDSN, $this->formatColumnSummaryDSN);
        $sheets['Total Summary'] = new ExportTotalSummary($this->data, $this->arrMergeHeader, $this->arrRowHeader, $this->arrRowSubHeader, $this->arrRowSubTotal, $this->arrRowGrandTotal, $this->formatColumn);
        $sheets['DC Summary'] = new ExportDCSummary($this->dataDCSummary, $this->arrMergeHeaderDCSummary, $this->arrRowHeaderDCSummary, $this->arrRowSubHeaderDCSummary, $this->arrRowTotalDCSummary, $this->formatColumnDCSummary);

        return $sheets;
    }
}
