<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Promo {{ @json_decode($arrData[$i])->promoHeader->refId }} </title>
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
            font-family: "Calibri";
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

        .date_section {
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

        .detail_mechanism > table {
            width: 100%;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            float: left;
            margin-top: 20px;
            margin-bottom: 10px;
        }

        .detail_mechanism > table .detail_mechanism_title > .col_left {
            border-right: 1px solid #8EB4E3;
            width: 50%;
            font-size: 14px;
            font-weight: 600;
            text-decoration: underline;
        }

        .detail_mechanism > table .detail_mechanism_title > .col_right {
            border-left: 1px solid #8EB4E3;
            width: 50%;
            font-size: 14px;
            font-weight: 600;
            text-decoration: underline;
        }

        .detail_mechanism > table .detail_mechanism_content > .col_left {
            border-right: 1px solid #8EB4E3;
            width: 50%;
            height: 100px;
            vertical-align: top;
            padding-left: 20px;
        }

        .detail_mechanism > table .detail_mechanism_content > .col_right {
            border-left: 1px solid #8EB4E3;
            width: 50%;
            height: 100px;
            vertical-align: top;
            padding-left: 20px;
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

        .detail_analysis table{
            width: 100%;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            float: left;
            margin-bottom:10px;
        }
        .detail_analysis table tr{

            border: 1px solid #8EB4E3;

        }

        .detail_analysis table .detail_analysis_title td {
            /* border: 1px solid black; */
            text-align: center;
        }

        .detail_analysis table .detail_analysis_content td {
        }

        .detail_approval {

        }

        .detail_approval table {
            width: 100%;
            border: 1px solid #8EB4E3;
            border-collapse: collapse;
            float: left;
        }

        .detail_approval table .detail_approval_title td {
            text-align: center;
            border: 1px solid #8EB4E3;

        }

        .detail_approval table .detail_approval_content td {
            border-right: 1px solid #8EB4E3;
            font-size: 14px;
            text-align: center;
        }

        .detail_approval table .detail_approval_content td .detail_approval_on {
            border: 0px solid #8EB4E3;
            width: 100%;
            text-align: center;
            margin: auto;
            vertical-align: top;
        }
    </style>
</head>
<body>
@for ($i=0;$i<count($arrData);$i++)
    <div class="header_id">
        <table>
            <tr>
                <td class="cell_label">Promo ID</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->refId }} </td>
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
                <td>{{ @date('d M Y', strtotime(json_decode($arrData[$i])->promoHeader->createOn)) }}</td>
            </tr>
        </table>
    </div>

    <div class="detail_content">
        <table>
            <tr>
                <td class="cell_label">Initiator</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->createBy }} </td>
            </tr>
            <tr>
                <td class="cell_label">Account</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->accounts as $accounts) {
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
                        ?>
                </td>
            </tr>
            <tr>
                <td class="cell_label">Channel</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->channels as $channels) {
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
                <td class="cell_label">Region</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->regions as $regions) {
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
                <td class="cell_label">Activity</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->activityLongDesc }} </td>
            </tr>
            <tr>
                <td class="cell_label">Sub Activity</td>
                <td class="cell_separator">:</td>
                <td>
                    {{ @json_decode($arrData[$i])->promoHeader->subActivityLongDesc }}

                </td>
            </tr>
            <tr>
                <td class="cell_label">Activity Name</td>
                <td class="cell_separator">:</td>
                <td>
                    {{ @json_decode($arrData[$i])->promoHeader->activityDesc }}
                </td>
            </tr>
            <tr>
                <td class="cell_label">Sub Brand</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        if (json_decode($arrData[$i])) {
                            $content = array();
                            foreach (json_decode($arrData[$i])->brands as $brands) {
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
                <td class="cell_label">SKU</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->skus as $skus) {
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
                <td class="cell_label">Activity Period (DD/MM/YY)</td>
                <td class="cell_separator">:</td>
                <td> {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->startPromo)) }}  -  {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->endPromo)) }}</td>
            </tr>
        </table>
    </div>

    <div class="detail_mechanism">
        <table>
            <tr class="detail_mechanism_title">
                <td class="col_right">Mechanism</td>
            </tr>
            <tr class="detail_mechanism_content">
                <td class="col_right">
                        <?php
                        if (json_decode($arrData[$i])) {
                            $content = array();
                            foreach (json_decode($arrData[$i])->mechanisms as $mechanism) {
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

    <div class="detail_value">
        <table>
            <tr>
                <td style="background-color: #8EB4E3;">SKUs</td>
                <td style="text-align:right;background-color: #8EB4E3;">(a) Baseline Sales</td>
                <td style="text-align:right;background-color: #8EB4E3;">(b) Incremental Sales</td>
                <td style="text-align:right;background-color: #8EB4E3;">Investment (IDR)</td>
            </tr>
            <tr>
                <td>All Value</td>
                <td style="text-align:right"> 0 </td>
                <td style="text-align:right"> 0 </td>
                <td style="text-align:right"> 0 </td>
            </tr>
            <tr>
                <td><b>Total</b></td>
                <td style="text-align:right"><b>0</b></td>
                <td style="text-align:right"><b>0</b></td>
                <td style="text-align:right"><b>0 </b></td>
            </tr>
        </table>
    </div>

    <div class="detail_analysis">
        <table>
            <tr class="detail_analysis_title">
                <td class="col_left" style="background-color: #8EB4E3;">Description</td>
                <td class="col_mid" width="35%" style="background-color: #8EB4E3;">Investment Analysis</td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Total sales (a+b)</td>
                <td class="col_mid" style="text-align:right">
                    0
                </td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Investment (c)</td>
                <td class="col_mid" style="text-align:right"> 0</td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">ROI</td>
                <td class="col_mid" style="text-align:right">  0 %</td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Cost Ratio</td>
                <td class="col_mid" style="text-align:right">0 %</td>
            </tr>
        </table>
    </div>
    <div >
        <table>
            <tr>
                <td style="color: #8EB4E3; font-size:14px">Cycle 1</td>
            </tr>
        </table>
    </div>
    <div class="detail_approval">
        <table>
            <tr class="detail_approval_title">
                <td width="20%" style="background-color: #8EB4E3;">Approval 1</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 2</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 3</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 4</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 5</td>
            </tr>
            <tr class="detail_approval_content">
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover1prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus1prev == "Approved" &&json_decode($arrData[$i])->promoHeader->approvalDate1prev <> "")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus1prev }}   by</br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover1prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover1prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate1prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate1prev)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover1prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus1prev !== "Approved")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus1prev }} </br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover1prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover1prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate1prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate1prev)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus1prev }} </br>
                            </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover1prevName}}
                            @endif
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover2prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus2prev == "Approved"  &&json_decode($arrData[$i])->promoHeader->approvalDate2prev <> "")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus2prev }}   by</br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover2prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover2prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate2prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate2prev)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover2prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus2prev !== "Approved")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus2prev }} </br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover2prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover2prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate2prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate2prev)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus2prev }} </br>
                            </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover2prevName}}
                            @endif
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover3prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus3prev == "Approved" &&json_decode($arrData[$i])->promoHeader->approvalDate3prev <> "")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus3prev }}   by</br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover3prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover3prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate3prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate3prev)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover3prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus3prev !== "Approved" )
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus3prev }} </br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover3prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover3prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate3prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate3prev)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus3prev }} </br>
                            </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover3prevName}}
                            @endif
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover4prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus4prev == "Approved" &&json_decode($arrData[$i])->promoHeader->approvalDate4prev <> "")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus4prev }}   by</br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover4prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover4prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate4prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate4prev)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover4prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus4prev !== "Approved")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus4prev }} </br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover4prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover4prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate4prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate4prev)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus4prev }} </br>
                            </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover4prevName}}
                            @endif
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover5prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus5prev =="Approved" &&json_decode($arrData[$i])->promoHeader->approvalDate5prev <> "")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus5prev }}   by</br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover5prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover5prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate5prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate5prev)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover5prev<>"" &&json_decode($arrData[$i])->promoHeader->lastStatus5prev !=="Approved")
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus5prev }} </br>
                            {{@json_decode($arrData[$i])->promoHeader->userApprover5prev }} </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover5prevName}}
                            @endif </br>
                            @if(json_decode($arrData[$i])->promoHeader->approvalDate5prev == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate5prev)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{@json_decode($arrData[$i])->promoHeader->lastStatus5prev }} </br>
                            </br></br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5prevName) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5prevName,0,14)."..."}}
                            @else
                                {{@json_decode($arrData[$i])->promoHeader->userApprover5prevName}}
                            @endif
                        </div>
                    @endif
                </td>
            </tr>
        </table>
    </div>

    <div >
        <table>
            <tr>
                <td style="color: #8EB4E3; font-size:14px">Cycle 2</td>
            </tr>
        </table>
    </div>
    <div class="detail_approval">
        <table>
            <tr class="detail_approval_title">
                <!-- <td style="vertical-align: top; text-align: left;background-color: #8EB4E3;" width="20%">Cycle 2 </td> -->
                <td width="20%" style="background-color: #8EB4E3;">Approval 1</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 2</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 3</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 4</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 5</td>
            </tr>
            <tr class="detail_approval_content">
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br>A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
            </tr>
        </table>
    </div>

    <hr>

    <div class="header_id">
        <table>
            <tr>
                <td class="cell_label">Promo ID</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->refId }} </td>
            </tr>
            {{-- <tr>
                <td class="cell_label">Reference</td>
                <td class="cell_separator">:</td>
                <td></td>
            </tr> --}}
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
                <td> {{ @date('d M Y', strtotime(json_decode($arrData[$i])->promoHeader->createOn)) }} </td>
            </tr>
            <tr>
                <td class="cell_label">Last Modified Date</td>
                <td class="cell_separator">:</td>
                <td> {{ @date('d M Y', strtotime(json_decode($arrData[$i])->promoHeader->modifiedOn)) }} </td>
            </tr>
            {{-- <tr>
                <td class="cell_label">Last Update</td>
                <td class="cell_separator">:</td>
                <td></td>
            </tr> --}}
        </table>
        {{-- <table class="table_right">
            <tr>
                <td class="cell_label">Cut Off Claim</td>
                <td class="cell_separator">:</td>
                <td></td>
            </tr>
        </table> --}}
    </div>

    <div class="detail_content">
        <table>
            <tr>
                <td class="cell_label">Initiator</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->createBy }} </td>
            </tr>
            <tr>
                <td class="cell_label">Account</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->accounts as $accounts) {
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
                        ?>
                </td>
            </tr>
            <tr>
                <td class="cell_label">Channel</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->channels as $channels) {
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
                <td class="cell_label">Region</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->regions as $regions) {
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
            {{-- <tr>
                <td class="cell_label">Selling Point</td>
                <td class="cell_separator">:</td>
                <td></td>
            </tr> --}}
            <tr>
                <td class="cell_label">Activity</td>
                <td class="cell_separator">:</td>
                <td> {{ @json_decode($arrData[$i])->promoHeader->activityLongDesc }} </td>
            </tr>
            <tr>
                <td class="cell_label">Sub Activity</td>
                <td class="cell_separator">:</td>
                <td>
                    {{ @json_decode($arrData[$i])->promoHeader->subActivityLongDesc }}

                </td>
            </tr>
            <tr>
                <td class="cell_label">Activity Name</td>
                <td class="cell_separator">:</td>
                <td>
                    {{ @json_decode($arrData[$i])->promoHeader->activityDesc }}
                </td>
            </tr>
            <tr>
                <td class="cell_label">Sub Brand</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        if (json_decode($arrData[$i])) {
                            $content = array();
                            foreach (json_decode($arrData[$i])->brands as $brands) {
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
                <td class="cell_label">SKU</td>
                <td class="cell_separator">:</td>
                <td>
                        <?php
                        $content = array();
                        if (json_decode($arrData[$i])) {
                            foreach (json_decode($arrData[$i])->skus as $skus) {
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
                <td class="cell_label">Activity Period (DD/MM/YY)</td>
                <td class="cell_separator">:</td>
                <td> {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->startPromo)) }}  -  {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->endPromo)) }}</td>
            </tr>
            {{-- <tr>
                <td class="cell_label">Number of Outlet (s)</td>
                <td class="cell_separator">:</td>
                <td></td>
            </tr> --}}
        </table>
    </div>

    <div class="detail_mechanism">
        <table>
            <tr class="detail_mechanism_title">
                {{-- <td class="col_left">Objective (s)</td> --}}
                <td class="col_right">Mechanism</td>
            </tr>
            <tr class="detail_mechanism_content">
                {{-- <td class="col_left">Sales Growth (%) : 0.00</td> --}}
                <td class="col_right">
                        <?php
                        //$content = array();
                        if (json_decode($arrData[$i])) {
                            $content = array();
                            foreach (json_decode($arrData[$i])->mechanisms as $mechanism) {
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

    <div class="detail_value">
        <table>
            <tr>
                <td style="background-color: #8EB4E3;">SKUs</td>
                <td style="text-align:right;background-color: #8EB4E3;">(a) Baseline Sales</td>
                <td style="text-align:right;background-color: #8EB4E3;">(b) Incremental Sales</td>
                <td style="text-align:right;background-color: #8EB4E3;">Investment (IDR)</td>
            </tr>
            <tr>
                <td>All Value</td>
                <td style="text-align:right"> 0 </td>
                <td style="text-align:right"> 0 </td>
                <td style="text-align:right"> 0 </td>
            </tr>
            <tr>
                <td><b>Total</b></td>
                <td style="text-align:right"><b>0</b></td>
                <td style="text-align:right"><b>0</b></td>
                <td style="text-align:right"><b>0</b></td>
            </tr>
            {{--<tr>
                <td><b>Running Rate Cost</b></td>
                <td style="text-align:right"><b></b></td>
                <td style="text-align:right"><b> AAAAAAAAAAAAAAAAAAAAAAA  %</b></td>
                <td style="text-align:right"><b></b></td>
            </tr>--}}
        </table>
    </div>

    <div class="detail_analysis">
        <table>
            <tr class="detail_analysis_title">
                <td class="col_left" style="background-color: #8EB4E3;">Description</td>
                <td class="col_mid" width="35%" style="background-color: #8EB4E3;">Investment Analysis</td>
                {{-- <td class="col_right" width="40%">Category</td> --}}
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Total sales (a+b)</td>
                <td class="col_mid" style="text-align:right">
                    0
                </td>
                {{-- <td class="col_right" style="vertical-align: top;" rowspan="4">
                     @json_decode($arrData[$i])->promoHeader->categoryDesc
                </td> --}}
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Investment (c)</td>
                <td class="col_mid" style="text-align:right"> 0</td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">ROI</td>
                <td class="col_mid" style="text-align:right">  0 %</td>
            </tr>
            <tr class="detail_analysis_content">
                <td class="col_left">Cost Ratio</td>
                <td class="col_mid" style="text-align:right">0 %</td>
            </tr>
        </table>
    </div>
    <div >
        <table>
            <tr>
                <td style="color: #8EB4E3; font-size:14px">Cycle 1</td>
            </tr>
        </table>
    </div>
    <div class="detail_approval">
        <table>
            <tr class="detail_approval_title">
                <td width="16%" style="background-color: #8EB4E3;">Approval 1</td>
                <td width="16%" style="background-color: #8EB4E3;">Approval 2</td>
                <td width="16%" style="background-color: #8EB4E3;">Approval 3</td>
                <td width="16%" style="background-color: #8EB4E3;">Approval 4</td>
                <td width="16%" style="background-color: #8EB4E3;">Approval 5</td>
            </tr>
            <tr class="detail_approval_content">
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
                <td>
                    <div class="detail_approval_on" style="color: white">A </br> A </br> A</br>A</div>
                </td>
            </tr>
        </table>
    </div>
    <div >
        <table>
            <tr>
                <td style="color: #8EB4E3; font-size:14px">Cycle 2</td>
            </tr>
        </table>
    </div>
    <div class="detail_approval">
        <table>
            <tr class="detail_approval_title">
                <!-- <td style="vertical-align: top; text-align: left;background-color: #8EB4E3;" width="20%">Cycle 2 </td> -->
                <td width="20%" style="background-color: #8EB4E3;">Approval 1</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 2</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 3</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 4</td>
                <td width="20%" style="background-color: #8EB4E3;">Approval 5</td>
            </tr>
            <!-- <tr class="detail_approval_content">
                <td rowspan="4" style="vertical-align: top; text-align: left" width="20%">
                    {{ @json_decode($arrData[$i])->promoHeader->notes }}
            </td>
        </tr> -->
            <tr class="detail_approval_content">
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover1<>"" && json_decode($arrData[$i])->promoHeader->lastStatus1 == "Approved" && json_decode($arrData[$i])->promoHeader->approvalDate1 <> "")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus1 }}   by</br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover1 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover1Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate1 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate1)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover1<>"" && json_decode($arrData[$i])->promoHeader->lastStatus1 !== "Approved")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus1 }} </br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover1 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover1Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate1 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate1)) }}
                            @endif

                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus1 }} </br>
                            </br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover1Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover1Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover1Name}}
                            @endif</br></br>
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover2<>"" && json_decode($arrData[$i])->promoHeader->lastStatus2 == "Approved"  && json_decode($arrData[$i])->promoHeader->approvalDate2 <> "")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus2 }}   by</br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover2 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover2Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate2 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate2)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover2<>"" && json_decode($arrData[$i])->promoHeader->lastStatus2 !== "Approved")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus2 }} </br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover2 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover2Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate2 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate2)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus2 }} </br>
                            </br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover2Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover2Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover2Name}}
                            @endif</br></br>
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover3<>"" && json_decode($arrData[$i])->promoHeader->lastStatus3 == "Approved" && json_decode($arrData[$i])->promoHeader->approvalDate3 <> "")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus3 }}   by</br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover3 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover3Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate3 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate3)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover3<>"" && json_decode($arrData[$i])->promoHeader->lastStatus3 !== "Approved" )
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus3 }} </br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover3 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover3Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate3 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate3)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus3 }} </br>
                            </br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover3Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover3Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover3Name}}
                            @endif</br></br>
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover4<>"" && json_decode($arrData[$i])->promoHeader->lastStatus4 == "Approved" && json_decode($arrData[$i])->promoHeader->approvalDate4 <> "")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus4 }}   by</br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover4 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover4prev}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate4 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate4)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover4<>"" && json_decode($arrData[$i])->promoHeader->lastStatus4 !== "Approved")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus4 }} </br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover4 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover4Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate4 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate4)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus4 }} </br>
                            </br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover4Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover4Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover4}}
                            @endif</br></br>
                        </div>
                    @endif
                </td>
                <td>
                    @if(json_decode($arrData[$i])->promoHeader->userApprover5<>"" && json_decode($arrData[$i])->promoHeader->lastStatus5 =="Approved" && json_decode($arrData[$i])->promoHeader->approvalDate5 <> "")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus5 }}   by</br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover5 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover5Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate5 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate5)) }}
                            @endif
                        </div>
                    @elseif(json_decode($arrData[$i])->promoHeader->userApprover5<>"" && json_decode($arrData[$i])->promoHeader->lastStatus5 !=="Approved")
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus5 }} </br>
                            {{ @json_decode($arrData[$i])->promoHeader->userApprover5 }} </br></br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover5Name}}
                            @endif </br>
                            @if(@json_decode($arrData[$i])->promoHeader->approvalDate5 == null)
                                {{""}}
                            @else
                                {{ @date('d/m/Y', strtotime(json_decode($arrData[$i])->promoHeader->approvalDate5)) }}
                            @endif
                        </div>
                    @else
                        <div class="detail_approval_on">
                            {{ @json_decode($arrData[$i])->promoHeader->lastStatus5 }} </br>
                            </br>
                            @if(@strlen(json_decode($arrData[$i])->promoHeader->userApprover5Name) > 15)
                                {{ @substr(json_decode($arrData[$i])->promoHeader->userApprover5Name,0,14)."..."}}
                            @else
                                {{ @json_decode($arrData[$i])->promoHeader->userApprover5Name}}
                            @endif</br></br>
                        </div>
                    @endif
                </td>
            </tr>

        </table>
    </div>
    @if($i < (count($arrData)-1))
        <hr>
    @endif
@endfor

<body><html>
