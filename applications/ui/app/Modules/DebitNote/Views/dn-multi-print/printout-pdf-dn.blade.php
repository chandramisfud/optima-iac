<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    {{--<link href="{{ asset('/assets/global/plugins/bootstrap/css/bootstrap.min.css') }}" rel="stylesheet" type="text/css" />--}}
    <title>
        DN Multi Print
    </title>

    <style>
        body {
            font-family: 'Open Sans', sans-serif;
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
        .bg-im{
            background-image: url('./');
        }
        div.page
        {
            page-break-after: always;
            page-break-inside: avoid;
        }
        .watermark-overbudget {
            background: url({{ public_path('/assets/media/logos/dnoverbudget.jpeg') }}) no-repeat;
            background-position: 70% 8%;
            position:relative;
            z-index:  -1000;
        }
        .watermark-dnmanual {
            background: url({{ public_path('/assets/media/logos/dnmanual.jpeg') }}) no-repeat;
            background-position: 65% 8%;
            position:relative;
            z-index:  -1000;
        }
        .main_container{
            /* background: url({{ public_path('/assets/media/logos/dnoverbudget.jpeg') }}) no-repeat;
            background-position: 75% 8%;  */
            /* padding-top:15px;
            padding-left:5px;
            z-index: 99; */
            /* background-size: cover; */
            /* width:652px;
            height:975px; */
            /* position:relative; */
        }
        .break{
            display: block;
            clear: both;
            page-break-after: always;
        }
    </style>
</head>
<body>
@for ($i=0;$i<count($arrData);$i++)
    @php $watermark = "";
    if(json_decode($arrData[$i])->isDNPromo){
        if(json_decode($arrData[$i])->isOverBudget){
            $watermark = 'dnoverbudget';
        }else{
            if(json_decode($arrData[$i])->isDNmanual){
                $watermark = 'dnmanual';
            }
        }
    }
    if($watermark==""){
        echo "<div class='main_container break' style='position:relative'>";
    }elseif($watermark=="dnoverbudget"){
        echo "<div class='watermark-overbudget break' style='position:relative'>";
    }else{
        echo "<div class='watermark-dnmanual break' style='position:relative'>";
    }
    @endphp

        <img class="logo" src="{{ public_path('/assets/media/logos/dist' . json_decode($arrData[$i])->distributorId .'.png') }}" style="width:auto;height:120px;">
        {{-- <img class="logo" src="{{ public_path('/assets/media/logos/TRS-Logo.jpeg') }}" style="width:120px;height:120px;"> --}}

        <div class="title_doc">
            <h2>NOTA DEBIT</h2>
        </div>

        <table class="header_dn">
            <tr>
                <td class="header_dn_label">Nomor DN :</td>
                {{--<td>:</td>--}}
                <td class="header_dn_value" width="300px">
                    <div class="box">
                        {{ @json_decode($arrData[$i])->refId }}
                    </div>
                </td>
            </tr>
            <tr>
                <td class="header_dn_label">Nomor Memorial :</td>
                {{--<td>:</td>--}}
                <td class="header_dn_value">
                    <div class="box">
                        {{ @json_decode($arrData[$i])->memDocNo }}
                    </div>
                </td>
            </tr>
            <tr>
                <td class="header_dn_label">Tanggal :</td>
                {{--<td>:</td>--}}
                <td class="header_dn_value">
                    <div class="box">
                        {{ date('Y-m-d', strtotime(@json_decode($arrData[$i])->deductionDate)) }}
                    </div>
                </td>
            </tr>
            <tr>
                <td class="header_dn_label">Internal Doc :</td>
                {{--<td>:</td>--}}
                <td class="header_dn_value">
                    <div class="box">
                        {{ @json_decode($arrData[$i])->intDocNo }}
                    </div>
                </td>
            </tr>
        </table>

        <table class="recipient">
            <tr>
                <td width="20%"><strong>Kepada :</strong></td>
                <td class="recipient_value"><strong> {{ @json_decode($arrData[$i])->entityLongDesc }}</strong></td>
            </tr>
            <tr>
                <td></td>
                <td class="recipient_value"><strong>Up : {{ @json_decode($arrData[$i])->entityUp }}</strong></td>
            </tr>
            <tr>
                <td></td>
                <td class="recipient_value">{{ @json_decode($arrData[$i])->entityAddress }}</td>
            </tr>
        </table>

        <p>Kami telah mendebit perkiraan saudara sebagai berikut :</p>

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
                    <td rowspan="3">{{ @json_decode($arrData[$i])->promoRefId }}</td>
                    <td rowspan="3">
                        {{ @json_decode($arrData[$i])->activityDesc }}<br>
                        {{ @json_decode($arrData[$i])->feeDesc }}<br>
                    </td>
                    <td align="right" rowspan="3">
                        {{ number_format(@json_decode($arrData[$i])->dnAmount,0,".",",") }}<br>
                        @if(@json_decode($arrData[$i])->feeDesc<>"")
                        {{ number_format(@json_decode($arrData[$i])->feeAmount,0,".",",") }}<br>
                        @endif
                    </td>
                </tr>
                <tr>
                    {{-- <td>Fix Cost :</td>
                    <td align="right"></td> --}}
                </tr>
                <tr>
                    {{-- <td>Var Cost :</td>
                    <td align="right"></td> --}}
                </tr>
                <tr>
                    <td colspan="2" rowspan="4">
                        <span style="text-decoration: underline">Terbilang : </span><br><br>
                        {{ @json_decode($arrData[$i])->terbilang }} rupiah
                    </td>
                    <td>DPP</td>
                    <td align="right">{{ number_format(@json_decode($arrData[$i])->dpp,0,".",",") }}</td>
                </tr>
                <tr>
                    <td>PPN ({{ number_format(@json_decode($arrData[$i])->ppnPct,0,".",",") }} %)</td>
                    <td align="right">{{ number_format(((float)@json_decode($arrData[$i])->dpp * (float)@json_decode($arrData[$i])->ppnPct) / 100,0,".",",") }}</td>
                </tr>
                <tr>
                    <td>Materai</td>
                    <td align="right">-</td>
                </tr>
                <tr>
                    <td><strong>TOTAL</strong></td>
                    <td align="right"><strong>{{ number_format( (float)@json_decode($arrData[$i])->dpp + ((float)@json_decode($arrData[$i])->dpp * (float)@json_decode($arrData[$i])->ppnPct) / 100,0,".",",") }}</strong></td>
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
                <td>: {{ @json_decode($arrData[$i])->bankName }}</td>
            </tr>
            <tr>
                <td>Cabang</td>
                <td>: {{ @json_decode($arrData[$i])->bankCabang }}</td>
            </tr>
            <tr>
                <td>No. A/C</td>
                <td>: {{ @json_decode($arrData[$i])->noRekening }}</td>
            </tr>
        </table>

        <table class="ttd">
            <tr heigth="50px">
                <td height="80px" colspan="3">{{ @json_decode($arrData[$i])->distributorLongDesc }}</td>
            </tr>
            <tr></tr>
            <tr></tr>
            <tr>
                <td>(</td>
                <td>{{ @json_decode($arrData[$i])->claimManager }}</td>
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
                    <td>(({{ @json_decode($arrData[$i])->profitCenter . " - " . @json_decode($arrData[$i])->profitCenterDesc }})), {{ date('Y-m-d', strtotime(@json_decode($arrData[$i])->deductionDate)) }}</td>
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
                    <td>{{ @json_decode($arrData[$i])->profitCenter . " - " . @json_decode($arrData[$i])->profitCenterDesc }}</td>
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
                    <td><strong>{{ @json_decode($arrData[$i])->entityLongDesc }}</strong></td>
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
                    <td>{{ @json_decode($arrData[$i])->startPromo }} to {{ @json_decode($arrData[$i])->endPromo }}</td>
                </tr>
                <tr>
                    <td>No Bon</td>
                    <td>:</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Nilai</td>
                    <td>:</td>
                    <td>Rp {{ number_format( (float)@json_decode($arrData[$i])->dpp + ((float)@json_decode($arrData[$i])->dpp * (float)@json_decode($arrData[$i])->ppnPct) / 100,0,".",",") }}</td>
                </tr>
                <tr>
                    <td>Materai</td>
                    <td>:</td>
                    <td>Rp -</td>
                </tr>
                <tr>
                    <td>Total Nilai</td>
                    <td>:</td>
                    <td>Rp {{ number_format( (float)@json_decode($arrData[$i])->dpp + ((float)@json_decode($arrData[$i])->dpp * (float)@json_decode($arrData[$i])->ppnPct) / 100,0,".",",") }}</td>
                </tr>
                <tr>
                    <td>Terbilang</td>
                    <td>:</td>
                    <td>{{ @json_decode($arrData[$i])->terbilang }} rupiah</td>
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
                    <td><strong>{{ @json_decode($arrData[$i])->entityLongDesc }}</strong></td>
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
        {{-- @if(@json_decode($arrData[$i])isDNPromo)
            @if(@json_decode($arrData[$i])isOverBudget)
                <img class="watermark-overbudget" src="{{ public_path('/assets/media/logos/dnoverbudget.jpeg') }}">
            @endif
            @if(@json_decode($arrData[$i])isDNmanual)
                <img class="watermark-dnmanual" src="{{ public_path('/assets/media/logos/dnmanual.jpeg') }}">
            @endif
        @endif --}}
    </div>
@endfor

</body>
</html>
