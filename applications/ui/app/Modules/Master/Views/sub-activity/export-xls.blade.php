<table>
    <tbody>
    <tr>
        <td colspan="7" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Sub Activity Promo</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Activity ID</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Activity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Type</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Short Desc</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Long Desc</th>
    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->subActivityRefId }}</td>
            <td>&#8203;{{ @$data[$i]->categoryLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subCategoryLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->activityLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subActivityTypeLongDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subActivityShortDesc }}</td>
            <td>&#8203;{{ @$data[$i]->subActivityLongDesc }}</td>
        </tr>
    @endfor
    </tbody>
</table>

