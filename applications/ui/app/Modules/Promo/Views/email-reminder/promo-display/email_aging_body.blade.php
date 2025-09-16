<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Promo Display</title>
    <style>
    @media screen{
        @font-face {
    font-family: 'Calibri';
        }
    }
        body {
            font-family: "Calibri";
            font-size: 10px;
            font-weight: 300;
            font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;
        }
        /* table tr td { padding: 5px 10px 5px 10px; vertical-align: top; } */
        td{
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 12px;
        }
        .email_intro{
            font-family: Calibri;
            font-size: 14px;
            font-weight: 600;
        }
        .email_body{
            font-family: Calibri;
            font-size: 14px;
            ;
        }



        .summary table { width: 100%; border: 1px solid #DDDDDD; border-collapse: collapse; float: left; margin-bottom: 20px; }
        .summary table tr { border: 1px solid #DDDDDD; }
        .summary table tr td { border: 1px solid #DDDDDD;}
        .summary_title{text-align: center; font-weight: 600}
        .summary_no{ width: 4%; text-align: center}
        .summary_category{ width: 8%; text-align: left}
        .summary_user{ width: 8%; text-align: left}
        .summary_pic{ width: 8%; text-align: left}
        .summary_qty{ width: 4%; text-align: right}
        .summary_value{ width: 13%; text-align: right}
        /* .header_id { width: 100%; margin-bottom: 10px; } */

    </style>
</head>
<body>
<table>
<!-- <tr>
    <td class="email_intro">EMAIL TO</br></br></td>
    <td>{{@$data2->email_to}}</td>
</tr> -->
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
    <td class="email_body">Please find the following summary of pending promo ID approval and sendback for your action.
</td>
</tr>
<tr>
    <td class="email_body">Do not hesitate to contact us if you have any questions.</td>
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
    <td class="email_intro">Table of summary </br></br></td>
</tr>
<tr>
    <td></td>
</tr>
</table>
<div class="summary">
<table>
<tr class="summary_title">
    <td style="background-color: #002060; color: #ffffff" rowspan="3">No</td>
    <td style="background-color: #002060; color: #ffffff" rowspan="3">Category</td>
    <td style="background-color: #002060; color: #ffffff" rowspan="3">User ID</td>
    <td style="background-color: #002060; color: #ffffff; text-align: center" rowspan="3">PIC</td>
    <td colspan="6" style="background-color: #002060; color: #ffffff">Aging</td>
    <td rowspan="2" style="background-color: #002060; color: #ffffff" colspan="2">Total</td>
</tr>
<tr class="summary_title">
    <td colspan="2" style="background-color: #002060; color: #ffffff">1-5 days</td>
    <td colspan="2" style="background-color: #002060; color: #ffffff">6-10 days</td>
    <td colspan="2" style="background-color: #002060; color: #ffffff">more than 10 days</td>
</tr>
<tr class="summary_title">
    <td style="background-color: #002060; color: #ffffff">Qty</td>
    <td style="background-color: #002060; color: #ffffff">Value</td>
    <td style="background-color: #002060; color: #ffffff">Qty</td>
    <td style="background-color: #002060; color: #ffffff">Value</td>
    <td style="background-color: #002060; color: #ffffff">Qty</td>
    <td style="background-color: #002060; color: #ffffff">Value</td>
    <td style="background-color: #002060; color: #ffffff">Qty</td>
    <td style="background-color: #002060; color: #ffffff">Value</td>
</tr>

@foreach(@$data as $data_aging)

<tr>
@if($data_aging->num == "Sub Total Approval")
    <td class="summary_pc" colspan="4" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->num}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty15}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val15}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty610}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val610}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty10}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val10}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qtytot}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->valtot}}</td>
@elseif($data_aging->num == "Sub Total Sendback")
    <td class="summary_pc" colspan="4" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->num}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty15}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val15}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty610}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val610}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qty10}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->val10}}</td>
    <td class="summary_qty" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->qtytot}}</td>
    <td class="summary_value" style="background-color:#DCE6F1; font-weight: 600">{{$data_aging->valtot}}</td>
@elseif($data_aging->num == "Grand Total")
    <td class="summary_pc" colspan="4" style="background-color:#538DD5; font-weight: 600">{{$data_aging->num}}</td>
    <td class="summary_qty" style="background-color:#538DD5; font-weight: 600">{{$data_aging->qty15}}</td>
    <td class="summary_value" style="background-color:#538DD5; font-weight: 600">{{$data_aging->val15}}</td>
    <td class="summary_qty" style="background-color:#538DD5; font-weight: 600">{{$data_aging->qty610}}</td>
    <td class="summary_value" style="background-color:#538DD5; font-weight: 600">{{$data_aging->val610}}</td>
    <td class="summary_qty" style="background-color:#538DD5; font-weight: 600">{{$data_aging->qty10}}</td>
    <td class="summary_value" style="background-color:#538DD5; font-weight: 600">{{$data_aging->val10}}</td>
    <td class="summary_qty" style="background-color:#538DD5; font-weight: 600">{{$data_aging->qtytot}}</td>
    <td class="summary_value" style="background-color:#538DD5; font-weight: 600">{{$data_aging->valtot}}</td>
@else
    <td class="summary_no">{{$data_aging->num}}</td>
    <td class="summary_category">{{$data_aging->category}}</td>
    <td class="summary_user" >{{$data_aging->userID}}</td>
    <td class="summary_pc">{{$data_aging->pic}}</td>
    <td class="summary_qty">{{$data_aging->qty15}}</td>
    <td class="summary_value">{{$data_aging->val15}}</td>
    <td class="summary_qty">{{$data_aging->qty610}}</td>
    <td class="summary_value">{{$data_aging->val610}}</td>
    <td class="summary_qty">{{$data_aging->qty10}}</td>
    <td class="summary_value">{{$data_aging->val10}}</td>
    <td class="summary_qty">{{$data_aging->qtytot}}</td>
    <td class="summary_value">{{$data_aging->valtot}}</td>
@endif

</tr>
@endforeach
</table>
</div>
<table>
    <tr>
        <td>EMAIL ATTACHMENT</td>
    </tr>
    <tr>
        {{-- <td><a href="{{ config('app.url') . '/reminder/emailaging/downloadattach' }}" target="_blank"> --}}
        <td><a href="{{ config('app.url') . @$fileAttachment }}" target="_blank">
                Download Attachment
        </a></td>
    </tr>
    {{-- <tr>
        <td><embed src="{{ config('app.url') . public_path('assets/media/promo/abc.pdf') }}" type="application/pdf"></td>
    </tr> --}}
</table>
</body>
</html>
