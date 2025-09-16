<table>
    <tbody>
    <tr>
        <td colspan="6" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Selling Point</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Selling Point ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Region</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Area Code</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Profit Center</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->sellingPointRefId }}</td>
            <td>&#8203;{{ @$data[$i]->regionLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->areaCode }}</td>
            <td>&#8203;{{ @$data[$i]->profitCenterDesc }}</td>
            <td>&#8203;{{ @$data[$i]->sellingPointShortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->sellingPointLongDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>



