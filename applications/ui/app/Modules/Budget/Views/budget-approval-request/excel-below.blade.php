<html lang="en">
<title>Approval Sheet Below 5bio</title>
<table>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td style="font-size: 20pt; font-weight: bold;">
            APPROVAL SHEET
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td style="font-size: 20pt; font-weight: bold;">
            OPTIMA PROMO ID [{{ @strtoupper($data->periodDesc) }} {{ @$data->period }}] (Below IDR 5bio)
        </td>
    </tr>
@php
    /* @var $data */
    $budgetBellow5BioAll = @$data->budgetBellow5BioAll;
@endphp
<tr>
</tr>
<tr>
    <td></td>
    <td></td>
    <td></td>
    <td>(All)</td>
</tr>
<tr>
    <td></td>
    <td></td>
    <td></td>
    @foreach($budgetBellow5BioAll->header as $header)
        <td
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
        </td>
    @endforeach
        <td
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
        </td>
        <td
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
        </td>
</tr>
<tr>
    <td></td>
    <td></td>
    <td></td>
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
            Channel
        </td>
    @foreach($valueSubHeader as $r)
        <td
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
        </td>
    @endforeach
</tr>
@foreach($budgetBellow5BioAll->body as $body)
    <tr>
        <td></td>
        <td></td>
        <td></td>
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
        <td></td>
        <td></td>
        <td></td>
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
@for($i=0; $i<3; $i++)
    <tr>
        <td></td>
    </tr>
@endfor
@php
    /* @var $data */
    $budgetBellow5BioContractual = @$data->budgetBellow5BioContractual;
@endphp
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td>Trading Term</td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        @foreach($budgetBellow5BioContractual->header as $header)
            <td
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
            </td>
        @endforeach
        <td
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
        </td>
        <td
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
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        @php
            /* @var $budgetBellow5BioContractual */
            /* @var $subHeader */
            $subHeader = $budgetBellow5BioContractual->subHeader;
            $valueSubHeader = $budgetBellow5BioContractual->subHeader[0]->valueSubHeader;
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
            Channel
        </td>
        @foreach($valueSubHeader as $r)
            <td
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
            </td>
        @endforeach
    </tr>
    @foreach($budgetBellow5BioContractual->body as $body)
        <tr>
            <td></td>
            <td></td>
            <td></td>
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
    @foreach($budgetBellow5BioContractual->footer as $footer)
        <tr>
            <td></td>
            <td></td>
            <td></td>
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
    @for($i=0; $i<3; $i++)
        <tr>
            <td></td>
        </tr>
    @endfor
@php
    /* @var $data */
    $budgetBellow5BioNonContractual = @$data->budgetBellow5BioNonContractual;
@endphp
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td>Adhoc</td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        @foreach($budgetBellow5BioNonContractual->header as $header)
            <td
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
            </td>
        @endforeach
        <td
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
        </td>
        <td
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
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        @php
            /* @var $budgetBellow5BioNonContractual */
            /* @var $subHeader */
            $subHeader = $budgetBellow5BioNonContractual->subHeader;
            $valueSubHeader = $budgetBellow5BioNonContractual->subHeader[0]->valueSubHeader;
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
            Channel
        </td>
        @foreach($valueSubHeader as $r)
            <td
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
            </td>
        @endforeach
    </tr>
    @foreach($budgetBellow5BioNonContractual->body as $body)
        <tr>
            <td></td>
            <td></td>
            <td></td>
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
    @foreach($budgetBellow5BioNonContractual->footer as $footer)
        <tr>
            <td></td>
            <td></td>
            <td></td>
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

    @for($i=0; $i<7; $i++)
        <tr>
            <td></td>
        </tr>
    @endfor

    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td style="text-align: center;">Approved by,</td>
    </tr>

    @for($i=0; $i<9; $i++)
        <tr>
            <td></td>
        </tr>
    @endfor

    @if (@$data->emailApprovalSigned)
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[0]->approvedOn)) : '') }}</td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[1]->approvedOn)) : '') }}</td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[2]->approvedOn)) : '') }}</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[0]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[1]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[2]->username }}</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[0]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[1]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApprovalSigned[2]->jobtitle }}</td>
        </tr>
    @else
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[0]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[1]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[2]->username }}</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[0]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[1]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ $data->emailApproval[2]->jobtitle }}</td>
        </tr>
    @endif
</table>
</html>
