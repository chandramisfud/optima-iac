<?php
namespace App\Exports\Budget\ApprovalRequest;

use JetBrains\PhpStorm\Pure;
use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithMultipleSheets;

class ExportBudgetApprovalRequestDC implements WithMultipleSheets
{
    use Exportable;

    protected mixed $data;
    protected mixed $dataAbove;
    protected mixed $dataBelow;
    protected mixed $dataDetails;
    protected mixed $headerAbove;
    protected mixed $headerBelow;
    protected mixed $header;
    protected mixed $headingAboveStyle;
    protected mixed $centerTextAbove;
    protected mixed $headingStyle;
    protected mixed $arrSubHeaderMerged;
    protected mixed $arrHeaderFill;
    protected mixed $arrHeaderBorder;
    protected mixed $arrFooterBorder;
    protected mixed $arrBodyBorder;
    protected mixed $centerTextBelow;
    protected mixed $formatAboveCell;
    protected mixed $formatBelowCell;
    protected mixed $formatCell;

    public function __construct($data, $dataAbove, $dataBelow, $dataDetails,
                                $headerAbove = [], $headerBelow = [], $header = [], $centerTextAbove = '',
                                $headingAboveStyle = '', $headingStyle = '', $arrSubHeaderMerged = [], $arrHeaderFill = [], $arrHeaderBorder = [], $arrBodyBorder = [], $arrFooterBorder = [], $centerTextBelow = '',
                                $formatAboveCell = [], $formatBelowCell = [], $formatCell = [])
    {
        $this->data = $data;
        $this->dataAbove = $dataAbove;
        $this->dataBelow = $dataBelow;
        $this->dataDetails = $dataDetails;
        $this->headerAbove = $headerAbove;
        $this->centerTextAbove = $centerTextAbove;
        $this->headerBelow = $headerBelow;
        $this->header = $header;
        $this->headingAboveStyle = $headingAboveStyle;
        $this->headingStyle = $headingStyle;
        $this->arrSubHeaderMerged = $arrSubHeaderMerged;
        $this->arrHeaderFill = $arrHeaderFill;
        $this->arrHeaderBorder = $arrHeaderBorder;
        $this->arrBodyBorder = $arrBodyBorder;
        $this->arrFooterBorder = $arrFooterBorder;
        $this->centerTextBelow = $centerTextBelow;
        $this->formatAboveCell = $formatAboveCell;
        $this->formatBelowCell = $formatBelowCell;
        $this->formatCell = $formatCell;
    }

    #[Pure] public function sheets(): array
    {
        $sheets = [];

        $sheets['Approval Sheet >5bio'] = new ExportApprovalRequestAbove($this->dataAbove, $this->headerAbove, $this->headingAboveStyle, $this->formatAboveCell, $this->centerTextAbove);
        $sheets['Approval Sheet <5bio'] = new ExportApprovalRequestBelow($this->dataBelow, $this->headerBelow, $this->formatBelowCell, $this->arrSubHeaderMerged, $this->arrHeaderFill, $this->arrHeaderBorder, $this->arrBodyBorder, $this->arrFooterBorder, $this->centerTextBelow);
        $sheets['Promo ID Optima'] = new ExportApprovalRequestPromo($this->dataDetails, $this->header, $this->headingStyle, $this->formatCell);

        return $sheets;
    }
}
