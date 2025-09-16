<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Promo {{ @$data->promoCreation->promo->refId }} </title>
    <style>
        @font-face {
            font-family: "Calibri";
            src: url({{ public_path('/assets/fonts/Calibri.ttf') }});
            font-size: 8pt;
        }

        hr {
            page-break-after: always;
        }
        body {
            font-family: "Calibri", serif;
        }

        table tr td {
            padding: 5px 5px 5px 5px;
            vertical-align: top;
        }

        .header_id {
            width: 100%;
            margin-bottom: 10px;
        }

        .header_id tr td {
            font-size: 12pt;
        }

        .header_id table tr > .cell_label {
            width: 250px;
        }

        .header_id table tr > .cell_separator {
            width: 10px;
        }

        .header_title {
            margin-bottom: 10px;
        }

        .header_title table {
            width: 100%;
            background-color: #8EB4E3;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            text-align: center;
        }

        .header_title table tr td {
            font-size: 20px;
            padding-top: 10px;
            padding-bottom: 10px;
            font-weight: bold;
        }

        .date_section > .table_left {
            width: 60%;
            float: left;
        }

        .date_section > .table_left tr > .cell_label {
            width: 250px;
        }

        .date_section > .table_left tr > .cell_separator {
            width: 10px;
        }

        .date_section > .table_right {
            width: 40%;
            float: right;
        }

        .date_section > .table_right tr > .cell_label {
            width: 100px;
        }

        .date_section > .table_right tr > .cell_separator {
            width: 10px;
        }

        .detail_content {
            margin-bottom: 10px;
        }

        .detail_content table {
            width: 100%;
        }

        .detail_content table  tr > .cell_label {
            width: 250px;
        }

        .detail_content table  tr > .cell_separator {
            width: 10px;
        }

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
    <div id="cycle1">
        <div class="header_id">
            <table>
                <tr>
                    <td class="cell_label">Promo ID</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoCreation->promo->refId }} </td>
                </tr>
            </table>
        </div>

        <div class="header_title">
            <table>
                <tr>
                    <td>PROMO DISPLAY</td>
                </tr>
            </table>
        </div>

        <div class="date_section">
            <table class="table_left">
                <tr>
                    <td class="cell_label">Creation Date</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @date('d M Y', strtotime($data->promoCreation->promo->createOn)) }}</td>
                </tr>
            </table>
        </div>

        <div class="detail_content">
            <table>
                <tr>
                    <td class="cell_label">Initiator</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoCreation->promo->createBy }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Account</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoCreation->promo->accountDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Channel</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoCreation->promo->channelDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Region</td>
                    <td class="cell_separator">:</td>
                    <td>
                        <?php
                        if (isset($data)) {
                            if ($data) {
                                $content = array();
                                foreach ($data->promoCreation->region as $region) {
                                    array_push($content, $region->regionDesc);
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
                    <td class="cell_label">Activity</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoCreation->promo->activityLongDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Sub Activity</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoCreation->promo->subActivityLongDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">Activity Name</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoCreation->promo->activityDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">Brand</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoCreation->promo->groupBrandLongDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">SKU</td>
                    <td class="cell_separator">:</td>
                    <td>
                        <?php
                        if ($data) {
                            $content = array();
                            foreach ($data->promoCreation->sku as $sku) {
                                array_push($content, $sku->skuDesc);
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
                    <td class="cell_label">Activity Period (DD/MM/YY)</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @date('d/m/Y', strtotime($data->promoCreation->promo->startPromo)) }}  -  {{ @date('d/m/Y', strtotime($data->promoCreation->promo->endPromo)) }}</td>
                </tr>
            </table>
        </div>

        <div class="detail_value">
            <table>
                <tr>
                    <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;width: 33%;">SKU</td>
                    <td style="background-color: #8EB4E3;font-weight: bold;width: 33%;">Mechanisme</td>
                    <td style="text-align:right;background-color: #8EB4E3;font-weight: bold;width: 33%;">Cost (IDR) </td>
                </tr>
                @foreach(@$data->promoCreation->mechanism as $mechanism)
                    <tr>
                        <td>{{ @$mechanism->skuDesc }}</td>
                        <td>{{ @$mechanism->mechanism }}</td>
                        <td style="text-align:right"> {{ @number_format($mechanism->cost,0,".",",") }} </td>
                    </tr>
                @endforeach
            </table>
        </div>

        <div class="detail_workflow">
            <table>
                <thead>
                <tr>
                    <td style="text-align:left;background-color: #8EB4E3;width: auto">Title</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Mass Approval</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Budget Approval (Deploy)</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Promo ID Approval</td>
                </tr>
                </thead>
                <tbody>
                @foreach(@$data->promoCreation->workflow as $val)
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
    </div>
@if (@$data->promoRecon != null)
    <div id="cycle2" style="page-break-before: always;">
        <div class="header_id">
            <table>
                <tr>
                    <td class="cell_label">Promo ID</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoRecon->promo->refId }} </td>
                </tr>
            </table>
        </div>

        <div class="header_title">
            <table>
                <tr>
                    <td>PROMO RECONCILIATION DISPLAY</td>
                </tr>
            </table>
        </div>

        <div class="date_section">
            <table class="table_left">
                <tr>
                    <td class="cell_label">Creation Date</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @date('d M Y', strtotime($data->promoRecon->promo->createOn)) }}</td>
                </tr>
            </table>
        </div>

        <div class="detail_content">
            <table>
                <tr>
                    <td class="cell_label">Initiator</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoRecon->promo->createBy }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Account</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoRecon->promo->accountDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Channel</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoRecon->promo->channelDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Region</td>
                    <td class="cell_separator">:</td>
                    <td>
                        <?php
                        if (isset($data)) {
                            if ($data) {
                                $content = array();
                                foreach ($data->promoRecon->region as $region) {
                                    array_push($content, $region->regionDesc);
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
                    <td class="cell_label">Activity</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @$data->promoRecon->promo->activityLongDesc }} </td>
                </tr>
                <tr>
                    <td class="cell_label">Sub Activity</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoRecon->promo->subActivityLongDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">Activity Name</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoRecon->promo->activityDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">Brand</td>
                    <td class="cell_separator">:</td>
                    <td>{{ @$data->promoRecon->promo->groupBrandLongDesc }}</td>
                </tr>
                <tr>
                    <td class="cell_label">SKU</td>
                    <td class="cell_separator">:</td>
                    <td>
                        <?php
                        if ($data) {
                            $content = array();
                            foreach ($data->promoRecon->sku as $sku) {
                                array_push($content, $sku->skuDesc);
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
                    <td class="cell_label">Activity Period (DD/MM/YY)</td>
                    <td class="cell_separator">:</td>
                    <td> {{ @date('d/m/Y', strtotime($data->promoRecon->promo->startPromo)) }}  -  {{ @date('d/m/Y', strtotime($data->promoRecon->promo->endPromo)) }}</td>
                </tr>
            </table>
        </div>

        <div class="detail_value">
            <table>
                <tr>
                    <td style="text-align:left;background-color: #8EB4E3;font-weight: bold;width: 33%;">SKU</td>
                    <td style="background-color: #8EB4E3;font-weight: bold;width: 33%;">Mechanisme</td>
                    <td style="text-align:right;background-color: #8EB4E3;font-weight: bold;width: 33%;">Cost (IDR) </td>
                </tr>
                @foreach(@$data->promoRecon->mechanism as $mechanism)
                    <tr>
                        <td>{{ @$mechanism->skuDesc }}</td>
                        <td>{{ @$mechanism->mechanism }}</td>
                        <td style="text-align:right"> {{ @number_format($mechanism->cost,0,".",",") }} </td>
                    </tr>
                @endforeach
            </table>
        </div>

        <div class="detail_workflow">
            <table>
                <thead>
                <tr>
                    <td style="text-align:left;background-color: #8EB4E3;width: auto">Title</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Mass Approval</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Budget Approval (Deploy)</td>
                    <td style="text-align:left;background-color: #8EB4E3;">Promo ID Approval</td>
                </tr>
                </thead>
                <tbody>
                @foreach(@$data->promoRecon->workflow as $val)
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

    </div>
@endif
</body>
</html>
