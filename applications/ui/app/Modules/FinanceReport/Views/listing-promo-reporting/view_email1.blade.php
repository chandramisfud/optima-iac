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
        body {
            font-family: "Calibri";
            font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;
        }
        td{
            font-family: Calibri, Arial, Helvetica, sans-serif;
        }
        .email_intro{
            font-family: Calibri;
            font-weight: 600;
        }
        .email_body{
            font-family: Calibri;
        }

    </style>
</head>

<body>
<table>
    <tr>
        <td class="email_intro">Dear OPTIMA Initiators & Approvers, </br></br></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td class="email_body">Please find the following summary of pending Promo ID approval for your action.
        </td>
    </tr>
    <tr>
        <td class="email_body">It is mandatory to have Promo ID fully approved D-1 before promo starts.</td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td>Thank you</td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td class="email_intro">Sales Finance Team </br></br></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td class="email_intro"><br><br></td>
    </tr>
    <tr>
        <td></td>
    </tr>
</table>

<div>
    <table style="border:solid 1px #000;border-collapse: collapse; width: 1920px"">
    <thead style="background-color: #366092; color: white">
    <tr style="background-color: #366092; color: white">
        <th rowspan="3" style="text-align: center; left; border:solid 1px #000;">Channel Head</th>
        <th rowspan="3" style="text-align: center; border:solid 1px #000;">Channel</th>
        <th rowspan="3" style="text-align: center; border:solid 1px #000;">Status Group</th>
        <th rowspan="3" style="text-align: center; border:solid 1px #000;">PIC</th>
        <th rowspan="3" style="text-align: center; border:solid 1px #000;">Pending Action</th>
        <th colspan="4" style="text-align: center; border:solid 1px #000;">{{ $strMonthStart }}</th>
        <th rowspan="3" style="text-align: center; width: 100px; border:solid 1px #000;">Total Count<br/>of Promo ID</th>
        <th rowspan="3" style="text-align: center; width: 100px; border:solid 1px #000;">Total Sum<br/>of Investment</th>
    </tr>
    <tr style="background-color: #366092; color: white">
        <th colspan="2" style="text-align: center; border:solid 1px #000;">Biweekly 1<br/>(Start 1-15)</th>
        <th colspan="2" style="text-align: center; border:solid 1px #000;">Biweekly 2<br/>(Start 16-31)</th>
    </tr>
    <tr style="background-color: #366092; color: white">
        <th style="text-align: center; width: 75px; border:solid 1px #000;">Count of<br/>Promo ID</th>
        <th style="text-align: center; width: 75px; border:solid 1px #000;">Sum of<br/>Investment</th>
        <th style="text-align: center; width: 75px; border:solid 1px #000;">Count of<br/>Promo ID</th>
        <th style="text-align: center; width: 75px; border:solid 1px #000;">Sum of<br/>Investment</th>
    </tr>
    </thead>
    @php
        $channelhead = '';
        $channel = '';
    @endphp
    @foreach(@$data as $data)
        @php
            if($data->channelhead == $channelhead){
                $strChannelHead = "";
            }else{
                $strChannelHead = $data->channelhead;
            }
            if($data->channel == $channel){
                $strChannel = "";
            }else{
                $strChannel = $data->channel;
            }

        @endphp
        <tr>
            @if(str_contains($data->channel, 'Total'))
                <td style="background-color: #DCE6F1; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap"></td>
            @else
                @if(str_contains($data->channelhead, 'Total'))
                    <td style="background-color: #366092; color: white; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap"><b>{{ $strChannelHead }}</b></td>
                @else
                    <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $strChannelHead }}</td>
                @endif
            @endif
            @if(str_contains($data->channelhead, 'Total'))
                <td style="background-color: #366092; color: white; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap"><b>{{ $strChannel }}</b></td>
                <td style="background-color: #366092; color: white; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->status2 }}</td>
                <td style="background-color: #366092; color: white; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->kamfcmcem }}</td>
                <td style="background-color: #366092; color: white; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->sb_group }}</td>
            @else
                @if(str_contains($data->channel, 'Total'))
                    <td style="background-color: #DCE6F1; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap"><b>{{ $strChannel }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->status2 }}</td>
                    <td style="background-color: #DCE6F1; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->kamfcmcem }}</td>
                    <td style="background-color: #DCE6F1; text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->sb_group }}</td>
                @else
                    <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $strChannel }}</td>
                    <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->status2 }}</td>
                    <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->kamfcmcem }}</td>
                    <td style=" text-align: left;border:solid 1px #000;padding-left:5px" class="cell_label text-nowrap">{{ $data->sb_group }}</td>
                @endif
            @endif
            @if(str_contains($data->channelhead, 'Total'))
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->w11) }}</b></td>
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->inv11) }}</b></td>
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->w12) }}</b></td>
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->inv12) }}</b></td>
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->wtot) }}</b></td>
                <td style="background-color: #366092; color: white; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->invTot) }}</b></td>
            @else
                @if(str_contains($data->channel, 'Total'))
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->w11) }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->inv11) }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->w12) }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->inv12) }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->wtot) }}</b></td>
                    <td style="background-color: #DCE6F1; text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap"><b>{{ number_format($data->invTot) }}</b></td>
                @else
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->w11) }}</td>
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->inv11) }}</td>
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->w12) }}</td>
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->inv12) }}</td>
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->wtot) }}</td>
                    <td style=" text-align: right;border:solid 1px #000;padding-right:5px" class="cell_label text-nowrap">{{ number_format($data->invTot) }}</td>
                @endif
            @endif
        </tr>
        @php
            $channelhead = $data->channelhead;
            $channel = $data->channel;
        @endphp
        @endforeach
        </table>
</div>
<p>
<table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
    <tr style="margin-top:10px;"><td width="500" class="email_body">File Attachment : </td></tr>
    <tr>
        <td class="email_body">
            <a href="{{ $fileName }}" target="_blank">
                Listing Promo
            </a>
            </br></br></td>
    </tr>
</table>
</body>
</html>
