<?php
namespace App\Exports\Budget\TTConsole;

use App\Exports\Budget\TTConsole\ExportBudgetTTConsole;
use App\Exports\Budget\TTConsole\ExportReferenceAccountTemplate;
use App\Exports\Budget\TTConsole\ExportReferenceActivityTemplate;
use App\Exports\Budget\TTConsole\ExportReferenceBrandTemplate;
use App\Exports\Budget\TTConsole\ExportReferenceDistributorTemplate;
use Maatwebsite\Excel\Concerns\Exportable;
use Maatwebsite\Excel\Concerns\WithMultipleSheets;

class ExportTemplateBudgetTTConsole implements WithMultipleSheets
{
    use Exportable;

    // Volume
    protected $dataTTConsole;
    protected $headingTTConsole;
    protected $headingTTConsoleCell1;
    protected $headingTTConsoleCell2;
    protected $formatCellTTConsole;
    protected $cellNoInput;

    // Activity
    protected $dataActivity;
    protected $headingActivity;
    protected $headerActivity;

    // Account
    protected $dataAccount;
    protected $headingAccount;
    protected $headerAccount;

    // Distributor
    protected $dataDistributor;
    protected $headingDistributor;
    protected $headerDistributor;

    // Brand
    protected $dataBrand;
    protected $headingBrand;
    protected $headerBrand;
    public function __construct($dataTTConsole, $headingTTConsole = [], $headingTTConsoleCell1 = '', $headingTTConsoleCell2 = '', $formatCellTTConsole = [],
                                $dataActivity, $headingActivity = [], $headerActivity = '',
                                $dataAccount, $headingAccount = [], $headerAccount = '',
                                $dataDistributor, $headingDistributor = [], $headerDistributor = '',
                                $dataBrand, $headingBrand = [], $headerBrand = '', $cellNoInput = '')
    {
        //Volume
        $this->dataTTConsole = $dataTTConsole;
        $this->headingTTConsole = $headingTTConsole;
        $this->headingTTConsoleCell1 = $headingTTConsoleCell1;
        $this->headingTTConsoleCell2 = $headingTTConsoleCell2;
        $this->formatCellTTConsole = $formatCellTTConsole;
        $this->cellNoInput = $cellNoInput;

        //Activity
        $this->dataActivity = $dataActivity;
        $this->headingActivity = $headingActivity;
        $this->headerActivity = $headerActivity;

        //Account
        $this->dataAccount = $dataAccount;
        $this->headingAccount = $headingAccount;
        $this->headerAccount = $headerAccount;

        //Distributor
        $this->dataDistributor = $dataDistributor;
        $this->headingDistributor = $headingDistributor;
        $this->headerDistributor = $headerDistributor;

        //Brand
        $this->dataBrand = $dataBrand;
        $this->headingBrand = $headingBrand;
        $this->headerBrand = $headerBrand;
    }

    public function sheets(): array
    {
        $sheets = [];

        $sheets[] = new ExportBudgetTTConsole($this->dataTTConsole, $this->headingTTConsole, $this->headingTTConsoleCell1, $this->headingTTConsoleCell2, $this->formatCellTTConsole, $this->cellNoInput);
        $sheets[] = new ExportReferenceActivityTemplate($this->dataActivity, $this->headingActivity, $this->headerActivity);
        $sheets[] = new ExportReferenceAccountTemplate($this->dataAccount, $this->headingAccount, $this->headerAccount);
        $sheets[] = new ExportReferenceDistributorTemplate($this->dataDistributor, $this->headingDistributor, $this->headerDistributor);
        $sheets[] = new ExportReferenceBrandTemplate($this->dataBrand, $this->headingBrand, $this->headerBrand);

        return $sheets;
    }
}
