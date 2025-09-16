<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title></title>
    <style>
        @media screen{
            @font-face {
                font-family: 'Calibri';
            }
        }
        /* img{

      width:100px;
      height:50px;


        } */
        body {
            font-family: "Calibri";
            font-size: 12px;
            font-weight: 300;
            font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;
        }
        /* table tr td { padding: 5px 10px 5px 10px; vertical-align: top; } */
        td{
            font-family: Calibri, Arial, Helvetica, sans-serif;
        }
        /* .header_id { width: 100%; margin-bottom: 10px; } */

    </style>


</head>

<body>



<table>
    <tr>
        <td style="font-weight: 600">Dear Sales Team,</td>
    </tr>
    <tr>
        <td>Please find enclosed the latest update of promo submission report in <b><i>OPTIMA</i></b> as of <?php echo date('M Y', strtotime(now()))  ?>  for your reference. For this month, promotion is counted as on-time if created at least {{ @$dataLatePromo[1]->days }} days before promo started.</td>
    </tr>
</table>
<br>
<table>
    <tr>
        <td>
            Below is the summary by Channel:
        </td>
    </tr>
</table>
{{-- TIMELINES OF OPTIMA SUBMISSION YTD OCT 2021 ['Jan','Feb', 'Mar','Apr', 'May','Jun','Jul','Aug','Sep','Oct','Nop','Dec']--}}
<table>
    <tr>
        <td display="block">
            <img style="width:600px; height:300px" width="480" height="240" src="https://quickchart.io/chart?c=%7B%0A%20%20type%3A%27line%27%2Cdata%3A%7Blabels%3A{{str_replace('"', "'",json_encode($month))   }}%2C%20datasets%3A%5B%7BborderWidth%3A1%2Clabel%3A%27%25%20Promo%20Late%20Submission%27%2C%20data%3A%20{{ str_replace('"', "'",json_encode($chartlatepct))   }}%2C%20fill%3Afalse%2CborderColor%3A%27red%27%7D%2C%7BborderWidth%3A1%2Clabel%3A%27%25%20Promo%20Submission%20before%20Promo%20Period%20Starts%27%2C%20data%3A{{str_replace('"', "'",json_encode($chartontimepct))  }}%2C%20fill%3Afalse%2CborderColor%3A%27green%27%7D%5D%7D%2Coptions%3A%20%7Bplugins%3A%20%7B%20%20%20%20%20%20%20datalabels%3A%20%7B%20%20%20%20%20%20%20%20%20display%3A%20true%2C%20%20%20%20%20%20%20%20%20align%3A%20'right'%2Cformatter%3A%20(value)%20%3D%3E%20%7Breturn%20value%20%2B%20'%2C0%25'%3B%7D%2C%20%20%20%20%20%20%20%20%20borderRadius%3A%203%20%20%20%20%20%20%20%7D%2C%20%7D%2Ctitle%3A%20%7BfontSize%3A%2012%2CfontFamily%3A%20'Calibri'%2Cdisplay%3A%20true%2Ctext%3A%20%27TIMELINESS%20OF%20OPTIMA%20SUBMISSION%20YTD%20<?php echo date('M Y', strtotime(now()))  ?>%27%7D%2Clegend%3A%20%7Bdisplay%3A%20false%2CfontSize%3A%2012%2CfontFamily%3A%20'Calibri'%2Cposition%3A%20%27bottom%27%2Calign%3A%20%27center%27%7D%2Cscales%3A%20%7ByAxes%3A%20%5B%7Bticks%3A%20%7BfontSize%3A%2012%2CbeginAtZero%3A%20true%2CfontFamily%3A%20'Calibri'%2Ccallback%3A%20(val)%20%3D%3E%20%7Breturn%20val.toLocaleString()%2B%27%2C0%20%25%27%3B%7D%7D%2C%7D%2C%5D%2CxAxes%3A%20%5B%7Bticks%3A%20%7BfontSize%3A%2012%2CfontFamily%3A%20'Calibri'%2C%7D%2C%7D%2C%5D%2C%7D%7D%0A%7D">
            <img style="width:600px; height:50px; display: inline-block" width="480" height="40" src="{{ asset('assets/media/bg/legendemail1.jpg') }}" alt="">
        </td>

    </tr>
    <tr>
        <td>

        </td>
    </tr>
</table>
<br><br>
<div>
    <table style="width: 70%;border:solid 1px #000;border-collapse: collapse;">
        <tr style="border:solid 1px #000">
            <td style="width:20%; text-align: center;border:solid 1px #000" class="cell_label"></td>
            <td style="width:8%; text-align: center;border:solid 1px #000" class="cell_label"><b>Total OPTIMA Created</b></td>
            <td style="width:10%; text-align: center;border:solid 1px #000" class="cell_label"><b># OPTIMA On Time</b></td>
            <td style="width:10%; text-align: center;border:solid 1px #000" class="cell_label"><b>% On Time Submission</b></td>
            <td style="width:12%; text-align: center;border:solid 1px #000" class="cell_label"><b># OPTIMA Late</b></td>
            <td style="width:10%; text-align: center;border:solid 1px #000" class="cell_label"><b>% Late Submission</b></td>
        </tr>

        @foreach(@$submissionlist2 as $value)

            <tr>
                <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label"><b>{{ $value->channelDesc }}</b> </td>
                <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label">{{ number_format($value->totOptimaCreated) }}</td>
                <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label">{{ number_format($value->onTime) }}</td>
                <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label">{{ number_format($value->onTimePCT,1) }} %</td>
                <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label">{{ number_format($value->late) }}</td>
                <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label">{{ number_format($value->latePCT,1) }} %</td>

            </tr>
        @endforeach

        <tr>
            <td style=" text-align: left;border:solid 1px #000;padding-left:5px;background-color:cornflowerblue" class="cell_label"><b> Total</b> </td>
            <td style=" text-align: right;border:solid 1px #000;padding-right:5px;background-color:cornflowerblue" class="cell_label"><b>{{ number_format($totaloptima)  }}</b> </td>
            <td style=" text-align: right;border:solid 1px #000;padding-right:5px;background-color:cornflowerblue" class="cell_label"><b>{{ number_format($ontime) }}</b></td>
            <td style=" text-align: right;border:solid 1px #000;padding-right:5px;background-color:cornflowerblue" class="cell_label"><b>{{  number_format($ontimepct,1) }} %</b></td>
            <td style=" text-align: right;border:solid 1px #000;padding-right:5px;background-color:cornflowerblue" class="cell_label"><b>{{ number_format($late) }}</b></td>
            <td style=" text-align: right;border:solid 1px #000;padding-right:5px;background-color:cornflowerblue" class="cell_label"><b>{{ number_format($latepct,1) }} %</b></td>


        </tr>

    </table>
</div>
<br>
<table>
    <tr>
        <td>EMAIL ATTACHMENT</td>
    </tr>
    <tr>
        <td><a href="{{ config('app.url') . '/fin-rpt/submission/export-xls-attachment/' .  $year  . '/' .  $entity  . '/' .  $distributor  . '/' .  $channel }}" target="_blank">
                Download Attachment
            </a></td>
    </tr>
    {{-- <tr>
        <td><embed src="{{ config('app.url') . public_path('assets/media/promo/abc.pdf') }}" type="application/pdf"></td>
    </tr> --}}
</table>
</body>
</html>
