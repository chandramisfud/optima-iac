<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>
        {{ $data->refId }}
    </title>

    <style>
        body {
            font-family: Calibri;
            padding-top: 3%;
        }
    </style>
</head>
<body>

<table style="width:100%; padding-left:20px;padding-right:20px">
    <tr>
        <td style="text-align:center"><img class="logo" src="{{ public_path('/assets/media/logos/skp_logos.jpg') }}" style="width:150px;height:70px;"></td>
        <td style="text-align:center;font-weight:600">DANONE SPECIALIZED NUTRITION (SN) INDONESIA</td>
        <td style="text-align:center">
            <img class="logo" src="{{ public_path('/assets/media/logos/entity/' . $data->entityId .'.png') }}" style="width:150px;height:80px;"></td>
    </tr>
</table>
<table style="width:100%; padding-left:20px;padding-right:20px">
    <tr>
        <td style="border-top:1px solid #000;border-bottom:1px solid #000; text-align: center;font-size: 12px;font-weight:600">
            SURAT KERJASAMA PROMOSI (SKP)
        </td>
    </tr>
    <tr>
        <td style="border-bottom:2px solid #000"></td>
    </tr>
    <tr>
        <td><br></td>
    </tr>
</table>
<table style="width:100%; padding-left:20px;padding-right:20px">
    <tr style="font-size: 12px;font-weight:600">
        <td style="width:20%">Channel / Sub Channel</td>
        <td style="width:1%">:</td>
        <td style="width:79%">{{ $data->channel }} / {{ $data->subChannel }}</td>
    </tr>
    <tr style="font-size: 12px;font-weight:600">
        <td>Account / Sub Account</td>
        <td>:</td>
        <td>{{ $data->account }} / {{ $data->subAccount }}</td>
    </tr>
    <tr style="font-size: 12px;font-weight:600">
        <td>Promo ID</td>
        <td>:</td>
        <td>{{ $data->refId }}</td>
    </tr>
    <tr>
        <td><br></td>
    </tr>
    <tr style="font-size: 12px">
        <td colspan="3">Dengan ini, saya yang bertanda tangan di bawah ini:</br></br></td>
    </tr>
    <tr style="font-size: 12px;">
        <td> Nama</td>
        <td>:</td>
        <td>{{ $data->initiatorName }}</td>
    </tr>
    <tr style="font-size: 12px">
        <td>Jabatan</td>
        <td>:</td>
        <td>{{ $data->jobTitle }}</td>
    </tr>
    <tr style="font-size: 12px">
        <td>Alamat Email</td>
        <td>:</td>
        <td>{{ $data->email }}</td>
    </tr>
    <tr style="font-size: 12px">
        <td>No. Handphone</td>
        <td>:</td>
        <td>{{ $data->contactInfo }}</td>

    </tr>
    <tr>
        <td><br></td>
    </tr>
    <tr style="font-size: 12px">
        <td colspan="3">Yang diberi kuasa bertindak untuk dan atas nama:</br></br></td>
    </tr>
    <tr style="font-size: 12px; font-weight:600">
        <td>Nama Perusahaan</td>
        <td>:</td>
        <td>{{ $data->entity }}</td>
    </tr>
    <tr style="font-size: 12px">
        <td>No. Telp</td>
        <td>:</td>
        <td>021-29961234</td>

    </tr>
    <tr>
        <td><br></td>
    </tr>
    <tr style="font-size: 12px">
        <td colspan="3">Menyatakan bahwa saya sepakat untuk berpartisipasi dalam kerjasama promosi sebagai berikut:</br></br></td>
    </tr>
    <tr style="font-size: 12px">
        <td>Periode Promosi</td>
        <td>:</td>
        <td>{{ date('d F Y', strtotime($data->startPromo)) }} - {{ date('d F Y', strtotime($data->endPromo)) }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Bentuk Promosi</td>
        <td>:</td>
        <td>{{ $data->subActivity }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Mekanisme Promosi</td>
        <td>:</td>
        <td>{{ $data->mechanisme1 }}</td>

    </tr>
    <tr style="font-size: 12px; vertical-align:top" >
        <td>Brand</td>
        <td>:</td>
        <td>{{ @str_replace("amp;", "", $data->brandDesc)}}</td>

    </tr>
    <tr style="font-size: 12px; vertical-align:top">
        <td>SKU</td>
        <td>:</td>
        <td>{{ $data->skuDesc }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Distributor</td>
        <td>:</td>
        <td>{{ $data->distributor }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Investment</td>
        <td>:</td>
        <td>{{ @number_format($data->investment,0,".",",") }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Baseline sales</td>
        <td>:</td>
        <td></td>
    <!-- {{ @number_format($data->normalSales,2,".",",") }} -->
    </tr>
    <tr style="font-size: 12px">
        <td>Incremental sales</td>
        <td>:</td>
        <td></td>
    <!-- {{ @number_format($data->incrSales,2,".",",") }} -->
    </tr>
    <tr style="font-size: 12px">
        <td>Sales Target</td>
        <td>:</td>
        <td>{{ @number_format($data->totalSales,0,".",",") }}</td>

    </tr>
    <tr style="font-size: 12px">
        <td>Cost Ratio</td>
        <td>:</td>
        <td>{{ $data->costRatio }}%</td>

    </tr>
    <tr style="font-size: 12px">
        <td>TS Code</td>
        <td>:</td>
        <td>{{ $data->tsCoding }}</td>

    </tr>
    <tr>
        <td><br></td>
    </tr>

</table>

<table style="width:100%; padding-left:20px;padding-right:20px">
    <tr>
        <td style="font-size:12px">{{ date('d F Y')}}</td>
    </tr>
    <tr>
        <td style="font-size:12px;font-weight:600">Yang menyetujui,</td>
    </tr>
    <tr>
        <td><br><br><br><br><br><br></td>
    </tr>
    <tr>
        <td style="font-size:12px">(………………………………................)</td>
        <td style="width:30%px"></td>
        <td style="font-size:12px">(………………………………............)</td>
    </tr>
    <tr>
        <td style="font-size:12px;font-weight:600">Key Acccount Manager Danone</td>
        <td style="width:30%px"></td>
        <td style="font-size:12px;font-weight:600">Retailer's Buyer / Distributor*</td>
    </tr>
    <tr>
        <td><br><br></td>
    </tr>
    <tr>
        <td style="font-size:12px;font-weight:600">CATATAN :</td>
    </tr>
    <tr>
        <td style="font-size:12px">Surat kerjasama promosi ini bukan merupakan bukti pembayaran.</td>
    </tr>
    <tr>
        <td style="font-size:12px">* Distributor sign hanya pada channel GT, MTI, MBS, Midwives, Pharma Reguler</td>
    </tr>
</table>



</body>
</html>
