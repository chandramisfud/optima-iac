<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    {{--<link href="{{ asset('/assets/global/plugins/bootstrap/css/bootstrap.min.css') }}" rel="stylesheet" type="text/css" />--}}
    <title>
        Surat Jalan {{ @$data->refId }}
    </title>

    <style>

        .content {
            font-family: Sans-serif;
            font-size: 8pt;
            width: 100%;
            border: 2px solid darkgray;
            border-collapse: collapse;
        }

        .content tr th {
            border: 2px solid darkgray;
            padding: 5px;
            background-color: #C0C0C0;
        }

        .content tr td {
            border: 2px solid darkgray;
            padding: 5px;
        }

        .ttd {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12pt;
            width: 100%;
            text-align: center;
        }


    </style>
</head>
<body>

<table class="content">
    <tr>
        <th>No</th>
        <th>No Promo</th>
        <th>No DN</th>
        <th>Memorial Doc No</th>
        <th>Account</th>
        <th>Activity</th>
        <th>Total Klaim</th>
    </tr>
    @php
        $no = 1;
        $total = 0;
    @endphp
    @foreach (@$data->dnId as $item)
        <tr>
            <td  width="3%" align="center">{{ $no }}</td>
            <td width="17%">{{ $item->promoNumber }}</td>
            <td width="20%">{{ $item->dnNumber }}</td>
            <td width="12%">{{ $item->memDocNo }}</td>
            <td width="15%">{{ $item->accountDesc }}</td>
            <td width="20%">{{ $item->activityDesc }}</td>
            <td  width="13%" align="right">{{ number_format($item->totalClaim, 0, ".", ",") }}</td>
        </tr>
        @php
            $total = (float)$total + (float)$item->totalClaim;
            $no++;
        @endphp
    @endforeach
    <tr>
        <td colspan="5"></td>
        <td><strong>Total</strong></td>
        <td align="right"><strong>{{ number_format($total, 0, ".", ",") }}</strong></td>
    </tr>
</table>

<table class="ttd">
    <tr>
        <td style="padding: 70px;">Pengirim</td>
        <td style="padding: 70px;">Penerima</td>
    </tr>
    <tr>
        <td><hr width="30%%" style="border-top: 1px solid black;"></td>
        <td><hr width="30%" style="border-top: 1px solid black;"></td>
    </tr>
</table>
</body>
</html>
