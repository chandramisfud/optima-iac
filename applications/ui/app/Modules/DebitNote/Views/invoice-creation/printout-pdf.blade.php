<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    {{--<link href="{{ asset('/assets/global/plugins/bootstrap/css/bootstrap.min.css') }}" rel="stylesheet" type="text/css" />--}}
    {{-- <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Calibri"> --}}
    {{-- <link href='http://fonts.googleapis.com/css?family=Calibri' rel='stylesheet' type='text/css'> --}}

    <title>
        Invoice No. {{ $data->refId }}
    </title>

    <style>
        @font-face {font-family: "Calibri"; src: url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.eot"); src: url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.eot?#iefix") format("embedded-opentype"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.woff2") format("woff2"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.woff") format("woff"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.ttf") format("truetype"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.svg#Calibri") format("svg"); }
        /* @font-face { */
        /* font-family: "Calibri";  */
        /* src: url('{{ public_path('fonts/Calibri-Font/Calibri.ttf') }}'); */
        /* src: url("https://fonts.googleapis.com/css?family=Calibri");  */
        /* src: url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.eot");  */
        /* src: url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.eot?#iefix")
            format("embedded-opentype"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.woff2")
            format("woff2"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.woff")
            format("woff"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.ttf")
            format("truetype"), url("//db.onlinewebfonts.com/t/a78cfad3beb089a6ce86d4e280fa270b.svg#Calibri") format("svg");  */
        /* font-size: 8pt; */
        /* font-weight: 400; */
        /* }  */
        body {
            /* font-family: "Calibri"; */
            /* font-size: 8pt; */
            padding-top: 5%;
        }

        .company {
            float: right;
            position: static;
        }

        .invoice_no {
            border: 1px solid black;
            width: 40%;
            margin-right: 0px;
            margin-left: auto;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        .invoice_no tr th {
            text-align: center;
            border: 1px solid black;
            padding: 5px;
        }

        .invoice_no tr td {
            padding: 5px;
        }

        .company_address {
            width: 40%;
            margin-bottom: 30px;
        }

        .recipient {
            /* border: 1px solid black; */
            width: 40%;
            margin-bottom: 10px;
            text-align: left;
        }

        .recipient th {
            padding-bottom: 5px;
        }

        .recipient  .recipient_label {
            width: 5%;
            padding-right: 10px;
        }

        .detail_invoice {
            border: 1px solid black;
            border-collapse: collapse;
            width: 100%;
            height: 30%;
            margin-bottom: 10px;
        }

        .detail_invoice tr th {
            padding: 5px;
            text-align: center;
            border: 1px solid black;
            border-collapse: collapse;
        }

        .detail_invoice tr td {
            /* border: 1px solid black; */
            border-collapse: collapse;
            padding: 20px;
            vertical-align: text-top;
        }

        .detail_invoice .detail_invoice_ket {
            width: 55%;
        }

        .detail_invoice .detail_invoice_jumlah {
            width: 35%;
        }

        .terbilang {
            width: 65%;
            margin-bottom: 10px;
        }

        .ttd {
            width: 30%;
            margin-right: 0px;
            margin-left: auto;
            text-align: center;
        }

        .lampiran {
            padding-top: 10%;
        }

        .lampiran table {
            border: 1px solid black;
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 30px;
        }

        .lampiran table tr th {
            font-size: 1.5rem;
            padding: 10px 0 10px 0;
        }

        .lampiran table tr > .title_header_lampiran {
            padding: 5px 5px 5px 5px;
            border: 1px solid black;
            font-weight: bold;
        }

        .lampiran table tr > td {
            padding: 5px 5px 5px 5px;
            border: 1px solid black;
        }

        .lampiran table tr > .total_lampiran {
            padding: 5px 5px 5px 5px;
            border: 1px solid black;
            font-weight: bold;
        }

        .ttd_lampiran {
            width: 100%;
            text-align: center;
        }

        /* .page {
            overflow: hidden;
            page-break-after: always;
        } */

        .tree {
            page-break-inside: avoid;
        }
        /* .whatever { page-break-after: always; } */
    </style>
</head>
@php
    $strPPN             ="PPN = " . $data->ppnpct . "% x Dasar Pengenaan Pajak";
@endphp
<body>
{{-- <img class="logo" src="{{ public_path('/assets/media/logos/Logo.png') }}" style="width:150px;height:150px;"> --}}
<img class="logo" src="{{ public_path('/assets/media/logos/dist' . $data->distributorId .'.png') }}" style="width:auto;height:120px;">

<div class="company">
    <h1>{{ strtoupper(@$data->distributorDesc) }}</h1>
</div>

<table class="invoice_no">
    <tr>
        <th colspan="3">INVOICE</th>
    </tr>
    <tr>
        <td>No</td>
        <td>:</td>
        <td>{{ @$data->refId }}</td>
    </tr>
    <tr>
        <td>Tanggal</td>
        <td>:</td>
        <td>{{ @$data->invoiceDate }}</td>
    </tr>
</table>

<table class="company_address">
    <tr>
        <td colspan="3">{{ strtoupper(@$data->distributorDesc) }}</td>
    </tr>
    <tr>
        <td colspan="3">{{ @$data->distributorAddress }}</td>
    </tr>
    <tr>
        <td width="25%">Phone</td>
        <td>:</td>
        <td>{{ @$data->distributorPhone }}</td>
    </tr>
    <tr>
        <td>Fax</td>
        <td width="2%">:</td>
        <td>{{ @$data->distributorFax }}</td>
    </tr>
    <tr>
        <td>NPWP</td>
        <td>:</td>
        <td>{{ @$data->distributorNPWP }}</td>
    </tr>
</table>

<table class="recipient">
    <tr>
        <th colspan="3">PEMBELI BKP / PENERIMA JKP</th>
    </tr>
    <tr>
        <td class="recipient_label">Nama</td>
        <td>:</td>
        <td>{{ @$data->entityDesc }}</td>
    </tr>
    <tr>
        <td class="recipient_label">Alamat</td>
        <td>:</td>
        <td>{{ @$data->entityAddress }}</td>
    </tr>
    <tr>
        <td class="recipient_label">NPWP</td>
        <td>:</td>
        <td>{{ @$data->entityNPWP }}</td>
    </tr>
    <tr>
        <td class="recipient_label">Up</td>
        <td>:</td>
        <td>{{ @$data->entityUp }}</td>
    </tr>
</table>

<table class="detail_invoice">
    <tr>
        <th width="10%">NO</th>
        <th class="detail_invoice_ket" style="line-height: 8px; height:8px">KETERANGAN</th>
        <th colspan="2" class="detail_invoice_jumlah">JUMLAH</th>
    </tr>
    {{-- @foreach (@$data->detailDN as $dtlDn)
    <tr>
        <td style="border-right: 1px solid black">{{ @$dtlDn->no }}</td>
        <td style="border-right: 1px solid black">{{ @$dtlDn->activityDesc . " - " . @$dtlDn->dnNumber }}</td>
        <td align="left" width="5%" style="border-right: none">Rp</td>
        <td align="right">{{ number_format(@$dtlDn->totalClaim, 0, ".", ",") }}</td>
    </tr>
    @endforeach --}}
    <tr>
        <td style="border-right: 1px solid black"></td>
        <td style="border-right: 1px solid black">{{ @$data->invoiceDesc }}</td>
        <td align="left" width="5%" style="border-right: none">Rp</td>
        <td align="right">{{ number_format(@$data->dpp, 0, ".", ",") }}</td>
    </tr>
    <tr>
        <td style="border: 1px solid black; padding: 10px; border-bottom: 1px solid black; line-height: 8px; height:8px" colspan="2">Dasar Pengenaan Pajak</td>
        <td align="left" width="5%" style="border-right: none; border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px;line-height: 8px; height:8px">Rp</td>
        <td align="right" style="border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px;line-height: 8px; height:8px">{{ number_format(@$data->dpp, 0, ".", ",") }}</td>
    </tr>
    <tr>
        <td style="border: 1px solid black; padding: 10px; border-bottom: 1px solid black; line-height: 8px; height:8px" colspan="2">{{ @$data->ppnLabel }}</td>
        <td align="left" width="5%" style="border-right: none; border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px">Rp</td>
        <td align="right" style="border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px">{{ number_format(@$data->ppn, 0, ".", ",") }}</td>
    </tr>
    @if (count($data->detailDN) === 1)
    <tr>
        <td style="border: 1px solid black; padding: 10px; border-bottom: 1px solid black; line-height: 8px; height:8px" colspan="2">{{ @$data->pphLabel }}</td>
        <td align="left" width="5%" style="border-right: none; border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px">Rp</td>
        <td align="right" style="border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px">{{ number_format(@$data->pph, 0, ".", ",") }}</td>
    </tr>
    @endif
    <tr>
        <td style="border: 1px solid black; padding: 10px; border-bottom: 1px solid black; line-height: 8px; height:8px" colspan="2"></td>
        <td align="left" width="5%" style="border-right: none; border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px"></td>
        <td align="right" style="border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px"></td>
    </tr>
    <tr>
        <td style="border: 1px solid black; padding: 10px; border-bottom: 1px solid black; line-height: 8px; height:8px" colspan="2">Jumlah</td>
        <td align="left" width="5%" style="border-right: none; border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px">Rp</td>
        <td align="right" style="border-top: 1px solid black; border-bottom: 1px solid black; padding: 10px; line-height: 8px; height:8px"><strong>{{ number_format(@$data->invoiceAmount, 0, ".", ",") }}</strong></td>
    </tr>
</table>

<table class="terbilang">
    <tr>
        <td style="width: 18%; vertical-align: top;">Terbilang :</td>
        <td style="width: 82%; vertical-align: top;">{{ @$data->terbilang }} rupiah</td>
    </tr>
</table>

<table class="ttd">
    <tr>
        <td style="padding-bottom:60px;">{{ @$data->distributorDesc }}</td>
    </tr>
    <tr>
        <td>{{ @$data->claimManager }}</td>
    </tr>
    <tr>
        <td><hr width="90%" style="border-top: 1px solid black;"></td>
    </tr>
    <tr>
        <td>Claim Manager</td>
    </tr>
</table>

<p style="page-break-before: always">

<div class="lampiran">
    {{-- <div style="page-break-after:always;"> --}}
    <table>
        <tr>
            <th colspan="5">Monitoring Debit Note</th>
        </tr>
        <tr>
            <td class="title_header_lampiran" widht="5px">No.</td>
            <td class="title_header_lampiran">TPP Number</td>
            <td class="title_header_lampiran">DN Number</td>
            <td class="title_header_lampiran">Description</td>
            <td class="title_header_lampiran" align="right">DN Amount</td>
        </tr>
        @php
            $ctr = 1;
            $total=0;
        @endphp
        @foreach (@$data->detailDN as $item)
            <tr>
                <td widht="5%">{{ $ctr }}</td>
                <td widht="20%">{{ $item->promoNumber }}</td>
                <td widht="20%">{{ $item->dnNumber }}</td>
                <td widht="30%">{{ $item->activityDesc }}</td>
                <td widht="25%" align="right">{{ number_format(@$item->totalClaim, 0, ".", ",") }}</td>
            </tr>
            @php
                $total = $total + $item->totalClaim;
                $ctr++;
            @endphp
            @if($loop->iteration == 17)
    </table> <!-- Table closed -->
</div>
<div style="page-break-after: always"></div> <!-- Page break -->
<div class="lampiran">
    <table>
        <tr>
            <th colspan="5">Monitoring Debit Note</th>
        </tr>
        <tr>
            <td class="title_header_lampiran" widht="5px">No.</td>
            <td class="title_header_lampiran">TPP Number</td>
            <td class="title_header_lampiran">DN Number</td>
            <td class="title_header_lampiran">Description</td>
            <td class="title_header_lampiran" align="right">DN Amount</td>
        </tr>
        @endif
        @endforeach
        <tr>
            <td colspan="4" class="total_lampiran">Total</td>
            <td align="right" class="total_lampiran">{{ number_format($total, 0, ".", ",") }}</td>
        </tr>
    </table>

    <p style="margin-bottom:30px">Jakarta, {{ date('d F Y', strtotime(@$data->invoiceDate)) }}</p>

</div>

<table class="ttd_lampiran">
    <tr>
        <td colspan="3" style="padding-bottom:80px;">Checked</td>
        <td colspan="3" style="padding-bottom:80px;">Verified</td>
        <td colspan="3" style="padding-bottom:80px;">Approved</td>
    </tr>
    <tr>
        <td>(</td>
        <td></td>
        <td>)</td>
        <td>(</td>
        <td></td>
        <td>)</td>
        <td>(</td>
        <td></td>
        <td>)</td>
    </tr>
</table>

</body>
</html>
