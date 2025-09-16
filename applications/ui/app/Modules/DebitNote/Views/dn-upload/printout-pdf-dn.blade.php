<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>

    <title>
        {{ @$data->refId }}
    </title>

    <style type = "text/css">
        body {
            font-family: sans-serif;
            padding: 10px;
        }

        .title_doc {
            margin: auto;
            text-align: center;
        }

        table tr td {
            padding-bottom: 10px;
            vertical-align: top;
            font-size: 16px;
        }

        .header_dn {
            width: 100%;
            border-collapse: collapse;
            text-align: center;
            margin-bottom: 20px;
        }

        .header_dn tr > .header_dn_label {
            padding-right: 10px;
            text-align: right;
        }

        .header_dn tr > .header_dn_value {
            padding-top: 0px;
            text-align: left;
        }

        .header_dn tr > .header_dn_value .box {
            /*position: fixed;*/
            width: 250px;
            height: 20px;
            padding: 3px;
            border: 1px solid black;
        }

        .recipient {
            clear: left;
            width: 50%;
            /*height: 120px;*/
            /*padding: 10px;*/
            padding: 10px;
            border: 2px solid black;
        }

        .recipient > table tr .recipient_value {
            padding-left: 20px;
        }

        .content_summary table {
            border-collapse: collapse;
            width: 100%;
            /* background-image: url('{{ public_path('/img/dnn.jpg')}}'); */
            /* background-repeat: repeat-y; */
        }

        .content_summary > table tr th {
            padding: 5px;
            border: 1px solid black;
        }

        .content_summary > table tr td {
            padding: 5px;
            border: 1px solid black;
        }


        .info_rekening {
            float: left;
            width: 70%;
            border-collapse: collapse;
        }

        .ttd {
            text-align: center;
            width: 30%;
            float: right;
        }

        .footer1 {
            font-family: "Helvetica Neue Light", "HelveticaNeue-Light", "Helvetica Neue", Calibri, Helvetica, Arial, sans-serif;
            bottom: 0;
            width: 100%;
            position:fixed;
            bottom:-150px;
        }

        .footer1 > hr {
            border-top: 2px solid black;
            width: 95.5%;
            text-align: center;
            position:fixed;
            bottom:-130px;
        }

        .footer_detail {
            width: 100%;
            text-align: center;
        }

        .lampiran {
            border: 2px solid black;
            padding: 10px;
        }

        .date_info {
            margin-bottom: 20px;
            width: 100%;
        }

        .outlet_info {
            width: 40%;
            margin-bottom: 30px;
        }

        .title_lampiran {
            width: 100%;
            text-align: center;
            margin-bottom: 10px;
        }

        .entity_info {
            width: 100%;
            margin-bottom: 20px;
        }

        .content_lampiran {
            width: 100%;
            margin-bottom: 20px;
        }

        .ttd_lampiran {
            width: 100%;
            text-align: center;
            margin-top: 50px;
            margin-bottom: 30px;
        }

        .ttd_lampiran .ttd_lampiran_label {
            padding-bottom: 100px;
        }

        .note_lampiran > table {
            width: 80%;
            border: 2px solid black;
            border-collapse: collapse;
        }

        .note_lampiran > table tr td {
            border: 2px solid black;
            padding-left: 10px;
        }

        .note_lampiran .note_lampiran_label {
            /*margin-bottom: 30px;*/
            padding-bottom: 0px;
            text-align: center;
        }

        .watermark {
            position: absolute;
            top:   0%;
            left:     0%;
            z-index:  -1000;
        }
        .watermark-overbudget {
            position: absolute;
            top:   8%;
            left:     70%;
            z-index:  -1000;
        }
        .watermark-dnmanual {
            position: absolute;
            top:   8%;
            left:     65%;
            z-index:  -1000;
        }
        .watermarkb {
            position: absolute;
            top:   20%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarkc {
            position: absolute;
            top:   30%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarkd {
            position: absolute;
            top:   40%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarke {
            position: absolute;
            top:   50%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarkf {
            position: absolute;
            top:   60%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarkg {
            position: absolute;
            top:   70%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarkh {
            position: absolute;
            top:   80%;
            left:     0%;
            z-index:  -1000;
        }
        .watermarki {
            position: absolute;
            top:   90%;
            left:     0%;
            z-index:  -1000;
        }

    </style>

</head>
<body>

<img class="logo" src="{{ public_path('/assets/media/logos/dist' . @$data->distributorId .'.png') }}" style="width:auto;height:120px;">
<div class="title_doc">
    <h2>NOTA DEBIT</h2>
</div>

<table class="header_dn">
    <tr>
        <td class="header_dn_label">Nomor DN :</td>
        {{--<td>:</td>--}}
        <td class="header_dn_value" width="300px">
            <div class="box">
                {{ @$data->refId }}
            </div>
        </td>
    </tr>
    <tr>
        <td class="header_dn_label">Nomor Memorial :</td>
        {{--<td>:</td>--}}
        <td class="header_dn_value">
            <div class="box">
                {{ @$data->memDocNo }}
            </div>
        </td>
    </tr>
    <tr>
        <td class="header_dn_label">Tanggal :</td>
        {{--<td>:</td>--}}
        <td class="header_dn_value">
            <div class="box">
                {{ date('Y-m-d', strtotime(@$data->deductionDate)) }}
            </div>
        </td>
    </tr>
    <tr>
        <td class="header_dn_label">Internal Doc :</td>
        {{--<td>:</td>--}}
        <td class="header_dn_value">
            <div class="box">
                {{ @$data->intDocNo }}
            </div>
        </td>
    </tr>
</table>

<table class="recipient">
    <tr>
        <td width="20%"><strong>Kepada :</strong></td>
        <td class="recipient_value"><strong> {{ @$data->entityLongDesc }}</strong></td>
    </tr>
    <tr>
        <td></td>
        <td class="recipient_value"><strong>Up : {{ @$data->entityUp }}</strong></td>
    </tr>
    <tr>
        <td></td>
        <td class="recipient_value">{{ @$data->entityAddress }}</td>
    </tr>
</table>

<p>Kami telah mendebit perkiraan saudara sebagai berikut :</p>

<table class="entity_info">
    <tr>
        <td width="20%">Principle</td>
        <td>:</td>
        <td><strong>{{ @$data->entityLongDesc }}</strong></td>
    </tr>
    <tr>
        <td>Klaim Program</td>
        <td>:</td>
        <td><strong>{{ @$data->activityDesc }}</strong></td>
    </tr>
</table>

<div class="content_summary">
    <table>
        <tr>
            <th colspan="2">No.</th>
            <th rowspan="2" width="40%">Keterangan</th>
            <th rowspan="2" width="20%">Jumlah (Rp)</th>
        </tr>
        <tr>
            <th width="20%">Reference</th>
            <th width="20%">Promo</th>
        </tr>
        <tr>
            <td rowspan="3"></td>
            <td rowspan="3">{{ @$data->promoRefId }}</td>
            <td rowspan="3">
                {{ @$data->activityDesc }}<br>
                {{ @$data->feeDesc }}<br>
            </td>
            <td align="right" rowspan="3">
                {{ number_format(@$data->dnAmount,0,".",",") }}<br>
                @if(@$data->feeDesc<>"")
                    {{ number_format(@$data->feeAmount,0,".",",") }}<br>
                @endif
            </td>
        </tr>
        <tr>
            {{-- <td>Fix Cost :</td>
            <td align="right"></td> --}}
        </tr>
        <tr>
            {{--  <td>Var Cost :</td>
             <td align="right"></td> --}}
        </tr>
        <tr>
            <td colspan="2" rowspan="4">
                <span style="text-decoration: underline">Terbilang : </span><br><br>
                {{ @$data->terbilang }} rupiah
            </td>
            <td>DPP</td>
            <td align="right">{{ number_format(@$data->dpp,0,".",",") }}</td>
        </tr>
        <tr>
            <td>PPN ({{ number_format(@$data->ppnPct,0,".",",") }} %)</td>
            {{--  <td align="right">{{ number_format(((float)@$data->dpp * (float)@$data->ppnPct) / 100,0,".",",") }}</td>  --}}
            <td align="right">{{ number_format((float)@$data->ppnAmt,0,".",",") }}</td>
        </tr>
        <tr>
            <td>Materai</td>
            <td align="right">-</td>
        </tr>
        <tr>
            <td><strong>TOTAL</strong></td>
            {{--  <td align="right"><strong>{{ number_format( (float)@$data->dpp + ((float)@$data->dpp * (float)@$data->ppnPct) / 100,0,".",",") }}</strong></td>  --}}
            <td align="right"><strong>{{ number_format( (float)@$data->dpp + (float)@$data->ppnAmt,0,".",",") }}</strong></td>
        </tr>
    </table>
</div>

<p>Mohon ditransfer ke rekening :</p>

<table class="info_rekening">
    <tr>
        <td width="20%">Atas Nama</td>
        <td>: -</td>
    </tr>
    <tr>
        <td>Bank</td>
        <td>: {{ @$data->bankName }}</td>
    </tr>
    <tr>
        <td>Cabang</td>
        <td>: {{ @$data->bankCabang }}</td>
    </tr>
    <tr>
        <td>No. A/C</td>
        <td>: {{ @$data->noRekening }}</td>
    </tr>
</table>

<table class="ttd">
    <tr heigth="50px">
        <td height="80px" colspan="3">{{ @$data->distributorLongDesc }}</td>
    </tr>
    <tr></tr>
    <tr></tr>
    <tr>
        <td>(</td>
        <td>{{ @$data->claimManager }}</td>
        <td>)</td>
    </tr>
    <tr>
        <td colspan="3">Claim Manager</td>
    </tr>
</table>

<p style="page-break-before: always">

<div class="lampiran">
    <table class="date_info">
        <tr>
            <td>(({{ @$data->profitCenter . " - " . @$data->profitCenterDesc }})), {{ date('Y-m-d', strtotime(@$data->deductionDate)) }}</td>
        </tr>
    </table>

    <table class="outlet_info">
        <tr>
            <td>Nama Outlet</td>
            <td>:</td>
            <td>TERLAMPIR</td>
        </tr>
        <tr>
            <td>Area</td>
            <td>:</td>
            <td>{{ @$data->profitCenter . " - " . @$data->profitCenterDesc }}</td>
        </tr>
    </table>

    <table class="title_lampiran">
        <tr>
            <td><h3>PROGRAM TPR / ADD. DISCOUNT / PROMOSI</h3></td>
        </tr>
    </table>

    <table class="entity_info">
        <tr>
            <td width="20%">Principle</td>
            <td>:</td>
            <td><strong>{{ @$data->entityLongDesc }}</strong></td>
        </tr>
        <tr>
            <td>Klaim Program</td>
            <td>:</td>
            <td><strong>{{ @$data->activityDesc }}</strong></td>
        </tr>
    </table>

    <table class="content_lampiran">
        <tr>
            <td width="20%">Periode</td>
            <td>:</td>
            <td>{{ @$data->startPromo }} to {{ @$data->endPromo }}</td>
        </tr>
        <tr>
            <td>No Bon</td>
            <td>:</td>
            <td></td>
        </tr>
        <tr>
            <td>Nilai</td>
            <td>:</td>
            <td>Rp {{ number_format( (float)@$data->dpp + ((float)@$data->dpp * (float)@$data->ppnPct) / 100,0,".",",") }}</td>
        </tr>
        <tr>
            <td>Materai</td>
            <td>:</td>
            <td>Rp -</td>
        </tr>
        <tr>
            <td>Total Nilai</td>
            <td>:</td>
            <td>Rp {{ number_format( (float)@$data->dpp + ((float)@$data->dpp * (float)@$data->ppnPct) / 100,0,".",",") }}</td>
        </tr>
        <tr>
            <td>Terbilang</td>
            <td>:</td>
            <td>{{ @$data->terbilang }} rupiah</td>
        </tr>
    </table>

    <p>Demikian kami sampaikan, agar dapat segera dibayar, atas perhatian dan kerjasamanya kami ucapkan terima kasih.</p>

    <table class="ttd_lampiran">
        <tr>
            <td class="ttd_lampiran_label">Dibuat oleh,</td>
            <td class="ttd_lampiran_label">Diperiksa oleh,</td>
            <td class="ttd_lampiran_label">Diperiksa oleh,</td>
        </tr>
        <tr>
            <td><strong>Admin Claim</strong></td>
            <td><strong>AOTC</strong></td>
            <td><strong>Area Sales Supervisor</strong></td>
        </tr>
    </table>

    <table class="ttd_lampiran">
        <tr>
            <td class="ttd_lampiran_label">Disetujui oleh,</td>
            <td class="ttd_lampiran_label">Disetujui oleh,</td>
        </tr>
        <tr>
            <td><strong>Area Sales Manager</strong></td>
            <td><strong>{{ @$data->entityLongDesc }}</strong></td>
        </tr>
    </table>

    <div class="note_lampiran">
        <table align="center">
            <tr>
                <td rowspan="2">Note :</td>
                <td width="20%" class="note_lampiran_label">Reviewed</td>
                <td width="20%" class="note_lampiran_label">Reviewed</td>
            </tr>
            <tr>
                <td height="100px"></td>
                <td></td>
            </tr>
        </table>
    </div>

</div>
@if($data->isDNPromo)
    @if($data->isOverBudget)
        <img class="watermark-overbudget" src="{{ public_path('/assets/media/logos/dnoverbudget.jpeg') }}">
    @endif
    @if($data->isDNmanual && $data->isDNPromo)
        <img class="watermark-dnmanual" src="{{ public_path('/assets/media/logos/dnmanual.jpeg') }}">
    @endif
@endif
</body>
</html>
