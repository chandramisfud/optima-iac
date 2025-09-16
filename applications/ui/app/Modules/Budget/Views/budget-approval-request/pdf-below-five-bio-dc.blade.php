<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>
        Approval Sheet Below 5 Bio
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
            font-size: 14pt;
            font-weight: bold;
        }

        .table_body {
            margin-bottom: 30px;
            width: 100%;
            border-collapse: collapse;
            font-size: 12pt;
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
<p class="title">
    APPROVAL SHEET
    <br/>
    OPTIMA PROMO ID {{ @strtoupper($data->periodDesc) }} {{ @$data->period }} (Below IDR 5bio)
</p>
@php
    /* @var $data */
    $budgetBellow5BioAll = @$data->budgetBellow5BioAll;
@endphp
<table class="table_body">
    <thead>
    <tr>
        @foreach($budgetBellow5BioAll->header as $header)
        <th
            style="
                text-align: {{ $header->textAlign ?? '' }};
                color: {{ $header->textColor ?? '' }};
                font-weight: {{ $header->fontWeigth ?? '' }};
                vertical-align: {{ $header->verticalAlign ?? '' }};
                border-top: {{ $header->borderTop ?? '' }};
                background-color: {{ $header->backgroundColor ?? '' }};
                "
            rowspan="1" colspan="{{ ($header->headerName === "") ? '1' : '2'  }}"
        >
            {{ $header->headerName }}
        </th>
        @endforeach
            <th
                style="
                text-align: center;
                color: #ffffff;
                font-weight: bold;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
                rowspan="2"
            >
                Total Count of Promo ID
            </th>
            <th
                style="
                    text-align: center;
                    color: #ffffff;
                    font-weight: bold;
                    vertical-align: middle;
                    border-top: unset;
                    background-color: #366092;
                    "
                rowspan="2"
            >
                Sum of Total Cost
            </th>
    </tr>
    <tr>
        @php
            /* @var $budgetBellow5BioAll */
            /* @var $subHeader */
            $subHeader = $budgetBellow5BioAll->subHeader;
            $valueSubHeader = $budgetBellow5BioAll->subHeader[0]->valueSubHeader;
        @endphp
        <td
            style="
                text-align: center;
                color: #ffffff;
                font-weight: bold;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Distributor
        </td>
        @foreach($valueSubHeader as $r)
            <th
                style="
                    text-align: {{ $r->textAlign ?? '' }};
                    color: {{ $r->textColor ?? '' }};
                    font-weight: {{ $r->fontWeigth ?? '' }};
                    vertical-align: {{ $r->verticalAlign ?? '' }};
                    border-top: {{ $r->borderTop ?? '' }};
                    background-color: {{ $r->backgroundColor ?? '' }};
                    "
            >
                {{ $r->headerName }}
            </th>
        @endforeach
    </tr>
    </thead>
    <tbody>
    @foreach($budgetBellow5BioAll->body as $body)
        <tr>
            <td
                style="
                    text-align: left;
                    color: {{ $body->textColor ?? '' }};
                    vertical-align: {{ $body->verticalAlign ?? '' }};
                    border-top: 1px solid #dce6f1;
                    background-color: {{ $body->backgroundColor ?? '' }};
                    "
            >
                {{ $body->text }}
            </td>
            @if ($body->value)
                @php
                    /* @var $body */
                    $value = $body->value;
                @endphp
                @foreach($value as $r)
                    <td
                        style="
                            text-align: right;
                            color: {{ $r->textColor ?? '' }};
                            vertical-align: {{ $r->verticalAlign ?? '' }};
                            border-top: 1px solid #dce6f1;
                            background-color: {{ $r->backgroundColor ?? '' }};
                            "
                    >
                        {{ ($r->value === 0 ? '' : number_format($r->value)) }}
                    </td>
                @endforeach
            @endif
        </tr>
    @endforeach
    @foreach($budgetBellow5BioAll->footer as $footer)
        <tr>
            <td
                style="
                    text-align: left;
                    color: {{ $footer->textColor ?? '' }};
                    vertical-align: {{ $footer->verticalAlign ?? '' }};
                    border-top: 1px double #000000;
                    background-color: {{ $footer->backgroundColor ?? '' }};
                    "
            >
                {{ $footer->text }}
            </td>
            @if ($footer->value)
                @php
                    /* @var $footer */
                    $value = $footer->value;
                @endphp
                @foreach($value as $r)
                    <td
                        style="
                            text-align: right;
                            color: {{ $r->textColor ?? '' }};
                            vertical-align: {{ $r->verticalAlign ?? '' }};
                            border-top: 1px double #000000;
                            background-color: {{ $r->backgroundColor ?? '' }};
                            "
                    >
                        {{ ($r->value === 0 ? '' : number_format($r->value)) }}
                    </td>
                @endforeach
            @endif
        </tr>
    @endforeach
    </tbody>
</table>

<table id="signature">
    <tr>
        <td colspan="5" style="text-align: center">Approved by,</td>
    </tr>
    @if (@$data->emailApprovalSigned)
        <tr>
            @for($i=0; $i<3; $i++)
                <td style="text-align: center;  width: 20%; padding-top: 25px; padding-bottom: 25px; font-weight: bold;">{{ @($data->emailApprovalSigned[$i]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[$i]->approvedOn)) : '') }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<3; $i++)
                <td>{{ @$data->emailApprovalSigned[$i]->username }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<3; $i++)
                <td>{{ @$data->emailApprovalSigned[$i]->jobtitle }}</td>
            @endfor
        </tr>
    @else
        <tr>
            @for($i=0; $i<3; $i++)
                <td style="width: 20%; padding-top: 50px; ">{{ @$data->emailApproval[$i]->username }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<3; $i++)
                <td>{{ @$data->emailApproval[$i]->jobtitle }}</td>
            @endfor
        </tr>
    @endif
</table>

<div id="footer">
    Note: This document represents automatic approval and does not require handwritten signature
</div>
</body>
</html>
