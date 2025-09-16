<html lang="en">
<title>Approval Sheet Above 5bio</title>
<table>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td style="font-size: 20pt; font-weight: bold;">
            APPROVAL SHEET
        </td>
    </tr>
    <tr>
        <td></td>
        <td style="font-size: 20pt; font-weight: bold;">
            OPTIMA PROMO ID [{{ @strtoupper($data->periodDesc) }} {{ @$data->period }}] (Above IDR 5bio)
        </td>
    </tr>
</table>
<table>
    <tr>
        <td></td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Period</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Promo ID</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Entity</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Distributor</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Activity</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Activity Name</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Channel</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Account</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Promo Start</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Promo End</td>
        <td style="background-color: #ffff00; font-weight: bold; font-size: 12pt;">Investment</td>
    </tr>
    @foreach(@$data->budgetOver5Bio as $over5bio)
        <tr>
            <td></td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt; text-align: right;">{{ @$over5bio->period }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->promoId }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->entity }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->distributor }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->activity }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->activityName }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->channel }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @$over5bio->account }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt;">{{ @date('d-m-Y', strtotime($over5bio->promoStart)) }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt; text-align: right;">{{ @date('d-m-Y', strtotime($over5bio->promoEnd)) }}</td>
            <td style="border-bottom: 1px solid #dce6f1; font-size: 12pt; text-align: right;">{{ @number_format($over5bio->investment) }}</td>
        </tr>
    @endforeach

    @for($i=0; $i<6; $i++)
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
        <td style="text-align: center;">Approved by,</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>

    @if (@$data->emailApprovalSigned)

        @for($i=0; $i<3; $i++)
            <tr>
                <td></td>
            </tr>
        @endfor

        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[0]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[0]->approvedOn)) : '') }}</td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[1]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[1]->approvedOn)) : '') }}</td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[2]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[2]->approvedOn)) : '') }}</td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[3]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[3]->approvedOn)) : '') }}</td>
            <td></td>
            <td style="text-align: center;">{{ @($data->emailApprovalSigned[4]->approvedOn ? 'Approved on ' . date('d-m-Y', strtotime($data->emailApprovalSigned[4]->approvedOn)) : '') }}</td>
            <td></td>
            <td></td>
        </tr>

        @for($i=0; $i<3; $i++)
            <tr>
                <td></td>
            </tr>
        @endfor

        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[0]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[1]->username }}</td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[2]->username }}</td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[3]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[4]->username }}</td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[0]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[1]->jobtitle }}</td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[2]->jobtitle }}</td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[3]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApprovalSigned[4]->jobtitle }}</td>
            <td></td>
            <td></td>
        </tr>
    @else


        @for($i=0; $i<8; $i++)
            <tr>
                <td></td>
            </tr>
        @endfor

        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[0]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[1]->username }}</td>
            <td style="text-align: center;">{{ @$data->emailApproval[2]->username }}</td>
            <td style="text-align: center;">{{ @$data->emailApproval[3]->username }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[4]->username }}</td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[0]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[1]->jobtitle }}</td>
            <td style="text-align: center;">{{ @$data->emailApproval[2]->jobtitle }}</td>
            <td style="text-align: center;">{{ @$data->emailApproval[3]->jobtitle }}</td>
            <td></td>
            <td style="text-align: center;">{{ @$data->emailApproval[4]->jobtitle }}</td>
            <td></td>
            <td></td>
        </tr>
    @endif
</table>
</html>
