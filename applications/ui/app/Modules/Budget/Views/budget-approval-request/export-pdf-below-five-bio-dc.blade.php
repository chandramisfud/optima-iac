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
            font-family: 'Calibri', sans-serif !important;
            padding: 5px 5px;
        }

        .title {
            font-size: 14pt;
            font-weight: bold;
        }

        .table_body {
            margin-bottom: 30px;
            width: 100%;
            border-collapse: collapse;
            font-size: 11pt;
        }

        #signature {
            width: 100%;
            font-size: 11pt;
        }

        #signature tr td {
            text-align: center;
        }

        #footer {
            font-family: 'Calibri', sans-serif;
            position: fixed;
            bottom: 10px;
            width: 100%;
            font-size: 11pt;
        }
    </style>
</head>
<body>
<p class="title">
    APPROVAL SHEET
    <br/>
    OPTIMA PROMO ID {{ @strtoupper($data->period->periodDesc) }} {{ @$data->period->period }} (Below IDR 5bio)
</p>
@php
    /* @var $data */
    $budgetBellow5BioAll = @$data->budgetBellow5BioAll;
@endphp
<table class="table_body">
    <thead>
    <tr>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
            rowspan="1" colspan="1"
        >

        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                border-bottom: 1px solid #fff;
                "
            rowspan="1" colspan="2"
        >
            Sari Husada Generasi Mahardika
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                border-bottom: 1px solid #fff;
                "
            rowspan="1" colspan="2"
        >
            Nutricia Indonesia Sejahtera
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                border-bottom: 1px solid #fff;
                "
            rowspan="1" colspan="2"
        >
            Nutricia Medical Nutrition
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
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
                    font-weight: normal;
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
        <td
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Distributor
        </td>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Count of Promo ID
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Total Cost
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Count of Promo ID
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Total Cost
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Count of Promo ID
        </th>
        <th
            style="
                text-align: center;
                color: #ffffff;
                font-weight: normal;
                vertical-align: middle;
                border-top: unset;
                background-color: #366092;
                "
        >
            Total Cost
        </th>
    </tr>
    </thead>
    <tbody>
    @php
        $sumPromoIdTotCountSGM = 0;
        $sumTotInvestmentSGM = 0;
        $sumPromoIdTotCountNIS = 0;
        $sumTotInvestmentNIS = 0;
        $sumPromoIdTotCountNMN = 0;
        $sumTotInvestmentNMN = 0;
        $sumPromoIdTotCount = 0;
        $sumTotInvestment = 0;
    @endphp
    @foreach($budgetBellow5BioAll as $body)
            <tr>
                <td
                    style="
                        text-align: left;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ $body->channel }}
                </td>
                <td
                    style="
                        text-align: center;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->promoIdTotCountSGM === 0 ? '' : number_format($body->promoIdTotCountSGM))  }}
                </td>
                <td
                    style="
                        text-align: right;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->totInvestmentSGM === 0 ? '' : number_format($body->totInvestmentSGM))  }}
                </td>
                <td
                    style="
                        text-align: center;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->promoIdTotCountNIS === 0 ? '' : number_format($body->promoIdTotCountNIS))  }}
                </td>
                <td
                    style="
                        text-align: right;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->totInvestmentNIS === 0 ? '' : number_format($body->totInvestmentNIS))  }}
                </td>
                <td
                    style="
                        text-align: center;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->promoIdTotCountNMN === 0 ? '' : number_format($body->promoIdTotCountNMN))  }}
                </td>
                <td
                    style="
                        text-align: right;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->totInvestmentNMN === 0 ? '' : number_format($body->totInvestmentNMN))  }}
                </td>
                <td
                    style="
                        text-align: center;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->promoIdTotCount === 0 ? '' : number_format($body->promoIdTotCount))  }}
                </td>
                <td
                    style="
                        text-align: right;
                        color: #000;
                        vertical-align: middle;
                        border-top: 1px solid #dce6f1;
                        background-color: #fff;
                        "
                >
                    {{ ($body->totInvestment === 0 ? '' : number_format($body->totInvestment))  }}
                </td>
            </tr>
            @php
                /* @var $body */
                /* @var $sumPromoIdTotCountSGM */
                /* @var $sumTotInvestmentSGM */
                /* @var $sumPromoIdTotCountNIS */
                /* @var $sumTotInvestmentNIS */
                /* @var $sumPromoIdTotCountNMN */
                /* @var $sumTotInvestmentNMN */
                /* @var $sumPromoIdTotCount */
                /* @var $sumTotInvestment */
                $sumPromoIdTotCountSGM = $sumPromoIdTotCountSGM + $body->promoIdTotCountSGM;
                $sumTotInvestmentSGM = $sumTotInvestmentSGM + $body->totInvestmentSGM;
                $sumPromoIdTotCountNIS = $sumPromoIdTotCountNIS + $body->promoIdTotCountNIS;
                $sumTotInvestmentNIS = $sumTotInvestmentNIS + $body->totInvestmentNIS;
                $sumPromoIdTotCountNMN = $sumPromoIdTotCountNMN + $body->promoIdTotCountNMN;
                $sumTotInvestmentNMN = $sumTotInvestmentNMN + $body->totInvestmentNMN;
                $sumPromoIdTotCount = $sumPromoIdTotCount + $body->promoIdTotCount;
                $sumTotInvestment = $sumTotInvestment + $body->totInvestment;
        @endphp
    @endforeach
        <tr>
            <td
                style="
                    font-weight: bold;
                    text-align: left;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                Grand Total
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: center;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumPromoIdTotCountSGM) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: right;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumTotInvestmentSGM) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: center;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumPromoIdTotCountNIS) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: right;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumTotInvestmentNIS) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: center;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumPromoIdTotCountNMN) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: right;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumTotInvestmentNMN) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: center;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumPromoIdTotCount) }}
            </td>
            <td
                style="
                    font-weight: bold;
                    text-align: right;
                    color: #000000;
                    vertical-align: middle;
                    border-top: 1px double #366092;
                    background-color: #fff;
                    "
            >
                {{ number_format($sumTotInvestment) }}
            </td>
        </tr>
    </tbody>
</table>

<table id="signature">
    <tr>
        <td colspan="5" style="text-align: center">Approved by,</td>
    </tr>
    @if (@$data->emailApproval)
        <tr>
            @for($i=0; $i<3; $i++)
                <td style="text-align: center;  width: 20%; padding-top: 15px; padding-bottom: 15px; font-weight: bold;">{{ @($data->emailApproval[$i]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApproval[$i]->approvedOn)) : '') }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<3; $i++)
                <td>{{ @$data->emailApproval[$i]->username }}</td>
            @endfor
        </tr>
        <tr>
            @for($i=0; $i<3; $i++)
                <td>{{ @$data->emailApproval[$i]->jobtitle }}</td>
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
