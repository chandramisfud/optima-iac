<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>
        Approval Sheet Above 5 Bio
    </title>

    <style>
        @font-face {
            font-family: "Calibri";
            src: url({{ public_path('assets/fonts/Calibri.ttf') }});
        }
        body {
            font-family: 'Calibri', sans-serif;
            padding: 10px;
        }

        .title {
            font-size: 16pt;
            font-weight: bold;
            padding-bottom: 100px;
        }

        #table_body {
            margin-top: 30px;
            margin-bottom: 200px;
            width: 100%;
            border-collapse: collapse;
            font-size: 12pt;
        }

        #table_body tr:last-child th {
            background-color: #ffff00;
            text-align: left;
        }

        #table_body tr td {
            border-bottom: 1px solid #dce6f1;
        }

        .text-right {
            text-align: right;
        }

        #signature {
            width: 100%;
            font-size: 12pt;
        }

        #signature tr td {
            text-align: center;
        }

        #footer {
            position: fixed;
            bottom: 15px;
            width: 100%;
            font-size: 12pt;
        }
    </style>
</head>
<body>
<span class="title">
    APPROVAL SHEET
</span>
<br/>
<span class="title">
    OPTIMA PROMO ID {{ @strtoupper($data->period->periodDesc) }} {{ @$data->period->period }} (Above IDR 5bio)
</span>
<table id="table_body">
    <thead>
    <tr>
        <th>Period</th>
        <th>Promo ID</th>
        <th>Entity</th>
        <th>Distributor</th>
        <th>Activity</th>
        <th>Activity Name</th>
        <th>Channel</th>
        <th>Account</th>
        <th>Promo Start</th>
        <th>Promo End</th>
        <th>Investment</th>
    </tr>
    </thead>
    <tbody>
    @foreach(@$data->budgetOver5Bio as $over5bio)
        <tr>
            <td class="text-right">{{ @$over5bio->period }}</td>
            <td>{{ @$over5bio->promoId }}</td>
            <td>{{ @$over5bio->entity }}</td>
            <td>{{ @$over5bio->distributor }}</td>
            <td>{{ @$over5bio->activity }}</td>
            <td>{{ @$over5bio->activityName }}</td>
            <td>{{ @$over5bio->channel }}</td>
            <td>{{ @$over5bio->account }}</td>
            <td class="text-right">{{ @date('d-m-Y', strtotime($over5bio->promoStart)) }}</td>
            <td class="text-right">{{ @date('d-m-Y', strtotime($over5bio->promoEnd) )}}</td>
            <td class="text-right">{{ @number_format($over5bio->investment) }}</td>
        </tr>
    @endforeach
    </tbody>
</table>

<table id="signature">
    <tr>
        <td colspan="5" style="text-align: center">Approved by,</td>
    </tr>
    @if (@$data->emailApproval)
        <tr>
            @for($i=0; $i<count($data->emailApproval); $i++)
                <td style="text-align: center; width: 20%; padding-top: 40px; padding-bottom: 40px; font-weight: bold;">{{ @($data->emailApproval[$i]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApproval[$i]->approvedOn)) : '') }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<count($data->emailApproval); $i++)
                <td style="text-align: center;">{{ @$data->emailApproval[$i]->username }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<count($data->emailApproval); $i++)
                <td style="text-align: center;">{{ @$data->emailApproval[$i]->jobtitle }}</td>
            @endfor
        </tr>
    @else
        <tr>
            @foreach(@$data->emailApproval as $signature)
                <td style="text-align: center; padding-top: 150px; width: 20%;">{{ @$signature->username }}</td>
            @endforeach
        </tr>
        <tr>
            @foreach(@$data->emailApproval as $signature)
                <td>{{ @$signature->jobtitle }}</td>
            @endforeach
        </tr>
    @endif
</table>

<div id="footer">
    Note: This document represents automatic approval and does not require handwritten signature
</div>
</body>
</html>
