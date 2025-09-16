<table>
    <tbody>
    <tr>
        <td colspan="2" style="text-align: left; vertical-align: middle; font-weight:bold;">Master profit Center</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Profit Center</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Description</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->profitCenter }}</td>
            <td>&#8203;{{ @$data[$i]->profitCenterDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>

