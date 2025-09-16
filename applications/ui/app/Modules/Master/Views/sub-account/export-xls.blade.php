<table>
    <tbody>
    <tr>
        <td colspan="6" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Sub Account</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Account ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Channel</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Channel</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Account</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->subAccountRefId }}</td>
            <td>&#8203;{{ @$data[$i]->subChannelLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->channelLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->accountLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subAccountShortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subAccountLongDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>


