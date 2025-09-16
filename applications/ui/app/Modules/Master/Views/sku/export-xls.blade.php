<table>
    <tbody>
    <tr>
        <td colspan="5" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Sku</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sku ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Entity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Brand</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->productRefId }}</td>
            <td>&#8203;{{ @$data[$i]->entityLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->brandLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->productShortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->productLongDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>

