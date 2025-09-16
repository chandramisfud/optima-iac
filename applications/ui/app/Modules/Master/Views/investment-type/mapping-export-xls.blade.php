<table>
    <tbody>
    <tr>
        <td colspan="10" style="text-align: left; vertical-align: middle; font-weight:bold;">Mapping Investment Type</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;"></td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;"></td>
        <td style="text-align: left; vertical-align: middle;"></td>
        <td style="text-align: left; vertical-align: middle;"></td>
        <td style="text-align: left; vertical-align: middle;"></td>
        <td colspan="2" style="text-align: center; vertical-align: middle; background-color: yellow;">Before</td>
        <td colspan="2" style="text-align: center; vertical-align: middle; background-color: yellow;">After</td>

    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Activity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Activity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Investment Type Code</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Investment Type</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Investment Type Code</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Investment Type</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Last Update On</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Last Update By</th>

    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->category }}</td>
            <td>&#8203;{{ @$data[$i]->subCategory }}</td>
            <td>&#8203;{{ @$data[$i]->activity }}</td>
            <td>&#8203;{{ @$data[$i]->subactivity }}</td>
            <td>&#8203;{{ @$data[$i]->investmentTypeCode_bfr }}</td>
            <td>&#8203;{{ @$data[$i]->investmentType_bfr }}</td>
            <td>&#8203;{{ @$data[$i]->investmentTypeCode }}</td>
            <td>&#8203;{{ @$data[$i]->investmentType }}</td>
            <td>&#8203;{{ date('d-m-Y',strtotime(date(@$data[$i]->createOn))) }}</td>
            <td>&#8203;{{ @$data[$i]->createBy }}</td>
        </tr>
    @endfor
    </tbody>
</table>


