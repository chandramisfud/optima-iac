<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Promo Display</title>
    <style>
        @media screen{
            @font-face {
                font-family: 'Calibri';
            }
        }
        body {
            font-family: "Calibri";
            font-size: 12px;
            font-weight: 300;
            font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;
        }
        td{
            font-family: Calibri, Arial, Helvetica, sans-serif;
        }
        .header_id { width: 100%; }
        .header_section tr td { width: 100%; margin-bottom: 5px; font-size: 13pt;}
        .header_id table tr > .cell_label { width: 250px; background-color: #F3F3F3; border: 1px solid #DAD9D9}
        .header_id table tr > .cell_separator { width: 10px; border: 1px solid #DAD9D9}
        .header_title { margin-bottom: 10px; }
        .header_title table { width: 100%; background-color: #F3F3F3; border: 1px solid #DAD9D9; border-collapse: collapse; text-align: center; }
        .header_title table tr td { font-size: 20px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; }
        .date_section > .table_left { width: 60%; float: left; }
        .date_section > .table_left tr > .cell_label { width: 250px; background-color: #F3F3F3; border: 1px solid #DAD9D9}
        .date_section > .table_left tr > .cell_separator { width: 10px; border: 1px solid #DAD9D9}
        .date_section > .table_right { width: 40%; float: right; }
        .date_section > .table_right tr > .cell_label { width: 100px; }
        .date_section > .table_right tr > .cell_separator { width: 10px; border: 1px solid #DAD9D9}
        .detail_content { margin-bottom: 10px; }
        .detail_content table { width: 100%; }
        .detail_content table  tr > .cell_label { width: 250px; background-color: #F3F3F3; border: 1px solid #DAD9D9}
        .detail_content table  tr > .cell_separator { width: 10px; border: 1px solid #DAD9D9}

        .detail_mechanism > table { width: 100%; border: 1px solid #DAD9D9; border-collapse: collapse; float: left; margin-top: 20px; margin-bottom: 20px; }
        .detail_mechanism > table .detail_mechanism_title > .col_left { border-right: 1px solid #DAD9D9; width: 50%; font-size: 14px; font-weight: bold; text-decoration: underline; }
        .detail_mechanism > table .detail_mechanism_title > .col_right { border-left: 1px solid #DAD9D9; width: 50%; font-size: 14px; font-weight: bold; text-decoration: underline; }
        .detail_mechanism > table .detail_mechanism_content > .col_left { border-right: 1px solid #DAD9D9; width: 50%; height: 100px; vertical-align: top; padding-left: 20px; }
        .detail_mechanism > table .detail_mechanism_content > .col_right { border-left: 1px solid #DAD9D9; width: 50%; height: 100px; vertical-align: top; padding-left: 20px; }

        .detail_value table { width: 100%; border: 1px solid #DAD9D9; border-collapse: collapse; float: left; margin-bottom: 20px; }
        .detail_value table tr { border: 1px solid #DAD9D9; }
        .detail_value table tr td { border: 1px solid #DAD9D9; }

        .detail_analysis table { width: 100%; border: 1px solid #DAD9D9; border-collapse: collapse; float: left; margin-bottom: 20px; }
        .detail_analysis table .detail_analysis_title td { border: 1px solid #DAD9D9; text-align: center; }
        .detail_analysis table .detail_analysis_content td { border-right: 1px solid #DAD9D9; }

        .detail_approval table { width: 100%; border: 1px solid #DAD9D9; border-collapse: collapse; float: left; }
        .detail_approval table .detail_approval_title td { text-align: center; border: 1px solid #DAD9D9; }
        .detail_approval table .detail_approval_content td { border-right: 1px solid #DAD9D9; text-align: center; height: 150px; vertical-align: bottom; }
        .detail_approval table .detail_approval_content td .detail_approval_on { border: 0px solid #DAD9D9; padding:5px 5px 5px 5px; width: 100px; margin: auto; margin-bottom: 10px; }
    </style>
</head>
<body>
<div class="header_title">
    <table style="background-color: #8EB4E3">
        <tr>
            <td>Promo Display</td>
        </tr>
    </table>
</div>
<div class="header_id">
    <table style="width: 100%">
        <tr>
            <td colspan="3">The following promo ID requires your approval : </td>
        </tr>
    </table>
</div>
<div class="detail_content">
    <table style="width: 100%">
        <tr>
            <td style="width: 20%" class="cell_label">Modified Reason</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promoHeader->modifReason }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Promo ID</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promoHeader->refId }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Creation Date</td>
            <td style="width: 80%; border: 1px solid #DAD9D9" >{{ @date('d M Y', strtotime($data->promoHeader->createOn)) }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">User/Initiator</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promoHeader->createBy }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Planning ID</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promoHeader->promoPlanRefId }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Distributor</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promoHeader->distributorName }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Channel</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->channels as $channels) {
                        if ($channels->flag) {
                            array_push($content, $channels->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Account, Sub Account</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->accounts as $accounts) {
                        if ($accounts->flag) {
                            array_push($content, $accounts->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>,
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->subAccounts as $subAccounts) {
                        if ($subAccounts->flag) {
                            array_push($content, $subAccounts->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Region / Sales Office</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->regions as $regions) {
                        if ($regions->flag) {
                            array_push($content, $regions->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Sub Brand</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                if ($data) {
                    $content = array();
                    foreach ($data->brands as $brands) {
                        if ($brands->flag) {
                            array_push($content, $brands->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">SKU</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->skus as $skus) {
                        if ($skus->flag) {
                            array_push($content, $skus->longDesc);
                        }
                    }
                    $str_content = substr(implode(", ", $content), 0, 60);
                    if (strlen($str_content) > 59) {
                        echo $str_content  . " .... ";
                    } else {
                        echo $str_content;
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Activity</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promoHeader->activityLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Sub Activity</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promoHeader->subActivityLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Activity Name</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promoHeader->activityDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Activity Period</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @date('d/m/Y', strtotime($data->promoHeader->startPromo)) }}  -  {{ @date('d/m/Y', strtotime($data->promoHeader->endPromo)) }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Budget Source</td>
            <td style="width: 80%; border: 1px solid #DAD9D9"> {{ @$data->promoHeader->allocationDesc }}</td>
        </tr>

        <tr>
            <td style="width: 20%" class="cell_label">Mechanism</td>
            <td style='width: 80%; border: 1px solid #DAD9D9'>
                <?php
                if ($data) {
                    $content = array();
                    foreach ($data->mechanisms as $mechanism) {
                        if ($mechanism->mechanism) {
                            array_push($content, $mechanism->mechanism);
                        }
                    }
                    $str_content = substr(implode("; ", $content), 0);

                    echo $str_content;
                }
                ?>
            </td>
        </tr>
    </table>
</div>

<div class="header_section">
    <table>
        <tr>
            <td colspan="3" style="color:#196DB5;text-decoration: underline;"><b>Promo Planning</b> </td>
        </tr>
    </table>
</div>
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Baseline Sales</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Incremental Sales</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Total Sales</b></td>
    </tr>
    <tr>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planNormalSales,2,".",",") }}</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planIncrSales,2,".",",") }}</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planTotSales,2,".",",") }}</td>
    </tr>
    <tr>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
    </tr>
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Investment</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Cost Ratio</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;b"><b>ROI</b></td>
    </tr>
    <tr>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planInvestment,2,".",",") }}</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planCostRatio,2,".",",") }} %</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->planRoi,2,".",",") }} %</td>
    </tr>
</table>
<br>
<div class="header_section">
    <table>
        <tr>
            <td colspan="3" style="color:#196DB5;text-decoration: underline;"><b>Promo Creation</b> </td>
        </tr>
    </table>
</div>
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black"><b>Baseline Sales</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Incremental Sales</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Total Sales</b></td>
    </tr>
    <tr>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->normalSales,2,".",",") }}</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->incrSales,2,".",",") }}</td>
        @php
            $totSales = 0;
                if(isset($data->promoHeader->totSales)) {
                    $totSales = $data->promoHeader->totSales;
                }
        @endphp
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($totSales,2,".",",") }}</td>
    </tr>
    <tr>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
    </tr>
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Investment</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Cost Ratio</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>ROI</b></td>
    </tr>
    <tr>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->investment,2,".",",") }}</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->costRatio,2,".",",")}} %</td>
        <td style="text-align:right; padding-right:10px;border: 1px solid black;">{{ @number_format($data->promoHeader->roi,2,".",",") }} %</td>
    </tr>
</table>
<br>
<div class="header_section">
    <table>
        <tr>
            <td colspan="3" style="color:#196DB5;text-decoration: underline;"><b>Approval & Notes</b> </td>
        </tr>
    </table>
</div>
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Approval 1</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Approval 2</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Approval 3</b></td>
    </tr>
    <tr>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">
            @if($data->promoHeader->userApprover1<>"")
                <div class="detail_approval_on">{{ @$data->promoHeader->lastStatus1 }}<br/>{{ @$data->promoHeader->userApprover1 }}</div>
            @endif
        </td>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">
            @if($data->promoHeader->userApprover2<>"")
                <div class="detail_approval_on">{{ @$data->promoHeader->lastStatus2 }}<br/>{{ @$data->promoHeader->userApprover2 }}</div>
            @endif
        </td>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">
            @if($data->promoHeader->userApprover3<>"")
                <div class="detail_approval_on">{{ @$data->promoHeader->lastStatus3 }}<br/> {{ @$data->promoHeader->userApprover3 }}</div>
            @endif
        </td>
    </tr>
    <tr>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
        <td style="text-align:left;padding-left: 10px;"></td>
    </tr>
    <tr style="background-color: #8EB4E3">
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Approval 4</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Approval 5</b></td>
        <td style="text-align:left;padding-left: 10px;border: 1px solid black;"><b>Notes</b></td>
    </tr>
    <tr>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">
            @if($data->promoHeader->userApprover4<>"")
                <div class="detail_approval_on">{{ @$data->promoHeader->lastStatus4 }}<br/>{{ @$data->promoHeader->userApprover4 }}</div>
            @endif
        </td>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">
            @if($data->promoHeader->userApprover5<>"")
                <div class="detail_approval_on">{{ @$data->promoHeader->lastStatus5 }}<br/> {{ @$data->promoHeader->userApprover5 }}</div>
            @endif
        </td>
        <td style="text-align:left; padding-left:10px;border: 1px solid black;">{{ @$data->promoHeader->approvalNotes }}</td>
    </tr>
</table>

<br>
<table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
    <tr style="margin-top:70px;"><td width="500">File Attachment : </td></tr>
    @if(isset($ar_fileattach[0]['row1']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row1/" . rawurlencode($ar_fileattach[0]['row1']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row1'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row2']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row2/" . rawurlencode($ar_fileattach[0]['row2']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row2'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row3']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row3/" . rawurlencode($ar_fileattach[0]['row3']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row3'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row4']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row4/" . rawurlencode($ar_fileattach[0]['row4']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row4'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row5']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row5/" . rawurlencode($ar_fileattach[0]['row5']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row5'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row6']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row6/" . rawurlencode($ar_fileattach[0]['row6']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row6'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row7']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promoHeader->id . "/row7/" . rawurlencode($ar_fileattach[0]['row7']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row7'] }}
                </a>
            </td></tr>
    @endif
</table>

<table cellspacing="0" cellpadding="0">
    <tr style="margin-top:25px;"><td>​​</td><td>​​</td><td>​​</td></tr>
    <tr>
        <td style="border-radius: 2px;" bgcolor="#5867dd">
            <a href="{{ config('app.url') . "/promo/approval/email/approve?p=" . @$paramEncrypted }}" target="_blank" style="padding: 8px 12px; border: 1px solid #5867dd;border-radius: 2px;font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;text-decoration: none;font-weight:bold;display: inline-block;">
                Approve
            </a>

        </td>
        <td width="30"></td>
        <td style="border-radius: 2px;" bgcolor="#5867dd">
            <a href="{{ config('app.url') . "/promo/approval/email/send-back?p=" . @$paramEncrypted }}" target="_blank" style="padding: 8px 12px; border: 1px solid #5867dd;border-radius: 2px;font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;text-decoration: none;font-weight:bold;display: inline-block;">
                Send Back
            </a>
        </td>
    </tr>
</table>
<p><p>Thank you,<br><a href=''>Optima System</a>
</body>
</html>
