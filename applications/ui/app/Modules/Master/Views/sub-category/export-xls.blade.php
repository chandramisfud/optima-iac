<table>
    <tbody>
    <tr>
        <td colspan="4" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Sub Category Promo</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Category ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->subCategoryRefId }}</td>
            <td>&#8203;{{ @$data[$i]->categoryLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subCategoryShortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subCategoryLongDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>

