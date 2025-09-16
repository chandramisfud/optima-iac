<table>
    <tbody>
    <tr>
        <td colspan="7" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Distributor</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Distributor ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Company Name</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
        <th style=" width: 60px; text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">NPWP</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Address</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Claim Manager</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->refId }}</td>
            <td>&#8203;{{ @$data[$i]->companyName }}</td>
            <td>&#8203;{{ @$data[$i]->shortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->longDesc }}</td>
            <td>&#8203;{{ @$data[$i]->npwp }}</td>
            <td>&#8203;{{ @$data[$i]->address }}</td>
            <td>&#8203;{{ @$data[$i]->claimManager  }}</td>
        </tr>
    @endfor
    </tbody>
</table>

