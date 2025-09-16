<html lang="en">
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

        .detail_value table {
            width: 100%;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            float: left;
            margin-bottom: 10px;
        }

        .detail_value table tr {
            border: 1px solid black;
        }

        .detail_value table tr td {
            border: 1px solid #8EB4E3;
        }

        .detail_workflow table {
            width: 100%;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            float: left;
            margin-bottom: 10px;
        }

        .detail_workflow table tr {
            border: 1px solid black;
        }

        .detail_workflow table tr td {
            border: 1px solid #8EB4E3;
        }
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
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promo->modifReason }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Promo ID</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promo->refId }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Creation Date</td>
            <td style="width: 80%; border: 1px solid #DAD9D9" >{{ @date('d M Y', strtotime($data->promo->createOn)) }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">User/Initiator</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">{{ @$data->promo->createBy }}</td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Distributor</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->distributorLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Channel</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->channelDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Account, Sub Account</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->accountDesc . ',' . @$data->promo->subAccountDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Region / Sales Office</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if (isset($data)) {
                    if ($data) {
                        foreach ($data->region as $regions) {
                            array_push($content, $regions->regionDesc);
                        }
                        $str_content = substr(implode(", ", $content), 0, 60);
                        if (strlen($str_content) > 59) {
                            echo $str_content  . " .... ";
                        } else {
                            echo $str_content;
                        }
                    }
                }
                ?>
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Brand</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->groupBrandLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">SKU</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                <?php
                $content = array();
                if ($data) {
                    foreach ($data->sku as $skus) {
                        array_push($content, $skus->skuDesc);
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
                {{ @$data->promo->activityLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Sub Activity</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->subActivityLongDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Activity Name</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @$data->promo->activityDesc }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Activity Period</td>
            <td style="width: 80%; border: 1px solid #DAD9D9">
                {{ @date('d/m/Y', strtotime($data->promo->startPromo)) }}  -  {{ @date('d/m/Y', strtotime($data->promo->endPromo)) }}
            </td>
        </tr>
        <tr>
            <td style="width: 20%" class="cell_label">Budget Name</td>
            <td style="width: 80%; border: 1px solid #DAD9D9"> {{ @$data->promo->budgetName }}</td>
        </tr>
    </table>
</div>

<div class="detail_value">
    <table>
        <tr>
            <td style="text-align:left;background-color: #8EB4E3;">SKU</td>
            <td style="text-align:right;background-color: #8EB4E3;width: 33%">Cost (IDR) </td>
        </tr>
        <tr>
            <td>All Value</td>
            <td style="text-align:right"> {{ @number_format($data->mechanism[0]->cost,0,".",",") }} </td>
        </tr>
    </table>
</div>

<div class="detail_workflow">
    <table>
        <thead>
        <tr>
            <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;width: auto">Title</td>
            <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;">Mass Approval</td>
            <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;">Budget Approval (Deploy)</td>
            <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;">Promo ID Approval</td>
        </tr>
        </thead>
        <tbody>
        @foreach(@$data->workflow as $val)
            <tr>
                <td>{{ @$val->title }}</td>
                <td>{{ @$val->masApproval }}</td>
                <td>{{ @$val->budgetDeployed }}</td>
                <td>{{ @$val->promoIdApproval }}</td>
            </tr>
        @endforeach
        </tbody>
    </table>
</div>

<br>
<table style="width: 100%; border-collapse: collapse; margin-bottom: 10px;" cellspacing="0" cellpadding="0">
    <tr style="margin-top:70px;"><td width="500">File Attachment : </td></tr>
    @if(isset($ar_fileattach[0]['row1']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row1/" . rawurlencode($ar_fileattach[0]['row1']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row1'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row2']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row2/" . rawurlencode($ar_fileattach[0]['row2']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row2'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row3']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row3/" . rawurlencode($ar_fileattach[0]['row3']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row3'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row4']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row4/" . rawurlencode($ar_fileattach[0]['row4']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row4'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row5']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row5/" . rawurlencode($ar_fileattach[0]['row5']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row5'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row6']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row6/" . rawurlencode($ar_fileattach[0]['row6']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row6'] }}
                </a>
            </td></tr>
    @endif
    @if(isset($ar_fileattach[0]['row7']))
        <tr><td>
                <a href="{{ config('app.url') . "/assets/media/promo/" .  $data->promo->id . "/row7/" . rawurlencode($ar_fileattach[0]['row7']) }}" target="_blank" >
                    {{ $ar_fileattach[0]['row7'] }}
                </a>
            </td></tr>
    @endif
</table>

<table cellspacing="0" cellpadding="0">
    <tr style="margin-top:25px;"><td></td><td></td><td></td></tr>
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
