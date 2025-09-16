<table>
    <tbody>
    <tr>
        <td colspan="3" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Region</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Region ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->refId }}</td>
            <td>&#8203;{{ @$data[$i]->shortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->longDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>
