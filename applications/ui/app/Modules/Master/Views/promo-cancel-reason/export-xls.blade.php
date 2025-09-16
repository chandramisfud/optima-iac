<table>
    <tbody>
    <tr>
        <td colspan="1" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Promo Cancellation Reason</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->longDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>
