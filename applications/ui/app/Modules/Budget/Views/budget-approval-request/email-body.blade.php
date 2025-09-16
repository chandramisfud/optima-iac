<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>[Approval Needed] OPTIMA Promo ID {{ @$periodDesc }} {{ @$period }}</title>
    <style>
        @font-face {
            font-family: "Calibri";
            src: url({{ public_path('assets/fonts/Calibri.ttf') }});
        }
        body {
            font-family: 'Calibri', sans-serif;
            font-size: 12px;
        }
    </style>
</head>
<body>
@php
/* @var $data */
$emailApproval = $data->emailApproval;
$emailApprovalSigned = $data->emailApprovalSigned;
@endphp

<p style="font-weight: bold;">Dear {{ @(count($data->nextApproval) > 0 ? $data->nextApproval[0]->jobtitle :  $emailApproval[0]->jobtitle) }},</p>
<p></p>
<p>
    Kindly help to mass-approve attached OPTIMA Promo ID {{ @$periodDesc }} {{ @$period }}. <br/>
    Please note that this approval is mandatory so that Promo ID can be proceeded to the next step in OPTIMA.
</p>

<span style="font-weight: bold;">
    Approval Matrix:
</span>
@for($i=0; $i<count($emailApproval); $i++)
    <div style="padding-left: 35px;">
        {{ $i+1 }}. {{ $emailApproval[$i]->jobtitle }} - {{ $emailApproval[$i]->username }} @if($i>=3) (for above 5 billion IDR) @endif @if(count($emailApprovalSigned) > 0) @foreach($emailApprovalSigned as $approved) @if($approved->seq === $emailApproval[$i]->appseq) @if(@$approved->approvedOn) - <span style="font-weight: bold; color: #114b89">Approved</span> @endif @endif @endforeach @endif
    </div>
@endfor
<div style="font-weight: bold;margin-top: 20px;">
    Attachment:
</div>
<div style="padding-left: 35px;">
    - <a href="{{ @$linkAttachment['linkDownloadFileAbove'] }}" style="font-weight: bold; color: #5867dd">{{ @$linkAttachment['fileNameAbove'] }}</a>
</div>
@if (count($data->nextApproval) > 0)
    @if ($data->nextApproval[0]->seq < 4)
    <div style="padding-left: 35px;">
        - <a href="{{ @$linkAttachment['linkDownloadFileBelow'] }}" style="font-weight: bold; color: #5867dd">{{ @$linkAttachment['fileNameBelow'] }}</a>
    </div>
    @endif
@else
    <div style="padding-left: 35px;">
        - <a href="{{ @$linkAttachment['linkDownloadFileBelow'] }}" style="font-weight: bold; color: #5867dd">{{ @$linkAttachment['fileNameBelow'] }}</a>
    </div>
@endif
<div style="padding-left: 35px;">
    - <a href="{{ @$linkAttachment['linkDownloadFileExcel'] }}" style="font-weight: bold; color: #5867dd">{{ @$linkAttachment['fileNameExcel'] }}</a>
</div>
<div style="margin-top: 20px;">
    <a href="{{ @$linkApproval }}" target="_blank" style="padding: 8px 12px; border: 1px solid #5867dd;border-radius: 3px; width: 60px; text-align: center; font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;text-decoration: none;font-weight:bold;display: inline-block; background-color: #5867dd; margin-right: 5px;">
        Approve
    </a>
    <a href="{{ @$linkReject }}" target="_blank" style="padding: 8px 12px; border: 1px solid #5867dd;border-radius: 3px; width: 60px; text-align: center; font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;text-decoration: none;font-weight:bold;display: inline-block; background-color: #5867dd; margin-left: 5px;">
        Reject
    </a>
</div>
</body>
</html>
